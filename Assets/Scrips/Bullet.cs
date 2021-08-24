using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed;
    public Rigidbody2D myRb;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetUp(Vector2 velocity, Vector3 direction) {
        myRb.velocity = velocity.normalized * speed;
        transform.rotation = Quaternion.Euler(direction);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Destroy(this.gameObject);
    }
}
