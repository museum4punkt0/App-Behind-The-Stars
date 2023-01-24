using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimationEventHandlerN03 : MonoBehaviour {
#region public Variables

    public SkyTimeController skyTimeController;
    public ClockController cc;
    public N03Helper n03H;
    public ProcedureController pC;
    public Translator translator;

    public Button okButton;

    public Image fillCircleImage;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dateText;

    public TextMeshPro text3dTage;

    //Animation Parameter
    public float alphaTextColor = 0;

#endregion

    private bool allowChangeTextColor = false;

    private void Update() {
        if (allowChangeTextColor) {
            //Text vor Beginn der Drehung ausblenden - vor Stopp der Drehung wieder einblenden
            Color textColor = text3dTage.color;
            textColor.a = alphaTextColor;
            text3dTage.color = textColor;
        }
    }

    public void StopAnimationForOneDayN03() {
        n03H.AnimationFinishedRotationOneDayN03();
    }

    //In der Animation wird der Text ausgeblendet wenn die Linie rotiert und eingeblendet kur bevor sie wieder zum stehen kommt
    public void SetAllowChangeTextColor() {
        allowChangeTextColor = true;
    }

    //Der Text mit der Tagesanzahl wird immer dann gewechselt, wenn er gerade ausgeblendet ist
    public void ChangeText(int addDays) {
        if (pC.GetLanguage() == 0) {
            text3dTage.text = "+" + addDays + " TAGE";
        } else if (pC.GetLanguage() == 1) {
            text3dTage.text = "+" + addDays + " DAYS";
        }
    }

    //Am Ende einer Umdrehung, wird die Lilafarbene Scheibe um einen tag erweitert und das neue Datum in der Datumsanzeige gesetzt
    public void SetDateInN03(int addDays) {
        fillCircleImage.fillAmount += 1.0f / 365.0f;
        Vector3 aktDate = skyTimeController.GetDate();
        timeText.text = "00:00";
        int temp = (int) aktDate.z + addDays;
        dateText.text = temp.ToString() + "." + aktDate.y + "." + aktDate.x;
        if (pC.GetLanguage() == 1) {
            timeText.text = "12:00 am";
            string month = translator.GetMonthName((int) aktDate.y);
            dateText.text = month + " " + temp.ToString() + ", " + aktDate.x;
        }

        cc.SetAnimatedDays(addDays);
    }

    public void FinishAnimation7Days() {
        okButton.interactable = true;
    }

    public void StopSetTextColorWithAnim() {
        alphaTextColor = 0;
        allowChangeTextColor = false;
    }
}