using System.Collections;
using TMPro;
using UnityEngine;

public class ToolTipController : MonoBehaviour {
#region public Variables

    public ProcedureController pC;

    public Animator toolTipAnimator;
    public Animator scrollClockAnimator;

    public float cameraContainerYAchse = 400;
    public float mainCameraPivotXAchse = 400;

    public GameObject parentFrontOfHead;
    public GameObject tutorialBlockButtonTT02Head;

    public RectTransform tutorialHandImage;
    public RectTransform canvas;

    public TextMeshProUGUI tutorialText;

    public Transform cameraContainer;
    public Transform moveObject;
    public Transform mainCameraPivot;

    public Vector2 handAnchoredPosition;

#endregion

#region private Variables

    private bool moveCamWithAnim = false;
    private bool tutorialCheckpointPart2Active = false;
    private bool moveHandWithAnim = false;
    private bool resizeHandPos = false;
    private bool toolTipActive = false;

#endregion

    public void ShutDownTooltip() {
        tutorialBlockButtonTT02Head.SetActive(false);
        moveObject.localScale = new Vector3(0, 0, 0);
        toolTipActive = false;
    }

    public void HideTooltop() {
        toolTipAnimator.Play("HideTooltip", 0, 0);
        tutorialBlockButtonTT02Head.SetActive(false);
        StopMoveCamWithAnim();
        scrollClockAnimator.enabled = false;
        toolTipActive = false;
    }

    public void HideToolTipCheckpointPart2() {
        if (tutorialCheckpointPart2Active) {
            toolTipAnimator.Play("HideTooltipCheckpointPart2", 0, 0);
            tutorialCheckpointPart2Active = false;
        }

        toolTipActive = false;
    }

    public void SetParentBack() {
        transform.parent = parentFrontOfHead.transform;
    }
    
    private void Update() {
        //Bei der Tutorialeinblendung wird der Himmel durch eine Animation gesteuert, um die mögliche Bewegung am Himmel zu verdeutlichen
        if (moveCamWithAnim) {
            cameraContainer.localEulerAngles = new Vector3(0, cameraContainerYAchse, 0);
            mainCameraPivot.localEulerAngles = new Vector3(mainCameraPivotXAchse, 0, 0);
        }
        // Bei der Tutorialeinblendung wird auch eine Hand angezeigt, deren Position ebenfalls durch eine Animation gesteuert wird
        if (moveHandWithAnim) {
            float yHandPos = canvas.sizeDelta.y * handAnchoredPosition.y;
            tutorialHandImage.anchoredPosition = new Vector2(handAnchoredPosition.x, yHandPos);
        }
    }

    public void AllowMoveCamWithAnim() {
        moveCamWithAnim = true;
    }

    public void StopMoveCamWithAnim() {
        moveCamWithAnim = false;
    }

    private IEnumerator ShowHandPart2() {
        yield return new WaitForSeconds(1.5f);
        toolTipAnimator.Play("Tutorial03Part2");
    }

    public void SetAllowMoveHandImageWithAnim() {
        moveHandWithAnim = true;
    }

    public void StopMoveHandImageWithAnim() {
        moveHandWithAnim = false;
    }

    public void SetAllowResizeHandPos() {
        resizeHandPos = true;
    }

    public void StopResizeHandPos() {
        resizeHandPos = false;
    }

    public void SetBoolToolTipActiveTrue() {
        toolTipActive = true;
    }

    public bool GetToolTipState() {
        return toolTipActive;
    }
}