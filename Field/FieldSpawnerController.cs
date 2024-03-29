using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


[RequireComponent(typeof(ZombieSpawnEvent))]
[RequireComponent(typeof(BoxCollider2D))]
public class FieldSpawnerController : MonoBehaviour
{
    private Field field;
    private ZombieSpawnEvent zombieSpawnEvent;
    private Coroutine zombieSpawnCoroutine;

    private BoxCollider2D areaCollider;
    private Vector2 topLeftCorner; //Collider top left vector2 position
    private Vector2 bottomRightCorner; //Collider bottom right vector2 position

    #region Tooltip
    [Tooltip("Populate with a zombie spawn template in order to spawn different type of zombies at different numbers")]
    #endregion
    [SerializeField] private ZombieSpawnTemplateSO zombieSpawnTemplateSO;

    #region Tooltip
    [Tooltip("Populate with the Spawnable Positions Tilemap to define zombie spawnable positions")]
    #endregion
    [SerializeField] private Tilemap zombieSpawnableAreasTilemap;

    //Safe spawnable positions. HideInInspector attribute can be added to this variable.
    public List<Vector3> spawnablePositions;

    private void Awake()
    {
        //Load components
        field = GetComponentInParent<Field>();
        zombieSpawnEvent = GetComponent<ZombieSpawnEvent>();
        areaCollider = GetComponent<BoxCollider2D>();

        SetColliderVectors(); //Set top left vector and bottom right vector

    }
    private void Start()
    {
        FindSpawnablePositions();


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Settings.playerTag))
        {
            zombieSpawnEvent.CallZombieSpawnEvent();
        }
    }

    private void OnEnable()
    {
        //Subscribe to zombie spawn event
        zombieSpawnEvent.OnZombieSpawn += ZombieSpawnEvent_OnZombieSpawn;
    }

    private void OnDisable()
    {
        //Unsubscribe from zombie spawn event
        zombieSpawnEvent.OnZombieSpawn -= ZombieSpawnEvent_OnZombieSpawn;
    }


    private void ZombieSpawnEvent_OnZombieSpawn(ZombieSpawnEvent obj, ZombieSpawnEventArgs zombieSpawnEventArgs)
    {
        zombieSpawnCoroutine = StartCoroutine(SpawnZombies());
    }
    private IEnumerator SpawnZombies()
    {
        //Spawn adult zombies
        SpawnAdultZombies();
        yield return new WaitForSeconds(0.1f);
        SpawnChildZombies();
        yield return new WaitForSeconds(0.1f);
        SpawnGiantZombies();
        yield return new WaitForSeconds(0.1f);
        SpawnWitchZombies();


    }

    //<summary>
    //Spawn adult zombies
    //</summary>
    private void SpawnAdultZombies()
    {
        int zombieSpawnCounter = 0;

        if (zombieSpawnTemplateSO.adultZombieSpawnAmount == 0) return;

        while (zombieSpawnCounter < zombieSpawnTemplateSO.adultZombieSpawnAmount && spawnablePositions.Count > 0)
        {
            //Get a random index to choose a random spawn position.
            int randomIndex = Random.Range(0, spawnablePositions.Count);

            //Set randomSpawnPosition according to randomIndex
            Vector3 randomSpawnPosition = spawnablePositions[randomIndex];

            Addressables.LoadAssetAsync<GameObject>(Settings.adultZombieRef).Completed += (asyncOperationHandle) =>
            {
                if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    //Instantiate Zombie
                    GameObject adultZombie = Instantiate(zombieSpawnTemplateSO.pfAdultZombie, randomSpawnPosition, Quaternion.identity);
                    InitialiseZombie(randomSpawnPosition, adultZombie);
                }
            };
            //Remove that randomSpawnPosition from spawnablePositions to obscure spawning the zombies on same position randomly.
            spawnablePositions.RemoveAt(randomIndex);

            zombieSpawnCounter++;
        }
    }
    //<summary>
    //Spawn giant zombies
    //</summary>
    private void SpawnGiantZombies()
    {
        int zombieSpawnCounter = 0;

        if (zombieSpawnTemplateSO.giantZombieSpawnAmount == 0) return;


        while (zombieSpawnCounter < zombieSpawnTemplateSO.giantZombieSpawnAmount && spawnablePositions.Count > 0)
        {
            //Get a random index to choose a random spawn position.
            int randomIndex = Random.Range(0, spawnablePositions.Count);

            //Set randomSpawnPosition according to randomIndex
            Vector3 randomSpawnPosition = spawnablePositions[randomIndex];

            Addressables.LoadAssetAsync<GameObject>(Settings.giantZombieRef).Completed += (asyncOperationHandle) =>
            {
                if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    //Instantiate Zombie
                    GameObject giantZombie = Instantiate(zombieSpawnTemplateSO.pfGiantZombie, randomSpawnPosition, Quaternion.identity);
                    InitialiseZombie(randomSpawnPosition, giantZombie);
                }
            };
            //Remove that randomSpawnPosition from spawnablePositions to obscure spawning the zombies on same position randomly.
            spawnablePositions.RemoveAt(randomIndex);

            zombieSpawnCounter++;
        }
    }
    //<summary>
    //Spawn kid zombies
    //</summary>
    private void SpawnChildZombies()
    {
        int zombieSpawnCounter = 0;

        if (zombieSpawnTemplateSO.childZombieSpawnAmount == 0) return;


        while (zombieSpawnCounter < zombieSpawnTemplateSO.childZombieSpawnAmount && spawnablePositions.Count > 0)
        {
            //Get a random index to choose a random spawn position.
            int randomIndex = Random.Range(0, spawnablePositions.Count);

            //Set randomSpawnPosition according to randomIndex
            Vector3 randomSpawnPosition = spawnablePositions[randomIndex];

            Addressables.LoadAssetAsync<GameObject>(Settings.kidZombieRef).Completed += (asyncOperationHandle) =>
            {
                if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    //Instantiate Zombie
                    GameObject childZombie = Instantiate(zombieSpawnTemplateSO.pfChildZombie, randomSpawnPosition, Quaternion.identity);
                    InitialiseZombie(randomSpawnPosition, childZombie);
                }
            };
                
            //Remove that randomSpawnPosition from spawnablePositions to obscure spawning the zombies on same position randomly.
            spawnablePositions.RemoveAt(randomIndex);

            zombieSpawnCounter++;
        }
    }


    //<summary>
    //Spawn witch zombies
    //</summary>
    private void SpawnWitchZombies()
    {
        int zombieSpawnCounter = 0;

        if (zombieSpawnTemplateSO.witchZombieSpawnAmount == 0) return;


        while (zombieSpawnCounter < zombieSpawnTemplateSO.witchZombieSpawnAmount && spawnablePositions.Count > 0)
        {
            //Get a random index to choose a random spawn position.
            int randomIndex = Random.Range(0, spawnablePositions.Count);

            //Set randomSpawnPosition according to randomIndex
            Vector3 randomSpawnPosition = spawnablePositions[randomIndex];

            //Instantiate Zombie
            Addressables.LoadAssetAsync<GameObject>(Settings.witchZombieRef).Completed += (asyncOperationHandle) =>
            {
                if(asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                {

                    GameObject witchPrefab = Instantiate(zombieSpawnTemplateSO.pfWitchZombie, randomSpawnPosition, Quaternion.identity);
                    InitialiseZombie(randomSpawnPosition, witchPrefab);
                }
            };
            //Remove that randomSpawnPosition from spawnablePositions to obscure spawning the zombies on same position randomly.
            spawnablePositions.RemoveAt(randomIndex);

            zombieSpawnCounter++;
        }
    }

    private void InitialiseZombie(Vector3 randomSpawnPosition, GameObject adultZombie)
    {
        Zombie zombie = adultZombie.GetComponent<Zombie>();
        zombie.InitialiseZombie(zombie.GetZombieDetailsSO(), randomSpawnPosition);
        zombie.SetFieldSpawnController(this);
    }

    //<summary>
    //Set the top left collider vector and bottom right collider vector at start.
    //</summary>
    private void SetColliderVectors()
    {
        topLeftCorner = areaCollider.transform.position + new Vector3(-areaCollider.size.x / 2f, areaCollider.size.y / 2f, 0);
        bottomRightCorner = areaCollider.transform.position + new Vector3(areaCollider.size.x / 2f, -areaCollider.size.y / 2f, 0);
    }

    //<summary>
    //Find safe spawnable positions that zombies will be able to spawn without any possible error or bug.
    //</summary>
    private void FindSpawnablePositions()
    {
        for (int i = (int)topLeftCorner.x; i < bottomRightCorner.x; i++)
        {
            for (int j = (int)topLeftCorner.y; j > bottomRightCorner.y; j--)
            {
                if (zombieSpawnableAreasTilemap.HasTile(new Vector3Int(i, j)))
                {
                    spawnablePositions.Add(new Vector3(i, j));
                }
            }
        }
    }

    public List<Vector3> GetWalkablePositions()
    {
        return spawnablePositions;
    }


}
