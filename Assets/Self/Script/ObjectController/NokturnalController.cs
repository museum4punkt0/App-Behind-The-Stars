using UnityEngine;

public class NokturnalController : MonoBehaviour {
#region public Variables

    public SkyTimeController skyTimeController;

    public Transform einstellring;
    public Transform backRingPivot;

    //Nokturnal backside
    public GameObject backlight;
    public GameObject griffBackside;
    public GameObject ringBackside;
    public GameObject schabloneBackside;
    public GameObject tageBackside;
    public GameObject kalenderscheibeBackside;

    //Nokturnal frontside
    public GameObject einstellringFrontside;
    public GameObject einstellringContent;
    public GameObject frontlight;
    public GameObject griffFrontside;
    public GameObject zeigerFrontside;
    public SpriteRenderer datumsanzeigeHighlight;

    //Nokturnal materials
    public Material nokturnalGold;
    public Material nokturnalGoldZeiger;
    public Material nokturnalSilber;
    public SpriteRenderer frontMonatstage;
    public SpriteRenderer nokturnalFrontVerzierung;
    public CanvasGroup fillCircleCG;

#endregion

#region private Variables

    private int[] daysPerMonthArray = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
    private float day = 0;
    private float month = 0;

#endregion

    private void Start() {
        InitEinstellring();
        NokturnalShowFront();
    }

    //Nokturnal Einstellring auf der Vorderseite nach aktuellen Datum ausrichten
    public void InitEinstellring() {
        day = skyTimeController.GetDay() - 1;
        month = skyTimeController.GetMonth();

        float tempDay = 0;

        if (month > 1) {
            for (int i = 0; i < month - 1; i++) {
                tempDay += daysPerMonthArray[i];
            }
        }

        tempDay += day;
        float angleDayRing = (tempDay - 0.0137f * tempDay);
        einstellring.localEulerAngles = new Vector3(90, 0, angleDayRing);
    }

    //Nokturnal ausblenden
    public void NokturnalOff() {
        ringBackside.SetActive(false);
        backlight.SetActive(false);
        griffBackside.SetActive(false);
        schabloneBackside.SetActive(false);
        tageBackside.SetActive(false);
        kalenderscheibeBackside.SetActive(false);

        einstellringFrontside.SetActive(false);
        einstellringContent.SetActive(false);
        frontlight.SetActive(false);
        griffFrontside.SetActive(false);
        zeigerFrontside.SetActive(false);

        Color nokturnalGoldColor = nokturnalGold.color;
        nokturnalGoldColor.a = 0f;
        nokturnalGold.color = nokturnalGoldColor;

        Color nokturnalGoldZeigerColor = nokturnalGoldZeiger.color;
        nokturnalGoldZeigerColor.a = 0;
        nokturnalGoldZeiger.color = nokturnalGoldZeigerColor;

        Color nokturnalSilberColor = nokturnalSilber.color;
        nokturnalSilberColor.a = 0f;
        nokturnalSilber.color = nokturnalSilberColor;

        Color datumsanzeigeHighlightColor = datumsanzeigeHighlight.color;
        datumsanzeigeHighlightColor.a = 0f;
        datumsanzeigeHighlight.color = datumsanzeigeHighlightColor;
    }

    //Nokturnal Vorderseite anzeigen
    public void NokturnalShowFront() {
        einstellringFrontside.SetActive(true);
        einstellringContent.SetActive(true);
        frontlight.SetActive(true);
        griffFrontside.SetActive(true);
        zeigerFrontside.SetActive(true);

        Color nokturnalGoldColor = nokturnalGold.color;
        nokturnalGoldColor.a = 1;
        nokturnalGold.color = nokturnalGoldColor;

        Color nokturnalGoldZeigerColor = nokturnalGoldZeiger.color;
        nokturnalGoldZeigerColor.a = 1;
        nokturnalGoldZeiger.color = nokturnalGoldZeigerColor;

        Color nokturnalSilberColor = nokturnalSilber.color;
        nokturnalSilberColor.a = 1;
        nokturnalSilber.color = nokturnalSilberColor;

        Color monatstageColor = frontMonatstage.color;
        monatstageColor.a = 1;
        frontMonatstage.color = monatstageColor;

        Color nokturnalFrontVerzierungColor = nokturnalFrontVerzierung.color;
        nokturnalFrontVerzierungColor.a = 0.5f;
        nokturnalFrontVerzierung.color = nokturnalFrontVerzierungColor;

        fillCircleCG.alpha = 0;
    }

    //Nokturnal Rückseite anzeigen
    public void NokturnalShowback() {
        ringBackside.SetActive(true);
        backlight.SetActive(true);
        griffBackside.SetActive(true);
        schabloneBackside.SetActive(true);
        tageBackside.SetActive(true);
        kalenderscheibeBackside.SetActive(true);
        einstellringFrontside.SetActive(true);
        einstellringContent.SetActive(false);
        Color nokturnalGoldColor = nokturnalGold.color;
        nokturnalGoldColor.a = 1;
        nokturnalGold.color = nokturnalGoldColor;

        Color nokturnalGoldZeigerColor = nokturnalGoldZeiger.color;
        nokturnalGoldZeigerColor.a = 1;
        nokturnalGoldZeiger.color = nokturnalGoldZeigerColor;

        Color nokturnalSilberColor = nokturnalSilber.color;
        nokturnalSilberColor.a = 1;
        nokturnalSilber.color = nokturnalSilberColor;
        backRingPivot.localEulerAngles = new Vector3(0, 0, 0);
    }
}