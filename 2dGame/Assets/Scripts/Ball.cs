using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Ball : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        transform.GetComponent<CinemachineCollisionImpulseSource>().enabled = true;
    }
}
