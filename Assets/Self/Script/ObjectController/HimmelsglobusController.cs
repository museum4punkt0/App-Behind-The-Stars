#region Description

/*Das Skript dient hauptsächlich dafür, um den Himmelsglobus smooth einzublenden. Die Alpha-Werte dafür werden
 * in einer Animation gesetzt. Beim URP gibt es leider Probleme mit transparenten Materials und der Priorisierung.
 * Deswegen müssen nach vollständiger Einblendung alle transparenten Materials durch Opaque Materials ausgestauscht
 * werden (FinalizeStepX()). Weitere Funktion ist die Positionierung der Kamera und Starten des Minipfads wenn
 * ein Marker auf dem Himmelsglobus ausgewählt wurde
 */

#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HimmelsglobusController : MonoBehaviour {
#region public Variables

    public HimmelsglobusRotation hGR;
    public ProcedureController pC;
    public SkyCamHelperForZ02 sCHFZ02;

    public Button okButton;

    public Camera mainCamera;

    public Transform cameraContainer;
    public Transform mainCamTransf;
    public Transform camPivotY;
    public Transform hgMondHalfHalf;
    public Transform starrySkyCompassPivot;
    public Transform from;

    //hg materials
    public Material hgHorizont;
    public Material hgHorizont2;
    public Material hgHorizontOpaque;
    public Material hgHorizont2Opaque;
    public Material hgMeridian;
    public Material hgMeridianOpaque;
    public MeshRenderer hgHorizontMR;
    public MeshRenderer hgHorizontMR_2;
    public MeshRenderer hgMeridianMR;

    //step3
    public Material hg24Uhr;
    public Material hg24Uhr_2;
    public Material hg24UhrOpaque;
    public Material hg24UhrOpaque_2;
    public MeshRenderer hg24UhrMR;
    public MeshRenderer hg24UhrMR_2;
    public Material hgEkliptik;
    public Material hgEkliptikOpaque;
    public MeshRenderer hgSonnenbahnMR;
    public MeshRenderer hgMondbahnMR;
    public Material hgGold;
    public Material hgGoldOpaque;
    public MeshRenderer hgGoldMR;
    public MeshRenderer hgMondMR;
    public MeshRenderer hgMondScheibeMR;
    public Material hgGold24UhrBody;
    public Material hgGold24UhrBodyOpaque;
    public MeshRenderer hgGold24UhrBodyViertelstundenUhrMR;
    public MeshRenderer hgGold24UhrBodyMR;
    public MeshRenderer hgGold12UhrBodyMR;
    public Material hgGoldSonne;
    public Material hgGoldSonneOpaque;
    public MeshRenderer hgGoldSonneMR;

    public SpriteRenderer hgUhrUnten;

    //step3 Armillarsphaere
    public Material hgArmGold;
    public Material hgArmGoldOpaque;
    public MeshRenderer hgArmDeckelMR;
    public MeshRenderer hgArmEkliptikMR;
    public MeshRenderer hgArmHalter1MR;
    public MeshRenderer hgArmHalter003MR;
    public MeshRenderer hgArmHimmel1MR;
    public MeshRenderer hgArmHimmel2MR;
    public MeshRenderer hgArmHimmel004MR;
    public MeshRenderer hgArmHorizontMR;
    public MeshRenderer hgArmKugelGrMR;
    public MeshRenderer hgArmKugelKlMR;
    public MeshRenderer hgArmMeridianMR;
    public MeshRenderer hgArmPolarNMR;
    public MeshRenderer hgArmPolarSMR;
    public MeshRenderer hgArmSegelMR;
    public MeshRenderer hgArmTropenNMR;
    public MeshRenderer hgArmTropenSMR;
    public MeshRenderer hgArmCylinder002MR;
    public MeshRenderer hgArmCylinder003MR;
    public MeshRenderer hgArmCylinder004MR;
    public MeshRenderer hgArmRotationsachseMR;
    public MeshRenderer hgArmSphere001MR;

    //step4 Gestell
    public Material hgBeineGold;
    public Material hgBeineflachesObjekt;
    public Material hgDrachenfluegel;
    public Material hgDrachenkopf;
    public Material hgHalterung;
    public Material hgSphereGestell;
    public Material hgBeineGoldOpaque;
    public Material hgBeineflachesObjektOpaque;
    public Material hgDrachenfluegelOpaque;
    public Material hgDrachenkopfOpaque;
    public Material hgHalterungOpaque;
    public Material hgSphereGestellOpaque;
    public MeshRenderer hgBeineMR;
    public MeshRenderer hgHalterungMR;

    public MeshRenderer hgSphererGestellMR;

    //Step4 Erdglobus
    public Material hgErdglobus;
    public Material hgErdglobusMeridian;
    public Material hgErdglobusMeridianOpaque;
    public Material hgErdglobusOpaque;
    public MeshRenderer hgErdglobusMR;
    public MeshRenderer hgErdglobusMeridianMR;
    public GameObject hgErdglobusUhr;

    //step4 Sockel
    public Material hgSockel;
    public Material hgBodenplatte;
    public Material hgEGHorizont;
    public Material hgGestellGold;
    public Material hgKompass;
    public Material hgSockelOpaque;
    public Material hgBodenplatteOpaque;
    public Material hgEGHorizontOpaque;
    public Material hgGestellGoldOpaque;
    public Material hgKompassOpaque;
    public MeshRenderer hgSockelMR;
    public MeshRenderer hgBodenplatteMR;
    public MeshRenderer hgEGHorizontMR;
    public MeshRenderer hgLoewenkopfMR;
    public MeshRenderer hgSauele01MR;
    public MeshRenderer hgSauele02MR;
    public MeshRenderer hgSauele03MR;
    public MeshRenderer hgSauele04MR;
    public MeshRenderer hgSockelNietenMR;
    public MeshRenderer hgSockelRingMR;
    public MeshRenderer hgSockelRingInnenMR;
    public MeshRenderer hgSockelKompassMR;
    public MeshRenderer hgBodenplatteInnenMR;
    public MeshRenderer hgKompassnadle01MR;
    public MeshRenderer hgKompassnadle02MR;
    public MeshRenderer hgKompassnadle03MR;
    public MeshRenderer hgKompassnadle04MR;
    public MeshRenderer hgKompassplatteMR;
    public MeshRenderer hgKompassNadelMitteMR;
    public MeshRenderer hgHandMR;
    public Material hgMarker;

    //Animation Parameters
    public Color hgHorizontColor;
    public Color hgMeridianColor;
    public Color hg24UhrColor;
    public Color hg24UhrColor_2;
    public Color hgEkliptikColor;
    public Color hgGoldColor;
    public Color hgGold24UhrBodyColor;
    public Color hgGoldSonneColor;
    public Color hgUhrUntenColor;
    public Color hgArmGoldColor;
    public Color hgBeineGoldColor;
    public Color hgBeineFlachesObjektColor;
    public Color hgDrachenfluegelColor;
    public Color hgDrachenkopfColor;
    public Color hgHalterungColor;
    public Color hgSphereGestellColor;
    public Color hgErdglobusColor;
    public Color hgErdglobusMeridianColor;
    public Color hgSockelColor;
    public Color hgBodenplatteColor;
    public Color hgEGHorizontColor;
    public Color hgGestellGoldColor;
    public Color hgKompassColor;

    public float hgMarkerAlpa = 0.0f;

    public Vector3 mainCameraPosition;
    public Vector3 meridianRotationAngles = new Vector3(0, 0, 0);

#endregion

#region private Variables

    private bool allowMoveToTarget = false;
    private bool allowSetColorsWithAnim = false;
    private bool allowMoveToInitPosition = false;
    private bool rotDown = false;
    private bool rotUp = false;
    private bool rotateHgSphereWithMeridian = false;
    private bool markerActive = false;

    private float newFOV = 15.0f;
    private float targetAngleMainCam = 0.0f;
    private float targetYRot = 0.0f;
    private float timeCount = 0.0f;

    private Vector3 newcamPivotY;

    private Quaternion newCameraContainerRotation;
    private Quaternion newMainCamXRot;
    private Quaternion toTarget;

#endregion

    void Start() {
        StartCoroutine(InitColors());
        ResetHimmelsglobus();
    }

    public void ShowButton() {
        okButton.interactable = true;
    }

    //Himmelsglobus in seinen Ausgangszustand bringen (insbesondere Transforms und Materials)
    public void ResetHimmelsglobus() {
        allowMoveToTarget = false;
        allowSetColorsWithAnim = false;
        allowMoveToInitPosition = false;
        rotDown = false;
        rotUp = false;
        newFOV = 15.0f;
        targetAngleMainCam = 0.0f;
        targetYRot = 0.0f;
        timeCount = 0.0f;
        rotateHgSphereWithMeridian = false;

        hgHorizontColor.a = 0;
        hgMeridianColor.a = 0;
        hg24UhrColor.a = 0;
        hg24UhrColor_2.a = 0;
        hgEkliptikColor.a = 0;
        hgGoldColor.a = 0;
        hgGold24UhrBodyColor.a = 0;
        hgGoldSonneColor.a = 0;
        hgUhrUntenColor.a = 0;
        hgArmGoldColor.a = 0;
        hgBeineGoldColor.a = 0;
        hgBeineFlachesObjektColor.a = 0;
        hgDrachenfluegelColor.a = 0;
        hgDrachenkopfColor.a = 0;
        hgHalterungColor.a = 0;
        hgSphereGestellColor.a = 0;
        hgErdglobusColor.a = 0;
        hgErdglobusMeridianColor.a = 0;
        hgSockelColor.a = 0;
        hgBodenplatteColor.a = 0;
        hgEGHorizontColor.a = 0;
        hgGestellGoldColor.a = 0;
        hgKompassColor.a = 0;

        hgHorizont.color = hgHorizontColor;
        hgHorizont2.color = hgHorizontColor;

        //Step3
        hgMeridian.color = hgMeridianColor;
        hg24Uhr.color = hg24UhrColor;
        hg24Uhr_2.color = hg24UhrColor_2;
        hgEkliptik.color = hgEkliptikColor;
        hgGold.color = hgGoldColor;
        hgGold24UhrBody.color = hgGold24UhrBodyColor;
        hgGoldSonne.color = hgGoldSonneColor;
        hgUhrUnten.color = hgUhrUntenColor;
        hgArmGold.color = hgArmGoldColor;

        hgMondHalfHalf.localScale = new Vector3(0, 0, 0);
        //Step4
        hgBeineGold.color = hgBeineGoldColor;
        hgDrachenfluegel.color = hgDrachenfluegelColor;
        hgDrachenkopf.color = hgDrachenkopfColor;
        hgHalterung.color = hgHalterungColor;
        hgSphereGestell.color = hgSphereGestellColor;
        hgBeineflachesObjekt.color = hgBeineFlachesObjektColor;
        hgErdglobus.color = hgErdglobusColor;
        hgErdglobusMeridian.color = hgErdglobusMeridianColor;
        hgBodenplatte.color = hgBodenplatteColor;
        hgEGHorizont.color = hgEGHorizontColor;
        hgGestellGold.color = hgGestellGoldColor;
        hgKompass.color = hgKompassColor;
        hgSockel.color = hgSockelColor;
        Color hgMarkerColor = hgMarker.color;
        hgMarkerColor.a = hgMarkerAlpa;
        hgMarker.color = hgMarkerColor;

        hgHorizontMR.sharedMaterial = hgHorizont;
        hgHorizontMR_2.sharedMaterial = hgHorizont2;

        hgMeridianMR.sharedMaterial = hgMeridian;
        hg24UhrMR.sharedMaterial = hg24Uhr;
        hg24UhrMR_2.sharedMaterial = hg24Uhr_2;
        hgSonnenbahnMR.sharedMaterial = hgEkliptik;
        hgMondbahnMR.sharedMaterial = hgEkliptik;
        hgMondScheibeMR.sharedMaterial = hgEkliptik;
        hgGoldMR.sharedMaterial = hgGold;
        hgMondMR.sharedMaterial = hgGold;
        hgGold24UhrBodyViertelstundenUhrMR.sharedMaterial = hgGold24UhrBody;
        hgGold24UhrBodyMR.sharedMaterial = hgGold24UhrBody;
        hgGold12UhrBodyMR.sharedMaterial = hgGold24UhrBody;
        hgGoldSonneMR.sharedMaterial = hgGoldSonne;

        //Armillarsphaere
        hgArmDeckelMR.sharedMaterial = hgArmGold;
        hgArmEkliptikMR.sharedMaterial = hgArmGold;
        hgArmHalter1MR.sharedMaterial = hgArmGold;
        hgArmHalter003MR.sharedMaterial = hgArmGold;
        hgArmHimmel1MR.sharedMaterial = hgArmGold;
        hgArmHimmel2MR.sharedMaterial = hgArmGold;
        hgArmHimmel004MR.sharedMaterial = hgArmGold;
        hgArmHorizontMR.sharedMaterial = hgArmGold;
        hgArmKugelGrMR.sharedMaterial = hgArmGold;
        hgArmKugelKlMR.sharedMaterial = hgArmGold;
        hgArmMeridianMR.sharedMaterial = hgArmGold;
        hgArmPolarNMR.sharedMaterial = hgArmGold;
        hgArmPolarSMR.sharedMaterial = hgArmGold;
        hgArmSegelMR.sharedMaterial = hgArmGold;
        hgArmTropenNMR.sharedMaterial = hgArmGold;
        hgArmTropenSMR.sharedMaterial = hgArmGold;
        hgArmCylinder002MR.sharedMaterial = hgArmGold;
        hgArmCylinder003MR.sharedMaterial = hgArmGold;
        hgArmCylinder004MR.sharedMaterial = hgArmGold;
        hgArmRotationsachseMR.sharedMaterial = hgArmGold;
        hgArmSphere001MR.sharedMaterial = hgArmGold;


        Material[] sharedMaterialsCopy = hgBeineMR.sharedMaterials;
        sharedMaterialsCopy[0] = hgBeineGold;
        sharedMaterialsCopy[1] = hgDrachenfluegel;
        sharedMaterialsCopy[2] = hgDrachenkopf;
        sharedMaterialsCopy[3] = hgBeineflachesObjekt;
        hgBeineMR.sharedMaterials = sharedMaterialsCopy;
        hgHalterungMR.sharedMaterial = hgHalterung;
        hgSphererGestellMR.sharedMaterial = hgSphereGestell;
        hgErdglobusMR.sharedMaterial = hgErdglobus;
        hgErdglobusMeridianMR.sharedMaterial = hgErdglobusMeridian;
        hgErdglobusUhr.SetActive(false);


        hgBodenplatteMR.sharedMaterial = hgBodenplatte;
        hgEGHorizontMR.sharedMaterial = hgEGHorizont;
        hgLoewenkopfMR.sharedMaterial = hgGestellGold;
        hgSauele01MR.sharedMaterial = hgGestellGold;
        hgSauele02MR.sharedMaterial = hgGestellGold;
        hgSauele03MR.sharedMaterial = hgGestellGold;
        hgSauele04MR.sharedMaterial = hgGestellGold;
        hgSockelNietenMR.sharedMaterial = hgGestellGold;
        hgSockelRingMR.sharedMaterial = hgGestellGold;
        hgSockelRingInnenMR.sharedMaterial = hgGestellGold;
        hgSockelKompassMR.sharedMaterial = hgGestellGold;
        hgBodenplatteInnenMR.sharedMaterial = hgBodenplatte;
        hgKompassnadle01MR.sharedMaterial = hgGestellGold;
        hgKompassnadle02MR.sharedMaterial = hgGestellGold;
        hgKompassnadle03MR.sharedMaterial = hgGestellGold;
        hgKompassnadle04MR.sharedMaterial = hgGestellGold;
        hgKompassplatteMR.sharedMaterial = hgKompass;
        hgKompassNadelMitteMR.sharedMaterial = hgGestellGold;
        hgHandMR.sharedMaterial = hgGestellGold;
        hgSockelMR.sharedMaterial = hgSockel;
    }

    private void Update() {
        if (allowMoveToTarget) {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, newFOV, 0.1f);
            camPivotY.transform.position = Vector3.MoveTowards(camPivotY.transform.position, newcamPivotY, 100f);

            cameraContainer.rotation =
                Quaternion.Slerp(cameraContainer.rotation, newCameraContainerRotation, Time.deltaTime * 5f);

            mainCamTransf.rotation = Quaternion.Slerp(from.rotation, toTarget, Time.deltaTime * 5f);
            mainCamTransf.localEulerAngles = new Vector3(0, 0, mainCamTransf.localEulerAngles.z);

            if (mainCamera.fieldOfView > newFOV - 0.5f && mainCamera.fieldOfView < newFOV + 0.5f &&
                camPivotY.transform.position == newcamPivotY &&
                mainCamTransf.localEulerAngles.x > targetAngleMainCam - 0.5f &&
                mainCamTransf.localEulerAngles.x <= targetAngleMainCam + 0.5f
                && cameraContainer.localEulerAngles.y >= targetYRot - 0.5f &&
                cameraContainer.localEulerAngles.y <= targetYRot + 0.5f) {
                allowMoveToTarget = false;
            }
        }

        if (allowMoveToInitPosition) {
            float resolutionFactor = DeviceInfo.GetResolutionFactor();
            float temp = ((resolutionFactor - 0.45f) * 2) + 1;
            mainCamTransf.localPosition = mainCameraPosition * temp;
        }

        if (allowSetColorsWithAnim) {
            //Step2
            hgHorizont.color = hgHorizontColor;
            hgHorizont2.color = hgHorizontColor;

            //Step3
            hgMeridian.color = hgMeridianColor;
            hg24Uhr.color = hg24UhrColor;
            hg24Uhr_2.color = hg24UhrColor_2;
            hgEkliptik.color = hgEkliptikColor;
            hgGold.color = hgGoldColor;
            hgGold24UhrBody.color = hgGold24UhrBodyColor;
            hgGoldSonne.color = hgGoldSonneColor;
            hgUhrUnten.color = hgUhrUntenColor;
            hgArmGold.color = hgArmGoldColor;

            //Step4
            hgBeineGold.color = hgBeineGoldColor;
            hgDrachenfluegel.color = hgDrachenfluegelColor;
            hgDrachenkopf.color = hgDrachenkopfColor;
            hgHalterung.color = hgHalterungColor;
            hgSphereGestell.color = hgSphereGestellColor;
            hgBeineflachesObjekt.color = hgBeineFlachesObjektColor;
            hgErdglobus.color = hgErdglobusColor;
            hgErdglobusMeridian.color = hgErdglobusMeridianColor;
            hgBodenplatte.color = hgBodenplatteColor;
            hgEGHorizont.color = hgEGHorizontColor;
            hgGestellGold.color = hgGestellGoldColor;
            hgKompass.color = hgKompassColor;
            hgSockel.color = hgSockelColor;
            Color hgMarkerColor = hgMarker.color;
            hgMarkerColor.a = hgMarkerAlpa;
            hgMarker.color = hgMarkerColor;
        }

        if (rotateHgSphereWithMeridian) {
            starrySkyCompassPivot.rotation = Quaternion.Euler(meridianRotationAngles);
        }
    }

    //Wird aufgerufen, wenn ein Marker auf dem Himmelsglobus berührt wurde, Startet den Minipfad und fliegt die Kamera zur Inital-Position des Minipfads
    public void HitedMarker(float targetFOV, Vector3 camPivot, float yAngleCamContainer, float xAngleMainCam) {
        hGR.StopRotationForASecond();
        markerActive = true;
        newFOV = targetFOV;
        newcamPivotY = camPivot;
        targetAngleMainCam = xAngleMainCam;
        if (yAngleCamContainer == 0) {
            newCameraContainerRotation = Quaternion.Euler(0, cameraContainer.localEulerAngles.y, 0);
            targetYRot = cameraContainer.localEulerAngles.y;
        } else {
            newCameraContainerRotation = Quaternion.Euler(0, yAngleCamContainer, 0);
            targetYRot = yAngleCamContainer;
        }

        if (targetAngleMainCam < 360) {
            rotUp = false;
            rotDown = true;
        } else {
            rotDown = false;
            rotUp = true;
        }

        if (mainCamTransf.localEulerAngles.z != 0 || xAngleMainCam != 0) {
            toTarget = Quaternion.Euler(mainCamTransf.eulerAngles.x, mainCamTransf.eulerAngles.y, xAngleMainCam);
        }

        allowMoveToTarget = true;
    }


    public void SetMarkerActiveFalse() {
        markerActive = false;
        pC.JumpToCheckPoint("Z02.14a");
    }

    public bool GetStateMarkerActive() {
        return markerActive;
    }

    private IEnumerator InitColors() {
        yield return new WaitForSeconds(2f);
        //step2
        hgHorizontColor = hgHorizont.color;
        //step3
        hgMeridianColor = hgMeridian.color;
        hg24UhrColor = hg24Uhr.color;
        hg24UhrColor_2 = hg24Uhr_2.color;
        hgEkliptikColor = hgEkliptik.color;
        hgGoldColor = hgGold.color;
        hgGold24UhrBodyColor = hgGold24UhrBody.color;
        hgGoldSonneColor = hgGoldSonne.color;
        hgArmGoldColor = hgArmGold.color;
    }
#region Animation Handling-Functions
    
    public void StopMoveToTarget() {
        mainCamTransf.localEulerAngles = new Vector3(0, 0, 0);
        allowMoveToTarget = false;
    }

    public void SetColorsWithAnim() {
        allowSetColorsWithAnim = true;
    }

    public void StopSetColorsWithAnim() {
        allowSetColorsWithAnim = false;
    }
    
    public void SetAllowMoveToInitPosition() {
        allowMoveToInitPosition = true;
    }

    public void StopMoveToInitPosition() {
        allowMoveToInitPosition = false;
        sCHFZ02.enabled = false;
    }

    public void AllowRotatioWithMeridian() {
        rotateHgSphereWithMeridian = true;
    }

    public void StopRotatioWithMeridian() {
        rotateHgSphereWithMeridian = false;
    }

    public void ShowHgMarker() {
        hgMarkerAlpa = 1.0f;
    }

    public void HideHgMarker() {
        allowSetColorsWithAnim = false;
        hgMarkerAlpa = 0.0f;
    }

#region Finalize Functions

    //Wird von der Animation aufgerufen, wenn die Animation beendet ist. Dann werden alle transparenten Materials durch Opaque ausgteauscht
    public void FinalizeStep2() {
        hgHorizontMR.sharedMaterial = hgHorizontOpaque;
        hgHorizontMR_2.sharedMaterial = hgHorizont2Opaque;
    }

    public void FinalizeStep3() {
        hgMeridianMR.sharedMaterial = hgMeridianOpaque;
        hg24UhrMR.sharedMaterial = hg24UhrOpaque;
        hg24UhrMR_2.sharedMaterial = hg24UhrOpaque_2;
        hgSonnenbahnMR.sharedMaterial = hgEkliptikOpaque;
        hgMondbahnMR.sharedMaterial = hgEkliptikOpaque;
        hgMondScheibeMR.sharedMaterial = hgEkliptikOpaque;
        hgMondHalfHalf.localScale = new Vector3(0.46838f, 0.46838f, 0.46838f);
        hgGoldMR.sharedMaterial = hgGoldOpaque;
        hgMondMR.sharedMaterial = hgGoldOpaque;
        hgGold24UhrBodyMR.sharedMaterial = hgGold24UhrBodyOpaque;
        hgGold24UhrBodyViertelstundenUhrMR.sharedMaterial = hgGold24UhrBodyOpaque;
        hgGold12UhrBodyMR.sharedMaterial = hgGold24UhrBodyOpaque;
        hgGoldSonneMR.sharedMaterial = hgGoldSonneOpaque;

        //Armillarsphaere
        hgArmDeckelMR.sharedMaterial = hgArmGoldOpaque;
        hgArmEkliptikMR.sharedMaterial = hgArmGoldOpaque;
        hgArmHalter1MR.sharedMaterial = hgArmGoldOpaque;
        hgArmHalter003MR.sharedMaterial = hgArmGoldOpaque;
        hgArmHimmel1MR.sharedMaterial = hgArmGoldOpaque;
        hgArmHimmel2MR.sharedMaterial = hgArmGoldOpaque;
        hgArmHimmel004MR.sharedMaterial = hgArmGoldOpaque;
        hgArmHorizontMR.sharedMaterial = hgArmGoldOpaque;
        hgArmKugelGrMR.sharedMaterial = hgArmGoldOpaque;
        hgArmKugelKlMR.sharedMaterial = hgArmGoldOpaque;
        hgArmMeridianMR.sharedMaterial = hgArmGoldOpaque;
        hgArmPolarNMR.sharedMaterial = hgArmGoldOpaque;
        hgArmPolarSMR.sharedMaterial = hgArmGoldOpaque;
        hgArmSegelMR.sharedMaterial = hgArmGoldOpaque;
        hgArmTropenNMR.sharedMaterial = hgArmGoldOpaque;
        hgArmTropenSMR.sharedMaterial = hgArmGoldOpaque;
        hgArmCylinder002MR.sharedMaterial = hgArmGoldOpaque;
        hgArmCylinder003MR.sharedMaterial = hgArmGoldOpaque;
        hgArmCylinder004MR.sharedMaterial = hgArmGoldOpaque;
        hgArmRotationsachseMR.sharedMaterial = hgArmGoldOpaque;
        hgArmSphere001MR.sharedMaterial = hgArmGoldOpaque;
    }

    public void FinalizeStep4() {
        Material[] sharedMaterialsCopy = hgBeineMR.sharedMaterials;
        sharedMaterialsCopy[0] = hgBeineGoldOpaque;
        sharedMaterialsCopy[1] = hgDrachenfluegelOpaque;
        sharedMaterialsCopy[2] = hgDrachenkopfOpaque;
        sharedMaterialsCopy[3] = hgBeineflachesObjektOpaque;
        hgBeineMR.sharedMaterials = sharedMaterialsCopy;
        hgHalterungMR.sharedMaterial = hgHalterungOpaque;
        hgSphererGestellMR.sharedMaterial = hgSphereGestellOpaque;
        hgErdglobusMR.sharedMaterial = hgErdglobusOpaque;
        hgErdglobusMeridianMR.sharedMaterial = hgErdglobusMeridianOpaque;
        hgErdglobusUhr.SetActive(true);


        hgBodenplatteMR.sharedMaterial = hgBodenplatteOpaque;
        hgEGHorizontMR.sharedMaterial = hgEGHorizontOpaque;
        hgLoewenkopfMR.sharedMaterial = hgGestellGoldOpaque;
        hgSauele01MR.sharedMaterial = hgGestellGoldOpaque;
        hgSauele02MR.sharedMaterial = hgGestellGoldOpaque;
        hgSauele03MR.sharedMaterial = hgGestellGoldOpaque;
        hgSauele04MR.sharedMaterial = hgGestellGoldOpaque;
        hgSockelNietenMR.sharedMaterial = hgGestellGoldOpaque;
        hgSockelRingMR.sharedMaterial = hgGestellGoldOpaque;
        hgSockelRingInnenMR.sharedMaterial = hgGestellGoldOpaque;
        hgSockelKompassMR.sharedMaterial = hgGestellGoldOpaque;
        hgBodenplatteInnenMR.sharedMaterial = hgBodenplatteOpaque;
        hgKompassnadle01MR.sharedMaterial = hgGestellGoldOpaque;
        hgKompassnadle02MR.sharedMaterial = hgGestellGoldOpaque;
        hgKompassnadle03MR.sharedMaterial = hgGestellGoldOpaque;
        hgKompassnadle04MR.sharedMaterial = hgGestellGoldOpaque;
        hgKompassplatteMR.sharedMaterial = hgKompassOpaque;
        hgKompassNadelMitteMR.sharedMaterial = hgGestellGoldOpaque;
        hgHandMR.sharedMaterial = hgGestellGoldOpaque;
        hgSockelMR.sharedMaterial = hgSockelOpaque;
    }

#endregion
    #endregion
}