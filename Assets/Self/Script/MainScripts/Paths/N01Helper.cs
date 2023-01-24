using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class N01Helper : MonoBehaviour {
    
    #region public Variables

    //Scripts
    public AnimationEventHandlerCameraN01 aEHN01;
    public OrbitalCameraController oCC;
    public SkyTimeController skyTimeController;
    public SkyProfileEventHandler sPEH;
    public CheckPointController cPC;
    public ClockController cc;
    public InitScene iSN;
    public NokturnalController nokturnalController;
    public N02Helper n02H;
    public ProcedureController pC;
    public RotateStarrSky rS;
    public RotateObjectTowards rOT;
    public Translator translator;

    //other Objects
    public Animator cameraAnimator;
    public Animator StarrySkyAnimator;

    public CanvasGroup fillCircleCG;

    public Camera camNokturnalPivot;
    public Camera mainCamera;
    public Camera skyCam;

    public GameObject arrowParent;
    public GameObject frontZahlenFullgo;
    public GameObject mainCam;
    public GameObject nokturnalCamera;
    public GameObject nokturnalZeiger;
    public GameObject schuettKantenSternTracer;
    public List<GameObject> traceParents = new List<GameObject>();

    public Image fillCirclePolarstern;
    public Image fillCircleProcess;

    public Material importantStars;
    public Material importantStarsSchuettkantenStern;
    public Material importantStarsPolar;
    public Material nokturnalGold;
    public Material nokturnalGoldZeiger;
    public Material nokturnalSilber;

    public MeshRenderer northObject;

    public SpriteRenderer einstellringZahlen;
    public SpriteRenderer frontZahlenFull;
    public SpriteRenderer frontMonatstage;
    public SpriteRenderer monatsTageFront;
    public SpriteRenderer nokturnalFrontVerzierung;
    public SpriteRenderer zeigerSeiteHighlightWeis;

    public Transform cameraContainer;
    public Transform frontZahlenTrans;
    public Transform gridTransf;
    public Transform leftArrow;
    public Transform mainCamParentTrnasf;
    public Transform nokturnal;
    public Transform nokturnalZeigerPivot;
    public Transform nokturnalZeigerPivotParent;
    public Transform nokturnalZeigerPivotChild;
    public Transform polarStar;
    public Transform rightArrow;
    public Transform schuettkantenStern_1;
    public Transform schuettkantenStern_2;
    public Transform starrySkyCompassPivot;
    public Transform skyCompassPivot;
    public Transform terrainCompassPivot;

    public Volume volume;

    #endregion

    #region private Variables

    private bool makeNokturnalTransparent = false;
    private bool checkNokturnalTargetPosition = false;
    private bool scaleNokturnalSmall = false;
    private bool fillCircleWhileCentered = false;
    private bool allowFillCheck = false;
    private bool showArrow = false;
    private bool flashHighlightGrosserWagen = false;
    private bool highlightGrosserWagenOn = false;
    private bool highlightGrosserWagenOff = false;
    private bool turnZeigerHighlightOn = false;
    private bool flashSchuettkantenStern = false;
    private bool highlightSchuettkantenSternOn = false;
    private bool highlightSchuettkantenSternOff = false;
    private bool makeNokturnalZeigerTransparent = false;
    private bool showNokturnal = false;
    private bool checkNokturnalPositionAfterFirstInput = false;

    private int lastcheckPoint = 1;

    private LensDistortion lD;

    #endregion

    private void Start() {
        LensDistortion tmp;

        if (volume.profile.TryGet(out tmp)) {
            lD = tmp;
        }

        ProcedureController.changeEvent += DoActionWhileStepUpdate;
    }

    //Bearbeitung aller Schritte mit der Bezeichnung N01.XX
    public void DoActionWhileStepUpdate(string stepId) {
        switch (stepId) {
            case "N01.00a":
                pC.JumpToNextStep();
                break;

            case "N01.02":
                ResetScript();
                mainCam.transform.localEulerAngles = new Vector3(0, 0, 0);
                cameraContainer.localEulerAngles = new Vector3(0, 0, 0);

                Color importantStarsColor = importantStars.color;
                importantStarsColor.a = 0;
                importantStars.color = importantStarsColor;
                importantStarsSchuettkantenStern.color = importantStarsColor;

                flashHighlightGrosserWagen = false;
                highlightGrosserWagenOn = false;
                highlightGrosserWagenOff = false;

                oCC.SetTouchSpeedFactor(1.0f);
                oCC.AllowRotating();
                break;

            case "N01.02a":
                //Sonne ausblenden und Sterne sichtbar machen
                sPEH.StopUpdatingPropertiesAndTurnSunOff();
                //kurz warten bis Veränderungen am Himmel passiert sind, dann zum nächsten Schritt springen
                StartCoroutine(WaitBeforJump());
                break;

            case "N01.03":
                //Erlauben einen roten Kreis für falschen Input zu setzen
                oCC.AllowSetWrongInput();
                //Pfeil anzeigen am Bildrand, wenn Kamera zu weit entfernt von Norden
                showArrow = true;
                break;

            #region Checkpoint 1

            case "N01.15C":
                ResetScript();
                StopAllCoroutines();
                StopFlashHighlightGrosserWagen();

                oCC.AllowSetWrongInput();
                oCC.AllowRotating();
                sPEH.StopUpdatingPropertiesAndTurnSunOff();
                n02H.ResetScript();

                importantStarsColor = importantStars.color;
                importantStarsColor.a = 1f;
                importantStars.color = importantStarsColor;
                importantStarsSchuettkantenStern.color = importantStarsColor;

                Color importantStarsPolarColor = importantStarsPolar.color;
                importantStarsPolarColor.a = 0f;
                importantStarsPolar.color = importantStarsPolarColor;

                nokturnalController.InitEinstellring();
                pC.JumpToNextStep();
                break;

            #endregion

            case "N01.15":
                //Checkpoint erreicht
                cPC.SetCheckPointColors("N01", 2);
                cPC.SetCheckPointReached("N01", 02);
                lastcheckPoint = 2;

                StopAllCoroutines();
                StopFlashHighlightGrosserWagen();
                break;

            case "N01.22":
                cPC.SetCheckPointColors("N01", 3);
                cPC.SetCheckPointReached("N01", 03);
                lastcheckPoint = 3;
                oCC.StopAllowSetWrongInput();
                break;

            case "N01.23A":
                //Nokturnal einblenden
                nokturnalCamera.SetActive(true);
                showNokturnal = true;
                nokturnalController.InitEinstellring();
                nokturnal.transform.localScale = new Vector3(0.9691705f, 0.9691705f, 0.9691705f);
                pC.JumpToNextStep();
                break;

            #region Checkpoint 2

            case "N01.24C":
                ResetScript();
                n02H.ResetScript();
                StopAllCoroutines();
                StopFlashHighlightGrosserWagen();

                nokturnalController.InitEinstellring();
                oCC.AllowRotating();
                sPEH.StopUpdatingPropertiesAndTurnSunOff();

                if (lastcheckPoint > 3) {
                    oCC.ResetVariables();
                    mainCam.transform.localEulerAngles = new Vector3(0, 0, 0);
                }

                Color importantStarsColor2 = importantStars.color;
                importantStarsColor2.a = 1f;
                importantStars.color = importantStarsColor2;
                importantStarsSchuettkantenStern.color = importantStarsColor2;
                importantStarsPolar.color = importantStarsColor2;
                nokturnal.transform.localPosition = new Vector3(0, 1, nokturnal.transform.localPosition.z);
                nokturnal.transform.localScale = new Vector3(1, 1, 1);

                Color nokturnalGoldcolor = nokturnalGold.color;
                Color nokturnalGoldZeigerColor = nokturnalGoldZeiger.color;
                Color nokturnalSilbercolor = nokturnalSilber.color;
                Color monatstageColor = frontMonatstage.color;
                Color einstellringZahlenColor = einstellringZahlen.color;
                Color nokturnalFrontVerzierungColor = nokturnalFrontVerzierung.color;
                nokturnalSilbercolor.a = 1f;
                nokturnalGoldcolor.a = 1;
                nokturnalGoldZeigerColor.a = 1f;
                monatstageColor.a = 1f;
                einstellringZahlenColor.a = 1f;
                nokturnalFrontVerzierungColor.a = 0.55f;
                nokturnalSilber.color = nokturnalSilbercolor;
                nokturnalGold.color = nokturnalGoldcolor;
                nokturnalGoldZeiger.color = nokturnalGoldZeigerColor;
                frontMonatstage.color = monatstageColor;
                einstellringZahlen.color = einstellringZahlenColor;
                nokturnalFrontVerzierung.color = nokturnalFrontVerzierungColor;

                nokturnalCamera.SetActive(true);
                pC.JumpToNextStep();
                break;

            #endregion

            case "N01.25":
                cameraAnimator.Play("ScaleNokturnalBig_N01", 0, 0);
                cameraAnimator.speed = 1;
                cameraAnimator.SetInteger("StateN01", 1);
                makeNokturnalTransparent = true;
                break;

            case "N01.27":
                //canvas Gruppe des Kreises um der Niete sichtbar machen
                fillCircleCG.alpha = 1;

                //Prüfen der richtigen Nokturnalposition und füllen des Kreises um der Niete
                allowFillCheck = true;
                checkNokturnalTargetPosition = true;

                showArrow = false;

                //Die Bewegung am Himmel verlangsamen, zur besseren Justierung
                oCC.SetTouchSpeedFactor(0.5f);
                break;

            case "N01.38":
                cameraContainer.transform.localEulerAngles = new Vector3(0, 0, 0);
                float resolutionFactor = DeviceInfo.GetResolutionFactor() - 0.45f;
                float tempX = -34f - (resolutionFactor * 2 / 0.3f);
                mainCam.transform.localEulerAngles = new Vector3(tempX, 0, 0);
                starrySkyCompassPivot.localEulerAngles = new Vector3(0.0f, 0, 0.0f);
                skyCompassPivot.localEulerAngles = new Vector3(0.0f, 0, 0.0f);
                terrainCompassPivot.localEulerAngles = new Vector3(0.0f, 0, 0.0f);

                cameraAnimator.Play("ScaleNokturnalSmall_N01", 0, 0);
                cameraAnimator.speed = 1;
                cameraAnimator.SetInteger("StateN01", 2);

                turnZeigerHighlightOn = true;
                flashSchuettkantenStern = true;
                highlightSchuettkantenSternOn = true;
                RotateObjectTowards rOT =
                    (RotateObjectTowards) nokturnalZeiger.GetComponent(typeof(RotateObjectTowards));
                rOT.AllowRotationVariableWrapper();
                break;

            case "N01.43":
            case "N01.43a":
                cameraAnimator.Play("ZoomToNokturnal_N01", 0, 0);
                cameraAnimator.speed = 1;
                cameraAnimator.SetInteger("StateN01", 3);
                makeNokturnalZeigerTransparent = true;
                Vector2 time = skyTimeController.GetTimeOfDay();
                int temp = (int) time.x;

                if (temp >= 8 && temp <= 16) {
                    frontZahlenFullgo.transform.localPosition = new Vector3(0.04f, 0.05f, -0.08f);

                    frontZahlenFullgo.SetActive(true);
                    Color frontZahlenFullColor = frontZahlenFull.color;
                    frontZahlenFullColor.a = 0.7f;
                    frontZahlenFull.color = frontZahlenFullColor;
                }

                frontZahlenTrans.localPosition = new Vector3(0, 0.04f, -0.08f);
                flashSchuettkantenStern = false;

                schuettkantenStern_1.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                schuettkantenStern_2.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                break;

            case "mainMenu":
                mainCam.SetActive(true);

                iSN.InitMenu();

                foreach (GameObject gO in traceParents) {
                    gO.GetComponent<TrailRenderer>().emitting = false;
                    TrailController tr = (TrailController) gO.GetComponent(typeof(TrailController));
                    tr.StopTracing();
                }

                nokturnalCamera.SetActive(false);
                break;
        }
    }
    
    private void Update() {
        //Blende den Hilfspfeil am rechten oder linken Bildrand ein, wenn das N am Horizont (northObject) nicht im Bild zu sehen ist
        if (showArrow) {
            if (!GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main),
                northObject.bounds)) {
                if (terrainCompassPivot.localEulerAngles.y < 180) {
                    if (cameraContainer.transform.localEulerAngles.y > terrainCompassPivot.localEulerAngles.y &&
                        cameraContainer.transform.localEulerAngles.y < terrainCompassPivot.localEulerAngles.y + 180) {
                        leftArrow.localScale = new Vector3(0.18489f, 0.04315f, 1);
                        rightArrow.localScale = new Vector3(0, 0, 0);
                    } else {
                        leftArrow.localScale = new Vector3(0, 0, 0);
                        rightArrow.localScale = new Vector3(0.18489f, 0.04315f, 1);
                    }
                } else {
                    if (cameraContainer.transform.localEulerAngles.y < terrainCompassPivot.localEulerAngles.y &&
                        cameraContainer.transform.localEulerAngles.y > terrainCompassPivot.localEulerAngles.y - 180) {
                        leftArrow.localScale = new Vector3(0, 0, 0);
                        rightArrow.localScale = new Vector3(0.18489f, 0.04315f, 1);
                    } else {
                        leftArrow.localScale = new Vector3(0.18489f, 0.04315f, 1);
                        rightArrow.localScale = new Vector3(0, 0, 0);
                    }
                }
            } else {
                leftArrow.localScale = new Vector3(0, 0, 0);
                rightArrow.localScale = new Vector3(0, 0, 0);
            }
        }

        //prüfen ob sich das Nokturnal in Zielposition befindet (Polarstern im Loch des Nokturnals)
        if (checkNokturnalTargetPosition) {
            if (Input.GetMouseButtonDown(1)) {
                if (!EventSystem.current.IsPointerOverGameObject()
#if !UNITY_EDITOR
                || EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId)
#endif
                ) {
                    //Mindestens ein Input erfolgt
                    checkNokturnalPositionAfterFirstInput = true;
                }
            }

            //Prüfung der Zielposition erfolgt erst nachdem mindestens ein Input erfolgt ist (Nokturnal am Himmel bewegt wurde)
            if (checkNokturnalPositionAfterFirstInput) {
                Vector3 screenPosPolarStar = mainCamera.WorldToScreenPoint(polarStar.position);
                Vector3 screenPosNokturnalPivot = camNokturnalPivot.WorldToScreenPoint(nokturnalZeigerPivot.position);
                screenPosPolarStar = new Vector3(screenPosPolarStar.x, screenPosPolarStar.y + 25, 0);
                screenPosNokturnalPivot = new Vector3(screenPosNokturnalPivot.x, screenPosNokturnalPivot.y, 0);
                float distance = Vector3.Distance(screenPosPolarStar, screenPosNokturnalPivot);
                //Toleranz von 20 Einheiten, wenn Nokturnal im Toleranzbereich, dann Bool-Variable = true
                if (distance < 20) {
                    fillCircleWhileCentered = true;
                } else {
                    fillCircleWhileCentered = false;
                }
            }
        }

        //Wenn Bool-Variable (fillCircleWhileCentered) aus oben stehender Prüfung true, dann wird der Kreis gefüllt
        if (allowFillCheck) {
            if (fillCircleWhileCentered) {
                fillCirclePolarstern.fillAmount += 0.02f;
                if (fillCirclePolarstern.fillAmount >= 1) {
                    //Position gefunden / Kreis vollständig gefüllt
                    StartCoroutine(FinishCheckNokturnalTargetPosition());
                    checkNokturnalTargetPosition = false;
                    allowFillCheck = false;
                }
            }
        }

        #region graphical actions

        if (showNokturnal) {
            Color monatsTageFrontColor = monatsTageFront.color;
            Color einstellringZahlenColor = einstellringZahlen.color;
            Color colorNG = nokturnalGold.color;
            Color nokturnalGoldZeigerColor = nokturnalGoldZeiger.color;
            Color colorSilber = nokturnalSilber.color;
            Color nokturnalFrontVerzierungColor = nokturnalFrontVerzierung.color;

            if (colorNG.a < 1f) {
                colorNG.a += 0.05f;
                nokturnalGoldZeigerColor.a += 0.05f;
                colorSilber.a += 0.05f;
                nokturnalFrontVerzierungColor.a += 0.025f;
                einstellringZahlenColor.a += 0.05f;
                monatsTageFrontColor.a += 0.05f;
                nokturnalGold.color = colorNG;
                nokturnalGoldZeiger.color = nokturnalGoldZeigerColor;
                nokturnalSilber.color = colorSilber;
                nokturnalFrontVerzierung.color = nokturnalFrontVerzierungColor;
                einstellringZahlen.color = einstellringZahlenColor;
                monatsTageFront.color = monatsTageFrontColor;
            } else {
                showNokturnal = false;
            }
        }

        if (flashHighlightGrosserWagen) {
            if (highlightGrosserWagenOn) {
                Color importantStarsColor = importantStars.color;

                if (importantStarsColor.a < 1) {
                    importantStarsColor.a += 0.05f;
                    importantStars.color = importantStarsColor;
                    importantStarsSchuettkantenStern.color = importantStarsColor;
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
                    importantStarsSchuettkantenStern.color = importantStarsColor;
                } else {
                    highlightGrosserWagenOn = true;
                    highlightGrosserWagenOff = false;
                }
            }
        }

        if (scaleNokturnalSmall) {
            if (nokturnal.transform.localScale.x > 0.5f) {
                nokturnal.transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
                nokturnal.transform.localPosition = new Vector3(0, nokturnal.transform.localPosition.y, 1);
            } else {
                scaleNokturnalSmall = false;
            }
        }

        if (makeNokturnalTransparent) {
            Color nokturnalGoldcolor = nokturnalGold.color;
            Color nokturnalGoldZeigerColor = nokturnalGoldZeiger.color;
            Color nokturnalSilbercolor = nokturnalSilber.color;
            Color monatstageColor = frontMonatstage.color;

            if (nokturnalSilbercolor.a > 0.275) {
                nokturnalSilbercolor.a -= 0.075f;
                nokturnalGoldcolor.a -= 0.075f;
                nokturnalGoldZeigerColor.a -= 0.075f;
                monatstageColor.a -= 0.075f;
                nokturnalSilber.color = nokturnalSilbercolor;
                nokturnalGold.color = nokturnalGoldcolor;
                nokturnalGoldZeiger.color = nokturnalGoldZeigerColor;
                frontMonatstage.color = monatstageColor;
            } else {
                pC.JumpToNextStep();
                makeNokturnalTransparent = false;
            }
        }

        if (makeNokturnalZeigerTransparent) {
            Color nokturnalGoldZeigerColor = nokturnalGoldZeiger.color;
            Color zeigerSeiteHighlightWeisColor = zeigerSeiteHighlightWeis.color;
            if (nokturnalGoldZeigerColor.a > 0.5) {
                nokturnalGoldZeigerColor.a -= 0.075f;
                nokturnalGoldZeiger.color = nokturnalGoldZeigerColor;
                zeigerSeiteHighlightWeisColor.a -= 0.075f;
                zeigerSeiteHighlightWeis.color = zeigerSeiteHighlightWeisColor;
            } else {
                makeNokturnalZeigerTransparent = false;
            }
        }

        if (turnZeigerHighlightOn) {
            Color zeigerSeiteHighlightWeisColor = zeigerSeiteHighlightWeis.color;
            if (zeigerSeiteHighlightWeisColor.a < 1) {
                zeigerSeiteHighlightWeisColor.a += 0.075f;
                zeigerSeiteHighlightWeis.color = zeigerSeiteHighlightWeisColor;
            } else {
                turnZeigerHighlightOn = false;
            }
        }

        if (flashSchuettkantenStern) {
            if (highlightSchuettkantenSternOn) {
                if (schuettkantenStern_1.localScale.x < 0.375f) {
                    schuettkantenStern_1.localScale += new Vector3(0.01f, 0.01f, 0.01f);
                    schuettkantenStern_2.localScale += new Vector3(0.01f, 0.01f, 0.01f);
                } else {
                    highlightSchuettkantenSternOff = true;
                    highlightSchuettkantenSternOn = false;
                }
            }

            if (highlightSchuettkantenSternOff) {
                if (schuettkantenStern_1.localScale.x > 0.275f) {
                    schuettkantenStern_1.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
                    schuettkantenStern_2.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
                } else {
                    highlightSchuettkantenSternOn = true;
                    highlightSchuettkantenSternOff = false;
                }
            }
        }

        #endregion
    }

    private void ResetScript() {
        #region Reset private Variables

        makeNokturnalTransparent = false;
        checkNokturnalTargetPosition = false;
        scaleNokturnalSmall = false;
        fillCircleWhileCentered = false;
        allowFillCheck = false;
        showArrow = false;
        fillCirclePolarstern.fillAmount = 0;
        flashSchuettkantenStern = false;
        highlightSchuettkantenSternOn = false;
        highlightSchuettkantenSternOff = false;
        checkNokturnalPositionAfterFirstInput = false;

        #endregion

        #region Reset Camera

        aEHN01.StopMoveWithAnimAEHN01();
        mainCamParentTrnasf.localPosition = new Vector3(0, 0, 0);

        float resolutionFactor = DeviceInfo.GetResolutionFactor() - 0.45f;
        mainCamera.fieldOfView = (resolutionFactor * 18.0f / 0.3f) + 42;
        skyCam.fieldOfView = (resolutionFactor * 18.0f / 0.3f) + 42;

        float xMultiplier = (resolutionFactor * 0.5f / 0.3f) + 0.5f;
        lD.xMultiplier.Override(xMultiplier);

        #endregion

        #region Reset Date and Time

        float aktHour = System.DateTime.Now.Hour;
        float aktMinute = System.DateTime.Now.Minute * 1.66f / 100;
        float currentTime = aktHour + aktMinute;
        skyTimeController.SetTimeline(currentTime);
        skyTimeController.SetLatitude(51);
        skyTimeController.SetUtc(1);
        skyTimeController.SetDate(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);

        cc.ChangeDateTextColor(1);
        cc.ChangeTimeTextColor(0);
        cc.SetCurrentDate();
        
        #endregion

        #region Reset Materials

        Color zeigerSeiteHighlightWeisColor = zeigerSeiteHighlightWeis.color;
        zeigerSeiteHighlightWeisColor.a = 0;
        zeigerSeiteHighlightWeis.color = zeigerSeiteHighlightWeisColor;

        Color importantStarsColor = importantStars.color;
        importantStarsColor.a = 0f;
        importantStars.color = importantStarsColor;
        importantStarsSchuettkantenStern.color = importantStarsColor;
        importantStarsPolar.color = importantStarsColor;

        Color monatsTageFrontColor = monatsTageFront.color;
        Color einstellringZahlenColor = einstellringZahlen.color;
        Color colorNG = nokturnalGold.color;
        Color nokturnalGoldZeigerColor = nokturnalGoldZeiger.color;
        Color colorSilber = nokturnalSilber.color;
        Color nokturnalFrontVerzierungColor = nokturnalFrontVerzierung.color;

        colorNG.a = 0;
        nokturnalGoldZeigerColor.a = 0;
        colorSilber.a = 0;
        nokturnalFrontVerzierungColor.a = 0;
        einstellringZahlenColor.a = 1;
        monatsTageFrontColor.a = 0;

        nokturnalGold.color = colorNG;
        nokturnalGoldZeiger.color = nokturnalGoldZeigerColor;
        nokturnalSilber.color = colorSilber;
        nokturnalFrontVerzierung.color = nokturnalFrontVerzierungColor;
        einstellringZahlen.color = einstellringZahlenColor;
        monatsTageFront.color = monatsTageFrontColor;

        Color frontZahlenFullColor = frontZahlenFull.color;
        frontZahlenFullColor.a = 0;
        frontZahlenFull.color = frontZahlenFullColor;

        #endregion

        #region Reset Nokturnal

        n02H.StopSnapZeiger();
        rOT.StopAllowRotationZeiger();
        nokturnalController.NokturnalShowFront();

        //Zeiger nach Animation (drehen mit schüttkantenstern zurücksetzen)
        nokturnalZeigerPivotChild.localEulerAngles = new Vector3(0, 0, 0);
        nokturnalZeigerPivotParent.localEulerAngles = new Vector3(90, 0, 0);
        nokturnal.transform.localScale = new Vector3(1, 1, 1);

        nokturnalCamera.SetActive(false);

        frontZahlenFullgo.transform.localPosition = new Vector3(0.04f, 0.05f, -0.02f);
        frontZahlenTrans.localPosition = new Vector3(0, 0.04f, 0f);

        cameraAnimator.enabled = true;
        cameraAnimator.Rebind();
        cameraAnimator.Update(0f);
        cameraAnimator.SetInteger("StateN01", 0);

        fillCircleCG.alpha = 0;
        fillCircleProcess.fillAmount = 0.05f;

        #endregion

        #region Reset Other

        arrowParent.SetActive(true);
        gridTransf.localScale = new Vector3(0.04374101f, 0.04374101f, 0.04374101f);

        #endregion

        #region Reset Starrysky

        StarrySkyAnimator.Rebind();
        TrailController tC = (TrailController) schuettKantenSternTracer.GetComponent(typeof(TrailController));
        tC.StopTracing();

        foreach (GameObject gO in traceParents) {
            gO.GetComponent<TrailRenderer>().emitting = false;
            TrailController tr = (TrailController) gO.GetComponent(typeof(TrailController));
            tr.StopTracing();
        }

        sPEH.ResetScript();
        rS.AllowCalculateRotationContinous();

        schuettkantenStern_1.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        schuettkantenStern_2.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        #endregion
    }

    public void StopFlashHighlightGrosserWagen() {
        Color importantStarsColor = importantStars.color;
        importantStarsColor.a = 1;
        importantStars.color = importantStarsColor;
        importantStarsSchuettkantenStern.color = importantStarsColor;

        flashHighlightGrosserWagen = false;
        highlightGrosserWagenOn = false;
        highlightGrosserWagenOff = false;
    }

    public void HighlightGrosserWagen(bool startDirectly) {
        if (startDirectly) {
            flashHighlightGrosserWagen = true;
            highlightGrosserWagenOn = true;
        } else {
            StartCoroutine(WaitForHighlightGrosserWagen());
        }
    }

    public void SetLastCheckPoint(int lCP) {
        lastcheckPoint = lCP;
    }

    private IEnumerator WaitForHighlightGrosserWagen() {
        yield return new WaitForSeconds(8f);
        flashHighlightGrosserWagen = true;
        highlightGrosserWagenOn = true;
    }

    private IEnumerator WaitBeforJump() {
        yield return new WaitForSeconds(1f);
        pC.JumpToNextStep();
    }

    private IEnumerator FinishCheckNokturnalTargetPosition() {
        fillCircleWhileCentered = false;

        OrbitalCameraController oCC = (OrbitalCameraController) mainCam.GetComponent(typeof(OrbitalCameraController));
        oCC.StopRotation();

        Color nokturnalGoldcolor = nokturnalGold.color;
        Color nokturnalGoldZeigerColor = nokturnalGoldZeiger.color;
        Color nokturnalSilbercolor = nokturnalSilber.color;
        Color frontMonatstageColor = frontMonatstage.color;
        nokturnalGoldcolor.a = 1;
        nokturnalGoldZeigerColor.a = 1;
        nokturnalSilbercolor.a = 1;
        frontMonatstageColor.a = 1;
        nokturnalGold.color = nokturnalGoldcolor;
        nokturnalGoldZeiger.color = nokturnalGoldZeigerColor;
        nokturnalSilber.color = nokturnalSilbercolor;
        frontMonatstage.color = frontMonatstageColor;

        fillCircleCG.alpha = 0;
        yield return new WaitForSeconds(0.1f);

        pC.JumpToNextStep();
    }
}