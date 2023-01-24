using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class S05Helper : MonoBehaviour {
#region public Variables

    public CheckPointController cPC;
    public ContentScrollSnapHorizontal cSC;
    public ContentScrollNonInteractableCalendar cSNIC;
    public ContentScrollSnapHorizontal cSIN;
    public ContentScrollSnapHorizontal cMoonDifference;
    public ProcedureController pC;
    public TimeInputController tIC;
    public RotateStarrSky rSS;
    public SkyRenderController sRC;
    public SpielstandLoader ssL;
    public SkyTimeController skyTimeController;
    public SkyProfileEventHandler sPEH;
    public RotateMoonVolvelle rMV;
    
    public Animator skyAnimator;
    public Animator sunDialAnimator;

    public Button okButton;

    public CanvasGroup mondVolvelleLupe;

    public Camera mainCam;
    public Camera skyCam;
    public Camera sunDialCam;

    public GameObject clockParentGO;
    public GameObject fakeMoon;
    public GameObject gameController;
    public GameObject mainCamObject;
    public GameObject skyCamObject;
    public GameObject sunDialCamGo;
    public GameObject sunDialObject;
    public GameObject sunDialParent;
    public GameObject weiterButtonTMPPRO;
    public GameObject weiterButtonUIText;
    public GameObject subheadlineUIText;
    public GameObject subheadlineTMPRO;
    public GameObject sunDialDirectionalLightNight;
    public GameObject sonnenUhrAmbientLight;

    public Light moonDirectLight;

    public RectTransform splitLine;

    public Sprite spriteHiglightMit12;
    public Sprite spriteHiglightOhne12;

    public SpriteRenderer moonVolvelleHighlight;
    public SpriteRenderer mondvolvelleHighlight;

    public Transform cameraCenterTransform;
    public Transform mainCamTransform;
    public Transform mainCamParenTransform;
    public Transform skyCamTransform;
    public Transform skyCamParentTransform;
    public Transform sunDirectionalLight;
    public Transform moonVolvellePivot;
    public Transform firstTextTmPro;
    public Transform secondTextTmPro;
    public Transform firstTextUnityUI;
    public Transform secondTextUnityUI;
    public Transform bestaetigenTmPro;
    public Transform bestaetigenUnityUI;

    public Volume volume;

#endregion

#region private Variables

    private int aktAnswerHour = 10;
    private int answerHourHalbmondSchritt1 = 10;
    private int answerHourHalbmondSchritt2 = 0;
    private int answerHourHalbmondSchritt3 = 10;

    private bool turnMoonVolvelleHighlightOn = false;
    private bool turnMoonVolvelleHighlightOff = false;
    private bool turnMoonVolvelleLupeOn = false;
    private bool turnMoonVolvelleLupeOff = false;
    private bool rotateMoonVolvelleToFullMoon = false;

    private float timeCount = 0.0f;
    private float t = 0.0f;

    private LensDistortion lD;

    private Quaternion startRot;
    private Quaternion outTarget;

#endregion

    void Start() {
        pC = (ProcedureController) gameController.GetComponent(typeof(ProcedureController));
        InitS05();
    }

    public void InitS05() {
        LensDistortion tmp;

        if (volume.profile.TryGet(out tmp)) {
            lD = tmp;
        }

        ProcedureController.changeEvent += DoActionWhileStepUpdate;
        skyTimeController.SetLatitude(48.0f);
    }


    public void ResetScript() {
        StopAllCoroutines();

    #region Reset Camera

        lD.intensity.Override(0.3f);
        lD.scale.Override(1.04f);

        skyCamTransform.localPosition = new Vector3(0, 0, 0);
        skyCamParentTransform.localPosition = new Vector3(0, 40, 0);
        mainCamTransform.localEulerAngles = new Vector3(0, 0, 0);
        mainCamObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        mainCamParenTransform.localEulerAngles = new Vector3(-14, 0, 0);
        mainCamParenTransform.localPosition = new Vector3(-38, 1.1f, -2f);

        float resolutionFactor = DeviceInfo.GetResolutionFactor();
        float newFOV = 35 + (((resolutionFactor - 0.45f) * 15.0f) / 0.3f);
        mainCam.fieldOfView = newFOV;
        skyCam.fieldOfView = newFOV + 5;

        cameraCenterTransform.localEulerAngles = new Vector3(0, -180, 0);
        skyCamObject.SetActive(true);

    #endregion

    #region Reset Date and Time

        ClockController cc = (ClockController) clockParentGO.GetComponent(typeof(ClockController));
        cc.SetTimeModeN07();
        cc.ChangeDateTextColor(1);
        cc.ChangeTimeTextColor(1);
        cc.SetCurrentDate();

        sPEH.SetAllowUpdateLightTrue();
        sPEH.TurnMoonOn();
        sPEH.StopTheRotationWithAnimInS05WithoutJump();
        skyTimeController.SetAllowSetTimeLineWithAnim();
        skyTimeController.StopIncreaseDayWitAnim();
        skyTimeController.SetDate(2021, 09, 22);
        skyTimeController.SetUtc(5);
        skyTimeController.SetLongitude(51);
        skyTimeController.SetLongitude(13.7f);

        tIC.HideScrollPanel();

    #endregion

    #region Reset Other

        splitLine.anchoredPosition = new Vector2(splitLine.anchoredPosition.x, 0);
        StartCoroutine(InitTexte());

    #endregion

    #region Reset private Variables

        startRot = Quaternion.Euler(0, 70, 0);
        outTarget = Quaternion.Euler(0, 10, 0);
        t = 0.0f;
        timeCount = 0.0f;
        rotateMoonVolvelleToFullMoon = false;
        aktAnswerHour = 10;
        answerHourHalbmondSchritt1 = 10;
        answerHourHalbmondSchritt2 = 0;
        answerHourHalbmondSchritt3 = 10;
        turnMoonVolvelleLupeOn = false;
        turnMoonVolvelleLupeOff = false;

    #endregion

    #region Reset Starry Sky

        rSS.AllowCalculateRotationContinous();
        skyAnimator.enabled = true;
        skyAnimator.Rebind();
        skyAnimator.Update(0f);
        skyAnimator.SetInteger("StartAnimateTimeline", 9);
        skyAnimator.Play("S05MoonNight", 0, 0);
        skyAnimator.speed = 0;

        fakeMoon.SetActive(true);
        sRC.moonTextureSize = 200;

    #endregion

    #region Reset Sonnenuhr

        moonVolvellePivot.transform.localEulerAngles = new Vector3(0, 250, 0);
        sunDialAnimator.Rebind();
        sunDialAnimator.Update(0f);

        ShowSplitScreen sSS = (ShowSplitScreen) sunDialParent.GetComponent(typeof(ShowSplitScreen));
        sSS.Reset();

        moonDirectLight.enabled = true;
        moonDirectLight.intensity = 0.6f;
        sunDialParent.SetActive(true);
        sunDialObject.SetActive(true);
        sunDialParent.transform.position = new Vector3(40, -7.2f, -320f);
        sunDialCam.rect = new Rect(0, 0, 1, 0);
        sunDialCam.orthographicSize = 65;
        sonnenUhrAmbientLight.SetActive(false);
        mondVolvelleLupe.alpha = 0;
        sunDirectionalLight.position = new Vector3(-102, 7.55f, 5.1f);

        rMV.StopRotation();

        mondvolvelleHighlight.sprite = spriteHiglightMit12;
        sunDialDirectionalLightNight.SetActive(true);

    #endregion
    }

    private void Update() {
        if (rotateMoonVolvelleToFullMoon) {
            timeCount = timeCount + Time.deltaTime;
            t = timeCount / 3.0f;
            t = t * t * (3f - 2f * t);
            moonVolvellePivot.transform.rotation = Quaternion.Slerp(startRot, outTarget, t);
            if (moonVolvellePivot.transform.localEulerAngles.y <= 191.5f) {
                okButton.interactable = true;
                moonVolvellePivot.transform.localEulerAngles = new Vector3(0, 190, 0);
                rotateMoonVolvelleToFullMoon = false;
            }
        }

    #region graphical Actions

        if (turnMoonVolvelleLupeOn) {
            Color moonVolvelleHighlightColor = moonVolvelleHighlight.color;
            if (mondVolvelleLupe.alpha < 1) {
                mondVolvelleLupe.alpha += 0.075f;
                moonVolvelleHighlightColor.a += 0.075f;
                moonVolvelleHighlight.color = moonVolvelleHighlightColor;
            } else {
                mondVolvelleLupe.alpha = 1;
                moonVolvelleHighlightColor.a = 1;
                moonVolvelleHighlight.color = moonVolvelleHighlightColor;
                turnMoonVolvelleLupeOn = false;
            }
        }

        if (turnMoonVolvelleLupeOff) {
            Color moonVolvelleHighlightColor = moonVolvelleHighlight.color;
            if (mondVolvelleLupe.alpha > 0) {
                mondVolvelleLupe.alpha -= 0.075f;
                moonVolvelleHighlightColor.a -= 0.075f;
                moonVolvelleHighlight.color = moonVolvelleHighlightColor;
            } else {
                mondVolvelleLupe.alpha = 0;
                moonVolvelleHighlightColor.a = 0;
                moonVolvelleHighlight.color = moonVolvelleHighlightColor;
                turnMoonVolvelleLupeOff = false;
            }
        }

        if (turnMoonVolvelleHighlightOn) {
            Color moonVolvelleHighlightColor = moonVolvelleHighlight.color;
            if (moonVolvelleHighlightColor.a < 1) {
                moonVolvelleHighlightColor.a += 0.075f;
                moonVolvelleHighlight.color = moonVolvelleHighlightColor;
            } else {
                turnMoonVolvelleHighlightOn = false;
            }
        }

        if (turnMoonVolvelleHighlightOff) {
            Color moonVolvelleHighlightColor = moonVolvelleHighlight.color;
            if (moonVolvelleHighlightColor.a > 0) {
                moonVolvelleHighlightColor.a -= 0.1f;
                moonVolvelleHighlight.color = moonVolvelleHighlightColor;
            } else {
                turnMoonVolvelleHighlightOff = false;
            }
        }

    #endregion
    }

    public void DoActionWhileStepUpdate(string stepId) {
        

        switch (stepId) {
            case "S05.00a":
            case "S05.00":
                ResetScript();
                pC.JumpToNextStep();
                break;

            case "S05.03a":
                skyAnimator.speed = 1;
                break;

            case "S05.07":
                sunDialCamGo.SetActive(true);
                sunDialAnimator.SetInteger("MoveSunDialCam", 5);
                break;

            case "S05.16":
                skyCamObject.SetActive(false);
                tIC.ShowInputNumbers();
                cSIN.UpdateScrollInputNumberSliderS05();
                break;

            case "S05.30":
                tIC.HideScrollPanel();
                sunDialAnimator.Play("S05_ZoomToMoonVolvelle", 0, 0);
                sunDialAnimator.speed = 1;
                sunDialAnimator.SetInteger("MoveSunDialCam", 6);
                break;

            case "S05.31":
                rotateMoonVolvelleToFullMoon = true;
                t = 0;
                timeCount = 0;
                break;

            case "S05.31a":
                turnMoonVolvelleHighlightOn = true;
                break;

            case "S05.35":
                mainCamObject.SetActive(true);
                skyCamObject.SetActive(true);
                turnMoonVolvelleHighlightOff = true;
                sunDialAnimator.Play("S037MoonVolvelleToSplitscreen", 0, 0);
                sunDialAnimator.speed = 1;
                sunDialAnimator.SetInteger("MoveSunDialCam", 7);
                mondvolvelleHighlight.sprite = spriteHiglightOhne12;
                break;

            case "S05.37":
                tIC.ShowMoonToHalfMoon();
                StartCoroutine(AnimateToHalfMoon());
                fakeMoon.SetActive(false);
                sPEH.HideMoon();
                StartCoroutine(WaitForASecondAndShowRealMoon());
                break;

            case "S05.38C":
                ResetScript();
                cameraCenterTransform.localEulerAngles = new Vector3(0, 180, 0);
                mainCamTransform.localEulerAngles = new Vector3(-25, 0, 0);
                lD.scale.Override(1.4f);
                skyTimeController.SetDate(2021, 9, 29);
                skyTimeController.SetLongitude(13.7f);

                tIC.HideScrollPanel();

                sunDialAnimator.SetInteger("MoveSunDialCam", 9);
                sunDialAnimator.Play("S038MoonFromZeroToSplit", 0, 0);
                break;

            case "S05.38":
                cPC.SetCheckPointColors("S05", 2);
                cPC.SetCheckPointReached("S05", 02);
                rMV.AllowRotation();
                break;

            case "S05.49":
                rMV.StopRotation();
                sunDialAnimator.Play("S049FullViewSunDial", 0, 0);
                sunDialAnimator.speed = 1;
                sunDialAnimator.SetInteger("MoveSunDialCam", 8);
                skyAnimator.speed = 1;
                break;
            case "S05.50":
                tIC.ShowInputNumbers();
                cSIN.UpdateScrollInputNumberSliderS05();
                break;

            case "S05.64":
                tIC.HideScrollPanel();
                sunDialAnimator.Play("S05_ZoomToMoonVolvelle", 0, 0);
                sunDialAnimator.speed = 1;
                sunDialAnimator.SetInteger("MoveSunDialCam", 6);
                break;

            case "S05.65":
                turnMoonVolvelleLupeOn = true;
                break;

            case "S05.66":
                tIC.ShowInputMoonDifference();
                cMoonDifference.UpdateMoonDifferenceSliderS05();
                break;

            case "S05.76":
                break;

            case "S05.78":
                //s0578Input.transform.localScale = new Vector3(1, 1, 1);
                //s0578Input.transform.GetComponent<CanvasGroup>().alpha = 1;
                tIC.ShowInputClock();
                cSC.UpdateScrollClockSlider();
                break;

            case "S05.83":

                ssL.FinishedPathPoint("S05");
                tIC.HideScrollPanel();
                break;

            case "S05.85":
                tIC.HideScrollPanel();
                break;
        }
    }

    private IEnumerator AnimateToHalfMoon() {
        yield return new WaitForSeconds(1f);
        skyAnimator.Play("S037AnimateToHalfMoon", 0, 0);
        skyAnimator.SetInteger("StartAnimateTimeline", 10);
        skyAnimator.speed = 2.5f;
        cSNIC.SetTargetScrollPosMoonCalendar(0.1418f);
        cSNIC.UpdateFontManually(1, 0);

        yield return new WaitForSeconds(1.2f);
        cSNIC.SetTargetScrollPosMoonCalendar(0.2855f);
        cSNIC.UpdateFontManually(2, 1);

        yield return new WaitForSeconds(1.2f);
        cSNIC.SetTargetScrollPosMoonCalendar(0.4288f);
        cSNIC.UpdateFontManually(3, 2);

        yield return new WaitForSeconds(1.2f);
        cSNIC.SetTargetScrollPosMoonCalendar(0.5713f);
        cSNIC.UpdateFontManually(4, 3);

        yield return new WaitForSeconds(1.2f);
        cSNIC.SetTargetScrollPosMoonCalendar(0.7136f);
        cSNIC.UpdateFontManually(5, 4);

        yield return new WaitForSeconds(1.2f);
        cSNIC.SetTargetScrollPosMoonCalendar(0.8565f);
        cSNIC.UpdateFontManually(6, 5);

        yield return new WaitForSeconds(1.2f);
        cSNIC.SetTargetScrollPosMoonCalendar(1f);
        cSNIC.UpdateFontManually(7, 6);

        skyTimeController.SetDate(2021, 9, 29);
        skyTimeController.SetLongitude(13.7f);

        yield return new WaitForSeconds(2f);
        tIC.HideScrollPanel();

        sunDialAnimator.SetInteger("MoveSunDialCam", 9);
        sunDialAnimator.Play("S038MoonFromZeroToSplit", 0, 0);
    }

#region Check Inputs

    public void CheckInputS0516() {
        int aktAnswerHour = cSIN.ClosestItemIndex;
        if (aktAnswerHour == 1) {
            pC.JumpWithQuestInput(0);
            tIC.HideScrollPanel();
        } else {
            pC.JumpWithQuestInput(1);
        }
    }

    public void CheckInputS0550() {
        answerHourHalbmondSchritt1 = cSIN.ClosestItemIndex;
        if (answerHourHalbmondSchritt1 == 8) {
            pC.JumpWithQuestInput(0);
            tIC.HideScrollPanel();
        } else {
            pC.JumpWithQuestInput(1);
        }
    }

    public void CheckInputS0566() {
        answerHourHalbmondSchritt2 = cMoonDifference.ClosestItemIndex;
        if (answerHourHalbmondSchritt2 == 18) {
            pC.JumpWithQuestInput(0);
            tIC.HideScrollPanel();
            turnMoonVolvelleLupeOff = true;
        } else {
            pC.JumpWithQuestInput(1);
        }
    }

    public void CheckInputS0578() {
        int inputTime = cSC.ClosestItemIndex;
        if (pC.GetLanguage() == 1) {
            inputTime += 1;
        }

        if (inputTime == 2) {
            pC.JumpWithQuestInput(0);
        } else {
            pC.JumpWithQuestInput(1);
        }
    }

#endregion

    //Have to use Unity.UI.Text in S05, because of Problems with Split Screen
    private IEnumerator InitTexte() {
        yield return new WaitForSeconds(0.4f);
        firstTextTmPro.localScale = new Vector3(0, 0, 0);
        firstTextUnityUI.localScale = new Vector3(1, 1, 1);
        secondTextTmPro.localScale = new Vector3(0, 0, 0);
        secondTextUnityUI.localScale = new Vector3(1, 1, 1);
        bestaetigenTmPro.localScale = new Vector3(0, 0, 0);
        bestaetigenUnityUI.localScale = new Vector3(1, 1, 1);
        weiterButtonTMPPRO.SetActive(false);
        weiterButtonUIText.SetActive(true);
        subheadlineTMPRO.SetActive(false);
        subheadlineUIText.SetActive(true);
    }

    private IEnumerator WaitForASecondAndShowRealMoon() {
        yield return new WaitForSeconds(2f);
        sRC.moonTextureSize = 9;
    }
}