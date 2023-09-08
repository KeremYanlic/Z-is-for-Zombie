using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Tilemap))]
[DisallowMultipleComponent]
public class BuildingManager : MonoBehaviour
{
    private Player player;

    private BoxCollider2D boxCollider2D;
    private TilemapRenderer tilemapRenderer;
    private Tilemap tilemap;
    private Color tilemapColor;
    private Material material;

    private const float OPAQUE_ALPHA_VALUE = 1;
    private const float TRANSPARENT_ALPHA_VALUE = .5f;
    private void Awake()
    {
        //Load components
        player = GameManager.Instance.GetPlayer();
        boxCollider2D = GetComponent<BoxCollider2D>();
        tilemap = GetComponent<Tilemap>();
        tilemapRenderer = GetComponent<TilemapRenderer>();
        material = tilemapRenderer.material;
        tilemapColor = tilemap.color;


    }
    // Start is called before the first frame update
    void Start()
    {
        tilemapColor.a = OPAQUE_ALPHA_VALUE;
        tilemap.color = tilemapColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (boxCollider2D.bounds.Contains(player.transform.position))
        {
            tilemapColor.a = TRANSPARENT_ALPHA_VALUE;
            material.color = tilemapColor;
        }
        else
        {
            tilemapColor.a = OPAQUE_ALPHA_VALUE;
            material.color = tilemapColor;
        }
    }
}
