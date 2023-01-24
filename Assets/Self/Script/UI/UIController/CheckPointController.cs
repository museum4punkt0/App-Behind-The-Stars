using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class CheckPointController : MonoBehaviour {
    [System.Serializable] public class CheckPointList {
        public List<Checkpoint> checkpoints = new List<Checkpoint>();
    }

    [Serializable] public class Checkpoint {
        public string pathID;
        public int checkPointID = 0;
        public bool checkPointReached = false;
        public string checkPointText;
        public string checkPointText_EN;
        public string targetCheckPoint;
    }

#region public Variables

    //Scripts
    public HelpController hC;
    public ToolTipController tTC;
    public ProcedureController pC;

    //Object
    public Animator checkPointCanvasAnimator;
    public Animator checkPointQuestionAnimator;

    [SerializeField] public CheckPointList myCheckpointListList = new CheckPointList();
    public CheckPointList myLoadedCheckpointListList = new CheckPointList();

    public GameObject checkPointPrefab;
    public GameObject checkPointListParent;
    public GameObject reallyJumpToCheckpointQuestionPanel;

    public Image fillCircleProcess;
    public Image fillCircleJumpBackWindow;
    public Image processHighlight;

    public RectTransform checkPointCanvasRect;

    public Text checkPointText;

    public Transform blockButton;

#endregion

#region private Variables

    private bool canvasActive = false;
    private bool blockPosX = false;
    private bool highlightProcess = false;
    private bool turnProcessHighlightOn = true;
    private bool turnProcessHighlightOff = false;
    private bool OpenJumpToCheckPointQuestioWasShown = false;

    private GameObject activeCheckPoint;

    private int highestStepId = 0;
    private int lastReachedCheckpoint = 0;

#endregion

    void Start() {
        CheckPointList myCheckpointListList = new CheckPointList();
    }

    private void Update() {
        if (blockPosX) {
            checkPointCanvasRect.position = new Vector3(0, 0, 0);
        }

        if (highlightProcess) {
            if (turnProcessHighlightOn) {
                Color processHighlightColor = processHighlight.color;
                if (processHighlightColor.a < 1) {
                    processHighlightColor.a += 0.1f;
                    processHighlight.color = processHighlightColor;
                } else {
                    turnProcessHighlightOff = true;
                    turnProcessHighlightOn = false;
                }
            }

            if (turnProcessHighlightOff) {
                Color processHighlightColor = processHighlight.color;
                if (processHighlightColor.a > 0) {
                    processHighlightColor.a -= 0.05f;
                    processHighlight.color = processHighlightColor;
                } else {
                    turnProcessHighlightOn = false;
                    turnProcessHighlightOff = false;
                    highlightProcess = false;
                }
            }
        }
    }

    //Die bereits erreichten Checkpoints aus der JSON-Datei werden laden und in eine Hilfsliste speichern
    public void LoadCheckPointFromFile() {
        string loadData = "";
        try {
            string filePath = Path.Combine(Application.persistentDataPath, "CheckPointsState.json");
            loadData = File.ReadAllText(filePath);
            myLoadedCheckpointListList = JsonUtility.FromJson<CheckPointList>(loadData);
            pC.InitCheckpoints();
        } catch (FileNotFoundException) {
            pC.InitCheckpoints();
        }
    }
    
    //Zum Anwendungsstart werden die Informationen zu den Checkpoints aus der CheckPointsState.json + die Informationen
    //zu Checkpoints aus der Content.json in die finale Checkpointliste gespeichert
    public void SetCheckpoint(string _pathID, string _checkPointID, bool _checkPointReached, string _checkPointText,
        string _checkPointTextEN, string _targetCheckPoint) {
        Checkpoint cp = new Checkpoint();
        cp.pathID = _pathID;
        cp.checkPointID = int.Parse(_checkPointID);
        cp.checkPointReached = _checkPointReached;
        foreach (Checkpoint cpLoaded in myLoadedCheckpointListList.checkpoints) {
            if (cpLoaded.pathID == _pathID && cpLoaded.checkPointID == int.Parse(_checkPointID)) {
                cp.checkPointReached = cpLoaded.checkPointReached;
            }
        }

        cp.checkPointText = _checkPointText;
        cp.checkPointText_EN = _checkPointTextEN;
        cp.targetCheckPoint = _targetCheckPoint;
        myCheckpointListList.checkpoints.Add(cp);
    }

    public void SetCheckPointReached(string _pathId, int _checkPointID) {
        foreach (Checkpoint cp in myCheckpointListList.checkpoints) {
            if (cp.pathID == _pathId && cp.checkPointID == _checkPointID) {
                cp.checkPointReached = true;
            }
        }

        SetCheckPointColors(_pathId, _checkPointID);
        lastReachedCheckpoint = _checkPointID - 1;
    }

    //Zum Pfadstart als erstes das Checkpointmenu leeren
    public void DestroyCheckpoints() {
        foreach (Transform child in checkPointListParent.transform) {
            Destroy(child.gameObject);
        }
    }

    //Zu Pfadstart die Checkpunkte initialisieren
    public void InitCheckPointList(string _pathID) {
        foreach (Transform child in checkPointListParent.transform) {
            Destroy(child.gameObject);
        }

        foreach (Checkpoint cp in myCheckpointListList.checkpoints) {
            if (cp.pathID == _pathID) {
                GameObject newCheckPoint = Instantiate<GameObject>(checkPointPrefab);
                newCheckPoint.transform.parent = checkPointListParent.transform;
                newCheckPoint.transform.localScale = new Vector3(1, 1, 1);

                CheckPointProperties cPP =
                    (CheckPointProperties) newCheckPoint.GetComponent(typeof(CheckPointProperties));
                cPP.SetTargetCheckPoint(cp.pathID, cp.checkPointID,
                    cp.checkPointText,
                    cp.checkPointText_EN,
                    cp.targetCheckPoint,
                    cp.checkPointReached,
                    pC.GetLanguage());
            }
        }
    }
 
    //Wenn ein neuer Checkpoint erreicht wurde auch dei JSON aktualisieren
    public void SaveCheckPointsToFile() {
        string filePath = Path.Combine(Application.persistentDataPath, "CheckPointsState.json");
        string jsonData = JsonUtility.ToJson(myCheckpointListList);
        File.WriteAllText(filePath, jsonData);
    }
    
    //Dem Zustand des Checkpoints entsprechend die Darstellung anpassen
    public void SetCheckPointColors(string _pathId, int id) {
        if (checkPointListParent.transform.childCount > 1) {
            float process = ((float) id - 1) / (float) checkPointListParent.transform.childCount;
            if (process == 1) {
                process -= 0.1f;
            }

            if (process == 0) {
                process = 0.05f;
            }

            fillCircleProcess.fillAmount = process;
            fillCircleJumpBackWindow.fillAmount = process;
            highlightProcess = true;
            turnProcessHighlightOn = true;
            turnProcessHighlightOff = false;
        }

        if (id > highestStepId) {
            highestStepId = id;
        }

        foreach (Transform child in checkPointListParent.transform) {
            CheckPointProperties cPP = (CheckPointProperties) child.GetComponent(typeof(CheckPointProperties));
            if (cPP.GetCheckPointID() == id) {
                cPP.SetStepActive();
            } else {
                foreach (Checkpoint cp in myCheckpointListList.checkpoints) {
                    if (cp.pathID == _pathId && cp.checkPointID == cPP.GetCheckPointID()) {
                        if (cp.checkPointReached) {
                            cPP.SetStepReached();
                        } else {
                            cPP.SetStepDeactive();
                        }
                    } else if (cPP.GetCheckPointID() == 1) {
                        cPP.SetStepReached();
                    }
                }
            }
        }

        SaveCheckPointsToFile();
    }
    
    //Checkpointmenu einblenden
    public void ShowCheckPoints() {
        activeCheckPoint = null;
        reallyJumpToCheckpointQuestionPanel.SetActive(false);
        blockButton.localScale = new Vector3(0, 0, 0);
        if (!canvasActive) {
            checkPointCanvasAnimator.Play("ShowCheckPointPanel", 0, 0);
            checkPointCanvasAnimator.SetInteger("CheckPointStatus", 1);
            blockPosX = true;
            canvasActive = true;
        }

        hC.HideHelpPanel();
    }

    //Checkpointmenu ausblenden
    public void HideCheckPoints() {
        blockButton.localScale = new Vector3(1, 1, 1);
        checkPointCanvasAnimator.Play("HideCheckPointPanel", 0, 0);
        checkPointCanvasAnimator.SetInteger("CheckPointStatus", 2);
        canvasActive = false;
        blockPosX = true;
        tTC.HideToolTipCheckpointPart2();
    }
    
    public void ResetScript() {
        blockButton.localScale = new Vector3(1, 1, 1);
        checkPointCanvasAnimator.Rebind();
        checkPointCanvasAnimator.Update(0f);
        canvasActive = false;
        blockPosX = true;
        lastReachedCheckpoint = 0;
    }


    //Wenn ein Checkpoint im Checkpoint Menu ausgewählt wurde, öffnet sich ein Fenster mit der Frage ob man wirklich zurückspringen möchte
    public void OpenReallyJumpToCheckpointQuestionWindow(GameObject clickedCheckpoint, string checkPointName) {
        tTC.HideToolTipCheckpointPart2();
        activeCheckPoint = clickedCheckpoint;
        reallyJumpToCheckpointQuestionPanel.SetActive(true);
        if (checkPointName == "START") {
            checkPointText.text = "Zurückspringen zum Start?";
            if (pC.GetLanguage() == 1) {
                checkPointText.text = "Back to Start?";
            }
        } else {
            checkPointText.text = "Zu Checkpoint: " + checkPointName + " springen?";
            if (pC.GetLanguage() == 1) {
                checkPointText.text = "Back to: " + checkPointName + " ?";
            }
        }
    }

    //Das Fenster im Checkpoint Menu, ob man wirklich zurückspringen möchte schließen
    public void CloseReallyJumpToCheckpointQuestionWindow() {
        reallyJumpToCheckpointQuestionPanel.SetActive(false);
        activeCheckPoint = null;
    }
    
    //Wenn man den zurück-Button angetippt hat, öffnet sich das Fenster ob man wirklich zurückspringen möchte
    public void OpenJumpToCheckPointQuestionInMainScreen() {
        checkPointQuestionAnimator.Play("ShowJumpBackQuestion", 0, 0);
        checkPointQuestionAnimator.SetInteger("AnimationState", 1);
        OpenJumpToCheckPointQuestioWasShown = true;
    }

    //Fenster ob man wirklich zurückspringen möchte schließen
    public void CloseJumpToCheckPointQuestionInMainScreen() {
        checkPointQuestionAnimator.Play("HideJumpBackQuestion", 0, 0);
        checkPointQuestionAnimator.SetInteger("AnimationState", 0);
    }

    public void AcceptJumpToCheckpoint() {
        CheckPointProperties cPP = (CheckPointProperties) activeCheckPoint.GetComponent(typeof(CheckPointProperties));
        cPP.JumpToCheckpoint();
        reallyJumpToCheckpointQuestionPanel.SetActive(false);
        activeCheckPoint = null;
    }

    //Bestätigung im Fenster ob man wirklich zurückspringen möchte (Zurück-Button)
    public void GoToLastCheckPointWithBackButton() {
        if (!pC.GetStepId().Contains("H0")) {
            GameObject childCheckPoint =
                checkPointListParent.transform.GetChild(lastReachedCheckpoint).transform.gameObject;
            CheckPointProperties cPP =
                (CheckPointProperties) childCheckPoint.GetComponent(typeof(CheckPointProperties));
            cPP.JumpToCheckpointWithBackButton();
            reallyJumpToCheckpointQuestionPanel.SetActive(false);
            activeCheckPoint = null;
            lastReachedCheckpoint -= 1;
            if (lastReachedCheckpoint < 0) {
                lastReachedCheckpoint = 0;
            }
        } else {
            pC.JumpToCheckPoint("Z02.15");
        }
    }
    
    public void CanvasTargetPosReached() {
        blockPosX = false;
    }

    public void HideCheckPointsWrapper() {
        StartCoroutine(WaitAndHideCheckPoints());
    }

    private IEnumerator WaitAndHideCheckPoints() {
        yield return new WaitForSeconds(1f);
        HideCheckPoints();
    }
}