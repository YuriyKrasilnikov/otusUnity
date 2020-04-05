using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    public GameObject CanTake;

    void OnTriggerEnter2D(Collider2D colliderInfo)
    {
        if (colliderInfo.gameObject == CanTake)
        {
            Debug.Log("Take Coin!!!");
            gameObject.SetActive(false);

            try
            {
                colliderInfo.gameObject.GetComponent<Player>().CoinCamera.GetComponent<SelectFocus>().StartAnimate(colliderInfo.transform);
            }
            catch (Exception e)
            {
                Debug.Log("Not have CoinCamera");
                Debug.Log(e);
            }
        }
    }
}
