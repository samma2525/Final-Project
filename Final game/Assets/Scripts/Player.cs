using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    public float speed = 3f;
    public int keys;
    public Rigidbody2D rb;
    public Animator anim;
    public AudioClip hurtClip;
    public AudioClip deathClip;
    public float invincibilityTime = 1f;
    


    private SpriteRenderer spriteRenderer;
    private Vector2 movement;
    private AudioSource audioSource;
    private bool isInvincible = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement = new Vector2(horizontal, vertical).normalized;

        if (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.DownArrow)))
        {
            anim.Play("Walking_front_player");
        }
        else if (Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.UpArrow)))
        {
            anim.Play("Walking_back_player");
        }
        else if (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.RightArrow)))
        {
            anim.Play("Walking_right_player");
            Debug.Log("Moving Right");
        }
        else if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.LeftArrow)))
        {
            anim.Play("Walking_left_player");
        }
        else
        {
            anim.Play("idle_player");
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movement * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);
        if (collision.gameObject.tag == "Damage" && !isInvincible)
        {
            StartCoroutine(TakeDamage(20));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CaveEnter" && GameManager.Instance.keys == 3)
        {
            SceneManager.LoadScene("CaveIntroScene");
        }
    }

    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    public void PlaySFX(AudioClip audioClip, float volume = 1f)
    {
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    private IEnumerator TakeDamage(int damage)
    {
        isInvincible = true;

        GameManager.Instance.health -= damage;
        PlaySFX(hurtClip);
        StartCoroutine(BlinkRed());
        if (GameManager.Instance.health <= 0)
        {
            yield return StartCoroutine(Die());
        }
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }

    private IEnumerator Die()
    {
        PlaySFX(deathClip);
        yield return new WaitForSeconds(2f);
        GameManager.Instance.health = 100;
        GameManager.Instance.ResetKeys();
        SceneManager.LoadScene("SampleScene");
    }
}
