using System.Collections;
using UnityEngine;

using TMPro;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using System;
using UnityEditor.UIElements;

public class N05Helper : MonoBehaviour {
    #region public Variables

    //Scripts
    public AnimationEventHandlerCameraN05 aEHCN05;
    public SkyProfileEventHandler sPEH;
    public SkyTimeController skyTimeController;
    public CheckPointController cPC;
    public ClockController cc;
    public DeviceInfo deviceInfo;
    public ProcedureController pC;
    public SpielstandLoader ssL;
    public Translator translator;

    //other Objects
    public Animator cameraAnimator;
    public Animator starrySkyAnimator;

    public GameObject clockParentGO;
    public GameObject cylinderN05Input;
    public GameObject earthLowN05;
    public GameObject gitterNetz;
    public GameObject grosserWagenN05;
    public GameObject mainCam;
    public GameObject uhr24HGameobject;
    public GameObject zeigerWeissOnSphere;
    public List<GameObject> traceParents = new List<GameObject>();
    public List<GameObject> traceParents2 = new List<GameObject>();

    public Material breitengradSterneHighlight;
    public Material earthLowN05Mat;
    public Material gitternetzGrade;
    public Material importantStars;
    public Material matStar_Breitengradsterne;
    public Material materialPolarstern;
    public Material matStarUnlit;
    public Material traceN05BS;

    public MeshRenderer ground;
    public Material planeGround;

    public SpriteRenderer gridGround;
    public SpriteRenderer zeigerWhite;
    public SpriteRenderer uhr24H;

    public Transform earthTransform;
    public Transform gridTransf;
    public Transform outOfSphereBackground;
    public Transform starrySkyCompassPivot;
    public Transform skyCompassPivot;
    public Transform starrySkyN05Pivot;
    public Transform terrainCompassPivot;

    public UniversalRenderPipelineAsset uRPA;

    #endregion

    #region private Variables

    private bool continueSphereRotation = false;
    private bool turnLaengengradOn = false;
    private bool turnBsStarsOff = false;
    private bool turnBsStarHighlightOn = false;
    private bool turnZeigerOff = false;
    private bool check90degrees = false;
    private bool degreesHigher360 = false;
    private bool flashHighlightGrosserWagen = false;
    private bool highlightGrosserWagenOn = false;
    private bool highlightGrosserWagenOff = false;
    private bool turnGrosserWagenOn = false;
    private bool show24HUhr = false;
    private bool slowDownRotationFast = false;
    private bool stopSphereRotation = false;

    private float target90Degrees = 0.0f;

    private int lowestURPA = 4;

    #endregion

    void Start() {
        ProcedureController.changeEvent += DoActionWhileStepUpdate;
    }

    public void DoActionWhileStepUpdate(string stepId) {
        switch (stepId) {
            #region Initialer Step / Checkpoint

            case "N05.00a":
            case "N05.00":
                ResetScript();
                aEHCN05.ResetScript();
                TurnTraceLinesOff();

                ClockController cc = (ClockController) clockParentGO.GetComponent(typeof(ClockController));
                cc.SetTimeModeN07();

                sPEH.StopUpdatingProperties();
                pC.JumpToNextStep();
                break;

            #endregion

            case "N05.03":
                CheckFPS();
                earthTransform.localPosition = new Vector3(0, -90, starrySkyCompassPivot.localPosition.z * 0.75f);
                earthTransform.localScale = new Vector3(4, 4, 4);

                //Himmelskugel im Loop drehen und 2 Sterne des GW tracen, wenn ein Tage absolviert -> AnimationEvent
                starrySkyAnimator.SetInteger("StateN05", 1);
                starrySkyAnimator.Play("RotateStarrySkyInLoopN05", 0, 0);

                AllowEmittigOfTrailRenderer();
                foreach (GameObject gO in traceParents) {
                    TrailController tr = (TrailController) gO.GetComponent(typeof(TrailController));
                    tr.StartTracingN05();
                }

                pC.JumpToNextStep();

                break;

            case "N05.05":
                CheckFPS();
                
                //Kamera bis an den Rand der Himmelskugel fahren
                cameraAnimator.Play("CamMove_N05_05", 0, 0);
                cameraAnimator.SetInteger("StateN05", 1);
                cameraAnimator.speed = 1.0f;
                turnZeigerOff = true;
                break;

            #region Checkpoint

            case "N05.09C":
                aEHCN05.ResetScript();
                CheckFPS();

                degreesHigher360 = false;
                target90Degrees = 0;

                Color zeigerWhiteColor = zeigerWhite.color;
                zeigerWhiteColor.a = 0;
                zeigerWhite.color = zeigerWhiteColor;

                Color uhr24HColor = uhr24H.color;
                uhr24HColor.a = 0;
                uhr24H.color = uhr24HColor;
                uhr24HGameobject.SetActive(false);

                cylinderN05Input.SetActive(false);
                turnBsStarsOff = true;
                gitterNetz.SetActive(true);
                turnLaengengradOn = true;

                earthLowN05.SetActive(true);

                cameraAnimator.Rebind();
                //cameraAnimator.Play("CamMove_N05_09", 0, 0);
                //cameraAnimator.SetInteger("StateN05", 3);
                //cameraAnimator.speed = 1;

                stopSphereRotation = false;
                slowDownRotationFast = false;
                starrySkyAnimator.speed = 1.0f;
                starrySkyAnimator.SetInteger("StateN05", 1);
                starrySkyAnimator.Play("RotateStarrySkyInLoopN05", 0, 0);

                earthTransform.localPosition = new Vector3(0, -90, starrySkyCompassPivot.localPosition.z * 0.75f);
                earthTransform.localScale = new Vector3(4, 4, 4);

                pC.JumpToNextStep();
                break;

            #endregion

            case "N05.09":
                CheckFPS();
                TurnTraceLinesOn();
                turnBsStarsOff = true;
                gitterNetz.SetActive(true);
                turnLaengengradOn = true;

                //Kamera aus Himmelskugel rausfahren, Horizont ausblenden und Erde aktivieren
                earthLowN05.SetActive(true);
                cameraAnimator.Play("CamMove_N05_07", 0, 0);
                cameraAnimator.speed = 1;
                cameraAnimator.SetInteger("StateN05", 2);
                break;

            case "N05.10":
                //Checkpoint erreicht
                cPC.SetCheckPointColors("N05", 2);
                cPC.SetCheckPointReached("N05", 02);
                break;

            case "N05.11":
                cameraAnimator.Play("CamMove_N05_09", 0, 0);
                cameraAnimator.speed = 1;
                cameraAnimator.SetInteger("StateN05", 3);
                StartCoroutine(WaitAndSlowDownSphereRotation());
                break;

            case "N05.13":
                CheckFPS();
                stopSphereRotation = true;
                break;

            case "N05.14":
                //Kamera zum Himmelsnordpol rotieren
                cameraAnimator.Play("CamMoveToNorth_N05", 0, 0);
                cameraAnimator.speed = 1;
                cameraAnimator.SetInteger("StateN05", 4);
                break;

            case "N05.20":
                //Für die Interaction Großer Wagen Position in 6 STunden antippen, ist um 90 Grad ein unsichtbares Mesh mit einem Collider
                //Wenn der COllider berührt wurde -> richtige Stelle gewählt
                cylinderN05Input.SetActive(true);
                
                //Großen Wagen Highlighten
                flashHighlightGrosserWagen = true;
                highlightGrosserWagenOn = true;
                highlightGrosserWagenOff = false;
                break;

            case "N05.22":
            case "N05.22b":
                cylinderN05Input.SetActive(false);
                //Himmelskugel Zielposition berechnen (+6 Stunden) = aktuelle Rotation + 90 Grad
                target90Degrees = starrySkyN05Pivot.localEulerAngles.y + 83;
                if (target90Degrees > 360) {
                    target90Degrees -= 360;
                    degreesHigher360 = true;
                } else {
                    degreesHigher360 = false;
                }

                //Rotation fortsetzen, wenn um 90 Grad gedreht wurde erneut stoppen
                check90degrees = true;
                continueSphereRotation = true;
                break;

            case "N05.23":
                //Highlight vom großen Wagen ausblenden
                flashHighlightGrosserWagen = false;
                highlightGrosserWagenOn = false;
                highlightGrosserWagenOff = false;
                turnGrosserWagenOn = true;
                
                //Himmelskugel weiter drehen und in die Kugel reinfliegen
                continueSphereRotation = true;
                StartCoroutine(WaitBeforeStartCamAnim());
                break;

            case "N05.29":
                //24 Stunden Uhr einblenden
                uhr24HGameobject.SetActive(true);
                zeigerWeissOnSphere.SetActive(true);
                show24HUhr = true;
                
                //Neuen Spielstand schreiben
                ssL.FinishedPathPoint("N05");
                break;
        }
    }

    private void Update() {
        //wenn um 90 Grad gedreht wurde, Rotation anhalten und zum nächsten Step springen
        if (check90degrees) {
            if (degreesHigher360) {
                if (starrySkyN05Pivot.localEulerAngles.y > target90Degrees &&
                    starrySkyN05Pivot.localEulerAngles.y < 180) {
                    slowDownRotationFast = true;
                    pC.JumpToNextStep();
                    check90degrees = false;
                }
            } else {
                if (starrySkyN05Pivot.localEulerAngles.y > target90Degrees) {
                    slowDownRotationFast = true;
                    pC.JumpToNextStep();
                    check90degrees = false;
                }
            }
        }

        //Rotation der Himmelskugel langsam anhalten
        if (stopSphereRotation) {
            if (starrySkyAnimator.speed > 0.0f) {
                starrySkyAnimator.speed -= 0.005f;
            } else {
                stopSphereRotation = false;
            }
        }

        //Rotation der Himmelskugel stoppen
        if (slowDownRotationFast) {
            if (starrySkyAnimator.speed > 0f) {
                starrySkyAnimator.speed -= 0.075f;
            } else {
                slowDownRotationFast = false;
            }
        }

        //Rotation der Himmelskugel fortsetzen
        if (continueSphereRotation) {
            if (starrySkyAnimator.speed < 1f) {
                starrySkyAnimator.speed += 0.05f;
            } else {
                continueSphereRotation = false;
            }
        }

        #region graphical Actions

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

        if (turnLaengengradOn) {
            Color lg = gitternetzGrade.color;
            if (lg.a < 0.1f) {
                lg.a += 0.005f;
                gitternetzGrade.color = lg;
            } else {
                turnLaengengradOn = false;
            }
        }

        if (turnBsStarsOff) {
            Color starsBS07TO = matStar_Breitengradsterne.color;
            if (starsBS07TO.a > 0) {
                starsBS07TO.a -= 0.05f;
                matStar_Breitengradsterne.color = starsBS07TO;
                breitengradSterneHighlight.color = starsBS07TO;
            } else {
                turnBsStarsOff = false;
            }
        }

        if (turnZeigerOff) {
            Color zeigerWhiteColor = zeigerWhite.color;
            if (zeigerWhiteColor.a > 0) {
                zeigerWhiteColor.a -= 0.075f;
                zeigerWhite.color = zeigerWhiteColor;
            } else {
                turnZeigerOff = false;
            }
        }

        if (show24HUhr) {
            Color uhr24HColor = uhr24H.color;
            if (uhr24HColor.a < 1) {
                uhr24HColor.a += 0.075f;
                uhr24H.color = uhr24HColor;
                zeigerWhite.color = uhr24HColor;
            } else {
                show24HUhr = false;
            }
        }

        if (turnGrosserWagenOn) {
            Color importantStarsColor = importantStars.color;
            if (importantStarsColor.a < 1) {
                importantStarsColor.a += 0.1f;
                importantStars.color = importantStarsColor;
            } else {
                importantStarsColor.a = 1f;
                importantStars.color = importantStarsColor;
                turnGrosserWagenOn = false;
            }
        }

        #endregion
    }


    private void ResetScript() {
        #region Reset private Variables

        continueSphereRotation = false;
        stopSphereRotation = false;
        turnLaengengradOn = false;
        flashHighlightGrosserWagen = false;
        highlightGrosserWagenOn = false;
        highlightGrosserWagenOff = false;
        degreesHigher360 = false;
        target90Degrees = 0;
        slowDownRotationFast = false;

        #endregion

        #region Reset Camera

        cameraAnimator.enabled = true;
        cameraAnimator.Play("CamMove_N05_05", 0, 0);
        cameraAnimator.SetInteger("StateN05", 1);
        cameraAnimator.speed = 0;
        
        uRPA.msaaSampleCount = 2;
        cameraAnimator.enabled = true;

        float latitude = skyTimeController.GetLatitude();
        latitude = -1.0f * latitude;
        float temp = latitude;

        float screenFactor = DeviceInfo.GetScreenfactor();
        float newZPos = (2.22f - screenFactor) * 469.337f;
        starrySkyCompassPivot.transform.localPosition = new Vector3(0, 0, newZPos);
        float resolutionFactor = DeviceInfo.GetResolutionFactor();
        float newXRot = temp + (((resolutionFactor - 0.45f) * 14.0f) / 0.3f);
        mainCam.transform.localEulerAngles = new Vector3(newXRot, 0, 0);

        starrySkyCompassPivot.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        skyCompassPivot.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        terrainCompassPivot.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

        #endregion

        #region Reset Date and Time

        skyTimeController.SetDate(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);
        skyTimeController.SetTimeline(0);

        cc.ChangeDateTextColor(1);
        cc.ChangeTimeTextColor(1);
        cc.SetCurrentDate();
        #endregion

        #region Reset Materials

        Color color = importantStars.color;
        color.a = 0f;
        importantStars.color = color;
        materialPolarstern.color = color;

        Color traceN05BSColor = traceN05BS.color;
        traceN05BSColor.a = 0.1f;
        traceN05BS.color = traceN05BSColor;

        Color starsBS00 = matStar_Breitengradsterne.color;
        starsBS00.a = 0.08f;
        matStar_Breitengradsterne.color = starsBS00;

        Color starsUnlit = matStarUnlit.color;
        starsUnlit.a = 0.3f;
        matStarUnlit.color = starsUnlit;
        earthLowN05Mat.renderQueue = 3000;

        Color zeigerWhiteColor = zeigerWhite.color;
        zeigerWhiteColor.a = 1;
        zeigerWhite.color = zeigerWhiteColor;

        Color uhr24HColor = uhr24H.color;
        uhr24HColor.a = 0;
        ;
        uhr24H.color = uhr24HColor;
        uhr24HGameobject.SetActive(false);

        Color lg = gitternetzGrade.color;
        lg.a = 0;
        gitternetzGrade.color = lg;

        #endregion

        #region Reset Other

        ground.sharedMaterial = planeGround;
        gridGround.sortingOrder = 1;
        gridTransf.localScale = new Vector3(0.2253756f, 0.2253756f, 0.2253756f);

        zeigerWeissOnSphere.SetActive(false);
        outOfSphereBackground.localPosition = new Vector3(0, 1000, 28526);
        outOfSphereBackground.localScale = new Vector3(6000, 6500, 8069);

        #endregion

        #region Reset Starry Sky

        gitterNetz.SetActive(false);
        cylinderN05Input.SetActive(false);
        grosserWagenN05.SetActive(true);

        #endregion
    }

    //Wenn Camera aus Himmelskugel gefahren ist, dann die Breitengradsterne tracen
    public void StartTracingAfterZoomIsFinishIn0505() {
        Color starsBS07 = matStar_Breitengradsterne.color;
        starsBS07.a = 0.4f;
        matStar_Breitengradsterne.color = starsBS07;
        breitengradSterneHighlight.color = starsBS07;

        foreach (GameObject gO in traceParents2) {
            TrailController tr = (TrailController) gO.GetComponent(typeof(TrailController));
            tr.StartTracingN05();
        }

        pC.JumpToNextStep();
    }

    private IEnumerator WaitAndSlowDownSphereRotation() {
        yield return new WaitForSeconds(2f);
        stopSphereRotation = true;
    }

    public void TurnTraceLinesOff() {
        foreach (GameObject gO in traceParents) {
            TrailController tr = (TrailController) gO.GetComponent(typeof(TrailController));
            tr.TurnTraceOff();
        }

        foreach (GameObject gO in traceParents2) {
            TrailController tr = (TrailController) gO.GetComponent(typeof(TrailController));
            tr.TurnTraceOff();
        }
    }

    public void TurnTraceLinesOn() {
        foreach (GameObject gO in traceParents) {
            TrailController tr = (TrailController) gO.GetComponent(typeof(TrailController));
            tr.ShowTrace();
        }

        foreach (GameObject gO in traceParents2) {
            TrailController tr = (TrailController) gO.GetComponent(typeof(TrailController));
            tr.ShowTrace();
        }
    }

    public void AllowEmittigOfTrailRenderer() {
        foreach (GameObject gO in traceParents) {
            gO.GetComponent<TrailRenderer>().emitting = true;
        }

        foreach (GameObject gO in traceParents2) {
            gO.GetComponent<TrailRenderer>().emitting = true;
        }
    }

    private void CheckFPS() {
        if (deviceInfo.GetUnscaledTime() < 0.035f) {
            if (lowestURPA > 2) {
                lowestURPA = 4;
                uRPA.msaaSampleCount = 4;
            }
        } else if (deviceInfo.GetUnscaledTime() > 0.0450f) {
            lowestURPA = 0;
            uRPA.msaaSampleCount = 0;
        } else {
            lowestURPA = 2;
            uRPA.msaaSampleCount = 2;
        }
    }

    private IEnumerator WaitBeforeStartCamAnim() {
        yield return new WaitForSeconds(1.5f);
        cameraAnimator.Play("CameraMove_N05_InSphere", 0, 0);
        cameraAnimator.speed = 1;
        cameraAnimator.SetInteger("StateN05", 5);
    }
}