using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    public ObjectPool enemyObjectPool;
    public ObjectPool healItemObjectPool;
    public ObjectPool chainShotObjectPool;
    public ObjectPool chainShotBlastObjectPool;
    public ObjectPool powerItemObjectPool;
    public Transform player;
    public float spawnInterval = 5.0f;
    public float minSpawnDistance = 10.0f;
    public float maxSpawnDistance = 50.0f;

    private float timeSinceLastSpawn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            timeSinceLastSpawn = 0;
            var enemyObject = SpawnEnemy(GetRandomSpawnPosition());
            NormalEnemyController enemyController = enemyObject.GetComponent<NormalEnemyController>();
            enemyController.InitializeController();
        }
    }

    public GameObject SpawnEnemy(Vector3 spawnPosition)
    {
        GameObject enemyObject = enemyObjectPool.GetPooledObject();
        enemyObject.transform.position = spawnPosition;
        enemyObject.SetActive(true);
        return enemyObject;
    }
    public GameObject SpawnItem(Vector3 spawnPosition)
    {
        GameObject itemObject = healItemObjectPool.GetPooledObject();
        itemObject.transform.position = spawnPosition;
        itemObject.SetActive(true);
        return itemObject;
    }

    public GameObject SpawnChainShot(Vector3 spawnPosition)
    {
        GameObject chainShotObject = chainShotObjectPool.GetPooledObject();
        chainShotObject.transform.position = spawnPosition;
        chainShotObject.SetActive(true);
        return chainShotObject;
    }
    public GameObject SpawnChainShotBlast(Vector3 spawnPosition)
    {
        GameObject chainShotBlastObject = chainShotBlastObjectPool.GetPooledObject();
        chainShotBlastObject.transform.position = spawnPosition;
        chainShotBlastObject.SetActive(true);
        return chainShotBlastObject;
    }
    public GameObject SpawnPowerItem(Vector3 spawnPosition)
    {
        GameObject powerItem = powerItemObjectPool.GetPooledObject();
        powerItem.transform.position = spawnPosition;
        powerItem.SetActive(true);
        return powerItem;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float angle = Random.Range(0.0f, 360.0f);
        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);

        Vector3 offset = new Vector3(Mathf.Sin(angle) * distance, Mathf.Cos(angle) * distance, 0);
        return player.position + offset;
    }
}