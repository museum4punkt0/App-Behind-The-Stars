using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimationEventHandlerCameraS03 : MonoBehaviour {
#region public Variables

    public SkyProfileEventHandler sPEH;

    public Animator skyAnimator;

    public Camera skyCamera;

    public GameObject sunDialParent;

    public Material breitengradLinienMat;
    public Material earthMaterial;
    public Material gitternetzGradeMat;
    public Material horizontLilaMat;
    public Material planeGroundMat;
    public Material outOfSphereSkymat;
    public Material matStarUnlit;
    public Material sunDialWoodMatTrans;

    public MeshRenderer sunDialWood;

    public SpriteRenderer gridGround;
    public SpriteRenderer earthFigueMat;
    public Material sunDialGroundPlateMatTrans;
    public SpriteRenderer sunDialGroundPlate;

    public TextMeshPro south;

    public Transform earthFigureTransf;
    public Transform earthFigureTransfParent;
    public Transform mainCamTransf;
    public Transform mainCameraObjectTransf;
    public Transform cameraContainer;
    public Transform southTransf;

    //Animation Parameters
    public float figureAlpha = 0.0f;
    public float gitterNetzAlpha = 0.0f;
    public float gridAlpha = 1.0f;
    public float horizontLilaAlpha = 0.0f;
    public float planeGroundAlpha = 1.0f;
    public float skyOutOfSphereBackgroundColorAlpha = 0.01f;
    public float skyCamFOV = 40;
    public float sunDialALpha = 1.0f;
    public float starAlpha = 0.0f;

    public int southAlpha = 255;

    public Vector3 earthFigureRotation = new Vector3(-38.982f, -2.068f, 1.301f);
    public Vector3 earthFigureScale = new Vector3(2.5f, 2.5f, 2.5f);
    public Vector3 maincamEuler = new Vector3(0, 180, 0);
    public Vector3 mainCamPosition = new Vector3(0, 60, -200);
    public Vector3 mainCamObjectEuler = new Vector3(0, 0, 0);
    public Vector3 southScale = new Vector3(0.18f, 0.18f, 0.18f);
    public Vector3 southLocalPos = new Vector3(0, 27, -260);

#endregion

#region private Variables

    private bool moveWithAnim = false;
    private float resolutionFactor = 0.0f;

#endregion

    private void Start() {
        resolutionFactor = DeviceInfo.GetResolutionFactor();
    }

    void Update() {
        if (moveWithAnim) {
        #region Camera Changes

            float newZpos = mainCamPosition.z - ((resolutionFactor - 0.45f) * 250 / 0.3f);
            mainCamTransf.localPosition = new Vector3(mainCamPosition.x, mainCamPosition.y, newZpos);
            cameraContainer.localEulerAngles = maincamEuler;
            mainCameraObjectTransf.localEulerAngles = mainCamObjectEuler;

            float fovStrength = skyCamFOV / 40.0f;
            float temp = ((resolutionFactor - 0.45f) * (20.0f * fovStrength) / 0.3f) + skyCamFOV;
            skyCamera.fieldOfView = temp;

        #endregion

        #region Material Changes

            Color earthFigureColor = earthFigueMat.color;
            earthFigureColor.a = figureAlpha;
            earthFigueMat.color = earthFigureColor;

            Color gitternetzGradeColor = gitternetzGradeMat.color;
            gitternetzGradeColor.a = gitterNetzAlpha;
            gitternetzGradeMat.color = gitternetzGradeColor;
            breitengradLinienMat.color = gitternetzGradeColor;

            Color gridGroundColor = gridGround.color;
            gridGroundColor.a = gridAlpha;
            gridGround.color = gridGroundColor;

            Color horizontLilaColor = horizontLilaMat.color;
            horizontLilaColor.a = horizontLilaAlpha;
            horizontLilaMat.color = horizontLilaColor;

            Color planeGroundColor = planeGroundMat.color;
            planeGroundColor.a = planeGroundAlpha;
            planeGroundMat.color = planeGroundColor;

            Color sunDialGroundPlateMatColor = sunDialGroundPlateMatTrans.color;
            sunDialGroundPlateMatColor.a = sunDialALpha;
            sunDialGroundPlateMatTrans.color = sunDialGroundPlateMatColor;

            Color matStarUnlitColor = matStarUnlit.color;
            matStarUnlitColor.a = starAlpha;
            matStarUnlit.color = matStarUnlitColor;

            Color sunDialWoodColor = sunDialWoodMatTrans.color;
            sunDialWoodColor.a = sunDialALpha;
            sunDialWoodMatTrans.color = sunDialWoodColor;

            earthFigureTransf.localEulerAngles = earthFigureRotation;
            Color earthMaterialColor = earthMaterial.color;
            earthMaterialColor.a = figureAlpha;
            earthMaterial.color = earthMaterialColor;

            outOfSphereSkymat.color = new Color(1.0f, 1.0f, 1.0f, skyOutOfSphereBackgroundColorAlpha);

        #endregion

        #region Other Changes

            southTransf.localPosition = southLocalPos;
            southTransf.localScale = southScale;
            south.faceColor = new Color32(255, 255, 255, (byte) southAlpha);

            earthFigureTransfParent.localScale = earthFigureScale;

        #endregion
        }
    }

    public void AllowMoveWithAnimS03AEH() {
        moveWithAnim = true;
    }

    public void StopMoveWithAnimS03AEH() {
        moveWithAnim = false;
    }

    //Sterne einblenden und die Atomsphere (Sonnenlicht) ausblenden
    public void ShowStars() {
        sPEH.ShowTheStars();
        sPEH.OnlyAllowUpdateSun();
    }
    
    //Die Himmelskugel weiter drehen und den Sonnenbogen tracen
    public void ContinueSkyAnimatorAndTraceBlueSunLine() {
        skyAnimator.speed = 1.0f;
    }

    //Sonnenuhr transparente Materials zuweisen, um sie Smooth ausblenden zu können bei der Kamerafahrt aus der Himmelskugel
    public void ChangeSunDIalMaterials() {
        sunDialGroundPlate.sharedMaterial = sunDialGroundPlateMatTrans;
        sunDialWood.sharedMaterial = sunDialWoodMatTrans;
    }

    //Sonnenuhr deaktivieren, wenn Kamera aus der Himmelskugel gefahren, bzw. wenn SOnnenuhr ausgeblendet ist
    public void SetSundDialActiveFalse() {
        sunDialParent.SetActive(false);
    }

    //Himmelskugel auf 10 Uhr rotieren und den Grossen Wagen ins Bild schieben
    public void RotateTo10() {
        skyAnimator.SetInteger("StartAnimateTimeline", 15);
        skyAnimator.Play("S03NEWAnimateTo10", 0, 0);
        skyAnimator.speed = 1.0f;
    }
}