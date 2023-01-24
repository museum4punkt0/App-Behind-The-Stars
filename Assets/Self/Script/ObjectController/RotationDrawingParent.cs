using UnityEngine;

public class RotationDrawingParent : MonoBehaviour {
    public Transform SchattenWerferKugel;
    public Transform sunRotation;
    
    private void Update() {
        transform.position = SchattenWerferKugel.position;
        transform.localEulerAngles = sunRotation.localEulerAngles;
    }
}
