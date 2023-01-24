using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class N03Helper : MonoBehaviour {
    #region public Variables

    //Scripts
    public AnimationEventHandlerN03 aEHN03;
    public SkyTimeController skyTimeController;
    public SkyProfileEventHandler sPEH;
    public CheckPointController cPC;
    public ClockController cc;
    public ContentScrollSnapHorizontal cSSH;
    public NokturnalController nokturnalController;
    public ProcedureController pC;
    public SpielstandLoader ssL;
    public TimeInputController tIC;
    public Translator translator;

    //other Variables
    public Animator StarrySkyAnimator;

    public Camera skyCamera;
    public Camera mainCamera;

    public CanvasGroup uiLinesCG;

    public GameObject clockParentGO;
    public GameObject mainCam;
    public GameObject nokturnalCamera;
    public GameObject starrySky;
    public GameObject weekCalendar;

    public Image fillCircleImage;

    public Material importantStars;
    public Material importantStarsPolarstern;
    public Material importantStarsSchuettkantenStern;
    public Material materialPolarstern;
    public Material newLineCylinder;

    public RectTransform fillCircle;
    public RectTransform startPathRect;

    public SpriteRenderer datumsanzeige;

    public TextMeshPro text3dTage;
    public TextMeshProUGUI dateText;

    public Transform einstellRingPivot;
    public Transform fillCircleTransf;
    public Transform nokturnalTransf;
    public Transform polarStern;
    public Transform schuettkantenstern;
    public Transform starrySkyCompassPivot;
    public Transform zeigerPivot;
    public Transform zeigerPivotGameobject;

    public UILineRenderer startUiLineRenderer;

    #endregion

    #region private Variables

    private bool turnStarGlowOff = false;
    private bool turnGrosserWagenOn = false;
    private bool showDatumsanzeige = false;
    private bool turnUiRendererStartOff = false;
    private bool turnUiCanvasgRoupOff = false;
    private bool rotateEinstellring = false;
    private bool turn3DTextOn = false;
    private bool turn3DTextOff = false;

    private Color newLineCylinderColor;

    private float akttime = 0;

    private Vector3 initNokturnalPosition;

    #endregion

    void Start() {
        ProcedureController.changeEvent += DoActionWhileStepUpdate;
        StartCoroutine(InitNokturnalPos());
        newLineCylinderColor = newLineCylinder.color;
    }

    public void DoActionWhileStepUpdate(string stepId) {
        ClockController cc = (ClockController) clockParentGO.GetComponent(typeof(ClockController));

        switch (stepId) {
            #region Initialer Step/Checkpoint

            case "N03.00a":
            case "N03.00":
                ResetScript();
                newLineCylinderColor.a = 0;
                newLineCylinder.color = newLineCylinderColor;
                cc.SetTimeModeN03();
                turnStarGlowOff = true;
                try {
                    cSSH.ResetWeekCalenderForN03();
                }catch(NullReferenceException ex){}

                skyTimeController.SetTimeline(0);
                StartCoroutine(WaitForASecondBeforeStart());
                break;

            #endregion

            case "N03.02":
                turnGrosserWagenOn = true;
                StartCoroutine(InitCircleAndStartLine(0));
                break;

            #region Checkpoint

            case "N03.05C":
                ResetScript();
                skyTimeController.SetDate(System.DateTime.Now.Year, System.DateTime.Now.Month,
                    System.DateTime.Now.Day);
                cc.SetTimeModeN03();
                Color color = importantStars.color;
                color.a = 1f;
                importantStars.color = color;
                importantStarsSchuettkantenStern.color = color;
                importantStarsPolarstern.color = color;
                    cSSH.ResetWeekCalenderForN03();
                StartCoroutine(InitCircleAndStartLine(0));
                pC.JumpToNextStep();
                break;

            #endregion

            case "N03.05A":
                //Größe des Jahreskreises berechnen und setzen 
                CalculateSizeOfYearCircle();

                turn3DTextOff = false;
                //Linie zwischen Polarstern und Schüttkantenstern einblenden (Linie die sich am Himmel mit dreht. Ist ein Zylinder im 3D-Raum)
                newLineCylinderColor.a = 1;
                newLineCylinder.color = newLineCylinderColor;

                //Himmel für einen Tag rotieren
                StarrySkyAnimator.enabled = true;
                StarrySkyAnimator.Rebind();
                StarrySkyAnimator.SetInteger("StateN03",1);
                StarrySkyAnimator.Play("RotateStarrySkyForOneDay",0,0);
                break;

            case "N03.12":
                //Checkpoint erreicht
                cPC.SetCheckPointReached("N03", 02);
                break;
            case "N03.13":
                //Himmel für 7 Tage rotieren
                StarrySkyAnimator.SetInteger("StateN03",2);
                StarrySkyAnimator.Play("RotateStarrySkyFor7Days",0,0);
                break;

            #region Checkpoint

            case "N03.15C":
                ResetScript();
                CalculateSizeOfYearCircle();
                cc.SetTimeModeN03();
                
                //Highlight wichtige Sterne
                Color color2 = importantStars.color;
                color2.a = 1f;
                importantStars.color = color2;
                importantStarsSchuettkantenStern.color = color2;
                importantStarsPolarstern.color = color2;

                StartCoroutine(InitCircleAndStartLine(7.0f));
                turn3DTextOff = true;
                weekCalendar.SetActive(false);
                break;

            #endregion

            case "N03.15":
                StarrySkyAnimator.Rebind();
                DateTime startDate = DateTime.Now;
                DateTime plus7days = startDate.AddDays(7);
                skyTimeController.SetDate(plus7days.Year, plus7days.Month, plus7days.Day);
                startUiLineRenderer.enabled = true;

                //3D Text ausblenden und Scroll-Kalender oben einblenden
                turn3DTextOff = true;
                tIC.ShowInputDateWeekPanelN03();
                
                cSSH.UpdateWeekCalenderForN03();
                break;

            case "N03.20":
                cPC.SetCheckPointColors("N03", 3);
                cPC.SetCheckPointReached("N03", 03);
                break;

            case "N03.21":
                //Scroll-Kalender oben ausblenden und UI-Linien sowie Jahreskreis ausblenden
                tIC.HideScrollPanel();
                turnUiCanvasgRoupOff = true;
                
                //Nokturnal einblenden und Einstellring rotieren
                if (einstellRingPivot.localEulerAngles.y > 45 && einstellRingPivot.localEulerAngles.y < 105) {
                    zeigerPivot.localEulerAngles = new Vector3(90, 280, 0);
                } else {
                    zeigerPivot.localEulerAngles = new Vector3(90, 0, 0);
                }
                nokturnalCamera.SetActive(true);
                nokturnalCamera.transform.GetChild(0).transform.gameObject.SetActive(true);
                nokturnalTransf.transform.localScale = new Vector3(0.9691705f, 0.9691705f, 0.9691705f);
                nokturnalTransf.transform.localPosition = new Vector3(0, 0.81f, 1.639942f);
                pC.JumpToNextStep();
                break;

            case "N03.24":
                rotateEinstellring = true;
                ssL.FinishedPathPoint("N03");
                showDatumsanzeige = true;
                turnStarGlowOff = true;

                pC.JumpToNextStep();
                break;
        }
    }

    private void Update() {
        #region graphical actions

        if (turnStarGlowOff) {
            Color color = importantStars.color;
            if (color.a > 0) {
                color.a -= 0.05f;
                importantStars.color = color;
                importantStarsSchuettkantenStern.color = color;
                importantStarsPolarstern.color = color;
                materialPolarstern.color = color;
            } else {
                turnStarGlowOff = false;
            }
        }

        if (turnGrosserWagenOn) {
            Color color = importantStars.color;
            if (color.a < 1) {
                color.a += 0.05f;
                importantStars.color = color;
                importantStarsSchuettkantenStern.color = color;
                importantStarsPolarstern.color = color;
            } else {
                pC.JumpToNextStep();
                turnGrosserWagenOn = false;
            }
        }

        if (showDatumsanzeige) {
            Color color = datumsanzeige.color;
            if (color.a < 1) {
                color.a += 0.1f;
                datumsanzeige.color = color;
            } else {
                color.a = 1f;
                datumsanzeige.color = color;
                showDatumsanzeige = false;
            }
        }

        if (turnUiRendererStartOff) {
            Color color = startUiLineRenderer.color;
            if (color.a > 0) {
                color.a -= 0.1f;
                startUiLineRenderer.color = color;
            } else {
                color.a = 0f;
                startUiLineRenderer.color = color;
                turnUiRendererStartOff = false;
            }
        }

        if (turnUiCanvasgRoupOff) {
            if (uiLinesCG.alpha > 0) {
                uiLinesCG.alpha -= 0.1f;
            } else {
                uiLinesCG.alpha = 0;
                turnUiCanvasgRoupOff = false;
            }
        }

        if (turn3DTextOn) {
            Color text3dTageColor = text3dTage.color;
            if (text3dTageColor.a < 1) {
                text3dTageColor.a += 0.075f;
                text3dTage.color = text3dTageColor;
            } else {
                text3dTageColor.a = 1;
                text3dTage.color = text3dTageColor;
                turn3DTextOn = false;
            }
        }

        if (turn3DTextOff) {
            Color newLineCylinderColor = newLineCylinder.color;
            Color text3dTageColor = text3dTage.color;
            if (text3dTageColor.a > 0) {
                newLineCylinderColor.a -= 0.1f;
                newLineCylinder.color = newLineCylinderColor;
                text3dTageColor.a -= 0.1f;
                text3dTage.color = text3dTageColor;
            } else {
                newLineCylinderColor.a = 0;
                newLineCylinder.color = newLineCylinderColor;
                text3dTageColor.a = 0;
                text3dTage.color = text3dTageColor;
                turn3DTextOn = false;
            }
        }

        if (rotateEinstellring) {
            einstellRingPivot.localEulerAngles += new Vector3(0, 0, 0.1f);
        }

        #endregion
    }


    private void ResetScript() {
        #region Reset private Variables

        turn3DTextOn = false;
        rotateEinstellring = false;
        sPEH.SetAllowUpdateLightTrue();
        turnStarGlowOff = false;
        turnGrosserWagenOn = false;
        showDatumsanzeige = false;
        turnUiRendererStartOff = false;
        turnUiCanvasgRoupOff = false;
        akttime = 0;

        #endregion
        
        #region Reset Camera
        float latitude = skyTimeController.GetLatitude();
        latitude = -1.0f * latitude;
        float temp = latitude;
        float screenFactor = DeviceInfo.GetScreenfactor();
        float newZPos = (2.22f - screenFactor) * 469.337f;
        starrySkyCompassPivot.transform.localPosition = new Vector3(0, 0, newZPos);
        float newXRot = temp + ((2.22f - screenFactor) * 18.7536f);
        mainCam.transform.localEulerAngles = new Vector3(newXRot, 0, 0);

        float newFOV = ((2.22f - screenFactor) * 16.0f) / 0.89f + 42.0f;
        mainCamera.fieldOfView = newFOV;
        skyCamera.fieldOfView = newFOV;

        #endregion

        #region Reset Date and Time

        skyTimeController.SetUtc(1);
        skyTimeController.SetTimeline(0);
        skyTimeController.SetDate(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);

        cc.ChangeDateTextColor(1);
        cc.ChangeTimeTextColor(1);
        cc.SetCurrentDate();

        Color datumsanzeigeColor = datumsanzeige.color;
        datumsanzeigeColor.a = 0;
        datumsanzeige.color = datumsanzeigeColor;
        tIC.HideScrollPanel();
        weekCalendar.SetActive(true);

        #endregion

        #region Reset Nokturnal

        nokturnalTransf.transform.position = initNokturnalPosition;
        nokturnalController.InitEinstellring();
        zeigerPivotGameobject.localEulerAngles = new Vector3(0, 0, 25);

        nokturnalController.NokturnalShowFront();
        nokturnalCamera.SetActive(false);

        #endregion

        #region Reset Other
        StarrySkyAnimator.Rebind();
        aEHN03.StopSetTextColorWithAnim();

        if (pC.GetLanguage() == 0) {
            text3dTage.text = "+1 TAG";
        } else if (pC.GetLanguage() == 1) {
            text3dTage.text = "+1 DAY";
        }

        #endregion

        #region Reset Starry Sky

        startUiLineRenderer.enabled = false;
        uiLinesCG.alpha = 1;

        Color color = startUiLineRenderer.color;
        color.a = 0.4f;
        startUiLineRenderer.color = color;
        fillCircleImage.fillAmount = 0;
        Color text3dTageColor = text3dTage.color;
        text3dTageColor.a = 0;
        text3dTage.color = text3dTageColor;

        RotateStarrSky rSS = (RotateStarrSky) starrySky.GetComponent(typeof(RotateStarrSky));
        rSS.AllowCalculateRotationContinous();
        #endregion
    }

    //Drehung für ein Tag komplett, Kreis um 1 Tag(ca. 1 Grad) füllen und Text einblenden
    public void AnimationFinishedRotationOneDayN03() {
        ClockController cc = (ClockController) clockParentGO.GetComponent(typeof(ClockController));
        cc.SetTimeModeN03();
        cc.SetAnimatedDays(1);

        fillCircleImage.fillAmount += 1.0f / 365.0f;
        turn3DTextOn = true;

        Vector3 aktDate = skyTimeController.GetDate();
        int temp = (int) aktDate.z + 1;
        dateText.text = temp.ToString() + "." + aktDate.y + "." + aktDate.x;

        if (pC.GetLanguage() == 1) {
            string month = translator.GetMonthName((int) aktDate.y);
            dateText.text = month + " " + temp.ToString() + ", " + aktDate.x;
        }

        pC.JumpToNextStep();
    }

    //Winkel zwischen Polarstern und Schüttkantenstern berechnen, Winkel wird zur Initialisierung des JahresKreis und der Tageslinie benötigt
    private float GetAngle() {
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

    private IEnumerator WaitForASecondBeforeStart() {
        yield return new WaitForSeconds(1f);
        pC.JumpToNextStep();
    }

    //Position und Drehung des Jahreskreis und der Startlinie zwischen Polarstern und Schüttkantenstern initialisieren
    //kann bspw. auch mit 7 Tage initialisiert werden, dann ist Kreis schon für eine Woche gefüllt (nötig z.B. bei Checkpoint sprung)
    private IEnumerator InitCircleAndStartLine(float days) {
        yield return new WaitForSeconds(0.4f);
        Vector2 polarScreenPos;
        Vector2 schuettkantensternScreenPos;
        polarScreenPos = Camera.main.WorldToScreenPoint(polarStern.transform.position);
        schuettkantensternScreenPos = Camera.main.WorldToScreenPoint(schuettkantenstern.transform.position);

        startUiLineRenderer.enabled = true;
        startUiLineRenderer.Points[0] = new Vector2(polarScreenPos.x, polarScreenPos.y);
        startUiLineRenderer.Points[1] = new Vector2(schuettkantensternScreenPos.x, schuettkantensternScreenPos.y);

        startPathRect.sizeDelta = new Vector2(150, 150);
        fillCircle.anchoredPosition = new Vector2(polarScreenPos.x, polarScreenPos.y);
        float angle = GetAngle();
        fillCircleTransf.localEulerAngles = new Vector3(0, 0, angle);
        fillCircleImage.fillAmount = days / 365.0f;

        yield return new WaitForSeconds(0.4f);

        if (days > 0) {
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime newDate = startDate.AddDays(7);
            skyTimeController.SetDate(newDate.Year, newDate.Month, newDate.Day);
            tIC.ShowInputDateWeekPanelN03();
            cSSH.UpdateWeekCalenderForN03();

            float dist = Vector2.Distance(schuettkantensternScreenPos, polarScreenPos) * 2;
            fillCircle.sizeDelta = new Vector2(dist, dist);
        }
    }

    //Berechne die Größe des Jahrescircle, notwendig wegen verschiedener Auflösungen
    private void CalculateSizeOfYearCircle() {
        Vector2 polarScreenPos;
        Vector2 schuettkantensternScreenPos;
        polarScreenPos = Camera.main.WorldToScreenPoint(polarStern.transform.position);
        schuettkantensternScreenPos = Camera.main.WorldToScreenPoint(schuettkantenstern.transform.position);

        float dist = Vector2.Distance(schuettkantensternScreenPos, polarScreenPos) * 2;

        fillCircle.sizeDelta = new Vector2(dist, dist);
        startPathRect.sizeDelta = new Vector2(150, 150);
    }
    
    private IEnumerator InitNokturnalPos() {
        yield return new WaitForSeconds(5f);
        initNokturnalPosition = nokturnalTransf.transform.localPosition;
    }
}