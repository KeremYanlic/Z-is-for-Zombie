using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static System.Net.Mime.MediaTypeNames;

public class CarStatusUI : MonoBehaviour
{
    private Player player;

    [SerializeField] private Image speedometerNeedle;
    [SerializeField] private Image gasMeterNeedle;

    [SerializeField] private TextMeshProUGUI speedText;

    private float zeroSpeedAngle = 70f;
    private float maxSpeedAngle;

    private const float ZERO_GAS_ANGLE = 15f;
    private const float MAX_GAS_ANGLE = -110f;

    private float speed;
    private float speedMax;

    private float gasLeft;
    private float gasLeftMax;

    private void Awake()
    {
        player = GameManager.Instance.GetPlayer();

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
        //Unubscribe from set active car event
        player.setActiveCarEvent.OnSetActiveCar -= SetActiveCarEvent_OnSetActiveCar;

        //Unubscribe to set deactive car event
        player.setActiveCarEvent.OnDeactiveCar -= SetActiveCarEvent_OnDeactiveCar;
    }

    // <summary>
    // Set active UI
    // </summary>
    private void SetActiveCarEvent_OnSetActiveCar(SetActiveCarEvent arg1, SetActiveCarEventArgs arg2)
    {
        ChangeActiveness(true);
    }

    // <summary>
    // Set deactive UI
    // </summary>
    private void SetActiveCarEvent_OnDeactiveCar(SetActiveCarEvent obj)
    {
        ChangeActiveness(false);
    }
    //<summary>
    //Change the activeness of the weapon status UI
    //</summary>
    private void ChangeActiveness(bool activeness)
    {
        foreach (Transform transform in transform)
        {
            transform.gameObject.SetActive(activeness);
        }
    }

    private void Update()
    {
        if (player.activeCar.activeCar != null)
        {
            GameObject activeCar = player.activeCar.activeCar;
            Car car = activeCar.GetComponent<Car>();
            CarController carController = activeCar.GetComponent<CarController>();

            speed = carController.GetSpeed();
            speedMax = carController.GetSpeedMax();

            gasLeft = car.gasAmount;
            gasLeftMax = car.carDetailsSO.startGasAmount;

            maxSpeedAngle = -carController.GetSpeedMax();

            speedText.text = ((int)speed).ToString();
            speedometerNeedle.rectTransform.eulerAngles = new Vector3(0, 0, GetSpeedRotation());

            gasMeterNeedle.rectTransform.eulerAngles = new Vector3(0, 0, GetGasRotation());
        }
    }
    private float GetSpeedRotation()
    {
        return Mathf.Lerp(zeroSpeedAngle, maxSpeedAngle, speed / speedMax);
    }
    private float GetGasRotation()
    {
        float totalAngleSize = ZERO_GAS_ANGLE - MAX_GAS_ANGLE;

        float gasNormalized = gasLeft / gasLeftMax;

        return ZERO_GAS_ANGLE - gasNormalized * totalAngleSize;
    }
}
