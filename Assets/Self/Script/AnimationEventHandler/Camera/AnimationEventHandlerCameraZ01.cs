using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AnimationEventHandlerCameraZ01 : MonoBehaviour {
    
#region public Variables

    //Scripts
    public ProcedureController pC;
    public Z01Helper z01H;

    //other Objects
    public Camera skyCamera;

    public Material breitengradeGitternetzMat;
    public Material earthZ01Mat;
    public Material planeGroundMat;
    public Material laengenNetzGrade;
    public Material horizontMat;
    public Material outOfSphereSkymat;
    public Material traceBeliebigeMat;
    public Material traceZ01GWMat;

    private LensDistortion lD;

    public SpriteRenderer gridGroundSpriteRenderer;

    public TextMeshPro north, east, south, west;

    public Transform centerTransformXPivot;
    public Transform earthPos;
    public Transform skyCameraTransf;

    public Volume volume;

    //Animation Parameters
    public float alphaFont = 255;
    public float alphaTraceBeliebige = 0.0f;
    public float alphaTraceGW = 1.0f;
    public float camFieldOfView = 40;
    public float earthAlphaZ01 = 0.0f;
    public float earthHeight = -90;
    public float gridAlpha = 1.0f;
    public float laengenNetzAlpha = 0.0f;
    public float ldIntensity = 0.4f;
    public float planeGroundAlpha = 1.0f;
    public float posX = 0f;
    public float posY = 3.0f;
    public float posZ = 0.0f;
    public float scaleVal = 1.04f;
    public float skyOutOfSphereBackgroundColorAlpha = 0.0f;
    public float horizontAlpha = 0.0f;
    public float xIdensity = 1.0f;
    public float xRot = 0.0f;

    public Vector3 targetRot = new Vector3(0, 0, 0);
    public Vector3 thisRot = new Vector3(0, 0, 0);

#endregion

#region private Variables

    private Color breitengradeGitternetzColor;
    private Color colorGround;
    private Color earthZ01Color;
    private Color gridGroundColor;
    private Color horizontColor;
    private Color laengenNetzColor;
    private Color traceBeliebigeColor;
    private Color traceZ01GWColor;

    private bool moveWithAnim = false;

    private float resolutionFactor = 0.0f;
    private float screenfactor = 0.0f;

#endregion

    private void Start() {
        LensDistortion tmp;

        if (volume.profile.TryGet(out tmp)) {
            lD = tmp;
        }

        //Farben mit den Ausgangswerten der Materials initialisieren, im folgenden werden nur die Alpha-Werte verändert
        breitengradeGitternetzColor = breitengradeGitternetzMat.color;
        colorGround = planeGroundMat.color;
        earthZ01Color = earthZ01Mat.color;
        gridGroundColor = gridGroundSpriteRenderer.color;
        horizontColor = horizontMat.color;
        laengenNetzColor = laengenNetzGrade.color;
        traceBeliebigeColor = traceBeliebigeMat.color;
        traceZ01GWColor = traceZ01GWMat.color;

        resolutionFactor = DeviceInfo.GetResolutionFactor();
        screenfactor = DeviceInfo.GetScreenfactor();
    }

    private void Update() {
        if (moveWithAnim) {
        #region Camera Changes

            float fovStrengthFactor = (camFieldOfView - 10) / 20.0f;
            float newFOV = camFieldOfView + ((((resolutionFactor - 0.45f) * 15.0f) / 0.3f) * fovStrengthFactor);
            skyCamera.fieldOfView = newFOV;

            float thisRotStrength = (resolutionFactor - 0.45f) / 0.3f;
            float newHisRotY = thisRot.y * thisRotStrength;
            float newHisRotZ = thisRot.z * thisRotStrength;
            skyCameraTransf.transform.localEulerAngles = new Vector3(thisRot.x, newHisRotY, newHisRotZ);

            float newPosY = posY + (((screenfactor - 1.3333333f) * 290.0f) / 0.89f);
            float newPosZ = posZ - (((screenfactor - 1.3333333f) * 200.0f) / 0.89f);
            skyCameraTransf.transform.localPosition = new Vector3(0, newPosY, newPosZ);

            lD.scale.Override(scaleVal);
            lD.intensity.Override(ldIntensity);
            lD.xMultiplier.Override(xIdensity);

        #endregion

        #region Material Changes

            colorGround.a = planeGroundAlpha;
            planeGroundMat.color = colorGround;

            gridGroundColor.a = gridAlpha;
            gridGroundSpriteRenderer.color = gridGroundColor;

            earthZ01Color.a = earthAlphaZ01;
            earthZ01Mat.color = earthZ01Color;

            laengenNetzColor.a = laengenNetzAlpha;
            laengenNetzGrade.color = laengenNetzColor;

            horizontColor.a = horizontAlpha;
            horizontMat.color = horizontColor;

            traceBeliebigeColor.a = alphaTraceBeliebige;
            traceBeliebigeMat.color = traceBeliebigeColor;
            traceZ01GWColor.a = alphaTraceGW;
            traceZ01GWMat.color = traceZ01GWColor;

            north.faceColor = new Color(255, 255, 255, alphaFont);
            east.faceColor = new Color(255, 255, 255, alphaFont);
            south.faceColor = new Color(255, 255, 255, alphaFont);
            west.faceColor = new Color(255, 255, 255, alphaFont);

            outOfSphereSkymat.color = new Color(1.0f, 1.0f, 1.0f, skyOutOfSphereBackgroundColorAlpha);

        #endregion

        #region Other Changes

            float newTargetXRot = targetRot.x;
            Quaternion temp = Quaternion.Euler(newTargetXRot, targetRot.y, targetRot.z);
            centerTransformXPivot.rotation = temp;
            earthPos.localPosition = new Vector3(earthPos.localPosition.x, earthHeight, earthPos.localPosition.z);

        #endregion
        }
    }

    public void AllowMoveWithAnimZ01EventHandler() {
        moveWithAnim = true;
    }

    public void StopMoveWithAnimZ01AEH() {
        moveWithAnim = false;
    }

    //In Z01 werden beliebige Sterne getraced die eine Himmelskugel bilden sollen, hier das Tracing starten
    public void StartTraceBeliebigeStars() {
        z01H.StartTraceBeliebige();
    }

    //Tracen der beliebigen Sterne stoppen, weil die Himmelskugel eine volle Drehung gemacht hat und somit jeder Stern einen vollen Kreis gezogen hat
    public void StopEmittingOfTraceBeliebigeStarsInZ01() {
        z01H.StopEmittingOfTrailRenderer();
    }

    //Himmelsaequator tracen und zum nächsten Step springen
    public void FinishJumpAndTraceHimmelsAequator() {
        z01H.ContinueAndTraceHimmelsAequator();
        pC.JumpToNextStep();
    }
}