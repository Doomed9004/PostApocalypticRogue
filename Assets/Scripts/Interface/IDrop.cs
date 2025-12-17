using System;
using UnityEngine;

public interface IDrop
{
    public void PickUp(IPicker picker,Transform trans);//捡起物体的对象
}
public interface IPicker
{
    public void PickedUp(EventArgs e);//拾取物体后发生什么
}

public class EnergyEventArgs : EventArgs
{
    public EnergyEventArgs(float energy)
    {
        this.energy = energy;
    }
    public float energy;
}
public class ModuleEventArgs : EventArgs
{
    public ModuleEventArgs(ModuleItem item)
    {
        this.item = item;
    }

    public ModuleItem item;
}