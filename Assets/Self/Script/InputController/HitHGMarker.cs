#region Description
/*Im Erkundungsmodus des Himmelsglobus können verschiedene Minipfade gestartet werden. Auf dem Himmelsglobus befinden
 * sich dafür lilafarbene Punkte. Jeder Punkt besitzt einen Collider. Wenn ein Collider per Touch berührt wurde,
 * wird entsprechend der Bezeichnung des Collider-Objects der Minipfad gestartet
 */
#endregion

using UnityEngine;

public class HitHGMarker : MonoBehaviour {
    public HimmelsglobusController hgController;
    public Camera skyCam;
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = skyCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider != null) {
                    if (hit.collider.gameObject == transform.gameObject) {
                        if (hit.collider.gameObject.name.Contains("Armillarsphaere")) {
                            ProcedureController pC = (ProcedureController)GameObject.Find("GameController").GetComponent(typeof(ProcedureController));
                            pC.JumpToMarkerPoint("H01AR.03");
                            hgController.HitedMarker(5, new Vector3(0, 2300, 0),26, 359);
                        }
                        else if (hit.collider.gameObject.name.Contains("Meridian")) {
                            ProcedureController pC = (ProcedureController)GameObject.Find("GameController").GetComponent(typeof(ProcedureController));
                            pC.JumpToMarkerPoint("H01AR.01");
                            hgController.HitedMarker(20, new Vector3(0, 365.0f, 0), 180f,359);
                        } 
                        else if (hit.collider.gameObject.name.Contains("Horizont")) {
                            hgController.HitedMarker(15, new Vector3(0, 1, 0), 0,270);
                        } 
                        else if (hit.collider.gameObject.name.Contains("Sternkugel")) {
                            hgController.HitedMarker(10, new Vector3(0, 26.8f, 0), 80,359);
                        }
                        else if (hit.collider.gameObject.name.Contains("Mond")) {
                            ProcedureController pC = (ProcedureController)GameObject.Find("GameController").GetComponent(typeof(ProcedureController));
                            pC.JumpToMarkerPoint("H01AR.02");
                            hgController.HitedMarker(8, new Vector3(0, -100f, 0), 320, 335);
                        } 
                        else if (hit.collider.gameObject.name.Contains("Erdglobus")) {
                            ProcedureController pC = (ProcedureController)GameObject.Find("GameController").GetComponent(typeof(ProcedureController));
                            pC.JumpToMarkerPoint("H01AR.04");
                            hgController.HitedMarker(9, new Vector3(0, -2750f, 0), 20f, 359);
                        } 
                        else if (hit.collider.gameObject.name.Contains("Sonnenuhr")) {
                            ProcedureController pC = (ProcedureController)GameObject.Find("GameController").GetComponent(typeof(ProcedureController));
                            pC.JumpToMarkerPoint("H01AR.05");
                            hgController.HitedMarker(13, new Vector3(0, -3700, 0), 86.5f, 330);
                        } 
                        else if (hit.collider.gameObject.name.Contains("Schublade")) {
                            hgController.HitedMarker(8, new Vector3(0, -3700, 0), 2, 359);
                        }
                    }
                }
            }
        }
    }
}
