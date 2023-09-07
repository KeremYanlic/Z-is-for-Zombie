using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InitialiseZombieEvent : MonoBehaviour
{
    public event Action<InitialiseZombieEvent, InitialiseZombieEventArgs> OnInitialiseZombie;

    public void CallInitialiseZombieEvent(ZombieDetailsSO zombieDetailsSO, Vector3 spawnPosition)
    {
        OnInitialiseZombie?.Invoke(this, new InitialiseZombieEventArgs() { zombieDetailsSO = zombieDetailsSO, spawnPosition = spawnPosition });
    }
}
public class InitialiseZombieEventArgs : EventArgs
{
    public ZombieDetailsSO zombieDetailsSO;
    public Vector3 spawnPosition;
}

