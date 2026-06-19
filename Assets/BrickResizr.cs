using UnityEngine;

public class MatchBrickToCube : MonoBehaviour
{
    [SerializeField] Transform brickMesh;

    Vector3 initialCubeScale;
    Vector3 initialBrickScale;

    void Awake()
    {
        initialCubeScale = transform.localScale;
        initialBrickScale = brickMesh.localScale;
    }

    void LateUpdate()
    {
        Vector3 ratio = new Vector3(
            transform.localScale.x / initialCubeScale.x,
            transform.localScale.y / initialCubeScale.y,
            transform.localScale.z / initialCubeScale.z
        );

        brickMesh.localScale = Vector3.Scale(initialBrickScale, ratio);
    }
}