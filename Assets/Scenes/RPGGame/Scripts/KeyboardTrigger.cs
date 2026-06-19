using UnityEngine;
using Unity.VisualScripting;

public class KeyboardTrigger : MonoBehaviour
{
    public GameObject graphObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            CustomEvent.Trigger(graphObject, "ToggleItemK");
        }
    }
}