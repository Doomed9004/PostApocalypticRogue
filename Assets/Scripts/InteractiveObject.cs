using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour,IInteractive
{
    virtual public void Interaction()
    {
        Debug.Log(this.gameObject+" interaction");
    }
}