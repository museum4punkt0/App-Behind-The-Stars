using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClockController : MonoBehaviour {
#region public Variables

    public SkyTimeController skyTimeController;
    public Translator translator;
    public ProcedureController pC;

    public GameObject starrySky;
    public GameObject gameController;

    public Image fillCircle;

    public RectTransform newPathRect;
    public RectTransform fillCircleRect;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dateText;

    public Transform schuettkantenstern;
    public Transform rotationPivotForN03;

    public UILineRenderer newPathUILineRenderer;

#endregion

#region private Variables

    private bool allowIncrease = true;
    //Zeitanzeige nach SkyTimeController
    private bool showRealTime = false;
    private bool updateDate = true;
    private bool allowSwitch = true;
    private bool firstclick = true;
    //Zeitanzeige mit der wahren Zeit
    private bool setTimeWithRealTime = true;

    private float dayOfTheYear = 0;

    private int timeMode = 0;
    private int animatedDays = 0;

    private Vector2 aktTime = new Vector2();
    private Vector3 startDate;

#endregion

    private void Start() {
        skyTimeController.changeMinute += DoActionWhileStepUpdate;
        skyTimeController.changeDay += DoActionWhileStepUpdate;
        StartCoroutine(InitStartDate());
    }

    //In manchen Pfaden ist das Datumsanzeige unrelevant, mit coloVarlue=1 kann sie "deaktiviert" werden
    public void ChangeDateTextColor(int colorValue) {
        //colorValue=0-weiß, 1-grau
        if (colorValue == 0) {
            dateText.faceColor = new Color32(255, 255, 255, 255);
        } else if (colorValue == 1) {
            dateText.faceColor = new Color32(61, 61, 67, 255);
        }
    }

    //In manchen Pfaden ist die Zeitanzeige unrelevant, mit coloVarlue=1 kann sie "deaktiviert" werden
    public void ChangeTimeTextColor(int colorValue) {
        //colorValue=0-weiß, 1-grau
        if (colorValue == 0) {
            timeText.faceColor = new Color32(255, 255, 255, 255);
        } else if (colorValue == 1) {
            timeText.faceColor = new Color32(61, 61, 67, 255);
        }
    }

    //Zeit des SkytimeControlles in der Zeitanzeige anzeigen
    public void SetTimecontrollerDate() {
        Vector3 timeControllerDate = skyTimeController.GetDate();
        dateText.text = timeControllerDate.z + "." + timeControllerDate.y + "." + timeControllerDate.x;

        if (pC.GetLanguage() == 1) {
            string month = translator.GetMonthName((int) startDate.y);
            dateText.text = month + " " + timeControllerDate.z + ", " + timeControllerDate.x;
        }
    }

    //Tatsächliche Ortszeit in der Zeitanzeige anzeigen
    public void SetCurrentDate() {
        dateText.text = DateTime.Today.Day + "." + DateTime.Today.Month + "." + DateTime.Today.Year;
        if (pC.GetLanguage() == 1) {
            string month = translator.GetMonthName(DateTime.Today.Month);
            dateText.text = month + " " + System.DateTime.Today.Day + ", " + System.DateTime.Today.Year;
        }
    }

    //Schreibt die Zeit in die Zeitanzeige
    public void SetTime(Vector2 _time) {
        int hours = (int) _time.x;
        int minutes = (int) _time.y;
        if (hours >= 24) {
            hours -= 24;
        } else if (hours < 0) {
            hours += 24;
        }

        //für eine saubere Zeitanzeige muss in den Strin eine "0" eiungefügt werrden, wenn Stunden oder Minuten einstellig sind
        if (hours < 10 && minutes < 10) {
            timeText.text = "0" + hours.ToString() + ":0" + minutes.ToString();
        } else if (hours < 10 && minutes >= 10) {
            timeText.text = "0" + hours.ToString() + ":" + minutes.ToString();
        } else if (hours >= 10 && minutes < 10) {
            timeText.text = hours.ToString() + ":0" + minutes.ToString();
        } else {
            timeText.text = hours.ToString() + ":" + minutes.ToString();
        }

        if (pC.GetLanguage() == 1) {
            if (hours >= 13) {
                hours -= 12;
                if (hours > 9) {
                    timeText.text = hours + ":" + minutes + " pm";
                } else {
                    timeText.text = "0" + hours + ":" + minutes + " pm";
                }
            } else {
                if (hours > 9) {
                    timeText.text = hours + ":" + minutes + " am";
                } else {
                    timeText.text = "0" + hours + ":" + minutes + " am";
                }
            }
        }
    }

    private IEnumerator InitStartDate() {
        skyTimeController.SetDate(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);
        yield return new WaitForSeconds(1f);
        startDate = skyTimeController.GetDate();
        aktTime = skyTimeController.GetTimeOfDay();

        int hours = (int) aktTime.x;
        int minutes = (int) aktTime.y;

        SetTime(new Vector2(hours, minutes));
        SetCurrentDate();
    }

    //Reagiert auf das Event ChangeMinute des SkyTimeControllers. Wenn sich die Minuten ändern, wird hier die Zeit in der Anzeige angepasst.
    public void DoActionWhileStepUpdate(int minute) {
        if (setTimeWithRealTime) {
            float aktHour = System.DateTime.Now.Hour;
            float aktMinute = System.DateTime.Now.Minute * 1.66f / 100;
            float currentTime = aktHour + aktMinute;
            skyTimeController.SetTimeline(currentTime);

            aktTime = skyTimeController.GetTimeOfDay();
            int hours = (int) aktTime.x;
            int minutes = (int) aktTime.y;

            SetTime(new Vector2(hours, minutes));

            if (updateDate) {
                SetTimecontrollerDate();
            }
        }
    }

    private void Update() {
        //Zeitanzeige entspricht der echten Zeit
        if (showRealTime && timeMode == 99) {
            int hours = DateTime.Now.Hour;
            int minutes = DateTime.Now.Minute;
            SetTime(new Vector2(hours, minutes));
            SetCurrentDate();
        }

        #region Description

        /*Die angezeigte Zeit entspricht nicht der realen Zeit, im Pfad wird eine bestimmte Zeit verwendet, um ein 
        *bestimmtes Himmelsphänomen an einem bestimmten zeitpunkt darzustellen oder die Zeit wird durch die Rotation
        *der Himmelskugel berechnet und mit der aktuellen Zeit addidert (N02)
        */

    #endregion
        if (!setTimeWithRealTime) {
            //In N02 berechnet die aktuelle Himmelsrotation eine Zeit, zu dieser Zeit muss die aktuelle Zeit des TimeControllers addiert werden
            if (timeMode == 1) {
                int currentHour = int.Parse(System.DateTime.Now.Hour.ToString());
                int currentMinute = int.Parse(System.DateTime.Now.Minute.ToString());
                float time = rotationPivotForN03.localEulerAngles.y / 15.0f + currentHour +
                             (currentMinute / 100) * 1.666f;
                if (time >= 24) {
                    time -= 24;
                }

                int hours = (int) time;
                if (hours == 0) {
                    if (allowIncrease) {
                        allowIncrease = false;
                    }
                } else {
                    allowIncrease = true;
                }

                float tempMinutes = (Mathf.Repeat(rotationPivotForN03.localEulerAngles.y / 15.0f, 1.0f)) * 60;
                int minutes = (int) tempMinutes + currentMinute;
                if (minutes > 59) {
                    minutes -= 60;
                }

                SetTime(new Vector2(hours, minutes));
                SetTimecontrollerDate();
            }

            //Die angezeigte Zeit wird durch der Rotation der Himmelskugel berechnet. In N03 wird nicht die Zeit
            //animiert sondern die Himmelskugel, da die Bewegung dann wesentlich smoother aussieht.
            if (timeMode == 2) {
                int hours = (int) rotationPivotForN03.localEulerAngles.y / 15;
                float time = rotationPivotForN03.localEulerAngles.y / 15.0f;
                float tempMinutes = (Mathf.Repeat(rotationPivotForN03.localEulerAngles.y / 15.0f, 1.0f)) * 60;
                int minutes = (int) tempMinutes;
                minutes -= animatedDays * 4;
                
                if (time >= 24) {
                    skyTimeController.IncreaseDay();
                }

                if (minutes > 59) {
                    minutes -= 60;
                }

                if (minutes < 0) {
                    minutes = 0;
                }

                SetTime(new Vector2(hours, minutes));
            }

            //In S02 soll anstatt der Zeit das Wort Mittag/Noon angezeigt werden
            if (timeMode == 3) {
                Vector3 aktDate = skyTimeController.GetDate();
                timeText.text = "Mittag";
                dateText.text = aktDate.z + "." + aktDate.y + "." + aktDate.x;
                if (pC.GetLanguage() == 1) {
                    timeText.text = "Noon";
                    string month = translator.GetMonthName((int) aktDate.y);
                    dateText.text = month + " " + aktDate.z + ", " + aktDate.x;
                }
            }
        }
    }

    public void ContinueUpdateDate() {
        updateDate = true;
    }

    public void StopUpdateDate() {
        updateDate = false;
    }

    public void ShowRealSystemTime() {
        showRealTime = true;
        setTimeWithRealTime = false;
        timeMode = 99;
    }

    public void StopShowRealSystemTime() {
        showRealTime = false;
    }

    //Zeigt die reale Szenenzeit des SkytimeControllers an
    public void SetRealTime() {
        showRealTime = false;
        setTimeWithRealTime = true;
        timeMode = 0;
    }

    public void SetTimeModeN02() {
        setTimeWithRealTime = false;
        timeMode = 1;
    }

    public void SetTimeModeN03() {
        setTimeWithRealTime = false;
        timeMode = 2;
        timeText.text = "00:00";
    }
    
    public void SetTimeModeN07() {
        setTimeWithRealTime = false;
        timeMode = 0;
        
        int hours = DateTime.Now.Hour;
        int minutes = DateTime.Now.Minute;
        SetTime(new Vector2(hours, minutes));

        skyTimeController.SetTimeline(0);
    }

    public void SetTimeModeS02() {
        timeMode = 3;
        setTimeWithRealTime = false;
    }

    public void SetAnimatedDays(int day) {
        animatedDays = day;
    }
}