using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public bool isPlayer1 = true;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 lastMove;

    // Flag zur Steuerung der Bewegung
    private bool canMove = true;

    #region Initialization
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    #endregion

    #region Main Update Loop
    private void Update()
    {
        if (canMove)
        {
            HandleMovement();
            UpdateAnimatorParameters();
        }
    }
    #endregion

    #region Physics Update Loop
    private void FixedUpdate()
    {
        if (canMove)
        {
            MovePlayer();
        }
    }
    #endregion

    #region Input Handling
    private void HandleMovement()
    {
        if (isPlayer1)
        {
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else
        {
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal2"), Input.GetAxisRaw("Vertical2"));
        }

        if (Mathf.Abs(moveInput.x) > 0.1f && Mathf.Abs(moveInput.y) > 0.1f)
        {
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                moveInput.y = 0;
            }
            else
            {
                moveInput.x = 0;
            }
        }

        if (moveInput.magnitude > 0)
        {
            lastMove = moveInput;
        }
    }
    #endregion

    #region Animator Updates
    private void UpdateAnimatorParameters()
    {
        bool isMoving = moveInput.magnitude > 0;
        animator.SetFloat("moveX", moveInput.x);
        animator.SetFloat("moveY", moveInput.y);
        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("lastMoveX", lastMove.x);
        animator.SetFloat("lastMoveY", lastMove.y);
    }
    #endregion

    #region Player Movement
    private void MovePlayer()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Deaktiviert die Spielerbewegung.
    /// </summary>
    public void DisableMovement()
    {
        canMove = false;
        // Optional: Setze die aktuelle Geschwindigkeit auf Null
        rb.velocity = Vector2.zero;
        // Optional: Aktualisiere den Animator, um den Idle-Zustand zu reflektieren
        animator.SetBool("isMoving", false);
        Debug.Log(gameObject.name + " Bewegung deaktiviert.");
    }

    /// <summary>
    /// Aktiviert die Spielerbewegung.
    /// </summary>
    public void EnableMovement()
    {
        canMove = true;
        Debug.Log(gameObject.name + " Bewegung aktiviert.");
    }
    #endregion
}
