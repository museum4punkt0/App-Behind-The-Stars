#region Description

/*In N01 soll der Große Wagen angetippt werden. Auf der Himmelskugel befinden sich Collider an der Position
 * des Großen Wagens, wenn einer davon berührt wird, gilt die Aufgabe als gelöst und die Sterne des Großen Wagens
 * werden gehighlighted (ShowGrosserWagen)
 */

#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class HitGrosserWagen : MonoBehaviour {
#region public Variables

    public HelpController hC;
    public ToolTipController tTC;

    public GameObject gameController;
    
    public Material importantStars;
    public Material importantStarsSchuettkantenStern;

#endregion

#region private Variables

    private bool allowTouchGrosserWagen = false;
    
    private string stepId = "";

#endregion

    private void Start() {
        ProcedureController.changeEvent += DoActionWhileStepUpdate;
    }

    public void DoActionWhileStepUpdate(string _stepId) {
        stepId = _stepId;
        if (_stepId == "N01.03" || _stepId == "N01.04") {
            allowTouchGrosserWagen = true;
        } else {
            allowTouchGrosserWagen = false;
        }
    }

    void Update() {
        if (allowTouchGrosserWagen) {
            if (!hC.GetState() && !tTC.GetToolTipState()) {
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
                                } else {
                                    if (hit.collider.gameObject.name.Contains("GrosserWagen")) {
                                        ShowGrosserWagenWrapper();
                                        allowTouchGrosserWagen = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    //Wenn der große Wagen berührt wurde, dann das aufleuchten des GW stoppen und zum nächsten Schritt springen
    public void ShowGrosserWagenWrapper() {
        N01Helper n01H = (N01Helper) GameObject.Find("GameController").GetComponent(typeof(N01Helper));
        n01H.StopFlashHighlightGrosserWagen();

        ProcedureController pC = (ProcedureController) gameController.GetComponent(typeof(ProcedureController));
        pC.JumpWithQuestInput(0);
    }

    private IEnumerator ShowGrosserWagen() {
        Color color = importantStars.color;
        do {
            color.a += 0.01f;
            importantStars.color = color;
            importantStarsSchuettkantenStern.color = color;
        } while (color.a < 1.0f);

        yield return new WaitForSeconds(1f);
        ProcedureController pC = (ProcedureController) gameController.GetComponent(typeof(ProcedureController));
        pC.JumpToNextStep();
    }
}