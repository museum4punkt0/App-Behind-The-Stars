#region Description

/*In N07 Soll das Element des Nokturnals angetippt werden, das in der Dunkelheit beim Ablesen hilft. Auf dem Nokturnal
 * liegen verschiedene Collider, die den ELementen des Nokturnals zugeordnet sind. Wird ein falsches Element ( z.B. Griff,
 * Zeiger, ...) angetippt leuchtes das Element kurz rot auf und es wird ein text angezeigt, für was das angetippte
 * Element nützlich ist. Beim Tipp auf die Zacken/Zähne wird zum nächsten Step gesprungen
 */

#endregion

using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class HitNokturnalElement : MonoBehaviour {
#region public Variables

    public HelpController hC;
    public N07Helper n07H;
    public ProcedureController pC;

    public Camera nokturnalCam;

    public TextMeshProUGUI textContent;

    public SpriteRenderer datumsanzeige;
    public SpriteRenderer einstellring;
    public SpriteRenderer griff;
    public SpriteRenderer niete;
    public SpriteRenderer zaehne;
    public SpriteRenderer zeiger;

#endregion

#region private Variables

    private bool allowTouchNokturnalElement = false;
    
    private string stepId = "";

#endregion

    private void Start() {
        ProcedureController.changeEvent += DoActionWhileStepUpdate;
    }

    public void DoActionWhileStepUpdate(string _stepId) {
        stepId = _stepId;
        if (_stepId == "N07.03") {
            allowTouchNokturnalElement = true;
        } else {
            allowTouchNokturnalElement = false;
        }
    }

    void Update() {
        if (allowTouchNokturnalElement) {
            if (!hC.GetState()) {
                if (Input.GetMouseButtonDown(0)) {
                    RaycastHit hit;
                    Ray ray = nokturnalCam.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit)) {
                        if (hit.collider != null) {
                            if (hit.collider.gameObject == transform.gameObject) {
                                if (EventSystem.current.IsPointerOverGameObject()) {
                                } else {
                                    if (hit.collider.gameObject.name.Contains("Datumsanzeige") &&
                                        transform.gameObject.name.Contains("Datumsanzeige")) {
                                        Color color = datumsanzeige.color;
                                        color.a = 255;
                                        datumsanzeige.color = color;
                                        string temp = "Falsch! Das ist die Datumsanzeige.";
                                        if (pC.GetLanguage() == 1) {
                                            temp = "Wrong! This is the date display.";
                                        }

                                        StartCoroutine(DidWrongInput(datumsanzeige, temp));
                                        allowTouchNokturnalElement = false;
                                    }

                                    if (hit.collider.gameObject.name.Contains("Einstellring") &&
                                        transform.gameObject.name.Contains("Einstellring")) {
                                        Color color = einstellring.color;
                                        color.a = 255;
                                        einstellring.color = color;
                                        string temp = "Falsch! Das ist der Einstellring. Versuch es nochmal!";
                                        if (pC.GetLanguage() == 1) {
                                            temp = "Wrong! This is the adjusting ring";
                                        }

                                        StartCoroutine(DidWrongInput(einstellring, temp));
                                        allowTouchNokturnalElement = false;
                                    }

                                    if (hit.collider.gameObject.name.Contains("Griff") &&
                                        transform.gameObject.name.Contains("Griff")) {
                                        Color color = griff.color;
                                        color.a = 255;
                                        griff.color = color;
                                        string temp = "Nein, das ist der Griff, damit hält man das Nokturnal fest.";
                                        if (pC.GetLanguage() == 1) {
                                            temp = "Wrong! This is the handle.";
                                        }

                                        StartCoroutine(DidWrongInput(griff, temp));
                                        allowTouchNokturnalElement = false;
                                    }

                                    if (hit.collider.gameObject.name.Contains("Niete") &&
                                        transform.gameObject.name.Contains("Niete")) {
                                        Color color = niete.color;
                                        color.a = 255;
                                        niete.color = color;
                                        string temp = "Leider nicht! Damit visiert man den Polarstern an.";
                                        if (pC.GetLanguage() == 1) {
                                            temp = "Wrong!";
                                        }

                                        StartCoroutine(DidWrongInput(niete, temp));
                                        allowTouchNokturnalElement = false;
                                    }

                                    if (hit.collider.gameObject.name.Contains("Zeiger") &&
                                        transform.gameObject.name.Contains("Zeiger")) {
                                        Color color = zeiger.color;
                                        color.a = 255;
                                        zeiger.color = color;
                                        string temp = "Falsch! Das ist der Zeiger!";
                                        if (pC.GetLanguage() == 1) {
                                            temp = "Wrong! This is the pointer.";
                                        }

                                        StartCoroutine(DidWrongInput(zeiger, temp));
                                        allowTouchNokturnalElement = false;
                                    }

                                    if (hit.collider.gameObject.name.Contains("Zaehne1") &&
                                        transform.gameObject.name.Contains("Zaehne1")) {
                                        StartCoroutine(DidRightInput());
                                        allowTouchNokturnalElement = false;
                                    }

                                    if (hit.collider.gameObject.name.Contains("Zaehne2") &&
                                        transform.gameObject.name.Contains("Zaehne2")) {
                                        StartCoroutine(DidRightInput());
                                        allowTouchNokturnalElement = false;
                                    }

                                    if (hit.collider.gameObject.name.Contains("Zaehne3") &&
                                        transform.gameObject.name.Contains("Zaehne3")) {
                                        StartCoroutine(DidRightInput());
                                        allowTouchNokturnalElement = false;
                                    }

                                    if (hit.collider.gameObject.name.Contains("Zaehne4") &&
                                        transform.gameObject.name.Contains("Zaehne4")) {
                                        StartCoroutine(DidRightInput());
                                        allowTouchNokturnalElement = false;
                                    }

                                    if (hit.collider.gameObject.name.Contains("Zaehne5") &&
                                        transform.gameObject.name.Contains("Zaehne5")) {
                                        StartCoroutine(DidRightInput());
                                        allowTouchNokturnalElement = false;
                                    }

                                    if (hit.collider.gameObject.name.Contains("Zaehne6") &&
                                        transform.gameObject.name.Contains("Zaehne6")) {
                                        StartCoroutine(DidRightInput());
                                        allowTouchNokturnalElement = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    //Wenn ein falsches Element angetippt wurde, dann den Hinweis-Text einblenden, für was das Element eigentlich gut ist, bei mehrfacher falscher Eingabe die Aufgabe auflösen
    private IEnumerator DidWrongInput(SpriteRenderer aktSR, string textWarning) {
        int wrongInputCount = n07H.GetWrongInputCount();
        n07H.HitWrongWrapper();
        if (wrongInputCount < 2) {
            textContent.text = textWarning;
        } else {
            pC.JumpWithQuestInput(1);
            allowTouchNokturnalElement = false;
        }

        yield return new WaitForSeconds(0.5f);
        if (stepId == "N07.03") {
            allowTouchNokturnalElement = true;
        }

        Color color = aktSR.color;
        color.a = 0;
        aktSR.color = color;
    }

    private IEnumerator DidRightInput() {
        Color color = zaehne.color;
        color.a = 1.0f;
        zaehne.color = color;
        pC.JumpWithQuestInput(0);
        yield return new WaitForEndOfFrame();
    }
}