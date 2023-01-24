#region Description
/*In N05 soll die Position angetippt werden, die der große Wagen in 6 Stunden haben wird.
 * Auf der Himmelskugel liegt ein Mesh (inputn05cylinder), das im 90°Grad Winkel zur aktuellen Position steht.
 * Wenn dieses Mesh angetippt wird, dann ist die Zielposition korrekt, wenn daneben geklickt wird, dann wird
 * ein roter Kreis erzeugt (wrongInputPrefab).
 */
#endregion

using UnityEngine;
using UnityEngine.EventSystems;

public class HitCylinderInN05 : MonoBehaviour {
#region public Variables

    public ProcedureController pC;

    public Camera skyCamera;
    public GameObject rightInputPrefab;
    public GameObject wrongInputPrefab;
    public GameObject canvasParent;

#endregion

#region private Variables

    private int hitInputCylinder = 0;
    private bool stopWhileOverUI = false;

    private HelpController hC;

#endregion

    void Start() {
        hC = (HelpController) GameObject.Find("HelpUI").GetComponent(typeof(HelpController));
    }

    void Update() {
        if (!hC.GetState()) {
            if (Input.GetMouseButtonDown(0)) {
                if (EventSystem.current.IsPointerOverGameObject()
#if !UNITY_EDITOR
                || EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId)
#endif
                ) {
                    stopWhileOverUI = true;
                } else {
                    stopWhileOverUI = false;
                    hitInputCylinder = 1;
                    RaycastHit hit;
                    Ray ray = skyCamera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit)) {
                        if (hit.collider != null) {
                            if (hit.collider.gameObject == transform.gameObject) {
                                if (hit.collider.gameObject.name.Contains("inputn05cylinder")) {
                                    hitInputCylinder = 2;
                                    GameObject newRightInputPoint = Instantiate<GameObject>(rightInputPrefab);
                                    newRightInputPoint.transform.parent = canvasParent.transform;
                                    newRightInputPoint.transform.localScale = new Vector3(1.0625f, 0.86247f, 1);
                                    newRightInputPoint.transform.position =
                                        new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
                                    pC.JumpWithQuestInput(1);
                                }
                            }
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(0)) {
                if (!stopWhileOverUI) {
                    if (hitInputCylinder == 1) {
                        if (EventSystem.current.IsPointerOverGameObject()) {
                        } else {
                            GameObject newWrongInputPoint = Instantiate<GameObject>(wrongInputPrefab);
                            newWrongInputPoint.transform.parent = canvasParent.transform;
                            newWrongInputPoint.transform.localScale = new Vector3(1.0625f, 0.86247f, 1);
                            newWrongInputPoint.transform.position =
                                new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

                            pC.JumpWithQuestInput(0);
                        }
                    }
                }
            }
        }
    }
}