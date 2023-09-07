using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ZombieStatus))]
[RequireComponent(typeof(InitialiseZombieEvent))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Zombie : MonoBehaviour
{
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public InitialiseZombieEvent initialiseZombieEvent;
    [HideInInspector] public FieldSpawnerController fieldSpawnerController;
    [HideInInspector] public ZombieStatus zombieStatus;
    [SerializeField] private ZombieDetailsSO zombieDetailsSO;

    private void Awake()
    {
        //Load components
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialiseZombieEvent = GetComponent<InitialiseZombieEvent>();
        zombieStatus = GetComponent<ZombieStatus>();
    }

    private void OnEnable()
    {
        //Subscribe to initialise zombie event
        initialiseZombieEvent.OnInitialiseZombie += InitialiseZombieEvent_OnInitialiseZombie;
    }
    private void OnDisable()
    {
        //Unsubscribe to initialise zombie event
        initialiseZombieEvent.OnInitialiseZombie -= InitialiseZombieEvent_OnInitialiseZombie;
    }

    private void InitialiseZombieEvent_OnInitialiseZombie(InitialiseZombieEvent initialiseZombieEvent, InitialiseZombieEventArgs initialiseZombieEventArgs)
    {

    }


    public void InitialiseZombie(ZombieDetailsSO zombieDetailsSO, Vector3 spawnPosition)
    {
        initialiseZombieEvent.CallInitialiseZombieEvent(zombieDetailsSO, spawnPosition);
    }


    //<summary>
    //Set the field spawner controller. We want updated spawnable positions from zombie's area for patrolling functionality.
    public void SetFieldSpawnController(FieldSpawnerController fieldSpawnerController)
    {
        this.fieldSpawnerController = fieldSpawnerController;
    }

    public ZombieDetailsSO GetZombieDetailsSO()
    {
        return zombieDetailsSO;
    }


}
