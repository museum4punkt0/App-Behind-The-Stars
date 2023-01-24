#region Credits
/* Credit Beka Westberg 
* Sourced from - https://bitbucket.org/UnityUIExtensions/unity-ui-extensions/pull-requests/28
* Updated by Stefan Neubert
*/
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class ContentScrollSnapHorizontal : MonoBehaviour, IBeginDragHandler, IEndDragHandler {
#region public Variables

    //Public Scripts
    public SkyTimeController skyTimeController;
    public HelpController hC;
    public ScrollPanelAnimatorWrapper sPAW;
    public ProcedureController pC;
    public WeekCreator wC;

    //other Objects
    public GameObject objectList;

    public Image fillCircle;

    public TextMeshProUGUI clockFormat;
    public TextMeshProUGUI dateText;

    //Vars
    public bool ignoreInactiveItems = true;
    public int snappingVelocityThreshold = 50;

#endregion

#region private Variables

    private bool mSliding = false;
    private bool mLerping = false;
    private bool allowScrolling = false;
    private bool updateClosestItem = false;
    private bool checkInputInN03 = false;
    private bool weekDaySliderActive = false;
    private bool scrollClockSliderActive = false;
    private bool weekDaysLongSliderActive = false;
    private bool scrollInputNumberSliderActive = false;
    private bool scrollMoonDifferenceSliderActive = false;

    private List<DateTime> weekDays = new List<DateTime>();
    private DateTime targetDateN03 = new DateTime();

    private float totalScrollableWidth = 0;
    private float mLerpTime = 0;

    private int _closestItem = 0;

    private RectTransform scrollRectTransform = null;
    private RectTransform contentTransform = null;

    private ScrollRect scrollRect = null;

    private Translator translator;

    private List<Vector3> contentPositions = new List<Vector3>();
    private Vector3 lerpTarget = Vector3.zero;

#endregion

    public int ClosestItemIndex {
        get { return contentPositions.IndexOf(FindClosestFrom(contentTransform.localPosition)); }
    }

    private void Reset() {
        allowScrolling = false;
        updateClosestItem = false;
        weekDaySliderActive = false;
        scrollClockSliderActive = false;
        weekDaysLongSliderActive = false;
        scrollInputNumberSliderActive = false;
        scrollMoonDifferenceSliderActive = false;
    }

#region Setup

    private void Start() {
        scrollRect = GetComponent<ScrollRect>();
        scrollRectTransform = (RectTransform) scrollRect.transform;
        contentTransform = scrollRect.content;

        if (IsScrollRectAvailable) {
            SetupDrivenTransforms();
            SetupSnapScroll();
            scrollRect.horizontalNormalizedPosition = 0;
            _closestItem = 0;
        }

        translator = (Translator) GameObject.Find("GameController").GetComponent(typeof(Translator));
        if (transform.name.Contains("WeekCalendar")) {
            StartCoroutine(GetWeekdays());
        }
    }

    private IEnumerator GetWeekdays() {
        yield return new WaitForSeconds(2f);
        weekDays = wC.GetWeekDayList();

        targetDateN03 = DateTime.Today.AddYears(1);
    }

    private bool IsScrollRectAvailable {
        get {
            if (scrollRect &&
                contentTransform &&
                contentTransform.childCount > 0) {
                return true;
            }

            return false;
        }
    }

    private void SetupDrivenTransforms() {
        foreach (RectTransform child in contentTransform) {
            child.anchorMax = new Vector2(0, 1);
            child.anchorMin = new Vector2(0, 1);
        }
    }

    private void SetupSnapScroll() {
        SetupWithCalculatedSpacing();
    }

    private void SetupWithCalculatedSpacing() {
        //we need them in order from left to right for pagination & buttons & our scrollRectWidth
        List<RectTransform> childrenFromLeftToRight = new List<RectTransform>();
        for (int i = 0; i < contentTransform.childCount; i++) {
            if (!ignoreInactiveItems || contentTransform.GetChild(i).gameObject.activeInHierarchy) {
                RectTransform childBeingSorted = ((RectTransform) contentTransform.GetChild(i));
                int insertIndex = childrenFromLeftToRight.Count;
                for (int j = 0; j < childrenFromLeftToRight.Count; j++) {
                    if (DstFromTopLeftOfTransformToTopLeftOfParent(childBeingSorted).x <
                        DstFromTopLeftOfTransformToTopLeftOfParent(childrenFromLeftToRight[j]).x) {
                        insertIndex = j;
                        break;
                    }
                }

                childrenFromLeftToRight.Insert(insertIndex, childBeingSorted);
            }
        }

        RectTransform childFurthestToTheRight = childrenFromLeftToRight[childrenFromLeftToRight.Count - 1];
        float totalWidth = DstFromTopLeftOfTransformToTopLeftOfParent(childFurthestToTheRight).x +
                           childFurthestToTheRight.sizeDelta.x;

        contentTransform.sizeDelta = new Vector2(totalWidth, contentTransform.sizeDelta.y);
        float scrollRectWidth = Mathf.Min(childrenFromLeftToRight[0].sizeDelta.x,
            childrenFromLeftToRight[childrenFromLeftToRight.Count - 1].sizeDelta.x);

        // Note: sizeDelta will not be calculated properly if the scroll view is set to stretch width.
        scrollRectTransform.sizeDelta = new Vector2(scrollRectWidth, scrollRectTransform.sizeDelta.y);

        contentPositions = new List<Vector3>();
        float widthOfScrollRect = scrollRectTransform.sizeDelta.x;
        totalScrollableWidth = totalWidth - widthOfScrollRect;
        for (int i = 0; i < childrenFromLeftToRight.Count; i++) {
            float offset = DstFromTopLeftOfTransformToTopLeftOfParent(childrenFromLeftToRight[i]).x +
                           ((childrenFromLeftToRight[i].sizeDelta.x - widthOfScrollRect) / 2);
            scrollRect.horizontalNormalizedPosition = offset / totalScrollableWidth;
            contentPositions.Add(contentTransform.localPosition);
        }
    }

#endregion

#region Utilities

    public void OnBeginDrag(PointerEventData ped) {
        if (checkInputInN03) {
            sPAW.StopMoveWithAnimation();
            hC.HideHelpPanel();
            checkInputInN03 = false;
        }

        if (contentPositions.Count < 2) {
            return;
        }

        StopMovement();
    }

    public void OnEndDrag(PointerEventData ped) {
        if (contentPositions.Count <= 1) {
            return;
        }

        if (IsScrollRectAvailable) {
            StartCoroutine("SlideAndLerp");
        }
    }

    private void StopMovement() {
        scrollRect.velocity = Vector2.zero;
        StopCoroutine("SlideAndLerp");
        StopCoroutine("LerpToContent");
    }

    private Vector2 DstFromTopLeftOfTransformToTopLeftOfParent(RectTransform rt) {
        //gets rid of any pivot weirdness
        return new Vector2(rt.anchoredPosition.x - (rt.sizeDelta.x * rt.pivot.x),
            rt.anchoredPosition.y + (rt.sizeDelta.y * (1 - rt.pivot.y)));
    }

    private Vector3 FindClosestFrom(Vector3 start) {
        Vector3 closest = Vector3.zero;
        float distance = Mathf.Infinity;

        foreach (Vector3 position in contentPositions) {
            if (Vector3.Distance(start, position) < distance) {
                distance = Vector3.Distance(start, position);
                closest = position;
            }
        }

        return closest;
    }

    private IEnumerator SlideAndLerp() {
        mSliding = true;
        while (Mathf.Abs(scrollRect.velocity.x) > snappingVelocityThreshold) {
            yield return null;
        }

        lerpTarget = FindClosestFrom(contentTransform.localPosition);

        while (Vector3.Distance(contentTransform.localPosition, lerpTarget) > 1) {
            contentTransform.localPosition =
                Vector3.Lerp(scrollRect.content.localPosition, lerpTarget, 7.5f * Time.deltaTime);
            yield return null;
        }

        mSliding = false;
        scrollRect.velocity = Vector2.zero;
        contentTransform.localPosition = lerpTarget;
    }

#endregion

    private void Update() {
        if (updateClosestItem) {
            if (scrollRect.horizontalNormalizedPosition < -0.005f) {
                scrollRect.horizontalNormalizedPosition = -0.005f;
            }

            if (_closestItem != ClosestItemIndex) {
            #region WeekScroller

                if (weekDaySliderActive) {
                    //letztes Item zurücksetzten (weiße Schrift und nicht fett)
                    objectList.transform.GetChild(_closestItem).transform.GetChild(0).transform
                        .GetComponent<TextMeshProUGUI>().faceColor = new Color32(255, 255, 255, 255);
                    objectList.transform.GetChild(_closestItem).transform.GetChild(0).transform
                        .GetComponent<TextMeshProUGUI>().fontStyle ^= FontStyles.Bold;
                    objectList.transform.GetChild(_closestItem).transform.GetChild(1).transform
                        .GetComponent<TextMeshProUGUI>().faceColor = new Color32(255, 255, 255, 255);
                    objectList.transform.GetChild(_closestItem).transform.GetChild(1).transform
                        .GetComponent<TextMeshProUGUI>().fontStyle ^= FontStyles.Bold;

                    //zentralisiertes Item highlighten (fett und lila)
                    objectList.transform.GetChild(ClosestItemIndex).transform.GetChild(0).transform
                        .GetComponent<TextMeshProUGUI>().faceColor = new Color32(158, 157, 236, 255);
                    objectList.transform.GetChild(ClosestItemIndex).transform.GetChild(0).transform
                        .GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;

                    objectList.transform.GetChild(ClosestItemIndex).transform.GetChild(1).transform
                        .GetComponent<TextMeshProUGUI>().faceColor = new Color32(158, 157, 236, 255);
                    objectList.transform.GetChild(ClosestItemIndex).transform.GetChild(1).transform
                        .GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;

                    skyTimeController.SetDate(weekDays[ClosestItemIndex].Year, weekDays[ClosestItemIndex].Month,
                        weekDays[ClosestItemIndex].Day);
                    dateText.text = weekDays[ClosestItemIndex].Day + "." + weekDays[ClosestItemIndex].Month + "." +
                                    weekDays[ClosestItemIndex].Year;
                    if (pC.GetLanguage() == 1) {
                        string month = translator.GetMonthName(weekDays[ClosestItemIndex].Month);
                        dateText.text = month + " " + weekDays[ClosestItemIndex].Day + ", " +
                                        weekDays[ClosestItemIndex].Year;
                    }

                    if (weekDays[ClosestItemIndex] >= targetDateN03) {
                        scrollRect.enabled = false;
                        pC.JumpToNextStep();
                        updateClosestItem = false;
                    }
                }

            #endregion

            #region ClockScroller

                if (scrollClockSliderActive) {
                    Color colorLila = new Color(0.6196f, 0.61568f, 0.925f, 1);
                    objectList.transform.GetChild(_closestItem).transform.GetChild(2).transform.GetComponent<Text>()
                        .color = Color.white;
                    objectList.transform.GetChild(_closestItem).transform.GetChild(2).transform.GetComponent<Text>()
                        .fontStyle ^= FontStyle.Bold;

                    objectList.transform.GetChild(ClosestItemIndex).transform.GetChild(2).transform.GetComponent<Text>()
                        .color = colorLila;
                    objectList.transform.GetChild(ClosestItemIndex).transform.GetChild(2).transform.GetComponent<Text>()
                        .fontStyle = FontStyle.Bold;

                    if (pC.GetLanguage() == 1) {
                        if (ClosestItemIndex >= 12) {
                            clockFormat.text = " pm";
                        } else {
                            clockFormat.text = " am";
                        }
                    }
                }

            #endregion

            #region WeekdaysLongScroller

                if (weekDaysLongSliderActive) {
                    objectList.transform.GetChild(_closestItem).transform.GetChild(0).transform
                        .GetComponent<TextMeshProUGUI>().faceColor = new Color32(255, 255, 255, 255);
                    objectList.transform.GetChild(_closestItem).transform.GetChild(0).transform
                        .GetComponent<TextMeshProUGUI>().fontStyle ^= FontStyles.Bold;

                    objectList.transform.GetChild(ClosestItemIndex).transform.GetChild(0).transform
                        .GetComponent<TextMeshProUGUI>().faceColor = new Color32(158, 157, 236, 255);
                    objectList.transform.GetChild(ClosestItemIndex).transform.GetChild(0).transform
                        .GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
                }

            #endregion

            #region Digital Slider

                if (scrollInputNumberSliderActive) {
                    objectList.transform.GetChild(_closestItem).transform.GetChild(0).transform
                        .GetComponent<TextMeshProUGUI>().faceColor = new Color32(255, 255, 255, 255);
                    objectList.transform.GetChild(_closestItem).transform.GetChild(0).transform
                        .GetComponent<TextMeshProUGUI>().fontStyle ^= FontStyles.Bold;

                    objectList.transform.GetChild(ClosestItemIndex).transform.GetChild(0).transform
                        .GetComponent<TextMeshProUGUI>().faceColor = new Color32(158, 157, 236, 255);
                    objectList.transform.GetChild(ClosestItemIndex).transform.GetChild(0).transform
                        .GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
                }

            #endregion

            #region Moon Difference Slider

                if (scrollMoonDifferenceSliderActive) {
                    Color colorLila = new Color(0.6196f, 0.61568f, 0.925f, 1);
                    objectList.transform.GetChild(_closestItem).transform.GetChild(2).transform.GetComponent<Text>()
                        .color = Color.white;
                    objectList.transform.GetChild(_closestItem).transform.GetChild(2).transform.GetComponent<Text>()
                        .fontStyle ^= FontStyle.Bold;

                    objectList.transform.GetChild(ClosestItemIndex).transform.GetChild(2).transform.GetComponent<Text>()
                        .color = colorLila;
                    objectList.transform.GetChild(ClosestItemIndex).transform.GetChild(2).transform.GetComponent<Text>()
                        .fontStyle = FontStyle.Bold;
                }

            #endregion

                _closestItem = ClosestItemIndex;
            }
        }
    }

    private void LateUpdate() {
        if (updateClosestItem && weekDaySliderActive) {
            try {
                TimeSpan numberOfDays = weekDays[ClosestItemIndex].Subtract(DateTime.Today);
                float fillValue = numberOfDays.Days / 365.0f;
                float correction = 0;

                if (fillValue < 0.5f) {
                    correction = fillValue * 0.01f / 0.5f;
                } else {
                    correction = 0.005f - ((fillValue - 0.5f) * 0.01f / 0.5f);
                }

                fillCircle.fillAmount = fillValue - correction;
            } catch (ArgumentOutOfRangeException ex) {
            }
        }
    }

    //Wenn Hilfefenster geöffnet wurde, Input checken und bei Input Hilfe schließen
    public void AllowCheckInput() {
        checkInputInN03 = true;
    }

    //Wochenkalender aktivieren und die Startposition initialisieren
    public void UpdateWeekCalenderForN03() {
        allowScrolling = true;
        updateClosestItem = true;
        scrollRect.enabled = true;
        weekDaySliderActive = true;
        scrollRect.horizontalNormalizedPosition = 0.0f;
    }

    //Bspw. wenn mit einem Checkpoint vor oder zurückgesprungen wird, den Wochenkalender zurücksetzen
    public void ResetWeekCalenderForN03() {
        weekDaySliderActive = false;
        scrollRect.horizontalNormalizedPosition = 0.0f;
    }

    public void UpdateScrollClockSlider() {
        updateClosestItem = true;
        scrollRect.horizontalNormalizedPosition = 0.4971429f;
        scrollClockSliderActive = true;
    }

    public void UpdateWeekDaysLongSliderInN08() {
        updateClosestItem = true;
        scrollRect.horizontalNormalizedPosition = 0.4971429f;
        weekDaysLongSliderActive = true;
    }

    public void UpdateScrollInputNumberSliderS05() {
        updateClosestItem = true;
        scrollInputNumberSliderActive = true;
        scrollRect.horizontalNormalizedPosition = 0.0f;
    }

    public void UpdateMoonDifferenceSliderS05() {
        updateClosestItem = true;
        scrollMoonDifferenceSliderActive = true;
        scrollRect.horizontalNormalizedPosition = 0.0f;
    }
}