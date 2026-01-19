using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positoner : MonoBehaviour
{
    public GameObject targetObject;

    public void GetBounds()
    {
        Collider collider = targetObject.GetComponent<Collider>();
        Debug.Log(collider.bounds);
    }
}
