using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine.Rendering.Universal;

public class N08Helper : MonoBehaviour {
#region public Variables

    public SkyTimeController skyTimeController;
    public CheckPointController cPC;
    public ClockController cc;
    public ContentScrollSnapHorizontal cSSH;
    public NokturnalController nokturnalController;
    public ProcedureController pC;
    public RotateSchablone rS;
    public SpielstandLoader ssL;
    public TimeInputController tIC;
    public Translator translator;

    public Button okButton;

    public Camera skyCam;

    public CanvasGroup moonSymbol;
    public CanvasGroup overview;
    public CanvasGroup sunSymbol;

    public GameObject contentScrollClockObject;
    public GameObject gameController;
    public GameObject mainCam;
    public GameObject nokturnalCamera;

    public SpriteRenderer aussenringBlackBackground;
    public SpriteRenderer aeusersterRing;
    public SpriteRenderer ausenringHighlightsBlackBackground;
    public SpriteRenderer ausenringHighlightSonne;
    public SpriteRenderer ausenringHighlightSonneRand;
    public SpriteRenderer ausenringHighlightMond;
    public SpriteRenderer ausenringHighlightMondRand;
    public SpriteRenderer ausenringHighlightMars;
    public SpriteRenderer ausenringHighlightVenus;
    public SpriteRenderer datumsRing;
    public SpriteRenderer datumsRingauswahl;
    public SpriteRenderer griffBlackBackground;
    public SpriteRenderer innersterRing;
    public SpriteRenderer zweitInnersterRing;

    public TextMeshProUGUI textContent;

    public Transform backRingPivot;
    public Transform einstellRingPivot;
    public Transform moonSymbulTransf;
    public Transform nokturnalParent;
    public Transform overViewtransf;
    public Transform sunSymbulTransf;

#endregion

#region private Variables

    private bool turnInnersterRingOn = false;
    private bool turnInnersterRingOff = false;
    private bool turnZweitInnersterRingOn = false;
    private bool turnZweitInnersterRingOff = false;
    private bool turnDatumsRingOn = false;
    private bool turnDatumsRingOff = false;
    private bool changeDatumsRing = false;
    private bool turnaeusersterRingOn = false;
    private bool turnaeusersterRingOff = false;
    private bool highlightAussenring = false;
    private bool showSunSymbol = false;
    private bool turnSunSymbolOff = false;
    private bool showMoonSymbol = false;
    private bool turnMoonSymbolOff = false;
    private bool turnHighlightAussenringOff = false;
    private bool showAusenringSunSymbol = false;
    private bool turnAusenringSunSymbolOff = false;
    private bool showAusenringMoonSymbol = false;
    private bool turnAusenringMoonSymbolOff = false;
    private bool marsFound = false;
    private bool venusFound = false;
    private bool rotateBackRing = false;
    private bool showOverview = false;
    private bool turnOverviewOff = false;

    private int month = 0;
    private int dayNameIndex = 0;
    private int wrongAnswerCount = 0;

    private Quaternion camTargetRot;

    private List<string[]> monthList = new List<string[]>();

    //Monat, Monat-EN, HimmlischesZeichen, HimmlischesZeichen-EN, Beginn Tag Sternzeichen, Beginn Tag Sternzeichen-EN,
    //SonnenAufgan, Sonnen-Untergang, Nachtlänge, Taglänge, Monatslänge
    string[] month0 = {"Januar", "January", "Wassermann", "Aquarius", "20", "20th", "8", "16", "4", "16", "8", "31"};
    string[] month1 = {"Februar", "February", "Fische", "Pisces", "19", "19th", "7", "17", "5", "14", "10", "28"};
    string[] month2 = {"März", "March", "Widder", "Aries", "21", "21st", "6", "18", "6", "12", "12", "31"};
    string[] month3 = {"April", "April", "Stier", "Taurus", "20", "20th", "5", "19", "7", "10", "14", "30"};
    string[] month4 = {"Mai", "May", "Zwillinge", "Gemini", "21", "21st", "4", "20", "8", "8", "16", "31"};
    string[] month5 = {"Juni", "June", "Krebs", "Cancer", "21", "21st", "3:30", "20:30", "8:30", "7", "17", "30"};
    string[] month6 = {"Juli", "July", "Löwe", "Leo", "23", "23rd", "4", "20", "8", "8", "16", "31"};
    string[] month7 = {"August", "August", "Jungfrau", "Virgo", "23", "23rd", "5", "19", "7", "10", "14", "31"};
    string[] month8 = {"September", "September", "Waage", "Libra", "23", "23rd", "6", "18", "6", "12", "12", "30"};
    string[] month9 = {"Oktober", "October", "Skorpion", "Scorpio", "23", "23rd", "7", "17", "5", "14", "10", "31"};

    string[] month10 =
        {"November", "November", "Schütze", "Sagittarius", "22", "22nd", "8", "16", "4", "16", "8", "30"};

    string[] month11 =
        {"Dezember", "December", "Steinbock", "Capricorn", "22", "22nd", "8:30", "15:30", "3:30", "17", "7", "31"};

#endregion

    private void Start() {
        ProcedureController.changeEvent += DoActionWhileStepUpdate;

        //die Daten der String-Arrays in den privaten Variablen in einer Liste abspeichern

    #region Init monthList

        monthList.Add(month0);
        monthList.Add(month1);
        monthList.Add(month2);
        monthList.Add(month3);
        monthList.Add(month4);
        monthList.Add(month5);
        monthList.Add(month6);
        monthList.Add(month7);
        monthList.Add(month8);
        monthList.Add(month9);
        monthList.Add(month10);
        monthList.Add(month11);

    #endregion
    }

    public void DoActionWhileStepUpdate(string stepId) {
        switch (stepId) {
        #region Initial Step/Checkpoint

            case "N08.00":
            case "N08.00a":
                ResetScript();
                rS.DisableRotation();
                rS.AllowAutomaticRotation();
                pC.JumpToNextStep();
                contentScrollClockObject.SetActive(true);
                break;

        #endregion

            case "N08.04":
                turnDatumsRingOn = true;

                pC.JumpToNextStep();
                break;

            case "N08.07":
                //Drehung der Schablone mit Touch auf der Rückseite des Nokturnals erlauben
                rS.AllowRotationVariableWrapper();
                okButton.interactable = false;
                break;

            case "N08.08":
                MonthChoosed();
                break;

            case "N08.09":
                int language = pC.GetLanguage();
                string content = pC.myBubbleList.bubbles[pC.indexOfAktStep].bubbleContent;
                if (language == 1) {
                    content = pC.myBubbleList.bubbles[pC.indexOfAktStep].bubbleContent_EN;
                }

                //In den Texten der Json-Datei können solche Tags stehen, String bis dahin abschneiden und Daten des gewählten Monats hier einfügen
                string[] arr = content.Split(new string[] {"<Monat>"}, StringSplitOptions.None);
                string[] arr2 = arr[1].Split(new string[] {"<X>"}, StringSplitOptions.None);

                textContent.text = arr[0] + monthList[month][0 + language] + arr2[0] + monthList[month][11] + arr2[1];
                break;

            case "N08.10":
                turnDatumsRingOff = true;
                turnInnersterRingOn = true;
                pC.JumpToNextStep();
                break;

            case "N08.11":
                language = pC.GetLanguage();
                string content10 = pC.myBubbleList.bubbles[pC.indexOfAktStep].bubbleContent;
                if (pC.GetLanguage() == 1) {
                    content10 = pC.myBubbleList.bubbles[pC.indexOfAktStep].bubbleContent_EN;
                }

                string[] arr10 = content10.Split(new string[] {"<Monat>"}, StringSplitOptions.None);
                string[] arr10_2 = arr10[1].Split(new string[] {"<Sonnenaufgangszeit>"}, StringSplitOptions.None);
                string[] arr10_3 = arr10_2[1].Split(new string[] {"<Sonnenuntergangszeit>"}, StringSplitOptions.None);
                textContent.text = arr10[0] + monthList[month][0 + language] + arr10_2[0] + monthList[month][6] +
                                   arr10_3[0] + monthList[month][7 + language] + arr10_3[1];
                break;

            case "N08.13":
                turnInnersterRingOff = true;
                turnZweitInnersterRingOn = true;
                pC.JumpToNextStep();
                break;

            case "N08.14":
                string content14 = pC.myBubbleList.bubbles[pC.indexOfAktStep].bubbleContent;
                if (pC.GetLanguage() == 1) {
                    content14 = pC.myBubbleList.bubbles[pC.indexOfAktStep].bubbleContent_EN;
                }

                string[] arr14 = content14.Split(new string[] {"<Sonnenstunden>"}, StringSplitOptions.None);
                string[] arr14_2 = arr14[1].Split(new string[] {"<Nachtstunden>"}, StringSplitOptions.None);

                textContent.text = arr14[0] + monthList[month][10] + arr14_2[0] + monthList[month][9] + arr14_2[1];
                break;

            case "N08.15":
                turnZweitInnersterRingOff = true;
                turnaeusersterRingOn = true;
                pC.JumpToNextStep();
                break;


            case "N08.16":
                language = pC.GetLanguage();
                string content16 = pC.myBubbleList.bubbles[pC.indexOfAktStep].bubbleContent;
                if (pC.GetLanguage() == 1) {
                    content16 = pC.myBubbleList.bubbles[pC.indexOfAktStep].bubbleContent_EN;
                }

                string[] arr16 = content16.Split(new string[] {"<Sternzeichen>"}, StringSplitOptions.None);
                string[] arr16_2 = arr16[1].Split(new string[] {"<Tag>"}, StringSplitOptions.None);

                textContent.text = arr16[0] + monthList[month][2 + language] + arr16_2[0] + monthList[month][4] + ". " +
                                   monthList[month][0 + language] + ".";
                if (language == 1) {
                    textContent.text = arr16[0] + monthList[month][2 + language] + arr16_2[0] +
                                       monthList[month][4 + language] + " of " + monthList[month][0 + language] + ".";
                }

                break;

        #region Checkpoint

            case "N08.17C":
                ResetScript();
                pC.JumpToNextStep();
                break;

        #endregion

            case "N08.17":
                Color griffBlackBackgroundColor = griffBlackBackground.color;
                griffBlackBackgroundColor.a = 0.9f;
                griffBlackBackground.color = griffBlackBackgroundColor;

                Color aussenringBlackBackgroundColor = aussenringBlackBackground.color;
                aussenringBlackBackgroundColor.a = 0.9f;
                aussenringBlackBackground.color = aussenringBlackBackgroundColor;

                cPC.SetCheckPointColors("N08", 2);
                cPC.SetCheckPointReached("N08", 02);
                turnaeusersterRingOff = true;

                //Hier dem Nutzer dei Möglichkeit geben die restlichen Monate selbst zu erkunden
                rS.AllowRotationVariableWrapper();
                break;

        #region Checkpoint

            case "N09.00C":
                ResetScript();
                Color griffBlackBackgroundColor2 = griffBlackBackground.color;
                griffBlackBackgroundColor2.a = 0.9f;
                griffBlackBackground.color = griffBlackBackgroundColor2;
                pC.JumpToNextStep();
                break;

        #endregion

            case "N09.00":
                rS.DisableRotation();
                pC.JumpToNextStep();
                break;

            case "N09.02":
                cPC.SetCheckPointColors("N09", 3);
                cPC.SetCheckPointReached("N09", 02);
                highlightAussenring = true;
                pC.JumpToNextStep();
                break;

            case "N09.08":
                showSunSymbol = true;
                showAusenringSunSymbol = true;
                sunSymbulTransf.localScale = new Vector3(1.2f, 1.2f, 1.2f);

                //Kalender mit Wochentagen einblenden
                tIC.ShowHorizontalLongDays();
                cSSH.UpdateWeekDaysLongSliderInN08();
                break;

            case "N09.14a":
                showMoonSymbol = true;
                showAusenringMoonSymbol = true;
                moonSymbulTransf.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                break;

            case "N09.15":
                turnMoonSymbolOff = true;
                turnAusenringMoonSymbolOff = true;
                break;

            case "N09.22":
                ssL.FinishedPathPoint("N08");
                Color color = ausenringHighlightMars.color;
                color.a = 0;
                ausenringHighlightMars.color = color;
                ausenringHighlightVenus.color = color;
                turnMoonSymbolOff = true;
                turnAusenringMoonSymbolOff = true;
                showOverview = true;

                float resolutionFactor = DeviceInfo.GetResolutionFactor();
                float overViewScale = (0.75f - resolutionFactor) * 0.35f / 0.3f;
                overViewScale = 0.75f + overViewScale;
                overViewtransf.localScale = new Vector3(overViewScale, overViewScale, overViewScale);
                break;

            case "N09.23":
                turnOverviewOff = true;
                overViewtransf.localScale = new Vector3(0, 0, 0);
                pC.JumpToNextStep();
                break;

            case "N09.24":
                turnHighlightAussenringOff = true;
                rotateBackRing = true;
                break;
        }
    }


    private void Update() {
        //Außenring zum Schluss automatisch rotieren lassen
        if (rotateBackRing) {
            backRingPivot.localEulerAngles += new Vector3(0, 0.2f, 0);
        }

    #region graphical Actions

        if (turnDatumsRingOn) {
            Color color = datumsRingauswahl.color;
            Color griffBlackBackgroundColor = griffBlackBackground.color;
            Color aussenringBlackBackgroundColor = aussenringBlackBackground.color;
            if (color.a < 0.55f) {
                color.a += 0.075f;
                griffBlackBackgroundColor.a += 0.09f;
                datumsRingauswahl.color = color;
                griffBlackBackground.color = griffBlackBackgroundColor;
                aussenringBlackBackground.color = griffBlackBackgroundColor;
            } else {
                color.a = 0.55f;
                datumsRingauswahl.color = color;
                griffBlackBackgroundColor.a = 0.86f;
                griffBlackBackground.color = griffBlackBackgroundColor;
                aussenringBlackBackground.color = griffBlackBackgroundColor;
                turnDatumsRingOn = false;
            }
        }

        if (changeDatumsRing) {
            Color datumsRingauswahlColor = datumsRingauswahl.color;
            Color datumsRingColor = datumsRing.color;
            if (datumsRingauswahlColor.a > 0) {
                datumsRingauswahlColor.a -= 0.1f;
                datumsRingauswahl.color = datumsRingauswahlColor;
                datumsRingColor.a += 0.055f;
                datumsRing.color = datumsRingColor;
            } else {
                datumsRingauswahlColor.a = 0f;
                datumsRingauswahl.color = datumsRingauswahlColor;
                datumsRingColor.a = 0.55f;
                datumsRing.color = datumsRingColor;
                changeDatumsRing = false;
            }
        }

        if (turnDatumsRingOff) {
            Color color = datumsRing.color;
            if (color.a > 0) {
                color.a -= 0.1f;
                datumsRing.color = color;
            } else {
                color.a = 0f;
                datumsRing.color = color;
                turnDatumsRingOff = false;
            }
        }

        if (turnInnersterRingOn) {
            Color color = innersterRing.color;
            if (color.a < 0.55f) {
                color.a += 0.1f;
                innersterRing.color = color;
            } else {
                color.a = 0.55f;
                innersterRing.color = color;
                turnInnersterRingOn = false;
            }
        }

        if (turnInnersterRingOff) {
            Color color = innersterRing.color;
            if (color.a > 0) {
                color.a -= 0.1f;
                innersterRing.color = color;
            } else {
                color.a = 0f;
                innersterRing.color = color;
                turnInnersterRingOff = false;
            }
        }

        if (turnZweitInnersterRingOn) {
            Color color = zweitInnersterRing.color;
            if (color.a < 0.55f) {
                color.a += 0.1f;
                zweitInnersterRing.color = color;
            } else {
                color.a = 0.55f;
                zweitInnersterRing.color = color;
                turnZweitInnersterRingOn = false;
            }
        }

        if (turnZweitInnersterRingOff) {
            Color color = zweitInnersterRing.color;
            if (color.a > 0) {
                color.a -= 0.1f;
                zweitInnersterRing.color = color;
            } else {
                color.a = 0f;
                zweitInnersterRing.color = color;
                turnZweitInnersterRingOff = false;
            }
        }

        if (turnaeusersterRingOn) {
            Color color = aeusersterRing.color;
            if (color.a < 0.55f) {
                color.a += 0.1f;
                aeusersterRing.color = color;
            } else {
                color.a = 0.55f;
                aeusersterRing.color = color;
                turnaeusersterRingOn = false;
            }
        }

        if (turnaeusersterRingOff) {
            Color color = aeusersterRing.color;
            if (color.a > 0) {
                color.a -= 0.1f;
                aeusersterRing.color = color;
            } else {
                color.a = 0f;
                aeusersterRing.color = color;
                turnaeusersterRingOff = false;
            }
        }

        if (highlightAussenring) {
            Color ausenringHighlightsBlackBackgroundColor = ausenringHighlightsBlackBackground.color;
            Color aussenringBlackBackgroundColor = aussenringBlackBackground.color;
            if (ausenringHighlightsBlackBackgroundColor.a < 0.82) {
                ausenringHighlightsBlackBackgroundColor.a += 0.082f;
                ausenringHighlightsBlackBackground.color = ausenringHighlightsBlackBackgroundColor;
                aussenringBlackBackgroundColor.a -= 0.1f;
                aussenringBlackBackground.color = aussenringBlackBackgroundColor;
            } else {
                ausenringHighlightsBlackBackgroundColor.a = 0.82f;
                ausenringHighlightsBlackBackground.color = ausenringHighlightsBlackBackgroundColor;
                aussenringBlackBackgroundColor.a = 0f;
                aussenringBlackBackground.color = aussenringBlackBackgroundColor;
                highlightAussenring = false;
            }
        }

        if (turnHighlightAussenringOff) {
            Color ausenringHighlightsBlackBackgroundColor = ausenringHighlightsBlackBackground.color;
            Color griffBlackBackgroundColor = griffBlackBackground.color;
            if (griffBlackBackgroundColor.a > 0) {
                ausenringHighlightsBlackBackgroundColor.a -= 0.089f;
                ausenringHighlightsBlackBackground.color = ausenringHighlightsBlackBackgroundColor;
                griffBlackBackgroundColor.a -= 0.1f;
                griffBlackBackground.color = griffBlackBackgroundColor;
            } else {
                ausenringHighlightsBlackBackgroundColor.a = 0f;
                ausenringHighlightsBlackBackground.color = ausenringHighlightsBlackBackgroundColor;
                griffBlackBackgroundColor.a = 0f;
                griffBlackBackground.color = griffBlackBackgroundColor;
                turnHighlightAussenringOff = false;
            }
        }

        if (showSunSymbol) {
            if (sunSymbol.alpha < 1) {
                sunSymbol.alpha += 0.1f;
            } else {
                showSunSymbol = false;
            }
        }

        if (turnSunSymbolOff) {
            if (sunSymbol.alpha > 0) {
                sunSymbol.alpha -= 0.1f;
            } else {
                turnSunSymbolOff = false;
            }
        }

        if (showMoonSymbol) {
            if (moonSymbol.alpha < 1) {
                moonSymbol.alpha += 0.1f;
            } else {
                sunSymbulTransf.localScale = new Vector3(0, 0, 0);
                showMoonSymbol = false;
            }
        }

        if (turnMoonSymbolOff) {
            if (moonSymbol.alpha > 0) {
                moonSymbol.alpha -= 0.1f;
            } else {
                moonSymbulTransf.localScale = new Vector3(0, 0, 0);
                turnMoonSymbolOff = false;
            }
        }

        if (showAusenringSunSymbol) {
            Color color = ausenringHighlightSonne.color;
            if (color.a < 1) {
                color.a += 0.1f;
                ausenringHighlightSonne.color = color;
                ausenringHighlightSonneRand.color = color;
            } else {
                color.a = 1f;
                ausenringHighlightSonne.color = color;
                ausenringHighlightSonneRand.color = color;
                showAusenringSunSymbol = false;
            }
        }

        if (turnAusenringSunSymbolOff) {
            Color color = ausenringHighlightSonne.color;
            if (color.a > 0) {
                color.a -= 0.1f;
                ausenringHighlightSonne.color = color;
                ausenringHighlightSonneRand.color = color;
            } else {
                color.a = 0f;
                ausenringHighlightSonne.color = color;
                ausenringHighlightSonneRand.color = color;
                turnAusenringSunSymbolOff = false;
            }
        }

        if (showAusenringMoonSymbol) {
            Color color = ausenringHighlightMond.color;
            if (color.a < 1) {
                color.a += 0.1f;
                ausenringHighlightMond.color = color;
                ausenringHighlightMondRand.color = color;
            } else {
                color.a = 1f;
                ausenringHighlightMond.color = color;
                ausenringHighlightMondRand.color = color;
                showAusenringMoonSymbol = false;
            }
        }

        if (turnAusenringMoonSymbolOff) {
            Color color = ausenringHighlightMond.color;
            if (color.a > 0) {
                color.a -= 0.1f;
                ausenringHighlightMond.color = color;
                ausenringHighlightMondRand.color = color;
            } else {
                color.a = 0f;
                ausenringHighlightMond.color = color;
                ausenringHighlightMondRand.color = color;
                turnAusenringMoonSymbolOff = false;
            }
        }

        if (showOverview) {
            if (overview.alpha < 1) {
                overview.alpha += 0.1f;
            } else {
                overview.alpha = 1;
                turnOverviewOff = false;
            }
        }

        if (turnOverviewOff) {
            if (overview.alpha > 0) {
                overview.alpha -= 0.1f;
            } else {
                overview.alpha = 0;
                overViewtransf.localScale = new Vector3(0, 0, 0);
                turnOverviewOff = false;
            }
        }

    #endregion
    }

    private void ResetScript() {
    #region Reset private Variables

        turnInnersterRingOn = false;
        turnInnersterRingOff = false;
        turnZweitInnersterRingOn = false;
        turnZweitInnersterRingOff = false;
        turnDatumsRingOn = false;
        turnDatumsRingOff = false;
        turnaeusersterRingOn = false;
        turnaeusersterRingOff = false;
        highlightAussenring = false;
        showSunSymbol = false;
        turnSunSymbolOff = true;
        showMoonSymbol = false;
        turnMoonSymbolOff = true;
        turnHighlightAussenringOff = false;
        showAusenringSunSymbol = false;
        turnAusenringSunSymbolOff = true;
        showAusenringMoonSymbol = false;
        turnAusenringMoonSymbolOff = true;
        rotateBackRing = false;
        month = 0;
        dayNameIndex = 0;
        showOverview = false;
        turnOverviewOff = false;

        marsFound = false;
        venusFound = false;

    #endregion

    #region Reset Camera

        skyCam.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = false;
        float latitude = skyTimeController.GetLatitude();
        latitude = -1.0f * latitude;

        float temp = latitude;
        mainCam.transform.localEulerAngles = new Vector3(0, 0, 0);
        mainCam.transform.parent.gameObject.transform.localEulerAngles = new Vector3(temp, 0, 0);

    #endregion

    #region Reset Date and Time

        cc.ShowRealSystemTime();
        cc.ChangeDateTextColor(1);
        cc.ChangeTimeTextColor(1);
        cc.SetCurrentDate();

        tIC.HideScrollPanel();

    #endregion

    #region Reset Other

        okButton.interactable = true;
        wrongAnswerCount = 0;

    #endregion

    #region ResetNokturnal

        backRingPivot.localEulerAngles = new Vector3(0, 0, 0);
        Color aeusersterRingColor = aeusersterRing.color;
        Color zweitInnersterRingcolor = zweitInnersterRing.color;
        Color innersterRingColor = innersterRing.color;
        Color datumsRingColor = datumsRing.color;
        Color datumsRingauswahlcolor = datumsRingauswahl.color;
        Color aussenringBlackBackgroundColor = aussenringBlackBackground.color;
        Color ausenringHighlightsBlackBackgroundColor = ausenringHighlightsBlackBackground.color;
        Color griffBlackBackgroundColor = griffBlackBackground.color;

        aeusersterRingColor.a = 0;
        aeusersterRing.color = aeusersterRingColor;
        zweitInnersterRingcolor.a = 0;
        zweitInnersterRing.color = zweitInnersterRingcolor;
        innersterRingColor.a = 0;
        innersterRing.color = innersterRingColor;
        datumsRingColor.a = 0;
        datumsRing.color = datumsRingColor;
        datumsRingauswahlcolor.a = 0;
        datumsRingauswahl.color = datumsRingauswahlcolor;
        aussenringBlackBackgroundColor.a = 0;
        aussenringBlackBackground.color = aussenringBlackBackgroundColor;
        ausenringHighlightsBlackBackgroundColor.a = 0f;
        ausenringHighlightsBlackBackground.color = ausenringHighlightsBlackBackgroundColor;
        griffBlackBackgroundColor.a = 0f;
        griffBlackBackground.color = griffBlackBackgroundColor;

        Color ausenringHighlightSonneColor = ausenringHighlightSonne.color;
        ausenringHighlightSonneColor = new Color(255, 255, 255, 0);
        ausenringHighlightSonne.color = ausenringHighlightSonneColor;
        ausenringHighlightSonneRand.color = ausenringHighlightSonneColor;
        ausenringHighlightMond.color = ausenringHighlightSonneColor;
        ausenringHighlightMondRand.color = ausenringHighlightSonneColor;

        nokturnalController.NokturnalShowback();
        nokturnalCamera.SetActive(true);
        nokturnalParent.gameObject.SetActive(true);

        float screenFactor = DeviceInfo.GetResolutionFactor() - 0.45f;
        if (screenFactor > 0.175f) {
            nokturnalParent.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        } else if (screenFactor < 0) {
            nokturnalParent.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        } else {
            float tempScale = screenFactor * 0.1f / 0.175f + 1.1f;
            nokturnalParent.localScale = new Vector3(tempScale, tempScale, tempScale);
        }

        nokturnalParent.localPosition = new Vector3(0, 0.25f, 1.954f);
        nokturnalParent.localEulerAngles = new Vector3(0, 180, 0);

        einstellRingPivot.localEulerAngles = new Vector3(90, 0, 75);
        einstellRingPivot.localPosition = new Vector3(-430, 377, 110.8f);

    #endregion
    }

    private IEnumerator WaitForTurnKalenderOn() {
        yield return new WaitForSeconds(1f);
    }

    public void MonthChoosed() {
        //Position der Schablone holen und als gewählten Monat setzen
        month = rS.GetMonth();

        //Drehung der Schablone mit Touch verbieten
        rS.DisableRotation();

        //Ersten Ring einblenden
        changeDatumsRing = true;

        pC.JumpToNextStep();
    }

    //Prüfen welche Eingabe im Wochentags-Kalender gemacht wurde
    public void CheckInput() {
        int answer = cSSH.ClosestItemIndex;

        //Answer=6 => 6.Element des Slider = Sonntag
        if (answer == 6) {
            tIC.HideScrollPanel();
            turnSunSymbolOff = true;
            turnAusenringSunSymbolOff = true;
            pC.JumpWithQuestInput(0);
        } else {
            wrongAnswerCount += 1;
            pC.JumpWithQuestInput(1);
            if (wrongAnswerCount == 2) {
                tIC.HideScrollPanel();
                turnSunSymbolOff = true;
                turnAusenringSunSymbolOff = true;
            }
        }
    }

    //Die einzelnen Symbole (Mars, Venus, Sonne, ...) reagieren auf ein Event und werden automatisch freigeschaltet wenn der 
    //Schritt N09.15 ist, bei Input auf eines der Symbole wird die folgende Funktion aufgerufen
    public void CheckResultN0915(string planet) {
        if (planet == "Mars") {
            marsFound = true;
        }

        if (planet == "Venus") {
            venusFound = true;
        }

        if (marsFound && venusFound) {
            ProcedureController pC = (ProcedureController) gameController.GetComponent(typeof(ProcedureController));
            pC.JumpToNextStep();
        }
    }
}