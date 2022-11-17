using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;

    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    public int health { get { return currentHealth; } }
    public GameObject projectilePrefab;
    public Footstep footstep;

    int currentHealth;

    Rigidbody2D rigidbody2d;

    bool isInvincible;
    float invincibleTimer;

    float horizontal;
    float vertical;
    int offset=1;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        offset = Input.GetKey(KeyCode.X)?2:1;

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x,0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if(hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if(character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
    }
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }

    private void FixedUpdate()
    {
        Vector2 position = transform.position;
        position.x = position.x + offset*speed * horizontal * Time.deltaTime;
        position.y = position.y + offset*speed * vertical * Time.deltaTime;
        if(horizontal>0 | vertical > 0)
        {
            footstep.PlayFootstep();
        }
        else
        {
            footstep.StopFootstep();
        }
        rigidbody2d.MovePosition(position);

    }

    public void ChangeHealth(int amount)
    {
        //Debug.Log(UIHealthBar.instance);
        //Debug.Log(UIHealthBar.instance.ToString() + "/" + maxHealth);
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible) return;
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        //Debug.Log(UIHealthBar.instance.ToString() + "/" + maxHealth);
    }
}
