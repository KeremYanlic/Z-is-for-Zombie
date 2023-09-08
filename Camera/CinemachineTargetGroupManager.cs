using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CinemachineTargetGroupManager : MonoBehaviour
{
    private CinemachineTargetGroup cinemachineTargetGroup;

    [SerializeField] private Transform cursorTarget;
    [SerializeField] private Transform scopeCursor;

    private Transform playerTarget;

    private CinemachineTargetGroup.Target target_Player;
    private CinemachineTargetGroup.Target target_Cursor;

    private void Awake()
    {
        playerTarget = GameManager.Instance.GetPlayer().transform;

        //Load components
        cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
    }
    private void Start()
    {
        InitializeTargetGroupPlayer();
    }
    private void OnEnable()
    {
        //Subscribe to the aim through sight event
        playerTarget.gameObject.GetComponent<Player>().aimThroughSightEvent.OnAimThroughSight += AimThroughSightEvent_OnAimThroughSight;
    }
    private void OnDisable()
    {
        //Unsubscribe from the aim through sight event
        playerTarget.gameObject.GetComponent<Player>().aimThroughSightEvent.OnAimThroughSight -= AimThroughSightEvent_OnAimThroughSight;
    }
    private void Update()
    {
        cursorTarget.position = UtilsClass.GetWorldMousePosition();
        scopeCursor.position = UtilsClass.GetWorldMousePosition();

        playerTarget = GameManager.Instance.GetPlayer().transform;
    }
    // <summary>
    // Aim through sighth event handler. When player click right mouse button we are gonna recalculate the average camera look
    // </summary>
    private void AimThroughSightEvent_OnAimThroughSight(AimThroughSightEvent arg1, AimThrouhSightEventArgs aimThrouhSightEventArgs)
    {
        if (aimThrouhSightEventArgs.hasGunScope)
        {
            scopeCursor.gameObject.SetActive(true);
            target_Cursor = new CinemachineTargetGroup.Target() { weight = aimThrouhSightEventArgs.gunWeight, radius = 2f, target = scopeCursor };
        }
        else
        {
            scopeCursor.gameObject.SetActive(false);
            target_Cursor = new CinemachineTargetGroup.Target() { weight = aimThrouhSightEventArgs.gunWeight, radius = 2f, target = cursorTarget };
        }

        target_Player = new CinemachineTargetGroup.Target() { weight = 2f, radius = 2.5f, target = playerTarget };


        SetTargetGroupPlayer();
    }

    //<summary>
    //Initialize target group at start
    //</summary>
    private void InitializeTargetGroupPlayer()
    {
        target_Player = new CinemachineTargetGroup.Target() { weight = 2f, radius = 2.5f, target = playerTarget };
        target_Cursor = new CinemachineTargetGroup.Target() { weight = 1f, radius = 2f, target = cursorTarget };

        SetTargetGroupPlayer();

    }

    //<summary>
    //From time to time we are going to change targets' weight and radius so this is gonna be compulsory for resetting targets.
    //</summary>
    private void SetTargetGroupPlayer()
    {
        CinemachineTargetGroup.Target[] cinemachineTargetGroupArray = new CinemachineTargetGroup.Target[] { target_Player, target_Cursor };
        cinemachineTargetGroup.m_Targets = cinemachineTargetGroupArray;
    }

}
