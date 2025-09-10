using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float enemyMovement;
    public bool spawnLeft;
    public float changeYValue = 2f;
    public float lifeTime = 15f;
    public float lifeTimer;
    [SerializeField] private Vector2 direction;
    private Rigidbody2D _rb;
    private float timer;
    
    private void OnBecameInvisible()
    {
        if (lifeTimer > 1f)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (spawnLeft)
            direction = new Vector2(1, 0);
        else
            direction = new Vector2(-1, 0);

    }
    void Update()
    {
        lifeTimer += Time.deltaTime;  
    }
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer > changeYValue)
        {
            int random = Random.Range(-1, 1);
            direction = new Vector2(direction.x, random);
            timer = 0;
        }
        _rb.linearVelocity = enemyMovement * direction;
    }
}
