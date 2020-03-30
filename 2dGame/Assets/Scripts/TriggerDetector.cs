using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    public bool InTrigger;
    public GameObject LevelGround;

    private void OnTriggerEnter2D(Collider2D colliderInfo)
    {
        foreach (Transform ground in LevelGround.transform.GetComponentsInChildren<Transform>())
        {
            if (colliderInfo.transform == ground)
            {
                InTrigger = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D colliderInfo)
    {
        foreach (Transform ground in LevelGround.transform.GetComponentsInChildren<Transform>())
        {
            if (colliderInfo.transform == ground)
            {
                InTrigger = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D colliderInfo)
    {
        foreach (Transform ground in LevelGround.transform.GetComponentsInChildren<Transform>())
        {
            if (colliderInfo.transform == ground)
            {
                InTrigger = false;
            }
        }
    }
}
