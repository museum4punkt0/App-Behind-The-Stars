using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkyCamHelperForZ02 : MonoBehaviour {
#region public Variables

    public ProcedureController pC;
    public SkyProfileEventHandler sPEH;
    public Z02Helper z02H;

    public Button okButton;

    public Camera mainCam;
    public Camera skyCamera;

    public GameObject sunDialParent;

    public Material earthMat;
    public Material fadenToPolarstarLine;
    public Material ground;
    public Material horizontMat;
    public Material importantPolarStar;
    public Material matStars;
    public Material suHolz;
    public Material suPlatte;
    public Material suVolvelle;
    public Material suFaden;
    public Material suKugel;
    public Material suMetall;
    public Material outOfSphereSkymat;

    public TextMeshPro west;

    public Transform cylinderZ02;
    public Transform earth;
    public Transform earthFigureTransf;
    public Transform skyCameraXPiovt;
    public Transform sundialObject;
    public Transform mainCamera;

    public SpriteRenderer earthFigureMat;
    public SpriteRenderer gridSVG;

    //Variables
    public float earthAlpha = 0.0f;
    public float earthFigureAlpha = 0.0f;
    public float fadenToPolarstarLineAlpha = 1.0f;
    public float groundAlpha = 255.0f;
    public float horizontAlpha = 0.0f;
    public float importantPolarStarAlpha = 1.0f;
    public float gridAlpha = 1.0f;
    public float f_suHolz;
    public float f_suPlatte;
    public float f_suVolvelle;
    public float f_suFaden;
    public float f_suKugel;
    public float f_suMetall;
    public float matStarAlpha = 1.0f;
    public float skyCameraFOV = 40.0f;
    public float westAlpha = 1.0f;
    public float skyOutOfSphereBackgroundColorAlpha = 0.0f;

    public Vector3 earthPos;
    public Vector3 sundialScale;
    public Vector3 skyCameraXPivotEuler = new Vector3(0, 0, 0);
    public Vector3 earthFigurEuler = new Vector3(0, -90, 39);
    public Vector3 mainCamAngles;
    public Vector3 cylinderpos = new Vector3(0, 0, 0);
    public Vector3 cylindersize = new Vector3(0.07509971f, 54.78939f, 0.07509971f);

#endregion

    private bool changeVariablesWithAnim = false;
    private float fovFactor = 0.0f;

    private void Update() {
        if (changeVariablesWithAnim) {
        #region Camera Changes

            float temp = skyCameraFOV;
            skyCameraXPiovt.localEulerAngles = skyCameraXPivotEuler;

            skyCamera.fieldOfView = temp;
            mainCam.fieldOfView = temp;
            mainCamera.localEulerAngles = mainCamAngles;

        #endregion

        #region MaterialChanges

            Color gridSVGColor = gridSVG.color;
            gridSVGColor.a = gridAlpha;
            gridSVG.color = gridSVGColor;

            Color horizontMskyTimeControllerolor = horizontMat.color;
            horizontMskyTimeControllerolor.a = horizontAlpha;
            horizontMat.color = horizontMskyTimeControllerolor;

            Color matStarsColor = matStars.color;
            matStarsColor.a = matStarAlpha;
            matStars.color = matStarsColor;

            Color importantPolarStarColor = importantPolarStar.color;
            importantPolarStarColor.a = importantPolarStarAlpha;
            importantPolarStar.color = importantPolarStarColor;

            Color fadenToPolarstarLineColor = fadenToPolarstarLine.color;
            fadenToPolarstarLineColor.a = fadenToPolarstarLineAlpha;
            fadenToPolarstarLine.color = fadenToPolarstarLineColor;

            Color groundColor = ground.color;
            groundColor.a = groundAlpha;
            ground.color = groundColor;

            Color earthMskyTimeControllerolor = earthMat.color;
            earthMskyTimeControllerolor.a = earthAlpha;
            earthMat.color = earthMskyTimeControllerolor;

            Color earthFigureMskyTimeControllerolor = earthFigureMat.color;
            earthFigureMskyTimeControllerolor.a = earthFigureAlpha;
            earthFigureMat.color = earthFigureMskyTimeControllerolor;

            Color suHolzColor = suHolz.color;
            suHolzColor.a = f_suHolz;
            suHolz.color = suHolzColor;

            Color suPlatteColor = suPlatte.color;
            suPlatteColor.a = f_suPlatte;
            suPlatte.color = suPlatteColor;

            Color suVolvelleColor = suVolvelle.color;
            suVolvelleColor.a = f_suVolvelle;
            suVolvelle.color = suVolvelleColor;

            Color suFadenColor = suFaden.color;
            suFadenColor.a = f_suFaden;
            suFaden.color = suFadenColor;

            Color suKugelColor = suKugel.color;
            suKugelColor.a = f_suKugel;
            suKugel.color = suKugelColor;

            Color suMetallColor = suMetall.color;
            suMetallColor.a = f_suMetall;
            suMetall.color = suMetallColor;
            outOfSphereSkymat.color = new Color(1.0f, 1.0f, 1.0f, skyOutOfSphereBackgroundColorAlpha);

        #endregion

        #region Other Changes

            cylinderZ02.localPosition = cylinderpos;
            cylinderZ02.localScale = cylindersize;
            west.faceColor = new Color(255, 255, 255, westAlpha);
            sundialObject.localScale = sundialScale;

            earth.position = earthPos;
            earthFigureTransf.eulerAngles = earthFigurEuler;

        #endregion
        }
    }

#region Animation Handling-Functions

    public void AllowChangingVariablesWithAnim() {
        sPEH.LetSetStarmaterialManually();
        float resolutionFactor = DeviceInfo.GetResolutionFactor();
        fovFactor = (0.625f - resolutionFactor) * 100.0f;
        changeVariablesWithAnim = true;
    }

    public void StopChangingVariablesWithAnim() {
        changeVariablesWithAnim = false;
    }

    //den Faden der Sonnenuhr zum Polarstern verlängern
    public void StartDrawLineToPolarstarInZ02() {
        z02H.StartDrawLineToPolarstar();
    }

    //erde einblenden
    public void ShowEarth() {
        earth.localScale = new Vector3(3.5f, 3.5f, 3.5f);
    }

    //Sonnenuhr deaktivieren, weil Kamera aus Himmelskugel gezoomt ist
    public void HideSunDial() {
        sunDialParent.SetActive(false);
    }

    public void FinishStep() {
        pC.JumpToNextStep();
    }

    //Den Weitere-Button erst an einer bestimmten Stelle der Animation wieder freigeben, um die Animation nicht überspringen zu können
    public void ShowOkButton() {
        okButton.interactable = true;
    }

#endregion
}