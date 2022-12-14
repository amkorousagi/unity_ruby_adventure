using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    AudioSource audioSource;
    public ParticleSystem hitEffect;
    public List<AudioClip> audioClip;
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
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
        GameObject.Instantiate<ParticleSystem>(hitEffect, collision.collider.gameObject.transform);
        Debug.Log(collision.gameObject);
        Debug.Log(collision.collider);
        Debug.Log(Random.Range(0, 10) < 5 ? audioClip[0] : audioClip[1]);
        AudioClip a = Random.Range(0, 10) < 5 ? audioClip[0] : audioClip[1];
        audioSource.PlayOneShot(a);
        

        EnemyController e = collision.collider.GetComponent<EnemyController>();
        if (e != null)
        {
            GameObject.Instantiate<ParticleSystem>(hitEffect, e.gameObject.transform);
            e.Fix();
                
        }
        Debug.Log("Projectile Collistion with" + collision.gameObject);

        
        //Destroy(gameObject);
    }
}
