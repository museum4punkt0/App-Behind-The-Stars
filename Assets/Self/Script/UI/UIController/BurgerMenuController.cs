using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BurgerMenuController : MonoBehaviour {
#region public Variables

    //Scripts
    public HelpController hC;

    //Objects
    public Animator burgerMenuAnimator;
    public Animator impressumAnimator;
    public Animator philosophieAnimator;

    public GameObject languageButtonDeutsch;
    public GameObject languageButtonEnglish;

    public RectTransform arrowDropdownButton;

    public Transform logpanel;

#endregion

    private bool dropdownLanguageActive = false;

    //Animation starten, die das Burger-Menu einblendet
    public void ShowBurgerMenu() {
        burgerMenuAnimator.Play("ShowMenu", 0, 0);
        burgerMenuAnimator.SetInteger("MenuState", 1);
        hC.HideHelpPanel();
    }

    public void ShowBurgerHideImpressum() {
        impressumAnimator.Play("HideImpressum", 0, 0);
        impressumAnimator.SetInteger("impressumState", 2);
        burgerMenuAnimator.Play("ShowMenu", 0, 0);
        burgerMenuAnimator.SetInteger("MenuState", 1);
    }

    public void ShowBurgerHidePhilosophe() {
        philosophieAnimator.Play("HidePhilosophie", 0, 0);
        philosophieAnimator.SetInteger("philosophieState", 2);
        burgerMenuAnimator.Play("ShowMenu", 0, 0);
        burgerMenuAnimator.SetInteger("MenuState", 1);
    }

    public void HideBurgerMenuAndShowImpressum() {
        impressumAnimator.Play("ShowImpressum", 0, 0);
        impressumAnimator.SetInteger("impressumState", 1);
        burgerMenuAnimator.Play("HideMenu", 0, 0);
        burgerMenuAnimator.SetInteger("MenuState", 2);
    }

    public void HideBurgerMenuAndShowOhilosophie() {
        philosophieAnimator.Play("ShowPhilosophie", 0, 0);
        philosophieAnimator.SetInteger("philosophieState", 1);
        burgerMenuAnimator.Play("HideMenu", 0, 0);
        burgerMenuAnimator.SetInteger("MenuState", 2);
    }

    public void HideImpressum() {
        impressumAnimator.Play("HideImpressum", 0, 0);
        impressumAnimator.SetInteger("impressumState", 2);
        impressumAnimator.speed = 1;
    }

    public void HidePhilosophie() {
        philosophieAnimator.Play("HidePhilosophie", 0, 0);
        philosophieAnimator.SetInteger("philosophieState", 2);
    }

    public void HideBurgerMenu() {
        logpanel.localScale = new Vector3(0, 0, 0);
        burgerMenuAnimator.Play("HideMenu", 0, 0);
        burgerMenuAnimator.SetInteger("MenuState", 2);
    }

    public void ChangeDropdownLanguage() {
        dropdownLanguageActive = !dropdownLanguageActive;
        if (dropdownLanguageActive) {
            OpenDropdownLanguage();
        } else {
            CloseDropdownLanguage();
        }
    }

    public void OpenDropdownLanguage() {
        arrowDropdownButton.localEulerAngles = new Vector3(0, 0, 0);
        languageButtonDeutsch.SetActive(true);
        languageButtonEnglish.SetActive(true);
    }

    public void CloseDropdownLanguage() {
        dropdownLanguageActive = false;
        arrowDropdownButton.localEulerAngles = new Vector3(0, 0, 180);
        languageButtonDeutsch.SetActive(false);
        languageButtonEnglish.SetActive(false);
    }

    public void QuitGame() {
        Application.Quit();
    }
}