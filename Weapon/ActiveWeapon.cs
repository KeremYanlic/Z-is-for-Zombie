using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AimThroughSightEvent))]
[RequireComponent(typeof(SetActiveWeaponEvent))]
[RequireComponent(typeof(DisableWeaponEvent))]
[DisallowMultipleComponent]
public class ActiveWeapon : MonoBehaviour
{
    #region Tooltip
    [Tooltip("Populate with the SpriteRenderer on the child Weapon gameobject")]
    #endregion
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;

    #region Tooltip
    [Tooltip("Populate with the Transform on the child Weapon gameobject")]
    #endregion
    [SerializeField] private Transform weaponTransform;
    private PolygonCollider2D weaponPolygonCollider2D;

    #region Tooltip
    [Tooltip("Populate with the Transform on the WeaponShootPosition gameobject")]
    #endregion
    [SerializeField] private Transform weaponShootPositionTransform;
   

    #region Tooltip
    [Tooltip("Populate with the Transform on the WeaponLightPosition gameobject")]
    #endregion
    [SerializeField] private Transform weaponLightPositionRight;
    [SerializeField] private Transform weaponLigthPositionLeft;

    #region Tooltip
    [Tooltip("Populate with the Transform on the WeaponEffectPosition gameobject")]
    #endregion
    [SerializeField] private Transform weaponEffectPositionTransform;

    private AimThroughSightEvent aimThroughSightEvent;
    private SetActiveWeaponEvent setActiveWeaponEvent;
    private DisableWeaponEvent disableWeaponEvent;
    private Weapon currentWeapon;


    private void Awake()
    {
        // Load components
        aimThroughSightEvent = GetComponent<AimThroughSightEvent>();
        setActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
        disableWeaponEvent = GetComponent<DisableWeaponEvent>();
    }
    private void OnEnable()
    {
        //Subscribe to set active weapon event
        setActiveWeaponEvent.OnSetActiveWeapon += SetWeaponEvent_OnSetActiveWeapon;

        //Subscribe to aim through sight event
        aimThroughSightEvent.OnAimThroughSight += AimThroughSightEvent_OnAimThroughSight;

        //Subscribe to disable weapon event
        disableWeaponEvent.OnDisableWeapon += DisableWeaponEvent_OnDisableWeapon;

        //Subscribe to enable weapon event
        setActiveWeaponEvent.OnEnableWeapon += SetWeaponEvent_OnEnableWeapon;
    }

   

    private void OnDisable()
    {
        //Unsubscribe from set active weapon event
        setActiveWeaponEvent.OnSetActiveWeapon -= SetWeaponEvent_OnSetActiveWeapon;

        //Unsubscribe from aim through sight event
        aimThroughSightEvent.OnAimThroughSight -= AimThroughSightEvent_OnAimThroughSight;

        //Unubscribe from disable weapon event
        disableWeaponEvent.OnDisableWeapon -= DisableWeaponEvent_OnDisableWeapon;

        //Unsubscribe from enable weapon event
        setActiveWeaponEvent.OnEnableWeapon -= SetWeaponEvent_OnEnableWeapon;

    }
    private void Start()
    {
        //Set polygonCollider2D
        weaponPolygonCollider2D = weaponTransform.GetComponent<PolygonCollider2D>();
    }
    private void SetWeaponEvent_OnSetActiveWeapon(SetActiveWeaponEvent setActiveWeaponEvent, SetActiveWeaponEventArgs setActiveWeaponEventArgs)
    {
        SetWeapon(setActiveWeaponEventArgs.weapon);
    }

    private void AimThroughSightEvent_OnAimThroughSight(AimThroughSightEvent aimThroughSightEvent, AimThrouhSightEventArgs aimThrouhSightEventArgs)
    {
        if (aimThrouhSightEventArgs.hasGunScope)
        {
            weaponSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
        else
        {
            weaponSpriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        }
    }
    private void DisableWeaponEvent_OnDisableWeapon(DisableWeaponEvent obj)
    {
        //Disable weapon sprite renderer
        weaponTransform.GetComponent<SpriteRenderer>().enabled = false;
    }
    private void SetWeaponEvent_OnEnableWeapon(SetActiveWeaponEvent obj)
    {
        //Enable weapon sprite renderer
        weaponTransform.GetComponent<SpriteRenderer>().enabled = true;
    }
    private void SetWeapon(Weapon weapon)
    {
        weaponTransform.gameObject.SetActive(true);

        currentWeapon = weapon;

        // Set current weapon sprite
        weaponSpriteRenderer.sprite = currentWeapon.weaponDetails.weaponSprite;

        // If the weapon has a polygon collider and a sprite then set it to the weapon sprite physics shape
        if (weaponPolygonCollider2D != null && weaponSpriteRenderer.sprite != null)
        {
            // Get sprite physics shape - this returns the sprite physics shape points as a list of Vector2s
            List<Vector2> spritePhysicsShapePointsList = new List<Vector2>();
            weaponSpriteRenderer.sprite.GetPhysicsShape(0, spritePhysicsShapePointsList);

            // Set polygon collider on weapon to pick up physics shap for sprite - set collider points to sprite physics shape points
            weaponPolygonCollider2D.points = spritePhysicsShapePointsList.ToArray();
        }

        // Set weapon shoot position
        weaponShootPositionTransform.localPosition = currentWeapon.weaponDetails.weaponShootPosition;

        // Set weapon light position
        weaponLightPositionRight.localPosition = currentWeapon.weaponDetails.weaponLightPosition;
        weaponLigthPositionLeft.localPosition = currentWeapon.weaponDetails.weaponLightPosition;
    }

    public AmmoDetailsSO GetCurrentAmmo()
    {
        return currentWeapon.weaponDetails.weaponCurrentAmmo;
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public Vector3 GetShootPosition()
    {
        return weaponShootPositionTransform.position;
    }

    public Vector3 GetShootEffectPosition()
    {
        return weaponEffectPositionTransform.position;
    }

    public void RemoveCurrentWeapon()
    {
        currentWeapon = null;
    }


    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        UtilsClass.ValidateCheckNullValue(this, nameof(weaponSpriteRenderer), weaponSpriteRenderer);
        UtilsClass.ValidateCheckNullValue(this, nameof(weaponTransform), weaponTransform);
        UtilsClass.ValidateCheckNullValue(this, nameof(weaponShootPositionTransform), weaponShootPositionTransform);
        UtilsClass.ValidateCheckNullValue(this, nameof(weaponEffectPositionTransform), weaponEffectPositionTransform);
    }
#endif
    #endregion
}