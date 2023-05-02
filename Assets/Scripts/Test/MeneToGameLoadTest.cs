using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeneToGameLoadTest : MonoBehaviour
{
    public List<ObjectPoolGroup> poolingDataGroupList = new List<ObjectPoolGroup>();
    public void OnClickGameStartBtn()
    {
        LoadingManager.Instance.LoadScene("GameScene", poolingDataGroupList);
    }
}
