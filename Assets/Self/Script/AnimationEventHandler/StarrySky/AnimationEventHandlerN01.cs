using UnityEngine;

public class AnimationEventHandlerN01 : MonoBehaviour {
    public N02Helper n02H;
    
    private string aktStepId = "";
    private void Start() {
        ProcedureController.changeEvent += DoActionWhileStepUpdate;
    }
    
    public void DoActionWhileStepUpdate(string stepId) {
        aktStepId = stepId;
    }
    
    //Wenn der Schuettkantenstern eine komplette Drehung gemacht hat, dann alle weiteren Sterne des GW tracen. 
    public void DidRotationForOneDay() {
        //Da die Animation in verschiedenen Pfaden verwendet wird, muss hier die Prüfung erfolgen ob der aktuelle Pfad N02 entspricht und nur dann das Tracing starten
        if (aktStepId.Contains("N02")) {
            n02H.StartTracingAllStars();
        }
    }
}
