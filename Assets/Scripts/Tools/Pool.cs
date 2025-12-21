using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T>
{
    Stack<T> stack;
    int defaultCount = 20;
    int maxCount = 100;
    private T t;
    
    Func<T,T> CreateFunc;
    Action<T,Vector3> ActionOnGet;
    Action<T> ActionOnRelease;
    Action<T> ActionOnDestroy;

    public Pool(int defaultCount, int maxCount, T t, Func<T, T> createFunc, Action<T,Vector3> actionOnGet, Action<T> actionOnRelease, Action<T> actionOnDestroy)
    {
        this.defaultCount = defaultCount;
        this.maxCount = maxCount;
        this.t = t;
        stack=new Stack<T>(maxCount);
        CreateFunc = createFunc;
        ActionOnGet = actionOnGet;
        ActionOnRelease = actionOnRelease;
        ActionOnDestroy = actionOnDestroy;
        for (int i = 0; i < defaultCount; i++)
        {
            T temp = CreateFunc.Invoke(t);
            stack.Push(temp);
        }
    }
    
    //保存到对象池中
    public void Release(T go)
    {
        if (stack.Count < maxCount)
        {
            ActionOnRelease.Invoke(go);
            stack.Push(go);
        }
        else
        {
            ActionOnDestroy.Invoke(go);
        }
    }
    
    //取出对象
    public T Get(Vector3 position)
    {
        if (stack.Count > 0)
        {
            T go = stack.Pop();
            ActionOnGet.Invoke(go,position);
            return go;
        }
        //生成新的
        T newGo = CreateFunc.Invoke(t);
        ActionOnGet.Invoke(newGo,position);
        return newGo;
    }
    
}
