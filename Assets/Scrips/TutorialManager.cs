using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy1;
    public GameObject enemy2;
    public IntValue playerHealth;
    public GameObject startBtn;
    public GameObject watchBtn;
    public BoolValue isPaused;

    [Header("Canvas")]
    [Space]
    public GameObject healthBar;
    public GameObject itemBar;
    public GameObject itemHeart;
    public GameObject itemFire;

    public static bool looted;
    public static bool smashed;
    public static bool shooted;
    public static int step;

    private Animator playerAnim;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (looted && step == 0) {
            step++;
            StateControl.Instance.hasRuler = true;
            playerAnim.SetBool("hasRuler", true);
            enemy1.SetActive(true);
        }

        if (smashed && step == 1) {
            step++;
            StateControl.Instance.hasGun = true;
            playerAnim.SetBool("hasGun", true);
            enemy2.SetActive(true);
        }

        if (shooted && step == 2) {
            step++;
            playerHealth.runtimeValue = 30;
            healthBar.SetActive(true);
            itemBar.SetActive(true);
        }

        if (step == 3 && Input.GetButtonDown("Healing")) {
            step++;
            itemHeart.SetActive(false);
            playerHealth.runtimeValue += 50;
            StartCoroutine(ShowButtonsCo());
        }
    }

    public void ReloadTutorial() {
        StateControl.Instance.ResetState();
        looted = false;
        smashed = false;
        shooted = false;
        step = 0;
        healthBar.SetActive(false);
        itemBar.SetActive(false);
        startBtn.SetActive(false);
        watchBtn.SetActive(false);
        itemFire.SetActive(true);
        itemHeart.SetActive(true);
        isPaused.runtimeValue = false;
    }

    private IEnumerator ShowButtonsCo() {
        yield return new WaitForSeconds(1.5f);
        isPaused.runtimeValue = true;
        startBtn.SetActive(true);
        watchBtn.SetActive(true);
    }
}
