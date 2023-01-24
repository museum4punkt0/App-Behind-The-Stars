using UnityEngine;

public class TrailControllerHorizontS03 : MonoBehaviour {
    public TrailRenderer trailOverHorizont;
    public Transform directionalSunLight;

    private bool checkAndDrawSunLine = false;
    private bool checkDrawOverHorizont = false;

    void Update() {
        float temp = directionalSunLight.eulerAngles.x;
        //Object nur tracen, wenn es sich unterm Horizont befindet
        if (checkAndDrawSunLine) {
            if (temp > 0 && temp < 180) {
                trailOverHorizont.emitting = false;
            } else {
                trailOverHorizont.emitting = true;
            }
        }

        //Object nur tracen, wenn es sich überm Horizont befindet
        if (checkDrawOverHorizont) {
            if (temp > 0 && temp < 180) {
                trailOverHorizont.emitting = true;
            } else {
                trailOverHorizont.emitting = false;
            }
        }
    }

    public void AllowCheckAndDraw() {
        trailOverHorizont.enabled = true;
        checkAndDrawSunLine = true;
        trailOverHorizont.time = 5000000;
    }

    public void AllowCheckAndDrawOverHorizont() {
        trailOverHorizont.enabled = true;
        trailOverHorizont.emitting = true;
        checkAndDrawSunLine = false;
        trailOverHorizont.time = 5000000;
        checkDrawOverHorizont = true;
    }

    public void StopCheckAndDraw() {
        trailOverHorizont.Clear();
        checkAndDrawSunLine = false;
        trailOverHorizont.time = 0;
        trailOverHorizont.emitting = false;
        checkDrawOverHorizont = false;
        trailOverHorizont.enabled = false;
    }

    public void StopDrawing() {
        trailOverHorizont.emitting = false;
        checkDrawOverHorizont = false;
        checkAndDrawSunLine = false;
        trailOverHorizont.SetPosition(trailOverHorizont.positionCount - 1,
            trailOverHorizont.GetPosition(trailOverHorizont.positionCount - 2));
    }
}