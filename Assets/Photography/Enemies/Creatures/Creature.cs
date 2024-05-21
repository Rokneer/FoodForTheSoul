using System;
using UnityEngine;

public class Creature : PhotoObject
{
    public override void WasPhotographed()
    {
        KillCreature();
        base.WasPhotographed();
    }

    private void KillCreature()
    {
        Debug.Log($"Killed {data.objectName}!");
    }
}
