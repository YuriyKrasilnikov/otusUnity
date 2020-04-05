using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{

    public CinemachineVirtualCamera CoinCamera;
    public GameObject Menu;

    Character character;

    // Start is called before the first frame update
    void Awake()
    {
        character = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        character.Move(Input.GetAxis("Horizontal"));

        if (Input.GetButtonDown("Jump"))
        {
            character.Jump();
        }

        //Debug.Log(rigidBody2D.velocity);
    }

    private void OnDestroy()
    {
        Menu.SetActive(true);
    }
}
