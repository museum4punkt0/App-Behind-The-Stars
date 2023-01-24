#region Description
/*Zwei Kalender (Mondkalender und Monatskalender) benötigen keine Interactionsmöglichkeiten und werden einfach durch setzen
 * von Zielpositionen (bspw. durch Animationen) verändert.
 */
#endregion

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContentScrollNonInteractableCalendar : MonoBehaviour {
    public GameObject objectList;

#region private Variables

    private bool moveUp = false;
    private bool moveDown = false;
    private bool moonCalendarActive = false;
    private bool monthsCalendarActive = false;

    private float targetScrollPos = 0.0f;
    private static float t = 0.0f;

    private RectTransform scrollRectTransform = null;

    private ScrollRect scrollRect = null;

#endregion

    private void Awake() {
        scrollRect = GetComponent<ScrollRect>();
        scrollRectTransform = (RectTransform) scrollRect.transform;
        scrollRect.enabled = false;
    }

    private void Update() {
    #region MoonCalendar

        if (moonCalendarActive) {
            if (moveUp) {
                if (scrollRect.horizontalNormalizedPosition < targetScrollPos) {
                    scrollRect.horizontalNormalizedPosition =
                        Mathf.Lerp(scrollRect.horizontalNormalizedPosition, targetScrollPos, t);
                    t += 0.03f;
                } else {
                    scrollRect.horizontalNormalizedPosition = targetScrollPos;
                    moveUp = false;
                }
            }
        }

    #endregion

    #region MonthNamesCalendar

        if (monthsCalendarActive) {
            if (moveUp) {
                if (scrollRect.horizontalNormalizedPosition < targetScrollPos) {
                    scrollRect.horizontalNormalizedPosition =
                        Mathf.Lerp(scrollRect.horizontalNormalizedPosition, targetScrollPos, t);
                    t += 0.05f;
                } else {
                    scrollRect.horizontalNormalizedPosition = targetScrollPos;
                    moveUp = false;
                }
            }

            if (moveDown) {
                if (scrollRect.horizontalNormalizedPosition > targetScrollPos) {
                    scrollRect.horizontalNormalizedPosition -= 0.05f;
                } else {
                    scrollRect.horizontalNormalizedPosition = targetScrollPos;
                    moveDown = false;
                }
            }
        }

    #endregion
    }

    public void UpdateFontManually(int aktChild, int lastChild) {
        if (lastChild > -1) {
            objectList.transform.GetChild(lastChild).transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>()
                .color = new Color32(255, 255, 255, 255);
            objectList.transform.GetChild(lastChild).transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>()
                .fontStyle ^= FontStyles.Bold;
        }

        objectList.transform.GetChild(aktChild).transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>()
            .color = new Color32(158, 157, 236, 255);
        objectList.transform.GetChild(aktChild).transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>()
            .fontStyle = FontStyles.Bold;
    }

    public void SetTargetScrollPosMoonCalendar(float target) {
        moonCalendarActive = true;
        moveUp = true;
        targetScrollPos = target;
        t = 0.0f;
    }

    public void SetTargetScrollPosMonthNamesCalendar(float target) {
        monthsCalendarActive = true;
        if (scrollRect.horizontalNormalizedPosition < target) {
            moveUp = true;
            moveDown = false;
        } else if (scrollRect.horizontalNormalizedPosition > target) {
            moveDown = true;
            moveUp = false;
        }

        targetScrollPos = target;
        t = 0.0f;
    }

    public void JumpToStartPos() {
        scrollRect.horizontalNormalizedPosition = 0.0f;
    }
}