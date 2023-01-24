using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class S01Helper : MonoBehaviour {
#region public Variables

    public SkyProfileEventHandler sPEH;
    public SkyRenderController sRC;
    public SkyTimeController skyTimeController;
    public CheckPointController cPC;
    public ClockController cc;
    public ContentScrollSnapHorizontal cSC;
    public MainMenuController mmc;
    public ProcedureController pC;
    public RotateStarrSky rotStarrySky;
    public SpielstandLoader ssL;
    public TimeInputController tIC;

    public Animator skyAnimator;
    public Animator horizontAnimator;
    public Animator sunDialCamAnimator;

    public Camera mainCam;
    public Camera sunDialCam;

    public GameObject kalender;
    public GameObject kalenderlinien;
    public GameObject mainCamObject;
    public GameObject mainCamera;
    public GameObject skyCamera;
    public GameObject skyCamObject;
    public GameObject sunDialCameraObject;
    public GameObject sunDialObject;
    public GameObject sunDialParent;
    public GameObject pfeil1;
    public GameObject pfeil2;
    public GameObject pfeil3;
    public GameObject pfeil4;
    public GameObject sonnenuhrMarkerparent;

    public Material fadenMat;
    public Material fadenHiglightLong;
    public Material radialLinienMat;
    public Material schattenwerferMat;
    public Material sunDialHighlight16;
    public Material sunDialKompassNadel;

    public Volume volume;

    public SpriteRenderer sunDialKompass;

    public Transform cameraCenterTransform;
    public Transform mainCamTransform;
    public Transform skyCamTransform;

#endregion

#region private Variables

    private bool turnRadialLinienOn = false;
    private bool turnRadialLinienOff = false;
    private bool stopSonnenAnim = false;
    private bool turnSchattenwerferOn = false;
    private bool turnFadenOn = false;
    private bool turnSchattenwerferOff = false;
    private bool turnFadenOff = false;
    private bool checkStopPos = false;
    private bool firstround = false;
    private bool turnSunDialKompassOff = false;
    private bool turnighlight16UhrOn = false;
    private bool turnfadenHighlightOn = false;
    private bool turnfadenHighlightOff = false;
    private bool moveCamToSunDial = false;
    private bool flashHighlightKompass = false;
    private bool highlightKompassOn = false;
    private bool highlightKompassOff = false;

    private float timeCount = 0.0f;
    private float t = 0.0f;

    private int aktAnswerHour = 12;

    private LensDistortion lD;

    private Quaternion startRot;
    private Quaternion outTarget;

#endregion

    void Start() {
        LensDistortion tmp;

        if (volume.profile.TryGet(out tmp)) {
            lD = tmp;
        }

        ProcedureController.changeEvent += DoActionWhileStepUpdate;
        skyTimeController.SetLatitude(48.0f);
    }


    public void DoActionWhileStepUpdate(string stepId) {
        switch (stepId) {
            case "S01.00a":
            case "S01.00":
                ResetScript();
                pC.JumpToNextStep();

                //Himmels-Simulation direkt zu Pfadbeginn starten
                skyAnimator.enabled = true;
                skyAnimator.speed = 1;
                skyAnimator.SetInteger("StartAnimateTimeline", 3);
                cameraCenterTransform.localEulerAngles = new Vector3(0, 240, 0);
                break;

            case "S01.10":
                //Kamera nach Süden drehen und Sonnenuhr einblenden
                moveCamToSunDial = true;
                stopSonnenAnim = true;
                skyAnimator.speed = 1.65f;
                sunDialParent.SetActive(true);
                sunDialObject.SetActive(true);
                break;

            case "S01.11":
                //SonnenuhrCamera aktivieren und in Split Screen übergehen
                sunDialCameraObject.SetActive(true);
                sunDialCamAnimator.enabled = true;
                sunDialCamAnimator.SetInteger("MoveSunDialCam", 8);
                StartCoroutine(FinishStep());
                break;

            case "S01.14":
                //Faden der Sonnenuhr highlighten
                turnFadenOn = true;
                turnfadenHighlightOn = true;
                turnRadialLinienOff = true;
                break;

            case "S01.15":
                checkStopPos = true;
                skyAnimator.speed = 1.55f;
                break;

        #region Checkpoint

            case "S01.26C":
                sunDialParent.SetActive(true);
                sunDialObject.SetActive(true);
                sunDialCameraObject.SetActive(true);
                ResetScript();
                StopAllCoroutines();
                checkStopPos = false;
                sunDialCamAnimator.enabled = true;
                cameraCenterTransform.rotation = Quaternion.Euler(0, 160, 0);
                turnFadenOn = true;
                turnfadenHighlightOn = true;
                sunDialParent.transform.parent = null;
                skyAnimator.enabled = false;
                skyTimeController.animTime = 16f;
                sunDialCamAnimator.Play("RotateSunDial", 0, 0);
                sunDialCamAnimator.speed = 0;
                skyTimeController.SetTimeline(16);
                StartCoroutine(WaitndInitAnim());
                pC.JumpToNextStep();
                break;

        #endregion

            case "S01.27":
                cPC.SetCheckPointColors("S01", 2);
                cPC.SetCheckPointReached("S01", 02);

                //Fadenhighlight ausblenden
                turnFadenOff = true;
                turnfadenHighlightOff = true;
                sunDialParent.transform.parent = null;

                //Sonnenuhr hin und her schwenken (Ausrichten mit Kompass andeuten)
                sunDialCamAnimator.SetInteger("MoveSunDialCam", 3);
                sunDialCamAnimator.speed = 1;
                break;

            case "S01.27a":
                //Highlight zum "S" nach Süden
                pfeil1.SetActive(true);
                pfeil2.SetActive(true);
                pfeil3.SetActive(true);
                pfeil4.SetActive(true);
                horizontAnimator.enabled = true;
                horizontAnimator.Play("HighlightSueden_S01", 0, 0);
                horizontAnimator.SetInteger("AnimateState", 1);
                horizontAnimator.speed = 0.35f;
                break;

        #region Checkpoint

            case "S01.49C":
                ResetScript();
                StopAllCoroutines();
                checkStopPos = false;
                sunDialCamAnimator.enabled = true;
                cameraCenterTransform.rotation = Quaternion.Euler(0, 160, 0);
                sunDialParent.transform.parent = null;
                skyAnimator.enabled = false;
                skyTimeController.animTime = 16f;
                skyTimeController.SetTimeline(16);
                horizontAnimator.Rebind();
                horizontAnimator.enabled = false;
                flashHighlightKompass = true;
                highlightKompassOn = true;

                sunDialParent.SetActive(true);
                sunDialObject.SetActive(true);
                sunDialCameraObject.SetActive(true);
                pC.JumpToNextStep();
                break;

        #endregion

            case "S01.49":
                horizontAnimator.Rebind();
                horizontAnimator.enabled = false;

                cPC.SetCheckPointColors("S01", 3);
                cPC.SetCheckPointReached("S01", 03);

                //Ortho-Kamera als Vollbild
                sunDialCamAnimator.Play("SunDialFullView", 0, 0);
                sunDialCamAnimator.speed = 1;
                sunDialCamAnimator.SetInteger("MoveSunDialCam", 4);

                //Kompass highlighten
                flashHighlightKompass = true;
                highlightKompassOn = true;
                Color sunDialKompassNadelColor = new Color32(158, 157, 236, 255);
                sunDialKompassNadel.color = sunDialKompassNadelColor;
                StartCoroutine(WaitBeforeJump());
                break;

            case "S01.51":
                flashHighlightKompass = false;
                highlightKompassOn = false;
                turnSunDialKompassOff = true;
                mainCamObject.SetActive(false);
                skyCamObject.SetActive(false);
                break;

            case "S01.52":
                //Slider einblenden für Uhrzeiteingabe
                tIC.ShowInputClock();
                cSC.UpdateScrollClockSlider();
                break;

            case "S01.53":
                turnighlight16UhrOn = true;
                break;

            case "S01.62":
                skyAnimator.speed = 0;
                pC.JumpToNextStep();
                break;

            case "S01.63":
                //Slider mit Uhrzeit ausblenden
                tIC.HideScrollPanel();
                
                mainCamObject.SetActive(true);
                sonnenuhrMarkerparent.SetActive(true);
                
                ssL.FinishedPathPoint("S01");
                pC.JumpToNextStep();
                mmc.SetS01Done();
                break;

            case "S01.64":
                tIC.HideScrollPanel();
                mainCamObject.SetActive(true);
                sonnenuhrMarkerparent.SetActive(true);
                ssL.FinishedPathPoint("S01");
                pC.JumpToNextStep();
                mmc.SetS01Done();
                break;
        }
    }


    private void Update() {
        //Wir beginnen mit Blick nach Westen, hier die Kamera Richtung Süden drehen und dadurch die Sonne einblenden
        if (moveCamToSunDial) {
            timeCount = timeCount + Time.deltaTime;
            t = timeCount / 3.0f;
            t = t * t * (3f - 2f * t);
            cameraCenterTransform.rotation = Quaternion.Slerp(startRot, outTarget, t);
            if (cameraCenterTransform.localEulerAngles.y <= 161f) {
                moveCamToSunDial = false;
            }
        }

        //Für den ersten Blick auf die Sonnenuhr sollte auch der Schatten zu sehen sein,
        //deswegen die Animation nur zweischen 10 und 16 uhr stoppen
        if (stopSonnenAnim) {
            float akttime = skyTimeController.GetAnimTime();
            if (akttime >= 24) {
                akttime -= 24;
            }

            if (akttime > 10.0f && akttime < 16.0f) {
                skyAnimator.speed = 0.0f;

                stopSonnenAnim = false;
            }
        }

        //Simulation einen Tag ablaufen lassen und dann bei 16 Uhr stoppen
        if (checkStopPos) {
            float akttime = skyTimeController.GetAnimTime();

            if (akttime > 24) {
                akttime -= 24;
            }

            if (akttime > 17) {
                firstround = true;
            }

            if (firstround && akttime > 16f && akttime < 17) {
                StartCoroutine(FinishStepAndJump());
                skyTimeController.animTime = 16;
                skyAnimator.speed = 0.0f;
                checkStopPos = false;
            }
        }

    #region graphical Actions

        if (flashHighlightKompass) {
            if (highlightKompassOn) {
                Color sundDialKompassColor = sunDialKompass.color;
                if (sundDialKompassColor.a < 1) {
                    sundDialKompassColor.a += 0.1f;
                    sunDialKompass.color = sundDialKompassColor;
                } else {
                    highlightKompassOff = true;
                    highlightKompassOn = false;
                }
            }

            if (highlightKompassOff) {
                Color sundDialKompassColor = sunDialKompass.color;
                if (sundDialKompassColor.a > 0) {
                    sundDialKompassColor.a -= 0.05f;
                    sunDialKompass.color = sundDialKompassColor;
                } else {
                    highlightKompassOn = true;
                    highlightKompassOff = false;
                }
            }
        }

        if (turnRadialLinienOn) {
            Color radialLinien = radialLinienMat.color;
            if (radialLinien.a < 1) {
                radialLinien.a += 0.03f;
                radialLinienMat.color = radialLinien;
            } else {
                turnRadialLinienOn = false;
            }
        }

        if (turnRadialLinienOff) {
            Color radialLinien = radialLinienMat.color;
            if (radialLinien.a > 0) {
                radialLinien.a -= 0.1f;
                radialLinienMat.color = radialLinien;
            } else {
                radialLinien.a = 0;
                radialLinienMat.color = radialLinien;
                turnRadialLinienOff = false;
            }
        }

        if (turnSchattenwerferOn) {
            Color schattenwerferColor = schattenwerferMat.color;
            if (schattenwerferColor.a < 0.705f) {
                schattenwerferColor.a += 0.05f;
                schattenwerferMat.color = schattenwerferColor;
            } else {
                StartCoroutine(FinishStepAndJump());
                turnSchattenwerferOn = false;
            }
        }

        if (turnFadenOn) {
            Color fadenSchattenwerfer = fadenMat.color;
            if (fadenSchattenwerfer.r < 1 && fadenSchattenwerfer.g > 0 && fadenSchattenwerfer.b > 0) {
                fadenSchattenwerfer.r += 0.05f;
                fadenSchattenwerfer.g -= 0.05f;
                fadenSchattenwerfer.b -= 0.05f;
                fadenMat.color = fadenSchattenwerfer;
            } else {
                turnFadenOn = false;
            }
        }

        if (turnfadenHighlightOn) {
            Color fadenHighlightLongColor = fadenHiglightLong.color;
            if (fadenHighlightLongColor.a < 1) {
                fadenHighlightLongColor.a += 0.075f;
                fadenHiglightLong.color = fadenHighlightLongColor;
            } else {
                turnfadenHighlightOn = false;
            }
        }

        if (turnSchattenwerferOff) {
            Color schattenwerferColor = schattenwerferMat.color;
            if (schattenwerferColor.a > 0f) {
                schattenwerferColor.a -= 0.1f;
                schattenwerferMat.color = schattenwerferColor;
            } else {
                StartCoroutine(FinishStepAndJump());
                turnSchattenwerferOff = false;
            }
        }

        if (turnFadenOff) {
            Color fadenSchattenwerfer = fadenMat.color;
            if (fadenSchattenwerfer.r > 0.45f && fadenSchattenwerfer.g < 0.45f && fadenSchattenwerfer.b < 0.45f) {
                fadenSchattenwerfer.r -= 0.1f;
                fadenSchattenwerfer.g += 0.1f;
                fadenSchattenwerfer.b += 0.1f;
                fadenMat.color = fadenSchattenwerfer;
            } else {
                turnFadenOff = false;
            }
        }

        if (turnfadenHighlightOff) {
            Color fadenHighlightLongColor = fadenHiglightLong.color;
            if (fadenHighlightLongColor.a > 0) {
                fadenHighlightLongColor.a -= 0.1f;
                fadenHiglightLong.color = fadenHighlightLongColor;
            } else {
                turnfadenHighlightOff = false;
            }
        }

        if (turnSunDialKompassOff) {
            Color sundDialKompassColor = sunDialKompass.color;
            if (sundDialKompassColor.a > 0) {
                sundDialKompassColor.a -= 0.1f;
                sunDialKompass.color = sundDialKompassColor;
            } else {
                //StartCoroutine(FinishStepAndJump());
                Color sunDialKompassNadelColor = new Color32(183, 157, 96, 255);
                sunDialKompassNadel.color = sunDialKompassNadelColor;
                turnSunDialKompassOff = false;
            }
        }

        if (turnighlight16UhrOn) {
            Color sunDialHighlight16Color = sunDialHighlight16.color;
            if (sunDialHighlight16Color.a < 1) {
                sunDialHighlight16Color.a += 0.75f;
                sunDialHighlight16.color = sunDialHighlight16Color;
            } else {
                turnighlight16UhrOn = false;
            }
        }

    #endregion
    }

    private void ResetScript() {
    #region Reset private Variables

        turnfadenHighlightOff = false;
        turnfadenHighlightOn = false;
        aktAnswerHour = 12;
        turnRadialLinienOn = false;
        turnRadialLinienOff = false;
        stopSonnenAnim = false;
        turnSchattenwerferOn = false;
        turnFadenOn = false;
        turnSchattenwerferOff = false;
        turnFadenOff = false;
        checkStopPos = false;
        firstround = false;
        turnSunDialKompassOff = false;
        turnighlight16UhrOn = false;
        flashHighlightKompass = false;
        highlightKompassOn = false;
        highlightKompassOff = false;

        startRot = Quaternion.Euler(0, 240, 0);
        outTarget = Quaternion.Euler(0, 160, 0);
        timeCount = 0.0f;
        t = 0.0f;

    #endregion

    #region Reset Camera

        mainCamera.SetActive(true);
        skyCamera.SetActive(true);

        cameraCenterTransform.rotation = Quaternion.Euler(0, 160, 0);
        sunDialCam.transform.localPosition = new Vector3(0, 112, 225.6f);
        skyCamTransform.localPosition = new Vector3(0, 40, -4);
        mainCamTransform.localEulerAngles = new Vector3(0, 0, 0);

        float resolutionFactor = DeviceInfo.GetResolutionFactor();
        float temp = ((resolutionFactor - 0.45f) * 25.0f / 0.3f) + 40;
        mainCam.fieldOfView = temp;

        sunDialCamAnimator.Rebind();
        sunDialCamAnimator.Update(0f);
        sunDialCamAnimator.enabled = true;

        ShowSplitScreen sSS = (ShowSplitScreen) sunDialParent.GetComponent(typeof(ShowSplitScreen));
        sSS.Reset();

        lD.scale.Override(1.04f);
        sunDialCam.orthographicSize = 65;

    #endregion

    #region Reset Date and Time

        cc.SetRealTime();
        cc.StopUpdateDate();

        tIC.HideScrollPanel();

        sPEH.SetAllowUpdateLightTrue();
        skyTimeController.SetAllowSetTimeLineWithAnim();
        skyTimeController.SetDate(2021, 4, 1);
        skyTimeController.SetLongitude(11);

        cc.SetCurrentDate();
        cc.ChangeDateTextColor(1);
    #endregion

    #region Reset Materials

        Color radialLinien = radialLinienMat.color;
        radialLinien.a = 0.0f;
        radialLinienMat.color = radialLinien;

        Color schattenwerferColor = schattenwerferMat.color;
        schattenwerferColor.a = 0.0f;
        schattenwerferMat.color = schattenwerferColor;

        Color fadenSchattenwerfer = fadenMat.color;
        fadenSchattenwerfer.r = 0.45f;
        fadenSchattenwerfer.g = 0.45f;
        fadenSchattenwerfer.b = 0.45f;
        fadenMat.color = fadenSchattenwerfer;

        Color fadenHighlightLongColor = fadenHiglightLong.color;
        fadenHighlightLongColor.a = 0.0f;
        fadenHiglightLong.color = fadenHighlightLongColor;

        Color sunDialHighlight16Color = sunDialHighlight16.color;
        sunDialHighlight16Color.a = 0;
        sunDialHighlight16.color = sunDialHighlight16Color;

        Color sundDialKompassColor = sunDialKompass.color;
        sundDialKompassColor.a = 0;
        sunDialKompass.color = sundDialKompassColor;

    #endregion

    #region Reset Other

        rotStarrySky.AllowCalculateRotationContinous();
        sRC.enabled = true;

        kalender.SetActive(false);
        kalenderlinien.SetActive(false);

        sunDialParent.transform.localPosition = new Vector3(35, -7.2f, -310);
        horizontAnimator.Rebind();
        horizontAnimator.Update(0f);

    #endregion
    }

    private IEnumerator WaitndInitAnim() {
        yield return new WaitForSeconds(0.01f);
        ShowSplitScreen sSS = (ShowSplitScreen) sunDialParent.GetComponent(typeof(ShowSplitScreen));
        sSS.SetAllowScaleLD();
        sSS.SetAllowSetMainCamFrame();
    }

    private IEnumerator FinishStep() {
        yield return new WaitForSeconds(0.5f);
        turnRadialLinienOn = true;
        pC.JumpToNextStep();
    }

    private IEnumerator WaitBeforeJump() {
        yield return new WaitForSeconds(2f);
        pC.JumpToNextStep();
    }

    private IEnumerator FinishStepAndJump() {
        yield return new WaitForSeconds(0.5f);
        pC.JumpToNextStep();
    }

    public void BtnS01InputClicked(int value) {
        pC.JumpWithQuestInput(value);
    }

    public void CheckS0152Task() {
        int answer = cSC.ClosestItemIndex;
        if (pC.GetLanguage() == 1) {
            answer += 1;
        }

        if (answer == 16) {
            pC.JumpWithQuestInput(1);
        } else if (answer == 4) {
            pC.JumpWithQuestInput(2);
        } else {
            pC.JumpWithQuestInput(0);
        }
    }
}