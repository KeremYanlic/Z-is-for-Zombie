using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ZombieSpawnEvent : MonoBehaviour
{
    public event Action<ZombieSpawnEvent> OnZombieSpawn;

    private bool hasSpawned = false;

    public void CallZombieSpawnEvent()
    {
        if (!hasSpawned)
        {
            OnZombieSpawn?.Invoke(this);
            hasSpawned = true;
        }

    }
}
