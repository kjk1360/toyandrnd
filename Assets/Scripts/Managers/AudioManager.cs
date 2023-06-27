using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;
    public Dictionary<string, AudioClip> soundClipDic;
    public Dictionary<string, AudioSource> soundSourceDic;
    [Header("#Volumes")]
    public float bgmVolume;
    public float sfxVolume;
    public ObjectPoolData volumeData;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject("AudioManager");
                    _instance = singleton.AddComponent<AudioManager>();
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
    public void Initialize(List<string> soundNameList, System.Action<float> onProgressUpdate = null)
    {

        StartCoroutine(InitializeCoroutine(soundNameList, onProgressUpdate));
    }
    public IEnumerator InitializeCoroutine(List<string> soundNameList, System.Action<float> onProgressUpdate)
    {
        yield return 0;
    }
}
