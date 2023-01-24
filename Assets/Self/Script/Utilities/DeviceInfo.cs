using UnityEngine;

public class DeviceInfo : MonoBehaviour {
    public RectTransform safeAreaRectTransf;

#region private Variables

    private short CurrentFPS = 0;
    private float m_unscaledDeltaTime = 0f;
    private static float resolutionFactor = 0f;
    private static float screenFactor = 0f;

    private Vector2 minAnchor;
    private Vector2 maxAnchor;

    private Rect safeArea;

#endregion

    private void Awake() {
        
#if UNITY_IOS && UNITY_EDITOR
        //Für iOS muss eine SafeArea berechnet werden, die die Bereiche von Notch (am oberen Bildrand) und Leiste am unteren Bildrand rausrechnet
        //für Android automatisch, dafür Haken entfernen unter Player Settings->Resolution and Presentation -> Render outside safe area
        safeArea = Screen.safeArea;
        minAnchor = safeArea.position;
        maxAnchor = minAnchor + safeArea.size;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;
        try {
            safeAreaRectTransf.anchorMin = minAnchor;
            safeAreaRectTransf.anchorMax = maxAnchor;
        }catch(Exception ex){}
#endif

        resolutionFactor = Screen.safeArea.width / Screen.safeArea.height;
        screenFactor = Screen.safeArea.height / Screen.safeArea.width;
    }

    void Update() {
        m_unscaledDeltaTime = Time.unscaledDeltaTime;
    }

    //Gibt Aufschluss über die Performance (FPS)
    public float GetUnscaledTime() {
        return m_unscaledDeltaTime;
    }

    public static float GetResolutionFactor() {
        return resolutionFactor;
    }

    public static float GetScreenfactor() {
        return screenFactor;
    }
}