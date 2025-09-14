using UnityEngine;
using System.Collections;
public class EnemyMovement : MonoBehaviour
{
    public float enemyMovement;
    public bool spawnLeft;
    public float changeYValue = 1f;
    [Header("Movement Pattern")]
    public float straightMoveDuration = 2f;
    public float randomMoveDuration = 2f;
    [SerializeField] private Vector2 direction;
    private Rigidbody2D _rb;
    private Animator _animator;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        if (spawnLeft)
            direction = new Vector2(1, 0);
        else
            direction = new Vector2(-1, 0);
    }

    void Update()
    {
        HandleAnimation();
    }
    void FixedUpdate()
    {
        _rb.linearVelocity = enemyMovement * direction;
    }
    private void HandleAnimation()
    {
        _animator.SetFloat("MoveX", _rb.linearVelocityX);
    }
    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("spawnArena"))
        {
            Destroy(gameObject);
        }
    }
}
