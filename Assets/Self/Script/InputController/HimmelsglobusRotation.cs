using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

public class HimmelsglobusRotation : MonoBehaviour {
#region public Variables

    public HimmelsglobusController hgC;

    public Animator mainCamAnimator;

    public Camera hgCamera;
    public Camera mainCam;
    public Camera skyCam;

    public Transform cameraContainer;
    public Transform camPivot;

    public float skycamFOV = 8.0f;
    public float cameraContainerRotationYAngle = -40;
    public float heightRect = 0;

    public Vector3 cameraPivotxPosition = new Vector3(0, 0, 0);

#endregion

#region private Variables

    private bool allowRotation = true;
    private bool stopWhileOverUI = false;
    private bool stopForce = false;
    private bool allowMoveFov = false;
    private bool allowMoveFovEG = false;
    private bool showSplitScreenHG = false;

    private float MouseZoomSpeed = 15.0f;
    private float TouchZoomSpeed = 0.1f;
    private float ZoomMinBound = 0.1f;
    private float ZoomMaxBound = 179.9f;
    private float m_mouseX = 0.0f;
    private float m_mouseY = 0.0f;
    private float resolutionFactor;

    private Vector2 FirstPoint;
    private Vector3 m_startRotation;
    private Vector3 SecondPoint;

#endregion

    private void Start() {
        m_startRotation = transform.eulerAngles;
        resolutionFactor = DeviceInfo.GetResolutionFactor();
    }

    public void InitCamPos() {
        cameraPivotxPosition = new Vector3(0, 0, 0);
        m_mouseX = 0.0f;
        m_mouseY = 0.0f;
        stopWhileOverUI = false;
        stopForce = false;
        allowMoveFov = false;
        allowMoveFovEG = false;
        showSplitScreenHG = false;
    }

    void Update() {
    #region Animation Changes

        if (allowMoveFov) {
            skyCam.fieldOfView = skycamFOV;
            cameraContainer.transform.eulerAngles = new Vector3(0, cameraContainerRotationYAngle, 0);
            camPivot.localPosition = cameraPivotxPosition;
        }

        if (allowMoveFovEG) {
            float fov = ((resolutionFactor - 0.45f) * 4 / 0.3f) + skycamFOV;
            skyCam.fieldOfView = fov;
        }

        if (showSplitScreenHG) {
            float camPivotX = (((resolutionFactor - 0.45f) * 2400 / 0.3f) + 3947);
            float camPivotY = (((resolutionFactor - 0.45f) * 470 / 0.3f) + 3060);
            camPivot.localPosition = new Vector3(camPivotX, camPivotY, 0);
            skyCam.fieldOfView = skycamFOV;
            hgCamera.rect = new Rect(0, 0.1f, 1, heightRect);
        }

    #endregion
    }

    private void LateUpdate() {
        if (allowRotation && !stopForce) {
        #region Mouse Input

            // Input mit rechter Maustaste um im Editor testen zu können
            if (Input.GetMouseButtonDown(1)) {
                m_startRotation = transform.eulerAngles;
                if (EventSystem.current.IsPointerOverGameObject()
#if !UNITY_EDITOR
                                    || EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId)
#endif
                ) {
                    stopWhileOverUI = true;
                } else {
                    stopWhileOverUI = false;
                    hgC.StopMoveToTarget();
                }
            }

            if (Input.GetMouseButton(1)) {
                if (!stopWhileOverUI) {
                    camPivot.transform.position += new Vector3(0, Input.GetAxis("Mouse Y") * 100f, 0);
                    if (camPivot.transform.position.y < -3700) {
                        camPivot.transform.position = new Vector3(0, -3700, 0);
                    } else if (camPivot.transform.position.y > 2000) {
                        camPivot.transform.position = new Vector3(0, 2000, 0);
                    }

                    cameraContainer.transform.eulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * 10f, 0);
                    if (hgC.GetStateMarkerActive()) {
                        hgC.SetMarkerActiveFalse();
                    }
                }
            }

            if (Input.GetMouseButtonUp(1)) {
                stopWhileOverUI = false;
            }

        #endregion

        #region Touch Input

            if (Input.touchCount == 1) {
                Touch touch = Input.GetTouch(0);
                int id = touch.fingerId;
                if (Input.GetTouch(0).phase == TouchPhase.Began) {
                    FirstPoint = Input.GetTouch(0).position;
                    if (EventSystem.current.IsPointerOverGameObject()
#if !UNITY_EDITOR
                                    || EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId)
#endif
                    ) {
                        stopWhileOverUI = true;
                    } else {
                        stopWhileOverUI = false;
                    }
                }

                if (Input.GetTouch(0).phase == TouchPhase.Moved) {
                    if (!stopWhileOverUI) {
                        hgC.StopMoveToTarget();
                        camPivot.transform.position -= new Vector3(0, Input.GetAxis("Mouse Y") * 20f, 0);
                        if (camPivot.transform.position.y < -3700) {
                            camPivot.transform.position = new Vector3(0, -3700, 0);
                        } else if (camPivot.transform.position.y > 2000) {
                            camPivot.transform.position = new Vector3(0, 2000, 0);
                        }

                        cameraContainer.transform.eulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * 4f, 0);
                        if (hgC.GetStateMarkerActive()) {
                            hgC.SetMarkerActiveFalse();
                        }
                    }
                }
            } else if (Input.touchCount == 2) {
                hgC.StopMoveToTarget();
                Touch tZero = Input.GetTouch(0);
                Touch tOne = Input.GetTouch(1);

                Vector2 tZeroPrevious = tZero.position - tZero.deltaPosition;
                Vector2 tOnePrevious = tOne.position - tOne.deltaPosition;

                float oldTouchDistance = Vector2.Distance(tZeroPrevious, tOnePrevious);
                float currentTouchDistance = Vector2.Distance(tZero.position, tOne.position);

                float deltaDistance = oldTouchDistance - currentTouchDistance;
                Zoom(deltaDistance, TouchZoomSpeed);
            }

        #endregion
        }
    }

    //Zoom-Funktion bei der Erkundung des Himmelsglobus, in beide Richtungen begrenzt
    private void Zoom(float deltaMagnitudeDiff, float speed) {
        mainCam.fieldOfView += (deltaMagnitudeDiff * speed) / 3.0f;
        mainCam.fieldOfView = Mathf.Clamp(mainCam.fieldOfView, ZoomMinBound, ZoomMaxBound);
        if (mainCam.fieldOfView > 55) {
            mainCam.fieldOfView = 55;
        } else if (mainCam.fieldOfView < 5) {
            mainCam.fieldOfView = 5;
        }
    }

    //Im Mini-Lernpfad Armilarsphäre soll ein Splitscreen angezeigt werden, je nach Auflösung des Endgeräts muss die Kamera-position angepasst werden
    public void AllowSetCamRectHeight() {
        skycamFOV = 9.0f;
        float mainCamX = (((resolutionFactor - 0.45f) * 5200 / 0.3f) + 1312) * -1;
        float mainCamY = (((resolutionFactor - 0.45f) * 300 / 0.3f) + 850) * -1;
        transform.localPosition = new Vector3(mainCamX, mainCamY, 75);
        float camPivotX = (((resolutionFactor - 0.45f) * 2400 / 0.3f) + 3947);
        float camPivotY = (((resolutionFactor - 0.45f) * 470 / 0.3f) + 3060);
        camPivot.localPosition = new Vector3(camPivotX, camPivotY, 0);
        showSplitScreenHG = true;
    }

    //Wenn z.B. über den zurück Button zum Knotenpunkt des AR-Modus gesprungen wird, soll sofort der Splitscreen geschlossen werden
    public void ForceCloseSplitScreen() {
        mainCamAnimator.Rebind();
        mainCamAnimator.enabled = false;
        showSplitScreenHG = false;
        hgCamera.rect = new Rect(0, 0.1f, 1, 0);
        mainCam.rect = new Rect(0, 0, 1, 1);
    }

#region Animation Handling-Functions

    public void AllowMoveCamFOVWithAnim() {
        allowMoveFov = true;
        allowMoveFovEG = false;
    }

    public void AllowMoveCamFOVWithAnimErdglobus() {
        skycamFOV = 9;
        allowMoveFov = false;
        allowMoveFovEG = true;
    }

    public void StopMoveCamFOVWithAnim() {
        allowMoveFov = false;
        allowMoveFovEG = false;
    }

    public void StopRotation() {
        allowRotation = false;
    }

    public void AllowRotating() {
        m_mouseY = 0.0f;
        m_mouseX = 0.0f;
        stopForce = false;
        allowRotation = true;
    }

    public void ForceStoppingBecauseOfPath() {
        stopForce = true;
    }

    public void ForceStoppingEnd() {
        stopForce = false;
    }

    public void StopSetCamRectHeight() {
        showSplitScreenHG = false;
    }

    public void StopRotationForASecond() {
        allowRotation = false;
        StartCoroutine(WaitAndAllowRotating());
    }

#endregion

    private IEnumerator StopMoveToTarger() {
        yield return new WaitForSeconds(2f);
        hgC.StopMoveToTarget();
    }
    
    private IEnumerator WaitAndAllowRotating() {
        yield return new WaitForSeconds(1f);
        allowRotation = true;
    }
}