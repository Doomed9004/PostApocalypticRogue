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

    //地面检测
    [SerializeField] Transform foot;
    [SerializeField] float checkRadius=0.1f;
    [SerializeField]LayerMask groundLayer;
    
    bool IsGrounded => Physics.CheckSphere(foot.position,checkRadius,groundLayer);

    //下落
    [SerializeField] float gravity = -9.8f;
    Vector3 velocity;
    
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

    private void FixedUpdate()
    {
        if(IsGrounded&&velocity.y<0)
        {
            velocity.y = 0;
            return;
        }
        
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }

    void MoveControl()
    {
        if (reader.InputDirV2 != Vector2.zero)
        {
            cc.Move(moveDir * MoveSpeed * Time.deltaTime);
        }
    }
}
