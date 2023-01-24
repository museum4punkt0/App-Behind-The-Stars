using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AnimationEventHandlerCameraN05 : MonoBehaviour {
    
#region public Variables

    public N05Helper n05H;
    public ProcedureController pC;

    public Camera skyCamera;
    private LensDistortion lD;

    public Material earthZ01Mat;
    public Material breitengradeGitternetzMat;
    public Material outOfSphereSkymat;
    public Material planeGroundMat;
    public Material traceN05Mat;

    public SpriteRenderer gridGroundSpriteRenderer;

    public Transform earthTransf;
    public Transform skyCameraCenterXPivotTransform;
    public Transform skyCameraTransf;
    public Transform starrySkyCompassPivot;

    public TextMeshPro north, east, south, west;

    public Volume volume;

    //Animation Parameters
    public float alphaFont = 255;
    public float camFieldOfView = 40;
    public float earthAlpha = 0.0f;
    public float gitterNetzAlpha = 0.0f;
    public float gridAlpha = 1.0f;
    public float ldIntensity = 0.4f;
    public float planeGroundAlpha = 1.0f;
    public float posY = 3.0f;
    public float posZ = 0.0f;
    public float scaleVal = 1.04f;
    public float skyOutOfSphereBackgroundColorAlpha = 0.0f;
    public float skyCameraCenterRotX = 0.0f;
    public float traceMatNo05alpha = 0.0f;
    public float xIdensity = 1.0f;
    public float xRot = 0.0f;

    public int earthRenderQueue = 3000;

    public Vector3 earthPos = new Vector3(0, 0, 0);

#endregion

#region private Variables

    private Color earthZ01Color;
    private Color breitengradeGitternetzColor;
    private Color groundColor;
    private Color gridGroundColor;
    private Color traceN05Color;

    private bool moveWithAnim = false;

    private float resolutionFactor = 0.0f;
    private float screenfactor = 0.0f;
    private float targetXRotation = 90.0f;

#endregion

    private void Start() {
        LensDistortion tmp;

        if (volume.profile.TryGet(out tmp)) {
            lD = tmp;
        }

        //Farben mit den Ausgangswerten der Materials initialisieren, im folgenden werden nur die Alpha-Werte verändert
        earthZ01Color = earthZ01Mat.color;
        breitengradeGitternetzColor = breitengradeGitternetzMat.color;
        traceN05Color = traceN05Mat.color;
        groundColor = planeGroundMat.color;
        gridGroundColor = gridGroundSpriteRenderer.color;

        resolutionFactor = DeviceInfo.GetResolutionFactor();
        screenfactor = DeviceInfo.GetScreenfactor();
    }

    private void Update() {
        if (moveWithAnim) {
        #region Camera Changes
            //für manche Kameraanimationen ist es notwendig die Parameter der Auflösung entsprechend anzupassen (resolutionFactor)
            float fovStrengthFactor = camFieldOfView / 35.0f;
            float newFOV = camFieldOfView + ((((resolutionFactor - 0.45f) * 15.0f) / 0.3f) * fovStrengthFactor);
            skyCamera.fieldOfView = newFOV;

            float newPosY = posY + (((screenfactor - 1.3333333f) * 290.0f) / 0.89f);
            float newPosZ = posZ - (((screenfactor - 1.3333333f) * 200.0f) / 0.89f);
            skyCameraTransf.localPosition = new Vector3(0, newPosY, newPosZ);

            float xRotStrengthFactor = 1.0f - ((xRot - 25.0f) / 74.0f);
            float newXRot = xRot + ((((screenfactor - 1.3333333f) * 10.0f) / 0.89f) * xRotStrengthFactor);
            skyCameraTransf.localEulerAngles = new Vector3(newXRot, 0, 0);

            lD.scale.Override(scaleVal);
            lD.intensity.Override(ldIntensity);
            lD.xMultiplier.Override(xIdensity);

            float xRotStrengthFactorForXPivot = skyCameraCenterRotX / 69.0f;
            float newXRotForXpivot = skyCameraCenterRotX +
                                     (((screenfactor - 1.33333f) * 12.5f / 0.89f) * xRotStrengthFactorForXPivot);
            skyCameraCenterXPivotTransform.transform.localEulerAngles = new Vector3(newXRotForXpivot, 0, 0);

        #endregion

        #region Material Changes

            earthZ01Color.a = earthAlpha;
            earthZ01Mat.color = earthZ01Color;
            earthZ01Mat.renderQueue = earthRenderQueue;

            breitengradeGitternetzColor.a = gitterNetzAlpha;
            breitengradeGitternetzMat.color = breitengradeGitternetzColor;

            traceN05Color.a = traceMatNo05alpha;
            traceN05Mat.color = traceN05Color;

            gridGroundColor.a = gridAlpha;
            gridGroundSpriteRenderer.color = gridGroundColor;

            groundColor.a = planeGroundAlpha;
            planeGroundMat.color = groundColor;

            north.faceColor = new Color(255, 255, 255, alphaFont);
            east.faceColor = new Color(255, 255, 255, alphaFont);
            south.faceColor = new Color(255, 255, 255, alphaFont);
            west.faceColor = new Color(255, 255, 255, alphaFont);

            outOfSphereSkymat.color = new Color(1.0f, 1.0f, 1.0f, skyOutOfSphereBackgroundColorAlpha);

        #endregion

        #region Other Changes

            float posyStrengthFactor = earthPos.y / 180.0f;
            float newEarthPosY = ((((resolutionFactor - 0.45f) * 360.0f) / 0.3f) - earthPos.y) * posyStrengthFactor;
            float posZStrengthFactor = earthPos.y / 200.0f;
            float newEarthPosZ = (earthPos.z - (((2.2222f - screenfactor) * 200.0f) / 0.89f)) * posZStrengthFactor;
            Vector3 targetPosearth = new Vector3(0, -90 + newEarthPosY,
                newEarthPosZ + starrySkyCompassPivot.localPosition.z * 0.75f);

            earthTransf.localPosition = targetPosearth;

        #endregion
        }
    }

    public void ResetScript() {
        moveWithAnim = false;
    }

    //Veränderungen durch Animation in der Update()-Funktion erlauben
    public void AllowMoveWithAnim() {
        moveWithAnim = true;
    }

    //Das Verändern der Objekte durch die Animation stoppen
    public void StopMoveWithAnim() {
        moveWithAnim = false;
    }

    //Kamera bis am Rand der Himmelskugel und dann das Tracing der Breitengradsterne starten
    public void FinishAndTurnLDOff() {
        n05H.StartTracingAfterZoomIsFinishIn0505();
    }

    //Getracte Linien der BreitengradSterne ausblenden, da ein sauberes 3D-Modell der Gitternetzkugel in der Animation eingeblendet wurde
    public void StopEmittingOfTrailInN05() {
        n05H.TurnTraceLinesOff();
    }

    //Wenn Animation beendet zu nächsten Step der Json springen
    public void FinishStep() {
        pC.JumpToNextStep();
    }
}