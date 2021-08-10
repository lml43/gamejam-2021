using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knowback : MonoBehaviour
{

    public float thrust;
    public float knockTime;
    public int damage;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Breakable") && gameObject.CompareTag("Player")) {
            other.GetComponent<Pot>().Smash();
        }

        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player")) {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();

            if (hit != null) {
                bool isEnemy = other.gameObject.CompareTag("Enemy");
                
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);

                if(isEnemy) {
                    hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
                    other.GetComponent<Enemy>().Knock(hit, knockTime, damage);
                } else if (other.GetComponent<PlayerManager>().currentState.runtimeValue != PlayerState.stagger){
                    other.GetComponent<PlayerManager>().currentState.runtimeValue = PlayerState.stagger;
                    other.GetComponent<PlayerManager>().Knock(knockTime, damage);
                }

                
            } 
        }
    }

}
