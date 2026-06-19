using UnityEngine;

public class PlayerBlockController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float invisibleRange = 5f;

    void Update()
    {
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.K))
        {
            MakeNearestBlockInvisible();
        }
    }

    void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(x, 0f, z);
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }

    void MakeNearestBlockInvisible()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("object");

        GameObject nearestBlock = null;
        float nearestDistance = invisibleRange;

        foreach (GameObject block in blocks)
        {
            float distance = Vector3.Distance(transform.position, block.transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestBlock = block;
            }
        }

        if (nearestBlock != null)
        {
            nearestBlock.SetActive(false);

            // Print CLEAN when a block is cleaned
            Debug.Log("CLEAN");
        }
    }
}