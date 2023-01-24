using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class HelpController : MonoBehaviour {
#region public Variables
    //Scripts
    public OrbitalCameraController oCC;
    public ContentScrollSnapHorizontal cSSHWeekCalendar;
    public ContentScrollSnapHorizontal cSCScrollClock;
    public ContentScrollSnapHorizontal cSCWeekDaysLong;
    public ContentScrollSnapHorizontal cSINInputNumber;
    public ContentScrollSnapHorizontal cSINMoonDif;
    public N01Helper n01H;
    public ProcedureController pC;
    public ScrollPanelAnimatorWrapper sPAW;
    public ToolTipController tTC;

    //Objects
    public Animator scrollClockAnimator;
    public Animator tippHighlightAnimator;
    public Animator toolTipAnimator;


    public Image helpImage;
    public Image tippHighlight;

    public RectTransform helpImageRectT;
    public RectTransform helpPanel;

    public Sprite tippHighlight_DE;
    public Sprite tippHighlight_EN;

    public Text helpText;

    public Transform buttonBlockTransf;

#endregion

#region private Variables

    private bool moveHelpPanelInWithImage = false;
    private bool moveHelpPanelInWithOutImage = false;
    private bool moveHelpPanelOut = false;
    private bool helpActive = false;

    private string aktStep = "";

#endregion

    private void Start() { 
        ProcedureController.changeEvent += DoActionWhileStepUpdate;
    }

    public void DoActionWhileStepUpdate(string stepId) {
        aktStep = stepId;
        
        //Wenn in der Content-Json ein Hilfe Text eingetragen ist, dann highlight die Schrift auf dem Hilfe-Button
        if (pC.myBubbleList.bubbles[pC.indexOfAktStep].bubbleHelp.Length > 3) {
            tippHighlightAnimator.enabled = true;
            tippHighlightAnimator.Play("HighlightTippFont", 0, 0);
            tippHighlightAnimator.SetInteger("AnimationState", 1);
            if (pC.GetLanguage() == 0) {
                tippHighlight.sprite = tippHighlight_DE;
            } else if (pC.GetLanguage() == 1) {
                tippHighlight.sprite = tippHighlight_EN;
            }
        } else {
            tippHighlightAnimator.Rebind();
            tippHighlightAnimator.enabled = false;
        }
    }

    private void Update() {
        if (moveHelpPanelInWithImage) {
            if (helpPanel.pivot.y > 0) {
                helpPanel.pivot -= new Vector2(0, 0.2f);
            } else {
                helpPanel.anchoredPosition = new Vector2(0, 0);
                moveHelpPanelInWithImage = false;
            }
        }

        if (moveHelpPanelInWithOutImage) {
            if (helpPanel.pivot.y > 0.3f) {
                helpPanel.pivot -= new Vector2(0, 0.2f);
            } else {
                helpPanel.anchoredPosition = new Vector2(0, 0.3f);
                moveHelpPanelInWithOutImage = false;
            }
        }

        if (moveHelpPanelOut) {
            if (helpPanel.pivot.y < 2) {
                helpPanel.pivot += new Vector2(0, 0.2f);
                helpPanel.anchoredPosition = new Vector2(0, 0);
            } else {
                buttonBlockTransf.localScale = new Vector3(0, 0, 0);
                moveHelpPanelOut = false;
            }
        }
    }

    //Hilfe Panel von unten einfahren
    public void ShowHelpPanel() {
        helpActive = true;
        moveHelpPanelOut = false;
        buttonBlockTransf.localScale = new Vector3(1, 1, 1);
        
        //Wenn ein Bildname in der Json eingetragen ist, wird hier das Bild aus dem Resources Ordner geladen und in der Hilge angezeigt
        string imageName = pC.myBubbleList.bubbles[pC.indexOfAktStep].bubbleImage;
        if (imageName.Length > 3) {
            var texHI = Resources.Load<Texture2D>("Images/" + imageName);
            var spriteHI = Sprite.Create(texHI, new Rect(0.0f, 0.0f, texHI.width, texHI.height),
                new Vector2(0.5f, 0.5f), 100.0f);
            helpImageRectT.sizeDelta = new Vector2(texHI.width, texHI.height);
            helpImage.sprite = spriteHI;
            moveHelpPanelInWithImage = true;
            moveHelpPanelInWithOutImage = false;
        } else {
            helpImageRectT.sizeDelta = new Vector2(1200, 0);
            moveHelpPanelInWithOutImage = true;
            moveHelpPanelInWithImage = false;
        }

        //Wenn in der Json kein Text für die Hilfe eingetragen ist, dann Hinweis auf Weiter-Button einblenden
        if (pC.GetLanguage() == 0) {
            helpText.text = pC.myBubbleList.bubbles[pC.indexOfAktStep].bubbleHelp;
            if (pC.myBubbleList.bubbles[pC.indexOfAktStep].bubbleHelp.Length < 3) {
                helpText.text = "Klicke auf WEITER, um zum nächsten Schritt zu gelangen.";
            }
        } else if (pC.GetLanguage() == 1) {
            helpText.text = pC.myBubbleList.bubbles[pC.indexOfAktStep].bubbleHelp_EN;
            if (pC.myBubbleList.bubbles[pC.indexOfAktStep].bubbleHelp.Length < 3) {
                helpText.text = "Tap NEXT to advance.";
            }
        }

        #region Show Tutorials
        if (aktStep == "N01.03" || aktStep == "N01.04") {
            buttonBlockTransf.localScale = new Vector3(0, 0, 0);
            oCC.SetAllowCheckInputWhileTutorial01IsOn();
            toolTipAnimator.enabled = true;
            toolTipAnimator.Play("Tutorial02MoveKamera", 0, 0);
            toolTipAnimator.SetInteger("AnimationState", 1);
            tTC.ShutDownTooltip();
            pC.HelpTutorialAbsolved();
        }

        if (aktStep == "N03.15" || aktStep == "N03.15C") {
            scrollClockAnimator.enabled = true;
            scrollClockAnimator.Play("Tutorial04InteraktionsWalzeOhneBestaetigen", 0, 0);
            scrollClockAnimator.SetInteger("AnimState", 2);
            sPAW.CalculateCorrectionPosition();
            cSSHWeekCalendar.AllowCheckInput();
            cSCScrollClock.AllowCheckInput();
        }

        if (aktStep == "N07.23" || aktStep == "N07.26" || aktStep == "N09.08" || aktStep == "N09.09"
            || aktStep == "S01.52" || aktStep == "S01.53" || aktStep == "S05.16" || aktStep == "S05.25"
            || aktStep == "S05.50" || aktStep == "S05.55" || aktStep == "S05.66" || aktStep == "S05.72"
            || aktStep == "S05.78" || aktStep == "S05.84") {
            scrollClockAnimator.enabled = true;
            scrollClockAnimator.Play("Tutorial04InteraktionsWalzeOverClock", 0, 0);
            scrollClockAnimator.SetInteger("AnimState", 1);
            sPAW.CalculateCorrectionPosition();
            buttonBlockTransf.localScale = new Vector3(1, 0.72f, 1);
            cSSHWeekCalendar.AllowCheckInput();
            cSCScrollClock.AllowCheckInput();
            cSCWeekDaysLong.AllowCheckInput();
            cSINInputNumber.AllowCheckInput();
            cSINMoonDif.AllowCheckInput();
        }
        #endregion
    }

    //Hilfe ausblenden
    public void HideHelpPanel() {
        moveHelpPanelInWithOutImage = false;
        moveHelpPanelInWithImage = false;
        moveHelpPanelOut = true;
        StartCoroutine(ResetBool());
        tTC.StopMoveCamWithAnim();
        toolTipAnimator.Rebind();
        sPAW.StopMoveWithAnimation();
        if (aktStep == "N01.03" || aktStep == "N01.04") {
            n01H.HighlightGrosserWagen(false);
        }
    }

    private IEnumerator ResetBool() {
        yield return new WaitForSeconds(1f);
        helpActive = false;
    }

    //Der State ob die Hilfe aktiv ist, ist für manche Interatkions-Inputs wichtig, wenn zum Beispiel eine Bewegung durch 
    //Touch am Himmel erfolgt, soll dei Hilfe in dem Moment ausgeblendet werden
    public bool GetState() {
        return helpActive;
    }
}