using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerAnimationEventHandler : MonoBehaviour {
    //Die Animation (Haken setzen nach Abschluss) wird nur einmal abgespielt (nach erstmaligen absolvieren des Pfades)
    //wenn die Animation also einmal abgespielt wurde, dann direkt die Animator-Komponente löschen
    public void DisableAnimator() {
        Destroy(transform.GetComponent<Animator>());
    }
}
