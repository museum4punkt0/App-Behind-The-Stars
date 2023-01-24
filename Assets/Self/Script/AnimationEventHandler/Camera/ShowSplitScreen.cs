using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ShowSplitScreen : MonoBehaviour {
#region public Variables

    public ProcedureController pC;
    public SkyTimeController skyTimeController;

    public Camera mainCam;
    public Camera sunDialCam;

    public RectTransform splitLine;

    public Transform kompassNadelPivot;
    public Transform pointPlatte;

    public Volume volume;

    //Animation Parameter
    public float yRotationNadel = 0.0f;
    public float yPos = 0;
    public float yHeight = 1;
    public float lensDScale = 1.04f;
    public float camSize = 65.0f;
    public float splitLinePos;

#endregion

#region private Variables

    private bool calculateMinMax = false;
    private bool allowScaleLD = false;
    private bool allowScaleCamSize = false;
    private bool allowPosSplitLineS05 = false;
    private bool allowSetMainCamFrame = false;

    private float min = 10000.0f;
    private float max = 0.0f;
    private float resolutionFactor = 1.0f;

    private LensDistortion lD;

#endregion

    private void Start() {
        CalculateResolutionFactor();
        LensDistortion tmp;

        if (volume.profile.TryGet(out tmp)) {
            lD = tmp;
        }
    }

    public void Reset() {
        splitLine.anchoredPosition = new Vector2(0, 0);
        allowSetMainCamFrame = false;
        allowScaleCamSize = false;
        allowScaleLD = false;
        yPos = 0;
        yHeight = 1;
        mainCam.rect = new Rect(0, 0, 1, 1);
        sunDialCam.rect = new Rect(0, 0, 1, 0);
        sunDialCam.orthographicSize = 65;
    }

    void Update() {
        if (calculateMinMax) {
            Vector3 calcedShadowPos = sunDialCam.WorldToScreenPoint(pointPlatte.position);
            if (calcedShadowPos.x < Screen.width && calcedShadowPos.x > 0) {
                if (calcedShadowPos.y < min) {
                    min = calcedShadowPos.y;
                }

                if (calcedShadowPos.y > max) {
                    max = calcedShadowPos.y;
                }
            }
        }

        kompassNadelPivot.localEulerAngles = new Vector3(0, yRotationNadel, 0);

        //Skalierung der Kameraverzerrung anpassen
        if (allowScaleLD) {
            lD.scale.Override(lensDScale);
            if (lensDScale == 1.3f) {
                allowScaleLD = false;
            }
        }

        //SplitScreen einblenden
        if (allowSetMainCamFrame) {
            splitLine.anchoredPosition = new Vector2(splitLine.anchoredPosition.x, splitLinePos);
            mainCam.rect = new Rect(0, yPos, 1, yHeight);
        }

        if (allowScaleCamSize) {
            splitLine.anchoredPosition = new Vector2(splitLine.anchoredPosition.x, splitLinePos);
            sunDialCam.orthographicSize = camSize * resolutionFactor;
        }

        //Splitscreen einblenden
        if (allowPosSplitLineS05) {
            splitLine.anchoredPosition = new Vector2(splitLine.anchoredPosition.x, splitLinePos);
            sunDialCam.orthographicSize = camSize * resolutionFactor;

            if (camSize > 65) {
                sunDialCam.orthographicSize = 65;
            }
        }
    }

    //Berechnung der Max und Min Pos des Schattens der Perle erlauben. Die Grenzpositionen sind die Grundlage, der zulässigen Range für die Interaktion in S02
    public void AllowCalculateMinMax() {
        min = 10000;
        max = 0;
        calculateMinMax = true;
        skyTimeController.ResetAnimatedDays();
    }

    //Stoppt die Berechnung der Grenzpositionen
    public void StopCalculateMinMax() {
        calculateMinMax = false;
    }

    //Gibt die ermittelten Grenzpositionen zurück
    public Vector2 GetYRange() {
        return new Vector2(min, max);
    }

    public void CalculateResolutionFactor() {
        resolutionFactor = ((float) Screen.height / (float) Screen.width - 1.77f) / 2.5f + 1.0f;
    }

    private IEnumerator WaitAndJumpToNextStep() {
        yield return new WaitForSeconds(1f);
        pC.JumpToNextStep();
    }

#region Animation Handling-Functions
    public void SetAllowSplitLineS05() {
        allowPosSplitLineS05 = true;
        allowScaleCamSize = false;
    }

    public void StopSetSplitline() {
        allowPosSplitLineS05 = false;
    }

    public void SetAllowSetMainCamFrame() {
        allowSetMainCamFrame = true;
    }

    public void SetCurrentDate() {
        skyTimeController.SetDate(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);
        StartCoroutine(WaitAndJumpToNextStep());
    }

    public void StopSetMainCamFrame() {
        allowSetMainCamFrame = false;
    }

    public void FinishStep() {
        pC.JumpToNextStep();
    }

    public void SetAllowScaleLD() {
        allowScaleLD = true;
    }

    public void SetAllowSize() {
        allowScaleCamSize = true;
    }

    public void StopScaleSize() {
        allowScaleCamSize = false;
    }

#endregion
    
}