using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

[RequireComponent(typeof(Zombie))]
public class ZombieController : MonoBehaviour, IZombie
{
    private Zombie zombie;
    private Seeker seeker;
    private AIPath aIPath;
    private AIDestinationSetter aIDestinationSetter;


    private Coroutine zombieVisibilityCoroutine;

    private float moveSpeed;
    private float targetCheckDistance, targetAttackDistance, targetChaseDistance;
    private float slowdownDistance, endReachDistance;
    private float radius;
    private float maxCalculatePathInterval;
    private bool isZombieReadyForAction = false;

    private Vector3 spawnPosition;

    private ZombieEvents currentZombieEvent;

    private float patrollTimer, patrollTimerMax = 10f;
    private float idleTimer, idleTimerMax;
    private Vector3 targetPatrollPos;

    private void Awake()
    {
        //Load components
        zombie = GetComponent<Zombie>();
        seeker = GetComponent<Seeker>();
        aIPath = GetComponent<AIPath>();
        aIDestinationSetter = GetComponent<AIDestinationSetter>();
    }
    private void Start()
    {
        //Arrange some values
        patrollTimer = patrollTimerMax;

        int randomIndex = Random.Range(0, zombie.fieldSpawnerController.GetWalkablePositions().Count);
        targetPatrollPos = zombie.fieldSpawnerController.GetWalkablePositions()[randomIndex];
    }
    private void OnEnable()
    {
        //Subscribe to initialise zombie event
        zombie.initialiseZombieEvent.OnInitialiseZombie += InitialiseZombieEvent_OnInitialiseZombie;
    }
    private void OnDisable()
    {
        //Unsubscribe from initialise zombie event
        zombie.initialiseZombieEvent.OnInitialiseZombie -= InitialiseZombieEvent_OnInitialiseZombie;
    }
    private void Update()
    {
        if (!isZombieReadyForAction) return;

        if (CheckTargetInAttackRange())
        {
            aIPath.canMove = true;
            currentZombieEvent = ZombieEvents.Chase;
        }
        if (currentZombieEvent == ZombieEvents.Chase)
        {
            if (CheckTargetInChaseRange())
            {
                currentZombieEvent = ZombieEvents.Idle;
            }
        }
        if (currentZombieEvent == ZombieEvents.Idle)
        {
            //Zombie idle event
            ZombieIdleEvent();
        }

        if (aIPath.canMove && currentZombieEvent == ZombieEvents.Patrolling)
        {
            //Zombie patrol event
            ZombiePatrolEvent();

        }


    }

    //<summary>
    //Zombie Patrol event
    //</summary>
    private void ZombiePatrolEvent()
    {
        //Decrease patroll timer
        patrollTimer -= Time.deltaTime;
        if (patrollTimer <= 0f)
        {
            //Choose a random index for patrol position
            int randomIndex = Random.Range(0, zombie.fieldSpawnerController.GetWalkablePositions().Count);
            //Choose a random patrol position from random index
            targetPatrollPos = zombie.fieldSpawnerController.GetWalkablePositions()[randomIndex];
            patrollTimer += patrollTimerMax;
        }
        //Start a path to target patrol position
        seeker.StartPath(transform.position, targetPatrollPos);
    }

    private void ZombieIdleEvent()
    {
        //AI cant move for a while after losing track of target
        aIPath.canMove = false;
        //Decrase idle timer
        idleTimer -= Time.deltaTime;

        if (idleTimer <= 0f)
        {
            //AI can move
            aIPath.canMove = true;
            //Go back to patrolling
            currentZombieEvent = ZombieEvents.Patrolling;
            idleTimer += idleTimerMax;
        }
    }


    //<summary>
    //Initialise zombie event handler
    //</summary>
    private void InitialiseZombieEvent_OnInitialiseZombie(InitialiseZombieEvent initialiseZombieEvent, InitialiseZombieEventArgs initialiseZombieEventArgs)
    {
        //Get zombieDetailsSO
        if (initialiseZombieEventArgs.zombieDetailsSO != null)
        {
            ZombieDetailsSO zombieDetailsSO = initialiseZombieEventArgs.zombieDetailsSO;

            SetZombieDetails(zombieDetailsSO, initialiseZombieEventArgs.spawnPosition);

        }
    }

    //<summary>
    //Set zombie details
    //</summary>
    private void SetZombieDetails(ZombieDetailsSO zombieDetailsSO, Vector3 spawnPosition)
    {
        //Set zombie's health
        zombie.zombieStatus.health = zombieDetailsSO.enemyHealth;

        //Set moveSpeed
        moveSpeed = zombieDetailsSO.GetMoveSpeed();

        //Set targetCheckDistance
        targetCheckDistance = zombieDetailsSO.targetCheckDistance;

        //Set targetChaseDistance
        targetChaseDistance = zombieDetailsSO.targetChaseDistance;

        //Set targetAttackDistance
        targetAttackDistance = zombieDetailsSO.targetAttackDistance;

        //Set idle timer and idle timer max
        idleTimerMax = zombieDetailsSO.idleDuration;
        idleTimer = idleTimerMax;

        //Set slowdownDistance
        slowdownDistance = zombieDetailsSO.slowdownDistance;

        //Set endReachDistance
        endReachDistance = zombieDetailsSO.endReachDistance;

        //Set radius
        radius = zombieDetailsSO.radius;

        //Set maxCalculatePathInterval
        maxCalculatePathInterval = zombieDetailsSO.maxCalculatePathInterval;

        //Update AI path variables
        UpdateAIPathVariables(radius, moveSpeed, maxCalculatePathInterval, slowdownDistance, endReachDistance);

        SetAIDestinationTarget();

        //Set spawn position
        this.spawnPosition = spawnPosition;

        isZombieReadyForAction = true;
    }

    //<summary>
    //Update the AI Path variables like speed,raidus etc. after creating the zombie.
    //</summary>
    private void UpdateAIPathVariables(float radius, float moveSpeed, float maxCalculatePathInterval, float slowdownDistance, float endReachDistance)
    {

        //Update ai path radius variable
        aIPath.radius = radius;

        //Update ai path max speed variable
        aIPath.maxSpeed = moveSpeed;

        //Update ai path maximum interval variable
        aIPath.repathRate = maxCalculatePathInterval;

        //Update ai path slowdown distance
        aIPath.slowdownDistance = slowdownDistance;

        //Update ai path end reach distance
        aIPath.endReachedDistance = endReachDistance;

        SetZombieEventAtStart();

        aIPath.canMove = true ? currentZombieEvent == ZombieEvents.Patrolling : currentZombieEvent == ZombieEvents.IdleAtStart;
    }
    //<summary>
    //Set AI Destination target to player
    //</summary>
    private void SetAIDestinationTarget()
    {
        aIDestinationSetter.target = GameManager.Instance.GetPlayer().transform;
    }

    public void ZombieVisibleProcess()
    {

        zombieVisibilityCoroutine = StartCoroutine(ZombieVisibleRoutine());
    }
    public void ZombieInvisibleProcess()
    {

        zombieVisibilityCoroutine = StartCoroutine(ZombieInvisibleRoutine());
    }

    private IEnumerator ZombieVisibleRoutine()
    {
        Color color = zombie.spriteRenderer.material.color;



        while (color.a < 1f)
        {
            color.a += Time.deltaTime * 4f;
            yield return null;
            zombie.spriteRenderer.material.color = color;
        }

    }
    private IEnumerator ZombieInvisibleRoutine()
    {
        Color color = zombie.spriteRenderer.material.color;



        while (color.a > 0f)
        {
            color.a -= Time.deltaTime * 4f;
            yield return null;
            zombie.spriteRenderer.material.color = color;
        }

    }

    private void SetZombieEventAtStart()
    {
        int randomIndex = Random.Range(0, 11);

        currentZombieEvent = randomIndex > 6 ? ZombieEvents.Patrolling : ZombieEvents.IdleAtStart;

    }

    private bool CheckTargetInAttackRange()
    {
        return Vector3.Distance(transform.position, GameManager.Instance.GetPlayer().transform.position) < targetAttackDistance;
    }
    private bool CheckTargetInChaseRange()
    {
        return Vector3.Distance(transform.position, GameManager.Instance.GetPlayer().transform.position) < targetChaseDistance;
    }


    public Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }


}