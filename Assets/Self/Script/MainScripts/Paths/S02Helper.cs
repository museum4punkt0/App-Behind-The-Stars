using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class S02Helper : MonoBehaviour {
#region public Variables

    //Scripts
    public SkyProfileEventHandler sPEH;
    public AnimationEventHandlerCameraS02 aEHS02;
    public SkyRenderController sRC;
    public SkyTimeController skyTimeController;
    public CheckPointController cPC;
    public ClockController cc;
    public ContentScrollNonInteractableCalendar cSINC;
    public ProcedureController pC;
    public RotateStarrSky rotStarrySky;
    public SpielstandLoader ssL;
    public TimeInputController tIC;

    //other public Objects

    public Animator skyAnimator;
    public Animator cameraAnimator;
    public Animator sunDialCamAnimator;

    public Button okButton;

    public Camera mainCam;
    public Camera skyCam;
    public Camera sunDialCamera;

    public GameObject dateSliderParent;
    public GameObject clockParentGO;
    public GameObject gameController;
    public GameObject kalender;
    public GameObject kalenderlinien;
    public GameObject kalenderlinienKrebs;
    public GameObject krebsRahmen;
    public GameObject lilaHelpPoint;
    public GameObject mainCamObject;
    public GameObject mainCamera;
    public GameObject moonLight;
    public GameObject shadowCalcParent;
    public GameObject skyCamObject;
    public GameObject skyCamera;
    public GameObject summerTextGO;
    public GameObject sunDialCameraObject;
    public GameObject sunDialObject;
    public GameObject sunDialParent;
    public GameObject winterTextGO;

    public Image krebsLupeSprite;

    public LineRenderer summerLineTR;
    public LineRenderer summerLineTRUhr;
    public LineRenderer winterLineTR;
    public LineRenderer winterLineTRUhr;

    public Material calendarLinesMat;
    public Material groundMat;
    public Material schattenwerferHighlight;
    public Material sunMarker;
    public Material sunMat;
    public Material sunLineSommer;
    public Material sunLineSommerUhr;
    public Material sunLineWinter;
    public Material sunLineWinterUhr;
    public Material kalenderSternzeichenBlack;

    public MeshRenderer groundPlane;

    public SpriteRenderer kalenderLinieSR;
    public SpriteRenderer kalenderSide;
    public SpriteRenderer kalenderKrebsHighlight;
    public SpriteRenderer krebsRahmenSprite;

    public TextMeshPro summerText;
    public TextMeshPro winterText;

    public Transform calcedShadowPoint;
    public Transform cameraCenterTransform;
    public Transform drawWithFingerObject;
    public Transform gridSvg;
    public Transform krebsLupe;
    public Transform mainCamTransform;
    public Transform schattenwerferKugel;
    public Transform skyCamTransform;
    public Transform sunDirectionalLight;
    public Transform sunMarkerTransf;

    public TrailRenderer calcedShadowPointTR;
    public TrailRenderer drawWitMouseTR;

    public Volume volume;

#endregion

#region private Variables

    private bool animateToSommer = false;
    private bool allowDrawLineWithMouse = false;
    private bool turnSunOn = false;
    private bool turnSunOff = false;
    private bool turnSunDialCalendarOn = false;
    private bool jumpToNextDates = false;
    private bool setDayAt24 = false;
    private bool traceafter24 = false;
    private bool checkRangeInS0207 = false;
    private bool checkMousePosInS0208 = false;
    private bool checkStartTraceDailyLine = false;
    private bool checkStopTraceDailyLine = false;
    private bool drawWinterLine = false;
    private bool drawSummerLine = false;
    private bool firstClickWhenDraw = false;
    private bool turnKalenderOn = false;
    private bool jumpIn02Done = false;
    private bool turnKrebsRahmenOn = false;
    private bool turnKrebsRahmenOff = false;
    private bool makeWinterLineTransparent = false;
    private bool showSummerText = false;
    private bool showWintertext = false;
    private bool drawSummerLineUhr = false;
    private bool drawWinterLineUhr = false;
    private bool turnBlacKalenderOff = false;
    private bool checkTimeAndStopIfDay = false;

    private float deltaMousePos = 0.0f;
    private float speed = 0.1f;
    private float startTime = 0.0f;
    private float mouseZCoordinate;

    private float[] monthScrollTargets = {
        0.0018f, 0.0836f, 0.1564f, 0.2222f, 0.2764f, 0.3257f, 0.3758f, 0.4252f, 0.4848f, 0.5737f, 0.6625f, 0.7475f,
        0.84f
    };

    private int countMouseOutOfRange = 0;
    private int wrongDraws = 0;

    private LensDistortion lD;

    private string aktStepId = "";

    private Vector2 mouseTargetRange;
    private Vector3 drawMouseOffset;
    private Vector3 winterStartPos;
    private Vector3 winterEndPos;
    private Vector3 summerStartPos;
    private Vector3 summerEndPos;

#endregion

    void Start() {
        LensDistortion tmp;

        if (volume.profile.TryGet(out tmp)) {
            lD = tmp;
        }

        ProcedureController.changeEvent += DoActionWhileStepUpdate;
    }

    public void DoActionWhileStepUpdate(string stepId) {
        aktStepId = stepId;
        switch (stepId) {
            #region Initialer Step /Checkpoint
            case "S02.00a":
            case "S02.00":
                ResetScript();
                mainCamera.SetActive(true);
                skyCamera.SetActive(true);
                sPEH.AllowStopTimelineReset();
                skyTimeController.StopSetTimeLineWithAnim();
                skyTimeController.SetTimeline(12.4f);
                skyTimeController.animTime = 12.4f;
                aEHS02.ResetScript();
                cc.SetTimeModeS02();
                sPEH.SetAllowUpdateLightTrue();

                CalculateShadowPosition cSP =
                    (CalculateShadowPosition) shadowCalcParent.GetComponent(typeof(CalculateShadowPosition));
                cSP.SetAllowPrintingPos();

                Color schattenwerferHighlightColor = schattenwerferHighlight.color;
                schattenwerferHighlightColor.a = 1;
                schattenwerferHighlight.color = schattenwerferHighlightColor;

                sunDialCamAnimator.enabled = true;
                sunDialCamAnimator.Play("PulseSphere_S02", 0, 0);
                sunDialCamAnimator.SetInteger("MoveSunDialCam", 9);

                pC.JumpToNextStep();
                break;
#endregion
            
            case "S02.02":
                //Kalender mit Monatsnamen einblenden
                tIC.ShowHorizontalLongMonths();
                cSINC.JumpToStartPos();
                cSINC.UpdateFontManually(2, -1);
                
                skyTimeController.SetAllowSetTimeLineWithAnim();
                skyAnimator.SetInteger("StartAnimateTimeline", 5);
                skyAnimator.speed = 0f;

                winterLineTR.positionCount = 2;
                winterLineTRUhr.positionCount = 2;
                summerLineTR.positionCount = 2;
                summerLineTRUhr.positionCount = 2;

                startTime = Time.time;
                winterStartPos = sunMarkerTransf.position;
                winterEndPos = calcedShadowPoint.position;
                winterLineTR.enabled = true;
                winterLineTRUhr.enabled = true;
                winterLineTR.SetPosition(0, winterStartPos);
                winterLineTR.SetPosition(1, winterStartPos);
                winterLineTRUhr.SetPosition(0, schattenwerferKugel.position);
                winterLineTRUhr.SetPosition(1, schattenwerferKugel.position);
                drawWinterLine = true;
                StartCoroutine(SafeJump());
                break;

            case "S02.02b":
                cSINC.SetTargetScrollPosMonthNamesCalendar(monthScrollTargets[6]);
                cSINC.UpdateFontManually(8, 2);
                skyTimeController.SetDate(2021, 6, 21);

                StartCoroutine(WaitBeforDrawSummerLine());
                break;

            case "S02.04":
                sPEH.SetStopTimelineResetFalse();
                cc.SetRealTime();
                turnSunOff = true;
                skyAnimator.enabled = true;
                skyAnimator.speed = 1f;

                //movecamtotopinspheres02 wird in skyAnimator mit event gestartet
                skyAnimator.SetInteger("StartAnimateTimeline", 5);
                sunDialCamAnimator.Play("SunDialFullViewFrom0", 0, 0);
                sunDialCamAnimator.speed = 0;
                sunDialCamAnimator.SetInteger("MoveSunDialCam", 4);
                break;

        #region Checkpoint

            case "S02.06C":
                ResetScript();
                cameraAnimator.Rebind();
                aEHS02.StopMoveWithAnimS02AEH();
                cc.SetRealTime();
                pC.JumpToNextStep();
                sPEH.SetStopTimelineResetFalse();
                skyAnimator.enabled = true;
                skyAnimator.speed = 1f;
                skyAnimator.Play("AnimateTimelineFrom12", 0, 0);
                skyAnimator.SetInteger("StartAnimateTimeline", 5);
                skyTimeController.SetAllowSetTimeLineWithAnim();
                break;

        #endregion

            case "S02.06":
                cPC.SetCheckPointColors("S02", 2);
                cPC.SetCheckPointReached("S02", 02);
                skyTimeController.StopIncreaseDayWitAnim();

                //show ortho Cam in fullscreen
                sunDialCameraObject.SetActive(true);
                sunDialCamAnimator.speed = 1;
                sunDialCamAnimator.Play("SunDialFullViewFrom0", 0, 0);
                sunDialCamAnimator.SetInteger("MoveSunDialCam", 4);

                break;

            case "S02.07":
                checkStartTraceDailyLine = true;
                checkRangeInS0207 = true;
                Color calendarLinesColor = calendarLinesMat.color;
                calendarLinesColor.a = 0;
                calendarLinesMat.color = calendarLinesColor;
                cSINC.SetTargetScrollPosMonthNamesCalendar(monthScrollTargets[System.DateTime.Now.Month]);
                cSINC.UpdateFontManually(System.DateTime.Now.Month + 2, 8);
                mainCamObject.SetActive(false);
                skyCamObject.SetActive(false);
                break;

            case "S02.08":
                checkMousePosInS0208 = true;
                ShowSplitScreen sss = (ShowSplitScreen) sunDialParent.GetComponent(typeof(ShowSplitScreen));
                mouseTargetRange = sss.GetYRange();
                break;

            case "S02.10":
            case "S02.11":
                lilaHelpPoint.SetActive(false);
                break;

            case "S02.12":
                drawWitMouseTR.Clear();
                calcedShadowPointTR.time = 0;
                cSINC.SetTargetScrollPosMonthNamesCalendar(monthScrollTargets[6]);
                cSINC.UpdateFontManually(8, 2);
                skyTimeController.SetDate(2021, 6, 21);
                skyAnimator.speed = 1.1f;
                StartCoroutine(WaitAndJumpToNextStep());
                break;

            case "S02.13":
                calcedShadowPointTR.enabled = true;
                calcedShadowPointTR.time = 0;
                traceafter24 = true;
                break;

            case "S02.15":
                checkTimeAndStopIfDay = true;
                kalender.SetActive(true);
                break;

            case "S02.17":
                turnBlacKalenderOff = true;
                kalenderlinienKrebs.SetActive(true);

                krebsRahmen.SetActive(true);
                krebsLupe.transform.localScale = new Vector3(1, 1, 1);
                turnKrebsRahmenOn = true;
                pC.JumpToNextStep();
                break;

            case "S02.19":
                turnKrebsRahmenOff = true;
                skyAnimator.speed = 1;
                pC.JumpToNextStep();
                break;

            case "S02.21":
                skyAnimator.speed = 1;
                pC.JumpToNextStep();
                break;

            case "S02.24":
                skyAnimator.speed = 0;

                skyTimeController.AllowIncreaseDayWithAnim();
                break;

            case "S02.25":
                setDayAt24 = true;
                skyAnimator.speed = 1;
                break;

            case "S02.28":
                cSINC.SetTargetScrollPosMonthNamesCalendar(monthScrollTargets[12]);
                cSINC.UpdateFontManually(14, 11);
                skyTimeController.SetDate(2021, 12, 22);
                skyAnimator.speed = 1;
                break;

            case "S02.31":
                sPEH.StopSunDialTREmitting();

                ssL.FinishedPathPoint("S02");
                pC.JumpToNextStep();
                break;

            case "S02.33":
                mainCamObject.SetActive(true);
                jumpToNextDates = false;
                calcedShadowPointTR.time = 0;
                calcedShadowPointTR.enabled = false;
                pC.JumpToNextStep();
                break;
        }
    }

    private void Update() {
        //Prüfen ob die Range berechnet wurde
        if (checkRangeInS0207) {
            if (skyTimeController.GetAnimatedDaysCount() == 2) {
                ShowSplitScreen sss = (ShowSplitScreen) sunDialParent.GetComponent(typeof(ShowSplitScreen));
                mouseTargetRange = sss.GetYRange();
                if (mouseTargetRange.x == 0 || mouseTargetRange.x >= 50000 || mouseTargetRange.y > 5000 ||
                    mouseTargetRange.y == 0) {
                    sss.AllowCalculateMinMax();
                } else {
                    pC.JumpToNextStep();
                    checkRangeInS0207 = false;
                }
            }
        }

        //Prüfen ob die gezeichnete Linie in Range liegt
        if (checkMousePosInS0208) {
            if (Input.GetMouseButtonDown(0)) {
                drawWitMouseTR.emitting = false;
                firstClickWhenDraw = true;
                mouseZCoordinate = sunDialCamera.WorldToScreenPoint(drawWithFingerObject.position).z;
                drawMouseOffset = drawWithFingerObject.position - GetMouseWorldPos();
                drawWithFingerObject.position = GetMouseWorldPos();

                countMouseOutOfRange = 0;
                deltaMousePos = Input.mousePosition.x;
            }

            if (Input.GetMouseButton(0)) {
                drawWithFingerObject.position = GetMouseWorldPos();
                if (firstClickWhenDraw) {
                    drawWitMouseTR.Clear();
                    drawWitMouseTR.time = 0;
                    firstClickWhenDraw = false;
                } else {
                    drawWitMouseTR.emitting = true;
                    drawWitMouseTR.time = 5000;
                }

                if (Input.mousePosition.x > 100 && Input.mousePosition.x < Screen.width - 100) {
                    if (Input.mousePosition.y > mouseTargetRange.y + 30 ||
                        Input.mousePosition.y < mouseTargetRange.x - 30) {
                        countMouseOutOfRange++;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0)) {
                float length = deltaMousePos - Input.mousePosition.x;
                if (length > Screen.width / 2) {
                    if (countMouseOutOfRange > 0) {
                        drawWitMouseTR.Clear();
                        wrongDraws++;
                        pC.JumpWithQuestInput(0);
                        lilaHelpPoint.SetActive(true);
                        if (wrongDraws > 2) {
                            Vector3 aktDate = skyTimeController.GetDate();
                            if (aktDate.y == 9 && aktDate.z > 18 && aktDate.z < 28) {
                                pC.JumpWithQuestInput(1);
                            } else if (aktDate.y == 6 && aktDate.z > 19 && aktDate.z < 24) {
                                pC.JumpWithQuestInput(3);
                            } else {
                                pC.JumpWithQuestInput(2);
                            }

                            Color calendarLinesColor = calendarLinesMat.color;
                            calendarLinesColor.a = 1;
                            calendarLinesMat.color = calendarLinesColor;
                            checkMousePosInS0208 = false;
                        }
                    } else {
                        Vector3 aktDate = skyTimeController.GetDate();
                        if (aktDate.y == 9 && aktDate.z > 18 && aktDate.z < 28) {
                            pC.JumpWithQuestInput(1);
                        } else if (aktDate.y == 6 && aktDate.z > 19 && aktDate.z < 24) {
                            pC.JumpWithQuestInput(3);
                        } else {
                            pC.JumpWithQuestInput(2);
                        }

                        //drawWitMouseTR.Clear();
                        Color calendarLinesColor = calendarLinesMat.color;
                        calendarLinesColor.a = 1;
                        calendarLinesMat.color = calendarLinesColor;
                        checkMousePosInS0208 = false;
                    }
                }

                drawWitMouseTR.emitting = false;
            }
        }

        //Wenn Uhrzeit zwischen 10 und 16 UHr dann Animation anhalten,um den Kalender besser erkennen zu können
        if (checkTimeAndStopIfDay) {
            if (skyTimeController.GetAnimTime() > 34 || skyTimeController.GetAnimTime() < 16) {
                skyAnimator.speed = 0;
                turnSunDialCalendarOn = true;
                checkTimeAndStopIfDay = false;
            }
        }

        //zu nächsten Datum springen wenn Ziel-Datum erreicht
        if (jumpToNextDates) {
            Vector3 aktDate = skyTimeController.GetDate();

            if (aktDate.y == 7 && aktDate.z == 22) {
                cSINC.SetTargetScrollPosMonthNamesCalendar(monthScrollTargets[8]);
                cSINC.UpdateFontManually(10, 9);
                skyTimeController.SetDate(2021, 8, 21);
            }

            if (aktDate.y == 8 && aktDate.z == 22) {
                cSINC.SetTargetScrollPosMonthNamesCalendar(monthScrollTargets[9]);
                cSINC.UpdateFontManually(11, 10);
                skyTimeController.SetDate(2021, 9, 22);
                pC.JumpToNextStep();
            }

            if (aktDate.y == 9 && aktDate.z == 23) {
                okButton.interactable = true;
                skyAnimator.speed = 0;
            }

            if (aktDate.y == 12 && aktDate.z == 23) {
                cSINC.SetTargetScrollPosMonthNamesCalendar(monthScrollTargets[12]);
                cSINC.UpdateFontManually(14, 11);
                skyTimeController.SetDate(2021, 12, 22);
                skyAnimator.speed = 0;
                okButton.interactable = true;
                jumpToNextDates = false;
            }

            if (aktDate.y == 1 && aktDate.z == 23) {
                cSINC.SetTargetScrollPosMonthNamesCalendar(monthScrollTargets[2]);
                cSINC.UpdateFontManually(4, 14);
            }

            if (aktDate.y == 2 && aktDate.z == 22) {
                cSINC.SetTargetScrollPosMonthNamesCalendar(monthScrollTargets[3]);
                cSINC.UpdateFontManually(5, 4);
                skyTimeController.SetDate(2021, 1, 22);
            }

            if (aktDate.y == 3 && aktDate.z == 23) {
                cSINC.SetTargetScrollPosMonthNamesCalendar(monthScrollTargets[4]);
                cSINC.UpdateFontManually(6, 5);
                skyTimeController.SetDate(2021, 2, 21);
            }

            if (aktDate.y == 4 && aktDate.z == 22) {
                cSINC.SetTargetScrollPosMonthNamesCalendar(monthScrollTargets[5]);
                cSINC.UpdateFontManually(7, 6);
                skyTimeController.SetDate(2021, 5, 22);
            }

            if (aktDate.y == 5 && aktDate.z == 23) {
                cSINC.SetTargetScrollPosMonthNamesCalendar(monthScrollTargets[6]);
                cSINC.UpdateFontManually(8, 7);
                skyTimeController.SetDate(2021, 6, 22);
                pC.JumpToNextStep();
                skyAnimator.speed = 0;
                jumpToNextDates = false;
            }
        }

        if (traceafter24) {
            float aktTime = skyTimeController.GetAnimTime();
            if (aktTime > 25 && aktTime < 30) {
                calcedShadowPointTR.time = 50000;
                calcedShadowPointTR.emitting = true;
                StartCoroutine(WaitAndJumpWithFlexTime());
                traceafter24 = false;
            }
        }

        if (setDayAt24) {
            float aktTime = skyTimeController.GetAnimTime();
            if (aktTime > 24 && aktTime < 30) {
                cSINC.SetTargetScrollPosMonthNamesCalendar(monthScrollTargets[7]);
                cSINC.UpdateFontManually(9, 8);
                skyTimeController.SetDate(2021, 7, 21);
                jumpToNextDates = true;
                setDayAt24 = false;
            }
        }

        if (checkStartTraceDailyLine) {
            float aktTime = skyTimeController.GetAnimTime();

            if (skyTimeController.GetAnimatedDaysCount() == 1 && aktTime >= 31f) {
                calcedShadowPoint.gameObject.SetActive(true);
                calcedShadowPointTR.Clear();
                calcedShadowPointTR.enabled = true;
                calcedShadowPointTR.time = 50000;
                calcedShadowPointTR.emitting = true;
                checkStopTraceDailyLine = true;
                checkStartTraceDailyLine = false;
            }
        }

        if (checkStopTraceDailyLine) {
            float aktTime = skyTimeController.GetAnimTime();
            if (skyTimeController.GetAnimatedDaysCount() == 1 && aktTime >= 20 && aktTime < 21) {
                calcedShadowPointTR.emitting = false;
                checkStopTraceDailyLine = false;
            }
        }

    #region graphical Actions

        if (drawWinterLine) {
            Vector3 pos = winterStartPos;
            float t = (Time.time - startTime) / 3.0f;
            pos = Vector3.Lerp(winterStartPos, schattenwerferKugel.position, t);
            winterLineTR.SetPosition(1, pos);
            if (pos == schattenwerferKugel.position) {
                startTime = Time.time;
                drawWinterLineUhr = true;
                drawWinterLine = false;
            }
        }

        if (drawWinterLineUhr) {
            Vector3 pos = schattenwerferKugel.position;
            float t = (Time.time - startTime) / 0.5f;
            pos = Vector3.Lerp(schattenwerferKugel.position, winterEndPos, t);
            winterLineTRUhr.SetPosition(1, pos);
            if (pos == winterEndPos) {
                if (!jumpIn02Done) {
                    jumpIn02Done = true;
                    pC = (ProcedureController) gameController.GetComponent(typeof(ProcedureController));
                    pC.JumpToNextStep();
                }

                showWintertext = true;
                drawWinterLineUhr = false;
            }
        }

        if (drawSummerLine) {
            Vector3 pos = summerStartPos;
            float t = (Time.time - startTime) / 3.0f;
            pos = Vector3.Lerp(summerStartPos, schattenwerferKugel.position, t);
            summerLineTR.SetPosition(1, pos);
            if (pos.z == schattenwerferKugel.position.z) {
                drawSummerLineUhr = true;
                startTime = Time.time;
                drawSummerLine = false;
            }
        }

        if (drawSummerLineUhr) {
            Vector3 pos = schattenwerferKugel.position;
            float t = (Time.time - startTime) / 0.5f;
            pos = Vector3.Lerp(schattenwerferKugel.position, summerEndPos, t);
            summerLineTRUhr.SetPosition(1, pos);
            if (pos == summerEndPos) {
                pC = (ProcedureController) gameController.GetComponent(typeof(ProcedureController));
                pC.JumpToNextStep();
                showSummerText = true;
                drawSummerLineUhr = false;
            }
        }

        if (showSummerText) {
            if (summerText.faceColor.a < 230) {
                int tempAlpha = summerText.faceColor.a + 50;
                summerText.faceColor = new Color32(255, 207, 64, (byte) tempAlpha);
            } else {
                summerText.faceColor = new Color32(255, 207, 64, 255);
                showSummerText = false;
            }
        }

        if (showWintertext) {
            if (winterText.faceColor.a < 230) {
                int tempAlpha = winterText.faceColor.a + 50;
                winterText.faceColor = new Color32(255, 207, 64, (byte) tempAlpha);
            } else {
                winterText.faceColor = new Color32(255, 207, 64, 255);
                showWintertext = false;
            }
        }

        if (makeWinterLineTransparent) {
            Color sunLineWinterColor = sunLineWinter.color;
            Color sunLineWinterUhrColor = sunLineWinterUhr.color;
            if (sunLineWinterColor.a > 0.5f) {
                int tempAlpha = winterText.faceColor.a - 20;
                winterText.faceColor = new Color32(255, 207, 64, (byte) tempAlpha);
                sunLineWinterColor.a -= 0.075f;
                sunLineWinterUhrColor.a -= 0.03f;
                sunLineWinter.color = sunLineWinterColor;
                sunLineWinterUhr.color = sunLineWinterUhrColor;
            } else {
                winterText.faceColor = new Color32(255, 207, 64, 150);
                sunLineWinterColor.a = 0.5f;
                sunLineWinter.color = sunLineWinterColor;
                sunLineWinterUhrColor.a = 0.75f;
                sunLineWinterUhr.color = sunLineWinterUhrColor;
                makeWinterLineTransparent = false;
            }
        }

        if (turnKrebsRahmenOn) {
            Color krebsRahmenSpriteColor = krebsRahmenSprite.color;
            Color krebsLupeSpriteColor = krebsLupeSprite.color;
            if (krebsLupeSpriteColor.a < 1) {
                krebsLupeSpriteColor.a += 0.075f;
                krebsLupeSprite.color = krebsLupeSpriteColor;
                krebsRahmenSprite.color = krebsLupeSpriteColor;
            } else {
                turnKrebsRahmenOn = false;
            }
        }

        if (turnKrebsRahmenOff) {
            Color krebsRahmenSpriteColor = krebsRahmenSprite.color;
            Color krebsLupeSpriteColor = krebsLupeSprite.color;
            if (krebsLupeSpriteColor.a > 0) {
                krebsLupeSpriteColor.a -= 0.075f;
                krebsLupeSprite.color = krebsLupeSpriteColor;
                krebsRahmenSprite.color = krebsLupeSpriteColor;
            } else {
                krebsLupe.localScale = new Vector3(0, 0, 0);
                krebsRahmen.SetActive(false);
                turnKrebsRahmenOff = false;
            }
        }

        if (turnSunOn) {
            Color sunColor = sunMat.color;
            if (sunColor.a < 1) {
                sunColor.a += 0.03f;
                sunMat.color = sunColor;
            } else {
                turnSunOn = false;
            }
        }

        if (turnSunOff) {
            Color sunMarkerColor = sunMarker.color;
            if (sunMarkerColor.a > 0) {
                sunMarkerColor.a -= 0.07f;
                sunMarker.color = sunMarkerColor;
            }

            Color sunLineSommerColor = sunLineSommer.color;
            Color sunLineSommerUhrColor = sunLineSommerUhr.color;
            Color sunLineWinterColor = sunLineWinter.color;
            Color sunLineWinterUhrColor = sunLineWinterUhr.color;
            if (sunLineSommerColor.a > 0) {
                sunLineSommerColor.a -= 0.1f;
                sunLineSommer.color = sunLineSommerColor;
                sunLineWinterColor.a -= 0.075f;
                sunLineWinter.color = sunLineWinterColor;
                sunLineSommerUhrColor.a -= 0.1f;
                sunLineSommerUhr.color = sunLineSommerUhrColor;
                sunLineWinterUhrColor.a -= 0.1f;
                sunLineWinterUhr.color = sunLineWinterUhrColor;
            } else {
                sunLineSommerColor.a = 0f;
                sunLineSommer.color = sunLineSommerColor;
                sunLineWinterColor.a = 0f;
                sunLineWinter.color = sunLineWinterColor;
                sunLineSommerUhrColor.a = 0f;
                sunLineSommerUhr.color = sunLineSommerUhrColor;
                sunLineWinterUhrColor.a = 0f;
                sunLineWinterUhr.color = sunLineWinterUhrColor;
                summerLineTR.enabled = false;
                summerLineTRUhr.enabled = false;
                winterLineTR.enabled = false;
                winterLineTRUhr.enabled = false;
                summerText.faceColor = new Color32(255, 207, 64, 0);
                winterText.faceColor = new Color32(255, 207, 64, 0);
                turnSunOff = false;
            }

            if (summerText.faceColor.a > 30) {
                int tempAlphaWinter = summerText.faceColor.a - 30;
                summerText.faceColor = new Color32(255, 207, 64, (byte) tempAlphaWinter);
            } else {
                summerText.faceColor = new Color32(255, 207, 64, 0);
            }

            if (winterText.faceColor.a > 25) {
                int tempAlpha = winterText.faceColor.a - 25;
                winterText.faceColor = new Color32(255, 207, 64, (byte) tempAlpha);
            } else {
                winterText.faceColor = new Color32(255, 207, 64, 0);
            }
        }

        if (turnSunDialCalendarOn) {
            Color kalenderSideColor = kalenderSide.color;
            Color kalenderSternzeichenBlackColor = kalenderSternzeichenBlack.color;
            if (kalenderSideColor.a < 1) {
                kalenderSideColor.a += 0.1f;
                kalenderSide.color = kalenderSideColor;
                kalenderSternzeichenBlackColor.a += 0.05f;
                kalenderSternzeichenBlack.color = kalenderSternzeichenBlackColor;
            } else {
                kalenderSternzeichenBlackColor.a = 0.5f;
                kalenderSternzeichenBlack.color = kalenderSternzeichenBlackColor;
                pC.JumpToNextStep();
                turnSunDialCalendarOn = false;
            }
        }

        if (turnBlacKalenderOff) {
            Color kalenderSternzeichenBlackColor = kalenderSternzeichenBlack.color;
            if (kalenderSternzeichenBlackColor.a > 0) {
                kalenderSternzeichenBlackColor.a -= 0.075f;
                kalenderSternzeichenBlack.color = kalenderSternzeichenBlackColor;
            } else {
                kalenderSternzeichenBlackColor.a = 0f;
                kalenderSternzeichenBlack.color = kalenderSternzeichenBlackColor;
                turnBlacKalenderOff = false;
            }
        }

    #endregion
    }

    private void ResetScript() {
    #region Reset private Variables

        jumpIn02Done = false;
        drawWinterLine = false;
        drawSummerLine = false;
        drawSummerLineUhr = false;
        drawWinterLineUhr = false;
        wrongDraws = 0;
        turnSunOn = false;
        turnSunOff = false;
        turnSunDialCalendarOn = false;
        jumpToNextDates = false;
        setDayAt24 = false;
        traceafter24 = false;
        checkRangeInS0207 = false;
        checkMousePosInS0208 = false;
        countMouseOutOfRange = 0;
        checkStartTraceDailyLine = false;
        checkStopTraceDailyLine = false;
        deltaMousePos = 0.0f;
        turnBlacKalenderOff = false;
        checkTimeAndStopIfDay = false;

    #endregion

    #region Reset Camera

        ShowSplitScreen sSS = (ShowSplitScreen) sunDialParent.GetComponent(typeof(ShowSplitScreen));
        sSS.Reset();
        cameraAnimator.Rebind();
        aEHS02.AllowMoveWithAnimS02AEH();
        lD.intensity.Override(0.01f);
        lD.scale.Override(1.04f);

        skyCamTransform.localPosition = new Vector3(0.3f, 41.2f, 0.9f);
        mainCamTransform.localEulerAngles = new Vector3(20, 0, 0);
        cameraCenterTransform.localEulerAngles = new Vector3(0, -92, 0);
        mainCamObject.transform.localEulerAngles = new Vector3(0, 0, 0);

        float resolutionFactor = DeviceInfo.GetResolutionFactor();
        float newFOV = 40 + (((resolutionFactor - 0.45f) * 20.0f) / 0.3f);
        mainCam.fieldOfView = newFOV;
        skyCam.fieldOfView = newFOV;

    #endregion

    #region Reset Date and Time

        skyTimeController.SetLatitude(48);
        skyTimeController.ResetAnimatedDays();
        skyTimeController.SetDate(2021, 12, 21);
        skyTimeController.SetTimeline(12.4f);
        skyTimeController.animTime = 12.4f;
        dateSliderParent.SetActive(true);

    #endregion

    #region Reset Materials

        Color kalenderSideColor = kalenderSide.color;
        kalenderSideColor.a = 0f;
        kalenderSide.color = kalenderSideColor;

        Color kalenderLinieSRColor = kalenderLinieSR.color;
        kalenderLinieSRColor.a = 1;
        kalenderLinieSR.color = kalenderLinieSRColor;

        groundPlane.material = groundMat;

        Color krebsRahmenSpriteColor = krebsRahmenSprite.color;
        Color krebsLupeSpriteColor = krebsLupeSprite.color;
        krebsLupeSpriteColor.a = 0;
        krebsLupeSprite.color = krebsLupeSpriteColor;
        krebsRahmenSprite.color = krebsLupeSpriteColor;

        Color sunLineSommerColor = sunLineSommer.color;
        sunLineSommerColor.a = 1;
        sunLineSommer.color = sunLineSommerColor;

        Color sunLineSommerUhrColor = sunLineSommerUhr.color;
        sunLineSommerUhrColor.a = 1;
        sunLineSommerUhr.color = sunLineSommerUhrColor;

        Color sunLineWinterColor = sunLineWinter.color;
        sunLineWinterColor.a = 1;
        sunLineWinter.color = sunLineWinterColor;

        Color sunLineWinterUhrColor = sunLineWinterUhr.color;
        sunLineWinterUhrColor.a = 1;
        sunLineWinterUhr.color = sunLineWinterUhrColor;

        summerText.faceColor = new Color32(255, 207, 64, 0);
        winterText.faceColor = new Color32(255, 207, 64, 0);

        Color kalenderSternzeichenBlackColor = kalenderSternzeichenBlack.color;
        kalenderSternzeichenBlackColor.a = 0;
        kalenderSternzeichenBlack.color = kalenderSternzeichenBlackColor;

    #endregion

    #region Reset Other

        rotStarrySky.AllowCalculateRotationContinous();
        kalender.SetActive(false);
        kalenderlinien.SetActive(true);
        summerTextGO.SetActive(true);
        winterTextGO.SetActive(true);

        drawWitMouseTR.Clear();
        drawWitMouseTR.time = 0;

        calcedShadowPointTR.Clear();
        calcedShadowPointTR.time = 0;

        sRC.enabled = true;
        moonLight.SetActive(false);

        skyAnimator.Rebind();
        skyAnimator.Update(0f);
        skyAnimator.enabled = false;

        sunDialParent.SetActive(true);
        sunDialObject.SetActive(true);

        aEHS02.ResetScript();

        sunDialParent.transform.position = new Vector3(-102, -7.2f, -185f);
        sunDialObject.transform.localPosition = new Vector3(0, -1.1f, 235.6f);
        sunDialObject.transform.localEulerAngles = new Vector3(0, 180, 0);
        sunDirectionalLight.position = new Vector3(-102, 7.55f, 5.1f);

        gridSvg.localPosition = new Vector3(0.021f, 0.0f, 0);

        summerLineTR.positionCount = 0;
        summerLineTRUhr.positionCount = 0;
        winterLineTR.positionCount = 0;
        winterLineTRUhr.positionCount = 0;
        calcedShadowPoint.gameObject.SetActive(false);

        lilaHelpPoint.SetActive(false);

    #endregion
    }
    
    private Vector3 GetMouseWorldPos() {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mouseZCoordinate;
        return sunDialCamera.ScreenToWorldPoint(mousePoint);
    }
    
    private IEnumerator SafeJump() {
        yield return new WaitForSeconds(4f);
        if (aktStepId == "S02.02" && !jumpIn02Done) {
            drawWinterLine = false;
            jumpIn02Done = true;
            pC.JumpToNextStep();
        }
    }

    private IEnumerator WaitBeforDrawSummerLine() {
        yield return new WaitForSeconds(0.5f);
        makeWinterLineTransparent = true;
        summerLineTR.enabled = true;
        summerLineTR.SetPosition(0, sunMarkerTransf.position);
        summerLineTR.SetPosition(1, sunMarkerTransf.position);
        summerLineTRUhr.enabled = true;
        summerLineTRUhr.SetPosition(0, schattenwerferKugel.position);
        summerLineTRUhr.SetPosition(1, schattenwerferKugel.position);

        summerStartPos = sunMarkerTransf.position;
        summerEndPos = calcedShadowPoint.position;
        startTime = Time.time;
        drawSummerLine = true;
    }

    private IEnumerator WaitAndJumpToNextStep() {
        yield return new WaitForSeconds(2f);
        pC.JumpToNextStep();
    }

    private IEnumerator WaitAndJumpWithFlexTime() {
        yield return new WaitForSeconds(5);
        pC.JumpToNextStep();
    }

    private IEnumerator AnimateWinterLines() {
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator AnimateSummerLines() {
        float startTime = Time.time;

        Vector3 summerStartPos = sunMarkerTransf.position;
        Vector3 summerEndPos = calcedShadowPoint.position;

        Vector3 pos = summerStartPos;
        while (pos != summerEndPos) {
            float t = (Time.time - startTime) / 5.0f;
            pos = Vector3.Lerp(summerStartPos, summerEndPos, t);
            summerLineTR.SetPosition(1, pos);
        }

        yield return new WaitForSeconds(0.1f);
        pC.JumpToNextStep();
    }
}