using UnityEngine;

public class MainMenuWrapper : MonoBehaviour {
    public GameObject mainMenu3DCarousel;

    public void HideMainMenu3DCarousel() {
        mainMenu3DCarousel.SetActive(false);
    }
}