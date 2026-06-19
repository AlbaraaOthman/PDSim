using UnityEngine;

public class KeyboardMoveObject : MonoBehaviour
{
    public float moveSpeed = 1000f;
    public float rotateSpeed = 100f;

    void Update()
    {
        // WASD / Arrow keys movement
        float h = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float v = Input.GetAxis("Vertical");   // W/S or Up/Down

        Vector3 move = new Vector3(h, 0f, v);
        transform.position += move * moveSpeed * Time.deltaTime;

        // Optional rotation (Q/E)
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.E))
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }
}