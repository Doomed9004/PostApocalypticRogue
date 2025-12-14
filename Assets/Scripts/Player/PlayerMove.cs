using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]private CharacterController cc;
    private Reader reader;
    PlayerStats playerStats;
    private float MoveSpeed => playerStats.MoveSpeed;
    
    Vector3 moveDir=>new Vector3(reader.InputDirV2.x,0,reader.InputDirV2.y);

    private void Start()
    {
        reader = FindObjectOfType<Reader>();
        cc = GetComponent<CharacterController>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        MoveControl();
    }

    void MoveControl()
    {
        if (reader.InputDirV2 != Vector2.zero)
        {
            cc.Move(moveDir * MoveSpeed * Time.deltaTime);
        }
    }
}
