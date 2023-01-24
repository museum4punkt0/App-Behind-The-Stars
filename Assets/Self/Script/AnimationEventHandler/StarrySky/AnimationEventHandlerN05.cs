using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandlerN05 : MonoBehaviour {
    public TrailController grosserWagenStern01;
    public TrailController grosserWagenStern02;
    
    //Tracen des GW stoppen, da Kreis komplett
    public void DidRotationForOneDayStopEmitting() {
        grosserWagenStern01.StopEmitting();
        grosserWagenStern02.StopEmitting();
    }
}
