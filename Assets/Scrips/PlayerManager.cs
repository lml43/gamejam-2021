using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public IntValue playerHealth;
    public PlayerStateValue currentState;
    public int heartValue;
    public float untouchableTime;

    private bool isProtected = false;
    private Rigidbody2D myRigidbody;
    private SpriteRenderer renderer;

    void Start() {
        myRigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
    }
    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Item")) {
            other.gameObject.SetActive(false);
            IncreaseHealth(heartValue);
        }
        
        if (other.gameObject.CompareTag("Mask")) {
            other.gameObject.SetActive(false);
            isProtected = true;
        }
    }

    public void Blur() {
        renderer.color = new Color(1f,1f,1f,.5f);
        currentState.runtimeValue = PlayerState.untouchable;
        
        Physics2D.IgnoreLayerCollision(9, 10);

        StartCoroutine(UnBlur(untouchableTime));
    }

    public void Knock(float knockTime, int damage) {
        // If wearing mask
        if (isProtected) {
            currentState.runtimeValue = PlayerState.stagger;
            isProtected = false;
        } else {
            playerHealth.runtimeValue -= damage;
        }


        if (playerHealth.runtimeValue > 0) {
            StartCoroutine(KnockCo(knockTime));
        } else {
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator UnBlur(float untouchableTime) {
        yield return new WaitForSeconds(untouchableTime);
        currentState.runtimeValue = PlayerState.idle;
        renderer.color = new Color(1f,1f,1f,1f);
        Physics2D.IgnoreLayerCollision(9, 10, false);
    }

    private IEnumerator KnockCo(float knockTime) {
        yield return new WaitForSeconds(knockTime);
        myRigidbody.velocity = Vector2.zero;
        Blur();
    }

    private void IncreaseHealth(int value) {
        playerHealth.runtimeValue = Mathf.Min(playerHealth.initialValue, playerHealth.runtimeValue + value);
    }
}
