using UnityEngine;

public class ScaleBrickPhysics : MonoBehaviour
{
    public string targetTag = "Bricks";

    // Values for a normal 1x1x1 scale brick
    public float baseMass = 0.003f;
    public float baseDrag = 0.05f;
    public float baseAngularDrag = 0.05f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ApplyScaledPhysics();
        }
    }

    void ApplyScaledPhysics()
    {
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("Bricks");

        foreach (GameObject brick in bricks)
        {
            Rigidbody rb = brick.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 scale = brick.transform.lossyScale;

                // Volume scale = width × height × depth
                float volumeScale = scale.x * scale.y * scale.z;

                rb.mass = baseMass * volumeScale;
                rb.drag = baseDrag;
                rb.angularDrag = baseAngularDrag;

                rb.useGravity = true;
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                rb.interpolation = RigidbodyInterpolation.Interpolate;
            }
        }

        Debug.Log("Scaled physics values applied to all Brick objects.");
    }
}