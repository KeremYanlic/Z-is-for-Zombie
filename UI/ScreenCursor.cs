using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class ScreenCursor : MonoBehaviour
{
    private Image cursorImage;
    private Player player;

    private void Awake()
    {
        //Load components
        cursorImage = GetComponent<Image>();
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
            cursorImage.enabled = false;
        }
        else
        {
            cursorImage.enabled = true;
        }
    }

    private void Start()
    {
        Cursor.visible = false;
    }
    private void Update()
    {
        transform.position = Input.mousePosition;
    }

}
