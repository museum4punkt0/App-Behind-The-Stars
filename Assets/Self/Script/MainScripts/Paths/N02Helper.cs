using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class N02Helper : MonoBehaviour {
    #region public Variables
    //Scripts
    public AnimationEventHandlerCameraN01 aEHN01;
    public OrbitalCameraController oCC;
    public SkyProfileEventHandler sPEH;
    public CheckPointController cPC;
    public MainMenuController mmc;
    public N01Helper n01H;
    public NokturnalController nokturnalController;
    public SpielstandLoader ssL;

    //Other Objects
    public Animator StarrySkyAnimator;

    public GameObject clockParentGO;
    public GameObject frontZahlenFull;
    public GameObject gameController;
    public GameObject mainCam;
    public GameObject nokturnalCamera;
    public GameObject schuettKantenSternTracer;
    public GameObject sonnenuhrButtonImage;
    public GameObject sonnenuhrSchloss;
    public GameObject sonnenuhr3dMainMenuSchloss;
    public List<GameObject> traceParents = new List<GameObject>();

    public Material nokturnalGold;
    public Material nokturnalGoldZeiger;
    public Material nokturnalSilber;
    public Material importantStars;
    public Material importantStarsSchuettkantenStern;
    public Material materialPolarstern;
    public Material traceMaterial;

    public SpriteRenderer einstellringZahlen;
    public SpriteRenderer monatsTageFront;
    public SpriteRenderer zeigerSeiteHighlightWeis;
    public SpriteRenderer nokturnalFrontVerzierung;

    public Transform cameraContainer;
    public Transform frontZahlenTrans;
    public Transform nokturnalParent;
    public Transform polarStern;
    public Transform schuettkantenstern;
    public Transform starrySkyCompassPivot;
    public Transform skyCompassPivot;
    public Transform terrainCompassPivot;
    public Transform zeigerPivot;
    public Transform zeigerRotateObject;

    public TrailRenderer schuettkantensternTR;

    public Volume volume;

    #endregion

    #region private Variables

    private bool turnNokturnalOff = false;
    private bool turnGrosserWagenOff = false;
    private bool turnTraceOff = false;
    private bool turnStarGlowOff = false;
    private bool showNokturnal = false;
    private bool snapZeiger = false;
    private bool firstBeta = true;

    private float correctBeta = 0.0f;
    private float startAngle = 0.0f;

    private LensDistortion lD;

    #endregion

    private void Start() {
        LensDistortion tmp;

        if (volume.profile.TryGet(out tmp)) {
            lD = tmp;
        }

        ProcedureController.changeEvent += DoActionWhileStepUpdate;
    }

    public void DoActionWhileStepUpdate(string stepId) {
        ClockController cc = (ClockController) clockParentGO.GetComponent(typeof(ClockController));

        switch (stepId) {
            #region Checkpoint
            case "N02.01C":
            case "N02.01":
                cPC.SetCheckPointReached("N01", 04);
                n01H.SetLastCheckPoint(4);
                n01H.StopFlashHighlightGrosserWagen();
                oCC.StopRotation();
                ResetScript();
                
                turnNokturnalOff = true;
                turnGrosserWagenOff = true;
                Color zeigerSeiteHighlightWeisColor = zeigerSeiteHighlightWeis.color;
                zeigerSeiteHighlightWeisColor.a = 0;
                zeigerSeiteHighlightWeis.color = zeigerSeiteHighlightWeisColor;

                nokturnalParent.transform.localScale = new Vector3(0, 0, 0);
                schuettkantensternTR.enabled = true;
                schuettkantensternTR.time = 0.0f;

                foreach (GameObject gO in traceParents) {
                    gO.GetComponent<TrailRenderer>().enabled = true;
                    gO.GetComponent<TrailRenderer>().time = 0.0f;
                }

                cameraContainer.transform.localEulerAngles = new Vector3(0, 0, 0);
                float screenfactor = DeviceInfo.GetResolutionFactor() - 0.45f;
                float tempX = -34f - (screenfactor * 3 / 0.3f);

                mainCam.transform.localEulerAngles = new Vector3(tempX, 0, 0);
                zeigerPivot.transform.localEulerAngles = new Vector3(90, 0, 0);
                StartCoroutine(WaitAMomentAndInitNokturnalZeigerPosition());

                break;
#endregion
            
            case "N02.03":
                //Himmel drehen und Schüttkantenstern tracen
                StarrySkyAnimator.SetInteger("StateN01", 1);
                StarrySkyAnimator.Play("RotateStarrySkyInLoop",0,0);
                snapZeiger = true;
                cc.SetTimeModeN02();

                TrailController tC = (TrailController) schuettKantenSternTracer.GetComponent(typeof(TrailController));
                tC.StartTracing();
                break;

            case "N02.20":
                //Nokturnal einblenden
                aEHN01.StopMoveWithAnimAEHN01();
                nokturnalCamera.SetActive(true);
                showNokturnal = true;
                nokturnalParent.transform.localScale = new Vector3(0.494171f, 0.494171f, 0.494171f);
                break;

            case "N02.21":
                mmc.SetN01Done();
                ssL.FinishedPathPoint("N01");

                //Sonnenuhr freischalten falls noch nicht geschehen
                sonnenuhrButtonImage.SetActive(true);
                sonnenuhrSchloss.SetActive(false);
                if (sonnenuhr3dMainMenuSchloss.activeSelf) {
                    mmc.UnlockSonnenuhrInNextStep();
                }

                //Neuen Spielstand schreiben                
                string loadData = "";
                var filePath = Path.Combine(Application.persistentDataPath, "spielstand.txt");
                try {
                    loadData = File.ReadAllText(filePath);
                } catch (FileNotFoundException ex) {
                }

                if (loadData != "2") {
                    string jsonData = "1";
                    File.WriteAllText(filePath, jsonData);
                }

                ProcedureController pC = (ProcedureController) gameController.GetComponent(typeof(ProcedureController));
                pC.JumpToNextStep();
                break;

            case "N02.22A":
                StartCoroutine(WaitandFinishPath());
                break;
        }
    }

    void Update() {
        //Winkel zwischen Polarstern und Schüttkantenstern berechnen und Nokturnaleiger auf Schüttkantenstern snappen
        if (snapZeiger) {
            float beta = CalculateBetaAngleOfNokturnalZeiger();

            if (firstBeta) {
                float temp = 0.0f;
                correctBeta = beta - temp;
                firstBeta = false;
            }

            float angle = Mathf.LerpAngle(correctBeta, beta - correctBeta, Time.time);
            zeigerRotateObject.localEulerAngles = new Vector3(0, 0, angle);
        }

        #region graphical actions

        if (turnNokturnalOff) {
            Color monatsTageFrontColor = monatsTageFront.color;
            Color einstellringZahlenColor = einstellringZahlen.color;
            Color nokturnalFrontVerzierungColor = nokturnalFrontVerzierung.color;
            Color color = nokturnalGold.color;
            Color nokturnalGoldZeigerColor = nokturnalGoldZeiger.color;
            Color colorSilber = nokturnalSilber.color;
            if (color.a > 0) {
                colorSilber.a -= 0.1f;
                color.a -= 0.1f;
                nokturnalGoldZeigerColor.a -= 0.1f;
                nokturnalFrontVerzierungColor.a -= 0.05f;
                einstellringZahlenColor.a -= 0.01f;
                monatsTageFrontColor.a -= 0.1f;
                nokturnalGold.color = color;
                nokturnalSilber.color = colorSilber;
                nokturnalGoldZeiger.color = nokturnalGoldZeigerColor;
                nokturnalFrontVerzierung.color = nokturnalFrontVerzierungColor;
                einstellringZahlen.color = einstellringZahlenColor;
                monatsTageFront.color = monatsTageFrontColor;
            } else {
                nokturnalGoldZeigerColor.a = 0.0f;
                nokturnalGoldZeiger.color = nokturnalGoldZeigerColor;
                nokturnalParent.transform.localScale = new Vector3(0, 0, 0);
                nokturnalCamera.SetActive(false);
                turnNokturnalOff = false;
            }
        }

        if (turnGrosserWagenOff) {
            Color color = importantStars.color;
            if (color.a > 0) {
                color.a -= 0.05f;
                importantStars.color = color;
                materialPolarstern.color = color;
            } else {
                turnGrosserWagenOff = false;
            }
        }

        if (turnTraceOff) {
            Color color = traceMaterial.color;
            if (color.a > 0) {
                color.a -= 0.05f;
                traceMaterial.color = color;
            } else {
                turnTraceOff = false;
            }
        }

        if (turnStarGlowOff) {
            Color color = importantStars.color;
            if (color.a > 0) {
                color.a -= 0.05f;
                importantStars.color = color;
                importantStarsSchuettkantenStern.color = color;
                materialPolarstern.color = color;
            } else {
                turnStarGlowOff = false;
            }
        }

        if (showNokturnal) {
            Color monatsTageFrontColor = monatsTageFront.color;
            Color einstellringZahlenColor = einstellringZahlen.color;
            Color colorNG = nokturnalGold.color;
            Color nokturnalGoldZeigerColor = nokturnalGoldZeiger.color;
            Color colorSilber = nokturnalSilber.color;
            Color nokturnalFrontVerzierungColor = nokturnalFrontVerzierung.color;

            if (colorNG.a < 0.75f) {
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
                colorNG.a = 0.75f;
                nokturnalGoldZeigerColor.a = 1f;
                colorSilber.a = 1f;
                nokturnalFrontVerzierungColor.a = 0.4f;
                einstellringZahlenColor.a = 1f;
                monatsTageFrontColor.a = 1f;
                nokturnalGold.color = colorNG;
                nokturnalGoldZeiger.color = nokturnalGoldZeigerColor;
                nokturnalSilber.color = colorSilber;
                nokturnalFrontVerzierung.color = nokturnalFrontVerzierungColor;
                einstellringZahlen.color = einstellringZahlenColor;
                monatsTageFront.color = monatsTageFrontColor;
                showNokturnal = false;
            }
        }

        #endregion
    }

    public void ResetScript() {
        #region Reset private Variables

        frontZahlenFull.SetActive(false);
        sPEH.StopUpdatingPropertiesAndTurnSunOff();
        turnNokturnalOff = false;
        turnGrosserWagenOff = false;
        turnTraceOff = false;
        turnStarGlowOff = false;
        showNokturnal = false;
        snapZeiger = false;
        firstBeta = true;
        correctBeta = 0.0f;

        #endregion

        #region Reset Starrysky

        starrySkyCompassPivot.localEulerAngles = new Vector3(0.0f, 0, 0.0f);
        skyCompassPivot.localEulerAngles = new Vector3(0.0f, 0, 0.0f);
        terrainCompassPivot.localEulerAngles = new Vector3(0.0f, 0, 0.0f);

        TrailController tC = (TrailController) schuettKantenSternTracer.GetComponent(typeof(TrailController));
        tC.StopTracing();
        foreach (GameObject gO in traceParents) {
            TrailController tr = (TrailController) gO.GetComponent(typeof(TrailController));
            tr.StopTracing();
        }

        #endregion

        #region Reset Camera

        float resolutionfactor = DeviceInfo.GetResolutionFactor() - 0.45f;
        float xMultiplier = (resolutionfactor * 0.5f / 0.3f) + 0.5f;
        lD.xMultiplier.Override(xMultiplier);

        #endregion

        #region Reset Other

        nokturnalController.InitEinstellring();
        frontZahlenFull.transform.localPosition = new Vector3(0.04f, 0.05f, -0.02f);
        frontZahlenTrans.localPosition = new Vector3(0, 0.04f, 0f);
        StarrySkyAnimator.Rebind();

        #endregion
    }

    private void InitZeigerPos() {
        float beta = CalculateBetaAngleOfNokturnalZeiger();
        zeigerPivot.transform.localEulerAngles = new Vector3(90, 0, beta + 102);
    }

    //Berechnung des Winkels zwischen Polarstern und Schüttkantenstern
    private float CalculateBetaAngleOfNokturnalZeiger() {
        Vector2 polarScreenPos = Camera.main.WorldToScreenPoint(polarStern.transform.position);
        Vector2 schuettkantensternScreenPos = Camera.main.WorldToScreenPoint(schuettkantenstern.transform.position);
        Vector2 helperpoint = new Vector2(schuettkantensternScreenPos.x, polarScreenPos.y);

        float a = polarScreenPos.x - helperpoint.x;
        float b = schuettkantensternScreenPos.y - helperpoint.y;
        float c = Mathf.Sqrt((a * a) + (b * b));
        float q = (a * a) / c;
        float p = c - q;
        float h = Mathf.Sqrt(p * q);
        float alpha = Mathf.Atan2(h, p) * Mathf.Rad2Deg;
        float beta = 90 - alpha;

        if (schuettkantensternScreenPos.x > polarScreenPos.x && schuettkantensternScreenPos.y > helperpoint.y) {
            beta += 0;
        }

        if (schuettkantensternScreenPos.x < polarScreenPos.x && schuettkantensternScreenPos.y > helperpoint.y) {
            beta = 180 - beta;
        }

        if (schuettkantensternScreenPos.x < polarScreenPos.x && schuettkantensternScreenPos.y < helperpoint.y) {
            beta = 180 + beta;
        }

        if (schuettkantensternScreenPos.x > polarScreenPos.x && schuettkantensternScreenPos.y < helperpoint.y) {
            beta = 360 - beta;
        }

        return beta;
    }

    public void StartTracingAllStars() {
        StartCoroutine(N0203HelperCoroutine());
    }

    public void StopSnapZeiger() {
        snapZeiger = false;
    }

    //Halbe Sekunde warten bis das Abschluss-Fenster eingefahren ist und dann Trace und Rotatiom stoppen
    private IEnumerator WaitandFinishPath() {
        yield return new WaitForSeconds(0.5f);
        turnTraceOff = true;
        turnStarGlowOff = true;
        TrailController tC2 = (TrailController) schuettKantenSternTracer.GetComponent(typeof(TrailController));
        tC2.StopTracing();
        StarrySkyAnimator.Rebind();
        snapZeiger = false;
        foreach (GameObject gO in traceParents) {
            TrailController tr = (TrailController) gO.GetComponent(typeof(TrailController));
            tr.StopTracing();
        }
    }

    private IEnumerator N0203HelperCoroutine() {
        ProcedureController pC = (ProcedureController) gameController.GetComponent(typeof(ProcedureController));
        pC.JumpToNextStep();

        yield return new WaitForSeconds(3f);

        Color color = importantStars.color;
        do {
            color.a += 0.01f;
            importantStars.color = color;
        } while (color.a < 1.0f);

        foreach (GameObject gO in traceParents) {
            TrailController tr = (TrailController) gO.GetComponent(typeof(TrailController));
            tr.StartTracing();
        }
    }

    private IEnumerator WaitAMomentAndInitNokturnalZeigerPosition() {
        yield return new WaitForSeconds(0.5f);
        InitZeigerPos();
    }
}