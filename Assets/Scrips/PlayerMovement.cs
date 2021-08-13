using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed;
    public PlayerStateValue currentState;

    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();

        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
    }

    // Update is called once per frame
    void Update()
    {
        change = Vector2.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate() {
        if (Input.GetButtonDown("Attack") && currentState.runtimeValue != PlayerState.attack && currentState.runtimeValue != PlayerState.stagger && currentState.runtimeValue != PlayerState.untouchable) {
            StartCoroutine(AttackCo());
        } else if (currentState.runtimeValue == PlayerState.walk || currentState.runtimeValue == PlayerState.idle || currentState.runtimeValue == PlayerState.untouchable) {
            UpdateAnimationAndMove();
        }
    }

    private IEnumerator AttackCo() {
        animator.SetBool("attacking", true);
        currentState.runtimeValue = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        currentState.runtimeValue = PlayerState.walk;
    }

    void UpdateAnimationAndMove() {
        if (change != Vector3.zero) {   
            MoveChar();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("isMoving", true);
        } else {
            animator.SetBool("isMoving", false);
        }
    }

    void MoveChar() {
        change.Normalize(); 
        myRigidbody.MovePosition(
            transform.position + change * speed * Time.deltaTime
        );
    }

    
}
