using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : Enemy
{

    public Transform target;
    public Vector3 homePosition;
    public float chaseRadius;
    public float attackRadius;
    public Animator anim;

    private Rigidbody2D myRb;

    // Start is called before the first frame update
    void Start()
    {
        homePosition = transform.position;
        currentState = EnemyState.idle;
        target = GameObject.FindWithTag("Player").transform;
        myRb = GetComponent<Rigidbody2D>(); 
        anim = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckDistance();
    }

    void CheckDistance() {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= chaseRadius && distance > attackRadius 
            && (currentState == EnemyState.idle || currentState == EnemyState.walk)) {
            Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            ChangeAnim(temp - transform.position);
            myRb.MovePosition(temp);

            ChangeState(EnemyState.walk);
            anim.SetBool("wakeUp", true);
        } else {
            Vector3 temp = Vector3.MoveTowards(transform.position, homePosition, moveSpeed * Time.deltaTime);
            myRb.MovePosition(temp);

            if (transform.position == homePosition) {
                anim.SetBool("wakeUp", false);
            }
        }
    }

    private void ChangeState(EnemyState newState) {
        if (newState != currentState) {
            currentState = newState;
        }
    }

    private void ChangeAnim(Vector2 direction) {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
            if (direction.x > 0) {
                SetAnimVector(Vector2.right);
            } else if (direction.x < 0) {
                SetAnimVector(Vector2.left);
            }
        } else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y)) {
            if (direction.y > 0) {
                SetAnimVector(Vector2.up);
            } else if (direction.y < 0) {
                SetAnimVector(Vector2.down);
            }
        }
    }

    private void SetAnimVector(Vector2 vector) {
        anim.SetFloat("moveX", vector.x);
        anim.SetFloat("moveY", vector.y);
    }
}
