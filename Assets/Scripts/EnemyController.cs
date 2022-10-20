using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 15.0f;
    public float changeTime = 0.5f;
    public bool vertical;
    bool broken = true;

    Rigidbody2D rigidbody2D;
    Animator animator;
    float timer;
    int direction = 1;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!broken)
        {
            return;
        }
        timer -= Time.deltaTime;

        

        if(timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log("broken " + broken);
        if (!broken)
        {
            Debug.Log("broken is false " + broken);
            return;
        }
        Vector2 position = rigidbody2D.position;
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        rigidbody2D.MovePosition(position);

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RubyController player = collision.gameObject.GetComponent<RubyController>();
        if(player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    public void Fix()
    {
        broken = false;
        rigidbody2D.simulated = false;
        animator.SetTrigger("Fixed");
    }
}
