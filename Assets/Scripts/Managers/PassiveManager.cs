using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveManager : MonoBehaviour
{
    public static PassiveManager _instance { get; private set; }
    public int curLV = 1;
    public int MaxExp = 5;
    public int CurExp = 0;
    public GameObject PassivePopup;
    public PlayerController player;
    public static PassiveManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PassiveManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject("PassiveManager");
                    _instance = singleton.AddComponent<PassiveManager>();
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

    public void KillEnemy()
    {
        CurExp++;
        if(CurExp >= MaxExp)
        {
            CurExp = 0;
            MaxExp *= 2;
            curLV++;
            SpawnManager.Instance.spawnInterval -= 0.1f;
            SpawnManager.Instance.maxSpawnEnemyCount *= 2;
            if (SpawnManager.Instance.spawnInterval <= 0)
            {
                SpawnManager.Instance.spawnInterval = 0.01f;
            }
            Time.timeScale = 0;

            PassivePopup.SetActive(true);
        }
    }

    public void OnClickPassive(int index)
    {
        switch (index)
        {
            case 0:
                player.AddStatValue(StatName.ProjectileCount, 1);
                break;
                case 1:
                player.AddStatValue(StatName.ChainCount, 1); break;
                case 2:
                player.AddStatMuliply(StatName.BlastRadius, 0.2f); break;
        }

        PassivePopup.SetActive(false);
        Time.timeScale = 1f;
    }



}
