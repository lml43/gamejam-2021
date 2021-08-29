using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knowback : MonoBehaviour
{

    public float thrust;
    public float knockTime;
    public int damage;

    private void OnTriggerEnter2D(Collider2D other) {
        if (gameObject.CompareTag("Player")) {
            FindObjectOfType<AudioManager>().Play("Hit");
            if (other.gameObject.CompareTag("FireBox")) {
                other.GetComponent<Breakable>().Smash();
                StateControl.Instance.didFireBoxBroke = true;
            }

            if (other.gameObject.CompareTag("WrenchBox")) {
                other.GetComponent<Breakable>().Smash();
                StateControl.Instance.didWrenchBoxBroke = true;
            }
        }

        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Boss") || other.gameObject.CompareTag("Student")) {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();

            if (hit != null) {
                bool isOtherEnemy = other.gameObject.CompareTag("Enemy");
                bool isOtherPlayer = other.gameObject.CompareTag("Player");
                bool isOtherBoss = other.gameObject.CompareTag("Boss");
                bool isOtherStudent = other.gameObject.CompareTag("Student");
                
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);

                if(isOtherEnemy) {
                    hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
                    other.GetComponent<Enemy>().Knock(hit, knockTime, damage);
                } else if(isOtherBoss) {
                    other.GetComponent<Boss>().TakeDamage(damage);
                } else if (isOtherStudent) {
                    other.GetComponent<Student>().Knock(knockTime, damage);
                } else if (isOtherPlayer && other.GetComponent<PlayerManager>().currentState.runtimeValue != PlayerState.stagger){
                    other.GetComponent<PlayerManager>().currentState.runtimeValue = PlayerState.stagger;
                    other.GetComponent<PlayerManager>().Knock(knockTime, damage);
                }
            } 
        }
    }

}
