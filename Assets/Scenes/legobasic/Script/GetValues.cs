using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class GetValues : MonoBehaviour
{


    Vector3 getValues()
    {
        Vector3 sizeVec = GetComponent<Collider>().bounds.size;

        return sizeVec;
    }
}
