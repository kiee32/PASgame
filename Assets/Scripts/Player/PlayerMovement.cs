using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Harus sama dengan Threshold di Blend Tree Move dan Idle
    private const float BlendDown = 0f;
    private const float BlendLeft = 1f / 3f;
    private const float BlendRight = 2f / 3f;
    private const float BlendUp = 1f;

    [SerializeField] private float speed = 5f;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 direct;

    public int score = 0;
    public GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        if (gameManager == null)
            gameManager = FindAnyObjectByType<GameManager>();
    }

    // Dipanggil PlayerInput (Behavior: Send Messages) untuk action "Move"
    void OnMove(InputValue value)
    {
        direct = value.Get<Vector2>();
    }

    void Update()
    {
        if (rb != null)
            rb.linearVelocity = direct * speed;

        bool isWalking = direct.sqrMagnitude > 0.01f;
        if (anim != null)
            anim.SetBool("IsWalking", isWalking);

        if (!isWalking || anim == null) return;

        float blend;
        if (Mathf.Abs(direct.x) > Mathf.Abs(direct.y))
            blend = direct.x > 0 ? BlendRight : BlendLeft;
        else
            blend = direct.y > 0 ? BlendUp : BlendDown;

        anim.SetFloat("Blend", blend);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            score++;
            Debug.Log("Score: " + score);
            if (gameManager != null)
                gameManager.AmbilKoin();
        }
    }
}