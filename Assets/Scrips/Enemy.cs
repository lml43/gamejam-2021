using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState {
    idle,
    walk,
    attack,
    stagger
}

public class Enemy : MonoBehaviour
{

    public bool isTutorial;

    public EnemyState currentState;
    public int baseAttack;
    public float moveSpeed;
    public float deadTime = 2f;
    public IntValue health;

    public void ChangeState(EnemyState newState) {
        if (newState != currentState) {
            currentState = newState;
        }
    }

    public void Knock(Rigidbody2D myRb, float knockTime, int damage) {
        StartCoroutine(KnockCo(myRb, knockTime));
        TakeDamage(myRb, damage);
    }

    private void TakeDamage(Rigidbody2D myRb, int damage) {
        health.runtimeValue -= damage;

        if (health.runtimeValue <= 0) {
            this.gameObject.GetComponent<Animator>().SetBool("isDead", true);
            myRb.velocity = Vector2.zero;
            StartCoroutine(Inactive(deadTime));
        }
    }

    private IEnumerator Inactive(float time) {
        yield return new WaitForSeconds(time);
        this.gameObject.SetActive(false);
        if (isTutorial) {
            if (TutorialManager.step == 1) {
                TutorialManager.smashed = true;
            } else if (TutorialManager.step == 2) {
                TutorialManager.shooted = true;
            }
        }
    }

    private IEnumerator KnockCo(Rigidbody2D myRb, float knockTime) {
        if (myRb != null) {
            yield return new WaitForSeconds(knockTime);
            myRb.velocity = Vector2.zero;
            currentState = EnemyState.idle;
        }
    }
}
