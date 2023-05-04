using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SharpConfig; // cool

public class MainMenu : MonoBehaviour
{
    public void StartSimulation()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartSimulation()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitApplication()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
