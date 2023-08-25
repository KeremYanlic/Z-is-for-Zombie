using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour, IFireable
{
    #region Tooltip
    [Tooltip("Populate with child trail renderer component")]
    #endregion
    [SerializeField] private TrailRenderer trailRenderer;

    private float ammoRange = 0f; //the range of each ammo
    private float ammoSpeed;
    private Vector3 fireDirectionVector;
    private float fireDirectionAngle;
    private SpriteRenderer spriteRenderer;
    private AmmoDetailsSO ammoDetailsSO;
    private bool isAmmoMaterialSet = false;

    private Player player;

    private void Awake()
    {
        //Load component
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameManager.Instance.GetPlayer();
    }

    private void OnEnable()
    {
        //Subscribe to aim through sight event
        player.aimThroughSightEvent.OnAimThroughSight += AimThroughSightEvent_OnAimThroughSight;


    }
    private void OnDisable()
    {
        //Unsubscribe from aim through sight event
        player.aimThroughSightEvent.OnAimThroughSight -= AimThroughSightEvent_OnAimThroughSight;
    }

    private void AimThroughSightEvent_OnAimThroughSight(AimThroughSightEvent aimThroughSightEvent, AimThrouhSightEventArgs aimThrouhSightEventArgs)
    {
        if (aimThrouhSightEventArgs.hasGunScope)
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            trailRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
        else
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
            trailRenderer.maskInteraction = SpriteMaskInteraction.None;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAmmoMaterialSet)
        {
            SetAmmoMaterial(ammoDetailsSO.ammoMaterial);
            isAmmoMaterialSet = true;
        }

        //Calculate distance vector to move ammo
        Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime;

        transform.position += distanceVector;

        ammoRange -= distanceVector.magnitude;

        if (ammoRange <= 0f)
        {
            DisableAmmo();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("collision"))
        {
            DisableAmmo();
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("collision"))
        {
            DisableAmmo();
        }
    }


    //<summary>
    //Initialise the ammo being fired - using the ammo details,the aimangle,weaponAngle and weaponAimDirectionVector
    //</summary>
    public void InitialiseAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, float ammoSpeed, Vector3 weaponAimDirectionVector)
    {
        #region Ammo
        this.ammoDetailsSO = ammoDetails;

        //Set Fire Direction
        SetFireDirection(ammoDetails, aimAngle, weaponAimAngle, weaponAimDirectionVector);

        //Set ammo sprite
        spriteRenderer.sprite = ammoDetails.ammoSprite;

        SetAmmoMaterial(ammoDetails.ammoMaterial);
        isAmmoMaterialSet = true;

        //Set ammo range
        ammoRange = ammoDetails.ammoRange;

        //Set ammo speed
        this.ammoSpeed = ammoSpeed;

        //Activate game object
        gameObject.SetActive(true);
        #endregion

        #region
        if (ammoDetails.isAmmoTrail)
        {
            trailRenderer.gameObject.SetActive(true);
            trailRenderer.emitting = true;
            trailRenderer.material = ammoDetails.ammoTrailMaterial;
            trailRenderer.startWidth = ammoDetails.ammoTrailStartWidth;
            trailRenderer.endWidth = ammoDetails.ammoTrailEndWidth;
            trailRenderer.time = ammoDetails.ammoTrailTime;
        }
        else
        {
            trailRenderer.emitting = false;
            trailRenderer.gameObject.SetActive(false);
        }
        #endregion
    }

    //<summary>
    //Set ammo fire direction and angle based on the input angle and direction adjusted by the random spread
    //</summary>

    private void SetFireDirection(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        //Calculate random spread angle between min and max
        float randomSpread = Random.Range(ammoDetails.ammoSpreadMin, ammoDetails.ammoSpreadMax);

        //Get a random spread toggle of 1 or -1
        float spreadToggle = Random.Range(0, 2) * 2 - 1;

        if (weaponAimDirectionVector.magnitude > Settings.weaponAngleDistance)
        {
            fireDirectionAngle = weaponAimAngle;
        }
        else
        {
            fireDirectionAngle = aimAngle;
        }

        // Adjust ammo fire angle by random spread
        fireDirectionAngle += randomSpread * spreadToggle;

        //Set ammo rotation
        transform.eulerAngles = new Vector3(0, 0, fireDirectionAngle);

        //Set ammo fire direction
        fireDirectionVector = UtilsClass.GetDirectionVectorFromAngle(fireDirectionAngle);
    }


    //<summary>
    //Set ammo material
    //</summary>
    private void SetAmmoMaterial(Material ammoMaterial)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.material = ammoMaterial;
        }
    }
    //<summary>
    //Disable ammo
    //</summary>
    private void DisableAmmo()
    {
        gameObject.SetActive(false);
    }
    public GameObject GetGameObject()
    {
        return this.gameObject;
    }


}
