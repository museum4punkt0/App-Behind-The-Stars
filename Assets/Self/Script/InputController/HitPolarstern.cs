#region Description

/*In N01 soll der Polarstern angetippt werden. Auf der Himmelskugel befinden sich ein Collider an der Position
 * des Polarsters, wenn dieser berührt wird, gilt die Aufgabe als gelöst und der Polarstern wird gehighlighted.
 */

#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class HitPolarstern : MonoBehaviour {
#region public Variables

    public HelpController hC;

    public GameObject gameController;
    public GameObject wrongInputPrefab;
    public GameObject canvasParent;

    public Material materialPolarstern;

#endregion

#region private Variables

    private bool allowWrongInputInN02 = false;
    private bool allowTouchPolarstern = false;
    private bool stopWhileOverUI = false;

    private string stepId = "";

#endregion

    private void Start() {
        ProcedureController.changeEvent += DoActionWhileStepUpdate;
    }

    public void DoActionWhileStepUpdate(string _stepId) {
        stepId = _stepId;
        if (stepId == "N01.15" || stepId == "N01.16") {
            allowTouchPolarstern = true;
            allowWrongInputInN02 = false;
        } else if (stepId == "N02.14") {
            allowTouchPolarstern = true;
            allowWrongInputInN02 = true;
        } else {
            allowTouchPolarstern = false;
            allowWrongInputInN02 = false;
        }
    }

    void Update() {
        if (allowTouchPolarstern) {
            if (!hC.GetState()) {
                if (Input.GetMouseButtonDown(0)) {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit)) {
                        if (hit.collider != null) {
                            if (hit.collider.gameObject == transform.gameObject) {
                                if (EventSystem.current.IsPointerOverGameObject()
#if !UNITY_EDITOR
                                    || EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId)
#endif
                                ) {
                                    stopWhileOverUI = true;
                                } else {
                                    if (hit.collider.gameObject.name.Contains("Polarstern")) {
                                        StartCoroutine(ShowPolarstern());
                                        stopWhileOverUI = false;
                                        allowTouchPolarstern = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(1)) {
                if (!stopWhileOverUI && allowWrongInputInN02) {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit)) {
                        if (hit.collider != null) {
                        }
                    } else {
                        GameObject newWrongInputPoint = Instantiate<GameObject>(wrongInputPrefab);
                        newWrongInputPoint.transform.parent = canvasParent.transform;
                        newWrongInputPoint.transform.localScale = new Vector3(1.0625f, 0.86247f, 1);
                        newWrongInputPoint.transform.position =
                            new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
                    }
                }
            }
        }
    }

    //Wenn der Polarstern berührt wurde, den Polarstern highlighten und zum nächsten Schritt springen
    private IEnumerator ShowPolarstern() {
        Color color = materialPolarstern.color;
        do {
            color.a += 0.01f;
            materialPolarstern.color = color;
        } while (color.a < 1.0f);

        yield return new WaitForSeconds(1f);
        ProcedureController pC = (ProcedureController) gameController.GetComponent(typeof(ProcedureController));
        pC.JumpWithQuestInput(0);
    }
}