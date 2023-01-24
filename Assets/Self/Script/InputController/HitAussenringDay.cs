#region Description
/*In N08 sollen verschiedene Planetensymbole auf dem Nokturnal angetippt werden. Jedes Symbol besitzt einen Collider.
 * Wenn Venus und Mars angetippt wurden ist die Aufgabe gelöst. Wenn ein anderes Smybol angetippt wird leuchtet es kurz
 * rot auf. Nach 0.5 Sekunden wird das highlight wieder ausgeblendet (TurnRendererOff).
 */
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAussenringDay : MonoBehaviour {
    
    #region public Variables

    public Camera nokturnalCam;

    public GameObject gameController;

    public SpriteRenderer ausenringHighlightSonne;
    public SpriteRenderer ausenringHighlightMond;
    public SpriteRenderer ausenringHighlightMars;
    public SpriteRenderer ausenringHighlightMerkur;
    public SpriteRenderer ausenringHighlightJupiter;
    public SpriteRenderer ausenringHighlightVenus;
    public SpriteRenderer ausenringHighlightSaturn;

#endregion

    private string stepId = "";
    private bool allowTouchNokturnalElement = false;

    private void Start() {
        ProcedureController.changeEvent += DoActionWhileStepUpdate;
    }

    public void DoActionWhileStepUpdate(string _stepId) {
        stepId = _stepId;
        if (_stepId == "N09.15") {
            Color red = new Color(255, 0, 0, 0);
            ausenringHighlightSonne.color = red;
            ausenringHighlightMond.color = red;
            allowTouchNokturnalElement = true;
        } else {
            allowTouchNokturnalElement = false;
        }
    }

    void Update() {
        if (allowTouchNokturnalElement) {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                Ray ray = nokturnalCam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit)) {
                    if (hit.collider != null) {
                        if (hit.collider.gameObject == transform.gameObject) {
                            if (hit.collider.gameObject.name.Contains("Sonne") &&
                                transform.gameObject.name.Contains("Sonne")) {
                                Color color = ausenringHighlightSonne.color;
                                color.a = 1;
                                ausenringHighlightSonne.color = color;
                                StartCoroutine(TurnRendererOff(ausenringHighlightSonne));
                            }

                            if (hit.collider.gameObject.name.Contains("Mond") &&
                                transform.gameObject.name.Contains("Mond")) {
                                Color color = ausenringHighlightMond.color;
                                color.a = 1;
                                ausenringHighlightMond.color = color;
                                StartCoroutine(TurnRendererOff(ausenringHighlightMond));
                            }

                            if (hit.collider.gameObject.name.Contains("Mars") &&
                                transform.gameObject.name.Contains("Mars")) {
                                Color color = ausenringHighlightMars.color;
                                color.a = 1;
                                ausenringHighlightMars.color = color;
                                CheckResult("Mars");
                            }

                            if (hit.collider.gameObject.name.Contains("Merkur") &&
                                transform.gameObject.name.Contains("Merkur")) {
                                Color color = ausenringHighlightMerkur.color;
                                color.a = 1;
                                ausenringHighlightMerkur.color = color;
                                StartCoroutine(TurnRendererOff(ausenringHighlightMerkur));
                            }

                            if (hit.collider.gameObject.name.Contains("Jupiter") &&
                                transform.gameObject.name.Contains("Jupiter")) {
                                Color color = ausenringHighlightJupiter.color;
                                color.a = 1;
                                ausenringHighlightJupiter.color = color;
                                StartCoroutine(TurnRendererOff(ausenringHighlightJupiter));
                            }

                            if (hit.collider.gameObject.name.Contains("Venus") &&
                                transform.gameObject.name.Contains("Venus")) {
                                Color color = ausenringHighlightVenus.color;
                                color.a = 1;
                                ausenringHighlightVenus.color = color;
                                CheckResult("Venus");
                            }

                            if (hit.collider.gameObject.name.Contains("Saturn") &&
                                transform.gameObject.name.Contains("Saturn")) {
                                Color color = ausenringHighlightSaturn.color;
                                color.a = 1;
                                ausenringHighlightSaturn.color = color;
                                StartCoroutine(TurnRendererOff(ausenringHighlightSaturn));
                            }
                        }
                    }
                }
            }
        }
    }

    private IEnumerator TurnRendererOff(SpriteRenderer thisRenderer) {
        yield return new WaitForSeconds(0.5f);
        Color color = thisRenderer.color;
        color.a = 0;
        thisRenderer.color = color;
    }

    public void CheckResult(string planet) {
        N08Helper n08H = (N08Helper) gameController.GetComponent(typeof(N08Helper));
        n08H.CheckResultN0915(planet);
    }
}