using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public void BtnClick()
    {
        DataManager.Instance.Load();
        SceneManager.LoadScene("IdleStage");
    }
}
