#region Description

/*In diesem Script werden sämtliche Variablen auf ihren Ausgangszustand zurückgesetzt. Jeder Pfad besitzt ein eigenes
 * Helper-Script. Wenn ein Pfad durchgespielt und zum MainMenu gespriungen wird, werden alle Helper-Skripte deaktiviert.
 * Wenn ein Pfad gestartet wird, wird nur das benötigte Helper-Skript aktiviert. Deswegen muss die App im MainMenu
 * immer wieder in den Ausgangszustand gebracht werden. Insbesondere Einblendungen, die Instrumente und Materialen
 * müssen immer wieder in ihren AUsgangszustand gebracht werden, um im nächsten Pfad keine Fehler darzustellen.
 */

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class InitScene : MonoBehaviour {
#region public Variables
#region Scripts

    public ProcedureController pC;
    public N01Helper n01H;
    public N02Helper n02H;
    public N03Helper n03H;
    public N05Helper n05H;
    public N07Helper n07H;
    public N08Helper n08H;
    public Z01Helper z01H;
    public S01Helper s01H;
    public S02Helper s02H;
    public S03NEWHelper s03NewH;
    public S05Helper s05H;
    public Z02Helper z02H;
    public NokturnalController nokturnalController;
    public SkyCamHelperForZ02 sCHZ02;
    public SkyTimeController skyTimeController;
    public SkyRenderController sRC;
    public GetLocationService gLS;
    public RotateStarrSky rS;
    public HimmelsglobusRotation hGR;
    public AnimationEventHandlerN03 aEHN03;
    public AnimationEventHandlerCameraN01 aEHN01;
    public AnimationEventHandlerCameraN05 aEHN05;
    public AnimationEventHandlerCameraZ01 aEHZ01;
    public AnimationEventHandlerCameraS02 aEHS02;
    public AnimationEventHandlerCameraS03 aEHS03;
    public ContentScrollSnapHorizontal cSSHWeekCalendar;
    public SkyProfileEventHandler sPEH;
    public MainMenuController mmc;
    public HimmelsglobusController hgc;
    public CheckPointController cpc;

#endregion

#region Animator

    public Animator cameraAnimator;
    public Animator starrySkyAnimator;
    public Animator skyAnimator;
    public Animator sunDialAnimator;
    public Animator skyCameraParentAnimator;
    public Animator mainMenuAnimator;
    public Animator himmelsglobusAnimator;
    public Animator mainCamPivotAnimator;
    public Animator gameControllerAnimator;

#endregion

#region Buttons

    public Button burgerMenuButton;
    public Button nokturnalImageMMC;
    public Button sonnenuhrImageMMC;

#endregion

#region Cameras

    public Camera mainCam;
    public Camera skyCam;
    public Camera sunDialCam;
    public Camera nokturnalCam;

    private LensDistortion lD;
    public Volume volume;
    public UniversalRenderPipelineAsset uRPA;

#endregion

#region Canvas Groups

    public CanvasGroup footUI;
    public CanvasGroup headUI;
    public CanvasGroup blackBlend;
    public CanvasGroup mainMenu;
    public CanvasGroup mondVolvelleLupe;
    public CanvasGroup overview;
    public CanvasGroup sunSymbol;
    public CanvasGroup moonSymbol;

#endregion

#region GameObjects

    public GameObject mainCamObject;
    public GameObject mainCamChildObject;
    public GameObject moonLight;
    public GameObject skyCameraParent;
    public GameObject sunDialParent;
    public GameObject sunDialCameraObject;
    public GameObject sunDialObject;
    public GameObject nokturnalCamera;
    public GameObject hgErdglobusUhr;
    public GameObject himmelsglobusobject;
    public GameObject bestaetigenButton;
    public GameObject fakeSun;
    public GameObject kalender;
    public GameObject kalenderlinien;
    public GameObject kalenderlinienKrebs;
    public GameObject s03_1;
    public GameObject s03_2;
    public GameObject s03_3;
    public GameObject timeParent;
    public GameObject clockParent;
    public GameObject uhr24HGameobject;
    public GameObject permissionDialog;
    public GameObject krebsRahmen;
    public GameObject summerText;
    public GameObject winterText;
    public GameObject pfeil1;
    public GameObject pfeil2;
    public GameObject pfeil3;
    public GameObject pfeil4;
    public GameObject n01ArrowParent;
    public GameObject directions;
    public GameObject sonnenuhrSchloss;
    public GameObject himmelsglobusSchloss;
    public GameObject weekCalendar;
    public GameObject cylinderN05Input;
    public GameObject cylinderZ02;
    public GameObject eastGo;
    public GameObject hgMarkerParent;
    public GameObject hgCamera;
    public GameObject hgHimmelsAequatorHighlight;
    public GameObject hgArmEkliptikhighlight;
    public GameObject sonnenUhrAmbientLight;
    public GameObject lilaHelpPoint;
    public GameObject kreisBogenZ01;
    public GameObject mainmenu3dHideObject;
    public GameObject northForOutofSphereGo;
    public GameObject fakeMoon;
    public GameObject weiterButtonTMPPRO;
    public GameObject weiterButtonUIText;
    public GameObject subheadlineUIText;
    public GameObject subheadlineTMPRO;
    public GameObject grosserWagenN05;
    public GameObject sunDialDirectionalLightNight;

    public List<GameObject> traceParentsN02 = new List<GameObject>();
    public List<GameObject> gitterNetzLaengengrade = new List<GameObject>();

#endregion

#region Images

    public Image krebsLupeSprite;
    public Image fillCircleImage;

    public SpriteRenderer kalenderOnSundDialSprite;
    public SpriteRenderer kalenderKrebsHighlight;
    public SpriteRenderer griffBlackBackground;
    public SpriteRenderer aussenringBlackBackground;
    public SpriteRenderer schabloneBlackBackground;
    public SpriteRenderer uhr24H;
    public SpriteRenderer krebsRahmenSprite;
    public SpriteRenderer gridGround;
    public SpriteRenderer sunDialKompass;
    public SpriteRenderer earthFigue;
    public SpriteRenderer sunDialGroundPlate;
    public SpriteRenderer frontZahlenFull;
    public SpriteRenderer moonVolvelleHighlight;
    public SpriteRenderer zeigerWhite;
    public SpriteRenderer moonSphereHighlight;
    public SpriteRenderer datumsanzeige;
    public SpriteRenderer einstellring;
    public SpriteRenderer griff;
    public SpriteRenderer niete;
    public SpriteRenderer zaehne;
    public SpriteRenderer zeiger;
    public SpriteRenderer handSprite;

#endregion

#region Lights

    public Light extraLight;
    public Light moonDirectLight;
    public Light sunDirectLight;
    public Light globusLight;
    public Light erdglobusLight;

#endregion

#region Materials

    public Material planeGround;
    public Material sun;
    public Material earthMaterial;
    public Material importantStars;
    public Material importantStarsSchuettkantenStern;
    public Material materialPolarstern;
    public Material matStar_Breitengradsterne;
    public Material horizontScheibe;
    public Material laengenGradNetz;
    public Material groundMat;
    public Material fadenHiglightLong;
    public Material sunLineHighlightOverHorizont;
    public Material breitengradSterneHighlight;
    public Material fakeSunMat;
    public Material gridGroundS03;
    public Material gridGroundZ02;
    public Material planeGroundPrio0;
    public Material importantstarsS03;
    public Material ekliptikBogen;
    public Material ekliptikBogenHinten;
    public Material sunLineSommerBlue;
    public Material sunLineWinterBlue;
    public Material sunLineMarchBlue;
    public Material newLineCylinder;
    public Material bogenZ02;
    public Material cylinderAchseZ01;
    public Material bogenZ01;
    public Material hgHidePlane;
    public Material outOfSphereSkymat;
    public Material gitternetzkugelN05;

    //Sonnenuhr Materials
    public Material suHolz;
    public Material suPlatte;
    public Material suVolvelle;
    public Material suFaden;
    public Material suKugel;
    public Material suMetall;
    public GameObject blockImage;
    public Material sunLineSommer;
    public Material sunLineWinter;
    public Material sunMarker;
    public Material sunLineMat;
    public Material schattenwerferHighlight;
    public Material sunDialSternzeichen;
    public Material sunDialSternzeichenKrebsHighlight;
    public Material sunDialHighlight16;
    public Material schattenwerferMat;
    public Material fadenMat;
    public Material sunDialRadialLines;
    public Material sunDialWoodMatOpaque;
    public Material sunDialGroundPlateMatOpaque;
    public Material sunDialWoodMatTrans;
    public Material sunDialGroundPlateMatTrans;
    public Material sunDialKompassNadel;
    public Material kalenderSternzeichenS02Black;

    //Himmelsgloubs Materials
    public Material hgHorizont;
    public Material hgHorizont2;
    public Material hgMeridian;
    public Material hg24Uhr;
    public Material hg24Uhr_2;
    public Material hgEkliptik;
    public Material hgGold;
    public Material hgGold24UhrBody;
    public Material hgGoldSonne;
    public SpriteRenderer hgUhrUnten;
    public Material hgArmGold;
    public Material hgGlobusMat;
    public Material hgMarker;
    public Material ekliptikLineZ02;
    public Material hgHimmelskugel;
    public Material horizontMat2;
    public Material hgHimmelskugelSonne;
    public Material ekliptikOnHG;
    public Material ekliptikMarker;
    public Material hgMeridianHighlightDresden;
    public Material hgMoonGlowHalf;
    public Material hgGoldSonneOpaque;
    public Material sonneAufHimmelskugel;

    //step4 HG Gestell
    public Material hgBeineGold;
    public Material hgBeineflachesObjekt;
    public Material hgDrachenfluegel;
    public Material hgDrachenkopf;
    public Material hgHalterung;
    public Material hgSphereGestell;
    public Material hgErdglobus;
    public Material hgErdglobusMeridian;

    //step4 HGSockel
    public Material hgBodenplatte;
    public Material hgEGHorizont;
    public Material hgGestellGold;
    public Material hgKompass;
    public Material hgSockel;

#endregion

#region MeshRenderer

    public MeshRenderer hgGlobus;
    public MeshRenderer groundPlane;
    public MeshRenderer planeGroundMR;
    public MeshRenderer sunDialWood;

#endregion

#region Line- And Trail Renderer

    public LineRenderer wintertrail;
    public LineRenderer wintertrailUhr;
    public LineRenderer sumerTrail;
    public LineRenderer sumerTrailUhr;

    public TrailRenderer ekliptikTrace;
    public TrailRenderer s03_1TR;
    public TrailRenderer s03_2TR;
    public TrailRenderer s03_3TR;
    public TrailRenderer sunConstanTR;
    public TrailRenderer calcedShadowObjectTR;
    public TrailRenderer drawWitMouseTR;
    public TrailRenderer s03_1Winter;
    public TrailRenderer s03_2March;
    public TrailRenderer s03_3Summer;
    public TrailRenderer s03_4Sept;
    public TrailRenderer s03_1WinterBlue;
    public TrailRenderer s03_2MarchBlue;
    public TrailRenderer s03_3SummerBlue;
    public TrailRenderer s03_4SeptBlue;

    public UILineRenderer newPathUILineRenderer;
    public UILineRenderer startUiLineRenderer;

#endregion

#region Text

    public TextMeshPro north;
    public TextMeshPro east;
    public TextMeshPro south;
    public TextMeshPro west;
    public TextMeshPro text3dTage;
    public TextMeshPro bogenGrad;
    public TextMeshPro hgMeridianDresdenGrad;
    public TextMeshPro bogenGradZ01;

    public TextMeshProUGUI dateText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI burgerMenuPfadBeendenText;
    public TextMeshProUGUI impressumText;
    public TextMeshProUGUI impressumTextEN;
    public TextMeshProUGUI impressumText2Down;
    public TextMeshProUGUI impressumText2DownEN;
    public TextMeshProUGUI impressumTextURL;
    public TextMeshProUGUI impressumTextVersion;
    public TextMeshProUGUI philsophieText;
    public TextMeshProUGUI philsophieTextEN;

#endregion

#region Transforms

    public Transform cameraPivotX;
    public Transform cameraPviotZ;
    public Transform earthTransform;
    public Transform skyParentTransf;
    public Transform skyCameraXPivot;
    public Transform skyCameraCenter;
    public Transform skyCamera;
    public Transform nokturnal;
    public Transform clockParentTransf;
    public Transform groundPlaneTransform;
    public Transform cameraContainer;
    public Transform sunlightPivot;
    public Transform sundDirectionalLightTransf;
    public Transform mainCamTransform;
    public Transform starrySkyParent;
    public Transform sunLightPivot;
    public Transform krebsLupe;
    public Transform figureEarthTransf;
    public Transform sunDialTransf;
    public Transform mainMenuTransf;
    public Transform gridTransf;
    public Transform southTransf;
    public Transform starrySkyCompassPivot;
    public Transform einstellringPivot;
    public Transform sunSymbulTransf;
    public Transform moonSymbulTransf;
    public Transform overViewtransf;
    public Transform timetextTransf;
    public Transform groundDirection;
    public Transform horizontLilaTransf;
    public Transform firstTextTmPro;
    public Transform secondTextTmPro;
    public Transform firstTextUnityUI;
    public Transform secondTextUnityUI;
    public Transform bestaetigenTmPro;
    public Transform bestaetigenUnityUI;
    public Transform hgMondHalfHalf;
    public Transform winkelBogenZ01;
    public Transform frontZahlenFullGo;
    public Transform frontZahlenTrans;
    public Transform sunObjectsParent;

    public RectTransform hgCamSplitLine;
    public RectTransform impressumUrlButton;
    public RectTransform impressumVersion;
    public RectTransform impressumText2;
    public RectTransform impressumText2EN;
    public RectTransform logoParent;
    public RectTransform splitLine;

#endregion
#endregion

    private int lastObject = -1;
    private bool turnBlackBlendOn;
    private bool turnBlackBlendOff;
    private DateTime realPlaceTime;


    private void Start() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        LensDistortion tmp;

        if (volume.profile.TryGet(out tmp))
            lD = tmp;

        InitSceneNow();
        StartCoroutine(InitTime());
        StartCoroutine(CloseLoadAnim());
    }

    private void Update() {
        if (turnBlackBlendOn) {
            if (blackBlend.alpha < 1f)
                blackBlend.alpha += 0.1f;
            else
                turnBlackBlendOn = false;
        }

        if (turnBlackBlendOff) {
            if (blackBlend.alpha > 0)
                blackBlend.alpha -= 0.1f;
            else
                turnBlackBlendOff = false;
        }
    }

    public void InitMenu() {
        turnBlackBlendOn = true;
        StartCoroutine(StartInitInMenu());
    }

    private IEnumerator StartInitInMenu() {
        yield return new WaitForSeconds(0.75f);
        InitSceneNow();
        yield return new WaitForSeconds(0.25f);
        turnBlackBlendOff = true;
        turnBlackBlendOn = false;
        var loadData = "";
        try {
            var filePath = Path.Combine(Application.persistentDataPath, "spielstand.txt");
            loadData = File.ReadAllText(filePath);
            if (loadData == "1") {
                sonnenuhrSchloss.SetActive(false);
                himmelsglobusSchloss.SetActive(true);
            } else if (loadData == "2") {
                sonnenuhrSchloss.SetActive(false);
                himmelsglobusSchloss.SetActive(false);
            }
        } catch (FileNotFoundException) {
            sonnenuhrSchloss.SetActive(true);
            himmelsglobusSchloss.SetActive(true);
        }
    }

    private IEnumerator CloseLoadAnim() {
        Input.location.Start();
        yield return new WaitForSeconds(3);
        permissionDialog.SetActive(true);
        permissionDialog.transform.localScale = new Vector3(1, 1, 1);

        //#if UNITY_ANDROID
        if (AndroidRuntimePermissions.CheckPermission("android.permission.ACCESS_FINE_LOCATION") ==
            AndroidRuntimePermissions.Permission.Granted) {
            permissionDialog.SetActive(false);
            permissionDialog.transform.localScale = new Vector3(0, 0, 0);
            gLS.InitLocationAgain();
        } else if (AndroidRuntimePermissions.CheckPermission("android.permission.ACCESS_FINE_LOCATION") ==
                   AndroidRuntimePermissions.Permission.Denied) {
            permissionDialog.SetActive(true);
            permissionDialog.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private IEnumerator InitTime() {
        float hour = DateTime.Now.Hour;
        var minute = DateTime.Now.Minute * 1.66f / 100;
        var aktTime = hour + minute;
        yield return new WaitForSeconds(3f);
        skyTimeController.SetDate(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        skyTimeController.SetTimeline(aktTime);
        skyTimeController.SetUtc(1);
        skyTimeController.AllowIncreaseDayWithAnim();
    }

    //Alle public Variablen werden hier auf ihren Ausgangszustand gesetzt (benötigt noch Sortierung)
    public void InitSceneNow() {
        n05H.TurnTraceLinesOff();
        z01H.TurnAllTrailLinesOff();

        n01H.enabled = false;
        n01H.StopAllCoroutines();
        n02H.enabled = false;
        n02H.StopAllCoroutines();
        n03H.enabled = false;
        n03H.StopAllCoroutines();
        n05H.enabled = false;
        n05H.StopAllCoroutines();
        n07H.enabled = false;
        n07H.StopAllCoroutines();
        n08H.enabled = false;
        n08H.StopAllCoroutines();
        z01H.enabled = false;
        z01H.StopAllCoroutines();
        s01H.enabled = false;
        s01H.StopAllCoroutines();
        s02H.enabled = false;
        s02H.StopAllCoroutines();
        s03NewH.enabled = false;
        s03NewH.StopAllCoroutines();
        s05H.enabled = false;
        s05H.StopAllCoroutines();
        z02H.enabled = false;
        z02H.StopAllCoroutines();
        sCHZ02.enabled = false;
        //----------------------------------

        weiterButtonTMPPRO.SetActive(true);
        weiterButtonUIText.SetActive(false);

        subheadlineTMPRO.SetActive(true);
        subheadlineUIText.SetActive(false);

        foreach (GameObject gO in gitterNetzLaengengrade) {
            gO.GetComponent<MeshRenderer>().sharedMaterial = gitternetzkugelN05;
        }

        aEHN01.StopMoveWithAnimAEHN01();
        aEHN05.StopMoveWithAnim();
        aEHZ01.StopMoveWithAnimZ01AEH();
        aEHS02.StopMoveWithAnimS02AEH();
        aEHS03.StopMoveWithAnimS03AEH();

        nokturnalCam.orthographicSize = 5;
        var resolutionFactor = DeviceInfo.GetResolutionFactor();
        var newFontSize = 46 + (0.75f - resolutionFactor) * 30 / 0.3f;
        impressumText.fontSize = newFontSize;
        impressumTextURL.fontSize = newFontSize;
        impressumText2Down.fontSize = newFontSize;
        impressumTextVersion.fontSize = newFontSize * 0.95f;
        philsophieText.fontSize = newFontSize;
        impressumTextEN.fontSize = newFontSize;
        impressumText2DownEN.fontSize = newFontSize;
        philsophieTextEN.fontSize = newFontSize;

        float newLogoParentScale = 0.7f + (0.75f - resolutionFactor) * 0.3f / 0.3f;
        logoParent.localScale = new Vector3(newLogoParentScale, newLogoParentScale, 1);
        float newLogoParentYPosition = 425 + (0.75f - resolutionFactor) * 75.0f / 0.3f;
        logoParent.anchoredPosition = new Vector2(0, newLogoParentYPosition);

        float newImpressumUrlButtonYPosition = -710 - (0.75f - resolutionFactor) * 800 / 0.3f;
        impressumUrlButton.anchoredPosition =
            new Vector2(impressumUrlButton.anchoredPosition.x, newImpressumUrlButtonYPosition);
        float newimpressumVersionYPosition = -900 - (0.75f - resolutionFactor) * 930 / 0.3f;
        impressumVersion.anchoredPosition =
            new Vector2(impressumVersion.anchoredPosition.x, newimpressumVersionYPosition);

        float newimpressumText2YPosition = -1050 - (0.75f - resolutionFactor) * 1070 / 0.3f;
        impressumText2.anchoredPosition = new Vector2(impressumText2.anchoredPosition.x, newimpressumText2YPosition);
        impressumText2EN.anchoredPosition =
            new Vector2(impressumText2EN.anchoredPosition.x, newimpressumText2YPosition);

        sunDialDirectionalLightNight.SetActive(false);
        grosserWagenN05.SetActive(false);
        hGR.enabled = false;
        hGR.InitCamPos();
        hgMondHalfHalf.localScale = new Vector3(0, 0, 0);
        erdglobusLight.intensity = 0;
        float hgCameraZ = ((resolutionFactor - 0.45f) * 28 / 0.3f) + 20;
        hgCamera.transform.localPosition = new Vector3(0.3f, 15, hgCameraZ);

        float newYPos = -121 - ((resolutionFactor - 0.45f) * 38 / 0.3f);
        float newZPos = ((resolutionFactor - 0.45f) * 380.0f / 0.3f) + 392;
        winkelBogenZ01.localPosition = new Vector3(-62, newYPos, newZPos);
        if (Screen.width == 1200 && Screen.height == 1920) {
            winkelBogenZ01.localPosition = new Vector3(-62, -147, 652);
        }

        hGR.StopMoveCamFOVWithAnim();
        lilaHelpPoint.SetActive(false);
        fakeMoon.SetActive(false);
        mainmenu3dHideObject.SetActive(false);
        float screenHeightfactor = (Screen.height / 2400.0f) * (-478.38f);
        Vector2 tempanchor = hgCamSplitLine.anchoredPosition;
        hgCamSplitLine.anchoredPosition = new Vector2(tempanchor.x, screenHeightfactor);

        frontZahlenFullGo.transform.localPosition = new Vector3(0.04f, 0.05f, -0.02f);
        frontZahlenTrans.localPosition = new Vector3(0, 0.04f, 0f);
        drawWitMouseTR.Clear();
        drawWitMouseTR.time = 0;

        northForOutofSphereGo.SetActive(false);

        kreisBogenZ01.SetActive(false);
        hgHimmelsAequatorHighlight.SetActive(false);
        hgArmEkliptikhighlight.SetActive(false);
        ;

        Color sonneAufHimmelskugelColor = sonneAufHimmelskugel.color;
        sonneAufHimmelskugelColor.a = 0;
        sonneAufHimmelskugel.color = sonneAufHimmelskugelColor;

        Color hgHidePlaneColor = hgHidePlane.color;
        hgHidePlaneColor.a = 0;
        hgHidePlane.color = hgHidePlaneColor;

        Color kalenderSternzeichenS02BlackColor = kalenderSternzeichenS02Black.color;
        kalenderSternzeichenS02BlackColor.a = 0;
        kalenderSternzeichenS02Black.color = kalenderSternzeichenS02BlackColor;
        sunObjectsParent.transform.localScale = new Vector3(1, 1, 1);
        mainCamPivotAnimator.enabled = false;
        gameControllerAnimator.enabled = false;
        hgCamera.SetActive(false);
        var hgMeridianHighlightDresdenColor = hgMeridianHighlightDresden.color;
        hgMeridianHighlightDresdenColor.a = 0f;
        hgMeridianHighlightDresden.color = hgMeridianHighlightDresdenColor;

        hgMarkerParent.SetActive(false);
        firstTextTmPro.localScale = new Vector3(1, 1, 1);
        firstTextUnityUI.localScale = new Vector3(0, 0, 0);
        secondTextTmPro.localScale = new Vector3(1, 1, 1);
        secondTextUnityUI.localScale = new Vector3(0, 0, 0);
        bestaetigenTmPro.localScale = new Vector3(1, 1, 1);
        bestaetigenUnityUI.localScale = new Vector3(0, 0, 0);

        var bogenZ02Color = bogenZ02.color;
        bogenZ02Color.a = 0;
        bogenZ02.color = bogenZ02Color;
        bogenGrad.faceColor = new Color(255, 52, 103, 0);
        hgMeridianDresdenGrad.faceColor = new Color(255, 52, 103, 0);
        bogenZ01.color = bogenZ02Color;
        bogenGradZ01.color = new Color(255, 52, 103, 0);

        Color cylinderAchseZ01Color = cylinderAchseZ01.color;
        cylinderAchseZ01Color.a = 0;
        cylinderAchseZ01.color = cylinderAchseZ01Color;

        bestaetigenButton.SetActive(false);
        foreach (var gO in traceParentsN02) {
            var tr = (TrailController) gO.GetComponent(typeof(TrailController));
            tr.StopTracing();
        }

        starrySkyAnimator.Rebind();
        starrySkyAnimator.speed = 1.0f;

        cylinderZ02.SetActive(false);
        eastGo.SetActive(true);
        mondVolvelleLupe.alpha = 0;
        var moonVolvelleHighlightColor = moonVolvelleHighlight.color;
        moonVolvelleHighlightColor.a = 0;
        moonVolvelleHighlight.color = moonVolvelleHighlightColor;
        groundDirection.localPosition = new Vector3(0, 0, 0);
        horizontLilaTransf.localPosition = new Vector3(0, 0, 0);
        var colorHand = handSprite.color;
        colorHand.a = 0;
        handSprite.color = colorHand;

        var zaehneColor = zaehne.color;
        zaehneColor.a = 0;
        zaehne.color = zaehneColor;

        var datumsanzeigeColor = datumsanzeige.color;
        datumsanzeigeColor.a = 0;
        datumsanzeige.color = datumsanzeigeColor;
        einstellring.color = datumsanzeigeColor;
        griff.color = datumsanzeigeColor;
        niete.color = datumsanzeigeColor;
        zeiger.color = datumsanzeigeColor;

        timetextTransf.localScale = new Vector3(1, 1, 1);
        cylinderN05Input.SetActive(false);
        moonSymbol.alpha = 0;
        moonSymbulTransf.localScale = new Vector3(0, 0, 0);
        sunSymbol.alpha = 0;
        sunSymbulTransf.localScale = new Vector3(0, 0, 0);
        overview.alpha = 0;
        overViewtransf.localScale = new Vector3(0, 0, 0);
        aEHN03.StopSetTextColorWithAnim();
        var text3dTageColor = text3dTage.color;
        text3dTageColor.a = 0;
        text3dTage.color = text3dTageColor;

        var newLineCylinderColor = newLineCylinder.color;
        newLineCylinderColor.a = 0;
        newLineCylinder.color = newLineCylinderColor;

        outOfSphereSkymat.color = new Color(1, 1, 1, 0);
        cpc.ResetScript();
        einstellringPivot.localPosition = new Vector3(-430, 377, 110.5f);
        hgc.StopMoveToTarget();
        hgc.ResetHimmelsglobus();
        startUiLineRenderer.enabled = false;
        newPathUILineRenderer.enabled = false;
        wintertrail.positionCount = 0;
        wintertrailUhr.positionCount = 0;
        sumerTrail.positionCount = 0;
        sumerTrailUhr.positionCount = 0;
        try {
            cSSHWeekCalendar.ResetWeekCalenderForN03();
        } catch (NullReferenceException ex) {
        }

        weekCalendar.SetActive(false);
        fillCircleImage.fillAmount = 0;
        directions.SetActive(true);
        skyTimeController.SetUtc(1);
        starrySkyCompassPivot.localEulerAngles = new Vector3(0, 0, 0);

        himmelsglobusAnimator.Rebind();
        himmelsglobusAnimator.Update(0f);
        himmelsglobusAnimator.enabled = false;

        Color hgMoonSphereColor = moonSphereHighlight.color;
        hgMoonSphereColor.a = 0f;
        moonSphereHighlight.color = hgMoonSphereColor;

        var hgGlobusMatColor = hgGlobusMat.color;
        hgGlobusMatColor.a = 0;
        hgGlobusMat.color = hgGlobusMatColor;
        hgGlobus.sharedMaterial = hgGlobusMat;

        cameraPivotX.localPosition = new Vector3(0, 0, 0);
        cameraPviotZ.localEulerAngles = new Vector3(0, 0, 0);
        cameraPivotX.localEulerAngles = new Vector3(0, 0, 0);
        calcedShadowObjectTR.emitting = false;
        calcedShadowObjectTR.time = 0;
        var gridGroundS03Color = gridGroundS03.color;
        gridGroundS03Color.a = 1;
        gridGroundS03.color = gridGroundS03Color;

        Color gitternetzkugelN05Color = gitternetzkugelN05.color;
        gitternetzkugelN05Color.a = 0;
        gitternetzkugelN05.color = gitternetzkugelN05Color;

        Color zeigerWhiteColor = zeigerWhite.color;
        zeigerWhiteColor.a = 0;
        zeigerWhite.color = zeigerWhiteColor;

        var gridGroundZ02Color = gridGroundZ02.color;
        gridGroundZ02Color.a = 1;
        gridGroundZ02.color = gridGroundZ02Color;

        n01ArrowParent.SetActive(false);
        himmelsglobusobject.SetActive(false);
        var hgMarkerColor = hgMarker.color;
        hgMarkerColor.a = 0;
        hgMarker.color = hgMarkerColor;

        var hgBodenplatteColor = hgBodenplatte.color;
        hgBodenplatteColor.a = 0;
        hgBodenplatte.color = hgBodenplatteColor;

        var hgEGHorizontColor = hgEGHorizont.color;
        hgEGHorizontColor.a = 0;
        hgEGHorizont.color = hgEGHorizontColor;

        var hgGestellGoldColor = hgGestellGold.color;
        hgGestellGoldColor.a = 0;
        hgGestellGold.color = hgGestellGoldColor;

        var hgKompassColor = hgKompass.color;
        hgKompassColor.a = 0;
        hgKompass.color = hgKompassColor;

        var hgSockelColor = hgSockel.color;
        hgSockelColor.a = 0;
        hgSockel.color = hgSockelColor;

        hgErdglobusUhr.SetActive(false);
        var hgErdglobusColor = hgErdglobus.color;
        hgErdglobusColor.a = 0;
        hgErdglobus.color = hgErdglobusColor;

        var hgErdglobusMeridianColor = hgErdglobusMeridian.color;
        hgErdglobusMeridianColor.a = 0;
        hgErdglobusMeridian.color = hgErdglobusMeridianColor;

        var hgBeineGoldColor = hgBeineGold.color;
        hgBeineGoldColor.a = 0;
        hgBeineGold.color = hgBeineGoldColor;
        hgBeineflachesObjekt.color = hgBeineGoldColor;
        hgDrachenfluegel.color = hgBeineGoldColor;
        hgDrachenkopf.color = hgBeineGoldColor;
        hgHalterung.color = hgBeineGoldColor;
        hgSphereGestell.color = hgBeineGoldColor;

        burgerMenuPfadBeendenText.color = new Color32(61, 61, 67, 255);
        burgerMenuButton.interactable = false;

        var ekliptikMarkerColor = ekliptikMarker.color;
        ekliptikMarkerColor.a = 0;
        ekliptikMarker.color = ekliptikMarkerColor;

        var earthFigueColor = earthFigue.color;
        earthFigueColor.a = 1;
        earthFigue.color = earthFigueColor;

        Color sunDialKompassNadelColor = new Color32(183, 157, 96, 255);
        sunDialKompassNadel.color = sunDialKompassNadelColor;
        var sunDialGroundPlateMatColor = sunDialGroundPlateMatTrans.color;
        sunDialGroundPlateMatColor.a = 1;
        sunDialGroundPlateMatTrans.color = sunDialGroundPlateMatColor;

        var sunDialWoodMatTransColor = sunDialWoodMatTrans.color;
        sunDialWoodMatTransColor.a = 1;
        sunDialWoodMatTrans.color = sunDialWoodMatTransColor;

        sunDialGroundPlate.sharedMaterial = sunDialGroundPlateMatOpaque;
        sunDialWood.sharedMaterial = sunDialWoodMatOpaque;

        var sunLineSommerBlueColor = sunLineSommerBlue.color;
        sunLineSommerBlueColor.a = 0;
        sunLineSommerBlue.color = sunLineSommerBlueColor;

        var sunLineWinterBlueColor = sunLineWinterBlue.color;
        sunLineWinterBlueColor.a = 0;
        sunLineWinterBlue.color = sunLineWinterBlueColor;

        var sunLineMarchBlueColor = sunLineMarchBlue.color;
        sunLineMarchBlueColor.a = 0;
        sunLineMarchBlue.color = sunLineMarchBlueColor;

        var ekliptikBogenColor = ekliptikBogen.color;
        ekliptikBogenColor.a = 0f;
        ekliptikBogen.color = ekliptikBogenColor;
        ekliptikBogenHinten.color = ekliptikBogenColor;

        var importantstarsS03Color = importantstarsS03.color;
        importantstarsS03Color.a = 0;
        importantstarsS03.color = importantstarsS03Color;

        southTransf.localPosition = new Vector3(0, 130, -2000);
        south.fontSize = 1500;
        s03_1Winter.Clear();
        s03_1Winter.emitting = false;
        s03_1Winter.time = 0;
        s03_1Winter.enabled = false;

        s03_2March.Clear();
        s03_2March.emitting = false;
        s03_2March.time = 0;
        s03_2March.enabled = false;

        s03_3Summer.Clear();
        s03_3Summer.emitting = false;
        s03_3Summer.time = 0;
        s03_3Summer.enabled = false;

        s03_4Sept.Clear();
        s03_4Sept.emitting = false;
        s03_4Sept.time = 0;
        s03_4Sept.enabled = false;

        s03_1WinterBlue.Clear();
        s03_1WinterBlue.emitting = false;
        s03_1WinterBlue.time = 0;
        s03_1WinterBlue.enabled = false;

        s03_2MarchBlue.Clear();
        s03_2MarchBlue.emitting = false;
        s03_2MarchBlue.time = 0;
        s03_2MarchBlue.enabled = false;

        s03_3SummerBlue.Clear();
        s03_3SummerBlue.emitting = false;
        s03_3SummerBlue.time = 0;
        s03_3SummerBlue.enabled = false;

        s03_4SeptBlue.Clear();
        s03_4SeptBlue.emitting = false;
        s03_4SeptBlue.time = 0;
        s03_4SeptBlue.enabled = false;

        skyParentTransf.localEulerAngles = new Vector3(0, 0, 0);
        pfeil1.SetActive(false);
        pfeil2.SetActive(false);
        pfeil3.SetActive(false);
        pfeil4.SetActive(false);
        var hgArmGoldColor = hgArmGold.color;
        var hgUhr24Color = hg24Uhr.color;
        var hgEkliptikColor = hgEkliptik.color;
        var hgGoldColor = hgGold.color;
        var hg24UhrgoldBodyColor = hgGold24UhrBody.color;
        var hgSonneColor = hgGoldSonne.color;
        var hgUhrUntenColor = hgUhrUnten.color;

        hgMoonGlowHalf.color = new Color(0.49f, 0.3215f, 0, 1);
        hgGoldSonneOpaque.color = new Color(0.49f, 0.3215f, 0, 1);

        hgUhr24Color.a = 0.00f;
        hg24Uhr.color = hgUhr24Color;
        hg24Uhr_2.color = hgUhr24Color;
        hgEkliptikColor.a = 0.00f;
        hgEkliptik.color = hgEkliptikColor;
        hgGoldColor.a = 0.00f;
        hgGold.color = hgGoldColor;
        hg24UhrgoldBodyColor.a = 0.00f;
        hgGold24UhrBody.color = hg24UhrgoldBodyColor;
        hgSonneColor.a = 0.00f;
        hgGoldSonne.color = hgSonneColor;
        hgUhrUntenColor.a = 0.00f;
        hgUhrUnten.color = hgUhrUntenColor;
        hgArmGoldColor.a = 0;
        hgArmGold.color = hgArmGoldColor;
        var hgHorizontColor = hgHorizont.color;
        hgHorizontColor.a = 0;
        hgHorizont.color = hgHorizontColor;
        hgHorizont2.color = hgHorizontColor;

        var hgMeridianColor = hgMeridian.color;
        hgMeridianColor.a = 0;
        hgMeridian.color = hgMeridianColor;

        summerText.SetActive(false);
        winterText.SetActive(false);
        var sunLineSommerColor = sunLineSommer.color;
        sunLineSommerColor.a = 0;
        sunLineSommer.color = sunLineSommerColor;

        var sunLineWinterColor = sunLineWinter.color;
        sunLineWinterColor.a = 0;
        sunLineWinter.color = sunLineWinterColor;

        var sundDialKompassColor = sunDialKompass.color;
        sundDialKompassColor.a = 0;
        sunDialKompass.color = sundDialKompassColor;
        var planeGroundPrio0Color = planeGroundPrio0.color;
        planeGroundPrio0Color = new Color32(24, 24, 27, 255);
        planeGroundPrio0.color = planeGroundPrio0Color;

        planeGroundMR.sharedMaterial = planeGroundPrio0;
        mainMenuAnimator.enabled = false;
        gridGround.sortingOrder = 0;
        var gridGroundColor = gridGround.color;
        gridGroundColor.a = 1;
        gridGround.color = gridGroundColor;

        gridTransf.localPosition = new Vector3(0.021f, -0.008f, 0);
        gridTransf.localScale = new Vector3(0.1338912f, 0.1338912f, 0.1338912f);
        sunDialTransf.localEulerAngles = new Vector3(0, 180f, 0);
        sunDialTransf.localPosition = new Vector3(0, -1.1f, 235.6f);
        sunDialTransf.localScale = new Vector3(0.0432f, 0.0432f, 0.0432f);
        dateText.faceColor = new Color32(255, 255, 255, 255);
        timeText.faceColor = new Color32(255, 255, 255, 255);
        figureEarthTransf.localEulerAngles = new Vector3(0, -90, 39);
        var ekliptikOnHGColor = ekliptikOnHG.color;
        ekliptikOnHGColor.a = 0;
        ekliptikOnHG.color = ekliptikOnHGColor;

        var krebsRahmenSpriteColor = krebsRahmenSprite.color;
        krebsRahmenSpriteColor.a = 0;
        krebsRahmenSprite.color = krebsRahmenSpriteColor;

        var krebsLupeSpriteColor = krebsLupeSprite.color;
        krebsLupeSpriteColor.a = 0;
        krebsLupeSprite.color = krebsLupeSpriteColor;

        krebsRahmen.SetActive(false);
        krebsLupe.transform.localScale = new Vector3(0, 0, 0);
        blockImage.SetActive(false);

        uhr24HGameobject.SetActive(false);
        var uhr24HColor = uhr24H.color;
        uhr24HColor.a = 0;
        uhr24H.color = uhr24HColor;

        var griffBlackBackgroundColor = griffBlackBackground.color;
        griffBlackBackgroundColor.a = 0;
        griffBlackBackground.color = griffBlackBackgroundColor;
        aussenringBlackBackground.color = griffBlackBackgroundColor;
        schabloneBlackBackground.color = griffBlackBackgroundColor;

        //sundial Mat
        var suHolzColor = suHolz.color;
        suHolzColor.a = 1;
        suHolz.color = suHolzColor;

        var suPlatteColor = suPlatte.color;
        suPlatteColor.a = 1;
        suPlatte.color = suPlatteColor;

        var suVolvelleColor = suVolvelle.color;
        suVolvelleColor.a = 1;
        suVolvelle.color = suVolvelleColor;

        var suFadenColor = suFaden.color;
        suFadenColor.a = 1;
        suFaden.color = suFadenColor;

        var suKugelColor = suKugel.color;
        suKugelColor.a = 1;
        suKugel.color = suKugelColor;

        var suMetallColor = suMetall.color;
        suMetallColor.a = 1;
        suMetall.color = suMetallColor;

        sunConstanTR.time = 0;
        sunConstanTR.enabled = false;
        extraLight.enabled = false;

        sunLightPivot.localPosition = new Vector3(0, 0, 0);
        groundPlaneTransform.localPosition = new Vector3(0, 0, 0);

        globusLight.intensity = 0;

        var hgHimmelskugelSonneColor = hgHimmelskugelSonne.color;
        hgHimmelskugelSonneColor.a = 0;
        hgHimmelskugelSonne.color = hgHimmelskugelSonneColor;

        var horizontMatColor = horizontMat2.color;
        horizontMatColor.a = 0;
        horizontMat2.color = horizontMatColor;

        var hgHimmelskugelColor = hgHimmelskugel.color;
        hgHimmelskugelColor.a = 0;
        hgHimmelskugel.color = hgHimmelskugelColor;

        var ekliptikLineZ02Color = ekliptikLineZ02.color;
        ekliptikLineZ02Color.a = 0;
        ekliptikLineZ02.color = ekliptikLineZ02Color;

        var tIC = (TimeInputController) timeParent.GetComponent(typeof(TimeInputController));
        tIC.ResetScript();

        splitLine.anchoredPosition = new Vector2(splitLine.anchoredPosition.x, 0);
        lD.intensity.Override(0.4f);
        lD.xMultiplier.Override(1.0f);
        lD.scale.Override(1.04f);
        s03_1TR.enabled = false;
        s03_2TR.enabled = false;
        s03_3TR.enabled = false;

        s03_1TR.emitting = false;
        s03_2TR.emitting = false;
        s03_3TR.emitting = false;

        s03_1.SetActive(false);
        s03_2.SetActive(false);
        s03_3.SetActive(false);

        sunlightPivot.localEulerAngles = new Vector3(0f, 0, 0);
        var kalenderKrebsHighlightColor = kalenderKrebsHighlight.color;
        kalenderKrebsHighlightColor.a = 0;
        kalenderKrebsHighlight.color = kalenderKrebsHighlightColor;
        kalenderlinienKrebs.SetActive(false);

        var kalenderOnSundDialSpriteColor = kalenderOnSundDialSprite.color;
        kalenderOnSundDialSpriteColor.a = 1;
        kalenderOnSundDialSprite.color = kalenderOnSundDialSpriteColor;

        kalender.SetActive(true);
        kalenderlinien.SetActive(true);
        sundDirectionalLightTransf.localPosition = new Vector3(0, 7.55f, 0);
        fakeSun.GetComponent<MeshRenderer>().enabled = false;
        fakeSun.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

        var fakeSunMatColor = fakeSunMat.color;
        fakeSunMatColor.a = 0;
        fakeSunMat.color = fakeSunMatColor;

        ekliptikTrace.Clear();
        ekliptikTrace.enabled = false;
        var breitengradSterneHighlightColor = breitengradSterneHighlight.color;
        breitengradSterneHighlightColor.a = 0;
        breitengradSterneHighlight.color = breitengradSterneHighlightColor;

        sRC.enabled = true;

        var sunLineHighlightOverHorizontColor = sunLineHighlightOverHorizont.color;
        sunLineHighlightOverHorizontColor.a = 0;
        sunLineHighlightOverHorizont.color = sunLineHighlightOverHorizontColor;

        var fadenHiglightLongColor = fadenHiglightLong.color;
        fadenHiglightLongColor.a = 0;
        fadenHiglightLong.color = fadenHiglightLongColor;

        skyCameraParent.SetActive(true);
        mainCamObject.SetActive(true);
        mainCamChildObject.SetActive(true);
        mainCam.fieldOfView = 40;

        nokturnalController.NokturnalOff();
        nokturnalController.InitEinstellring();
        nokturnalCamera.SetActive(false);
        nokturnal.localEulerAngles = new Vector3(0, 0, 0);
        nokturnal.localPosition = new Vector3(0, 1, 1.95f);
        nokturnal.localScale = new Vector3(1, 1, 1);

        moonLight.SetActive(true);


        sunDialAnimator.enabled = true;
        sunDialAnimator.Rebind();
        sunDialAnimator.Update(0f);

        skyCameraParentAnimator.enabled = true;
        skyCameraParentAnimator.Rebind();
        skyCameraParentAnimator.Update(0f);
        skyCameraParentAnimator.enabled = false;

        uRPA.msaaSampleCount = 1;

        skyCam.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = true;
        groundPlane.material = groundMat;
        float hour = DateTime.Now.Hour;
        var minute = DateTime.Now.Minute * 1.66f / 100;
        var aktTime = hour + minute;
        skyTimeController.SetTimeline(aktTime);
        mmc.ResetScript();
        mainMenu.alpha = 1f;
        mainMenuTransf.localScale = new Vector3(1, 1, 1);
        mmc.ShowMainMenu();

        if (lastObject == 0) {
            if (mmc.GetN01Done() && !mmc.GetFinishNokturnalState()) {
                mmc.ActivateNokturnalPath();
            } else {
                nokturnalImageMMC.interactable = true;
            }
        } else if (lastObject == 1) {
            if (mmc.GetS01Done() && !mmc.GetfinishSonnenuhrState()) {
                mmc.ActivateSonnenUhrPath();
            } else {
                sonnenuhrImageMMC.interactable = true;
            }
        } else if (lastObject == 2) {
            mmc.ActivateHimmelsglobusPath();
        }

        sPEH.ResetScript();

        sunDirectLight.enabled = true;
        moonDirectLight.enabled = false;

        var oCC = (OrbitalCameraController) mainCamObject.GetComponent(typeof(OrbitalCameraController));
        oCC.StopRotation();
        oCC.SetTouchSpeedFactor(1.0f);
        var cc = (ClockController) clockParent.GetComponent(typeof(ClockController));
        cc.SetRealTime();
        cc.ContinueUpdateDate();
        clockParentTransf.localScale = new Vector3(1, 1, 1);

        sunDialCam.rect = new Rect(0, 0, 1, 0);
        mainCam.rect = new Rect(0, 0, 1, 1);
        earthTransform.localScale = new Vector3(0, 0, 0);

        var laengenGradNetzColor = laengenGradNetz.color;
        laengenGradNetzColor.a = 0;
        laengenGradNetz.color = laengenGradNetzColor;

        sunDialCameraObject.SetActive(false);
        sunDialObject.SetActive(false);
        sunDialParent.SetActive(false);
        sunDialParent.transform.localEulerAngles = new Vector3(0, 0, 0);
        skyTimeController.StopSetTimeLineWithAnim();

        skyAnimator.Rebind();
        skyAnimator.Update(0f);
        skyAnimator.SetInteger("StartAnimateTimeline", 0);
        skyAnimator.speed = 1;
        skyAnimator.enabled = false;

        skyCam.fieldOfView = 40.0f;

        cameraAnimator.enabled = false;
        cameraContainer.localEulerAngles = new Vector3(0, 0, 0);
        cameraContainer.localPosition = new Vector3(0, 3, 0);
        mainCamObject.transform.localPosition = new Vector3(0, 0, 0);
        mainCamObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        mainCamTransform.localPosition = new Vector3(0, 0, 0);
        mainCamTransform.localEulerAngles = new Vector3(0, 0, 0);
        skyCameraCenter.localEulerAngles = new Vector3(0, 0, 0);
        skyCameraCenter.localPosition = new Vector3(0, 0, 0);
        skyCameraXPivot.localEulerAngles = new Vector3(0, 0, 0);
        skyCameraXPivot.localPosition = new Vector3(0, 0, 0);

        skyCamera.localEulerAngles = new Vector3(0, 0, 0);
        skyCamera.localPosition = new Vector3(0, 0, 0);
        starrySkyParent.localPosition = new Vector3(0, 0, 0);
        sunLightPivot.localPosition = new Vector3(0, 0, 0);

        var horizontScheibeColor = horizontScheibe.color;
        horizontScheibeColor.a = 0;
        horizontScheibe.color = horizontScheibeColor;

        var sunDialSternzeichenKrebsHighlightColor = sunDialSternzeichenKrebsHighlight.color;
        sunDialSternzeichenKrebsHighlightColor.a = 0;
        sunDialSternzeichenKrebsHighlight.color = sunDialSternzeichenKrebsHighlightColor;

        var sunDialSternzeichenColor = sunDialSternzeichen.color;
        sunDialSternzeichenColor.a = 0;
        sunDialSternzeichen.color = sunDialSternzeichenColor;

        var schattenwerferHighlightColor = schattenwerferHighlight.color;
        schattenwerferHighlightColor.a = 0;
        schattenwerferHighlight.color = schattenwerferHighlightColor;

        var sunMarkerColor = sunMarker.color;
        sunMarkerColor.a = 0;
        sunMarker.color = sunMarkerColor;

        var sunLineColor = sunLineMat.color;
        sunLineColor.a = 0;
        sunLineMat.color = sunLineColor;

        var colorGround = planeGround.color;
        colorGround.a = 1;
        planeGround.color = colorGround;

        north.faceColor = new Color(255, 255, 255, 255);
        east.faceColor = new Color(255, 255, 255, 255);
        south.faceColor = new Color(255, 255, 255, 255);
        west.faceColor = new Color(255, 255, 255, 255);

        var sunDialHighlight16Color = sunDialHighlight16.color;
        sunDialHighlight16Color.a = 0;
        sunDialHighlight16.color = sunDialHighlight16Color;

        var schattenwerferColor = schattenwerferMat.color;
        schattenwerferColor.a = 0;
        schattenwerferMat.color = schattenwerferColor;

        var fadenColor = fadenMat.color;
        fadenColor.r = 0.45f;
        fadenColor.g = 0.45f;
        fadenColor.b = 0.45f;
        fadenMat.color = fadenColor;

        var sDRadial = sunDialRadialLines.color;
        sDRadial.a = 0;
        sunDialRadialLines.color = sDRadial;

        var sunColor = sun.color;
        sunColor.a = 0;
        sun.color = sunColor;

        var earthColor = earthMaterial.color;
        earthColor.a = 0;
        earthMaterial.color = earthColor;

        var colorIS = importantStars.color;
        colorIS.a = 0f;
        importantStars.color = colorIS;
        importantStarsSchuettkantenStern.color = colorIS;

        var colorP = materialPolarstern.color;
        colorP.a = 0f;
        materialPolarstern.color = colorP;

        var starsBS = matStar_Breitengradsterne.color;
        starsBS.a = 0f;
        matStar_Breitengradsterne.color = starsBS;
        footUI.alpha = 0;
        headUI.alpha = 0;
        InitTime();
        rS.StopRotation();
        rS.CalculateRotationOneTime();

        var frontZahlenFullColor = frontZahlenFull.color;
        frontZahlenFullColor.a = 0;
        frontZahlenFull.color = frontZahlenFullColor;

        sonnenUhrAmbientLight.SetActive(true);
    }

    public void SetLastObject(int lastObjectId) {
        lastObject = lastObjectId;
    }
}