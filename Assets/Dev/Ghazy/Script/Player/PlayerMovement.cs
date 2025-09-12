using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed;
    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 3f;
    public int dashHealthReduction = 2;
    public bool isHoldInteract = false;
    public bool _isDashing;
    [Header("Dolphin Jump")]
    public float jumpForce = 30f;
    public float durationAfterJump = .3f;
    private bool _isDolphinJumping = false;
    [SerializeField] private bool _canDash;
    private Vector2 _moveInput;
    private Rigidbody2D _rb;
    private PlayerHealth _playerHealth;
    private Animator _animator;
     private PlayerInteract _playerInteract;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerHealth = GetComponent<PlayerHealth>();
        _rb = GetComponent<Rigidbody2D>();
        _playerInteract = GetComponentInChildren<PlayerInteract>();
    }

    void Update()
    {
        HandleAnimation();
    }
    void FixedUpdate()
    {
        if (_isDolphinJumping) return;
        if (_isDashing)
        {
            _rb.linearVelocity = dashSpeed * _moveInput;
        }
        else
        {
            _rb.linearVelocity = movementSpeed * _moveInput;
        }
    }
  public IEnumerator DoDolphinJump(Transform sampah, Transform shoreTransform)
    {
        _isDolphinJumping = true;
        
        _playerHealth.effectsPaused = true;
        _rb.gravityScale = 5f;

        _rb.linearVelocity = Vector2.zero;
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        yield return new WaitUntil(() => _rb.linearVelocityY < 0.1f);

        InteracableObject obj = sampah.GetComponent<InteracableObject>();
        EventManager.instance.WhenObjectDropped(obj.amountScore, sampah.gameObject);
        
        sampah.SetParent(shoreTransform);
        Destroy(sampah.gameObject);

        _playerInteract.isHolding = false;

        yield return new WaitForSeconds(durationAfterJump); // Sesuaikan durasi ini

        _playerHealth.effectsPaused = false;
        _isDolphinJumping = false;
        
        _rb.gravityScale = 0; 
    }
    private void HandleAnimation()
    {
        if (_moveInput != Vector2.zero)
        {
            _animator.SetBool("IsMoving", true);
            _animator.SetFloat("MoveX", _moveInput.x);
            _animator.SetFloat("MoveY", _moveInput.y);
        }
        else
        {
            _animator.SetBool("IsMoving", false);
        }
    }
    public IEnumerator Dashing()
    {
        _playerHealth.currentHealth -= dashHealthReduction;
        _isDashing = true;
        _canDash = false;

        yield return new WaitForSeconds(dashDuration);
        _isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }
    public void OnMove(InputValue input)
    {
        _moveInput = input.Get<Vector2>().normalized;
    }
    public void OnDash(InputValue input)
    {
        if (input.isPressed && _canDash && _moveInput != Vector2.zero)
            StartCoroutine(Dashing());
    }

    public void OnInteract(InputValue input)
    {
        isHoldInteract = input.isPressed;
    }
}

