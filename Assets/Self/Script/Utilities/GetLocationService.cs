using UnityEngine;
using System.Collections;

using TMPro;

public class GetLocationService : MonoBehaviour {
#region public Variables

    public SkyTimeController skyTimeController;

    public TextMeshProUGUI stats;

#endregion

#region private Variables

    private float latitude = 51.05004f;
    private float longitude = 13.7383f;

    private System.DateTime realPlaceTime;

#endregion

    public void InitLocationAgain() {
        StartCoroutine(InitLocation());
    }

    private IEnumerator InitLocation() {
        stats.text = "StartLocationService";
        // Prüfen ob Nutzer die Standortermittlung aktiviert hat
        if (!Input.location.isEnabledByUser) {
            stats.text = "Service not enabled";
            skyTimeController.SetLatitude(51.05004f);
            skyTimeController.SetLongitude(13.7383f);
            CalculateRealTimeForDresden();
            yield break;
        }

        Input.location.Start();

        // Maximal 20 Sekunden warten bis der Standort service initialisiert ist
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        //Wenn nach 20 Sekunden der Standortservice nicht aktualisiert ist, dann Dresden als Standort verwenden
        if (maxWait < 1) {
            stats.text = "Timed out";
            skyTimeController.SetLatitude(51.05004f);
            skyTimeController.SetLongitude(13.7383f);
            CalculateRealTimeForDresden();
            yield break;
        }

        // Wenn der Standort des Gerätes nicht bestimmt werden kann, dann Dresden als Standort verwenden
        if (Input.location.status == LocationServiceStatus.Failed) {
            stats.text = "Unable to determine device location";
            skyTimeController.SetLatitude(51.05004f);
            skyTimeController.SetLongitude(13.7383f);
            CalculateRealTimeForDresden();
            yield break;
        } else {
            Debug.Log("Location: " + Input.location.lastData.latitude + " | " + Input.location.lastData.longitude +
                      " | " + Input.location.lastData.altitude + " | " + Input.location.lastData.horizontalAccuracy +
                      " | " + Input.location.lastData.timestamp);
            stats.text = "Location: " + Input.location.lastData.latitude.ToString() + " " +
                         Input.location.lastData.longitude.ToString() + " " +
                         Input.location.lastData.altitude.ToString() + " " +
                         Input.location.lastData.horizontalAccuracy.ToString() + " " +
                         Input.location.lastData.timestamp.ToString();

            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            skyTimeController.SetLatitude(latitude);
            skyTimeController.SetLongitude(longitude);

            int realMinutes = (int) longitude * 4;
            float longitudedecimal = longitude % 1;
            float secondsToMinute = longitudedecimal * 4;
            realMinutes += (int) secondsToMinute;
            float realseconds = (secondsToMinute % 1) * 60f;
            realPlaceTime = System.DateTime.UtcNow;
            System.TimeSpan duration = new System.TimeSpan(0, realMinutes, (int) realseconds);
            realPlaceTime = realPlaceTime.Add(duration);
            System.TimeSpan difference = realPlaceTime.Subtract(System.DateTime.Now);
        }

        // Standortabfrage nur einmal notwendig, Standortservice nach Erfolg direkt wieder stoppen
        Input.location.Stop();
    }
        
    //Wahre Uhrzeit für Dresden berechnen, da Nutzer Standort nicht freigegeben hat
    public void CalculateRealTimeForDresden() {
        latitude = 51;
        longitude = 13.7f;
        skyTimeController.SetLatitude(latitude);
        skyTimeController.SetLongitude(longitude);
        int realMinutes = (int) longitude * 4;
        float longitudedecimal = longitude % 1;
        float secondsToMinute = longitudedecimal * 4;
        realMinutes += (int) secondsToMinute;
        float realseconds = (secondsToMinute % 1) * 60f;
        realPlaceTime = System.DateTime.UtcNow;
        System.TimeSpan duration = new System.TimeSpan(0, realMinutes, (int) realseconds);
        realPlaceTime = realPlaceTime.Add(duration);
        System.TimeSpan difference = realPlaceTime.Subtract(System.DateTime.Now);
    }
    
    //Wahre Uhrzeit unabhängig von der Zeitzone berechnen, für den aktuellen Standpunkt des Nutzers
    public System.DateTime GetRealPlaceTime() {
        latitude = skyTimeController.GetLatitude();
        longitude = skyTimeController.GetLongitude();

        int realMinutes = (int) longitude * 4;
        float longitudedecimal = longitude % 1;
        float secondsToMinute = longitudedecimal * 4;
        realMinutes += (int) secondsToMinute;
        float realseconds = (secondsToMinute % 1) * 60f;
        realPlaceTime = System.DateTime.UtcNow;
        System.TimeSpan duration = new System.TimeSpan(0, realMinutes, (int) realseconds);
        realPlaceTime = realPlaceTime.Add(duration);
        System.TimeSpan difference = realPlaceTime.Subtract(System.DateTime.Now);

        return realPlaceTime;
    }
}