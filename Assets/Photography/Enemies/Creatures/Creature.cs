using UnityEngine;

public class Creature : PhotoObject
{
    public override void WasPhotographed()
    {
        KillCreature();
        CreatureSpawnManager.Instance.RemoveObject(gameObject);
    }

    private void KillCreature()
    {
        Debug.Log($"Killed {data.label}!");
    }
}
