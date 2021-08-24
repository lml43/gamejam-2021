﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public string gameScene;

    public void LoadMenuScene() {
        SceneManager.LoadScene(0);
    }

    public void LoadGameScene() {
        SceneManager.LoadScene(gameScene);
    }

    public void QuitGame() {
        Application.Quit();
    }
}