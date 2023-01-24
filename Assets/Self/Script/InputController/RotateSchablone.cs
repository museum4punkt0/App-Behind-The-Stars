#region Description

/*Ermöglicht das Drehen der Schablone auf der Nokturnal Rückseite. In N08 soll mit Hilf dieser Scheibe ein Monat vom
 * Nutzer eingestellt werden. Zur Hilfe snappt die Schablone automatisch zur optimal Position, wenn der Nutzer
 * seine Eingabe beendet.
 */

#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RotateSchablone : MonoBehaviour {
#region public Variables

    public Button okButton;

    public Camera nokturnalCam;

    public Transform schablone;
    public Transform schabloneRotationHelper;

#endregion

#region private Variables

    private bool allowRotationPointer = false;
    private bool targetReached = false;
    private bool firstInput = true;
    private bool rotateSchabloneAutomatic = false;

    private float startSchabloneZ = 0.0f;
    private float lastAngle = 0.0f;

    private int month = 0;

#endregion

    private void Update() {
        if (rotateSchabloneAutomatic) {
            schablone.localEulerAngles += new Vector3(0, 0.1f, 0);
        }

        if (allowRotationPointer) {
            if (IsPointerOverUIObject()) {
                return;
            } else {
                if (Input.GetMouseButtonDown(0)) {
                    okButton.interactable = true;
                    firstInput = true;
                }

                if (Input.GetMouseButton(0)) {
                    Vector2 mouseScreenPosition = nokturnalCam.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 direction = (mouseScreenPosition - (Vector2) schabloneRotationHelper.position).normalized;
                    schabloneRotationHelper.up = direction;

                    float tempVar = 0.0f;
                    if (firstInput) {
                        startSchabloneZ = schabloneRotationHelper.localEulerAngles.z;
                        tempVar = startSchabloneZ - schabloneRotationHelper.localEulerAngles.z;
                        firstInput = false;
                    } else {
                        tempVar = lastAngle - schabloneRotationHelper.localEulerAngles.z;
                    }

                    schablone.localEulerAngles -= new Vector3(0, tempVar, 0);
                    lastAngle = schabloneRotationHelper.localEulerAngles.z;
                }

                if (Input.GetMouseButtonUp(0)) {
                    CheckDestination();
                }
            }
        }
    }

    private bool IsPointerOverUIObject() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    //Um die Einstellung eines Monats zu erleichtern, snappt die Schablone in die optimale Position, wenn der Nutzer seine Eingabe beendet hat.
    public void CheckDestination() {
        if (schablone.localEulerAngles.y < 15 || schablone.localEulerAngles.y >= 345) {
            month = 1;
            schablone.localEulerAngles = new Vector3(0, 0, 0);
        } else if (schablone.localEulerAngles.y >= 15 && schablone.localEulerAngles.y < 45) {
            month = 0;
            schablone.localEulerAngles = new Vector3(0, 30, 0);
        } else if (schablone.localEulerAngles.y >= 45 && schablone.localEulerAngles.y < 75) {
            month = 11;
            schablone.localEulerAngles = new Vector3(0, 60, 0);
        } else if (schablone.localEulerAngles.y >= 75 && schablone.localEulerAngles.y < 105) {
            month = 10;
            schablone.localEulerAngles = new Vector3(0, 90, 0);
        } else if (schablone.localEulerAngles.y >= 105 && schablone.localEulerAngles.y < 135) {
            month = 9;
            schablone.localEulerAngles = new Vector3(0, 120, 0);
        } else if (schablone.localEulerAngles.y >= 135 && schablone.localEulerAngles.y < 165) {
            month = 8;
            schablone.localEulerAngles = new Vector3(0, 150, 0);
        } else if (schablone.localEulerAngles.y >= 165 && schablone.localEulerAngles.y < 195) {
            month = 7;
            schablone.localEulerAngles = new Vector3(0, 180, 0);
        } else if (schablone.localEulerAngles.y >= 195 && schablone.localEulerAngles.y < 225) {
            month = 6;
            schablone.localEulerAngles = new Vector3(0, 210, 0);
        } else if (schablone.localEulerAngles.y >= 225 && schablone.localEulerAngles.y < 255) {
            month = 5;
            schablone.localEulerAngles = new Vector3(0, 240, 0);
        } else if (schablone.localEulerAngles.y >= 255 && schablone.localEulerAngles.y < 285) {
            month = 4;
            schablone.localEulerAngles = new Vector3(0, 270, 0);
        } else if (schablone.localEulerAngles.y >= 285 && schablone.localEulerAngles.y < 315) {
            month = 3;
            schablone.localEulerAngles = new Vector3(0, 300, 0);
        } else if (schablone.localEulerAngles.y >= 315 && schablone.localEulerAngles.y < 345) {
            month = 2;
            schablone.localEulerAngles = new Vector3(0, 330, 0);
        }
    }

    //Schablone rotiert anfangs automatisch, um den Nutzer klarer zu machen um welche Scheibe es sich handelt
    public void AllowAutomaticRotation() {
        rotateSchabloneAutomatic = true;
    }

    public void StopAutomaticRotation() {
        rotateSchabloneAutomatic = false;
    }

    public void DisableRotation() {
        allowRotationPointer = false;
    }

    //Rotation der Schablone per Touch Input erlauben
    public void AllowRotationVariableWrapper() {
        StartCoroutine(AllowRotationVariable());
    }

    private IEnumerator AllowRotationVariable() {
        yield return new WaitForSeconds(1f);
        allowRotationPointer = true;
        rotateSchabloneAutomatic = false;
    }

    //Gibt den ausgewählten Monat zurück
    public int GetMonth() {
        return month;
    }
}