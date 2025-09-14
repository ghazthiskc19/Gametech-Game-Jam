using System.ComponentModel;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    public BoxCollider2D waterArena;
    public BoxCollider2D objectiveArena;
    public float spawnDuration;
    public GameObject[] spawner;
    private float timer;

    [Header("Enemy List")]
    public GameObject Enemy1;
    public GameObject Enemy2;
    [Header("Objective List")]
    public GameObject[] ObjectivePrefab;
    public float objectiveSpawnInterval = 5f;
    public int maxObjectiveCount = 15;
    private float objectiveTimer;
    private int currentObjectiveCount = 0;
    private bool _isSpawningActive = true;
    void Awake() {
        if (instance == null)
        {
            instance = this;
        }    
    }
    private void Start()
    {
        EventManager.instance.OnPlayerDied += HandlePlayerDied;
    }
    private void OnDisable()
    {
        EventManager.instance.OnPlayerDied -= HandlePlayerDied;
    }
    void Update()
    {
        int episode = GameManager.instance.episode;
        timer += Time.deltaTime;
        if (timer >= spawnDuration)
        {
            SpawnEnemy(episode);
            timer = 0;
        }

        objectiveTimer += Time.deltaTime;
        if (objectiveTimer >= objectiveSpawnInterval)
        {
            SpawnObjective();
            objectiveTimer = 0;
        }
    }
    private void HandlePlayerDied()
    {
        _isSpawningActive = false;
    }
    private void SpawnEnemy(float wave)
    {
        if (!_isSpawningActive) return;
        int random = Random.Range(1, 11);
        bool spawnLeft = random % 2 == 0;

        var zoneBounds = waterArena.bounds;
        float minY = zoneBounds.min.y;
        float maxY = zoneBounds.max.y;
        float randomY = Random.Range(minY, maxY);

        Vector2 spawnPos = spawnLeft
            ? new Vector2(spawner[0].transform.position.x, randomY)
            : new Vector2(spawner[1].transform.position.x, randomY);
        GameObject enemy = null;
        if (wave == 1)
        {
            enemy = Instantiate(Enemy1, spawnPos, Quaternion.identity);
        }
        else if (wave == 2)
        {
            enemy = Instantiate(Enemy2, spawnPos, Quaternion.identity);
        }
        enemy.GetComponent<EnemyMovement>().spawnLeft = spawnLeft;
    }

    private void SpawnObjective()
    {
        if (ObjectivePrefab == null || objectiveArena == null) return;
        if (currentObjectiveCount >= maxObjectiveCount) return;

        int randomIdx = Random.Range(0, ObjectivePrefab.Length);
        var objective = ObjectivePrefab[randomIdx];

        var zoneBounds = objectiveArena.bounds;
        float minX = zoneBounds.min.x;
        float maxX = zoneBounds.max.x;
        float minY = zoneBounds.min.y;
        float maxY = zoneBounds.max.y;

        Vector2 spawnPos = new Vector2(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY)
        );

        Instantiate(objective, spawnPos, Quaternion.identity);
        currentObjectiveCount++;
    }

}
