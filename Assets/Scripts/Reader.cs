using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reader : MonoBehaviour
{
    public GameInputAction inputAction;
    public static Reader Ins;

    private void Awake()
    {
        if (Ins == null)
        {
            Ins = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        inputAction=new GameInputAction();
        inputAction.Enable();
    }
    private void OnDisable()
    {
        inputAction?.Disable();
    }

    private void OnDestroy()
    {
        inputAction?.Disable();
    }

    public Vector2 InputDir => inputAction.Gameplay.Move.ReadValue<Vector2>();
    public bool Aim => inputAction.Gameplay.Aim.IsPressed();
    public bool Shoot => inputAction.Gameplay.Shoot.IsPressed();
    public bool Interactive => inputAction.Gameplay.Interactive.WasReleasedThisFrame();
}
