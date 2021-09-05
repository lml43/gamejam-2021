using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public string gameScene;
    public GameObject scripPopups;
    public TutorialManager tutorialManager;

    public void LoadMenuScene() {
        SceneManager.LoadScene(0);
    }

    public void LoadGameScene() {
        StateControl.Instance.ResetState();
        SceneManager.LoadScene(gameScene);
    }

    public void ReloadTutorial() {
        tutorialManager.ReloadTutorial();
    }

    public void HideHowToPlay() {
        scripPopups.SetActive(false);
    }

    public void QuitGame() {
        Application.Quit();
    }
}