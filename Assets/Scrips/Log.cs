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
    private int randomSpot;

    // Start is called before the first frame update
    void Start()
    {

        homePosition = transform.position;
        currentState = EnemyState.walk;
        target = GameObject.FindWithTag("Player").transform;
        myRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // // Update is called once per frame
    // void FixedUpdate()
    // {
    //     CheckDistance();
    // }

    // void Patrol() {
    //     transform.position = Vector3.MoveTowards(transform.position, moveSpots[randomSpot].position, moveSpeed * Time.deltaTime);

    //     if (Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f) {
    //         ChangeSpot();
    //     }
    // }

    // void ChangeSpot() {
    //     Debug.Log("Change Spot");
    //     if (waitTime <= 0) {
    //         int currentSpot = randomSpot;
    //         while (currentSpot == randomSpot) {
    //             randomSpot = Random.Range(0, moveSpots.Length);
    //         }
    //         waitTime = startWaitTime;
    //     } else {
    //         waitTime -= Time.deltaTime;
    //     }
    // }

    // void CheckDistance() {
    //     float distance = Vector3.Distance(target.position, transform.position);
    //     if (distance <= chaseRadius && distance > attackRadius 
    //         && currentState != EnemyState.stagger) {
    //         Attack();

    //         ChangeState(EnemyState.attack);
    //         // anim.SetBool("wakeUp", true);
    //     } else {
    //         Patrol();
    //         ChangeState(EnemyState.walk);

    //         // Vector3 temp = Vector3.MoveTowards(transform.position, homePosition, moveSpeed * Time.deltaTime);
    //         // myRb.MovePosition(temp);

    //         // if (transform.position == homePosition) {
    //             // anim.SetBool("wakeUp", false);
    //         // }
    //     }
    // }

    // private void Attack() {
    //     Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

    //     ChangeAnim(temp - transform.position);
    //     myRb.MovePosition(temp);
    // }

    

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
