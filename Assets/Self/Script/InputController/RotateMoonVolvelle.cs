#region Description

/*In S05 soll die Mond-Volvelle der Sonnenuhr auf eine bestimmte Position gedreht werden (Halbmond).
 * In einem bestimmten Bereich snappt die Mond-Volvelle zur Zielposition um die Aufgabe zu erleichtern.
 * Bei mehrfacher falscher Eingabe (wrongInputs) rotiert die Mondvolvelle automatisch zur Zielposition.
 */

#endregion

using UnityEngine;
using UnityEngine.EventSystems;

public class RotateMoonVolvelle : MonoBehaviour {
#region public Variables

    public Camera nokturnalCam;

    public GameObject gameController;

    public Transform moonVolvellePivot;

#endregion

#region private Variables

    private bool allowRotationPointer = false;
    private bool stopWhileOverUI = false;

    private int wrongInputs = 0;

#endregion

    private void Update() {
        if (allowRotationPointer) {
            if (Input.GetMouseButtonDown(0)) {
                if (EventSystem.current.IsPointerOverGameObject()
#if !UNITY_EDITOR
                                    || EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId)
#endif
                ) {
                    stopWhileOverUI = true;
                } else {
                    stopWhileOverUI = false;
                }
            }

            if (Input.GetMouseButton(0)) {
                if (!stopWhileOverUI) {
                    Vector3 mouse = Input.mousePosition;
                    Vector3 mouseWorld =
                        nokturnalCam.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, moonVolvellePivot.position.y));
                    Vector3 forward = mouseWorld - moonVolvellePivot.transform.position;
                    forward.y = 0;
                    moonVolvellePivot.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
                }
            }

            if (Input.GetMouseButtonUp(0)) {
                if (!stopWhileOverUI) {
                    if (moonVolvellePivot.transform.localEulerAngles.y > 80 &&
                        moonVolvellePivot.transform.localEulerAngles.y < 120) {
                        ProcedureController pC =
                            (ProcedureController) gameController.GetComponent(typeof(ProcedureController));
                        pC.JumpWithQuestInput(0);
                        moonVolvellePivot.transform.localEulerAngles = new Vector3(0, 100, 0);
                        allowRotationPointer = false;
                    } else {
                        wrongInputs++;
                        if (wrongInputs == 1) {
                            ProcedureController pC =
                                (ProcedureController) gameController.GetComponent(typeof(ProcedureController));
                            pC.JumpWithQuestInput(1);
                        }

                        if (wrongInputs >= 3) {
                            moonVolvellePivot.transform.localEulerAngles = new Vector3(0, 100, 0);
                            ProcedureController pC =
                                (ProcedureController) gameController.GetComponent(typeof(ProcedureController));
                            pC.JumpWithQuestInput(1);
                            allowRotationPointer = false;
                        }
                    }
                }
            }
        }
    }

    public void AllowRotation() {
        allowRotationPointer = true;
    }

    public void StopRotation() {
        allowRotationPointer = false;
    }
}