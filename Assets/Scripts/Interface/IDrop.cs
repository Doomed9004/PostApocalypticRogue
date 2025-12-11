using UnityEngine;

public interface IDrop
{
    public float Value { get; }
    public void PickUp(IPicker picker,Transform trans);//捡起物体的对象
}
public interface IPicker
{
    public void PickedUp(float value);//拾取物体后发生什么
}