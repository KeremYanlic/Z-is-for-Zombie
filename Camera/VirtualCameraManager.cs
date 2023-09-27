using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class VirtualCameraManager : MonoBehaviour
{
    
    #region Tooltip
    [Tooltip("Populate with the CinemachineTargetGroupTransform from Game scene")]
    #endregion
    [SerializeField] private Transform CinemachineTargetGroup;


    private CinemachineVirtualCamera virtualCamera;
    private Player player;


    private void Awake()
    {
        //Load components
        player = GameManager.Instance.GetPlayer();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        //Set virtual camera follow as cinemachinetargetgroup at awake
        virtualCamera.Follow = CinemachineTargetGroup.transform;
    }
    private void OnEnable()
    {
        //Subscribe to set active car event
        player.setActiveCarEvent.OnSetActiveCar += SetActiveCarEvent_OnSetActiveCar;

        //Subscribe to deactive car event
        player.setActiveCarEvent.OnDeactiveCar += SetActiveCarEvent_OnDeactiveCar;
    }
    private void OnDisable()
    {
        //Unsubscribe from set active car event
        player.setActiveCarEvent.OnSetActiveCar -= SetActiveCarEvent_OnSetActiveCar;

        //Unsubscribe from deactive car event
        player.setActiveCarEvent.OnDeactiveCar -= SetActiveCarEvent_OnDeactiveCar;
    }

    //<summary>
    //Set follow to car handler
    //</summary>
    private void SetActiveCarEvent_OnSetActiveCar(SetActiveCarEvent arg1, SetActiveCarEventArgs setActiveCarEventArgs)
    {
        SetFollow(setActiveCarEventArgs.activeCar.transform);
    }

    //<summary>
    //Set follow to cinemachine target group handler
    //</summary>
    private void SetActiveCarEvent_OnDeactiveCar(SetActiveCarEvent obj)
    {
        SetFollow(CinemachineTargetGroup.transform);
    }

    //<summary>
    //Set Virtual Camera follow function
    //</summary>
    private void SetFollow(Transform followTarget)
    {
        virtualCamera.Follow = followTarget;
    }
}
