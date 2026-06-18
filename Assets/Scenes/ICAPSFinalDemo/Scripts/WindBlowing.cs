using UnityEngine;

public class WindGust : MonoBehaviour
{
    // Tag applied to all your bricks
    public string targetTag = "Bricks";

    // Strength of the wind gust
    public float windStrength = 50f;

    void Update()
    {
        // Press O to blow all bricks to the right
        if (Input.GetKeyDown(KeyCode.G))
        {
            BlowWind();
        }
    }

    void BlowWind()
    {
        GameObject[] bricks = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject brick in bricks)
        {
            Rigidbody rb = brick.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(
                    Vector3.right * windStrength,
                    ForceMode.Impulse
                );
            }
        }

        Debug.Log("Wind gust blew all bricks to the right!");
    }
}