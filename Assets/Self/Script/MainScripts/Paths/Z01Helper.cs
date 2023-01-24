using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class Z01Helper : MonoBehaviour {
    #region public Variables

    //Scripts
    public SkyProfileEventHandler sPEH;
    public SkyRenderController sRC;
    public SkyTimeController skyTimeController;
    public DeviceInfo deviceInfo;
    public MainMenuController mMC;
    public ProcedureController pC;
    public RotateStarrSky rSS;
    public SpielstandLoader ssL;
    public Translator translator;

    //Other Objects
    public Animator cameraAnimator;
    public Animator starrySkyAnimator;

    public GameObject clockParentGO;
    public GameObject gameController;
    public GameObject gitterNetzObject;
    public GameObject grosserWagenZ01;
    public GameObject himmelsglobusOutline;
    public GameObject himmelsGlobusButton;
    public GameObject himmelsGlobus3dMainMenuSchloss;
    public GameObject himmelsglobusMarkerparent;
    public GameObject himmelsHorizontFront;
    public GameObject himmelsHorizontBack;
    public GameObject kreisBogenZ01;
    public GameObject mainCam;
    public GameObject northForOutofSphereGo;
    public GameObject sunObject;
    public GameObject zeigerHighlightWeiss;
    public List<GameObject> traceParents = new List<GameObject>();
    public List<GameObject> traceParents2 = new List<GameObject>();
    public List<GameObject> gitterNetzLaengengrade = new List<GameObject>();

    public Light moonDirectLight;
    public Light sunDirectLight;

    public Material bogenZ01;
    public Material cylinderAchseZ01;
    public Material earthMat;
    public Material gitternetzLaengenGrade;
    public Material importantStars;
    public Material importantStarsSchuettkantenStern;
    public Material materialPolarstern;
    public Material matStar_GW_N05;
    public Material matStar_Breitengradsterne;
    public Material matStar_BreitengradsterneHighlight;
    public Material matStarUnlit;
    public Material planeGround;
    public Material sunMaterial;
    public Material traceN05GW;
    public Material traceN05beliebige;

    public MeshRenderer ground;

    public SpriteRenderer gridGround;

    public TextMeshPro bogenGradZ01;

    public Transform earthTransform;
    public Transform gridTransf;
    public Transform groundPlane;
    public Transform outOfSphereBackground;
    public Transform rotationByDay;
    public Transform rotationByHour;
    public Transform skyCompassPivot;
    public Transform starrySkyCompassPivot;
    public Transform terrainCompassPivot;

    public UniversalRenderPipelineAsset uRPA;

    public Volume volume;

    #endregion

    #region private Variables

    private bool checkFrontBackTrace = false;
    private bool turnLaengengradeOn = false;
    private bool turnSunOn = false;
    private bool turnAxisAndAngleOn = false;
    private bool turnAxisAndAngleOff = false;

    private float m_unscaledDeltaTime = 0f;

    private int lowestURPA = 4;

    #endregion

    void Start() {
        InitZ05();
    }

    public void InitZ05() {
        ProcedureController.changeEvent += DoActionWhileStepUpdate;
    }

    public void DoActionWhileStepUpdate(string stepId) {

        switch (stepId) {
            case "Z01.00a":
            case "Z01.00":
                ResetScript();
                sPEH.StopUpdatingPropertiesAndTurnSunOffZ01();
                
                //1. Frame der Animation setzen und Animator anhalten
                cameraAnimator.enabled = true;
                cameraAnimator.SetInteger("StateZ01", 1);
                cameraAnimator.Play("CamMove_Z01", 0, 0);
                cameraAnimator.speed = 0;
                
                sunDirectLight.enabled = false;
                moonDirectLight.enabled = false;
                
                starrySkyAnimator.enabled = true;
                starrySkyAnimator.Rebind();
                starrySkyAnimator.Update(0f);
                StartCoroutine(WaitBeforeDeactivateSkyRenderer());
                break;

            case "Z01.05":
                CheckFPS();
                earthTransform.localPosition = new Vector3(0, -90, starrySkyCompassPivot.localPosition.z);

                //Sterne vom Großen Wagen erlauben zu zeichnen
                foreach (GameObject gO in traceParents) {
                    TrailController tr = (TrailController) gO.GetComponent(typeof(TrailController));
                    tr.StartTracingN05();
                }
                
                //Himmelskugel rotieren
                starrySkyAnimator.SetInteger("StateZ01", 1);
                starrySkyAnimator.Play("RotateStarrySkyInLoopZ01",0,0);
                starrySkyAnimator.speed = 1.0f;
                break;

            case "Z01.08a":
                CheckFPS();
                groundPlane.localPosition = new Vector3(0, 0, starrySkyCompassPivot.localPosition.z);
                earthTransform.localScale = new Vector3(4, 4, 4);
                gitterNetzObject.SetActive(true);
                
                //Kameraanimation beginnen, Fahrt bis zum Rand der Himmelskugel
                cameraAnimator.speed = 1;
                break;

            case "Z01.13":
                CheckFPS();
                checkFrontBackTrace = true;
                
                //Kamera auf die Seite rotieren, mit Blick nach Osten
                cameraAnimator.SetInteger("StateZ01", 2);
                cameraAnimator.Play("CameraMove_Z01_09", 0, 0);
                cameraAnimator.speed = 1;
                break;

            case "Z01.16":
                //Einblendung der Achse mit Gradzahl
                turnAxisAndAngleOn = true;
                break;

            case "Z01.16b":
                //Achse durch Himmelskugel mit Gradzahl ausblenden
                turnAxisAndAngleOff = true;
                
                //Sonne auf der Himmelskugel tracen
                turnSunOn = true;
                break;

            case "Z01.20":
                //Himmelsglobus freischalten, Schloss entfernen und neue Spielstand schreiben
                himmelsglobusOutline.SetActive(true);
                himmelsGlobusButton.SetActive(true);

                string filePath = Path.Combine(Application.persistentDataPath, "spielstand.txt");
                string jsonData = "2";
                File.WriteAllText(filePath, jsonData);
                ssL.FinishedPathPoint("Z01");

                if (himmelsGlobus3dMainMenuSchloss.activeSelf) {
                    mMC.UnlockHimmelsglobusInNextStep();
                }

                himmelsglobusMarkerparent.SetActive(true);
                break;
        }
    }

    private void ResetScript() {
        #region Reset Camera

        float latitude = skyTimeController.GetLatitude();
        latitude = -1.0f * latitude;
        float temp = latitude;

        float screenFactor = DeviceInfo.GetScreenfactor();
        float newZPos = (2.22f -  screenFactor) * 469.337f;
        starrySkyCompassPivot.transform.localPosition = new Vector3(0, 0, newZPos);

        float newXRot = temp + (2.22f - screenFactor) * 18.7536f;
        mainCam.transform.localEulerAngles = new Vector3(newXRot, 0, 0);

        starrySkyCompassPivot.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        skyCompassPivot.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        terrainCompassPivot.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

        cameraAnimator.SetInteger("StateZ01", 0);
        //mscGameObject.enabled = true;
        cameraAnimator.enabled = true;
        uRPA.msaaSampleCount = 2;

        #endregion

        #region Reset Date and Time

        skyTimeController.SetDate(System.DateTime.Now.Year, 2, 7);
        skyTimeController.SetTimeline(0);

        ClockController cc = (ClockController) clockParentGO.GetComponent(typeof(ClockController));
        cc.ShowRealSystemTime();

        cc.SetCurrentDate();
        cc.ChangeDateTextColor(1);
        cc.ChangeTimeTextColor(1);

        #endregion

        #region Reset Materials

        ground.sharedMaterial = planeGround;
        gridGround.sortingOrder = 1;

        earthMat.renderQueue = 3000;

        Color traceN05GWColor = traceN05GW.color;
        traceN05GWColor.a = 1;
        traceN05GW.color = traceN05GWColor;

        Color traceN05beliebigeColor = traceN05beliebige.color;
        traceN05beliebigeColor.a = 0.5f;
        traceN05beliebige.color = traceN05beliebige.color;

        Color matStar_BreitengradsterneHighlightColor = matStar_BreitengradsterneHighlight.color;
        matStar_BreitengradsterneHighlightColor.a = 0;
        matStar_BreitengradsterneHighlight.color = matStar_BreitengradsterneHighlightColor;

        Color starsBS00 = matStar_Breitengradsterne.color;
        starsBS00.a = 0f;
        matStar_Breitengradsterne.color = starsBS00;

        Color starsGWN05 = matStar_GW_N05.color;
        starsGWN05.a = 1;
        matStar_GW_N05.color = starsGWN05;

        Color color = importantStars.color;
        color.a = 0f;
        importantStars.color = color;
        importantStarsSchuettkantenStern.color = color;

        Color matImportantPolar = materialPolarstern.color;
        matImportantPolar.a = 1;
        materialPolarstern.color = matImportantPolar;

        Color starsUnlit = matStarUnlit.color;
        starsUnlit.a = 0.4f;
        matStarUnlit.color = starsUnlit;

        Color bogenZ01Color = bogenZ01.color;
        Color cylinderAchseZ01Color = cylinderAchseZ01.color;
        Color bogenGradColor = bogenGradZ01.faceColor;

        bogenZ01Color.a = 0f;
        bogenZ01.color = bogenZ01Color;

        cylinderAchseZ01Color.a = 0;
        cylinderAchseZ01.color = cylinderAchseZ01Color;

        bogenGradColor.a = 0;
        bogenGradZ01.color = bogenGradColor;

        Color sunColor = sunMaterial.color;
        sunColor.a = 0;
        sunMaterial.color = sunColor;

        #endregion

        #region Reset Other

        zeigerHighlightWeiss.SetActive(false);
        gridTransf.localScale = new Vector3(0.2253756f, 0.2253756f, 0.2253756f);

        kreisBogenZ01.SetActive(true);
        turnAxisAndAngleOn = false;
        turnAxisAndAngleOff = false;

        northForOutofSphereGo.SetActive(true);
        northForOutofSphereGo.transform.localPosition = new Vector3(0f, -0.62f, 8.82f);
        northForOutofSphereGo.transform.localEulerAngles = new Vector3(0, -90, 0);

        outOfSphereBackground.localPosition = new Vector3(0, 0, 28526);
        outOfSphereBackground.localScale = new Vector3(5000, 4000, 8069);
        grosserWagenZ01.SetActive(true);

        #endregion

        #region Reset private Variables

        checkFrontBackTrace = false;
        turnLaengengradeOn = false;
        turnSunOn = false;
        turnAxisAndAngleOn = false;
        turnAxisAndAngleOff = false;

        #endregion

        #region Reset Starry Sky

        Color lg = gitternetzLaengenGrade.color;
        lg.a = 0;
        gitternetzLaengenGrade.color = lg;

        foreach (GameObject gO in gitterNetzLaengengrade) {
            gO.GetComponent<MeshRenderer>().sharedMaterial = gitternetzLaengenGrade;
        }

        foreach (GameObject gO in traceParents) {
            TrailController tr = (TrailController) gO.GetComponent(typeof(TrailController));
            tr.TurnTraceOff();
        }

        foreach (GameObject gO in traceParents2) {
            TrailController tr = (TrailController) gO.GetComponent(typeof(TrailController));
            tr.TurnTraceOff();
        }

        rSS.AllowCalculateRotationContinous();
        AllowEmittingOfTrailRenderer();

        TrailController tr3 = (TrailController) himmelsHorizontFront.GetComponent(typeof(TrailController));
        tr3.TurnTraceOff();

        TrailController tr4 = (TrailController) himmelsHorizontBack.GetComponent(typeof(TrailController));
        tr4.TurnTraceOff();
        TrailController tr5 = (TrailController) sunObject.GetComponent(typeof(TrailController));
        tr5.StopTracing();
        tr5.TurnTraceOff();

        #endregion
    }

    private void Update() {
        //Prioritäten der Materialien funktionieren nicht immer 100%, deswegen gibt es für den Himmelsaequator
        //zwei TrailRenderer, einer räumlich gesehen vor der Erde und einer dahinter
        if (checkFrontBackTrace) {
            float temp = rotationByDay.localEulerAngles.y + rotationByHour.localEulerAngles.y;
            if (temp > 235 && temp < 415) {
                himmelsHorizontBack.GetComponent<TrailRenderer>().emitting = true;
                himmelsHorizontFront.GetComponent<TrailRenderer>().emitting = false;
            } else {
                himmelsHorizontBack.GetComponent<TrailRenderer>().emitting = false;
                himmelsHorizontFront.GetComponent<TrailRenderer>().emitting = true;
            }
        }

        #region graphical Actions

        if (turnLaengengradeOn) {
            Color lg = gitternetzLaengenGrade.color;
            if (lg.a < 0.16f) {
                lg.a += 0.01f;
                gitternetzLaengenGrade.color = lg;
            } else {
                ProcedureController pC = (ProcedureController) gameController.GetComponent(typeof(ProcedureController));
                pC.JumpToNextStep();
                turnLaengengradeOn = false;
            }
        }

        if (turnSunOn) {
            Color sunColor = sunMaterial.color;
            if (sunColor.a < 1f) {
                sunColor.a += 0.05f;
                sunMaterial.color = sunColor;
            } else {
                StartCoroutine(TraceSun());
                turnSunOn = false;
            }
        }


        if (turnAxisAndAngleOn) {
            Color bogenZ01Color = bogenZ01.color;
            Color cylinderAchseZ01Color = cylinderAchseZ01.color;
            Color bogenGradColor = bogenGradZ01.faceColor;

            if (bogenZ01Color.a < 1) {
                bogenZ01Color.a += 0.075f;
                bogenZ01.color = bogenZ01Color;

                cylinderAchseZ01Color.a += 0.075f;
                cylinderAchseZ01.color = cylinderAchseZ01Color;

                bogenGradColor.a += 0.075f;
                bogenGradZ01.color = bogenGradColor;
            } else {
                bogenZ01Color.a = 1f;
                bogenZ01.color = bogenZ01Color;

                cylinderAchseZ01Color.a = 1;
                cylinderAchseZ01.color = cylinderAchseZ01Color;

                bogenGradColor.a = 1;
                bogenGradZ01.color = bogenGradColor;
                turnAxisAndAngleOn = false;
            }
        }

        if (turnAxisAndAngleOff) {
            Color bogenZ01Color = bogenZ01.color;
            Color cylinderAchseZ01Color = cylinderAchseZ01.color;
            Color bogenGradColor = bogenGradZ01.faceColor;

            if (bogenZ01Color.a > 0) {
                bogenZ01Color.a -= 0.075f;
                bogenZ01.color = bogenZ01Color;

                cylinderAchseZ01Color.a -= 0.075f;
                cylinderAchseZ01.color = cylinderAchseZ01Color;

                bogenGradColor.a -= 0.075f;
                bogenGradZ01.color = bogenGradColor;
            } else {
                bogenZ01Color.a = 0f;
                bogenZ01.color = bogenZ01Color;

                cylinderAchseZ01Color.a = 0;
                cylinderAchseZ01.color = cylinderAchseZ01Color;

                bogenGradColor.a = 0;
                bogenGradZ01.color = bogenGradColor;
                turnAxisAndAngleOff = false;
            }
        }

        #endregion
    }


    public void StopTracingGW() {
        StartCoroutine(CoroutineStopTracingGW());
    }

    private IEnumerator WaitBeforeDeactivateSkyRenderer() {
        yield return new WaitForSeconds(0.5f);

        Color starsUnlit = matStarUnlit.color;
        starsUnlit.a = 0.4f;
        matStarUnlit.color = starsUnlit;
        yield return new WaitForSeconds(1f);
        sRC.enabled = false;
    }

    #region TrailRenderer Handler

    //Trace beliebige Sterne, die die Himmelskugel bilden
    public void StartTraceBeliebige() {
        foreach (GameObject gO in traceParents2) {
            TrailController tr = (TrailController) gO.GetComponent(typeof(TrailController));
            tr.StartTracingN05();
        }
    }

    //Trace die Sonne auf der Himmelskugel
    private IEnumerator TraceSun() {
        TrailController tr2 = (TrailController) sunObject.GetComponent(typeof(TrailController));
        tr2.StartTracing();
        yield return new WaitForEndOfFrame();
    }

    //Trace den Himmelsaequator auf der Himmelskugel
    public void ContinueAndTraceHimmelsAequator() {
        TrailController tr3 = (TrailController) himmelsHorizontFront.GetComponent(typeof(TrailController));
        tr3.StartTracingN05();

        TrailController tr4 = (TrailController) himmelsHorizontBack.GetComponent(typeof(TrailController));
        tr4.StartTracingN05();
    }

    //Blende alle getracten Linien aus
    public void TurnAllTrailLinesOff() {
        TrailController tr2 = (TrailController) sunObject.GetComponent(typeof(TrailController));
        tr2.TurnTraceOff();

        foreach (GameObject gO in traceParents) {
            TrailController tr = (TrailController) gO.GetComponent(typeof(TrailController));
            tr.TurnTraceOff();
        }

        foreach (GameObject gO in traceParents2) {
            TrailController tr = (TrailController) gO.GetComponent(typeof(TrailController));
            tr.TurnTraceOff();
        }

        TrailController tr3 = (TrailController) himmelsHorizontFront.GetComponent(typeof(TrailController));
        tr3.TurnTraceOff();

        TrailController tr4 = (TrailController) himmelsHorizontBack.GetComponent(typeof(TrailController));
        tr4.TurnTraceOff();
    }

    //Stoppe das Zeichnen der TrailRenderer
    public void StopEmittingOfTrailRenderer() {
        foreach (GameObject gO in traceParents) {
            gO.GetComponent<TrailRenderer>().emitting = false;
        }

        foreach (GameObject gO in traceParents2) {
            gO.GetComponent<TrailRenderer>().emitting = false;
        }
    }

    //Erlaube allen TrailRenderer zu zeichenen
    public void AllowEmittingOfTrailRenderer() {
        foreach (GameObject gO in traceParents) {
            gO.GetComponent<TrailRenderer>().emitting = true;
        }

        foreach (GameObject gO in traceParents2) {
            gO.GetComponent<TrailRenderer>().emitting = true;
        }

        sunObject.GetComponent<TrailRenderer>().emitting = true;
    }

//Stoppt das Zeichnen des Großen Wagens wenn die Himmelskugel einmal komplett rotiert wurde
    private IEnumerator CoroutineStopTracingGW() {
        yield return new WaitForSeconds(0.25f);
        foreach (GameObject gO in traceParents) {
            gO.GetComponent<TrailRenderer>().emitting = false;
        }

        pC.JumpToNextStep();
    }

    #endregion

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
}