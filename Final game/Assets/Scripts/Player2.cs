using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Player2 : MonoBehaviour
{

    public AudioClip jumpClip;
    public AudioClip hurtClip;
    public AudioClip deathClip;
    public float speed = 5f;
    public float jumpforce = 10f;
    public int extraJumpsValue = 1;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public TextMeshProUGUI keyText;
    public float invincibilityTime = 1f;


    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private int extraJumps;
    private bool isGrounded;
    private bool isInvincible = false;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        extraJumps = extraJumpsValue;
        keyText = GameObject.FindWithTag("KeyText").GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce);
                audioSource.PlayOneShot(jumpClip);
            }
            else if (extraJumps > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce);
                extraJumps--;
                audioSource.PlayOneShot(jumpClip);
            }
        }
        SetAnimator(moveInput);
        if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void SetAnimator(float moveInput)
    {
        if (isGrounded)
        {
            if (moveInput == 0)
            {
                anim.Play("Idle_player_2");
            }
            else
            {
                anim.Play("Running_player_2");
            }
        }
        else
        {
            anim.Play("Jump_player_2");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Damage")
        {
            StartCoroutine(TakeDamage(25));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "FallBar")
        {
            GameManager.Instance.ResetKeys();
            UnityEngine.SceneManagement.SceneManager.LoadScene("InsideCave");
        }
        if (collision.gameObject.tag == "Door")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Outro");
        }
    }

    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private IEnumerator Die()
    {
        anim.Play("Death_player_2");
        audioSource.PlayOneShot(deathClip);
        yield return new WaitForSeconds(2f);
        GameManager.Instance.health = 100;
        GameManager.Instance.ResetKeys();
        UnityEngine.SceneManagement.SceneManager.LoadScene("InsideCave");
    }
    
    private IEnumerator TakeDamage(int damage)
    {
        isInvincible = true;

        GameManager.Instance.health -= damage;
        audioSource.PlayOneShot(hurtClip);
        StartCoroutine(BlinkRed());
        if (GameManager.Instance.health <= 0)
        {
            yield return StartCoroutine(Die());
        }
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }
}
