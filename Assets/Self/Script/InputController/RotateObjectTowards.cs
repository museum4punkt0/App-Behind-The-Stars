#region Description

/*Ermöglicht das Drehen des Zeiger des Nokturnals durch Touch-Input. In N01 soll mit Hilfe des Zeigers und
 * des Schüttkantensterns die Uhrzeit bestimmt werden. Wenn der Nutzer die Zielposition eingestellt hat,
 * wird zum nächsten Schritt gesprungen. Ist die aktuelle Uhrzeit zwischen 8 und 16 Uhr wird das Nokturnal transparent
 * dargestellt, da sonst der Griff des Nokturnals den Großen Wagen verdeckt.
 */

#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateObjectTowards : MonoBehaviour {
#region public Variables

    public GetLocationService gLS;

    public Camera nokturnalCam;

    public GameObject gameController;

    public Material nokturnalGold;
    public Material nokturnalGoldZeiger;

    public Transform m_starrySkyDay;
    public Transform m_starrySkyHour;
    public Transform camAngle;
    public Transform schuettkantenstern;
    public Transform polarStern;
    public Transform zentrumZeiger;
    public Transform spitzeZeiger;
    public Transform zeigerPivot;

#endregion

#region private Variables

    private bool makeNokturnalTransparent = false;
    private bool checkIsRunning = false;
    private bool stopWhileOverUI = false;
    private bool doRotate = false;
    private bool allowRotationPointer = false;
    private bool targetReached = false;
    private bool animateto = false;

    private float targetAngle;
    private float correctAngle = 0;
    private float day = 0;
    private float month = 0;
    private float longitude = 0;
    private float targetangle = 0;

    private Vector3 blockstarrySkyHour;
    private Vector3 blockstarrySkyDay;
    private Vector3 target;

#endregion

    private void Update() {
        if (makeNokturnalTransparent) {
            Color nokturnalGoldColor = nokturnalGold.color;
            Color nokturnalGoldZeigerColor = nokturnalGoldZeiger.color;
            if (nokturnalGoldColor.a > 0.35f) {
                nokturnalGoldColor.a -= 0.075f;
                nokturnalGold.color = nokturnalGoldColor;
                nokturnalGoldZeigerColor.a -= 0.075f;
            } else {
                nokturnalGoldColor.a = 0.35f;
                nokturnalGold.color = nokturnalGoldColor;
                nokturnalGoldZeigerColor.a = 0.35f;
                makeNokturnalTransparent = false;
            }
        }

        if (allowRotationPointer) {
            if (Input.GetMouseButtonDown(0)) {
                if (EventSystem.current.IsPointerOverGameObject() ||
                    EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId)) {
                    stopWhileOverUI = true;
                } else {
                    stopWhileOverUI = false;
                }
            }

            if (Input.GetMouseButton(0)) {
                if (!stopWhileOverUI) {
                    Vector2 mouseScreenPosition = nokturnalCam.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 direction = (mouseScreenPosition - (Vector2) zeigerPivot.position).normalized;
                    zeigerPivot.up = direction;

                    if (camAngle.localEulerAngles.y < 270 && camAngle.localEulerAngles.y > 90) {
                        float tempVar = (zeigerPivot.localEulerAngles.z + camAngle.localEulerAngles.y) % 360;
                        float tempVar2 = 90 - tempVar;
                        tempVar = 90 + tempVar2;
                        zeigerPivot.localEulerAngles = new Vector3(90, 0, tempVar);
                    } else {
                        float tempVar = (zeigerPivot.localEulerAngles.z - camAngle.localEulerAngles.y) % 360;
                        zeigerPivot.localEulerAngles = new Vector3(90, 0, tempVar);
                    }

                    m_starrySkyDay.localEulerAngles = blockstarrySkyDay;
                    m_starrySkyHour.localEulerAngles = blockstarrySkyHour;
                }
            }

            if (Input.GetMouseButtonUp(0)) {
                if (!stopWhileOverUI) {
                    Vector2 zentrumZeigerScreenPos = Camera.main.WorldToScreenPoint(zentrumZeiger.transform.position);
                    Vector2 spitzeZeigerScreenPos = Camera.main.WorldToScreenPoint(spitzeZeiger.transform.position);

                    float currentZeigerPos = CalculateAngle(zentrumZeigerScreenPos, spitzeZeigerScreenPos);
                    float correctur = 0;
                    if (targetangle < 60 || targetangle > 300) {
                        correctur = 4;
                    }

                    float leftangle = targetangle - 5 - correctur;

                    if (leftangle < 0) {
                        leftangle += 360;
                    }

                    float rightAngle = targetangle + 5 - correctur;
                    if (rightAngle > 360) {
                        rightAngle -= 360;
                    }

                    if (currentZeigerPos > leftangle && currentZeigerPos < rightAngle) {
                        targetReached = true;
                        if (!checkIsRunning) {
                            StartCoroutine(DestinationReached());
                            checkIsRunning = true;
                        }

                        allowRotationPointer = false;
                    }
                }
            }
        }
    }

    public void StopAllowRotationZeiger() {
        allowRotationPointer = false;
    }

    public void AllowRotationVariableWrapper() {
        StartCoroutine(AllowRotationVariable());
    }

    //Nachdem die Zielpositoin berechnet wurde, kann der Nutzer den Zeiger des Nokturnals drehen
    private IEnumerator AllowRotationVariable() {
        Vector2 polarScreenPos = Camera.main.WorldToScreenPoint(polarStern.transform.position);
        Vector2 schuettkantensternScreenPos = Camera.main.WorldToScreenPoint(schuettkantenstern.transform.position);

        targetangle = CalculateAngle(polarScreenPos, schuettkantensternScreenPos);
        //Ist die eingestellt Zeit zwischen 8 und 16 Uhr, dann wird das Nokturnal transparent dargestellt, da der Griff sonst den großen Wagen verdeckt.
        if (targetangle > 240 && targetangle < 300) {
            makeNokturnalTransparent = true;
        }

        blockstarrySkyDay = m_starrySkyDay.localEulerAngles;
        blockstarrySkyHour = m_starrySkyHour.localEulerAngles;
        yield return new WaitForSeconds(0.1f);
        checkIsRunning = false;
        allowRotationPointer = true;
    }

    private float CalculateAngle(Vector2 pointA, Vector2 pointB) {
        Vector2 helperpoint = new Vector2(pointB.x, pointA.y);

        float a = pointA.x - helperpoint.x;
        float b = pointB.y - helperpoint.y;
        float c = Mathf.Sqrt((a * a) + (b * b));
        float q = (a * a) / c;
        float p = c - q;
        float h = Mathf.Sqrt(p * q);
        float alpha = Mathf.Atan2(h, p) * Mathf.Rad2Deg;
        float beta = 90 - alpha;
        if (pointB.x > pointA.x && pointB.y > helperpoint.y) {
            beta += 0;
        }

        if (pointB.x < pointA.x && pointB.y > helperpoint.y) {
            beta = 180 - beta;
        }

        if (pointB.x < pointA.x && pointB.y < helperpoint.y) {
            beta = 180 + beta;
        }

        if (pointB.x > pointA.x && pointB.y < helperpoint.y) {
            beta = 360 - beta;
        }

        return beta;
    }

    private IEnumerator DestinationReached() {
        checkIsRunning = true;
        yield return new WaitForSeconds(0.25f);

        if (targetReached) {
            ProcedureController pC = (ProcedureController) gameController.GetComponent(typeof(ProcedureController));

            System.DateTime realPlaceTime = gLS.GetRealPlaceTime();
            System.TimeSpan difference = realPlaceTime.Subtract(System.DateTime.Now);

            if (difference.TotalMinutes > -45 && difference.TotalMinutes < 45) {
                pC.JumpWithQuestInput(0);
            } else {
                pC.JumpWithQuestInput(1);
            }
        }
    }
}