using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CarouselController : MonoBehaviour {
#region public Variables

    //Scripts
    public MainMenuController mMC;

    //Objects
    public Button buttonHimmelsglobusStart;
    public Button buttonSonnenuhrStart;

    public GameObject cell;
    public GameObject markerParentHimmelsglobus;
    public GameObject markerParentSonnenuhr;
    public GameObject schlossHimmelsglobus;
    public GameObject schlossSonnenuhr;
    public List<GameObject> cellContainer = new List<GameObject>();

    public RectTransform canvasRect;
    public RectTransform _center;
    public RectTransform scrollRT;
    public RectTransform panel;

    public ScrollRect scroll;

    //Vars
    public bool shouldFocusCenter = true;
    public bool shouldLoop = true;

    public float cellGap = 105f;
    public float scaleSpeed = 5f;
    public float rotateSpeed = 5f;
    public float moveSpeed = 10f;
    public float focusCenterVelocityThreshold = 50f;

#endregion

#region private Variables

    private bool allowUpdateLoopMenu = true;
    private bool dragging = false;
    private bool moveRight = false;
    private bool moveLeft = false;

    private float width;
    private float height;
    private float tempDrag = 0;

    private int selectIndex = -1;
    private int centerCellIndex = 0;
    private int arcLength;
    private int offsetIndex;
    private int tempCenterIndex = -1;
    private int centerItem = 0;
    private int oldCenterItem = 0;

    private Vector3 boundary;

    private Vector3 dragStartPos;

    private Vector3 newPos;
    private Vector3 newScale;
    private Vector3 newRot;
    private Vector3 offset;

    private Vector3 final;

#endregion
    public void Start() {
        //Spielstand laden und je nach Spielstand die Instrumente initialisieren
        var loadData = "0";
        var filePath = Path.Combine(Application.persistentDataPath, "spielstand.txt");
        try {
            loadData = File.ReadAllText(filePath);
        } catch (FileNotFoundException ex) {
        }

        cellGap = Screen.width;
        if (loadData == "1") {
            schlossHimmelsglobus.SetActive(true);
            schlossSonnenuhr.SetActive(false);
            markerParentHimmelsglobus.SetActive(false);
            markerParentSonnenuhr.SetActive(true);
            buttonHimmelsglobusStart.interactable = false;
            buttonSonnenuhrStart.interactable = true;
        } else if (loadData == "2") {
            schlossHimmelsglobus.SetActive(false);
            schlossSonnenuhr.SetActive(false);
            markerParentHimmelsglobus.SetActive(true);
            markerParentSonnenuhr.SetActive(true);
            buttonHimmelsglobusStart.interactable = true;
            buttonSonnenuhrStart.interactable = true;
        } else {
            schlossHimmelsglobus.SetActive(true);
            schlossSonnenuhr.SetActive(true);
            markerParentHimmelsglobus.SetActive(false);
            markerParentSonnenuhr.SetActive(false);
            buttonHimmelsglobusStart.interactable = false;
            buttonSonnenuhrStart.interactable = false;
        }

        //Main Menu Carousel muss je nach Bildschirmauflösung skaliert werden
        float resolutionFactor = DeviceInfo.GetResolutionFactor();
        float tempScale = 0;
        if (resolutionFactor < 0.63f) {
            tempScale = 1 - ((resolutionFactor - 0.45f) * 0.475f / 0.3f);
        } else {
            tempScale = 1 - ((resolutionFactor - 0.45f) * 0.45f / 0.3f);
        }

        float tempPosY = 0 - (resolutionFactor - 0.45f) * 68 / 0.3f;
        scrollRT.localScale = new Vector3(tempScale, tempScale, tempScale);
        scrollRT.anchoredPosition = new Vector2(0, tempPosY);

        dragStartPos = panel.position;
        centerCellIndex = 0;
        SetupCarousel(0);
    }

    public void StartDrag() {
        allowUpdateLoopMenu = true;
        selectIndex = -1;
        dragging = true;
        dragStartPos = panel.position;
    }

    public void EndDrag() {
        dragging = false;
    }
    
    void SetupCarousel(int mode) {
        for (int i = 0; i < cellContainer.Count; i++) {
            RectTransform rt = cellContainer[i].GetComponent<RectTransform>();
            if (rt == null)
                continue;

            newPos = Vector3.zero;
            newScale = new Vector3(1, 1, 1);
            newRot = Vector3.zero;
            offset = (panel.position - dragStartPos);
            offsetIndex = i - centerCellIndex;

            width = (rt.rect.width + cellGap) * canvasRect.localScale.x;
            newPos.x = width * offsetIndex;

            final = (_center.position + newPos + offset);

            if (mode==0) {
                rt.position = final;
                rt.localScale = newScale;
                rt.localRotation = Quaternion.Euler(newRot);
                panel.ForceUpdateRectTransforms();
            } else {
                rt.position = Vector3.Lerp(rt.position, final, Time.deltaTime * moveSpeed);
                rt.localScale = Vector3.Lerp(rt.localScale, newScale, Time.deltaTime * scaleSpeed);
                rt.localRotation = Quaternion.Lerp(rt.localRotation, Quaternion.Euler(newRot),
                    Time.deltaTime * rotateSpeed);
            }
        }
    }

    void Update() {
        if (moveRight) {
            panel.position -= new Vector3(35, 0, 0);
            tempDrag += 35;
            if (tempDrag > Screen.width) {
                shouldFocusCenter = true;
                moveRight = false;
            }
        }

        if (moveLeft) {
            panel.position += new Vector3(35, 0, 0);
            tempDrag += 35;
            if (tempDrag > Screen.width) {
                shouldFocusCenter = true;
                moveLeft = false;
            }
        }
    }

    void LateUpdate() {
        scroll.horizontal = true;
        if (allowUpdateLoopMenu) {
            SetupCarousel(1);
            CheckBoundary();
            FindCenterCellIndex();
            CheckAutoFocus();
        }
    }

    //Pfeil am Seitenrand wurde benutzt, automatisch zum nächsten Instrument rotieren (rechts vom aktuellen Instrument)
    public void BtnClickedMoveRight() {
        allowUpdateLoopMenu = true;
        if (!moveRight && !moveLeft) {
            tempDrag = 0;
            moveRight = true;
            moveLeft = false;
            shouldFocusCenter = false;
        }
    }

    //Pfeil am Seitenrand wurde benutzt, automatisch zum nächsten Instrument rotieren (links vom aktuellen Instrument)
    public void BtnClickedMoveLeft() {
        if (!moveRight && !moveLeft) {
            tempDrag = 0;
            moveLeft = true;
            moveRight = false;
            shouldFocusCenter = false;
        }
    }
    
    void CheckAutoFocus() {
        if (!shouldFocusCenter)
            return;
        if (selectIndex != -1)
            return;
        if (dragging)
            return;
        if (scroll.horizontal && Mathf.Abs(scroll.velocity.x) > focusCenterVelocityThreshold)
            return;
        if (cellContainer.Count == 0)
            return;
        dragStartPos = panel.position;
    }

    void FindCenterCellIndex() {
        if (cellContainer.Count == 0)
            return;

        tempCenterIndex = -1;
        for (int i = 0; i < cellContainer.Count; i++) {
            if (tempCenterIndex == -1) {
                tempCenterIndex = i;
            } else {
                GameObject go = cellContainer[i];
                GameObject oldGo = cellContainer[tempCenterIndex];

                float oldDis = Vector3.Distance(oldGo.transform.position, _center.position);
                float dis = Vector3.Distance(go.transform.position, _center.position);
                if (dis < oldDis) {
                    //gefundenes GameObject aktivieren (Teasertexte + ggf. Pfadauswahl anzeigen)
                    if (go.name == "0") {
                        centerItem = 0;
                        if (centerItem != oldCenterItem) {
                            mMC.SetNokturnalActive();
                            oldCenterItem = 0;
                        }
                    } else if (go.name == "1") {
                        centerItem = 1;
                        if (centerItem != oldCenterItem) {
                            mMC.SetSonnenuhrActive();
                            oldCenterItem = 1;
                        }
                    } else if (go.name == "2") {
                        centerItem = 2;
                        if (centerItem != oldCenterItem) {
                            mMC.SetHimmelsglobusActive();
                            oldCenterItem = 2;
                        }
                    }

                    tempCenterIndex = i;
                }
            }
        }

        centerCellIndex = tempCenterIndex;
        CalculateDragOffset();
    }

    void CalculateDragOffset() {
        if (cellContainer.Count == 0)
            return;
        if (centerCellIndex < 0 || centerCellIndex >= cellContainer.Count)
            return;
        RectTransform center = cellContainer[centerCellIndex].GetComponent<RectTransform>();

        dragStartPos = (panel.position + (_center.position - center.position));
    }

    void CheckBoundary() {
        if (cell == null)
            return;
        if (!shouldLoop)
            return;
        if (cellContainer.Count == 0)
            return;
        RectTransform rt = cell.GetComponent<RectTransform>();
        if (rt == null)
            return;

        float cellWidth = (rt.rect.width + cellGap) * canvasRect.localScale.x;
        float cellHeight = (rt.rect.height + cellGap) * canvasRect.localScale.y;
        boundary.x = cellContainer.Count * cellWidth;
        boundary.y = cellContainer.Count * cellHeight;
        float leftBoundary = (_center.position.x - boundary.x / 2);
        float rightBoundary = (_center.position.x + boundary.x / 2);

        GameObject go = cellContainer[0];
        if (go.transform.position.x < leftBoundary) {
            UpdateCell(go, new Vector3(rightBoundary, go.transform.position.y, go.transform.position.z), false);
        }

        if (go.transform.position.x > rightBoundary) {
            UpdateCell(go, new Vector3(leftBoundary, go.transform.position.y, go.transform.position.z), true);
        }
    }

    void UpdateCell(GameObject passGO, Vector3 newPos, bool isInsert) {
        passGO.transform.position = newPos;
        GameObject selectGO = null;
        if (selectIndex != -1)
            selectGO = cellContainer[selectIndex];
        if (isInsert) {
            cellContainer.Remove(passGO);
            cellContainer.Insert(0, passGO);
        } else {
            cellContainer.Remove(passGO);
            cellContainer.Add(passGO);
        }

        if (selectGO) {
            selectIndex = cellContainer.IndexOf(selectGO);
        }
    }

    public int GetCenterItem() {
        return centerItem;
    }

    public void StopUpdateLoopMenu() {
        allowUpdateLoopMenu = false;
    }
}