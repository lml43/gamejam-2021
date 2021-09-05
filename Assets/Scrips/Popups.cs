using UnityEngine;
using UnityEngine.SceneManagement;

public class Popups : MonoBehaviour
{

    public GameObject popupsHamburger;
    public GameObject popupsInfo;
    public BoolValue isPaused;
    public BoolValue isFirstLoad;

    // TODO move to GameManager

    void Awake() {
        if (isFirstLoad.runtimeValue) {
            ShowPopupsInfo();
            // GenerateItems();
        }
    }

    public void BackToMenu() {
        StateControl.Instance.ResetState();
        SceneManager.LoadScene("Menu");
    }

    public void ShowPopups() {
        isPaused.runtimeValue = true;
        popupsHamburger.SetActive(true);
    }

    public void HidePopups() {
        isPaused.runtimeValue = false;
        popupsHamburger.SetActive(false);
        FindObjectOfType<CountDown>().UpdateNextTimestamp();
    }

    public void ShowPopupsInfo() {
        isPaused.runtimeValue = true;
        popupsInfo.SetActive(true);
    }

    public void HidePopupsInfo() {
        if (isFirstLoad.runtimeValue) {
            isFirstLoad.runtimeValue = false;
        }
        isPaused.runtimeValue = false;
        popupsInfo.SetActive(false);
        FindObjectOfType<CountDown>().UpdateNextTimestamp();
    }

}
