using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObstacle : MonoBehaviour
{
    Material _material;
    float curAlpha;
    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
    }

    Coroutine coroutine;
    public void FadeIn()
    {
        //不透明
        if (coroutine == null)
        {
            coroutine=StartCoroutine( FadeCoroutine(1));
        }
        else if (onFadeOut && !onFadeIn)//在淡出 或者 不在淡入
        {
            StopAllCoroutines();
            coroutine=StartCoroutine( FadeCoroutine(1));
        }
    }

    public void FadeOut()
    {
        if (coroutine == null)
        {
            coroutine=StartCoroutine( FadeCoroutine(0.3f));
        }
        else if (onFadeIn && !onFadeOut)
        {
            StopAllCoroutines();
            coroutine=StartCoroutine( FadeCoroutine(0.3f));
        }
    }
        

    bool onFadeIn = false;
    bool onFadeOut = false;
    IEnumerator FadeCoroutine(float targetAlpha)
    {
        if(targetAlpha==1)onFadeIn = true;
        else onFadeOut = true;
        
        Debug.Log("onFade "+targetAlpha);
        float timer = 0;
        float duration = 0.3f;
        float t = 0;
        float currentAlpha = _material.GetFloat("_Alpha");
        
        while (true)
        {
            if (timer > duration)
            {
                onFadeIn = false;
                onFadeOut = false;
                coroutine = null;
                yield break;
            }
            timer += Time.deltaTime;
            t = timer / duration;
            curAlpha = Mathf.Lerp(currentAlpha, targetAlpha, t);
            _material.SetFloat("_Alpha",curAlpha);
            yield return null;
        }
    }
}
