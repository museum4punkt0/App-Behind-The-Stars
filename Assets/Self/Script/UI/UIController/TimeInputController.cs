using UnityEngine;

public class TimeInputController : MonoBehaviour {
#region public Varibales

    //stoppt den Input, bei manchen Scrollern soll kein Input möglich sein
    public GameObject blockImage;

    //bei manchen Scrollern wird ein anderer Hintergrund benötigt bspw. MoonToHalfMoon, Monthnames in S02/S03
    public GameObject backgroundImage;

    public GameObject checkInputButton;
    public GameObject gameController;

    //Scroller
    public GameObject horizontalScrollweekCalendar;
    public GameObject horizontalClock;
    public GameObject horizontalLongDays;
    public GameObject horizontalLongMonths;
    public GameObject horizontalInputNumber;
    public GameObject horizontalInputMoonDifference;
    public GameObject horizontalMoonToHalfMoon;

    public RectTransform timePanel;

#endregion

#region private Variables

    private bool moveScrollPanelIn = false;
    private bool moveScrollPanelOut = false;
    private float pivotHeight = 0.0f;
    private string aktStep = "";

#endregion

    private void Start() {
        ProcedureController.changeEvent += DoActionWhileStepUpdate;
    }

    public void DoActionWhileStepUpdate(string stepId) {
        aktStep = stepId;
    }

    public void ResetScript() {
        moveScrollPanelIn = false;
        moveScrollPanelOut = false;

        blockImage.SetActive(false);
        timePanel.pivot = new Vector2(0, 0);
        timePanel.anchoredPosition = new Vector2(timePanel.anchoredPosition.x, 0);
    }

    private void Update() {
        //Slider Parent Panel ins Bild fahren
        if (moveScrollPanelIn) {
            if (timePanel.pivot.y < pivotHeight) {
                timePanel.pivot += new Vector2(0, 0.2f);
                timePanel.anchoredPosition = new Vector2(timePanel.anchoredPosition.x, 0);
            } else {
                moveScrollPanelIn = false;
            }
        }

        //Slider Parent Panel ausblenden
        if (moveScrollPanelOut) {
            if (timePanel.pivot.y > 0) {
                timePanel.pivot -= new Vector2(0, 0.2f);
                timePanel.anchoredPosition = new Vector2(timePanel.anchoredPosition.x, 0);
            } else {
                timePanel.pivot = new Vector2(0, 0);
                timePanel.anchoredPosition = new Vector2(timePanel.anchoredPosition.x, 0);
                moveScrollPanelOut = false;
                checkInputButton.SetActive(false);
            }
        }
    }

#region Show Slider Functions
    //Benötigten Slider einblenden und mit pivotHeight angeben, wie weit das Panle ausgefahren werden soll
    public void ShowInputClock() {
        DeactivateAllScrollPanels();
        horizontalClock.SetActive(true);
        pivotHeight = 2.7f;
        //Bestätigen-Button aktivieren, um bei einer Interactionsaufgabe seine Eingabe prüfen zu können
        checkInputButton.SetActive(true);
        moveScrollPanelIn = true;
        moveScrollPanelOut = false;
    }

    public void ShowInputNumbers() {
        DeactivateAllScrollPanels();
        horizontalInputNumber.SetActive(true);
        pivotHeight = 2.7f;
        checkInputButton.SetActive(true);
        moveScrollPanelIn = true;
        moveScrollPanelOut = false;
    }

    public void ShowInputMoonDifference() {
        DeactivateAllScrollPanels();
        horizontalInputMoonDifference.SetActive(true);
        pivotHeight = 2.7f;
        checkInputButton.SetActive(true);
        moveScrollPanelIn = true;
        moveScrollPanelOut = false;
    }

    public void ShowMoonToHalfMoon() {
        DeactivateAllScrollPanels();
        horizontalMoonToHalfMoon.SetActive(true);
        pivotHeight = 3.5f;
        checkInputButton.SetActive(false);
        moveScrollPanelIn = true;
        moveScrollPanelOut = false;
        blockImage.SetActive(true);
        backgroundImage.SetActive(true);
    }

    public void ShowHorizontalLongDays() {
        DeactivateAllScrollPanels();
        horizontalLongDays.SetActive(true);
        pivotHeight = 2.7f;
        checkInputButton.SetActive(true);
        moveScrollPanelIn = true;
        moveScrollPanelOut = false;
    }

    public void ShowHorizontalLongMonths() {
        DeactivateAllScrollPanels();
        horizontalLongMonths.SetActive(true);
        pivotHeight = 2.7f;
        checkInputButton.SetActive(false);
        moveScrollPanelIn = true;
        moveScrollPanelOut = false;
        blockImage.SetActive(true);
    }

    public void ShowInputDateWeekPanelN03() {
        DeactivateAllScrollPanels();
        horizontalScrollweekCalendar.SetActive(true);
        pivotHeight = 2.7f;
        checkInputButton.SetActive(false);
        moveScrollPanelIn = true;
        moveScrollPanelOut = false;
    }
#endregion
    
    public void HideScrollPanel() {
        moveScrollPanelOut = true;
        moveScrollPanelIn = false;
        blockImage.SetActive(false);
    }
    
    //Alle Slider sind grundsätzlich deaktiviert, nur der benötigte Slider ist active
    private void DeactivateAllScrollPanels() {
        horizontalScrollweekCalendar.SetActive(false);
        horizontalClock.SetActive(false);
        horizontalLongDays.SetActive(false);
        horizontalLongMonths.SetActive(false);
        horizontalInputNumber.SetActive(false);
        horizontalInputMoonDifference.SetActive(false);
        horizontalMoonToHalfMoon.SetActive(false);

        blockImage.SetActive(false);
    }

    //Je nach aktuellen Schritt, wird die Helper Klasse aufgerufen und dort wird dann die Eingabe des Nutzer (z.B. eine Uhrzeit) geprüft
    public void CheckInput() {
        if (aktStep.Contains("N07")) {
            N07Helper n07 = (N07Helper) gameController.GetComponent(typeof(N07Helper));
            n07.CheckInput();
        } else if (aktStep.Contains("N09")) {
            N08Helper n08 = (N08Helper) gameController.GetComponent(typeof(N08Helper));
            n08.CheckInput();
        } else if (aktStep.Contains("S01")) {
            S01Helper s01 = (S01Helper) gameController.GetComponent(typeof(S01Helper));
            s01.CheckS0152Task();
        } else if (aktStep.Contains("S05.16") || aktStep.Contains("S05.25")) {
            S05Helper s05 = (S05Helper) gameController.GetComponent(typeof(S05Helper));
            s05.CheckInputS0516();
        } else if (aktStep.Contains("S05.50") || aktStep.Contains("S05.55")) {
            S05Helper s05 = (S05Helper) gameController.GetComponent(typeof(S05Helper));
            s05.CheckInputS0550();
        } else if (aktStep.Contains("S05.66") || aktStep.Contains("S05.72")) {
            S05Helper s05 = (S05Helper) gameController.GetComponent(typeof(S05Helper));
            s05.CheckInputS0566();
        } else if (aktStep.Contains("S05.78") || aktStep.Contains("S05.84")) {
            S05Helper s05 = (S05Helper) gameController.GetComponent(typeof(S05Helper));
            s05.CheckInputS0578();
        }
    }
}