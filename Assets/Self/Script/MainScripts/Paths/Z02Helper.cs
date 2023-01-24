using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.UI;

public class Z02Helper : MonoBehaviour {
#region public Variables

    public SpielstandLoader ssL;
    public SkyTimeController skyTimeController;
    public SkyProfileEventHandler sPEH;
    public ProcedureController pC;
    public SkyCamHelperForZ02 sCHFZ02;
    public HimmelsglobusController hgc;
    public HimmelsglobusRotation hGR;
    public RotateStarrSky rSS;

    public Animator gameControllerAnimator;
    public Animator sunDialAnimator;
    public Animator himmelsglobusAnimator;
    public Animator skyAnimator;
    public Animator skyCameraAnimator;
    public Animator mainCamPivotAnimator;

    public Button okButton;

    public Camera skyCam;

    public GameObject earth;
    public GameObject gitternetzKugel;
    public GameObject hgParent;
    public GameObject sunDialParent;
    public GameObject sunDialObject;
    public GameObject hgCamera;
    public GameObject himmelsglobusobject;
    public GameObject directions;
    public GameObject cylinderZ02;
    public GameObject markerParent;
    public GameObject hgHimmelsAequatorHighlight;
    public GameObject hgArmEkliptikhighlight;
    public GameObject northForOutofSphereGo;
    public GameObject clockParentGO;

    public Light extraLight;
    public Light globusLight;
    public Light erdglobusLight;

    public Material polarStarLineTrans;
    public Material earthMaterial;
    public Material gitternetzKugelMat;
    public Material hgHimmelskugelSonne;
    public Material hgHimmelskugel;
    public Material hgHimmelskugelOpaque;
    public Material matStarUnlit;
    public Material sunMat;
    public Material sunDialToPolarstarLine;
    public Material planeHorizont;
    public Material planeGroundPrio4;
    public Material hgHorizont;
    public Material hgHorizontOpaque;
    public Material hgHorizont2;
    public Material hgHorizont2Opaque;
    public Material hgMeridian;
    public Material hgMeridianOpaque;
    public Material hg24Uhr;
    public Material hg24Uhr_2;
    public Material hg24UhrOpaque;
    public Material hg24UhrOpaque_2;
    public Material hgEkliptik;
    public Material hgEkliptikOpaque;
    public Material hgGold;
    public Material hgGoldOpaque;
    public Material hgGold24UhrBody;
    public Material hgGold24UhrBodyOpaque;
    public Material hgGoldSonne;
    public Material hgGoldSonneOpaque;
    public Material hgUhrUnten;
    public Material hgUhrUntenOpaque;
    public Material bogenZ02;
    public Material hgMeridianHighlightDresden;
    public Material hgMoonGlowHalf;
    public Material hgTransparentArmillar;
    public Material hgTransparentArmillarHorizont;
    public Material hgTransparentArmillarErde;
    public Material hgTransparentArmillarEkliptik;
    public Material hgArmGoldOpaque;
    public Material hgArmillarHimmelsaequator;
    public Material hgArmEkliptikOpaque;
    public Material hgArmHorizontOpaque;
    public Material mondscheibeMatOpaque;
    public Material hgArmHighlight;
    public Material hgBeineGoldTrans;
    public Material hgBeineflachesObjektTrans;
    public Material hgDrachenfluegelTrans;
    public Material hgDrachenkopfTrans;
    public Material hgBeineGoldOpaque;
    public Material hgBeineflachesObjektOpaque;
    public Material hgDrachenfluegelOpaque;
    public Material hgDrachenkopfOpaque;
    public Material hgSaeuleGoldOpaque;
    public Material hgSaeulaGoldTrans;
    public Material hgEGHorizontOpaque;
    public Material hgEGHorizontTrans;
    public Material hgEGMeridianOpaque;
    public Material hgEGMeridianTrans;
    public Material erdglobusHighlightMat;
    public Material hgHidePlane;

    public MeshRenderer cylinderZ02MR;
    public MeshRenderer hgHimmelskugelMR;
    public MeshRenderer planeGround;
    public MeshRenderer hgHorizontMR;
    public MeshRenderer hgHorizontMR2;
    public MeshRenderer hgMeridianMR;
    public MeshRenderer hg24UhrMR;
    public MeshRenderer hg24UhrMR_2;
    public MeshRenderer hgSonnenbahnMR;
    public MeshRenderer hgMondbahnMR;
    public MeshRenderer hgGoldMR;
    public MeshRenderer hgGold24UhrBodyMR;
    public MeshRenderer hgGoldSonneMR;
    public MeshRenderer hgUhrUntenMR;
    public MeshRenderer hgMondMR;
    public MeshRenderer hgArmillarDeckel;
    public MeshRenderer hgArmillarEkliptik;
    public MeshRenderer hgArmillarHalter1;
    public MeshRenderer hgArmillarHalter003;
    public MeshRenderer hgArmillarHorizont;
    public MeshRenderer hgArmillarMeridian;
    public MeshRenderer hgArmillarSegel;
    public MeshRenderer hgArmillarRotationsachse;
    public MeshRenderer hgArmillarErde;
    public MeshRenderer hgArmHimmelsaequatorMR;
    public MeshRenderer hgMondscheibeMr;
    public MeshRenderer hgBeineMR;
    public MeshRenderer hgEgSaeule01MR;
    public MeshRenderer hgEgSaeule02MR;
    public MeshRenderer hgEgSaeule03MR;
    public MeshRenderer hgEgSaeule04MR;
    public MeshRenderer hgEgHorizontMR;
    public MeshRenderer hgEGMeridianMR;

    public Texture emptyHighlightTexture;
    public Texture arktisTexture;
    public Texture antarktisTexture;
    public Texture australienTexture;
    public Texture alaskaTexture;
    public Texture japanTexture;
    public Texture hispaniaMaiorTexture;
    public Texture hispaniaNovaTexture;

    public TextMeshPro bogenGrad;
    public TextMeshPro hgMeridianDresdenGrad;

    public Transform mainCamera;
    public Transform polarStar;
    public Transform sunlightPivot;
    public Transform starrySkyParent;
    public Transform fadenStartPoint;
    public Transform sunDial;
    public Transform meridianHighlightDresden;
    public Transform meridianHighlightOslo;
    public Transform meridianHighlightAlexandria;
    public Transform erdglobus;
    public Transform outOfSphereBackground;

    public TrailRenderer sunTR;

    public SpriteRenderer gridGround;
    public SpriteRenderer moonSphereHighlight;

    public Volume volume;

    public Color hgHorizontHighlightColor = new Color(0.49f, 0.3215f, 0, 1);
    public Color hgMoonGlowHalfColor = new Color(0.49f, 0.3215f, 0, 1);
    public Color hgSunColor = new Color(0.49f, 0.3215f, 0, 1);
    public Color hgArmAequatorColor = new Color(0.49f, 0.3215f, 0, 1);
    public Color hgArmEkliptikColor = new Color(0.49f, 0.3215f, 0, 1);
    public Color hgArmHorizontColor = new Color(0.49f, 0.3215f, 0, 1);
    public Color erdglobusHighlightColor = new Color(1, 1, 1, 0);

    public float alphaHGArmHighlight = 0.0f;

#endregion

#region private Variables

    private bool showGitterNetz = false;
    private bool showGitterNetzFull = false;
    private bool showHGHimmelskugel = false;
    private bool showHGHimmelskugelFull = false;
    private bool showHGHimmelskugelSonne = false;
    private bool turnSunLineOnGlobusOff = false;
    private bool turnGlobusLightOn = false;
    private bool turnStarsOn = false;
    private bool turnStarsOff = false;
    private bool turnHgHorizontOn = false;
    private bool turnStep3On = false;
    private bool turnPlaneHorizontOff = false;
    private bool showHimmelskugelAnimateTime = false;
    private bool turnGitterNetzOff = false;
    private bool showBogen = false;
    private bool turnBogenOff = false;
    private bool turnCylinderOff = false;
    private bool highlightHorizont = false;
    private bool flashHighlightMeridianDresden = false;
    private bool highlightMeridianDresdenOn = false;
    private bool highlightMeridianDresdenOff = false;
    private bool showHgMeridianDresdenText = false;
    private bool turnHgMeridianDresdenTextOff = false;
    private bool turnMeridianDresdenOff = false;
    private bool turnMeridianOsloOff = false;
    private bool turnMeridianAlexandriaOff = false;
    private bool flashHighlightMoonSphere = false;
    private bool highlightMoonSphereOn = false;
    private bool highlightMoonSphereOff = false;
    private bool turnhighlightMoonSphereOff = false;
    private bool highlightSundAndMoonhalf = false;
    private bool turnArmillarOff = false;
    private bool makehgHimmelskugelTransparent = false;
    private bool highlightHimmelsaequator = false;
    private bool showArmEkliptik = false;
    private bool highlightArmEkliptik = false;
    private bool highlightArmHorizont = false;
    private bool showhorizontArm = false;
    private bool turnGestellForErdglobusOff = false;
    private bool rotateEgToTarget = false;
    private bool turnEgLightOn = false;
    private bool turnEgLightOff = false;
    private bool highlightErdglobus = false;
    private bool highlightEGLila = false;

    private float timeCount = 0.0f;
    private float t = 0.0f;

    private LensDistortion lD;
    private Quaternion targetRotation;

    private Vector2 polarScreenPos;
    private Vector2 schuettkantensternScreenPos;
    private Vector3 startRotation;

#endregion

    void Start() {
        InitZ02();
    }

    private void InitZ02() {
        LensDistortion tmp;

        if (volume.profile.TryGet(out tmp)) {
            lD = tmp;
        }

        ProcedureController.changeEvent += DoActionWhileStepUpdate;
        skyTimeController.SetLatitude(48.0f);
    }

    private void ResetScript() {
    #region Reset private Variables

        showGitterNetzFull = false;
        turnMeridianOsloOff = false;
        turnMeridianDresdenOff = false;
        showHgMeridianDresdenText = false;
        turnHgMeridianDresdenTextOff = false;
        turnMeridianAlexandriaOff = false;
        showBogen = false;
        highlightHorizont = false;
        flashHighlightMeridianDresden = false;
        highlightMeridianDresdenOn = false;
        highlightMeridianDresdenOff = false;
        highlightHimmelsaequator = false;
        showArmEkliptik = false;
        highlightArmEkliptik = false;
        highlightArmHorizont = false;
        showhorizontArm = false;
        rotateEgToTarget = false;
        flashHighlightMoonSphere = false;
        highlightMoonSphereOn = false;
        highlightMoonSphereOff = false;
        turnhighlightMoonSphereOff = false;
        highlightSundAndMoonhalf = false;
        turnEgLightOn = false;
        turnEgLightOff = false;
        highlightErdglobus = false;

    #endregion

    #region Reset Camera

        float resolutionFactor = DeviceInfo.GetResolutionFactor();
        float ldIntentisdyFactor = 0.625f - resolutionFactor;
        lD.intensity.Override(ldIntentisdyFactor);
        sCHFZ02.enabled = true;
        sCHFZ02.StopChangingVariablesWithAnim();
        hgCamera.SetActive(false);

        skyCameraAnimator.enabled = true;
        skyCameraAnimator.Rebind();
        skyCameraAnimator.Play("CameraInit_Z02", 0, 0);
        skyCameraAnimator.SetInteger("CameraMoveZ02", 1);
        skyCameraAnimator.speed = 0;

        mainCamera.localEulerAngles = new Vector3(0, 0, 0);
        lD.scale.Override(1.04f);
        sCHFZ02.enabled = true;

    #endregion

    #region Reset Date and Time

        skyTimeController.SetDate(2021, 3, 21);
        skyTimeController.SetTimeline(6);
        skyTimeController.SetLatitude(48);
        skyTimeController.SetUtc(1);
        skyTimeController.StopIncreaseDayWitAnim();
        skyTimeController.SetAllowSetTimeLineWithAnim();

        ClockController cc = (ClockController) clockParentGO.GetComponent(typeof(ClockController));
        cc.StopShowRealSystemTime();

    #endregion

    #region Reset Materials

        planeGround.sharedMaterial = planeGroundPrio4;
        gridGround.sortingOrder = 1;

        Color earthColor = earthMaterial.color;
        earthColor.a = 1;
        earthMaterial.color = earthColor;

        cylinderZ02MR.sharedMaterial = sunDialToPolarstarLine;
        Color sunDialToPolarstarLineColor = sunDialToPolarstarLine.color;
        sunDialToPolarstarLineColor.a = 1;
        sunDialToPolarstarLine.color = sunDialToPolarstarLineColor;

        Color polarStarLineTransColor = polarStarLineTrans.color;
        polarStarLineTransColor.a = 1;
        polarStarLineTrans.color = polarStarLineTransColor;

        Color gitternetzKugelColor = gitternetzKugelMat.color;
        gitternetzKugelColor.a = 0.0f;
        gitternetzKugelMat.color = gitternetzKugelColor;

        gitternetzKugel.SetActive(true);

        showHimmelskugelAnimateTime = false;
        sunDialAnimator.enabled = false;

        Color hgArmillarTransparentColor = hgTransparentArmillar.color;
        hgArmillarTransparentColor.a = 0;
        hgTransparentArmillar.color = hgArmillarTransparentColor;
        hgTransparentArmillarHorizont.color = hgArmillarTransparentColor;
        hgTransparentArmillarErde.color = hgArmillarTransparentColor;
        hgTransparentArmillarEkliptik.color = hgArmillarTransparentColor;
        hgArmillarHimmelsaequator.color = hgArmillarTransparentColor;
        hgSaeulaGoldTrans.color = hgArmillarTransparentColor;
        hgEGHorizontTrans.color = hgArmillarTransparentColor;
        hgBeineGoldTrans.color = hgArmillarTransparentColor;
        hgBeineflachesObjektTrans.color = hgArmillarTransparentColor;
        hgDrachenfluegelTrans.color = hgArmillarTransparentColor;
        hgDrachenkopfTrans.color = hgArmillarTransparentColor;
        hgEGMeridianTrans.color = hgArmillarTransparentColor;

        hgArmHimmelsaequatorMR.sharedMaterial = hgTransparentArmillar;
        hgArmillarDeckel.sharedMaterial = hgTransparentArmillar;
        hgArmillarEkliptik.sharedMaterial = hgTransparentArmillar;
        hgArmillarHalter1.sharedMaterial = hgTransparentArmillar;
        hgArmillarHalter003.sharedMaterial = hgTransparentArmillar;
        hgArmillarHorizont.sharedMaterial = hgTransparentArmillar;
        hgArmillarMeridian.sharedMaterial = hgTransparentArmillar;
        hgArmillarSegel.sharedMaterial = hgTransparentArmillar;
        hgArmillarRotationsachse.sharedMaterial = hgTransparentArmillar;
        hgArmillarErde.sharedMaterial = hgTransparentArmillar;
        turnGestellForErdglobusOff = false;
        startRotation = new Vector3(309.8f, 86.4f, 26.6f);
        erdglobus.transform.localEulerAngles = startRotation;
        erdglobusLight.intensity = 0;
        erdglobusLight.enabled = false;
        erdglobusHighlightMat.mainTexture = emptyHighlightTexture;
        erdglobusHighlightColor = new Color(1, 1, 1, 0);
        highlightEGLila = false;

        Color hgHidePlaneColor = hgHidePlane.color;
        hgHidePlaneColor.a = 0f;
        hgHidePlane.color = hgHidePlaneColor;

    #endregion

    #region Reset Other

        meridianHighlightDresden.localScale = new Vector3(1, 1, 1);
        meridianHighlightOslo.localScale = new Vector3(0, 0, 0);
        meridianHighlightAlexandria.localScale = new Vector3(0, 0, 0);
        extraLight.enabled = true;

        sunDialParent.SetActive(true);
        sunDialObject.SetActive(true);

        gameControllerAnimator.enabled = true;
        gameControllerAnimator.Rebind();
        gameControllerAnimator.Update(0f);

        northForOutofSphereGo.SetActive(true);
        northForOutofSphereGo.transform.localPosition = new Vector3(0.7275f, -0.62f, 2.288f);
        northForOutofSphereGo.transform.localEulerAngles = new Vector3(0, 90, 0);

        outOfSphereBackground.localPosition = new Vector3(0, 0, 28526);
        outOfSphereBackground.localScale = new Vector3(4000, 6000, 8069);

    #endregion

    #region Reset Starry Sky

        rSS.AllowCalculateRotationContinous();
        skyAnimator.enabled = true;
        skyAnimator.Rebind();
        skyAnimator.Play("AnimateTimelineFrom6", 0, 0);
        skyAnimator.SetInteger("StartAnimateTimeline", 11);
        skyAnimator.speed = 0;

    #endregion

    #region Reset Himmelsglobus

        himmelsglobusobject.SetActive(true);
        hgc.StopMoveToTarget();
        hgc.ResetHimmelsglobus();
        himmelsglobusAnimator.enabled = true;
        himmelsglobusAnimator.Rebind();
        himmelsglobusAnimator.Update(0f);

        cylinderZ02.SetActive(true);
        makehgHimmelskugelTransparent = false;

        hGR.StopRotation();
        hGR.StopMoveCamFOVWithAnim();

    #endregion

    #region Reset Sundial

        sunDialParent.transform.localPosition = new Vector3(0, -7.2f, 0);
        sunTR.enabled = true;
        sunTR.time = 0;

        sunlightPivot.localPosition = new Vector3(0, 0, starrySkyParent.transform.localPosition.z);
        sunDial.localEulerAngles = new Vector3(0, 178f, 0);
        sunDial.localPosition = new Vector3(-2, -1.1f, 55f);

    #endregion
    }

    public void DoActionWhileStepUpdate(string stepId) {
        switch (stepId) {
            case "Z02.00":
            case "Z02.00a":
                ResetScript();
                skyTimeController.SetLatitude(48);
                sPEH.OnlyAllowUpdateSun();
                pC.JumpToNextStep();
                break;

            case "Z02.02":
                skyTimeController.SetLatitude(48);
                sunTR.enabled = false;
                sPEH.StopUpdatingProperties();
                break;

            case "Z02.04":
                skyCameraAnimator.speed = 1.0f;
                skyCameraAnimator.SetInteger("CameraMoveZ02", 2);
                break;

            case "Z02.06":
                skyCameraAnimator.Play("RotateCamToSouth", 0, 0);
                skyCameraAnimator.SetInteger("CameraMoveZ02", 3);
                sunTR.enabled = true;
                sunTR.time = 0;
                break;

            case "Z02.08":
                directions.SetActive(false);
                skyAnimator.speed = 1;
                break;

            case "Z02.09":
                showBogen = true;
                turnBogenOff = false;
                break;

            case "Z02.10":
                turnBogenOff = true;
                showBogen = false;
                break;

            case "Z02.11":
                hgParent.SetActive(true);
                turnGlobusLightOn = true;
                showHGHimmelskugel = true;
                break;

            case "Z02.12":
                skyAnimator.enabled = false;
                sCHFZ02.StopChangingVariablesWithAnim();
                himmelsglobusAnimator.Play("ShowHimmelsglobus", 0, 0);
                himmelsglobusAnimator.SetInteger("AnimationState", 1);

                turnStarsOff = true;
                turnPlaneHorizontOff = true;
                northForOutofSphereGo.SetActive(false);
                earth.transform.localScale = new Vector3(0, 0, 0);
                break;

            case "Z02.13":
                cylinderZ02MR.sharedMaterial = polarStarLineTrans;
                turnCylinderOff = true;
                showHGHimmelskugelFull = true;

                showGitterNetz = false;
                turnGitterNetzOff = true;

                ClockController cc = (ClockController) clockParentGO.GetComponent(typeof(ClockController));
                cc.SetCurrentDate();
                cc.ShowRealSystemTime();
                cc.ChangeDateTextColor(1);
                cc.ChangeTimeTextColor(1);
                break;

            case "Z02.14a":
                ssL.FinishedPathPoint("Z02");
                Color gitternetzKugelColor = gitternetzKugelMat.color;
                gitternetzKugelColor.a = 0f;
                gitternetzKugelMat.color = gitternetzKugelColor;
                markerParent.SetActive(true);
                hGR.enabled = true;
                hGR.InitCamPos();
                hGR.AllowRotating();

                Color hgArmillarTransparentColor = hgTransparentArmillar.color;
                hgArmillarTransparentColor.a = 1;
                hgTransparentArmillar.color = hgArmillarTransparentColor;
                hgTransparentArmillarHorizont.color = hgArmillarTransparentColor;
                hgTransparentArmillarErde.color = hgArmillarTransparentColor;
                hgTransparentArmillarEkliptik.color = hgArmillarTransparentColor;
                hgArmillarHimmelsaequator.color = hgArmillarTransparentColor;
                hgSaeulaGoldTrans.color = hgArmillarTransparentColor;
                hgEGHorizontTrans.color = hgArmillarTransparentColor;
                hgBeineGoldTrans.color = hgArmillarTransparentColor;
                hgBeineflachesObjektTrans.color = hgArmillarTransparentColor;
                hgDrachenfluegelTrans.color = hgArmillarTransparentColor;
                hgDrachenkopfTrans.color = hgArmillarTransparentColor;
                hgEGMeridianTrans.color = hgArmillarTransparentColor;

                hgArmHimmelsaequatorMR.sharedMaterial = hgTransparentArmillar;
                hgArmillarDeckel.sharedMaterial = hgTransparentArmillar;
                hgArmillarEkliptik.sharedMaterial = hgTransparentArmillar;
                hgArmillarHalter1.sharedMaterial = hgTransparentArmillar;
                hgArmillarHalter003.sharedMaterial = hgTransparentArmillar;
                hgArmillarHorizont.sharedMaterial = hgTransparentArmillar;
                hgArmillarMeridian.sharedMaterial = hgTransparentArmillar;
                hgArmillarSegel.sharedMaterial = hgTransparentArmillar;
                hgArmillarRotationsachse.sharedMaterial = hgTransparentArmillar;
                hgArmillarErde.sharedMaterial = hgTransparentArmillar;

                break;

            //step after minipath to reset some variables
            case "Z02.15":
                mainCamPivotAnimator.enabled = false;
                himmelsglobusAnimator.enabled = false;
                markerParent.SetActive(true);
                hGR.enabled = true;
                hGR.AllowRotating();
                flashHighlightMeridianDresden = false;
                highlightMeridianDresdenOn = false;
                highlightMeridianDresdenOff = false;
                turnMeridianAlexandriaOff = true;
                turnHgMeridianDresdenTextOff = true;
                turnStarsOn = false;
                turnStarsOff = true;
                starrySkyParent.localEulerAngles = new Vector3(0, 0, 0);
                hgc.StopRotatioWithMeridian();
                StartCoroutine(ShowHgMarker());
                hGR.StopMoveCamFOVWithAnim();
                gameControllerAnimator.Rebind();
                gameControllerAnimator.Update(0f);
                gameControllerAnimator.SetInteger("hgAnimationState", 0);
                gameControllerAnimator.SetInteger("hgAnimationState", 0);
                mainCamPivotAnimator.enabled = false;
                hgArmHimmelsaequatorMR.sharedMaterial = hgArmGoldOpaque;
                hgArmillarDeckel.sharedMaterial = hgArmGoldOpaque;
                hgArmillarEkliptik.sharedMaterial = hgArmGoldOpaque;
                hgArmillarHalter1.sharedMaterial = hgArmGoldOpaque;
                hgArmillarHalter003.sharedMaterial = hgArmGoldOpaque;
                hgArmillarHorizont.sharedMaterial = hgArmGoldOpaque;
                hgArmillarMeridian.sharedMaterial = hgArmGoldOpaque;
                hgArmillarSegel.sharedMaterial = hgArmGoldOpaque;
                hgArmillarRotationsachse.sharedMaterial = hgArmGoldOpaque;
                hgArmillarErde.sharedMaterial = hgArmGoldOpaque;
                hgMondscheibeMr.sharedMaterial = mondscheibeMatOpaque;
                hgHorizontMR.sharedMaterial = hgHorizontOpaque;
                hgHorizontMR2.sharedMaterial = hgHorizont2Opaque;
                hGR.StopSetCamRectHeight();
                Material[] sharedMaterialsCopy = hgBeineMR.sharedMaterials;
                sharedMaterialsCopy[0] = hgBeineGoldOpaque;
                sharedMaterialsCopy[1] = hgDrachenfluegelOpaque;
                sharedMaterialsCopy[2] = hgDrachenkopfOpaque;
                sharedMaterialsCopy[3] = hgBeineflachesObjektOpaque;
                hgBeineMR.sharedMaterials = sharedMaterialsCopy;

                hgEgHorizontMR.sharedMaterial = hgEGHorizontOpaque;
                hgEGMeridianMR.sharedMaterial = hgEGMeridianOpaque;
                hgEgSaeule01MR.sharedMaterial = hgSaeuleGoldOpaque;
                hgEgSaeule02MR.sharedMaterial = hgSaeuleGoldOpaque;
                hgEgSaeule03MR.sharedMaterial = hgSaeuleGoldOpaque;
                hgEgSaeule04MR.sharedMaterial = hgSaeuleGoldOpaque;

                hgArmillarTransparentColor = hgTransparentArmillar.color;
                hgArmillarTransparentColor.a = 1;
                hgTransparentArmillar.color = hgArmillarTransparentColor;
                hgTransparentArmillarHorizont.color = hgArmillarTransparentColor;
                hgTransparentArmillarErde.color = hgArmillarTransparentColor;
                hgTransparentArmillarEkliptik.color = hgArmillarTransparentColor;
                hgArmillarHimmelsaequator.color = hgArmillarTransparentColor;
                hgSaeulaGoldTrans.color = hgArmillarTransparentColor;
                hgEGHorizontTrans.color = hgArmillarTransparentColor;
                hgBeineGoldTrans.color = hgArmillarTransparentColor;
                hgBeineflachesObjektTrans.color = hgArmillarTransparentColor;
                hgDrachenfluegelTrans.color = hgArmillarTransparentColor;
                hgDrachenkopfTrans.color = hgArmillarTransparentColor;
                hgEGMeridianTrans.color = hgArmillarTransparentColor;

                hgArmHimmelsaequatorMR.sharedMaterial = hgTransparentArmillar;
                hgArmillarDeckel.sharedMaterial = hgTransparentArmillar;
                hgArmillarEkliptik.sharedMaterial = hgTransparentArmillar;
                hgArmillarHalter1.sharedMaterial = hgTransparentArmillar;
                hgArmillarHalter003.sharedMaterial = hgTransparentArmillar;
                hgArmillarHorizont.sharedMaterial = hgTransparentArmillar;
                hgArmillarMeridian.sharedMaterial = hgTransparentArmillar;
                hgArmillarSegel.sharedMaterial = hgTransparentArmillar;
                hgArmillarRotationsachse.sharedMaterial = hgTransparentArmillar;
                hgArmillarErde.sharedMaterial = hgTransparentArmillar;

                showGitterNetz = false;
                showGitterNetzFull = false;
                turnGitterNetzOff = true;

                highlightErdglobus = false;
                erdglobusHighlightMat.mainTexture = emptyHighlightTexture;

                Color hgHidePlaneColor = hgHidePlane.color;
                hgHidePlaneColor.a = 0f;
                hgHidePlane.color = hgHidePlaneColor;
                hgc.SetMarkerActiveFalse();
                hGR.ForceCloseSplitScreen();
                break;

            case "H02.01":
                hGR.StopRotation();
                hgc.StopMoveToTarget();
                markerParent.SetActive(false);
                turnStarsOn = true;
                himmelsglobusAnimator.Rebind();
                himmelsglobusAnimator.Update(0f);
                pC.JumpToNextStep();
                break;

            case "H02.03":
                hgMeridianDresdenGrad.text = "51°";
                meridianHighlightDresden.localScale = new Vector3(1, 1, 1);
                meridianHighlightOslo.localScale = new Vector3(0, 0, 0);
                meridianHighlightAlexandria.localScale = new Vector3(0, 0, 0);
                highlightHorizont = true;
                gameControllerAnimator.enabled = true;
                gameControllerAnimator.Rebind();
                gameControllerAnimator.Update(0f);
                gameControllerAnimator.Play("HighlightHGHorizontRing", 0, 0);
                gameControllerAnimator.SetInteger("hgAnimationState", 1);
                pC.JumpToNextStep();
                break;

            case "H02.05":
                highlightHorizont = false;
                hgHorizontOpaque.color = new Color(0.49f, 0.3215f, 0, 1);
                hgHorizont2Opaque.color = new Color(0.49f, 0.3215f, 0, 1);
                gameControllerAnimator.Rebind();
                gameControllerAnimator.Update(0f);
                gameControllerAnimator.SetInteger("hgAnimationState", 0);

                himmelsglobusAnimator.enabled = true;
                himmelsglobusAnimator.Play("HGRotateMeridian", 0, 0);
                himmelsglobusAnimator.SetInteger("AnimationState", 3);
                hgc.AllowRotatioWithMeridian();
                pC.JumpToNextStep();
                break;

            case "H02.08":
                gameControllerAnimator.Rebind();
                gameControllerAnimator.Update(0f);

                himmelsglobusAnimator.Rebind();
                himmelsglobusAnimator.Update(0f);

                flashHighlightMeridianDresden = true;
                highlightMeridianDresdenOn = true;
                highlightMeridianDresdenOff = false;
                showHgMeridianDresdenText = true;
                pC.JumpToNextStep();
                break;

            case "H02.10":
                flashHighlightMeridianDresden = false;
                highlightMeridianDresdenOn = false;
                highlightMeridianDresdenOff = false;
                turnMeridianDresdenOff = true;
                turnHgMeridianDresdenTextOff = true;
                break;

            case "H02.11":
                meridianHighlightDresden.localScale = new Vector3(0, 0, 0);
                meridianHighlightOslo.localScale = new Vector3(1, 1, 1);
                flashHighlightMeridianDresden = true;
                highlightMeridianDresdenOn = true;
                highlightMeridianDresdenOff = false;
                hgMeridianDresdenGrad.text = "70°";
                turnHgMeridianDresdenTextOff = false;
                showHgMeridianDresdenText = true;
                break;

            case "H02.13":
                flashHighlightMeridianDresden = false;
                highlightMeridianDresdenOn = false;
                highlightMeridianDresdenOff = false;
                turnMeridianOsloOff = true;
                turnHgMeridianDresdenTextOff = true;
                break;

            case "H02.14":
                StartCoroutine(WaitForEndOfAnim());
                hgMeridianDresdenGrad.text = "20°";
                turnHgMeridianDresdenTextOff = false;
                showHgMeridianDresdenText = true;
                break;

            case "H02.15":
                himmelsglobusAnimator.enabled = false;
                break;

            case "H03.01":
                hGR.StopRotation();
                //mainCamera.localPosition = new Vector3(0, 0, 0);
                markerParent.SetActive(false);
                flashHighlightMoonSphere = true;
                highlightMoonSphereOn = true;
                highlightMoonSphereOff = false;
                pC.JumpToNextStep();
                break;

            case "H03.03":
                flashHighlightMoonSphere = false;
                highlightMoonSphereOn = false;
                turnhighlightMoonSphereOff = true;

                highlightSundAndMoonhalf = true;
                hgc.StopMoveToTarget();
                hGR.AllowMoveCamFOVWithAnim();
                mainCamPivotAnimator.enabled = true;
                mainCamPivotAnimator.Rebind();
                mainCamPivotAnimator.Play("HgMondMarkerShowSun", 0, 0);
                mainCamPivotAnimator.SetInteger("AnimationStateHGSun", 1);

                gameControllerAnimator.enabled = true;
                gameControllerAnimator.Rebind();
                gameControllerAnimator.Update(0f);
                gameControllerAnimator.Play("HighlightHGSunMoonHalf", 0, 0);
                gameControllerAnimator.SetInteger("hgAnimationState", 2);
                pC.JumpToNextStep();
                break;

            case "H03.05":
                himmelsglobusAnimator.enabled = false;
                mainCamPivotAnimator.enabled = false;
                gameControllerAnimator.enabled = false;
                break;

            case "H04.01":
                hgArmHimmelsaequatorMR.sharedMaterial = hgArmillarHimmelsaequator;
                hGR.StopRotation();
                hgc.StopMoveToTarget();
                hGR.AllowSetCamRectHeight();
                markerParent.SetActive(false);
                himmelsglobusAnimator.enabled = false;
                hgCamera.SetActive(true);
                mainCamPivotAnimator.enabled = true;
                mainCamPivotAnimator.Rebind();
                mainCamPivotAnimator.Play("HGShowSplitScreenArmilar", 0, 0);
                mainCamPivotAnimator.SetInteger("AnimationStateHGSun", 2);
                pC.JumpToNextStep();
                break;

            case "H04.04":
                hgArmillarDeckel.sharedMaterial = hgTransparentArmillar;
                hgArmillarEkliptik.sharedMaterial = hgTransparentArmillarEkliptik;
                hgArmillarHalter1.sharedMaterial = hgTransparentArmillar;
                hgArmillarHalter003.sharedMaterial = hgTransparentArmillar;
                hgArmillarHorizont.sharedMaterial = hgTransparentArmillarHorizont;
                hgArmillarMeridian.sharedMaterial = hgTransparentArmillar;
                hgArmillarSegel.sharedMaterial = hgTransparentArmillar;
                hgArmillarRotationsachse.sharedMaterial = hgTransparentArmillar;
                hgArmillarErde.sharedMaterial = hgTransparentArmillarErde;
                turnArmillarOff = true;
                gitternetzKugel.SetActive(true);
                showGitterNetzFull = true;
                break;

            case "H04.06":
                hgHimmelsAequatorHighlight.SetActive(true);
                highlightHimmelsaequator = true;
                gameControllerAnimator.enabled = true;
                gameControllerAnimator.Rebind();
                gameControllerAnimator.Update(0f);
                gameControllerAnimator.Play("HighlightHgHimmelsaequator", 0, 0);
                gameControllerAnimator.SetInteger("hgAnimationState", 3);
                pC.JumpToNextStep();
                break;

            case "H04.08":
                highlightHimmelsaequator = false;
                hgHimmelsAequatorHighlight.SetActive(false);
                gameControllerAnimator.Rebind();
                gameControllerAnimator.Update(0f);
                gameControllerAnimator.SetInteger("hgAnimationState", 0);
                hgArmillarHimmelsaequator.color = new Color(0.49f, 0.3215f, 0, 1);
                showArmEkliptik = true;
                pC.JumpToNextStep();
                break;

            case "H04.10":
                hgArmEkliptikhighlight.SetActive(false);
                gameControllerAnimator.Rebind();
                gameControllerAnimator.Update(0f);
                gameControllerAnimator.SetInteger("hgAnimationState", 0);
                highlightArmEkliptik = false;
                hgArmEkliptikOpaque.color = new Color(0.49f, 0.3215f, 0, 1);
                showhorizontArm = true;
                pC.JumpToNextStep();
                break;

            case "H04.12":
                gameControllerAnimator.enabled = false;
                gameControllerAnimator.Rebind();
                gameControllerAnimator.Update(0f);
                gameControllerAnimator.SetInteger("hgAnimationState", 0);
                highlightArmHorizont = false;
                hgArmHorizontOpaque.color = new Color(0.49f, 0.3215f, 0, 1);
                mainCamPivotAnimator.Rebind();
                mainCamPivotAnimator.Play("HGCloseSplitScreenArm", 0, 0);
                mainCamPivotAnimator.SetInteger("AnimationStateHGSun", 3);
                StartCoroutine(WaitForEndOfAnimBeforeJump());
                break;

            case "H05.01":
                hGR.AllowMoveCamFOVWithAnimErdglobus();
                hGR.StopRotation();
                hgc.StopMoveToTarget();
                erdglobusLight.enabled = true;
                turnEgLightOn = true;
                markerParent.SetActive(false);
                hgc.StopSetColorsWithAnim();
                Material[] sharedMaterialsCopyTrans = hgBeineMR.sharedMaterials;
                sharedMaterialsCopyTrans[0] = hgBeineGoldTrans;
                sharedMaterialsCopyTrans[1] = hgDrachenfluegelTrans;
                sharedMaterialsCopyTrans[2] = hgDrachenkopfTrans;
                sharedMaterialsCopyTrans[3] = hgBeineflachesObjektTrans;
                hgBeineMR.sharedMaterials = sharedMaterialsCopyTrans;

                hgEgHorizontMR.sharedMaterial = hgEGHorizontTrans;
                hgEGMeridianMR.sharedMaterial = hgEGMeridianTrans;
                hgEgSaeule01MR.sharedMaterial = hgSaeulaGoldTrans;
                hgEgSaeule02MR.sharedMaterial = hgSaeulaGoldTrans;
                hgEgSaeule03MR.sharedMaterial = hgSaeulaGoldTrans;
                hgEgSaeule04MR.sharedMaterial = hgSaeulaGoldTrans;
                turnGestellForErdglobusOff = true;
                mainCamPivotAnimator.enabled = true;
                mainCamPivotAnimator.Rebind();
                mainCamPivotAnimator.Play("HgErdglobusZoomCamera", 0, 0);
                mainCamPivotAnimator.SetInteger("AnimationStateHGSun", 4);
                highlightEGLila = false;
                erdglobusHighlightMat.mainTexture = arktisTexture;
                StartCoroutine(WaitForEndOfAnimBeforeRotateGlobe());
                pC.JumpToNextStep();
                break;

            case "H05.03":
                erdglobusHighlightMat.mainTexture = antarktisTexture;
                timeCount = 0;
                t = 0;
                targetRotation = Quaternion.Euler(344.9f, 106.6f, 185f);
                pC.JumpToNextStep();
                break;

            case "H05.07":
                timeCount = 0;
                t = 0;
                targetRotation = Quaternion.Euler(295f, 91.6f, 131.3f);
                highlightEGLila = true;
                erdglobusHighlightMat.mainTexture = australienTexture;
                break;

            case "H05.08":
                timeCount = 0;
                t = 0;
                targetRotation = Quaternion.Euler(291.2f, 300.1f, 348.3f);
                erdglobusHighlightMat.mainTexture = alaskaTexture;
                pC.JumpToNextStep();
                break;

            case "H05.11":
                erdglobusHighlightMat.mainTexture = hispaniaMaiorTexture;
                break;

            case "H05.12":
                erdglobusHighlightMat.mainTexture = hispaniaNovaTexture;
                break;

            case "H05.13":
                timeCount = 0;
                t = 0;
                targetRotation = Quaternion.Euler(305.1f, 288.1f, 328.6f);
                erdglobusHighlightMat.mainTexture = emptyHighlightTexture;
                pC.JumpToNextStep();
                break;
            case "H05.15":
                erdglobusHighlightMat.mainTexture = japanTexture;
                break;

            case "H05.16":
                timeCount = 0;
                t = 0;
                targetRotation = Quaternion.Euler(309.8f, 86.4f, 26.6f);
                StartCoroutine(WaitForEndOfRotation());
                break;
        }
    }

    void Update() {
        if (rotateEgToTarget) {
            timeCount = timeCount + Time.deltaTime;
            t = timeCount / 3.0f;
            t = (t * t * (3f - 2f * t)) / 10.0f;
            erdglobus.transform.rotation = Quaternion.Slerp(erdglobus.transform.rotation, targetRotation, t);
        }

    #region graphical Actions

        if (showHimmelskugelAnimateTime) {
            if (skyTimeController.animTime < 13.01f) {
                skyTimeController.animTime += 0.075f;
            } else {
                skyTimeController.animTime = 13.01f;
                showHimmelskugelAnimateTime = false;
            }
        }
        
        if (turnStep3On) {
            Color hgUhr24Color = hg24Uhr.color;
            Color hgMeridianColor = hgMeridian.color;
            Color hgEkliptikColor = hgEkliptik.color;
            Color hgGoldColor = hgGold.color;
            Color hg24UhrgoldBodyColor = hgGold24UhrBody.color;
            Color hgSonneColor = hgGoldSonne.color;
            Color hgUhrUntenColor = hgUhrUnten.color;

            if (hgMeridianColor.a < 1) {
                hgMeridianColor.a += 0.02f;
                hgMeridian.color = hgMeridianColor;
                hgUhr24Color.a += 0.02f;
                hg24Uhr.color = hgUhr24Color;
                hg24Uhr_2.color = hgUhr24Color;
                hgEkliptikColor.a += 0.02f;
                hgEkliptik.color = hgEkliptikColor;
                hgGoldColor.a += 0.02f;
                hgGold.color = hgGoldColor;
                hg24UhrgoldBodyColor.a += 0.02f;
                hgGold24UhrBody.color = hg24UhrgoldBodyColor;
                hgSonneColor.a += 0.02f;
                hgGoldSonne.color = hgSonneColor;
                hgUhrUntenColor.a += 0.02f;
                hgUhrUnten.color = hgUhrUntenColor;
            } else {
                hg24UhrMR.sharedMaterial = hg24UhrOpaque;
                hg24UhrMR_2.sharedMaterial = hg24UhrOpaque_2;
                hgMeridianMR.sharedMaterial = hgMeridianOpaque;
                hgGoldMR.sharedMaterial = hgGoldOpaque;
                hgGold24UhrBodyMR.sharedMaterial = hgGold24UhrBodyOpaque;
                hgSonnenbahnMR.sharedMaterial = hgEkliptikOpaque;
                hgMondbahnMR.sharedMaterial = hgEkliptikOpaque;
                hgGoldSonneMR.sharedMaterial = hgGoldSonneOpaque;
                hgUhrUntenMR.sharedMaterial = hgUhrUntenOpaque;
                hgMondMR.sharedMaterial = hgGoldOpaque;
                turnStep3On = false;
            }
        }

        if (turnGlobusLightOn) {
            if (globusLight.intensity < 1) {
                globusLight.intensity += 0.075f;
            } else {
                globusLight.intensity = 1;
                turnGlobusLightOn = false;
            }
        }

        if (turnPlaneHorizontOff) {
            Color planeHorizontColor = planeHorizont.color;
            if (planeHorizontColor.a > 0) {
                planeHorizontColor.a -= 0.1f;
                planeHorizont.color = planeHorizontColor;
            } else {
                planeHorizontColor.a = 0;
                planeHorizont.color = planeHorizontColor;
                turnPlaneHorizontOff = false;
            }
        }
        
    #region Breitengradeinstellung

        if (turnHgHorizontOn) {
            Color hgHorizontColor = hgHorizont.color;
            if (hgHorizontColor.a < 1) {
                hgHorizontColor.a += 0.015f;
                hgHorizont.color = hgHorizontColor;
                hgHorizont2.color = hgHorizontColor;
            } else {
                turnStep3On = true;
                hgHorizontMR.sharedMaterial = hgHorizontOpaque;
                hgHorizontMR2.sharedMaterial = hgHorizont2Opaque;
                turnHgHorizontOn = false;
            }
        }

        if (highlightHorizont) {
            hgHorizontOpaque.color = hgHorizontHighlightColor;
            hgHorizont2Opaque.color = hgHorizontHighlightColor;
        }

        if (flashHighlightMeridianDresden) {
            if (highlightMeridianDresdenOn) {
                Color hgMeridianHighlightDresdenColor = hgMeridianHighlightDresden.color;

                if (hgMeridianHighlightDresdenColor.a < 1) {
                    hgMeridianHighlightDresdenColor.a += 0.05f;
                    hgMeridianHighlightDresden.color = hgMeridianHighlightDresdenColor;
                } else {
                    highlightMeridianDresdenOff = true;
                    highlightMeridianDresdenOn = false;
                }
            }

            if (highlightMeridianDresdenOff) {
                Color hgMeridianHighlightDresdenColor = hgMeridianHighlightDresden.color;
                if (hgMeridianHighlightDresdenColor.a > 0) {
                    hgMeridianHighlightDresdenColor.a -= 0.05f;
                    hgMeridianHighlightDresden.color = hgMeridianHighlightDresdenColor;
                } else {
                    highlightMeridianDresdenOn = true;
                    highlightMeridianDresdenOff = false;
                }
            }
        }

        if (turnMeridianDresdenOff) {
            Color hgMeridianHighlightDresdenColor = hgMeridianHighlightDresden.color;
            if (hgMeridianHighlightDresdenColor.a > 0) {
                hgMeridianHighlightDresdenColor.a -= 0.05f;
                hgMeridianHighlightDresden.color = hgMeridianHighlightDresdenColor;
            } else {
                himmelsglobusAnimator.Rebind();
                himmelsglobusAnimator.Update(0f);
                himmelsglobusAnimator.Play("HgRotateToOslo", 0, 0);
                himmelsglobusAnimator.SetInteger("AnimationState", 4);
                turnMeridianDresdenOff = false;
                pC.JumpToNextStep();
            }
        }

        if (turnMeridianOsloOff) {
            Color hgMeridianHighlightDresdenColor = hgMeridianHighlightDresden.color;
            if (hgMeridianHighlightDresdenColor.a > 0) {
                hgMeridianHighlightDresdenColor.a -= 0.05f;
                hgMeridianHighlightDresden.color = hgMeridianHighlightDresdenColor;
            } else {
                himmelsglobusAnimator.Rebind();
                himmelsglobusAnimator.Update(0f);
                himmelsglobusAnimator.Play("HgRotateToAlexandria", 0, 0);
                himmelsglobusAnimator.SetInteger("AnimationState", 5);
                turnMeridianOsloOff = false;
                pC.JumpToNextStep();
            }
        }

        if (turnMeridianAlexandriaOff) {
            Color hgMeridianHighlightDresdenColor = hgMeridianHighlightDresden.color;
            if (hgMeridianHighlightDresdenColor.a > 0) {
                hgMeridianHighlightDresdenColor.a -= 0.05f;
                hgMeridianHighlightDresden.color = hgMeridianHighlightDresdenColor;
            } else {
                himmelsglobusAnimator.Rebind();
                himmelsglobusAnimator.Update(0f);
                turnMeridianAlexandriaOff = false;
            }
        }

        if (showHgMeridianDresdenText) {
            Color hgMeridianDresdenGradColor = hgMeridianDresdenGrad.faceColor;
            if (hgMeridianDresdenGradColor.a < 1) {
                hgMeridianDresdenGradColor.a += 0.075f;
                hgMeridianDresdenGrad.faceColor = hgMeridianDresdenGradColor;
            } else {
                hgMeridianDresdenGrad.faceColor = new Color(255, 52, 103, 255);
                showHgMeridianDresdenText = false;
            }
        }

        if (turnHgMeridianDresdenTextOff) {
            Color hgMeridianDresdenGradColor = hgMeridianDresdenGrad.faceColor;
            if (hgMeridianDresdenGradColor.a > 0) {
                hgMeridianDresdenGradColor.a -= 0.075f;
                hgMeridianDresdenGrad.faceColor = hgMeridianDresdenGradColor;
            } else {
                hgMeridianDresdenGrad.faceColor = new Color(255, 52, 103, 0);
                turnHgMeridianDresdenTextOff = false;
            }
        }

    #endregion

    #region Armilarsphaere

        if (highlightHimmelsaequator) {
            hgArmillarHimmelsaequator.color = hgArmAequatorColor;
            Color hgArmHighlightColor = hgArmHighlight.color;
            hgArmHighlightColor.a = alphaHGArmHighlight;
            hgArmHighlight.color = hgArmHighlightColor;
        }

        if (highlightArmEkliptik) {
            hgArmEkliptikOpaque.color = hgArmEkliptikColor;
            Color hgArmHighlightColor = hgArmHighlight.color;
            hgArmHighlightColor.a = alphaHGArmHighlight;
            hgArmHighlight.color = hgArmHighlightColor;
        }

        if (highlightArmHorizont) {
            hgArmHorizontOpaque.color = hgArmHorizontColor;
        }

        if (turnArmillarOff) {
            Color hgTransparentArmillarColor = hgTransparentArmillar.color;
            if (hgTransparentArmillarColor.a > 0.1) {
                hgTransparentArmillarColor.a -= 0.075f;
                hgTransparentArmillar.color = hgTransparentArmillarColor;
                hgTransparentArmillarHorizont.color = hgTransparentArmillarColor;
                hgTransparentArmillarErde.color = hgTransparentArmillarColor;
                hgTransparentArmillarEkliptik.color = hgTransparentArmillarColor;
            } else {
                hgTransparentArmillarColor.a = 0.1f;
                hgTransparentArmillar.color = hgTransparentArmillarColor;
                hgTransparentArmillarHorizont.color = hgTransparentArmillarColor;
                hgTransparentArmillarErde.color = hgTransparentArmillarColor;
                hgTransparentArmillarEkliptik.color = hgTransparentArmillarColor;
                turnArmillarOff = false;
                pC.JumpToNextStep();
            }
        }

        if (showArmEkliptik) {
            Color hgTransparentArmillarColor = hgTransparentArmillarEkliptik.color;
            if (hgTransparentArmillarColor.a < 1) {
                hgTransparentArmillarColor.a += 0.075f;
                hgTransparentArmillarEkliptik.color = hgTransparentArmillarColor;
            } else {
                hgTransparentArmillarColor.a = 1f;
                hgTransparentArmillarEkliptik.color = hgTransparentArmillarColor;
                hgArmillarEkliptik.sharedMaterial = hgArmEkliptikOpaque;
                hgArmEkliptikhighlight.SetActive(true);
                gameControllerAnimator.Rebind();
                gameControllerAnimator.Update(0f);
                gameControllerAnimator.Play("HighlightHgArmEkliptik", 0, 0);
                gameControllerAnimator.SetInteger("hgAnimationState", 4);
                highlightArmEkliptik = true;
                showArmEkliptik = false;
            }
        }

        if (showhorizontArm) {
            Color hgTransparentArmillarColor = hgTransparentArmillarHorizont.color;
            if (hgTransparentArmillarColor.a < 1) {
                hgTransparentArmillarColor.a += 0.075f;
                hgTransparentArmillarHorizont.color = hgTransparentArmillarColor;
            } else {
                hgTransparentArmillarColor.a = 1f;
                hgTransparentArmillarHorizont.color = hgTransparentArmillarColor;
                hgHorizontMR.sharedMaterial = hgArmHorizontOpaque;
                hgHorizontMR2.sharedMaterial = hgArmHorizontOpaque;
                hgArmillarHorizont.sharedMaterial = hgArmHorizontOpaque;
                gameControllerAnimator.Rebind();
                gameControllerAnimator.Update(0f);
                gameControllerAnimator.Play("HighlightHgArmHorizont", 0, 0);
                gameControllerAnimator.SetInteger("hgAnimationState", 5);
                highlightArmHorizont = true;
                showhorizontArm = false;
            }
        }

    #endregion

    #region Himmelskugel

        if (makehgHimmelskugelTransparent) {
            Color hgHimmelskugelColor = hgHimmelskugel.color;
            if (hgHimmelskugelColor.a > 0.5f) {
                hgHimmelskugelColor.a -= 0.075f;
                hgHimmelskugel.color = hgHimmelskugelColor;
            } else {
                makehgHimmelskugelTransparent = false;
            }
        }

        if (showHGHimmelskugel) {
            Color hgHimmelskugelColor = hgHimmelskugel.color;
            if (hgHimmelskugelColor.a < 0.5f) {
                hgHimmelskugelColor.a += 0.075f;
                hgHimmelskugel.color = hgHimmelskugelColor;
            } else {
                okButton.interactable = true;
                showHGHimmelskugel = false;
            }
        }

        if (showHGHimmelskugelFull) {
            Color hgHimmelskugelColor = hgHimmelskugel.color;
            if (hgHimmelskugelColor.a < 1) {
                hgHimmelskugelColor.a += 0.035f;
                hgHimmelskugel.color = hgHimmelskugelColor;
            } else {
                himmelsglobusAnimator.Play("ShowHimmelsglobusPart2", 0, 0);
                himmelsglobusAnimator.SetInteger("AnimationState", 2);
                hgHimmelskugelMR.sharedMaterial = hgHimmelskugelOpaque;
                showHGHimmelskugelFull = false;
            }
        }

    #endregion

    #region Sonne und Mond

        if (showHGHimmelskugelSonne) {
            Color hgHimmelskugelSonneColor = hgHimmelskugelSonne.color;
            if (hgHimmelskugelSonneColor.a < 1f) {
                hgHimmelskugelSonneColor.a += 0.015f;
                hgHimmelskugelSonne.color = hgHimmelskugelSonneColor;
            } else {
                pC.JumpToNextStep();
                showHGHimmelskugelSonne = false;
            }
        }

        if (flashHighlightMoonSphere) {
            if (highlightMoonSphereOn) {
                Color hgMoonSphereColor = moonSphereHighlight.color;

                if (hgMoonSphereColor.a < 1) {
                    hgMoonSphereColor.a += 0.085f;
                    moonSphereHighlight.color = hgMoonSphereColor;
                } else {
                    highlightMoonSphereOff = true;
                    highlightMoonSphereOn = false;
                }
            }

            if (highlightMoonSphereOff) {
                Color hgMoonSphereColor = moonSphereHighlight.color;
                if (hgMoonSphereColor.a > 0) {
                    hgMoonSphereColor.a -= 0.085f;
                    moonSphereHighlight.color = hgMoonSphereColor;
                } else {
                    highlightMoonSphereOn = true;
                    highlightMoonSphereOff = false;
                }
            }
        }

        if (turnhighlightMoonSphereOff) {
            Color hgMoonSphereColor = moonSphereHighlight.color;
            if (hgMoonSphereColor.a > 0) {
                hgMoonSphereColor.a -= 0.085f;
                moonSphereHighlight.color = hgMoonSphereColor;
            } else {
                turnhighlightMoonSphereOff = false;
            }
        }


        if (highlightSundAndMoonhalf) {
            hgMoonGlowHalf.color = hgMoonGlowHalfColor;
            hgGoldSonneOpaque.color = hgSunColor;
        }

    #endregion

    #region Starry Sky

        if (turnCylinderOff) {
            Color cylinderMatColor = polarStarLineTrans.color;
            if (cylinderMatColor.a > 0) {
                cylinderMatColor.a -= 0.075f;
                polarStarLineTrans.color = cylinderMatColor;
            } else {
                cylinderMatColor.a = 0f;
                polarStarLineTrans.color = cylinderMatColor;
                turnCylinderOff = false;
            }
        }

        if (showBogen) {
            Color bogenZ02Color = bogenZ02.color;
            Color bogenGradColor = bogenGrad.faceColor;
            if (bogenZ02Color.a < 1) {
                bogenGradColor.a += 0.075f;
                bogenGrad.faceColor = bogenGradColor;

                bogenZ02Color.a += 0.075f;
                bogenZ02.color = bogenZ02Color;
            } else {
                bogenGrad.faceColor = new Color(255, 52, 103, 255);
                bogenZ02Color.a = 1f;
                bogenZ02.color = bogenZ02Color;
                showBogen = false;
            }
        }

        if (turnBogenOff) {
            Color bogenZ02Color = bogenZ02.color;
            Color bogenGradColor = bogenGrad.faceColor;
            if (bogenZ02Color.a > 0) {
                bogenGradColor.a -= 0.075f;
                bogenGrad.faceColor = bogenGradColor;

                bogenZ02Color.a -= 0.075f;
                bogenZ02.color = bogenZ02Color;
            } else {
                bogenGrad.faceColor = new Color(255, 52, 103, 0);
                bogenZ02Color.a = 0f;
                bogenZ02.color = bogenZ02Color;
                turnBogenOff = false;
            }
        }

        if (showGitterNetz) {
            Color gitternetzKugelColor = gitternetzKugelMat.color;
            if (gitternetzKugelColor.a < 0.12) {
                gitternetzKugelColor.a += 0.025f;
                gitternetzKugelMat.color = gitternetzKugelColor;
            } else {
                gitternetzKugelColor.a = 0.12f;
                gitternetzKugelMat.color = gitternetzKugelColor;
                okButton.interactable = true;
                showGitterNetz = false;
            }
        }

        if (showGitterNetzFull) {
            Color gitternetzKugelColor = gitternetzKugelMat.color;
            if (gitternetzKugelColor.a < 0.75f) {
                gitternetzKugelColor.a += 0.01f;
                gitternetzKugelMat.color = gitternetzKugelColor;
            } else {
                gitternetzKugelColor.a = 0.75f;
                gitternetzKugelMat.color = gitternetzKugelColor;
                showGitterNetzFull = false;
            }
        }

        if (turnGitterNetzOff) {
            Color gitternetzKugelColor = gitternetzKugelMat.color;
            if (gitternetzKugelColor.a > 0) {
                gitternetzKugelColor.a -= 0.1f;
                gitternetzKugelMat.color = gitternetzKugelColor;
            } else {
                gitternetzKugelColor.a = 0f;
                gitternetzKugelMat.color = gitternetzKugelColor;
                gitternetzKugel.SetActive(false);
                turnGitterNetzOff = false;
            }
        }

        if (turnStarsOn) {
            Color matStarUnlitColor = matStarUnlit.color;
            if (matStarUnlit.color.a < 1) {
                matStarUnlitColor.a += 0.1f;
                matStarUnlit.color = matStarUnlitColor;
            } else {
                turnStarsOn = false;
            }
        }

        if (turnStarsOff) {
            Color matStarUnlitColor = matStarUnlit.color;
            if (matStarUnlit.color.a > 0) {
                matStarUnlitColor.a -= 0.1f;
                matStarUnlit.color = matStarUnlitColor;
            } else {
                turnStarsOff = false;
            }
        }

    #endregion

    #region Erdglobus

        if (turnGestellForErdglobusOff) {
            Color hgHidePlaneColor = hgHidePlane.color;

            Color hgGestellColor = hgBeineGoldTrans.color;
            if (hgGestellColor.a > 0f) {
                hgGestellColor.a -= 0.08f;
                hgBeineGoldTrans.color = hgGestellColor;
                hgDrachenfluegelTrans.color = hgGestellColor;
                hgDrachenkopfTrans.color = hgGestellColor;
                hgBeineflachesObjektTrans.color = hgGestellColor;
                hgEGHorizontTrans.color = hgGestellColor;
                hgEGMeridianTrans.color = hgGestellColor;
                hgSaeulaGoldTrans.color = hgGestellColor;
                hgHidePlaneColor.a += 0.05f;
                hgHidePlane.color = hgHidePlaneColor;
            } else {
                hgGestellColor.a = 0f;
                hgHidePlaneColor.a = 1f;
                hgHidePlane.color = hgHidePlaneColor;
                hgBeineGoldTrans.color = hgGestellColor;
                hgDrachenfluegelTrans.color = hgGestellColor;
                hgDrachenkopfTrans.color = hgGestellColor;
                hgBeineflachesObjektTrans.color = hgGestellColor;
                hgEGHorizontTrans.color = hgGestellColor;
                hgEGMeridianTrans.color = hgGestellColor;
                hgSaeulaGoldTrans.color = hgGestellColor;
                turnGestellForErdglobusOff = false;
            }
        }


        if (turnEgLightOn) {
            if (erdglobusLight.intensity < 0.5f) {
                erdglobusLight.intensity += 0.075f;
            } else {
                erdglobusLight.intensity = 0.5f;
                turnEgLightOn = false;
            }
        }

        if (turnEgLightOff) {
            if (erdglobusLight.intensity > 0) {
                erdglobusLight.intensity -= 0.075f;
            } else {
                erdglobusLight.intensity = 0;
                erdglobusLight.enabled = false;
                turnEgLightOff = false;
            }
        }

        if (highlightErdglobus) {
            if (highlightEGLila) {
                erdglobusHighlightColor.a *= 2.5f;
            }

            erdglobusHighlightMat.color = erdglobusHighlightColor;
        }

    #endregion
    #endregion
    }

    
    private IEnumerator WaitForEndOfAnim() {
        yield return new WaitForSeconds(2.2f);
        meridianHighlightOslo.localScale = new Vector3(0, 0, 0);
        meridianHighlightAlexandria.localScale = new Vector3(1, 1, 1);
        flashHighlightMeridianDresden = true;
        highlightMeridianDresdenOn = true;
        highlightMeridianDresdenOff = false;
    }

    public void StartDrawLineToPolarstar() {
        showGitterNetz = true;
    }

    private IEnumerator ShowHgMarker() {
        yield return new WaitForSeconds(2f);
        hgc.ShowHgMarker();
    }

    private IEnumerator WaitForEndOfAnimBeforeJump() {
        yield return new WaitForSeconds(1.5f);
        pC.JumpToNextStep();
    }

    private IEnumerator WaitForEndOfRotation() {
        yield return new WaitForSeconds(2.5f);
        turnEgLightOff = true;
        pC.JumpToNextStep();
        rotateEgToTarget = false;
    }

    private IEnumerator WaitForEndOfAnimBeforeRotateGlobe() {
        yield return new WaitForSeconds(1.5f);
        rotateEgToTarget = true;
        targetRotation = Quaternion.Euler(351.3f, 289.2f, 72.9f);
        yield return new WaitForSeconds(1f);
        gameControllerAnimator.enabled = true;
        gameControllerAnimator.Rebind();
        gameControllerAnimator.Update(0f);
        gameControllerAnimator.Play("HighlightHgErdglobus", 0, 0);
        gameControllerAnimator.SetInteger("hgAnimationState", 6);
        highlightErdglobus = true;
    }
}