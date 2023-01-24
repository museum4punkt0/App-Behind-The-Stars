using UnityEngine.EventSystems;
using UnityEngine;

public class OrbitalCameraController : MonoBehaviour {
#region public Variables

    public ProcedureController pC;
    public ToolTipController tTC;
    public HelpController hC;

    public GameObject wrongInputPrefab;
    public GameObject canvasParent;


    public Transform cameraContainer;

#endregion

#region private Variables

    private bool allowRotation = false;
    private bool stopWhileOverUI = false;
    private bool allowSetWrongInput = false;
    private bool checkInputWhileTutorial01IsActive = false;
    
    private float m_mouseX = 0.0f;
    private float m_mouseY = 0.0f;
    private float tempMouseDistanceX = 0.0f;
    private float tempMouseDistanceY = 0.0f;
    private float touchSpeedFactor = 1.0f;

    private Vector3 m_StartRotation;

#endregion

    private void Start() {
        m_StartRotation = transform.eulerAngles;
    }

    private void LateUpdate() {
        if (allowRotation) {
        #region Mouse Input

#if UNITY_EDITOR
            //Mouse Input zum Testen in Unity-Editor mit rechter Maus-Taste
            if (Input.GetMouseButtonDown(1)) {
                if (EventSystem.current.IsPointerOverGameObject()) {
                    stopWhileOverUI = true;
                } else {
                    tempMouseDistanceX = 0.0f;
                    tempMouseDistanceY = 0.0f;
                    stopWhileOverUI = false;
                }
            }

            if (Input.GetMouseButton(1)) {
                if (!stopWhileOverUI) {
                    m_mouseX += Input.GetAxis("Mouse X") * 5f * touchSpeedFactor;
                    m_mouseY -= Input.GetAxis("Mouse Y") * 5f * touchSpeedFactor;
                    tempMouseDistanceX += Input.GetAxis("Mouse X") * 5f * touchSpeedFactor;
                    tempMouseDistanceY -= Input.GetAxis("Mouse Y") * 5f * touchSpeedFactor;

                    if (m_mouseY < -50) {
                        m_mouseY = -50;
                    } else if (m_mouseY > 10) {
                        m_mouseY = 10;
                    }

                    Vector3 temp = new Vector3(m_mouseY, 0, 0) + m_StartRotation;
                    if (temp.x < -50) {
                        temp = new Vector3(-50, 0, 0);
                    } else if (temp.x > 10) {
                        temp = new Vector3(10, 0, 0);
                    }

                    cameraContainer.transform.localRotation =
                        Quaternion.Euler(new Vector3(0, m_mouseX, 0) + m_StartRotation);
                    if (temp.x < 10 && temp.x > -50) {
                        transform.localRotation = Quaternion.Euler(new Vector3(m_mouseY, 0, 0) + m_StartRotation);
                    }

                    if (checkInputWhileTutorial01IsActive && transform.eulerAngles.y > 2 &&
                        transform.eulerAngles.y < 358) {
                        tTC.HideTooltop();
                        hC.HideHelpPanel();
                        checkInputWhileTutorial01IsActive = false;
                    }
                }
            }

            if (Input.GetMouseButtonUp(1)) {
                if (allowSetWrongInput && !stopWhileOverUI && tempMouseDistanceX > -5 && tempMouseDistanceX < 5 &&
                    tempMouseDistanceY > -5 && tempMouseDistanceY < 5) {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit)) {
                        if (hit.collider != null) {
                        }
                    } else {
                        GameObject newWrongInputPoint = Instantiate<GameObject>(wrongInputPrefab);
                        newWrongInputPoint.transform.parent = canvasParent.transform;
                        newWrongInputPoint.transform.localScale = new Vector3(1.0625f, 0.86247f, 1);
                        newWrongInputPoint.transform.position =
                            new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
                        if (!pC.GetStepId().Contains("N02")) {
                            pC.JumpWithQuestInput(1);
                        }
                    }
                }

                stopWhileOverUI = false;
            }
#endif

        #endregion

        #region Touch Input

            if (Input.touchCount > 0) {
                Touch touch = Input.GetTouch(0);
                int id = touch.fingerId;
                if (Input.GetTouch(0).phase == TouchPhase.Began) {
                    if (EventSystem.current.IsPointerOverGameObject(id)) {
                        stopWhileOverUI = true;
                    } else {
                        tempMouseDistanceX = 0.0f;
                        tempMouseDistanceY = 0.0f;
                        stopWhileOverUI = false;
                    }
                }

                if (Input.GetTouch(0).phase == TouchPhase.Moved) {
                    if (!stopWhileOverUI) {
                        m_mouseX -= Input.GetAxis("Mouse X") * touchSpeedFactor;
                        m_mouseY += Input.GetAxis("Mouse Y") * touchSpeedFactor;
                        tempMouseDistanceX -= Input.GetAxis("Mouse X") * touchSpeedFactor;
                        tempMouseDistanceY += Input.GetAxis("Mouse Y") * touchSpeedFactor;

                        //Nach oben und unten die Rotation begrenzen
                        if (m_mouseY < -50) {
                            m_mouseY = -50;
                        } else if (m_mouseY > 10) {
                            m_mouseY = 10;
                        }

                        Vector3 temp = new Vector3(m_mouseY, 0, 0) + m_StartRotation;
                        if (temp.x < -50) {
                            temp = new Vector3(-50, 0, 0);
                        } else if (temp.x > 10) {
                            temp = new Vector3(10, 0, 0);
                        }

                        cameraContainer.transform.localRotation =
                            Quaternion.Euler(new Vector3(0, m_mouseX, 0) + m_StartRotation);
                        if (temp.x < 10 && temp.x > -50) {
                            transform.localRotation = Quaternion.Euler(new Vector3(m_mouseY, 0, 0) + m_StartRotation);
                        }

                        //Wenn das erste Tutorial eingeblendet ist muss um mindestens 2 Grad gedreht werden um das Tutorial zu beenden
                        if (checkInputWhileTutorial01IsActive && transform.eulerAngles.y > 2 &&
                            transform.eulerAngles.y < 358) {
                            tTC.HideTooltop();
                            hC.HideHelpPanel();
                            checkInputWhileTutorial01IsActive = false;
                        }
                    }
                }

                if (Input.GetTouch(0).phase == TouchPhase.Ended) {
                    //Wenn allowSetWrongInput=true z.B. bei Polarstern antippen und der Input nicht auf der UI passiert (stopWhileOverUI=false)
                    //und nicht geswipped wurde, dann wird ein roter Kreis (wrongInputPrefab) instanziiert,
                    //der Kreis zerstört sich nach Ablauf seiner Animation selbst.
                    if (allowSetWrongInput && !stopWhileOverUI && tempMouseDistanceX > -2 && tempMouseDistanceX < 2 &&
                        tempMouseDistanceY > -2 && tempMouseDistanceY < 2) {
                        RaycastHit hit;
                        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                        if (Physics.Raycast(ray, out hit)) {
                            if (hit.collider != null) {
                            }
                        } else {
                            GameObject newWrongInputPoint = Instantiate<GameObject>(wrongInputPrefab);
                            newWrongInputPoint.transform.parent = canvasParent.transform;
                            newWrongInputPoint.transform.localScale = new Vector3(1.0625f, 0.86247f, 1);
                            newWrongInputPoint.transform.position =
                                new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

                            if (!pC.GetStepId().Contains("N02")) {
                                pC.JumpWithQuestInput(1);
                            }
                        }
                    }
                }
            }

        #endregion
        }
    }

    public void ResetVariables() {
        m_mouseY = 0.0f;
        m_mouseX = 0.0f;
    }

    //Erlauben den Himmel zu bewegen
    public void AllowRotating() {
        allowRotation = true;
    }

    public void StopRotation() {
        allowRotation = false;
    }

    //Zur Feinjustierung speed verlangsamen
    public void SetTouchSpeedFactor(float speed) {
        touchSpeedFactor = speed;
    }

    //Der app erlauben, das WrongInputPrefab zu instanziieren und einen roten Kreis bei falsch Eingabe zu erzeigen
    public void AllowSetWrongInput() {
        allowSetWrongInput = true;
    }

    public void StopAllowSetWrongInput() {
        allowSetWrongInput = false;
    }

    //Wenn das Tutorial in N01 gestartet ist, Input checken und bei Input Tutorial ausblenden
    public void SetAllowCheckInputWhileTutorial01IsOn() {
        checkInputWhileTutorial01IsActive = true;
    }
}