using UnityEngine;
using UnityEngine.UI;

public class ScrollPanelAnimatorWrapper : MonoBehaviour {
#region public Variables

    public Animator scrollPanelAnimator;

    public ScrollRect scrollRectClock = null;
    public ScrollRect scrollRectWeekCalendar;
    public ScrollRect scrollRectWeekCalendarDaysLong;
    public ScrollRect scrollRectInputNumber;
    public ScrollRect scrollRectInputDifference;

    //Animation Parameter
    public float normalizedPositionAnimationValue = 0.497143f;

#endregion

#region private Variables

    private bool alleMoveWithAnimation = false;
    private float correctionPositionScrollRectClock = 0.0f;
    private float correctionPositionRectWeekCalendar = 0.0f;
    private float correctionPositionRectWeekCalendarDaysLong = 0.0f;
    private float correctionPositionRectInputNumber = 0.0f;
    private float correctionPositionRectInputDifference = 0.0f;

#endregion

    void Update() {
        if (alleMoveWithAnimation) {
            scrollRectClock.horizontalNormalizedPosition =
                correctionPositionScrollRectClock + normalizedPositionAnimationValue;
            scrollRectWeekCalendar.horizontalNormalizedPosition =
                correctionPositionRectWeekCalendar + normalizedPositionAnimationValue;
            scrollRectWeekCalendarDaysLong.horizontalNormalizedPosition =
                correctionPositionRectWeekCalendarDaysLong + normalizedPositionAnimationValue;
            scrollRectInputNumber.horizontalNormalizedPosition =
                correctionPositionRectInputNumber + normalizedPositionAnimationValue;
            scrollRectInputDifference.horizontalNormalizedPosition =
                correctionPositionRectInputDifference + normalizedPositionAnimationValue;
        }
    }

    //Ausgangsposition des Sliders speichern. Wenn die Hilfe geöffnet wird, soll die Overlay Hand, den Slider ab dieser Position verschieben
    public void CalculateCorrectionPosition() {
        correctionPositionScrollRectClock = scrollRectClock.horizontalNormalizedPosition;
        correctionPositionRectWeekCalendar = scrollRectWeekCalendar.horizontalNormalizedPosition;
        correctionPositionRectWeekCalendarDaysLong = scrollRectWeekCalendarDaysLong.horizontalNormalizedPosition;
        correctionPositionRectInputNumber = scrollRectInputNumber.horizontalNormalizedPosition;
        correctionPositionRectInputDifference = scrollRectInputDifference.horizontalNormalizedPosition;
    }

    public void SetAllowMoveWithAnimation() {
        alleMoveWithAnimation = true;
    }

    public void StopMoveWithAnimation() {
        alleMoveWithAnimation = false;
        scrollPanelAnimator.Rebind();
        scrollPanelAnimator.enabled = false;
    }
}