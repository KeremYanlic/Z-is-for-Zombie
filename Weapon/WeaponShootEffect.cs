using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class WeaponShootEffect : MonoBehaviour
{
    private ParticleSystem shootEffectParticleSystem;

    private void Awake()
    {
        //Load components
        shootEffectParticleSystem = GetComponent<ParticleSystem>();
    }
    public void SetShootEffect(WeaponShootEffectSO weaponShootEffectSO, float aimAngle)
    {
        //Set shoot effect color gradient
        SetShootEffectColorGradient(weaponShootEffectSO.colorGradient);

        //Set shoot effect particle system starting values
        SetShootEffectParticleStartingValues(
            weaponShootEffectSO.duration,
            weaponShootEffectSO.startParticleSize,
            weaponShootEffectSO.startParticleSpeed,
            weaponShootEffectSO.startLifetime,
            weaponShootEffectSO.effectGravity,
            weaponShootEffectSO.maxParticleNumber
            );

        //Set shoot effect particle system particle burst, particle number
        SetShootEffectParticleEmission(weaponShootEffectSO.emissionRate, weaponShootEffectSO.burstParticleNumber);

        //Set emitter rotation
        SetEmitterRotation(aimAngle);

        //Set shoot effect particle sprite
        SetShootEffectParticleSprite(weaponShootEffectSO.sprite);

        //Set shoot effect lifetime min and max velocities
        SetShootEffectVelocityOverLifeTime(weaponShootEffectSO.velocityOverLifetimeMin, weaponShootEffectSO.velocityOverLifetimeMax);
    }


    //<summary>
    //Set the shoot effect particle system color gradient
    //</summary>
    private void SetShootEffectColorGradient(Gradient colorGradient)
    {
        //Set color gradient
        ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = shootEffectParticleSystem.colorOverLifetime;
        colorOverLifetimeModule.color = colorGradient;
    }
    //<summary>
    //Set the shoot effect particle system starting values
    //</summary>
    private void SetShootEffectParticleStartingValues(float duration, float startParticleSize, float startParticleSpeed, float startLifetime, float effectGravity, int maxParticleNumber)
    {
        ParticleSystem.MainModule mainModule = shootEffectParticleSystem.main;
        //Set particle system duration
        mainModule.duration = duration;

        //Set particle system start size
        mainModule.startSize = startParticleSize;

        //Set particle system start speed
        mainModule.startSpeed = startParticleSpeed;

        //Set particle system start life time
        mainModule.startLifetime = startLifetime;

        //Set particle system gravity modifier
        mainModule.gravityModifier = effectGravity;

        //Set particle system max particles
        mainModule.maxParticles = maxParticleNumber;
    }
    //<summary>
    //Set the shoot effect particle system particle burst,particle number
    //</summary>
    private void SetShootEffectParticleEmission(int emissionRate, int burstParticleNumber)
    {
        ParticleSystem.EmissionModule emissionModule = shootEffectParticleSystem.emission;

        //Set particle burst number
        ParticleSystem.Burst burst = new ParticleSystem.Burst(0f, burstParticleNumber);
        emissionModule.SetBurst(0, burst);

        //Set particle emission rate
        emissionModule.rateOverTime = emissionRate;
    }
    // <summary>
    // Set the rotation of the emmitter to match the aim angle
    // </summary>
    private void SetEmitterRotation(float aimAngle)
    {
        transform.eulerAngles = new Vector3(0f, 0f, aimAngle);
    }

    // <summary>
    // Set the shoot effect particle sprite
    // </summary>
    private void SetShootEffectParticleSprite(Sprite sprite)
    {
        ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule = shootEffectParticleSystem.textureSheetAnimation;
        textureSheetAnimationModule.SetSprite(0, sprite);
    }

    // <summary>
    // Set shoot effect velocity over lifetime
    // </summary>
    private void SetShootEffectVelocityOverLifeTime(Vector3 velocityOverLifetimeMin, Vector3 velocityOverLifetimeMax)
    {
        ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = shootEffectParticleSystem.velocityOverLifetime;

        //Define min max X velocity
        ParticleSystem.MinMaxCurve minMaxCurveX = new ParticleSystem.MinMaxCurve();
        minMaxCurveX.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveX.constantMin = velocityOverLifetimeMin.x;
        minMaxCurveX.constantMax = velocityOverLifetimeMax.x;
        velocityOverLifetimeModule.x = minMaxCurveX;

        //Define min max Y velocity
        ParticleSystem.MinMaxCurve minMaxCurveY = new ParticleSystem.MinMaxCurve();
        minMaxCurveY.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveY.constantMin = velocityOverLifetimeMin.y;
        minMaxCurveY.constantMax = velocityOverLifetimeMax.y;
        velocityOverLifetimeModule.y = minMaxCurveY;

        //Define min max Z velocity
        ParticleSystem.MinMaxCurve minMaxCurveZ = new ParticleSystem.MinMaxCurve();
        minMaxCurveZ.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveZ.constantMin = velocityOverLifetimeMin.z;
        minMaxCurveZ.constantMax = velocityOverLifetimeMax.z;
        velocityOverLifetimeModule.z = minMaxCurveZ;

    }




}
