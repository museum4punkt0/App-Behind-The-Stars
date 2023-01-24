#region Description
/*Für die Textanzeige gibt es 2 Slots. Der neue Text wird auf Slot 2 gesetzt (rechts außerhalb des Screens),
 dann fährt die Animation nach links. Wenn Slot 1 außerhalb des Screens ist (links vom Screen) wird der Text syncronisiert
 mit Slot 2 und die Positionen der SLots werden zurückgesetzt.*/
#endregion

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextOutputController : MonoBehaviour {
#region public Variables

    public ProcedureController pc;

    public Animator interactionDragAnimator;
    public Animator interactionRotateAnimator;
    public Animator interactionTouchAnimator;
    public Animator textContentPanelAnimator;

    public Button okButton;

    public CanvasGroup interActionDragFirst;
    public CanvasGroup interActionRotateFirst;
    public CanvasGroup interActionTouchFirst;

    public RectTransform firstTextRect;
    public RectTransform firstTextRectUnityUI;

    public Text firstTextUnityUI;

    public TextMeshProUGUI firstText;
    public TextMeshProUGUI secondText;

#endregion

#region private Variables

    private bool interactionTaskActive = false;

    private int interaction = 0;

    private string aktStepType = "";

#endregion

    //Text auf Slot 1 synchronisieren, wenn er komplett außerhalb des Bildes ist
    public void SynchronizeText() {
        firstText.text = secondText.text;
        firstTextUnityUI.text = secondText.text;
        if (pc.GetAktEventType() == "Text") {
            okButton.interactable = true;
        }
    }

    //Am Ende der Animation, den Animator in Ausgangszustand versetzen -> Slot 1 rückt wieder ins Bild
    public void SetAnimStateBack() {
        textContentPanelAnimator.Rebind();
        textContentPanelAnimator.Update(0f);
    }

    //Wenn eine Interactionsbeschreibung angezeigt wird, muss das Textfeld verkleinert werden, um das Interacitons-Icon anzeigen zu können
    public void WaitAndSetFirstTextSize() {
        if (interactionTaskActive) {
            firstTextRect.offsetMin = new Vector2(365, 30);
            firstTextRectUnityUI.offsetMin = new Vector2(365, 30);
            if (interaction == 1) {
                interactionTouchAnimator.enabled = true;
                interActionTouchFirst.alpha = 1;
                interactionDragAnimator.enabled = false;
                interActionDragFirst.alpha = 0;
            } else if (interaction == 2) {
                interactionTouchAnimator.enabled = false;
                interActionTouchFirst.alpha = 0;
                interactionDragAnimator.enabled = true;
                interActionDragFirst.alpha = 1;
                interactionRotateAnimator.enabled = false;
                interActionRotateFirst.alpha = 0;
            } else if (interaction == 3) {
                interactionTouchAnimator.enabled = false;
                interActionTouchFirst.alpha = 0;
                interactionDragAnimator.enabled = false;
                interActionDragFirst.alpha = 0;
                interactionRotateAnimator.enabled = true;
                interActionRotateFirst.alpha = 1;
            }
        } else {
            interActionDragFirst.alpha = 0;
            interActionTouchFirst.alpha = 0;
            interactionTouchAnimator.enabled = false;
            interactionDragAnimator.enabled = false;
            interactionRotateAnimator.enabled = false;
            interActionRotateFirst.alpha = 0;
            firstTextRect.offsetMin = new Vector2(50, 30);
            firstTextRectUnityUI.offsetMin = new Vector2(50, 30);
        }
    }

    public void SetInteractionTaskState(bool stepType, int _interaction) {
        interaction = _interaction;
        interactionTaskActive = stepType;
    }
}