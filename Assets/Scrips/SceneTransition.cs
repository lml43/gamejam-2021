using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

    public Transform player;
    public string scenceToLoad;
    public FloatValue availableTime;
    public bool isRealWorld;

    public Transform[] spots;

    private float endTime;
    private bool hasShown = false;

    private void Awake() {
        endTime = Time.time + availableTime.initialValue;
    }

    private void Update() {
        if (Time.time > endTime) {
            if (isRealWorld) {
                StateControl.Instance.playerPos = player.position;
                SceneManager.LoadScene(scenceToLoad);
            } else if (!hasShown) {
                hasShown = true;
                System.Random ran = new System.Random();
                int randomIdx = ran.Next(spots.Length);
                transform.position = spots[randomIdx].position;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !other.isTrigger) {
            SceneManager.LoadScene(scenceToLoad);
        }
    }

}
