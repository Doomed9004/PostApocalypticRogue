using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]private CharacterController cc;
    private Reader reader;
    [SerializeField]private float moveSpeed;
    
    Vector3 moveDir=>new Vector3(reader.InputDir.x,0,reader.InputDir.y);

    private void Start()
    {
        reader = Reader.Ins;
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MoveControl();
    }

    void MoveControl()
    {
        if (reader.InputDir != Vector2.zero)
        {
            cc.Move(moveDir * moveSpeed * Time.deltaTime);
        }
    }
}
