using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public void BtnClick()
    {
        SceneManager.LoadScene("IdleStage");
    }
}
