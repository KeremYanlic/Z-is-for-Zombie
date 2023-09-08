using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    //Base speed for player animations
    public static int baseSpeedForPlayerAnimations = 10;

    //Base distance for choosing weapon angle degree or player angle degree
    public static float weaponAngleDistance = 3.5f;


    #region Animation Hashes
    public static int aimUp = Animator.StringToHash("aimUp");
    public static int aimUpRight = Animator.StringToHash("aimUpRight");
    public static int aimUpLeft = Animator.StringToHash("aimUpLeft");
    public static int aimRight = Animator.StringToHash("aimRight");
    public static int aimLeft = Animator.StringToHash("aimLeft");
    public static int aimDown = Animator.StringToHash("aimDown");
    public static int isIdle = Animator.StringToHash("isIdle");
    public static int isMoving = Animator.StringToHash("isMoving");
    public static int rollUp = Animator.StringToHash("rollUp");
    public static int rollLeft = Animator.StringToHash("rollLeft");
    public static int rollRight = Animator.StringToHash("rollRight");
    public static int rollDown = Animator.StringToHash("rollDown");
    #endregion

    #region Tags
    public static string playerTag = "Player";
    public static string carTag = "Car";
    public static string collisionTag = "collision";
    public static string zombieTag = "Zombie";
    public static string zombieSpawnerTag = "EnemySpawner";
    #endregion

    #region Layers
    public static int playerLayer = LayerMask.GetMask("Player");
    public static int carLayer = LayerMask.GetMask("Car");
    public static int zombieLayer = LayerMask.GetMask("Zombie");
    #endregion

    //Space for adjusting ammo icons position on the UI
    public static float uiAmmoIconSpacing = 4f;

    //This radius is for checking the cars that are close to the player.
    public static float carDetectRadius = 2f;

    //This number is gonna be used for calculating the car's hit damage when it crash with a object which has a collision tag
    public static int carHitDamageDivider = 16;
}
