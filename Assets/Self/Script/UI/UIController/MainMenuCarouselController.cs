using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCarouselController : MonoBehaviour {
#region public Variables

    public ProcedureController pC;

    public Animator mainMenu2dLoopAnimator;

    public Button startButtonMainMenu;

    public GameObject schlossHimmelsglobus;
    public GameObject schlossSonnenuhr;
    public GameObject mainmenu3dHideObject;

    public Transform loopMenu;

#endregion

    public void SetLockOrUnlockImage(bool fileExists) {
        //Wenn ein Spielstand existiert, je nach Spielstand die Instrumente aktivieren oder sperren
        //wenn kein Spielstand existiert dann alle Instrumente sperren
        if (fileExists) {
            var filePath = Path.Combine(Application.persistentDataPath, "spielstand.txt");
            var loadData = File.ReadAllText(filePath);

            if (loadData == "1") {
                schlossHimmelsglobus.SetActive(true);
                schlossSonnenuhr.SetActive(false);
            } else if (loadData == "2") {
                schlossHimmelsglobus.SetActive(false);
                schlossSonnenuhr.SetActive(false);
            }
        } else {
            schlossHimmelsglobus.SetActive(true);
            schlossSonnenuhr.SetActive(true);
        }
    }

#region Animation Handler

    public void Show2dLoopMenu() {
        loopMenu.localScale = new Vector3(1, 1, 1);
        mainMenu2dLoopAnimator.enabled = true;
        mainMenu2dLoopAnimator.Play("Show2DlLoopMenu", 0, 0);
        mainMenu2dLoopAnimator.SetInteger("AnimationState", 1);
        startButtonMainMenu.enabled = true;
    }

    public void ShowPfadAuswahl() {
        loopMenu.localScale = new Vector3(1, 1, 1);
        mainMenu2dLoopAnimator.enabled = true;
        mainMenu2dLoopAnimator.Play("LoopCarouselShowMarker", 0, 0);
        mainMenu2dLoopAnimator.SetInteger("AnimationState", 2);
    }

    public void SonnenuhrUnlocked() {
        pC.StartCheckMarkerPathAnimation(0);
    }

    public void HimmelsglobusUnlocked() {
        pC.StartCheckMarkerPathAnimation(7);
    }

    public void UnlockSonnenuhr() {
        schlossSonnenuhr.SetActive(false);
    }

    public void UnlockHimmelsglobus() {
        schlossHimmelsglobus.SetActive(false);
    }

    public void DeactivateHideObject() {
        mainmenu3dHideObject.SetActive(false);
    }

#endregion
}