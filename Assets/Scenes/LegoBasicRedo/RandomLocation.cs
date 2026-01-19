using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLocation : MonoBehaviour
{
    public Vector3 RandomPlace(Vector3 currentLocation)
    {
        float x = Random.Range(-5f, 5f);
        float z = Random.Range(-5f, 5f);

        return new Vector3(x, currentLocation.y, z);
    }
}
