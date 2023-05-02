using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static ObjectPoolData;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager _instance;
    public List<ObjectPoolGroup> poolingDataGroupList;
    public SlicedFilledImage loadingBar;
    public string loadingSceneName = "LoadingScene";
    public static LoadingManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LoadingManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject("LoadingManager");
                    _instance = singleton.AddComponent<LoadingManager>();
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
    public void LoadScene(string sceneName, List<ObjectPoolGroup> poolingDataGroupList)
    {
        StartCoroutine(LoadSceneAsync(sceneName, poolingDataGroupList));
    }
    private IEnumerator LoadSceneAsync(string sceneName, List<ObjectPoolGroup> poolingDataGroupList)
    {
        // Load the LoadingScene
        AsyncOperation loadLoadingScene = SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Single);
        while (!loadLoadingScene.isDone)
        {
            yield return null;
        }
        loadingBar = GameObject.FindObjectOfType<SlicedFilledImage>();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadingBar.fillAmount = progress;
            yield return null;
        }

        //풀링 정보가 게임 씬에 있으므로 게임씬 호출 후에 불러야 함
        yield return StartCoroutine(PoolingManager.Instance.InitializeCoroutine(poolingDataGroupList, (poolingProgress) =>
        {
            loadingBar.fillAmount = 0.9f + (poolingProgress * 0.1f);
        }));

        SceneManager.UnloadSceneAsync(loadingSceneName);

        //임시로 여기서 불렀지만 게임 매니저에서 업데이트 관련 로직 일괄처리가 필요하다
        StartCoroutine(SpawnManager.Instance.SpawnEnemiesCoroutine());
    }
}