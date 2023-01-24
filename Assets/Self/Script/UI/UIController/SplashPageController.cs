using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SplashPageController : MonoBehaviour {
#region public Variables

    //Scripts

    public ProcedureController pC;

    //Objects
    public Animator carouselAnimator;
    public Animator splashAnimator;

    public Button deutschButton;
    public Button englischButton;

    public GameObject blackblend;
    public GameObject splashPage;
    public GameObject splashPageContentDE;
    public GameObject splashPageContentEN;

    public TextMeshProUGUI deutschSplashButtonText;
    public TextMeshProUGUI englishSplashButtonText;
    public TextMeshProUGUI languageButtonText;
    public TextMeshProUGUI splashContent;
    public TextMeshProUGUI splashContentEN;
    public TextMeshProUGUI splashHeadline;

    public RectTransform splashHeadlineRect;
    public RectTransform videoRawImage;
    public RectTransform deutschSplashContent;
    public RectTransform englishSplashContent;

    //Variables
    public float sizeDelta = 1920;
    public float anchorY = 0;

#endregion

    private float resolutionFactor = 0;

    //Je nach Auflösung müssen die Elemente des Abschlusspanels repositioniert werden
    //Resolutionfactor 0.75 entspricht (1620x2160)
    private void Start() {
        resolutionFactor = DeviceInfo.GetResolutionFactor();
        float textHead = (0.75f - resolutionFactor) * 100.0f / 0.3f;
        textHead = -250 - textHead;
        splashHeadlineRect.anchoredPosition = new Vector2(0, textHead);

        float headtextfontsize = (0.75f - resolutionFactor) * 25.0f / 0.3f;
        headtextfontsize = 120 + headtextfontsize;
        splashHeadline.fontSize = headtextfontsize;

        float textfontSize = (0.75f - resolutionFactor) * 20.0f / 0.3f;
        textfontSize = 70 + textfontSize;
        splashContent.fontSize = textfontSize;
        splashContentEN.fontSize = textfontSize;

        float newLeft = ((resolutionFactor - 0.45f) * 90.0f / 0.3f) + 210;
        deutschSplashContent.offsetMin = new Vector2(newLeft, deutschSplashContent.offsetMin.y);
        deutschSplashContent.offsetMax = new Vector2(-newLeft, deutschSplashContent.offsetMax.y);
        englishSplashContent.offsetMin = new Vector2(newLeft, englishSplashContent.offsetMin.y);
        englishSplashContent.offsetMax = new Vector2(-newLeft, englishSplashContent.offsetMax.y);
    }

    private void Update() {
        float videoPosStrengthFactor = anchorY / 850.0f;
        float videoPosY = ((resolutionFactor - 0.45f) * 300.0f / 0.3f) * videoPosStrengthFactor;
        videoPosY = anchorY - videoPosY;
        videoRawImage.anchoredPosition = new Vector2(0, videoPosY);

        float videoSize = (resolutionFactor - 0.45f) * 500.0f / 0.3f;
        videoSize = sizeDelta - videoSize;
        videoRawImage.sizeDelta = new Vector2(videoSize, videoSize);
    }

    //Wenn der Start-Button gedrückt wurde, SplashPage ausblenden und die Karusell-Animation starten
    public void HideSplashPage(int _language) {
        splashAnimator.Play("HideSplashPage", 0, 0);
        splashAnimator.SetInteger("splashState", 1);

        carouselAnimator.enabled = true;
        var loadData = "0";
        try {
            var filePath = Path.Combine(Application.persistentDataPath, "spielstand.txt");
            loadData = File.ReadAllText(filePath);
        } catch (FileNotFoundException ex) {
        }

        if (loadData == "2") {
            carouselAnimator.Play("3dCarouselStartRotationSchnell", 0, 0);
            carouselAnimator.SetInteger("CarouselAnimationState", 6);
        } else {
            carouselAnimator.Play("3dCarouselStartRotationToNokturnal", 0, 0);
            carouselAnimator.SetInteger("CarouselAnimationState", 1);
        }
    }

    public void DeactivateSplashPage() {
        blackblend.SetActive(true);
        splashPage.SetActive(false);
    }

    //Sprachwahl auf der SplashPage
    public void BtnDeutschClicked() {
        pC.SetLanguage(0);
        splashPageContentDE.SetActive(true);
        splashPageContentEN.SetActive(false);
        deutschButton.interactable = false;
        englischButton.interactable = true;
        deutschSplashButtonText.color = new Color32(158, 157, 236, 255);
        englishSplashButtonText.color = new Color32(255, 255, 255, 255);
    }

    public void BtnEnglishClicked() {
        pC.SetLanguage(1);
        splashPageContentDE.SetActive(false);
        splashPageContentEN.SetActive(true);
        englischButton.interactable = false;
        deutschButton.interactable = true;
        deutschSplashButtonText.color = new Color32(255, 255, 255, 255);
        englishSplashButtonText.color = new Color32(158, 157, 236, 255);
    }
}