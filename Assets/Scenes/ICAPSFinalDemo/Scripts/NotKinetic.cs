using UnityEngine;

public class DropObjectButton : MonoBehaviour
{
    public string targetTag = "Bricks";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            DropObjects();
        }
    }

    void DropObjects()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(targetTag))
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();

            if (rb != null)
            {
                if (rb.isKinematic) {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                }
                else {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }
            }
        }

        Debug.Log("Dropped all " + targetTag + " objects.");
    }
}