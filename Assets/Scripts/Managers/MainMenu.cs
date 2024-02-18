using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    public void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0) return;
        
        PlayerInputHandler[] inGameControls = GameObject.FindObjectsOfType<PlayerInputHandler>();
        foreach(PlayerInputHandler p in inGameControls)
        {
            Destroy(p.gameObject);
        }

        Destroy(GameObject.Find("GameInfo"));

        GetComponent<PlayerInput>().enabled = false;
        GetComponent<PlayerInput>().enabled = true;
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }
}
