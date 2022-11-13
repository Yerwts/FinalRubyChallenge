using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;

    public int maxHealth = 5;

    public GameObject projectilePrefab;

    public AudioClip throwSound;
    public AudioClip hitSound;

    public int health { get { return currentHealth; } }
    int currentHealth;

    public ParticleSystem HealEffect;
    public ParticleSystem HarmEffect;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    // Fixed Robots TMP Integers
    public TextMeshProUGUI fixedText;
    private int scoreFixed = 0;

    // Win text
    public GameObject WinTextObject;

    //Lose text
    public GameObject LoseTextObject;
    bool gameOver;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        HealEffect.Stop();
        HarmEffect.Stop();
        audioSource = GetComponent<AudioSource>();

        // Fixed Robot Text
        fixedText.text = "Fixed Robots: " + scoreFixed.ToString() + "/4";

        // Win Text
        WinTextObject.SetActive(false);
        LoseTextObject.SetActive(false);
        gameOver = false;

    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
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

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }

            }

        }
        // Close Game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            HarmEffect.Play();
            PlaySound(hitSound);
        }
        if (amount > 0)
        {
            HealEffect.Play();
        }
        if (currentHealth <= 0)
        {
            LoseTextObject.SetActive(true);

            transform.position = new Vector3(-5f, 0f, -100f);
            speed = 0;
            Destroy(gameObject.GetComponent<SpriteRenderer>());

            gameOver = true;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");

        PlaySound(throwSound);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    public void FixedRobots(int amount)
    {
        scoreFixed += amount;
        fixedText.text = "Fixed Robots: " + scoreFixed.ToString() + "/4";

        Debug.Log("Fixed Robots: " + scoreFixed);

        // Win Text Appears
        if (scoreFixed >= 6)
        {
            WinTextObject.SetActive(true);

            transform.position = new Vector3(-5f, 0f, -100f);
            speed = 0;

            Destroy(gameObject.GetComponent<SpriteRenderer>());

            gameOver = true;
        }
    }
}
public void FixedRobots(int amount)
{
    scoreFixed += amount;
    fixedText.text = "Fixed Robots: " + scoreFixed.ToString() + "/4";

    Debug.Log("Fixed Robots: " + scoreFixed);
}
}