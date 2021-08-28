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

    private float endTime;

    private void Awake() {
        endTime = Time.time + availableTime.initialValue;
    }

    private void Update() {
        if (isRealWorld && Time.time > endTime) {
            StateControl.Instance.playerPos = player.position;
            SceneManager.LoadScene(scenceToLoad);
        }
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
