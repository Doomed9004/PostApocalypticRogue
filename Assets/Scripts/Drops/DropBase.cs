using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBase : MonoBehaviour,IDrop
{
    //[SerializeField] protected float t=0.3f;
    [SerializeField] protected float duration=0.3f;
    

    public virtual void PickUp(IPicker picker, Transform trans)
    {
        gameObject.layer = 0;//切换层级 拾取者不会再识别到这个物体
        StartCoroutine(PickingCoroutine(picker,trans));
        Debug.Log("Picked up");
    }
    
    protected virtual IEnumerator PickingCoroutine(IPicker picker,Transform trans)
    {
        //实现移动并销毁的动画
        float timer = 0;
        float t = 0;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer >= duration)break;
            t=timer / duration;
            this.transform.position = Vector3.Lerp(this.transform.position,trans.position,t);
            yield return null;
        }

        Destroy(gameObject);
    }
}
