using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

    public string scenceToLoad;
    public float availableTime;

    private float currentTime;

    private void Awake() {
        currentTime = availableTime;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // if (currentTime < 0) {
            if (other.CompareTag("Player") && !other.isTrigger) {
                SceneManager.LoadScene(scenceToLoad);
            }
        // }
    }

    // private void FixedUpdate() {
    //     if ()
    // }
}
