﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void OnPlayerVsPlayerClick()
    {
        SceneManager.LoadScene(1);
    }

    public void OnPlayerVsAIClick()
    {
        SceneManager.LoadScene(2);
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
}
