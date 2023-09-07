using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponShootEffect_", menuName = "Scriptable Objects/Weapon/Weapon Shoot Effect")]
public class WeaponShootEffectSO : ScriptableObject
{
    #region Header WEAPON SHOOT EFFECT DETAILS
    [Space(10)]
    [Header("WEAPON SHOOT EFFECT DETAILS")]
    #endregion WEAPON SHOOT EFFECT DETAILS

    #region Tooltip
    [Tooltip("The color gradient for the shoot effect. This gradient show the color of particles during their lifetime - from left to right")]
    #endregion
    public Gradient colorGradient;

    #region Tooltip
    [Tooltip("The length of time the particles system is emitting particles")]
    #endregion
    public float duration = .5f;

    #region Tooltip
    [Tooltip("The start particle size for the particle effect")]
    #endregion
    public float startParticleSize = 0.25f;

    #region Tooltip
    [Tooltip("The start particle speed for the particle effect")]
    #endregion
    public float startParticleSpeed = 3f;

    #region Tooltip
    [Tooltip("The particle lifetime for the particle effect")]
    #endregion
    public float startLifetime = 0.5f;

    #region Tooltip
    [Tooltip("The maximum number of particles to be emitted")]
    #endregion
    public int maxParticleNumber = 100;

    #region Tooltip
    [Tooltip("The number of particles emitted per second. If zero it will just be the burst number")]
    #endregion
    public int emissionRate = 100;

    #region Tooltip
    [Tooltip("How many particles should be emitted in the particle effect burst")]
    #endregion
    public int burstParticleNumber = 20;

    #region Tooltip
    [Tooltip("The gravity on the particles - a small negative number will make them float up")]
    #endregion
    public float effectGravity = -0.8f;

    #region Tooltip
    [Tooltip("The sprite for the particle effect. If none is specified then the default particle sprite will be used")]
    #endregion
    public Sprite sprite;

    #region Tooltip
    [Tooltip("The min velocity for the particle over its lifetime. A random value between min and max will be generated.")]
    #endregion
    public Vector3 velocityOverLifetimeMin;

    #region Tooltip
    [Tooltip("The max velocity for the particle over its lifetime. A random value between min and max will be generated.")]
    #endregion
    public Vector3 velocityOverLifetimeMax;

    #region Tooltip
    [Tooltip("weaponShootEffectPrefab contains the particle system for the shoot effect - and is configured by the weaponShootEffectSO")]
    #endregion
    public GameObject weaponShootEffectPrefab;

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        UtilsClass.ValidateCheckPositiveValue(this, nameof(duration), duration, false);
        UtilsClass.ValidateCheckPositiveValue(this, nameof(startParticleSize), startParticleSize, false);
        UtilsClass.ValidateCheckPositiveValue(this, nameof(startParticleSpeed), startParticleSpeed, false);
        UtilsClass.ValidateCheckPositiveValue(this, nameof(startLifetime), startLifetime, false);
        UtilsClass.ValidateCheckPositiveValue(this, nameof(maxParticleNumber), maxParticleNumber, false);
        UtilsClass.ValidateCheckPositiveValue(this, nameof(emissionRate), emissionRate, true);
        UtilsClass.ValidateCheckPositiveValue(this, nameof(burstParticleNumber), burstParticleNumber, true);
        UtilsClass.ValidateCheckNullValue(this, nameof(weaponShootEffectPrefab), weaponShootEffectPrefab);
    }
#endif
    #endregion

}