using System.Collections;
using UnityEngine;

public class RotateStarrSky : MonoBehaviour {
#region public Variables

    public SkyTimeController skyTimeController;
    public Transform m_starrySkyPivot;
    public Transform m_starrySkyDay;
    public Transform m_starrySkyHour;

#endregion

#region private Variables

    private bool updateStarrySky = false;

    private float day = 0;
    private float month = 0;
    private float utc = 0;
    private float latitude;
    private float longitude;
    private float hour = 0;
    private float minute = 0;
    private float aktTime = 0;

    private int[] daysPerMonthArray = new int[] {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};

    private Vector2 time;

#endregion

    private IEnumerator Start() {
        skyTimeController.SetDate(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);
        hour = System.DateTime.Now.Hour;
        minute = System.DateTime.Now.Minute * 1.66f / 100;
        aktTime = hour + minute;
        skyTimeController.SetTimeline(aktTime);

        yield return new WaitForSeconds(1f);
        CalculateRotation();
        updateStarrySky = true;
    }

    private void Update() {
        if (updateStarrySky) {
            CalculateRotation();
        }
    }

    private void CalculateRotation() {
        day = skyTimeController.GetDay();
        month = skyTimeController.GetMonth();
        time = skyTimeController.GetTimeOfDay();
        utc = skyTimeController.GetUtc();
        latitude = skyTimeController.GetLatitude();
        latitude = 90 - latitude;
        longitude = skyTimeController.GetLongitude();

        float tempDay = 0;
        float tempMinute = 0;
        
        if (day > 0) {
            if (month > 1) {
                for (int i = 0; i < month - 1; i++) {
                    tempDay += daysPerMonthArray[i];
                    if (i == 0) {
                        tempDay += day;
                    }
                }
            } else {
                tempDay = day;
            }
        }

        tempDay = tempDay * 0.986301f;

        tempMinute = (60 * time[0] + time[1]) + (60 * utc);
        tempMinute = tempMinute / 1440 * 360;

        tempDay = tempDay + 242.75f + longitude;
        m_starrySkyPivot.transform.localEulerAngles = new Vector3(latitude, 0, 0);
        m_starrySkyDay.transform.localEulerAngles = new Vector3(0, tempDay, 0);
        
        //Kleine Korrektur um die Kameraverzerrung auszugleichen
        float gwRotation = (m_starrySkyDay.transform.localEulerAngles.y + tempMinute + 16.5f) % 360;
        float correction = 0.0f;
        if (gwRotation >= 0 && gwRotation < 90) {
            correction = gwRotation * 5.0f / 90.0f * -1;
        } else if (gwRotation >= 90 && gwRotation < 180) {
            float temp = gwRotation - 90;
            correction = (5 - (temp * 5.0f / 90.0f)) * -1;
        } else if (gwRotation >= 180 && gwRotation < 270) {
            float temp = gwRotation - 180;
            correction = temp * 5.0f / 90.0f;
        } else if (gwRotation >= 270 && gwRotation < 360) {
            float temp = gwRotation - 270;
            correction = 5 - (temp * 5.0f / 90.0f);
        }

        tempMinute += correction;
        m_starrySkyHour.transform.localEulerAngles = new Vector3(0, tempMinute, 0);
    }

    public void AllowCalculateRotationContinous() {
        updateStarrySky = true;
    }

    public void CalculateRotationOneTime() {
        CalculateRotation();
    }

    public void StopRotation() {
        updateStarrySky = false;
    }
}