using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloorTiltController : MonoBehaviour
{
    public float maxTiltAngle = 12f;
    public float tiltSpeed = 6f;

    Rigidbody rb;
    Quaternion startRotation;

    float inputX;
    float inputZ;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startRotation = rb.rotation;
    }

    void Update()
    {
        // Read input in Update (best practice)
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        // Convert input to tilt angles
        float tiltX = -inputZ * maxTiltAngle;
        float tiltZ =  inputX * maxTiltAngle;

        Quaternion targetRot = startRotation * Quaternion.Euler(tiltX, 0f, tiltZ);

        // Smooth + physics-friendly rotation
        Quaternion newRot = Quaternion.Slerp(rb.rotation, targetRot, tiltSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(newRot);
    }
}