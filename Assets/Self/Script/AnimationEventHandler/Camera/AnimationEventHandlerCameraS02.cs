using UnityEngine;

public class AnimationEventHandlerCameraS02 : MonoBehaviour {
    
#region public Variables

    public ProcedureController pC;

    public Material schattenwerferKugel;

    public Transform skyCamCenter;
    public Transform mainCamera;
    public Transform sunDialPrefab;
    public Transform gridSVG;

    //animation Parameters
    public float alphaSchattenwerfer;

    public Vector3 gridPosition = new Vector3(0.021f, -0.008f, 0);
    public Vector3 mainCamXRot;
    public Vector3 skycamCenterPos;
    public Vector3 sunDialPos;

#endregion

    private bool allowMoveWithAnim = false;

    public void ResetScript() {
        skycamCenterPos = new Vector3(0, 0, 0);
        mainCamXRot = new Vector3(0, 0, 0);
        sunDialPos = new Vector3(0, 0, 0);
        allowMoveWithAnim = false;
    }

    void Update() {
        if (allowMoveWithAnim) {
            skyCamCenter.localPosition = skycamCenterPos;
            sunDialPrefab.localPosition = sunDialPos;
            mainCamera.localEulerAngles = mainCamXRot;
            gridSVG.localPosition = gridPosition;

            //Schattenwerferkugel highlighten
            Color swKugelColor = schattenwerferKugel.color;
            swKugelColor.a = alphaSchattenwerfer;
            schattenwerferKugel.color = swKugelColor;
        }
    }

    public void AllowMoveWithAnimS02AEH() {
        allowMoveWithAnim = true;
    }

    public void StopMoveWithAnimS02AEH() {
        allowMoveWithAnim = false;
    }

    //Wenn Kamera Animation beendet (Kamera in S02 nach oben rotiert), dann das Verändern der Variablen durch die Animation stoppen und zum nächsten Step springen
    public void FinishKameraAnimationAndStopMoveWithAnim() {
        allowMoveWithAnim = false;
        pC.JumpToNextStep();
    }
}