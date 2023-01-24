using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandlerZ01 : MonoBehaviour {
    public Z01Helper z01H;

    //Tracing des GW stoppen, da eine komplette Drehung erfolgt
    public void StopTracingGWInZ01() {
        z01H.StopTracingGW();
    }
}