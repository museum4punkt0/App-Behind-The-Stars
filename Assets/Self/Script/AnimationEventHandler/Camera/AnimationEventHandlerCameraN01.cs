#region Description
/*Ein AnimationEventHandler reagiert und verarbeitet Informationen aus Animationen seines GameObjects.
 * In der Animation werden public Variablen verändert, die dann die Game-Objekte transformieren. Das direkte transformieren
 * der Game-Objekte in der Animation, blockiert die Transformationen unglücklicherweise häufig, um dieses Problem zu
 * umgehen, werden in diesem Projekt in der Regel ausschließlich public Variablen animiert, die an ein AnimationEventHandler
 * Skript gebunden sind.
 */
#endregion

using UnityEngine;

public class AnimationEventHandlerCameraN01 : MonoBehaviour {
    
#region public Variables

    public Camera mainCamera;
    public Camera skyCamera;

    public Transform nokturnalPivot;
    public Transform mainCameraTransf;

    //Animation parameters
    public Vector3 nokturnalScale = new Vector3(1, 1, 1);
    public Vector3 mainCamEuler = new Vector3(0, 0, 0);
    public Vector3 mainCamPos = new Vector3(0, 0, 0);
    public float skyCameraFOV = 40.0f;

#endregion

#region private Variables

    private bool moveWithAnim = false;
    private bool scaleNokturnalWithAnim = false;

    private float resolutionFactor = 0.0f;

#endregion

    void Start() {
        resolutionFactor = DeviceInfo.GetResolutionFactor();
    }
    
    private void Update() {
        //Nokturnal mit dem Parameter der Animation skalieren
        if (scaleNokturnalWithAnim) {
            nokturnalPivot.localScale = nokturnalScale;
        }

        //Kamera entsprechend der Parameter der Animation verändern
        if (moveWithAnim) {
            float newXRot = (resolutionFactor - 0.45f) * 0.5f / 0.3f;
            newXRot = mainCamEuler.x - newXRot;
            mainCameraTransf.localEulerAngles = new Vector3(newXRot, 0, 0);
            mainCameraTransf.localPosition = mainCamPos;

            float temp = ((resolutionFactor - 0.45f) * 18.0f / 0.3f) + skyCameraFOV;
            skyCamera.fieldOfView = temp;
            mainCamera.fieldOfView = temp;
        }
    }

    //Skalieren des Nokturnals mit der Animation erlauben
    public void AllowScaleNokturnalWithAnimAEHN01() {
        scaleNokturnalWithAnim = true;
    }

    //Verschieben des Nokturnals mit der Animation erlauben
    public void AllowMoveWithAnimAEHN01() {
        moveWithAnim = true;
    }

    //Das Verändern der Objekte durch die Animation stoppen, Objekte können dadurch in anderen Skripten wieder frei gehandelt werden
    public void StopMoveWithAnimAEHN01() {
        moveWithAnim = false;
        scaleNokturnalWithAnim = false;
    }
}