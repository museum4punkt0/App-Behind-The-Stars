using UnityEngine;

public class ImpressumController : MonoBehaviour {
    //Öffnet im Standardbrowser des Endgeräts die Impressumseite der SKD
    public void CallImpressumURL() {
        Application.OpenURL("https://www.skd.museum/impressum/#c29011");
    }
}