using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed;
    [Header("Movement Boundary")]
    public BoxCollider2D playableArea;
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
    public bool _isDolphinJumping = false;
    [SerializeField] private bool _canDash;
    private Vector2 _moveInput;
    private Rigidbody2D _rb;
    private PlayerHealth _playerHealth;
    private Animator _animator;
    private PlayerInteract _playerInteract;
    private bool _isDead = false;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerHealth = GetComponent<PlayerHealth>();
        _rb = GetComponent<Rigidbody2D>();
        _playerInteract = GetComponentInChildren<PlayerInteract>();
        EventManager.instance.OnPlayerDied += HandlePlayerDied;
    }
    private void OnDisable()
    {
        EventManager.instance.OnPlayerDied -= HandlePlayerDied;
    }

    void Update()
    {
        HandleAnimation();
    }
    void FixedUpdate()
    {
        if (_isDead) return;
        if (_isDashing)
        {
            _rb.linearVelocity = dashSpeed * _moveInput;
        }
        else if (_isDolphinJumping)
        {
            _rb.linearVelocity = new Vector2(
                _moveInput.x * movementSpeed,
                _rb.linearVelocity.y
            );
        }
        else
        {
            _rb.linearVelocity = movementSpeed * _moveInput;
        }
    }

    void LateUpdate()
    {
        if (_isDolphinJumping || playableArea == null)
        {
            return;
        }

        Bounds areaBounds = playableArea.bounds;
        float minX = areaBounds.min.x;
        float maxX = areaBounds.max.x;
        float minY = areaBounds.min.y;
        float maxY = areaBounds.max.y;

        Vector3 currentPosition = transform.position;

        float clampedX = Mathf.Clamp(currentPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(currentPosition.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, currentPosition.z);
    }
    public IEnumerator DoDolphinJump(Transform sampah = null)
    {
        _isDolphinJumping = true;

        _playerHealth.effectsPaused = true;
        _rb.gravityScale = 5f;

        _rb.linearVelocity = Vector2.zero;
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        yield return new WaitUntil(() => _rb.linearVelocityY < 0.1f);
        if (sampah != null)
        {
            InteracableObject obj = sampah.GetComponent<InteracableObject>();
            EventManager.instance.WhenObjectDropped(obj.amountScore, sampah.gameObject);

            Destroy(sampah.gameObject);
            Debug.Log("masuk sini");
            _playerInteract.isHolding = false;
        }


        yield return new WaitUntil(() => _playerHealth.IsInSaveZone);

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
        _animator.SetBool("IsDashing", true);

        yield return new WaitForSeconds(dashDuration);

        _isDashing = false;
        _animator.SetBool("IsDashing", false);

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
    private void HandlePlayerDied()
    {
        _isDead = true;
        _rb.linearVelocity = Vector2.zero; // stop gerakan
        _animator.SetBool("IsMoving", false);
        _animator.SetBool("IsDashing", false);

        Debug.Log("Player mati, movement dimatiin!");
    }
}

