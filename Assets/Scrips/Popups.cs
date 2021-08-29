using UnityEngine;
using UnityEngine.SceneManagement;

public class Popups : MonoBehaviour
{

    public GameObject popups;

    // TODO move to GameManager


    public void BackToMenu() {
        StateControl.Instance.ResetState();
        SceneManager.LoadScene("Menu");
    }

    public void ShowPopups() {
        popups.SetActive(true);
    }

    public void HidePopups() {
        popups.SetActive(false);
    }

}
