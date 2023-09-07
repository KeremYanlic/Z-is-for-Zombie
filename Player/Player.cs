using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


#region REQUIRE COMPONENTS

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerStatus))]
[RequireComponent(typeof(GetDamageEvent))]
[RequireComponent(typeof(MovementByVelocityEvent))]
[RequireComponent(typeof(MovementByVelocity))]
[RequireComponent(typeof(MovementToPositionEvent))]
[RequireComponent(typeof(MovementToPosition))]
[RequireComponent(typeof(IdleEvent))]
[RequireComponent(typeof(Idle))]
[RequireComponent(typeof(RunEvent))]
[RequireComponent(typeof(AimWeaponEvent))]
[RequireComponent(typeof(AimWeapon))]
[RequireComponent(typeof(FireWeaponEvent))]
[RequireComponent(typeof(FireWeapon))]
[RequireComponent(typeof(SetActiveWeaponEvent))]
[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(WeaponFiredEvent))]
[RequireComponent(typeof(ReloadWeaponEvent))]
[RequireComponent(typeof(ReloadWeapon))]
[RequireComponent(typeof(WeaponReloadedEvent))]
[RequireComponent(typeof(AimThroughSightEvent))]
[RequireComponent(typeof(AimThroughSight))]
[RequireComponent(typeof(ActiveCar))]
[RequireComponent(typeof(SetActiveCarEvent))]
[RequireComponent(typeof(AnimatePlayer))]
[RequireComponent(typeof(SortingGroup))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[DisallowMultipleComponent]
#endregion REQUIRE COMPONENTS

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerDetailsSO playerDetails;

    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public PlayerStatus playerHealth;
    [HideInInspector] public GetDamageEvent getDamageEvent;
    [HideInInspector] public MovementByVelocityEvent movementByVelocityEvent;
    [HideInInspector] public MovementToPositionEvent movementToPositionEvent;
    [HideInInspector] public IdleEvent idleEvent;
    [HideInInspector] public RunEvent runEvent;
    [HideInInspector] public AimWeaponEvent aimWeaponEvent;
    [HideInInspector] public FireWeaponEvent fireWeaponEvent;
    [HideInInspector] public SetActiveWeaponEvent setActiveWeaponEvent;
    [HideInInspector] public ActiveWeapon activeWeapon;
    [HideInInspector] public WeaponFiredEvent weaponFiredEvent;
    [HideInInspector] public ReloadWeaponEvent reloadWeaponEvent;
    [HideInInspector] public WeaponReloadedEvent weaponReloadedEvent;
    [HideInInspector] public AimThroughSightEvent aimThroughSightEvent;
    [HideInInspector] public ActiveCar activeCar;
    [HideInInspector] public SetActiveCarEvent setActiveCarEvent;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;

    [SerializeField] private SpriteRenderer handSpriteRenderer;
    [SerializeField] private SpriteRenderer noWeaponHandSpriteRenderer;
    public List<Weapon> weaponList = new List<Weapon>();

    private void Awake()
    {
        // Load components

        playerController = GetComponent<PlayerController>();
        playerHealth = GetComponent<PlayerStatus>();
        getDamageEvent = GetComponent<GetDamageEvent>();
        movementByVelocityEvent = GetComponent<MovementByVelocityEvent>();
        movementToPositionEvent = GetComponent<MovementToPositionEvent>();
        idleEvent = GetComponent<IdleEvent>();
        runEvent = GetComponent<RunEvent>();
        aimWeaponEvent = GetComponent<AimWeaponEvent>();
        fireWeaponEvent = GetComponent<FireWeaponEvent>();
        setActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
        activeWeapon = GetComponent<ActiveWeapon>();
        weaponFiredEvent = GetComponent<WeaponFiredEvent>();
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        weaponReloadedEvent = GetComponent<WeaponReloadedEvent>();
        aimThroughSightEvent = GetComponent<AimThroughSightEvent>();
        activeCar = GetComponent<ActiveCar>();
        setActiveCarEvent = GetComponent<SetActiveCarEvent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }


    private void OnEnable()
    {
        aimThroughSightEvent.OnAimThroughSight += AimThroughSightEvent_OnAimThroughSight;
    }
    private void OnDisable()
    {
        aimThroughSightEvent.OnAimThroughSight -= AimThroughSightEvent_OnAimThroughSight;
    }

    private void AimThroughSightEvent_OnAimThroughSight(AimThroughSightEvent arg1, AimThrouhSightEventArgs aimThrouhSightEventArgs)
    {
        if (aimThrouhSightEventArgs.hasGunScope)
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            handSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            noWeaponHandSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
        else
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
            handSpriteRenderer.maskInteraction = SpriteMaskInteraction.None;
            noWeaponHandSpriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        }
    }

    /// <summary>
    /// Initialize the player
    /// </summary>
    public void Initialize(PlayerDetailsSO playerDetails)
    {
        this.playerDetails = playerDetails;

        //Set the player starting health
        playerHealth.InitializePlayerHealth(playerDetails);

        //Set the player starting stamina
        playerHealth.InitializePlayerStamina(playerDetails);

        //Create player starting weapons
        CreatePlayerStartingWeapons();


    }



    /// <summary>
    /// Set the player starting weapon
    /// </summary>
    private void CreatePlayerStartingWeapons()
    {
        // Clear list
        weaponList.Clear();

        // Populate weapon list from starting weapons
        foreach (WeaponDetailsSO weaponDetails in playerDetails.StartingWeaponList)
        {
            // Add weapon to player
            AddWeaponToPlayer(weaponDetails);
        }
    }



    /// <summary>
    /// Returns the player position
    /// </summary>
    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }


    /// <summary>
    /// Add a weapon to the player weapon dictionary
    /// </summary>
    public Weapon AddWeaponToPlayer(WeaponDetailsSO weaponDetails)
    {
        Weapon weapon = new Weapon()
        {
            weaponDetails = weaponDetails,
            weaponReloadTimer = 0f,
            weaponClipRemainingAmmo = weaponDetails.weaponClipAmmoCapacity,
            weaponRemainingAmmo = weaponDetails.weaponAmmoCapacity,
            isWeaponReloading = false
        };

        // Add the weapon to the list
        weaponList.Add(weapon);

        // Set weapon position in list
        weapon.weaponListPosition = weaponList.Count;

        // Set the added weapon as active
        setActiveWeaponEvent.CallSetActiveWeaponEvent(weapon);

        return weapon;
    }

}