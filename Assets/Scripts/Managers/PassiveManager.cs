using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveManager : MonoBehaviour
{
    public static PassiveManager Instance { get; private set; }
    public int curLV = 1;
    public int MaxExp = 5;
    public int CurExp = 0;
    public GameObject PassivePopup;
    public PlayerController player;
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
