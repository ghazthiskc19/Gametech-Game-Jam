using System.Xml.Serialization;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float spawnDuration;
    public GameObject[] spawner;
    private float timer;
    [Header("Enemy List")]
    public GameObject BasicEnemy;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnDuration)
        {
            SpawnEnemy();
            timer = 0;
        }
    }

    private void SpawnEnemy()
    {
        int random = Random.Range(1, 11);
        bool spawnLeft = random % 2 == 0 ? true : false;


        float minY = Camera.main.transform.position.y - Camera.main.orthographicSize;
        float maxY = Camera.main.transform.position.y + Camera.main.orthographicSize;
        float randomY = Random.Range(minY, maxY);

        Vector2 spawnPos;
        if (spawnLeft)
        {
            spawnPos = new Vector2(spawner[0].transform.position.x, randomY);
        }
        else
        {
            spawnPos = new Vector2(spawner[1].transform.position.x, randomY);
        }


        GameObject enemy = Instantiate(BasicEnemy, spawnPos, Quaternion.identity);
        enemy.GetComponent<EnemyMovement>().spawnLeft = spawnLeft;
    }
}
