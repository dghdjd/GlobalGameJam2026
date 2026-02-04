using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveInput;


    // Animation Variables
    Animator animator;
    private Vector2 lastMoveDirection = Vector2.down; // Default to facing down
    private bool canMove = true;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetMovementInteractionEnabled(bool enabled)
    {
        canMove = enabled;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Process Input
        ProcessInput();

        // Handle Animation
        HandleAnimation();

        HandleFootstep();
    }
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void HandleFootstep()
    {
        SfxPlayer sfx = FindAnyObjectByType<SfxPlayer>();
        if(!sfx) return;
        if (canMove && moveInput.magnitude > 0.1f)
        {  
            sfx.PlayFootstep();
        }
        else sfx.EndFootstep();
    }

    void ProcessInput()
    {
        // Handle Movement Input
        Vector2 keyboardInput = Vector2.zero;
        if (canMove && Keyboard.current != null)
        {

            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) {
                keyboardInput.y = 1;
            }
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) {
                keyboardInput.y = -1;
            }
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) {
                keyboardInput.x = -1;
            }
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) {
                keyboardInput.x = 1;
            }
        }

        // Set Move Input
        moveInput = keyboardInput;
        
        if (moveInput.magnitude > 0.1f)
        {
            lastMoveDirection = moveInput.normalized;
        }

    }

    void HandleAnimation(){
        if (animator == null) return;
        
        // Normalize moveInput
        Vector2 normalizedMove = moveInput.magnitude > 0.1f ? moveInput.normalized : Vector2.zero;
        
        // Set animator parameters
        animator.SetFloat("MoveX", normalizedMove.x);
        animator.SetFloat("MoveY", normalizedMove.y);
        animator.SetFloat("MoveMagnitude", moveInput.magnitude);
        animator.SetFloat("LastMoveX", lastMoveDirection.x);
        animator.SetFloat("LastMoveY", lastMoveDirection.y);
    }

}