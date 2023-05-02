using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ObjectPoolData;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;
    public List<ObjectPoolGroup> poolingDataGroupList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        PoolingManager.Instance.Initialize(poolingDataGroupList);

        SceneManager.UnloadSceneAsync("LoadScene");
    }
}