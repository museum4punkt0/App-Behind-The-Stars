using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class N07Helper : MonoBehaviour {
#region public Variables

    public SkyTimeController skyTimeController;
    public CheckPointController cPC;
    public ClockController cc;
    public ContentScrollSnapHorizontal cSSH;
    public NokturnalController nokturnalController;
    public ProcedureController pC;
    public SpielstandLoader ssL;
    public TimeInputController tIC;
    public Translator translator;

    public Animator handOnZaehne;

    public Camera nokturnalCam;
    public Camera skyCam;

    public GameObject contentScrollClockObject;
    public GameObject nokturnalCamera;
    public GameObject timeParent;

    public Material nokturnalGoldZeiger;

    public SpriteRenderer einstellringZahlen;
    public SpriteRenderer handSprite;
    public SpriteRenderer zaehne;
    public SpriteRenderer zeigerHighlight;

    public Transform einstellringPivot;
    public Transform nokturnalParent;
    public Transform zeigerPivot;
    public Transform zeigerPivotRotationObject;

#endregion

#region private Variables

    private bool moveZeigerTo9 = false;
    private bool turnHandOff = false;
    private bool turnZaehneOff = false;
    private bool showNokturnalZahlen = false;
    private bool turnNokturnalZahlenOff = false;
    private bool turnZeigerHighlightOn = false;
    private bool makeZeigerTransparent = false;
    private bool flashHighlightZeiger = false;
    private bool highlightZeigerOn = false;
    private bool highlightZeigerOff = true;

    private int wrongInput = 0;

#endregion

    private void Start() {
        ProcedureController.changeEvent += DoActionWhileStepUpdate;
        StartCoroutine(InitNokturnalPos());
    }

    private IEnumerator InitNokturnalPos() {
        yield return new WaitForSeconds(5f);
    }

    public void DoActionWhileStepUpdate(string stepId) {
        switch (stepId) {
            case "N07.00a":
            case "N07.00":
                ResetScript();
                nokturnalParent.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                contentScrollClockObject.SetActive(true);
                cSSH.UpdateScrollClockSlider();
                pC.JumpToNextStep();
                break;

        #region Checkpoint

            case "N07.18C":
                ResetScript();
                nokturnalParent.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                pC.JumpToNextStep();
                break;

        #endregion

            case "N07.18":
                //Starte die Animation (Hand Icpn fährt Zähne ab)
                handOnZaehne.enabled = true;
                handOnZaehne.SetInteger("allowPlayAnimation", 1);

                //Checkpoint Reached
                cPC.SetCheckPointColors("N07", 2);
                cPC.SetCheckPointReached("N07", 02);

                pC.JumpToNextStep();
                break;

            case "N07.23":
                handOnZaehne.StopPlayback();
                handOnZaehne.enabled = false;

                //Nokturnalzeiger zu 9 Uhr rotieren und transparent machen  
                moveZeigerTo9 = true;
                makeZeigerTransparent = true;
                //Nokturnalzahlen und weitere Overlays ausblenden
                turnNokturnalZahlenOff = true;
                turnHandOff = true;
                turnZaehneOff = true;
                break;

            case "N07.26":
            case "N07.26a":
            case "N07.25":
                //Neuen Spielstand setzen,
                ssL.FinishedPathPoint("N07");

                //Variablen zurücksetzen
                flashHighlightZeiger = false;
                highlightZeigerOn = false;
                highlightZeigerOff = false;
                Color zeigerHighlightColor = zeigerHighlight.color;
                zeigerHighlightColor.a = 0;
                zeigerHighlight.color = zeigerHighlightColor;
                break;
        }
    }

    private void Update() {
        //Rotiere den Zeiger auf die Zahl 9
        if (moveZeigerTo9) {
            if (zeigerPivot.localEulerAngles.y < 214) {
                zeigerPivot.localEulerAngles += new Vector3(0, 9.125f, 0);
            } else {
                zeigerPivot.localEulerAngles = new Vector3(90, 0, 146);
                turnZeigerHighlightOn = true;
                moveZeigerTo9 = false;
            }
        }

    #region graphical Actions

        if (showNokturnalZahlen) {
            Color color = einstellringZahlen.color;
            if (color.a < 1) {
                color.a += 0.075f;
                einstellringZahlen.color = color;
            } else {
                color.a = 1;
                einstellringZahlen.color = color;
                showNokturnalZahlen = false;
            }
        }

        if (turnNokturnalZahlenOff) {
            Color color = einstellringZahlen.color;
            if (color.a > 0) {
                color.a -= 0.075f;
                einstellringZahlen.color = color;
            } else {
                color.a = 0;
                einstellringZahlen.color = color;
                turnNokturnalZahlenOff = false;
            }
        }

        if (turnHandOff) {
            Color color = handSprite.color;
            if (color.a > 0) {
                color.a -= 0.1f;
                handSprite.color = color;
            } else {
                color.a = 0f;
                handSprite.color = color;
                turnHandOff = false;
            }
        }

        if (turnZaehneOff) {
            Color color = zaehne.color;
            if (color.a > 0) {
                color.a -= 0.1f;
                zaehne.color = color;
            } else {
                color.a = 0f;
                zaehne.color = color;
                turnZaehneOff = false;
            }
        }

        if (turnZeigerHighlightOn) {
            Color zeigerHighlightColor = zeigerHighlight.color;
            if (zeigerHighlightColor.a < 1) {
                zeigerHighlightColor.a += 0.1f;
                zeigerHighlight.color = zeigerHighlightColor;
            } else {
                ShowInputClock();
                flashHighlightZeiger = true;
                turnZeigerHighlightOn = false;
            }
        }

        if (flashHighlightZeiger) {
            if (highlightZeigerOn) {
                Color zeigerHighlightColor = zeigerHighlight.color;
                if (zeigerHighlightColor.a < 1) {
                    zeigerHighlightColor.a += 0.075f;
                    zeigerHighlight.color = zeigerHighlightColor;
                } else {
                    highlightZeigerOff = true;
                    highlightZeigerOn = false;
                }
            }


            if (highlightZeigerOff) {
                Color zeigerHighlightColor = zeigerHighlight.color;
                if (zeigerHighlightColor.a > 0.4f) {
                    zeigerHighlightColor.a -= 0.03f;
                    zeigerHighlight.color = zeigerHighlightColor;
                } else {
                    highlightZeigerOn = true;
                    highlightZeigerOff = false;
                }
            }
        }

        if (makeZeigerTransparent) {
            Color nokturnalGoldZeigerColor = nokturnalGoldZeiger.color;
            if (nokturnalGoldZeigerColor.a > 0.4f) {
                nokturnalGoldZeigerColor.a -= 0.075f;
                nokturnalGoldZeiger.color = nokturnalGoldZeigerColor;
            } else {
                nokturnalGoldZeigerColor.a = 0.45f;
                nokturnalGoldZeiger.color = nokturnalGoldZeigerColor;
                makeZeigerTransparent = false;
            }
        }

    #endregion
    }

    private void ResetScript() {
        
    #region Reset Camera

        float resolutionFactor = DeviceInfo.GetResolutionFactor();
        float temp = ((resolutionFactor - 0.45f) * 2.0f / 0.3f) + 5;
        nokturnalCam.orthographicSize = temp;

        skyCam.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = false;

    #endregion

    #region Reset private Variables

        moveZeigerTo9 = false;
        turnHandOff = false;
        turnZaehneOff = false;
        showNokturnalZahlen = false;
        turnNokturnalZahlenOff = false;
        wrongInput = 0;
        flashHighlightZeiger = false;
        highlightZeigerOn = false;
        highlightZeigerOff = true;

    #endregion

    #region Reset Date and Time
        cc.ChangeDateTextColor(1);
        cc.ChangeTimeTextColor(1);
        cc.SetCurrentDate();
        tIC.HideScrollPanel();

        cc.SetTimeModeN07();
        skyTimeController.SetTimeline(0);

    #endregion

    #region Reset Materials

        Color color = zaehne.color;
        color.a = 0.0f;
        zaehne.color = color;

        Color zeigerHighlightColor = zeigerHighlight.color;
        zeigerHighlightColor.a = 0;
        zeigerHighlight.color = zeigerHighlightColor;

        Color einstellringZahlenColor = einstellringZahlen.color;
        einstellringZahlenColor.a = 1;
        einstellringZahlen.color = einstellringZahlenColor;

    #endregion

    #region Reset Nokturnal

        nokturnalCamera.SetActive(true);
        nokturnalController.NokturnalShowFront();
        nokturnalParent.transform.localPosition = new Vector3(0, -0.25f, 1.954f);
        nokturnalParent.transform.localEulerAngles = new Vector3(0, 0, 0);
        zeigerPivot.localEulerAngles = new Vector3(90, 0, -27);
        zeigerPivotRotationObject.localEulerAngles = new Vector3(0, 0, 0);
        einstellringPivot.localEulerAngles = new Vector3(90, 0, 75);

    #endregion
    }

    //Uhr Slider zur Eingabe der Uhrzeit einblenden
    private void ShowInputClock() {
        tIC.ShowInputClock();
        cSSH.UpdateScrollClockSlider();
    }

    //Jedes Element am Nokturnal besitzt Collider, die reagieren wenn sie angetippt wurden, wenn ein falsches Element anegtippt wurde
    //dann ruft es diese Funktion auf, je nach Häufigkeit der Fehleingabe wird die Textausgabe generiert
    public void HitWrongWrapper() {
        StartCoroutine(HitWrong());
    }

    private IEnumerator HitWrong() {
        yield return new WaitForSeconds(1f);
        wrongInput += 1;
        if (wrongInput == 3) {
            Color color = zaehne.color;
            color.a = 255;
            zaehne.color = color;
            pC.JumpWithQuestInput(1);
        }
    }

    //Prüfen welche Eingabe im Slider gemacht wurde 
    public void CheckInput() {
        //richtige Antwort =21, fast richtig = 9, alles andere falsch
        int answer = cSSH.ClosestItemIndex;
        if (pC.GetLanguage() == 1) {
            answer += 1;
        }

        if (answer == 9) {
            showNokturnalZahlen = true;
            TimeInputController tIC = (TimeInputController) timeParent.GetComponent(typeof(TimeInputController));
            tIC.HideScrollPanel();
            pC.JumpWithQuestInput(2);
        } else if (answer == 21) {
            showNokturnalZahlen = true;
            TimeInputController tIC = (TimeInputController) timeParent.GetComponent(typeof(TimeInputController));
            tIC.HideScrollPanel();
            pC.JumpWithQuestInput(0);
        } else {
            pC.JumpWithQuestInput(1);
        }
    }

    public int GetWrongInputCount() {
        return wrongInput;
    }
}