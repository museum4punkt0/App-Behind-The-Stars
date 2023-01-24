using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class S03NEWHelper : MonoBehaviour {
#region public Variables

    public AnimationEventHandlerCameraS03 aEHS03;
    public ClockController cc;
    public ContentScrollNonInteractableCalendar cSINC;
    public HelpController hC;
    public RotateStarrSky rotStarrySky;
    public ProcedureController pC;
    public SpielstandLoader ssL;
    public SkyTimeController skyTimeController;
    public SkyRenderController sRC;
    public TimeInputController tIC;
    public TrailControllerHorizontS03 s03_March;
    public TrailControllerHorizontS03 s03_MarchBlue;
    public TrailControllerHorizontS03 s03_Winter;
    public TrailControllerHorizontS03 s03_WinterBlue;
    public TrailControllerHorizontS03 s03_Summer;
    public TrailControllerHorizontS03 s03_SummerBlue;
    public TrailControllerHorizontS03 s03_Sept;
    public TrailControllerHorizontS03 s03_SeptBlue;

    public Animator cameraAnimator;
    public Animator sunDialAnimator;
    public Animator skySunAnimator;

    public Button okButton;

    public Camera mainCam;
    public Camera skyCam;

    public CanvasGroup blackBlend;
    public CanvasGroup inputS0310;

    public GameObject gameController;
    public GameObject sunDialCameraObject;
    public GameObject sunDialObject;
    public GameObject sunDialParent;
    public GameObject moonLight;
    public GameObject skyObject;
    public GameObject trailObjectHorizontHighlight;
    public GameObject ekliptikPointParent;
    public GameObject northForOutofSphereGo;
    public GameObject rightInputPrefab;
    public GameObject wrongInputPrefab;
    public GameObject canvasParent;
    public GameObject grosserWagenN05;

    public Material breitengradLinien;
    public Material matStarUnlit;
    public Material importantStars;
    public Material ekliptikBogen;
    public Material ekliptikBogenHinten;
    public Material ekliptikMarker;
    public Material earthhorizontCircle;
    public Material earthMaterial;
    public Material gitterNetzMat;
    public Material planeGround;
    public Material sunLineWinterMat;
    public Material sunLineMarchMat;
    public Material sunLineSummerMat;
    public Material sunLineWinterMatBlue;
    public Material sunLineMarchMatBlue;
    public Material sunLineSummerMatBlue;
    public Material sunLineSeptMat;
    public Material sunLineSeptMatBlue;
    public Material sunLineHighlightOverHorizont;
    public Material sunTraceLineConstant;
    public Material sunMaterial;
    public Material ekliptikPointTransparent;
    public Material ekliptikPointTransparentBehind;
    public Material sunDialWoodMatOpaque;
    public Material sunDialGroundPlateMatOpaque;
    public Material gitternetzGrade;
    public Material horizontLila;

    public MeshRenderer planeGroundMR;
    public MeshRenderer sunDialWood;

    public SpriteRenderer figureEarth;
    public SpriteRenderer gridHorizont;
    public SpriteRenderer gridGround;
    public SpriteRenderer sunDialGroundPlate;

    public TextMeshPro north;
    public TextMeshPro east;
    public TextMeshPro south;
    public TextMeshPro west;
    public TextMeshProUGUI dateText;

    public TrailRenderer s03_1Winter;
    public TrailRenderer s03_2March;
    public TrailRenderer s03_3Summer;
    public TrailRenderer s03_4Sept;
    public TrailRenderer s03_1WinterBlue;
    public TrailRenderer s03_2MarchBlue;
    public TrailRenderer s03_3SummerBlue;
    public TrailRenderer ekliptikTrace;
    public TrailRenderer sunTraceS03;
    public TrailRenderer sunTraceS03OnSkySphere;

    public Transform cameraCenterTransform;
    public Transform inputS0310Transf;
    public Transform mainCamTransform;
    public Transform mainCamParent;
    public Transform skyCamCenterTransf;
    public Transform skyCameraTransf;
    public Transform rotateSkarrySkyPivot;
    public Transform southTransf;
    public Transform skyParentTransf;
    public Transform starrySkyCompassPivot;
    public Transform groundTransf;
    public Transform earthTransform;
    public Transform earthFiure;
    public Transform gridGroundTransf;
    public Transform groundDirections;
    public Transform horizontLilaTransf;
    public Transform sunObjectsParent;
    public Transform outOfSphereBackground;

    public UniversalRenderPipelineAsset uRPA;

    public Volume volume;

#endregion

#region private Variables

    private bool checkIf16Reached = false;
    private bool checkIf17Reached = false;
    private bool checkIf19Reached = false;
    private bool turnBlackBlendOn = false;
    private bool turnBlackBlendOff = false;
    private bool makeS03_1Transparent = false;
    private bool makeS03_2Transparent = false;
    private bool makeS03_3Transparent = false;
    private bool turnImportantStarsOn = false;
    private bool flashHighlightGrosserWagen = false;
    private bool highlightGrosserWagenOn = false;
    private bool highlightGrosserWagenOff = false;
    private bool flashHighlightEkliptik = false;
    private bool highlightEkliptikOn = false;
    private bool highlightEkliptikOff = false;
    private bool flashHighlightMarchLine = false;
    private bool highlightMarchLineOn = false;
    private bool highlightMarchLineOff = false;
    private bool flashHighlightWinterLine = false;
    private bool highlightWinterLineOn = false;
    private bool highlightWinterLineOff = false;
    private bool flashHighlightSummerLine = false;
    private bool highlightSummerLineOff = false;
    private bool highlightSummerLineOn = false;
    private bool turnMarchLineOff = false;
    private bool turnSummerLineOff = false;
    private bool turnWinterLineOff = false;
    private bool turnMarchLineFullOff = false;
    private bool turnSummerLineFullOff = false;
    private bool turnWinterLineFullOff = false;
    private bool turnEkliptikBogenOn = false;
    private bool turnEkliptikBogenOff = false;
    private bool turnGrosserWagenOn = false;
    private bool turnSunOn = false;
    private bool turnDailyLineJulyOff = true;
    private bool turnDailyLineJulyOn = true;
    private bool changeEkliptikPointsColorTrans = false;
    private bool changeEkliptikPointsColorFull = false;

    private float resolutionFactor = 1.0f;

    private LensDistortion lD;

    private Color ekliptikPointFull = new Color(1, 0.2039f, 0.4039f, 1);
    private Color ekliptikPointTransparentColor = new Color(0.63137f, 0.3137f, 0.39215f, 1);
    private Color lerpedColor = new Color(0, 0, 0, 1);
    private Coroutine highlightTageskreise = null;

#endregion

    void Start() {
        LensDistortion tmp;

        if (volume.profile.TryGet(out tmp)) {
            lD = tmp;
        }

        ProcedureController.changeEvent += DoActionWhileStepUpdate;
    }

    public void DoActionWhileStepUpdate(string stepId) {

        switch (stepId) {
            case "S03NEW.00":
            case "S03NEW.00a":
                ResetScript();
                pC.JumpToNextStep();
                tIC.ShowHorizontalLongMonths();
                skyParentTransf.localEulerAngles = new Vector3(-5, 0, 0);
                cSINC.SetTargetScrollPosMonthNamesCalendar(0.5737f);
                cSINC.UpdateFontManually(11, -1);
                break;

            case "S03NEW.04":
                skyTimeController.StopIncreaseDayWitAnim();
                skySunAnimator.speed = 1;
                break;

            case "S03NEW.05a":
                s03_March.StopCheckAndDraw();
                s03_MarchBlue.StopCheckAndDraw();
                s03_2MarchBlue.Clear();
                s03_2MarchBlue.time = 0;
                turnMarchLineFullOff = true;
                skyTimeController.SetDate(System.DateTime.Now.Year, 12, 21);
                cSINC.SetTargetScrollPosMonthNamesCalendar(0.0018f);
                cSINC.UpdateFontManually(2, 11);
                StartCoroutine(WaitAMomentBeforeTraceWinterLine());
                break;

            case "S03NEW.07":
                s03_1Winter.emitting = false;
                s03_Winter.StopDrawing();
                cameraAnimator.Play("S03NEW07_ZoomCamBack", 0, 0);
                cameraAnimator.SetInteger("StateS03", 1);
                cameraAnimator.speed = 1;
                sRC.enabled = false;
                break;

            case "S03NEW.09":
                cameraAnimator.Play("S03NEW09_ZoomOutOfSphere", 0, 0);
                cameraAnimator.SetInteger("StateS03", 2);
                s03_WinterBlue.StopDrawing();
                s03_1WinterBlue.emitting = false;
                break;
            case "S03NEW.10":
                sunDialParent.SetActive(false);
                inputS0310.alpha = 1;
                inputS0310Transf.localScale = new Vector3(1, 1, 1);
                break;

            case "S03NEW.14":
                makeS03_1Transparent = true;

                Color sunMaterialColor = sunMaterial.color;
                sunMaterialColor.a = 0;
                sunMaterial.color = sunMaterialColor;

                Color sunLineMarchMatColor = sunLineMarchMat.color;
                sunLineMarchMatColor.a = 0;
                sunLineMarchMat.color = sunLineMarchMatColor;
                
                Color sunLineMarchMatBlueColor = sunLineMarchMatBlue.color;
                sunLineMarchMatBlueColor.a = 0;
                sunLineMarchMatBlue.color = sunLineMarchMatBlueColor;
                
                StartCoroutine(WaitBeforeTurnSunOn());
                break;

            case "S03NEW.15":
                s03_2March.emitting = false;
                s03_2MarchBlue.emitting = false;
                s03_March.StopDrawing();
                s03_MarchBlue.StopDrawing();
                break;

            case "S03NEW.16":
                makeS03_2Transparent = true;

                sunMaterialColor = sunMaterial.color;
                sunMaterialColor.a = 0;
                sunMaterial.color = sunMaterialColor;

                StartCoroutine(WaitBeforeTraceSummer());
                break;

            case "S03NEW.17":
                s03_3Summer.emitting = false;
                s03_3SummerBlue.emitting = false;
                s03_Summer.StopDrawing();
                s03_SummerBlue.StopDrawing();
                break;

            case "S03NEW.19":
                cameraAnimator.Play("S03NEW_19RotateToSide", 0, 0);
                cameraAnimator.SetInteger("StateS03", 3);
                break;

            case "S03NEW.20":
                makeS03_3Transparent = true;
                break;

            case "S03NEW.22":
                flashHighlightGrosserWagen = true;
                highlightGrosserWagenOn = true;

                sunMaterialColor = sunMaterial.color;
                sunMaterialColor.a = 0;
                sunMaterial.color = sunMaterialColor;

                skySunAnimator.SetInteger("StartAnimateTimeline", 15);
                skySunAnimator.Play("S03NEWAnimateTo10", 0, 0);
                skySunAnimator.speed = 1.0f;
                break;

            case "S03NEW.23":
                flashHighlightGrosserWagen = false;
                highlightGrosserWagenOff = false;
                turnGrosserWagenOn = true;

                //Himmelspositionberechnung stoppen, weil sonst die Himmelskugel kurz hin und her springt
                rotStarrySky.StopRotation();
                //Start if weekcalendar is in

                skySunAnimator.Play("S03NewChangeAnimWithScriptMonthly", 0, 0);
                skySunAnimator.SetInteger("StartAnimateTimeline", 171);
                skySunAnimator.speed = 1f;
                break;

            case "S03NEW.23b":
                Color sunLineSeptMatColor = sunLineSeptMat.color;
                Color sunLineSeptMatBlueColor = sunLineSeptMatBlue.color;
                sunLineSeptMatColor.a = 1;
                sunLineSeptMatBlueColor.a = 1;
                sunLineSeptMat.color = sunLineSeptMatColor;
                sunLineSeptMatBlue.color = sunLineSeptMatBlueColor;
                changeEkliptikPointsColorTrans = true;
                changeEkliptikPointsColorFull = false;
                skySunAnimator.Play("S03New23b_DailyRotationJuly", 0, 0);
                skySunAnimator.SetInteger("StartAnimateTimeline", 181);
                skySunAnimator.speed = 1f;
                //Himmelspositionberechnung für Rotation wieder erlauben
                rotStarrySky.AllowCalculateRotationContinous();
                break;

            case "S03NEW.24":
                turnDailyLineJulyOff = true;
                rotStarrySky.StopRotation();

                changeEkliptikPointsColorFull = true;
                changeEkliptikPointsColorTrans = false;
                skySunAnimator.Play("S03New25ContinueFromJuly", 0, 0);
                skySunAnimator.SetInteger("StartAnimateTimeline", 191);
                skySunAnimator.speed = 1.25f;
                break;

            case "S03NEW.25":
                Color sunLineSeptMatColor2 = sunLineSeptMat.color;
                Color sunLineSeptMatBlueColor2 = sunLineSeptMatBlue.color;
                sunLineSeptMatColor.a = 1;
                sunLineSeptMatBlueColor.a = 1;
                sunLineSeptMat.color = sunLineSeptMatColor2;
                sunLineSeptMatBlue.color = sunLineSeptMatBlueColor2;
                rotStarrySky.AllowCalculateRotationContinous();

                changeEkliptikPointsColorTrans = true;
                changeEkliptikPointsColorFull = false;
                skySunAnimator.Play("S03NEW24_ekliptik407", 0, 0);
                skySunAnimator.SetInteger("StartAnimateTimeline", 17);
                skySunAnimator.speed = 1f;
                break;

            case "S03NEW.26":
                turnDailyLineJulyOff = true;
                rotStarrySky.StopRotation();
                break;

            case "S03NEW.28":
                flashHighlightMarchLine = false;
                highlightMarchLineOn = false;
                highlightMarchLineOff = true;
                turnMarchLineOff = true;
                changeEkliptikPointsColorFull = true;
                changeEkliptikPointsColorTrans = false;
                skySunAnimator.Play("S03New_27ContinueMonthlyToWinter", 0, 0);
                skySunAnimator.SetInteger("StartAnimateTimeline", 201);
                skySunAnimator.speed = 1.75f;
                break;

            case "S03NEW.30":
                skySunAnimator.Play("S03New_30ContinueToFullCircle", 0, 0);
                skySunAnimator.SetInteger("StartAnimateTimeline", 211);
                skySunAnimator.speed = 1.75f;
                break;

            case "S03NEW.32":
                tIC.HideScrollPanel();
                cc.ChangeDateTextColor(1);
                cc.ChangeTimeTextColor(1);

                sunMaterialColor = sunMaterial.color;
                sunMaterialColor.a = 0;
                sunMaterial.color = sunMaterialColor;

                foreach (Transform child in ekliptikPointParent.transform) {
                    if (child.transform.name != "EkliptikMarkerMarch" &&
                        child.transform.name != "EkliptikMarkerFebruary") {
                        child.transform.GetChild(0).transform.GetComponent<MeshRenderer>().sharedMaterial =
                            ekliptikPointTransparent;
                    } else {
                        child.transform.GetChild(0).transform.GetComponent<MeshRenderer>().sharedMaterial =
                            ekliptikPointTransparentBehind;
                    }
                }

                turnEkliptikBogenOn = true;
                break;


            case "S03NEW.34":
                turnEkliptikBogenOff = true;
                highlightTageskreise = StartCoroutine(HighlightTagesKreise());
                foreach (Transform child in ekliptikPointParent.transform) {
                    child.transform.gameObject.SetActive(false);
                }

                Color ekliptikMarkerColor = ekliptikMarker.color;
                ekliptikMarkerColor = new Color(1, 0.2039f, 0.4039f, 1);
                ekliptikPointTransparent.color = ekliptikMarkerColor;
                ekliptikPointTransparentBehind.color = ekliptikMarkerColor;
                break;

            case "S03NEW.35":
                StopCoroutine(highlightTageskreise);
                ssL.FinishedPathPoint("S03NEW");
                flashHighlightMarchLine = false;
                highlightMarchLineOn = false;
                highlightMarchLineOff = true;
                flashHighlightSummerLine = false;
                highlightSummerLineOn = false;
                highlightSummerLineOff = true;
                flashHighlightWinterLine = false;
                highlightWinterLineOn = false;
                highlightWinterLineOff = true;
                turnWinterLineOff = true;
                turnMarchLineOff = true;
                turnSummerLineOff = true;

                StartCoroutine(ActivateEachEkliptikPointAndThenHighlightEkliptik());
                break;
        }
    }
    
    private void Update() {
        if (checkIf16Reached) {
            if (skyTimeController.GetAnimTime() >= 18 && skyTimeController.GetAnimTime() < 19f) {
                okButton.interactable = true;
                skySunAnimator.speed = 0;
                s03_1Winter.emitting = false;
                s03_Winter.StopDrawing();
                s03_1WinterBlue.emitting = false;
                s03_WinterBlue.StopDrawing();
                checkIf16Reached = false;
            }
        }

        if (checkIf17Reached) {
            if (skyTimeController.GetAnimTime() >= 18 && skyTimeController.GetAnimTime() < 19f) {
                okButton.interactable = true;
                skySunAnimator.speed = 0;
                s03_2March.emitting = false;
                s03_March.StopDrawing();
                s03_2MarchBlue.emitting = false;
                s03_MarchBlue.StopDrawing();
                checkIf17Reached = false;
            }
        }

        if (checkIf19Reached) {
            if (skyTimeController.GetAnimTime() >= 21 && skyTimeController.GetAnimTime() < 24f) {
                okButton.interactable = true;
                skySunAnimator.speed = 0;
                sunTraceS03.emitting = false;
                s03_3Summer.emitting = false;
                s03_Summer.StopDrawing();
                s03_3SummerBlue.emitting = false;
                s03_SummerBlue.StopDrawing();
                checkIf19Reached = false;
            }
        }

    #region graphical Actions

        if (changeEkliptikPointsColorTrans) {
            lerpedColor = Color.Lerp(ekliptikPointFull, ekliptikPointTransparentColor, Time.time);
            ekliptikMarker.color = lerpedColor;
            ekliptikPointTransparent.color = lerpedColor;
            ekliptikPointTransparentBehind.color = lerpedColor;
            if (ekliptikMarker.color == ekliptikPointTransparentColor) {
                changeEkliptikPointsColorTrans = false;
            }
        }

        if (changeEkliptikPointsColorFull) {
            lerpedColor = Color.Lerp(ekliptikPointTransparentColor, ekliptikPointFull, Time.time);
            ekliptikMarker.color = lerpedColor;
            ekliptikPointTransparent.color = lerpedColor;
            ekliptikPointTransparentBehind.color = lerpedColor;
            if (ekliptikMarker.color == ekliptikPointFull) {
                changeEkliptikPointsColorFull = false;
            }
        }

        if (turnGrosserWagenOn) {
            Color importantStarsColor = importantStars.color;
            if (importantStarsColor.a < 1) {
                importantStarsColor.a += 0.1f;
                importantStars.color = importantStarsColor;
            } else {
                turnGrosserWagenOn = false;
            }
        }

        if (turnEkliptikBogenOn) {
            Color ekliptikMarkerColor = ekliptikPointTransparent.color;
            Color ekliptikBogenColor = ekliptikBogen.color;
            if (ekliptikBogenColor.a < 1) {
                ekliptikBogenColor.a += 0.075f;
                ekliptikMarkerColor.a -= 0.1f;
                ekliptikBogen.color = ekliptikBogenColor;
                ekliptikBogenHinten.color = ekliptikBogenColor;
                ekliptikPointTransparent.color = ekliptikMarkerColor;
                ekliptikPointTransparentBehind.color = ekliptikMarkerColor;
            } else {
                ekliptikBogenColor.a = 1f;
                ekliptikBogen.color = ekliptikBogenColor;
                ekliptikBogenHinten.color = ekliptikBogenColor;
                ekliptikMarkerColor.a = 0f;
                ekliptikPointTransparent.color = ekliptikMarkerColor;
                ekliptikPointTransparentBehind.color = ekliptikMarkerColor;
                turnEkliptikBogenOn = false;
            }
        }

        if (turnEkliptikBogenOff) {
            Color ekliptikBogenColor = ekliptikBogen.color;
            if (ekliptikBogenColor.a > 0.3f) {
                ekliptikBogenColor.a -= 0.1f;
                ekliptikBogen.color = ekliptikBogenColor;
                ekliptikBogenHinten.color = ekliptikBogenColor;
            } else {
                ekliptikBogenColor.a = 0.3f;
                ekliptikBogen.color = ekliptikBogenColor;
                ekliptikBogenHinten.color = ekliptikBogenColor;
                turnEkliptikBogenOn = false;
            }
        }

        if (flashHighlightGrosserWagen) {
            if (highlightGrosserWagenOn) {
                Color importantStarsColor = importantStars.color;

                if (importantStarsColor.a < 1) {
                    importantStarsColor.a += 0.05f;
                    importantStars.color = importantStarsColor;
                } else {
                    highlightGrosserWagenOff = true;
                    highlightGrosserWagenOn = false;
                }
            }

            if (highlightGrosserWagenOff) {
                Color importantStarsColor = importantStars.color;
                if (importantStarsColor.a > 0) {
                    importantStarsColor.a -= 0.05f;
                    importantStars.color = importantStarsColor;
                } else {
                    highlightGrosserWagenOn = true;
                    highlightGrosserWagenOff = false;
                }
            }
        }

        if (flashHighlightEkliptik) {
            if (highlightEkliptikOn) {
                Color ekliptikBogenColor = ekliptikBogen.color;

                if (ekliptikBogenColor.a < 1) {
                    ekliptikBogenColor.a += 0.05f;
                    ekliptikBogen.color = ekliptikBogenColor;
                    ekliptikBogenHinten.color = ekliptikBogenColor;
                } else {
                    highlightEkliptikOff = true;
                    highlightEkliptikOn = false;
                }
            }

            if (highlightEkliptikOff) {
                Color ekliptikBogenColor = ekliptikBogen.color;
                if (ekliptikBogenColor.a > 0.2f) {
                    ekliptikBogenColor.a -= 0.05f;
                    ekliptikBogen.color = ekliptikBogenColor;
                    ekliptikBogenHinten.color = ekliptikBogenColor;
                } else {
                    highlightEkliptikOn = true;
                    highlightEkliptikOff = false;
                }
            }
        }

        if (flashHighlightMarchLine) {
            if (highlightMarchLineOn) {
                Color sunLineMarchMatColor = sunLineMarchMat.color;
                Color sunLineMarchMatColorBlue = sunLineMarchMatBlue.color;
                if (sunLineMarchMatColor.a < 1) {
                    sunLineMarchMatColor.a += 0.05f;
                    sunLineMarchMatColorBlue.a += 0.05f;
                    sunLineMarchMat.color = sunLineMarchMatColor;
                    sunLineMarchMatBlue.color = sunLineMarchMatColorBlue;
                } else {
                    highlightMarchLineOff = true;
                    highlightMarchLineOn = false;
                }
            }

            if (highlightMarchLineOff) {
                Color sunLineMarchMatColor = sunLineMarchMat.color;
                Color sunLineMarchMatColorBlue = sunLineMarchMatBlue.color;
                if (sunLineMarchMatColor.a > 0.2f) {
                    sunLineMarchMatColor.a -= 0.05f;
                    sunLineMarchMatColorBlue.a -= 0.05f;
                    sunLineMarchMat.color = sunLineMarchMatColor;
                    sunLineMarchMatBlue.color = sunLineMarchMatColorBlue;
                } else {
                    highlightMarchLineOff = false;
                    highlightMarchLineOn = true;
                }
            }
        }

        if (flashHighlightWinterLine) {
            if (highlightWinterLineOn) {
                Color sunLineWinterMatColor = sunLineWinterMat.color;
                Color sunLineWinterMatColorBlue = sunLineWinterMatBlue.color;
                if (sunLineWinterMatColor.a < 1) {
                    sunLineWinterMatColor.a += 0.05f;
                    sunLineWinterMatColorBlue.a += 0.05f;
                    sunLineWinterMat.color = sunLineWinterMatColor;
                    sunLineWinterMatBlue.color = sunLineWinterMatColorBlue;
                } else {
                    highlightWinterLineOff = true;
                    highlightWinterLineOn = false;
                }
            }

            if (highlightWinterLineOff) {
                Color sunLineWinterMatColor = sunLineWinterMat.color;
                Color sunLineWinterMatColorBlue = sunLineWinterMatBlue.color;
                if (sunLineWinterMatColor.a > 0.2f) {
                    sunLineWinterMatColor.a -= 0.05f;
                    sunLineWinterMatColorBlue.a -= 0.05f;
                    sunLineWinterMat.color = sunLineWinterMatColor;
                    sunLineWinterMatBlue.color = sunLineWinterMatColorBlue;
                } else {
                    highlightWinterLineOff = false;
                    highlightWinterLineOn = true;
                }
            }
        }

        if (flashHighlightSummerLine) {
            if (highlightSummerLineOn) {
                Color sunLineSummerMatColor = sunLineSummerMat.color;
                Color sunLineSummerMatColorBlue = sunLineSummerMatBlue.color;
                if (sunLineSummerMatColor.a < 1) {
                    sunLineSummerMatColor.a += 0.05f;
                    sunLineSummerMatColorBlue.a += 0.05f;
                    sunLineSummerMat.color = sunLineSummerMatColor;
                    sunLineSummerMatBlue.color = sunLineSummerMatColorBlue;
                } else {
                    highlightSummerLineOff = true;
                    highlightSummerLineOn = false;
                }
            }

            if (highlightSummerLineOff) {
                Color sunLineSummerMatColor = sunLineSummerMat.color;
                Color sunLineSummerMatColorBlue = sunLineSummerMatBlue.color;
                if (sunLineSummerMatColor.a > 0.2f) {
                    sunLineSummerMatColor.a -= 0.05f;
                    sunLineSummerMatColorBlue.a -= 0.05f;
                    sunLineSummerMat.color = sunLineSummerMatColor;
                    sunLineSummerMatBlue.color = sunLineSummerMatColorBlue;
                } else {
                    highlightSummerLineOff = false;
                    highlightSummerLineOn = true;
                }
            }
        }

        if (turnMarchLineOff) {
            Color sunLineMarchMatColor = sunLineMarchMat.color;
            Color sunLineMarchMatColorBlue = sunLineMarchMatBlue.color;

            if (sunLineMarchMatColor.a > 0.3f) {
                sunLineMarchMatColor.a -= 0.1f;
                sunLineMarchMat.color = sunLineMarchMatColor;
                sunLineMarchMatColorBlue.a -= 0.1f;
                sunLineMarchMatBlue.color = sunLineMarchMatColorBlue;
            } else {
                sunLineMarchMatColorBlue.a = 0.3f;
                sunLineMarchMatBlue.color = sunLineMarchMatColorBlue;
                sunLineMarchMatColor.a = 0.3f;
                sunLineMarchMat.color = sunLineMarchMatColor;
                turnMarchLineOff = false;
            }
        }

        if (turnMarchLineFullOff) {
            Color sunLineMarchMatColor = sunLineMarchMat.color;
            Color sunLineMarchMatColorBlue = sunLineMarchMatBlue.color;
            if (sunLineMarchMatColor.a > 0f) {
                sunLineMarchMatColor.a -= 0.1f;
                sunLineMarchMat.color = sunLineMarchMatColor;
                sunLineMarchMatColorBlue.a -= 0.1f;
                sunLineMarchMatBlue.color = sunLineMarchMatColorBlue;
            } else {
                sunLineMarchMatColor.a = 0f;
                sunLineMarchMat.color = sunLineMarchMatColor;
                sunLineMarchMatColorBlue.a = 0f;
                sunLineMarchMatBlue.color = sunLineMarchMatColorBlue;
                turnMarchLineFullOff = false;
            }
        }

        if (turnSummerLineOff) {
            Color sunLineSummerMatColor = sunLineSummerMat.color;
            Color sunLineSummerMatColorBlue = sunLineSummerMatBlue.color;

            if (sunLineSummerMatColor.a > 0.3f) {
                sunLineSummerMatColor.a -= 0.1f;
                sunLineSummerMat.color = sunLineSummerMatColor;
                sunLineSummerMatColorBlue.a -= 0.1f;
                sunLineSummerMatBlue.color = sunLineSummerMatColorBlue;
            } else {
                sunLineSummerMatColorBlue.a = 0.3f;
                sunLineSummerMatBlue.color = sunLineSummerMatColorBlue;
                sunLineSummerMatColor.a = 0.3f;
                sunLineSummerMat.color = sunLineSummerMatColor;
                turnSummerLineOff = false;
            }
        }

        if (turnSummerLineFullOff) {
            Color sunLineSummerMatColor = sunLineSummerMat.color;
            Color sunLineSummerMatColorBlue = sunLineSummerMatBlue.color;
            if (sunLineSummerMatColor.a > 0f) {
                sunLineSummerMatColor.a -= 0.1f;
                sunLineSummerMat.color = sunLineSummerMatColor;
                sunLineSummerMatColorBlue.a -= 0.1f;
                sunLineSummerMatBlue.color = sunLineSummerMatColorBlue;
            } else {
                sunLineSummerMatColor.a = 0f;
                sunLineSummerMat.color = sunLineSummerMatColor;
                sunLineSummerMatColorBlue.a = 0f;
                sunLineSummerMatBlue.color = sunLineSummerMatColorBlue;
                turnSummerLineFullOff = false;
            }
        }

        if (turnWinterLineOff) {
            Color sunLineWinterMatColor = sunLineWinterMat.color;
            Color sunLineWinterMatColorBlue = sunLineWinterMatBlue.color;

            if (sunLineWinterMatColor.a > 0.3f) {
                sunLineWinterMatColor.a -= 0.1f;
                sunLineWinterMat.color = sunLineWinterMatColor;
                sunLineWinterMatColorBlue.a -= 0.1f;
                sunLineWinterMatBlue.color = sunLineWinterMatColorBlue;
            } else {
                sunLineWinterMatColorBlue.a = 0.3f;
                sunLineWinterMatBlue.color = sunLineWinterMatColorBlue;
                sunLineWinterMatColor.a = 0.3f;
                sunLineWinterMat.color = sunLineWinterMatColor;
                turnWinterLineOff = false;
            }
        }

        if (turnWinterLineFullOff) {
            Color sunLineWinterMatColor = sunLineWinterMat.color;
            Color sunLineWinterMatColorBlue = sunLineWinterMatBlue.color;
            if (sunLineWinterMatColor.a > 0f) {
                sunLineWinterMatColor.a -= 0.1f;
                sunLineWinterMat.color = sunLineWinterMatColor;
                sunLineWinterMatColorBlue.a -= 0.1f;
                sunLineWinterMatBlue.color = sunLineWinterMatColorBlue;
            } else {
                sunLineWinterMatColor.a = 0f;
                sunLineWinterMat.color = sunLineWinterMatColor;
                sunLineWinterMatColorBlue.a = 0f;
                sunLineWinterMatBlue.color = sunLineWinterMatColorBlue;
                turnWinterLineFullOff = false;
            }
        }

        if (turnImportantStarsOn) {
            Color importantStarsColor = importantStars.color;
            if (importantStarsColor.a < 1) {
                importantStarsColor.a += 0.075f;
                importantStars.color = importantStarsColor;
            } else {
                importantStarsColor.a = 1f;
                importantStars.color = importantStarsColor;
                turnImportantStarsOn = false;
            }
        }

        if (turnSunOn) {
            Color sunColor = sunMaterial.color;
            if (sunColor.a < 1) {
                sunColor.a += 0.08f;
                sunMaterial.color = sunColor;
            } else {
                sunColor.a = 1f;
                sunMaterial.color = sunColor;
                StartCoroutine(StartWhileSunIsOn());
                turnSunOn = false;
            }
        }

        if (turnDailyLineJulyOff) {
            Color sunLineSeptMatColor = sunLineSeptMat.color;
            Color sunLineSeptMatBlueColor = sunLineSeptMatBlue.color;
            if (sunLineSeptMatColor.a > 0) {
                sunLineSeptMatBlueColor.a -= 0.1f;
                sunLineSeptMatBlue.color = sunLineSeptMatBlueColor;
                sunLineSeptMatColor.a -= 0.1f;
                sunLineSeptMat.color = sunLineSeptMatColor;
            } else {
                sunLineSeptMatBlueColor.a = 0;
                sunLineSeptMatBlue.color = sunLineSeptMatBlueColor;
                sunLineSeptMatColor.a = 0;
                sunLineSeptMat.color = sunLineSeptMatColor;
                s03_Sept.StopCheckAndDraw();
                s03_SeptBlue.StopCheckAndDraw();
                turnDailyLineJulyOff = false;
            }
        }

        if (turnBlackBlendOn) {
            if (blackBlend.alpha < 1f) {
                blackBlend.alpha += 0.07f;
            } else {
                turnBlackBlendOn = false;
            }
        }

        if (turnBlackBlendOff) {
            if (blackBlend.alpha > 0) {
                blackBlend.alpha -= 0.07f;
            } else {
                ProcedureController pC = (ProcedureController) gameController.GetComponent(typeof(ProcedureController));
                pC.JumpToNextStep();
                turnBlackBlendOff = false;
            }
        }

        if (makeS03_1Transparent) {
            Color sunLineWinterMatColor = sunLineWinterMat.color;
            Color sunLineWinterMatColorBlue = sunLineWinterMatBlue.color;
            if (sunLineWinterMatColor.a > 0.5f) {
                sunLineWinterMatColor.a -= 0.1f;
                sunLineWinterMat.color = sunLineWinterMatColor;
                sunLineWinterMatColorBlue.a -= 0.1f;
                sunLineWinterMatBlue.color = sunLineWinterMatColorBlue;
            } else {
                sunLineWinterMatColor.a = 0.5f;
                sunLineWinterMat.color = sunLineWinterMatColor;
                sunLineWinterMatColorBlue.a = 0.5f;
                sunLineWinterMatBlue.color = sunLineWinterMatColorBlue;
                makeS03_1Transparent = false;
            }
        }

        if (makeS03_2Transparent) {
            Color sunLineMarchMatColor = sunLineMarchMat.color;
            Color sunLineMarchMatColorBlue = sunLineMarchMatBlue.color;
            if (sunLineMarchMatColor.a > 0.5f) {
                sunLineMarchMatColor.a -= 0.1f;
                sunLineMarchMat.color = sunLineMarchMatColor;
                sunLineMarchMatColorBlue.a -= 0.1f;
                sunLineMarchMatBlue.color = sunLineMarchMatColorBlue;
            } else {
                sunLineMarchMatColor.a = 0.5f;
                sunLineMarchMat.color = sunLineMarchMatColor;
                sunLineMarchMatColorBlue.a = 0.5f;
                sunLineMarchMatBlue.color = sunLineMarchMatColorBlue;
                makeS03_2Transparent = false;
            }
        }

        if (makeS03_3Transparent) {
            Color sunLineSummerMatColor = sunLineSummerMat.color;
            Color sunLineSummerMatColorBlue = sunLineSummerMatBlue.color;
            if (sunLineSummerMatColor.a > 0.5f) {
                sunLineSummerMatColor.a -= 0.1f;
                sunLineSummerMat.color = sunLineSummerMatColor;
                sunLineSummerMatColorBlue.a -= 0.1f;
                sunLineSummerMatBlue.color = sunLineSummerMatColorBlue;
            } else {
                sunLineSummerMatColor.a = 0.5f;
                sunLineSummerMat.color = sunLineSummerMatColor;
                sunLineSummerMatColorBlue.a = 0.5f;
                sunLineSummerMatBlue.color = sunLineSummerMatColorBlue;
                makeS03_3Transparent = false;
            }
        }

    #endregion
    }
    
    
    private void ResetScript() {
    #region Reset private Variables

        flashHighlightMarchLine = false;
        highlightMarchLineOn = false;
        highlightMarchLineOff = false;
        flashHighlightSummerLine = false;
        highlightSummerLineOn = false;
        highlightSummerLineOff = false;
        flashHighlightWinterLine = false;
        highlightWinterLineOn = false;
        highlightWinterLineOff = false;
        flashHighlightEkliptik = false;
        highlightEkliptikOn = false;
        highlightEkliptikOff = false;
        turnEkliptikBogenOff = false;
        flashHighlightGrosserWagen = false;
        highlightGrosserWagenOff = false;
        highlightGrosserWagenOn = false;
        highlightTageskreise = null;
        checkIf16Reached = false;
        checkIf17Reached = false;
        checkIf19Reached = false;
        flashHighlightEkliptik = false;
        highlightEkliptikOn = false;
        highlightEkliptikOff = false;

    #endregion
        
    #region Reset Camera

        ShowSplitScreen sss = (ShowSplitScreen) sunDialParent.GetComponent(typeof(ShowSplitScreen));
        sss.Reset();

        skyCam.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = true;
        skyCamCenterTransf.localPosition = new Vector3(0, 28, -4);
        skyCameraTransf.localPosition = new Vector3(0, 0, 0);
        sunDialCameraObject.transform.localPosition = new Vector3(0, 112, 225.6f);
        cameraCenterTransform.localEulerAngles = new Vector3(0, 180, 0);
        mainCamTransform.localEulerAngles = new Vector3(0, 0, 0);

        float resolutionFactor2 = DeviceInfo.GetResolutionFactor();
        float newZpos = 0 - ((resolutionFactor2 - 0.45f) * 250 / 0.3f);
        mainCamParent.localPosition = new Vector3(0, 60, newZpos);
        mainCamParent.localEulerAngles = new Vector3(0, 0, 0);

        float screenfactor = DeviceInfo.GetResolutionFactor();
        float tempFOV = ((screenfactor - 0.45f) * 20.0f / 0.3f) + 40;
        skyCam.fieldOfView = tempFOV;

        lD.intensity.Override(0.4f);
        lD.scale.Override(1.04f);
        lD.xMultiplier.Override(1.0f);

        mainCam.fieldOfView = 40;
        cameraAnimator.enabled = true;
        cameraAnimator.Rebind();
        cameraAnimator.Update(0f);

    #endregion

    #region Reset Date and Time

        skyTimeController.SetLatitude(48.0f);
        SkyProfileEventHandler sPEH =
            (SkyProfileEventHandler) skyObject.GetComponent(typeof(SkyProfileEventHandler));
        sPEH.ResetScript();
        sPEH.SetAllowUpdateLightTrue();
        sPEH.HideTheStars();
        sPEH.OnlyShowFakeSun();
        skyTimeController.SetDate(System.DateTime.Now.Year, 9, 21);
        skyTimeController.SetAllowSetTimeLineWithAnim();
        skyTimeController.animTime = 14.0f;

        cc.SetRealTime();

        dateText.text = "21.9." + System.DateTime.Now.Year;
        if (pC.GetLanguage() == 1) {
            dateText.text = "Sep 21, " + System.DateTime.Now.Year;
        }

        cc.ChangeDateTextColor(0);
        cc.ChangeTimeTextColor(0);

    #endregion

    #region Reset Materials

        Color sunTraceLineConstantColor = sunTraceLineConstant.color;
        sunTraceLineConstantColor.a = 0;
        sunTraceLineConstant.color = sunTraceLineConstantColor;

        Color gitterNetzMatColor = gitterNetzMat.color;
        gitterNetzMatColor.a = 0;
        gitterNetzMat.color = gitterNetzMatColor;

        planeGroundMR.sharedMaterial = planeGround;
        Color planeGroundColor = planeGround.color;
        planeGroundColor.a = 1;
        planeGround.color = planeGroundColor;
        gridGround.sortingOrder = 1;

        north.faceColor = new Color(255, 255, 255, 0);
        east.faceColor = new Color(255, 255, 255, 0);
        south.faceColor = new Color(255, 255, 255, 255);
        west.faceColor = new Color(255, 255, 255, 0);

        Color earthhorizontCircleColor = earthhorizontCircle.color;
        earthhorizontCircleColor.a = 0;
        earthhorizontCircle.color = earthhorizontCircleColor;

        Color earthMaterialColor = earthMaterial.color;
        earthMaterialColor.a = 0;
        earthMaterial.color = earthMaterialColor;

        Color figureEarthColor = figureEarth.color;
        figureEarthColor.a = 0;
        figureEarth.color = figureEarthColor;

        Color sunMaterialColor = sunMaterial.color;
        sunMaterialColor.a = 1;
        sunMaterial.color = sunMaterialColor;

        Color gridHorizontColor = gridHorizont.color;
        gridHorizontColor.a = 1;
        gridHorizont.color = gridHorizontColor;

        Color sunLineWinterMatColor = sunLineWinterMat.color;
        sunLineWinterMatColor.a = 1;
        sunLineWinterMat.color = sunLineWinterMatColor;
        sunLineMarchMat.color = sunLineWinterMatColor;
        sunLineSummerMat.color = sunLineWinterMatColor;
        sunLineSeptMat.color = sunLineWinterMatColor;

        Color sunlineWinterMatColorBlue = sunLineWinterMatBlue.color;
        sunlineWinterMatColorBlue.a = 1;
        sunLineWinterMatBlue.color = sunlineWinterMatColorBlue;
        sunLineMarchMatBlue.color = sunlineWinterMatColorBlue;
        sunLineSummerMatBlue.color = sunlineWinterMatColorBlue;
        sunLineSeptMatBlue.color = sunlineWinterMatColorBlue;

        Color sunLineHighlightOverHorizontColor = sunLineHighlightOverHorizont.color;
        sunLineHighlightOverHorizontColor.a = 0;
        sunLineHighlightOverHorizont.color = sunLineHighlightOverHorizontColor;

        Color breitenGradeColor = breitengradLinien.color;
        breitenGradeColor.a = 0;
        breitengradLinien.color = breitenGradeColor;

        Color matStarUnlitColor = matStarUnlit.color;
        matStarUnlitColor.a = 0.4f;
        matStarUnlit.color = matStarUnlitColor;

        Color gridGroundColor = gridGround.color;
        gridGroundColor.a = 1;
        gridGround.color = gridGroundColor;

        Color gitternetzGradeColor = gitternetzGrade.color;
        gitternetzGradeColor.a = 0;
        gitternetzGrade.color = gitternetzGradeColor;
        breitengradLinien.color = gitternetzGradeColor;

        Color horizontLilaColor = horizontLila.color;
        horizontLilaColor.a = 0;
        horizontLila.color = horizontLilaColor;

        Color ekliptikMarkerColor = ekliptikMarker.color;
        ekliptikMarkerColor = new Color(1, 0.2039f, 0.4039f, 0);
        ekliptikMarker.color = ekliptikMarkerColor;
        ekliptikPointTransparent.color = ekliptikMarkerColor;
        ekliptikPointTransparentBehind.color = ekliptikMarkerColor;
        ekliptikMarkerColor = new Color(1, 0.2039f, 0.4039f, 0);
        ekliptikBogen.color = ekliptikMarkerColor;
        ekliptikBogenHinten.color = ekliptikMarkerColor;

        Color sunDialGroundPlateColor = sunDialGroundPlate.color;
        sunDialGroundPlateColor.a = 1;
        sunDialGroundPlate.color = sunDialGroundPlateColor;

        sunDialWood.sharedMaterial = sunDialWoodMatOpaque;
        sunDialGroundPlate.sharedMaterial = sunDialGroundPlateMatOpaque;

        Color importantStarsColor = importantStars.color;
        importantStarsColor.a = 0f;
        importantStars.color = importantStarsColor;

    #endregion

    #region Reset Other

        earthFiure.localEulerAngles = new Vector3(-38.982f, -2.068f, 1.301f);
        earthTransform.localPosition = new Vector3(0, -45, starrySkyCompassPivot.localPosition.z * 0.75f);
        earthTransform.localScale = new Vector3(2.5f, 2.5f, 2.5f);

        skyParentTransf.localEulerAngles = new Vector3(-5, 0, 0);

        if (highlightTageskreise != null) {
            StopCoroutine(highlightTageskreise);
        }

        foreach (Transform child in ekliptikPointParent.transform) {
            Destroy(child.gameObject);
        }

        uRPA.msaaSampleCount = 2;
        ekliptikTrace.Clear();
        ekliptikTrace.enabled = false;
        ekliptikTrace.time = 0;

        aEHS03.StopMoveWithAnimS03AEH();

        southTransf.localPosition = new Vector3(0, 27, -260);
        southTransf.localScale = new Vector3(0.18f, 0.18f, 0.18f);
        south.fontSize = 1500;
        groundTransf.localPosition = new Vector3(0, -340, 0);
        gridGroundTransf.localPosition = new Vector3(0.021f, 0.126f, 0);
        groundDirections.localPosition = new Vector3(0, 0.11f, 0);
        horizontLilaTransf.localPosition = new Vector3(0, 0.13f, 0);

        northForOutofSphereGo.SetActive(true);
        northForOutofSphereGo.transform.localPosition = new Vector3(0f, -0.62f, 8.82f);
        northForOutofSphereGo.transform.localEulerAngles = new Vector3(0, -90, 0);

        outOfSphereBackground.localPosition = new Vector3(0, -2000, 28526);
        outOfSphereBackground.localScale = new Vector3(6000, 6000, 8069);

        grosserWagenN05.SetActive(true);

        sRC.enabled = true;

    #endregion

    #region Reset Starry Sky

        rotStarrySky.AllowCalculateRotationContinous();
        rotateSkarrySkyPivot.localEulerAngles = new Vector3(-10, 0, 0);

    #endregion

    #region Reset Sundial

        sunDialCameraObject.SetActive(true);
        sunDialObject.SetActive(true);
        sunDialParent.SetActive(true);
        moonLight.SetActive(false);
        sunDialAnimator.enabled = false;

        resolutionFactor = (0.75f - (float) Screen.width / (float) Screen.height) * 256 / 0.3f;
        float tempSunDialPos = -70 - resolutionFactor;
        sunDialParent.transform.localPosition = new Vector3(0, -7.2f, tempSunDialPos);


        skySunAnimator.enabled = true;
        skySunAnimator.SetInteger("StartAnimateTimeline", 6);
        skySunAnimator.Play("RotateSunAndSkipNight", 0, 0);
        skySunAnimator.speed = 0;
        sunObjectsParent.localScale = new Vector3(0.78f, 0.78f, 1);

    #endregion

    #region Reset TrailRenderer

        sunTraceS03.enabled = false;
        sunTraceS03.Clear();
        sunTraceS03.emitting = false;
        sunTraceS03.time = 50000;

        s03_1Winter.enabled = false;
        s03_1Winter.Clear();
        s03_1Winter.emitting = false;
        s03_1Winter.time = 0;

        s03_2March.Clear();
        s03_2March.emitting = false;
        s03_2March.time = 0;

        s03_3Summer.Clear();
        s03_3Summer.emitting = false;
        s03_3Summer.time = 0;

        s03_1WinterBlue.enabled = false;
        s03_1WinterBlue.Clear();
        s03_1WinterBlue.emitting = false;
        s03_1WinterBlue.time = 0;

        s03_2MarchBlue.Clear();
        s03_2MarchBlue.emitting = false;
        s03_2MarchBlue.time = 0;

        s03_3SummerBlue.Clear();
        s03_3SummerBlue.emitting = false;
        s03_3SummerBlue.time = 0;

        s03_4Sept.Clear();
        s03_4Sept.emitting = false;
        s03_4Sept.time = 0;

        sunTraceS03OnSkySphere.Clear();
        sunTraceS03OnSkySphere.emitting = false;
        sunTraceS03OnSkySphere.time = 0;

        TrailControllerHorizontS03 tch =
            (TrailControllerHorizontS03) trailObjectHorizontHighlight.GetComponent(typeof(TrailControllerHorizontS03));
        tch.StopCheckAndDraw();

    #endregion
    }
    
    public void InputS03UnterUeber(int inputVal) {
        //0 über, 1 unter
        if (!hC.GetState()) {
            if (inputVal == 0) {
                GameObject newWrongInputPoint = Instantiate<GameObject>(wrongInputPrefab);
                newWrongInputPoint.transform.parent = canvasParent.transform;
                newWrongInputPoint.transform.localScale = new Vector3(1.0625f, 0.86247f, 1);
                newWrongInputPoint.transform.position =
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
                pC.JumpWithQuestInput(0);
            }

            if (inputVal == 1) {
                GameObject newRightInputPoint = Instantiate<GameObject>(rightInputPrefab);
                newRightInputPoint.transform.parent = canvasParent.transform;
                newRightInputPoint.transform.localScale = new Vector3(1.0625f, 0.86247f, 1);
                newRightInputPoint.transform.position =
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
                pC.JumpWithQuestInput(1);
            }

            inputS0310.alpha = 0;
            inputS0310Transf.localScale = new Vector3(0, 0, 0);
        }
    }

    private IEnumerator WaitAMomentBeforeTraceWinterLine() {
        yield return new WaitForSeconds(0.5f);
        skySunAnimator.SetInteger("StartAnimateTimeline", 61);
        skySunAnimator.Play("RotateSunWinterS03NEW", 0, 0);
        skySunAnimator.speed = 1;

        sunObjectsParent.localScale = new Vector3(1, 1, 1);
    }

    private IEnumerator WaitBeforeTurnSunOn() {
        yield return new WaitForSeconds(0.1f);
        cSINC.SetTargetScrollPosMonthNamesCalendar(0.2222f);
        cSINC.UpdateFontManually(5, 2);
        skyTimeController.SetDate(System.DateTime.Now.Year, 3, 21);
        skySunAnimator.SetInteger("StartAnimateTimeline", 13);
        skySunAnimator.Play("RotateSunInS03New", 0, 0);
        skySunAnimator.speed = 0;

        yield return new WaitForSeconds(0.5f);
        Color sunLineMarchMatColor = sunLineMarchMat.color;
        sunLineMarchMatColor.a = 1;
        sunLineMarchMat.color = sunLineMarchMatColor;
        Color sunLineMarchMatBlueColor = sunLineMarchMatBlue.color;
        sunLineMarchMatBlueColor.a = 1;
        sunLineMarchMatBlue.color = sunLineMarchMatBlueColor;
        turnSunOn = true;
    }

    private IEnumerator StartWhileSunIsOn() {
        yield return new WaitForSeconds(1f);
        skySunAnimator.speed = 1;
    }

    private IEnumerator WaitBeforeTraceSummer() {
        yield return new WaitForSeconds(0.1f);

        cSINC.SetTargetScrollPosMonthNamesCalendar(0.3758f);
        cSINC.UpdateFontManually(8, 5);
        skyTimeController.SetDate(System.DateTime.Now.Year, 6, 21);
        skySunAnimator.SetInteger("StartAnimateTimeline", 14);
        skySunAnimator.Play("RotateSunInS03NewFrom95", 0, 0);
        yield return new WaitForSeconds(0.5f);
        turnSunOn = true;
    }

    private IEnumerator HighlightTagesKreise() {
        Color sunLineYellow = sunLineMarchMat.color;
        Color sunLineBlue = sunLineMarchMatBlue.color;
        sunLineYellow.a = 0.3f;
        sunLineBlue.a = 0.3f;

        int i = 0;
        do {
            flashHighlightSummerLine = true;
            highlightSummerLineOn = true;
            highlightSummerLineOff = false;
            flashHighlightMarchLine = false;
            flashHighlightWinterLine = false;
            sunLineMarchMat.color = sunLineYellow;
            sunLineMarchMatBlue.color = sunLineBlue;
            sunLineWinterMat.color = sunLineYellow;
            sunLineWinterMatBlue.color = sunLineBlue;

            yield return new WaitForSeconds(2f);
            flashHighlightMarchLine = true;
            highlightMarchLineOn = true;
            highlightMarchLineOff = false;
            flashHighlightSummerLine = false;
            flashHighlightWinterLine = false;
            sunLineSummerMat.color = sunLineYellow;
            sunLineSummerMatBlue.color = sunLineBlue;
            sunLineWinterMat.color = sunLineYellow;
            sunLineWinterMatBlue.color = sunLineBlue;

            yield return new WaitForSeconds(2f);
            flashHighlightWinterLine = true;
            highlightWinterLineOn = true;
            highlightWinterLineOff = false;
            flashHighlightSummerLine = false;
            flashHighlightMarchLine = false;
            sunLineMarchMat.color = sunLineYellow;
            sunLineMarchMatBlue.color = sunLineBlue;
            sunLineSummerMat.color = sunLineYellow;
            sunLineSummerMatBlue.color = sunLineBlue;
            sunLineMarchMat.color = sunLineYellow;
            sunLineMarchMatBlue.color = sunLineBlue;

            yield return new WaitForSeconds(2f);
            flashHighlightMarchLine = true;
            highlightMarchLineOn = true;
            highlightMarchLineOff = false;
            flashHighlightSummerLine = false;
            flashHighlightWinterLine = false;
            sunLineSummerMat.color = sunLineYellow;
            sunLineSummerMatBlue.color = sunLineBlue;
            sunLineWinterMat.color = sunLineYellow;
            sunLineWinterMatBlue.color = sunLineBlue;

            yield return new WaitForSeconds(2f);
        } while (i < 1);
    }

    private IEnumerator ActivateEachEkliptikPointAndThenHighlightEkliptik() {
        foreach (Transform child in ekliptikPointParent.transform) {
            child.transform.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.4f);
        }

        yield return new WaitForSeconds(0.1f);
        flashHighlightEkliptik = true;
        highlightEkliptikOn = true;
        highlightEkliptikOff = false;
        turnEkliptikBogenOff = false;

        yield return new WaitForSeconds(0.1f);
        foreach (Transform child in ekliptikPointParent.transform) {
            child.transform.gameObject.SetActive(false);
        }
    }
}