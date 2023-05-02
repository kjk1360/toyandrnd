using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager _instance { get; private set; }

    //public ObjectPool enemyObjectPool;
    //public ObjectPool healItemObjectPool;
    //public ObjectPool chainShotObjectPool;
    //public ObjectPool chainShotBlastObjectPool;
    //public ObjectPool powerItemObjectPool;
    public Transform player;
    public float spawnInterval = 5.0f;
    public float minSpawnDistance = 10.0f;
    public float maxSpawnDistance = 50.0f;
    public int maxSpawnEnemyCount = 10;
    public int curLiveEnemy = 0;

    private float timeSinceLastSpawn;
    public static SpawnManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SpawnManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject("SpawnManager");
                    _instance = singleton.AddComponent<SpawnManager>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    //private void Update()
    //{
    //    timeSinceLastSpawn += Time.deltaTime;

    //    if (timeSinceLastSpawn >= spawnInterval)
    //    {
    //        timeSinceLastSpawn = 0;
    //        var enemyObject = SpawnEnemy(GetRandomSpawnPosition());
    //        if(enemyObject != null)
    //        {
    //            NormalEnemyController enemyController = enemyObject.GetComponent<NormalEnemyController>();
    //            enemyController.InitializeController();
    //        }

    //    }
    //}
    public IEnumerator SpawnEnemiesCoroutine()
    {
        while (true)
        {
            var enemyObject = SpawnEnemy(GetRandomSpawnPosition());
            if (enemyObject != null)
            {
                NormalEnemyController enemyController = enemyObject.GetComponent<NormalEnemyController>();
                enemyController.InitializeController();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
    public GameObject SpawnEnemy(Vector3 spawnPosition)
    {
        if(curLiveEnemy >= maxSpawnEnemyCount)
        {
            return null;
        }
        //GameObject enemyObject = enemyObjectPool.GetPooledObject();
        //enemyObject.transform.position = spawnPosition;
        //enemyObject.SetActive(true);
        curLiveEnemy++;
        return PoolingManager.Instance.GetObjectByID(6, spawnPosition);
    }
    public GameObject SpawnBoss(Vector3 spawnPosition)
    {
        return PoolingManager.Instance.GetObjectByID(7, spawnPosition);
    }
    public GameObject SpawnItem(Vector3 spawnPosition)
    {
        //GameObject itemObject = healItemObjectPool.GetPooledObject();
        //itemObject.transform.position = spawnPosition;
        //itemObject.SetActive(true);
        return PoolingManager.Instance.GetObjectByID(4, spawnPosition);
    }

    public GameObject SpawnChainShot(Vector3 spawnPosition)
    {
        //GameObject chainShotObject = chainShotObjectPool.GetPooledObject();
        //chainShotObject.transform.position = spawnPosition;
        //chainShotObject.SetActive(true);
        return PoolingManager.Instance.GetObjectByID(1, spawnPosition);
    }
    public GameObject SpawnChainShotBlast(Vector3 spawnPosition)
    {
        //GameObject chainShotBlastObject = chainShotBlastObjectPool.GetPooledObject();
        //chainShotBlastObject.transform.position = spawnPosition;
        //chainShotBlastObject.SetActive(true);
        return PoolingManager.Instance.GetObjectByID(0, spawnPosition);
    }
    public GameObject SpawnPowerItem(Vector3 spawnPosition)
    {
        //GameObject powerItem = powerItemObjectPool.GetPooledObject();
        //powerItem.transform.position = spawnPosition;
        //powerItem.SetActive(true);
        return PoolingManager.Instance.GetObjectByID(5, spawnPosition);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float angle = Random.Range(0.0f, 360.0f);
        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);

        Vector3 offset = new Vector3(Mathf.Sin(angle) * distance, Mathf.Cos(angle) * distance, 0);
        return player.position + offset;
    }
}