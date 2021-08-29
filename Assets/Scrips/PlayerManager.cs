using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public IntValue playerHealth;
    public PlayerStateValue currentState;
    public float untouchableTime;
    public BoolValue isFirstLoad;

    private bool isProtected = false;
    private Rigidbody2D myRigidbody;
    private SpriteRenderer renderer;
    private Animator anim;

    void Awake() {

        if (!isFirstLoad.runtimeValue) {
            transform.position = StateControl.Instance.playerPos;
        }

        myRigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

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
            FindObjectOfType<AudioManager>().Play("Hurt");
            playerHealth.runtimeValue -= damage;
        }


        if (playerHealth.runtimeValue > 0) {
            StartCoroutine(KnockCo(knockTime));
        } else {
            FindObjectOfType<AudioManager>().Play("Dead");
            anim.SetBool("isDead", true);
            StartCoroutine(ReloadGame());
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

    private IEnumerator ReloadGame() {
        yield return new WaitForSeconds(1);
        FindObjectOfType<GameManager>().ShowLostPopups();
    }

}
