using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume : MonoBehaviour
{
    public event Action<VolumeType> PlayerEnter ;
    public event Action<VolumeType> PlayerExit;
    public VolumeType vType;
    public enum VolumeType
    {
        Safe
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerEnter?.Invoke(vType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerExit?.Invoke(vType);
        }
    }
}
