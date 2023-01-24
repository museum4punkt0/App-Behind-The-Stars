using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour {
    public TrailRenderer traceDrawer;
    private void Awake() {
        traceDrawer = transform.GetComponent<TrailRenderer>();
    }
    //Startet das Tracing, mit einem Schweif, nach 6.5Sekunden blendet verschiwndet ein gezeichneter Punkt wieder
    public void StartTracing()
    {
        traceDrawer.enabled = true;
        traceDrawer.emitting = true;
        traceDrawer.time = 0;
        traceDrawer.time = 6.5f;
    }

    //Startet das Tracing. Getracte Linien sollen kontinuierlich stehen bleiben, time wird dafür auf eine sehr hohe Zahl gesetzt.
    public void StartTracingN05() {
        traceDrawer.enabled = true;
        traceDrawer.time = 0;
        traceDrawer.time = 600000000f;
    }

    //Stoppt das Tracing und blendet die getracten Linien aus.
    public void StopTracing() {
        traceDrawer.time = 0;
        traceDrawer.enabled = false;
    }

    public void TurnLaengengradTraceOn() {
        traceDrawer.enabled = true;
    }

    //Stoppt das Tracing, blendet die getracten Linien aus und löscht diese auch aus dem Speicher
    public void TurnTraceOff() {
        traceDrawer.Clear();
        traceDrawer.time = 0;
        traceDrawer.enabled = false;
    }

    //Stoppt das Zeichnen des TrailRenderers
    public void StopEmitting() {
        traceDrawer.emitting = false;
    }

    //Blendet die getracten Linien ein
    public void ShowTrace() {
        traceDrawer.enabled = true;
    }
    //Blendet die getracten Linien aus
    public void HideTrace() {
        traceDrawer.enabled = false;
    }
}
