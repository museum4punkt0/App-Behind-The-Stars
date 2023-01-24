using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {
#region public Variables

    public CarouselController cControll;
    public InitScene iScene;
    public ProcedureController pC;

    public Animator menu3dCarousel;
    public Animator abschlussAnimator;
    public Animator mainMenuAnimator;
    public Animator teasertextAnimator;

    public Button burgerMenuButton;
    public Button himmelsglobusImageButton;
    public Button n08Button;
    public Button nokturnalImageButton;
    public Button sonnenuhrImageButton;
    public Button z01Button;
    public Button startButton;
    public Button s02Button;
    public Button s03Button;
    public Button s05Button;

    public CanvasGroup gameFootUI;
    public CanvasGroup gameHeadUI;
    public CanvasGroup mainMenu;

    public GameObject mainMenu3dCarouselGo;
    public GameObject mainmenu3dHideObject;
    public GameObject n08Schloss;
    public GameObject z01Schloss;
    public GameObject s02Schloss;
    public GameObject s03Schloss;
    public GameObject s05Schloss;
    public GameObject sonnenUhrButton;
    public GameObject himmelsglobusButton;
    public GameObject schlossHimmelsglobus;
    public GameObject schlossSonnenuhr;
    public List<GameObject> markerPoints = new List<GameObject>();

    public Sprite markerpointFinished;
    public Sprite markerpointFinishedN08;

    public SpielstandLoader.PathList myPathPointList = new SpielstandLoader.PathList();

    public TextMeshProUGUI burgerMenuPfadBeendenText;
    public TextMeshProUGUI headLineLernpfad;
    public TextMeshProUGUI secondTeaserText;
    public TextMeshProUGUI teaserText;
    public TextMeshProUGUI n08Text;
    public TextMeshProUGUI z01Text;
    public TextMeshProUGUI s02Text;
    public TextMeshProUGUI s03Text;
    public TextMeshProUGUI s05Text;
    public TextMeshProUGUI startButtonText;

    public Transform nokturnalMarkerParent;
    public Transform sonnenuhrMarkerParent;
    public Transform himmelsglobusMarkerParent;
    public Transform arrowParentTransf;
    public Transform mainMenuTransf;
    public Transform gameHeadUITransf;
    public Transform gameFooutUITransf;

#endregion

#region private Variables

    private bool pfadAuswahlActive = false;
    private bool startPath = false;
    private bool showMainMenu = false;
    private bool fileExists = true;
    private bool n01done = true;
    private bool s01Done = false;
    private bool s02Done = false;
    private bool z02Done = false;
    private bool unlockSonnenuhr = false;
    private bool unlockHimmelsglobus = false;
    private bool finishNokturnal = false;
    private bool finishSonnenuhr = false;
    private bool finishNokturnalANimationPlayed = false;
    private bool finishSonnenuhrANimationPlayed = false;

    private int currentMarker = -1;

    private string nokturnalMainTeaser = "";
    private string sonnenuhrMainteaser = "";
    private string himmelsglobusMainTeaser = "";
    private string currentMarkerPath = "-1";

#endregion

    private void Update() {
        if (showMainMenu) {
            if (mainMenu.alpha < 1) {
                mainMenu.alpha += 0.1f;
                gameHeadUI.alpha -= 0.1f;
                gameFootUI.alpha -= 0.1f;
            } else {
                showMainMenu = false;
            }
        }

        if (startPath) {
            if (mainMenu.alpha > 0) {
                mainMenu.alpha -= 0.075f;
                gameHeadUI.alpha += 0.1f;
                gameFootUI.alpha += 0.1f;
            } else {
                mainMenu.alpha = 0f;
                gameHeadUI.alpha = 1f;
                gameFootUI.alpha = 1f;
                mainMenuTransf.localScale = new Vector3(0, 0, 0);
                gameHeadUITransf.localScale = new Vector3(1, 1, 1);
                gameFooutUITransf.localScale = new Vector3(1, 1, 1);
                startPath = false;
            }
        }
    }

    public void ResetScript() {
        abschlussAnimator.Rebind();
        abschlussAnimator.Update(0f);
        currentMarkerPath = "-1";
        currentMarker = -1;
    }

    //Zu Anwendungsstart werden aus der Spielstand.json die erreichten Pfade geladen und hier in der Pathpointlist initialisiert
    //Bei bestimmten Kombinationen (nicht erreichten Pfaden) werden Instrumente gesperrt und müssen freigespielt werden
    public void SetPathPointList(SpielstandLoader.PathList pList, bool initMarker) {
        myPathPointList = pList;
        foreach (SpielstandLoader.PathPoint pP in pList.pathpoins) {
            if (pP.pathID == "N03" && pP.pathFinished || pP.pathID == "N05" && pP.pathFinished
                                                      || pP.pathID == "N07" && pP.pathFinished) {
                n08Button.interactable = true;

                n08Text.faceColor = new Color(158, 157, 236, 255f);
                n08Schloss.SetActive(false);
            }

            if (pP.pathID == "S01" && pP.pathFinished) {
                s01Done = true;

                s02Button.interactable = true;

                s02Text.faceColor = new Color(158, 157, 236, 255f);
                s02Schloss.SetActive(false);
            }

            if (pP.pathID == "S02" && pP.pathFinished) {
                z01Button.interactable = true;

                z01Text.faceColor = new Color(158, 157, 236, 255f);
                z01Schloss.SetActive(false);
            }

            if (pP.pathID == "Z01" && pP.pathFinished) {
                s03Button.interactable = true;
                s05Button.interactable = true;
                s03Text.faceColor = new Color(158, 157, 236, 255f);
                s05Text.faceColor = new Color(158, 157, 236, 255f);
                s03Schloss.SetActive(false);
                s05Schloss.SetActive(false);
            }

            if (pP.pathID == "Z02" && pP.pathFinished) {
                z02Done = true;
            }

            if (initMarker) {
                InitMarkerPointLook(pP.pathID, pP.pathFinished);
            }
        }
    }

    //Aussehen und Zustand der Markerpunkte in der Pfadauswahl initialisieren
    private void InitMarkerPointLook(string pathID, bool pathFinished) {
        foreach (GameObject child in markerPoints) {
            if (child.transform.name.Contains(pathID) && pathFinished) {
                if (pathID == "N08") {
                    child.transform.GetComponent<Image>().sprite = markerpointFinishedN08;
                } else {
                    child.transform.GetComponent<Image>().sprite = markerpointFinished;
                }

                Destroy(child.transform.GetComponent<Animator>());
                child.transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>().color =
                    new Color32(158, 157, 236, 255);
            }
        }
    }

    //Main Menu anzeigen und ggf. eine spezielle Action ausführen, wenn z.B. ein Instrument freigeschaltet wurde oder alle Pfade eines Instruments absolviert wurden
    public void ShowMainMenu() {
        mainMenuTransf.localScale = new Vector3(1, 1, 1);
        showMainMenu = true;
        nokturnalImageButton.interactable = true;
        sonnenuhrImageButton.interactable = true;

    #region unlock Sundial

        if (unlockSonnenuhr) {
            abschlussAnimator.Rebind();
            abschlussAnimator.Update(0f);
            mainMenuAnimator.enabled = true;
            mainMenuAnimator.Rebind();
            mainMenuAnimator.Update(0f);
            mainMenuAnimator.Play("HideLoopMenu", 0, 0);
            mainMenuAnimator.SetInteger("AnimationState", 3);
            mainMenu3dCarouselGo.SetActive(true);
            menu3dCarousel.enabled = true;
            menu3dCarousel.Play("UnlockSonneunhr", 0, 0);
            menu3dCarousel.SetInteger("CarouselAnimationState", 2);
            startButton.interactable = false;
            startButtonText.color = new Color32(61, 61, 67, 255);
            nokturnalImageButton.interactable = false;
            sonnenuhrImageButton.interactable = true;
            pfadAuswahlActive = true;

            headLineLernpfad.text = "NOKTURNAL";
            if (pC.GetLanguage() == 1) {
                headLineLernpfad.text = "NOCTURNAL";
            }

            ChooseLowestNokturnalPath();
            nokturnalMarkerParent.localScale = new Vector3(0.81757f, 0.81757f, 0.81757f);
            StartCoroutine(WaitBeforeSetUnlockFalse());
        }

    #endregion

    #region unlock Celestial Globe

        if (unlockHimmelsglobus) {
            abschlussAnimator.Rebind();
            abschlussAnimator.Update(0f);
            mainMenuAnimator.enabled = true;
            mainMenuAnimator.Rebind();
            mainMenuAnimator.Update(0f);
            mainMenuAnimator.Play("HideLoopMenu", 0, 0);
            mainMenuAnimator.SetInteger("AnimationState", 3);
            mainMenu3dCarouselGo.SetActive(true);
            menu3dCarousel.enabled = true;
            menu3dCarousel.Play("UnlockHimmelsglobus", 0, 0);
            menu3dCarousel.SetInteger("CarouselAnimationState", 3);
            startButton.interactable = false;
            startButtonText.color = new Color32(61, 61, 67, 255);
            nokturnalImageButton.interactable = false;
            sonnenuhrImageButton.interactable = false;
            himmelsglobusImageButton.interactable = false;
            pfadAuswahlActive = true;

            headLineLernpfad.text = "SONNENUHR";
            if (pC.GetLanguage() == 1) {
                headLineLernpfad.text = "SUNDIAL";
            }

            ChooseLowestSonnenuhrPath();
            nokturnalMarkerParent.localScale = new Vector3(0.81757f, 0.81757f, 0.81757f);
            sonnenuhrMarkerParent.localScale = new Vector3(0.8328435f, 0.81757f, 0.81757f);
            StartCoroutine(WaitBeforeSetUnlockFalse());
        }

    #endregion

    #region Finish Nokturnal

        if (finishNokturnal && !s01Done && !finishNokturnalANimationPlayed) {
            abschlussAnimator.Rebind();
            abschlussAnimator.Update(0f);
            mainMenuAnimator.enabled = true;
            mainMenuAnimator.Rebind();
            mainMenuAnimator.Update(0f);
            mainMenuAnimator.Play("HideLoopMenu", 0, 0);
            mainMenuAnimator.SetInteger("AnimationState", 3);
            mainMenu3dCarouselGo.SetActive(true);
            menu3dCarousel.enabled = true;
            menu3dCarousel.Play("3DCarouselFinishNokturnal", 0, 0);
            menu3dCarousel.SetInteger("CarouselAnimationState", 4);
            cControll.BtnClickedMoveRight();
            StartCoroutine(WaitBeforeSetUnlockFalse());
            startButton.interactable = true;
            startButtonText.color = new Color32(255, 255, 255, 255);
            nokturnalImageButton.interactable = true;
            sonnenuhrImageButton.interactable = true;
            nokturnalMarkerParent.localScale = new Vector3(0.81757f, 0.81757f, 0.81757f);
            sonnenuhrMarkerParent.localScale = new Vector3(0, 0, 0);
            pfadAuswahlActive = true;
            finishNokturnalANimationPlayed = true;
            pC.PrinTeaserText(2);
        }

    #endregion

    #region Finish Sundial

        if (finishSonnenuhr && !z02Done && !finishSonnenuhrANimationPlayed) {
            abschlussAnimator.Rebind();
            abschlussAnimator.Update(0f);
            mainMenuAnimator.enabled = true;
            mainMenuAnimator.Rebind();
            mainMenuAnimator.Update(0f);
            mainMenuAnimator.Play("HideLoopMenu", 0, 0);
            mainMenuAnimator.SetInteger("AnimationState", 3);
            mainmenu3dHideObject.SetActive(true);
            mainMenu3dCarouselGo.SetActive(true);
            menu3dCarousel.enabled = true;
            menu3dCarousel.Play("3DCarouselFinishSOnnenuhr", 0, 0);
            menu3dCarousel.SetInteger("CarouselAnimationState", 5);
            cControll.BtnClickedMoveRight();
            StartCoroutine(WaitBeforeSetUnlockFalse());
            startButton.interactable = true;
            startButtonText.color = new Color32(255, 255, 255, 255);
            nokturnalImageButton.interactable = true;
            sonnenuhrImageButton.interactable = true;
            himmelsglobusImageButton.interactable = true;
            nokturnalMarkerParent.localScale = new Vector3(0, 0, 0);
            sonnenuhrMarkerParent.transform.localScale = new Vector3(0, 0, 0);
            finishSonnenuhrANimationPlayed = true;
            pfadAuswahlActive = false;
            pC.PrinTeaserText(3);
        }

    #endregion
    }

#region Handler-Function Instrument Choosed

    //CarouselController ruft die Funktion zum ausgewählten Instrument. Instrument wird aktiv gesetzt (Teaser + ggf. Pfadauswahl werden angezeigt)
    public void SetNokturnalActive() {
        headLineLernpfad.text = "NOKTURNAL";
        if (pC.GetLanguage() == 1) {
            headLineLernpfad.text = "NOCTURNAL";
        }

        if (!pfadAuswahlActive) {
            teaserText.text = nokturnalMainTeaser;
        } else {
            ChooseLowestNokturnalPath();
            nokturnalImageButton.interactable = false;
            nokturnalMarkerParent.localScale = new Vector3(0.81757f, 0.81757f, 0.81757f);
        }

        startButton.interactable = true;
        startButtonText.color = new Color32(255, 255, 255, 255);
    }

    public void SetSonnenuhrActive() {
        headLineLernpfad.text = "SONNENUHR";
        if (pC.GetLanguage() == 1) {
            headLineLernpfad.text = "SUNDIAL";
        }

        if (!pfadAuswahlActive) {
            teaserText.text = sonnenuhrMainteaser;
        } else {
            if (s01Done) {
                ChooseLowestSonnenuhrPath();
                sonnenuhrImageButton.interactable = false;
                sonnenuhrMarkerParent.transform.localScale = new Vector3(0.8328435f, 0.81757f, 0.81757f);
            } else {
                teaserText.text = sonnenuhrMainteaser;
                sonnenuhrImageButton.interactable = true;
            }
        }

        startButton.interactable = true;
        startButtonText.color = new Color32(255, 255, 255, 255);
        if (schlossSonnenuhr.activeSelf) {
            teaserText.text = "Um die Sonnenuhr besser zu verstehen, erkunde bitte zuerst die Sternuhr.";
            if (pC.GetLanguage() == 1) {
                teaserText.text =
                    "To better understand this sundial, please first explore the instrument to the left. ";
            }

            startButton.interactable = false;
            startButtonText.color = new Color32(61, 61, 67, 255);
            sonnenuhrImageButton.interactable = false;
        } else {
            if (!s01Done) {
                sonnenuhrImageButton.interactable = true;
            }
        }
    }

    public void SetHimmelsglobusActive() {
        headLineLernpfad.text = "HIMMELSGLOBUS";
        if (pC.GetLanguage() == 1) {
            headLineLernpfad.text = "CELESTIAL GLOBE";
        }

        if (schlossHimmelsglobus.activeSelf) {
            teaserText.text =
                "Um diesen Himmelsglobus besser zu verstehen, erkunde bitte zunächst die anderen beiden Instrumente.";
            if (pC.GetLanguage() == 1) {
                teaserText.text = "To better understand this globe, please explore the other instruments first.";
            }

            startButton.interactable = false;
            startButtonText.color = new Color32(61, 61, 67, 255);
        } else {
            himmelsglobusImageButton.interactable = true;
            teaserText.text = himmelsglobusMainTeaser;
            startButton.interactable = true;
            startButtonText.color = new Color32(255, 255, 255, 255);
        }
    }

#endregion

#region Teaser Handling

    //Die teaser texte der Content.json werden zum Anwendungsstart in lokale Variablen gespeichert, bzw. bei Sprachwechsel aktualisiert
    public void InitNokturnalMainTeaser(string _teaserText) {
        nokturnalMainTeaser = _teaserText;
        teaserText.text = _teaserText;
    }

    public void InitSonnenuhrMainTeaser(string _teaserText) {
        sonnenuhrMainteaser = _teaserText;
    }

    public void InitHimmelsglobusMainTeaser(string _teaserText) {
        himmelsglobusMainTeaser = _teaserText;
    }

    //Teasertext des ausgewählten Instruments setzen. Wenn Instrument noch nicht freigespielt ist, auf andere instrumente verweisen
    public void UpdateTeaserTextWhileChangeLanguage() {
        if (!pfadAuswahlActive) {
            if (cControll.GetCenterItem() == 0) {
                teaserText.text = nokturnalMainTeaser;
            }

            if (cControll.GetCenterItem() == 1) {
                teaserText.text = sonnenuhrMainteaser;
                if (schlossSonnenuhr.activeSelf) {
                    teaserText.text = "Um die Sonnenuhr besser zu verstehen, erkunde zunächst die Sternuhr.";
                    if (pC.GetLanguage() == 1) {
                        teaserText.text =
                            "To better understand this instrument please explore the instrument to the left.";
                    }
                }
            }

            if (cControll.GetCenterItem() == 2) {
                teaserText.text = himmelsglobusMainTeaser;
                if (schlossHimmelsglobus.activeSelf) {
                    teaserText.text =
                        "Um den Himmelsglobus besser zu verstehen, erkunde zunächst die anderen beiden Instrumente.";
                    if (pC.GetLanguage() == 1) {
                        teaserText.text =
                            "To better understand this instrument please explore the other instruments first.";
                    }
                }
            }
        }
    }

#endregion

    //Markerpunkt in der Pfadauswahl wurde angeklickt
    public void MarkerPointClicked(string markerPathID) {
        if (currentMarkerPath != markerPathID) {
            pC.JumpToMarkerPoint(markerPathID);
            startButton.interactable = true;
            startButtonText.color = new Color32(255, 255, 255, 255);
            currentMarkerPath = markerPathID;
        }
    }

    //Ein Pfad wurde automatisch als nächster Pfad ausgewählt, wenn er der niedrigste noch nicht gespielte Pfad ist
    public void SetMarkerPoint(string markerPathID) {
        pC.JumpToMarkerPoint(markerPathID);
        startButton.interactable = true;
        startButtonText.color = new Color32(255, 255, 255, 255);
        currentMarkerPath = markerPathID;
    }

    //Wenn ein Marker geklickt wurde, alle anderen Marker in ihren Ausgangszustand zurückversetzen
    public void MarkerPointClickedSetButtonBackground(int markerNumber) {
        if (markerNumber != currentMarker) {
            foreach (GameObject child in markerPoints) {
                child.transform.GetChild(0).transform.localScale = new Vector3(0, 0, 0);
                if (child.transform.GetComponent<Image>().sprite.name == "markerpunkt"
                    || child.transform.GetComponent<Image>().sprite.name == "markerpunktrueck") {
                    child.transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>().color =
                        new Color32(158, 157, 236, 255);
                } else {
                    child.transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>().color =
                        new Color32(255, 255, 255, 255);
                }
            }

            markerPoints[markerNumber].transform.GetChild(0).transform.localScale = new Vector3(1, 1, 1);
            markerPoints[markerNumber].transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>().color =
                new Color32(61, 61, 67, 255);
        }
    }

    //Wird ausgeführt wenn der Startbutton (Mainmenu unten) oder auf das Istrument Image geklickt wird,
    //je nach Zustand (Instrumentauswahl oder Pfadauswahl) wird die nächste Aktion ausgeführt
    public void StartClicked() {
        nokturnalImageButton.interactable = false;
        sonnenuhrImageButton.interactable = false;
        mainMenuAnimator.enabled = true;

        //Wenn der erste Sonnenuhrpfad absolviert wurde, Marker der Sonnenuhrpfade anzeigen
        if (sonnenUhrButton.activeSelf && s01Done) {
            sonnenuhrMarkerParent.transform.localScale = new Vector3(0.8328435f, 0.81757f, 0.81757f);
        }

        if (himmelsglobusButton.activeSelf) {
            himmelsglobusMarkerParent.transform.localScale = new Vector3(0.81757f, 0.81757f, 0.81757f);
        }

        if (!n01done) {
            //Wenn n01 noch nicht absolviert, dann direkt den ersten Nokturnal-Pfad starten und nicht die Marker anzeigen
            iScene.SetLastObject(0);
            StartCoroutine(WaitbeforeHideMenu());

            gameHeadUITransf.localScale = new Vector3(1, 1, 1);
            gameFooutUITransf.localScale = new Vector3(1, 1, 1);
            burgerMenuPfadBeendenText.color = new Color32(158, 157, 236, 255);
            burgerMenuButton.interactable = true;
            cControll.StopUpdateLoopMenu();
            pC.JumpToCheckPoint("N01.00");
        } else if (cControll.GetCenterItem() == 1 && !s01Done) {
            //Wenn S01 noch nicht absolviert, direkt ersten Sonnenuhr-Pfad starten und nicht die Marker anzeigen
            iScene.SetLastObject(1);
            StartCoroutine(WaitbeforeHideMenu());

            gameHeadUITransf.localScale = new Vector3(1, 1, 1);
            gameFooutUITransf.localScale = new Vector3(1, 1, 1);
            burgerMenuPfadBeendenText.color = new Color32(158, 157, 236, 255);
            burgerMenuButton.interactable = true;
            headLineLernpfad.text = "SONNENUHR";
            if (pC.GetLanguage() == 1) {
                headLineLernpfad.text = "SUNDIAL";
            }

            cControll.StopUpdateLoopMenu();
            pC.JumpToCheckPoint("S01.00");
        } else if (cControll.GetCenterItem() == 2) {
            //Himmelsglobus starten
            headLineLernpfad.text = "HIMMELSGLOBUS";
            if (pC.GetLanguage() == 1) {
                headLineLernpfad.text = "CELESTIAL GLOBE";
            }

            iScene.SetLastObject(cControll.GetCenterItem());
            StartCoroutine(WaitbeforeHideMenu());

            gameHeadUITransf.localScale = new Vector3(1, 1, 1);
            gameFooutUITransf.localScale = new Vector3(1, 1, 1);
            burgerMenuPfadBeendenText.color = new Color32(158, 157, 236, 255);
            burgerMenuButton.interactable = true;
            mainMenuAnimator.enabled = false;
            cControll.StopUpdateLoopMenu();
            pC.JumpToCheckPoint("Z02.00");
        } else {
            if (pfadAuswahlActive) {
                //Ausgewählten Pfad starten
                iScene.SetLastObject(cControll.GetCenterItem());
                StartCoroutine(WaitbeforeHideMenu());

                gameHeadUITransf.localScale = new Vector3(1, 1, 1);
                gameFooutUITransf.localScale = new Vector3(1, 1, 1);
                burgerMenuPfadBeendenText.color = new Color32(158, 157, 236, 255);
                burgerMenuButton.interactable = true;
                mainMenuAnimator.enabled = false;
                cControll.StopUpdateLoopMenu();
                pC.JumpToNextStep();
            } else {
                mainMenuAnimator.Play("LoopCarouselShowMarker", 0, 0);
                mainMenuAnimator.SetInteger("AnimationState", 2);
                if (cControll.GetCenterItem() == 0) {
                    ChooseLowestNokturnalPath();
                    headLineLernpfad.text = "NOKTURNAL";

                    if (pC.GetLanguage() == 1) {
                        headLineLernpfad.text = "NOCTURNAL";
                    }

                    nokturnalMarkerParent.localScale = new Vector3(0.81757f, 0.81757f, 0.81757f);
                } else if (cControll.GetCenterItem() == 1) {
                    ChooseLowestSonnenuhrPath();
                    headLineLernpfad.text = "SONNENUHR";

                    if (pC.GetLanguage() == 1) {
                        headLineLernpfad.text = "SUNDIAL";
                    }

                    if (s01Done) {
                        sonnenuhrMarkerParent.localScale = new Vector3(0.8328435f, 0.81757f, 0.81757f);
                    }
                }

                pfadAuswahlActive = true;
            }
        }
    }

    //Nach Pfadende oder Pfadabbruch die Instrumentauswahl anzeigen
    public void GoBackToInstrumentChoosing() {
        mainMenuAnimator.enabled = true;
        if (cControll.GetCenterItem() == 0) {
            secondTeaserText.text = nokturnalMainTeaser;
        } else if (cControll.GetCenterItem() == 1) {
            secondTeaserText.text = sonnenuhrMainteaser;
        } else if (cControll.GetCenterItem() == 2) {
            secondTeaserText.text = himmelsglobusMainTeaser;
        }

        teasertextAnimator.SetInteger("AnimStateMoveText", 1);

        nokturnalImageButton.interactable = true;
        sonnenuhrImageButton.interactable = true;
        himmelsglobusImageButton.interactable = true;
        startButton.interactable = true;
        startButtonText.color = new Color32(255, 255, 255, 255);
        arrowParentTransf.localScale = new Vector3(1, 1, 1);
        pfadAuswahlActive = false;
        ResetScript();
        foreach (GameObject child in markerPoints) {
            child.transform.GetChild(0).transform.localScale = new Vector3(0, 0, 0);
        }
    }

    public void InitMainMenuWhileFileNotExists() {
        fileExists = false;
        n01done = false;
    }

    //Wenn ein Nokturnalpfad beendet oder abgebrochen wurde, die Nokturnal-Pfadauswahl wieder anzeigen (außer bei finishnokturnal)
    public void ActivateNokturnalPath() {
        if (!unlockSonnenuhr && !unlockHimmelsglobus && !finishNokturnal) {
            foreach (SpielstandLoader.PathPoint pP in myPathPointList.pathpoins) {
                if (pP.pathID == "N03" && pP.pathFinished || pP.pathID == "N05" && pP.pathFinished
                                                          || pP.pathID == "N07" && pP.pathFinished) {
                    n08Button.interactable = true;

                    n08Text.faceColor = new Color(158, 157, 236, 255f);
                }
            }

            mainMenuAnimator.enabled = true;
            headLineLernpfad.text = "NOKTURNAL";

            if (pC.GetLanguage() == 1) {
                headLineLernpfad.text = "NOCTURNAL";
            }

            pfadAuswahlActive = true;
            nokturnalImageButton.interactable = false;
            sonnenuhrImageButton.interactable = false;
            startButton.interactable = false;
            startButtonText.color = new Color32(61, 61, 67, 255);
            mainMenuAnimator.Play("LoopCarouselShowMarker", 0, 0);
            mainMenuAnimator.SetInteger("AnimationState", 2);

            ChooseLowestNokturnalPath();
            nokturnalMarkerParent.localScale = new Vector3(0.81757f, 0.81757f, 0.81757f);
        } else {
            ChooseLowestNokturnalPath();
            nokturnalMarkerParent.localScale = new Vector3(0.81757f, 0.81757f, 0.81757f);
        }
    }

    //Wenn ein Sonnenuhrpfad beendet oder abgebrochen wurde, die Sonnenuhr-Pfadauswahl wieder anzeigen (außer bei finishsundial)
    public void ActivateSonnenUhrPath() {
        if (!unlockHimmelsglobus && !finishNokturnal && !finishSonnenuhr) {
            foreach (SpielstandLoader.PathPoint pP in myPathPointList.pathpoins) {
                if (pP.pathID == "S01" && pP.pathFinished) {
                    s02Button.interactable = true;

                    s02Text.faceColor = new Color(158, 157, 236, 255f);
                }

                if (pP.pathID == "S02" && pP.pathFinished) {
                    z01Button.interactable = true;

                    z01Text.faceColor = new Color(158, 157, 236, 255f);
                }

                if (pP.pathID == "Z01" && pP.pathFinished) {
                    s03Button.interactable = true;
                    s05Button.interactable = true;

                    s03Text.faceColor = new Color(158, 157, 236, 255f);
                    s05Text.faceColor = new Color(158, 157, 236, 255f);
                }
            }

            mainMenuAnimator.enabled = true;
            headLineLernpfad.text = "SONNENUHR";
            if (pC.GetLanguage() == 1) {
                headLineLernpfad.text = "SUNDIAL";
            }

            mainMenuAnimator.Play("LoopCarouselShowMarker", 0, 0);
            mainMenuAnimator.SetInteger("AnimationState", 2);
            nokturnalImageButton.interactable = false;
            sonnenuhrImageButton.interactable = false;
            startButton.interactable = false;
            startButtonText.color = new Color32(61, 61, 67, 255);
            pfadAuswahlActive = true;
            ChooseLowestSonnenuhrPath();
            if (s01Done) {
                sonnenuhrMarkerParent.localScale = new Vector3(0.8328435f, 0.81757f, 0.81757f);
            }
        } else {
            ChooseLowestSonnenuhrPath();
            if (s01Done) {
                sonnenuhrMarkerParent.localScale = new Vector3(0.8328435f, 0.81757f, 0.81757f);
            }
        }
    }

    //Wenn der Himmelsglobuspfad beendet oder abgebrochen wurde, den Himmelsglobus in der Instrumentauswahl anzeigen
    public void ActivateHimmelsglobusPath() {
        pfadAuswahlActive = true;
        nokturnalImageButton.interactable = false;
        sonnenuhrImageButton.interactable = false;
        himmelsglobusImageButton.interactable = true;
        mainMenuAnimator.enabled = true;
        startButton.interactable = true;
        startButtonText.color = new Color32(255, 255, 255, 255);
        //cSMM.StopMoving();
        headLineLernpfad.text = "HIMMELSGLOBUS";
        if (pC.GetLanguage() == 1) {
            headLineLernpfad.text = "CELESTIAL GLOBE";
        }

        mainMenuAnimator.Play("LoopCarouselShowMarker", 0, 0);
        mainMenuAnimator.SetInteger("AnimationState", 2);
        himmelsglobusMarkerParent.localScale = new Vector3(0.81757f, 0.81757f, 0.81757f);
        pC.PrinTeaserText(3);
    }


    //Niedrigsten Nokturnalpfad automatisch auswählen
    private void ChooseLowestNokturnalPath() {
        int lowestNotFinishedPath = 0;
        for (int i = myPathPointList.pathpoins.Count - 1; i >= 0; i--) {
            if (myPathPointList.pathpoins[i].pathID.Contains("N0")) {
                if (!myPathPointList.pathpoins[i].pathFinished) {
                    if (i != lowestNotFinishedPath) {
                        lowestNotFinishedPath = i;
                    }
                }
            }
        }

        switch (lowestNotFinishedPath) {
            case 1:
                SetMarkerPoint("N03T");
                MarkerPointClickedSetButtonBackground(1);
                break;
            case 2:
                SetMarkerPoint("N05T");
                MarkerPointClickedSetButtonBackground(2);
                break;
            case 3:
                SetMarkerPoint("N07T");
                MarkerPointClickedSetButtonBackground(3);
                break;
            case 4:
                SetMarkerPoint("N08T");
                MarkerPointClickedSetButtonBackground(4);
                break;
        }

        if (lowestNotFinishedPath == 0) {
            SetMarkerPoint("N01T");
            MarkerPointClickedSetButtonBackground(0);
        }
    }

    //Niedrgisten Sonnenuhrpfad automatisch auswählen
    private void ChooseLowestSonnenuhrPath() {
        int lowestNotFinishedPath = 0;
        int temp = 0;
        for (int i = myPathPointList.pathpoins.Count - 1; i >= 0; i--) {
            if (myPathPointList.pathpoins[i].pathID.Contains("S0") ||
                myPathPointList.pathpoins[i].pathID.Contains("Z01")) {
                if (!myPathPointList.pathpoins[i].pathFinished) {
                    if (myPathPointList.pathpoins[i].pathID.Contains("S05")) {
                        temp = 9;
                    }

                    if (myPathPointList.pathpoins[i].pathID.Contains("S03NEW")) {
                        temp = 8;
                    }

                    if (myPathPointList.pathpoins[i].pathID.Contains("S02")) {
                        temp = 6;
                    }

                    if (myPathPointList.pathpoins[i].pathID.Contains("S01")) {
                        temp = 5;
                    }

                    if (myPathPointList.pathpoins[i].pathID.Contains("Z01")) {
                        temp = 7;
                    }

                    if (temp != lowestNotFinishedPath) {
                        if (lowestNotFinishedPath == 0) {
                            lowestNotFinishedPath = temp;
                        } else if (temp < lowestNotFinishedPath) {
                            lowestNotFinishedPath = temp;
                        }
                    }
                }
            }
        }

        switch (lowestNotFinishedPath) {
            case 6:
                SetMarkerPoint("S02T");
                MarkerPointClickedSetButtonBackground(7);
                break;
            case 7:
                SetMarkerPoint("Z01T");
                MarkerPointClickedSetButtonBackground(5);
                break;
            case 8:
                SetMarkerPoint("S03NEWT");
                MarkerPointClickedSetButtonBackground(8);
                break;
            case 9:
                SetMarkerPoint("S05T");
                MarkerPointClickedSetButtonBackground(9);
                break;
        }

        if (lowestNotFinishedPath == 0) {
            SetMarkerPoint("S01T");
            MarkerPointClickedSetButtonBackground(6);
        }
    }

#region Some Get Functions

    public bool GetPfadAuswahlActiveStatus() {
        return pfadAuswahlActive;
    }

    public bool GetN01Done() {
        return n01done;
    }

    public bool GetS01Done() {
        return s01Done;
    }


    public bool GetFinishNokturnalState() {
        return finishNokturnal;
    }

    public bool GetfinishSonnenuhrState() {
        return finishSonnenuhr;
    }

#endregion


    //Ende eines Lernpfads erreicht, der die Sonnenuhr entsperren kann, darauf vorbereiten, wenn der Pfad beendet wird,
    //dass nicht sofort das Main-Menu angezeigt wird sondern eine Animation, die die Sonnenuhr entsperrt
    public void UnlockSonnenuhrInNextStep() {
        unlockSonnenuhr = true;
    }

    //Ende eines Lernpfads erreicht, der den Himmelsglobus entsperren kann, darauf vorbereiten, wenn der Pfad beendet wird,
    //dass nicht sofort das Main-Menu angezeigt wird sondern eine Animation, den Himmelsglobus entsperrt
    public void UnlockHimmelsglobusInNextStep() {
        unlockHimmelsglobus = true;
    }

    //Wenn alle Pfade des Nokturnals durchgespielt, soll nach Pfadende nicht sofort zum Main-Menu gegangen werden, sondern
    //es soll eine Animation angezeigt werden, bei der das Carousel bei der Sonnenuhr endet
    public void FinishNokturnalInNextStep() {
        if (!finishNokturnalANimationPlayed) {
            finishNokturnal = true;
        } else {
            finishNokturnal = false;
        }
    }

    //Wenn alle Pfade der Sonnenuhr durchgespielt, soll nach Pfadende nicht sofort zum Main-Menu gegangen werden, sondern
    //es soll eine Animation angezeigt werden, bei der das Carousel beim Himmelsglobus endet
    public void FinishSonnenuhrInNextStep() {
        if (!finishSonnenuhrANimationPlayed) {
            finishSonnenuhr = true;
        } else {
            finishSonnenuhr = false;
        }
    }

    public void SetN01Done() {
        n01done = true;
    }

    public void SetS01Done() {
        s01Done = true;
    }


    private IEnumerator WaitBeforeSetUnlockFalse() {
        yield return new WaitForSeconds(2f);
        unlockSonnenuhr = false;
        unlockHimmelsglobus = false;
        finishNokturnal = false;
    }

    private IEnumerator WaitbeforeHideMenu() {
        yield return new WaitForSeconds(0.3f);
        startPath = true;
    }
}