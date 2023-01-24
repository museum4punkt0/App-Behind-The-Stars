using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InitAbschlussPanel : MonoBehaviour {
#region public Variables

    public RectTransform abschlusstext;
    public RectTransform imageCircle;
    public RectTransform imageCheckmark;
    public RectTransform n03FillCircle;
    public RectTransform statusText;
    public RectTransform textHeadline;
    public RectTransform trennstrichHeadline;
    public RectTransform trennstrich2;

    public TextMeshProUGUI astextfont;
    public TextMeshProUGUI statustextfont;

#endregion

    //Je nach Auflösung müssen die Elemente des Abschlusspanels repositioniert werden
    //Resolutionfactor 0.75 entspricht (1620x2160)
    void Start() {
        float resolutionFactor = DeviceInfo.GetResolutionFactor();
        float textHead = (0.75f - resolutionFactor) * 100.0f / 0.3f;
        textHead = -200 - textHead;
        textHeadline.anchoredPosition = new Vector2(0, textHead);

        float trennstrichHeadlinePosy = (0.75f - resolutionFactor) * 100.0f / 0.3f;
        trennstrichHeadlinePosy = 0 - trennstrichHeadlinePosy;
        trennstrichHeadline.anchoredPosition = new Vector2(0, trennstrichHeadlinePosy);

        float n03fPosY = (0.75f - resolutionFactor) * 475.0f / 0.3f;
        n03fPosY = -325 - n03fPosY;
        n03FillCircle.anchoredPosition = new Vector2(0, n03fPosY);

        float n03sizeDelta = (0.75f - resolutionFactor) * 520.0f / 0.3f;
        n03sizeDelta = 600 + n03sizeDelta;
        n03FillCircle.sizeDelta = new Vector2(n03sizeDelta, n03sizeDelta);

        float imageScale = (0.75f - resolutionFactor) * 0.45f / 0.3f;
        imageScale = 0.55f + imageScale;
        imageCircle.localScale = new Vector3(imageScale, imageScale, imageScale);
        imageCheckmark.localScale = new Vector3(imageScale, imageScale, imageScale);

        float imageCirclePos = (0.75f - resolutionFactor) * 150f / 0.3f;
        imageCirclePos = -380 - imageCirclePos;
        imageCircle.anchoredPosition = new Vector2(0, imageCirclePos);
        imageCheckmark.anchoredPosition = new Vector2(0, imageCirclePos);
        float statustextfontsize = 0;
        if (resolutionFactor < 0.625f) {
            statustextfontsize = (0.625f - resolutionFactor) * 20.0f / 0.175f;
        }

        statustextfontsize = 60 + statustextfontsize;
        statustextfont.fontSize = statustextfontsize;

        float statustextPosY = (0.75f - resolutionFactor) * 80.0f / 0.3f;
        statustextPosY = -150 - statustextPosY;
        statusText.anchoredPosition = new Vector2(0, statustextPosY);

        float trennstrich2Posy = (0.75f - resolutionFactor) * 100.0f / 0.3f;
        trennstrich2Posy = -100 - trennstrich2Posy;
        trennstrich2.anchoredPosition = new Vector2(0, trennstrich2Posy);

        float asPosy = (0.75f - resolutionFactor) * 80.0f / 0.3f;
        asPosy = -50 - asPosy;
        abschlusstext.anchoredPosition = new Vector2(0, asPosy);

        float astextfontsize = 0;
        if (resolutionFactor < 0.625f) {
            astextfontsize = (0.625f - resolutionFactor) * 20.0f / 0.175f;
        }

        astextfontsize = 60 + astextfontsize;
        astextfont.fontSize = astextfontsize;
    }
}