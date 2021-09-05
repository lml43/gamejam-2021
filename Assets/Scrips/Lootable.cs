using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootable : MonoBehaviour
{

    public bool isTutorial;

    public GameObject itemGFX;
    public GameObject fireBox;
    public GameObject wrenchBox;
    public GameObject heart1;
    public GameObject heart2;

    private bool isPlayerInRange;
    private GameObject player;
    private GameManager gameManager;


    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player");
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && isPlayerInRange) {

            FindObjectOfType<AudioManager>().Play("Loot");

            if (isTutorial) {
                gameObject.SetActive(false);
                TutorialManager.looted = true;
                return;
            }

            if (gameObject.CompareTag("Ruler")) {
                gameManager.ShowRulerPopups();
                player.GetComponent<Animator>().SetBool("hasRuler", true);
            }

            if (gameObject.CompareTag("FireExtinguisher")) {
                fireBox.GetComponent<Animator>().SetBool("looted", true);
            }

            if (gameObject.CompareTag("Wrench")) {
                wrenchBox.GetComponent<Animator>().SetBool("looted", true);
            }

            if (gameObject.CompareTag("Heart1")) {
                if (StateControl.Instance.hasHeart) {
                    return;
                }
                StateControl.Instance.lootHeart1 = true;
                StateControl.Instance.hasHeart = true;
            }

            if (gameObject.CompareTag("Heart2")) {
                if (StateControl.Instance.hasHeart) {
                    return;
                }
                StateControl.Instance.lootHeart2 = true;
                StateControl.Instance.hasHeart = true;
            }

            gameObject.SetActive(false);
            int index = gameManager.AddItem(StateControl.Instance.itemArr, gameObject.tag);
            ShowItem(itemGFX, index);
            gameManager.CheckGunMaterials();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            isPlayerInRange = false;
        }
    }

    private void ShowItem(GameObject item, int index) {
        Vector3 pos = new Vector3(46 * (index - 2), 0, 0);
        item.GetComponent<RectTransform>().localPosition = pos;
        item.SetActive(true);
    }


    
}
