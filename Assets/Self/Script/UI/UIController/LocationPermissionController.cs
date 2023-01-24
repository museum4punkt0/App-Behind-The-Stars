using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

using TMPro;

#if UNITY_ANDROID
//using DeadMosquito.AndroidGoodies;
#endif
using System.Linq;

public class LocationPermissionController : MonoBehaviour {
    public GetLocationService gLS;
    public SkyTimeController skyTimeController;
    public void AllowLocationPermission() {
#if UNITY_ANDROID
        //AGSettings.OpenSettingsScreen(AGSettings.ACTION_APPLICATION_SETTINGS);
#endif
        StartCoroutine(CheckAfterComeBack());
    }

    public void CloseDialog() {
        skyTimeController.SetLatitude(51.05004f);
        skyTimeController.SetLongitude(13.7383f);
        transform.localScale = new Vector3(0, 0, 0);
        gLS.CalculateRealTimeForDresden();
    }

    private IEnumerator TryInitLocationAgain() {
        yield return new WaitForSeconds(1f);
        gLS.InitLocationAgain();
    }

    private IEnumerator CheckAfterComeBack() {
        yield return new WaitForSeconds(1f);
        if (AndroidRuntimePermissions.CheckPermission("android.permission.ACCESS_FINE_LOCATION") == AndroidRuntimePermissions.Permission.ShouldAsk ||
           AndroidRuntimePermissions.CheckPermission("android.permission.ACCESS_FINE_LOCATION") == AndroidRuntimePermissions.Permission.Granted) {
            gLS.InitLocationAgain();
            transform.localScale = new Vector3(0, 0, 0);
        } else if (AndroidRuntimePermissions.CheckPermission("android.permission.ACCESS_FINE_LOCATION") == AndroidRuntimePermissions.Permission.Denied) {
            CloseDialog();
        }
    }
}
