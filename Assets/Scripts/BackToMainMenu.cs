using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
    public void BackMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
