using UnityEngine;
using UnityEngine.SceneManagement;

public class Popups : MonoBehaviour
{

    public GameObject popupsHamburger;
    public GameObject popupsInfo;

    // TODO move to GameManager


    public void BackToMenu() {
        StateControl.Instance.ResetState();
        SceneManager.LoadScene("Menu");
    }

    public void ShowPopups() {
        popupsHamburger.SetActive(true);
    }

    public void HidePopups() {
        popupsHamburger.SetActive(false);
    }

    public void ShowPopupsInfo() {
        popupsInfo.SetActive(true);
    }

    public void HidePopupsInfo() {
        popupsInfo.SetActive(false);
    }

}
