using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheckPointProperties : MonoBehaviour {
#region public Variables

    public Button checkPointButton;
    public Image statusCircle;
    public Image statusCircleLine;

    public TextMeshProUGUI checkPointText;

#endregion

#region private Variables

    private int checkPointID = 0;

    private string pathID = "";
    private string targetCheckPoint = "";

#endregion

    //initialisierung des Checkpoints
    public void SetTargetCheckPoint(string _pathID, int _checkPointID, string _checkPointText, string _checkPointTextEN,
        string _targetCheckPoint, bool _checkPointReached, int language) {
        pathID = _pathID;
        checkPointID = _checkPointID;
        checkPointText.text = _checkPointText;
        if (language == 1) {
            checkPointText.text = _checkPointTextEN;
        }

        targetCheckPoint = _targetCheckPoint;

        if (checkPointID == 1) {
            SetStepActive();
        } else if (_checkPointReached) {
            SetStepReached();
        } else {
            SetStepDeactive();
        }
    }

    public int GetCheckPointID() {
        return checkPointID;
    }

    //Wenn Checkpoint ausgewählt wurde im Menu als Active anzeigen
    public void SetStepActive() {
        statusCircleLine.color = new Color(158, 157, 236, 255) / 255;
        statusCircle.color = new Color(158, 157, 236, 255) / 255;
        checkPointText.color = new Color(158, 157, 236, 255) / 255;
        checkPointText.fontStyle = FontStyles.Bold;
        checkPointButton.interactable = true;
    }

    //Wenn der Checkpoint erreicht wurde im Menu als verfügbar anzeigen
    public void SetStepReached() {
        statusCircleLine.color = new Color(158, 157, 236, 255) / 255;
        statusCircle.color = new Color(158, 157, 236, 0) / 255;
        checkPointText.color = new Color(255, 255, 255, 255) / 255;
        checkPointText.fontStyle = FontStyles.Normal;
        checkPointButton.interactable = true;
    }

    //Wenn Checkpoint noch nicht erreicht wurde, im Menu als nicht verfügbar anzeigen
    public void SetStepDeactive() {
        statusCircleLine.color = new Color(61, 61, 67, 255) / 255;
        statusCircle.color = new Color(158, 157, 236, 0) / 255;
        checkPointText.color = new Color(61, 61, 67, 255) / 255;
        checkPointText.fontStyle = FontStyles.Normal;
        checkPointButton.interactable = false;
    }

    public void CallCheckpointController() {
        CheckPointController cPC =
            (CheckPointController) GameObject.Find("CheckPointParent").GetComponent(typeof(CheckPointController));
        cPC.OpenReallyJumpToCheckpointQuestionWindow(this.transform.gameObject, checkPointText.text);
    }

    public void JumpToCheckpoint() {
        GameObject.Find("Block").transform.localScale = new Vector3(1, 1, 1);
        ProcedureController pC =
            (ProcedureController) GameObject.Find("GameController").GetComponent(typeof(ProcedureController));
        pC.JumpToCheckPoint(targetCheckPoint);

        CheckPointController cPC =
            (CheckPointController) GameObject.Find("CheckPointParent").GetComponent(typeof(CheckPointController));
        cPC.HideCheckPointsWrapper();
        cPC.SetCheckPointColors(pathID, checkPointID);
    }

    public void JumpToCheckpointWithBackButton() {
        GameObject.Find("Block").transform.localScale = new Vector3(1, 1, 1);
        ProcedureController pC =
            (ProcedureController) GameObject.Find("GameController").GetComponent(typeof(ProcedureController));
        pC.JumpToCheckPoint(targetCheckPoint);

        CheckPointController cPC =
            (CheckPointController) GameObject.Find("CheckPointParent").GetComponent(typeof(CheckPointController));
        cPC.SetCheckPointColors(pathID, checkPointID);
    }
}