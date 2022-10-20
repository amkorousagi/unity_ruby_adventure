using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject);
        Debug.Log(collision.collider);
        EnemyController e = collision.collider.GetComponent<EnemyController>();
        if(e != null)
        {
            e.Fix();
        }

        Debug.Log("Projectile Collistion with" + collision.gameObject);
        if(collision.gameObject.GetComponent<RubyController>() == null)
            Destroy(gameObject);
    }
}
