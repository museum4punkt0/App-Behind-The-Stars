#region Description
/*Berechnet den Schatten der Perle auf der Sonnenuhr. In S02 wird nach falsch gezeichneter Linie ein lila Punkt an
 * die Position des berechneten Schattens gesetzt, um den zu beobachtenden Schatten noch deutlicher darzustellen
 */
#endregion

using UnityEngine;

public class CalculateShadowPosition : MonoBehaviour {
#region public Variables

    public Transform mp;
    public Transform sonne;
    public Transform pointPlatte;
    public Transform directionalSunLight;
    public Transform rotationObjectToSun;

#endregion

#region private Variables

    private bool allowPrintingPos = true;

    private float min = 10000.0f;
    private float max = 0.0f;

#endregion

    void Update() {
        if (allowPrintingPos) {
            Vector3 p_0 = mp.position;
            Vector3 n = -mp.up;

            Vector3 l_0 = sonne.position;
            Vector3 l = sonne.forward;

            float denominator = Vector3.Dot(l, n);

            if (denominator > 0.00001f) {
                float t = Vector3.Dot(p_0 - l_0, n) / denominator;

                //Punkt an dem der Strahl die Platte berührt
                Vector3 p = l_0 + l * t;
                pointPlatte.position = p;
                pointPlatte.rotation = directionalSunLight.rotation;

                rotationObjectToSun.position = p;
                rotationObjectToSun.localEulerAngles = new Vector3(directionalSunLight.localEulerAngles.x, 180, 0);
            }
        }
    }

    public void SetAllowPrintingPos() {
        allowPrintingPos = true;
    }

    public void StopPrintingPos() {
        allowPrintingPos = false;
    }
}