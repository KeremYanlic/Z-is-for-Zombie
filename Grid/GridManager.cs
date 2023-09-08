using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[DisallowMultipleComponent]
public class GridManager : MonoBehaviour
{
    private Player player;

    private List<TilemapRenderer> tilemapRenderers = new List<TilemapRenderer>();


    private void Awake()
    {
        //Load player
        player = GameManager.instance.GetPlayer();

        foreach (TilemapRenderer tilemapRenderer in gameObject.GetComponentsInChildren<TilemapRenderer>())
        {
            tilemapRenderers.Add(tilemapRenderer);
        }
    }
    private void OnEnable()
    {
        player.aimThroughSightEvent.OnAimThroughSight += AimThroughSightEvent_OnAimThroughSight;
    }
    private void OnDisable()
    {
        player.aimThroughSightEvent.OnAimThroughSight -= AimThroughSightEvent_OnAimThroughSight;
    }

    private void AimThroughSightEvent_OnAimThroughSight(AimThroughSightEvent arg1, AimThrouhSightEventArgs aimThrouhSightEventArgs)
    {
        if (aimThrouhSightEventArgs.hasGunScope)
        {
            foreach (TilemapRenderer tilemapRenderer in tilemapRenderers)
            {
                tilemapRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }
        }
        else
        {
            foreach (TilemapRenderer tilemapRenderer in tilemapRenderers)
            {
                tilemapRenderer.maskInteraction = SpriteMaskInteraction.None;
            }
        }
    }


}
