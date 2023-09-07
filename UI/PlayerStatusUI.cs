using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class PlayerStatusUI : MonoBehaviour
{
    //Components
    private Player player;
    private PlayerStatus playerHealth;

    private const float DAMAGED_HEALTH_SHRINK_TIMER_MAX = .5f;

    private float damagedHealthShrinkTimer;

    #region Header OBJECT REFERENCES
    [Space(10)]
    [Header("OBJECT REFERENCES")]
    #endregion

    #region Tooltip
    [Tooltip("Health bar image in order to control the health bar UI")]
    #endregion
    [SerializeField] private Image healthBar;

    #region Tooltip
    [Tooltip("Health damaged bar image in order to add some visual effect")]
    #endregion
    [SerializeField] private Image healthDamagedBar;

    #region Tooltip
    [Tooltip("Stamina bar image in order to control the stamina bar UI")]
    #endregion
    [SerializeField] private Image staminaBar;


    private void Awake()
    {
        player = GameManager.Instance.GetPlayer();
        playerHealth = player.GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        damagedHealthShrinkTimer -= Time.deltaTime;

        if (damagedHealthShrinkTimer < 0f)
        {
            if (healthBar.fillAmount < healthDamagedBar.fillAmount)
            {
                float shrinkSpeed = 1f;
                healthDamagedBar.fillAmount -= shrinkSpeed * Time.deltaTime;
            }
        }
        staminaBar.fillAmount = (playerHealth.GetCurrentStamina() / 100);
    }
    private void OnEnable()
    {
        //Subscribe to get damage event
        player.getDamageEvent.OnGetDamage += GetDamageEvent_OnGetDamage;


    }
    private void OnDisable()
    {
        //Unsubcribe from get damage event
        player.getDamageEvent.OnGetDamage -= GetDamageEvent_OnGetDamage;


    }

    private void GetDamageEvent_OnGetDamage(GetDamageEvent getDamageEvent, GetDamageEventArgs getDamageEventArgs)
    {
        damagedHealthShrinkTimer = DAMAGED_HEALTH_SHRINK_TIMER_MAX;


        healthBar.fillAmount -= (float)(getDamageEventArgs.damageAmount / 100);
    }


    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        UtilsClass.ValidateCheckNullValue(this, nameof(healthBar), healthBar);
        UtilsClass.ValidateCheckNullValue(this, nameof(staminaBar), staminaBar);
    }
#endif
    #endregion
}
