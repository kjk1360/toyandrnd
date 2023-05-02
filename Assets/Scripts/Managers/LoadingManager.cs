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

        //Ǯ�� ������ ���� ���� �����Ƿ� ���Ӿ� ȣ�� �Ŀ� �ҷ��� ��
        yield return StartCoroutine(PoolingManager.Instance.InitializeCoroutine(poolingDataGroupList, (poolingProgress) =>
        {
            loadingBar.fillAmount = 0.9f + (poolingProgress * 0.1f);
        }));

        SceneManager.UnloadSceneAsync(loadingSceneName);

        //�ӽ÷� ���⼭ �ҷ����� ���� �Ŵ������� ������Ʈ ���� ���� �ϰ�ó���� �ʿ��ϴ�
        StartCoroutine(SpawnManager.Instance.SpawnEnemiesCoroutine());
    }
}