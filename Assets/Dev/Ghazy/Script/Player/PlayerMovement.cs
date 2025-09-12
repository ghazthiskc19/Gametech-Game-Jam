using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
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
    [SerializeField] private bool _canDash;
    [SerializeField] private bool _isDashing;
    private Vector2 _moveInput;
    private Rigidbody2D _rb;
    private PlayerHealth _playerHealth;
    void Start()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (_isDashing)
        {
            _rb.linearVelocity = dashSpeed * _moveInput;
            _playerHealth.currentHealth -= dashHealthReduction;
        }
        else
        {
            _rb.linearVelocity = movementSpeed * _moveInput;
        }
    }
    public IEnumerator Dashing()
    {
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

