using UnityEngine;

public class BossFireballShooter : MonoBehaviour
{
    [Header("Fireball Settings")]
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireInterval = 4f;
    public float fireballSpeed = 10f;

    [Header("Enemy Spawn Settings")]
    [SerializeField] private GameObject[] enemyPrefabs; // Multiple enemy types
    public float spawnInterval = 25f;
    public int enemiesPerSpawn = 5;
    public float spawnRadius = 5f;

    private Transform player;
    private float fireTimer;
    private float spawnTimer;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("Player not found. Make sure your player GameObject is tagged as 'Player'.");

        fireTimer = fireInterval;
        spawnTimer = spawnInterval;
    }

    void Update()
    {
        if (player == null) return;

        // Rotate boss to face the player smoothly
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;
        if (directionToPlayer.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        // Fireball shooting timer
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            FireAtPlayer();
            fireTimer = fireInterval;
        }

        // Enemy spawning timer
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnEnemies();
            spawnTimer = spawnInterval;
        }
    }

    void FireAtPlayer()
    {
        Vector3 direction = (player.position - firePoint.position).normalized;

        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);

        Collider bossCollider = GetComponent<Collider>();
        Collider fireballCollider = fireball.GetComponent<Collider>();
        if (bossCollider != null && fireballCollider != null)
        {
            Physics.IgnoreCollision(fireballCollider, bossCollider);
        }

        BossFireball fireballScript = fireball.GetComponent<BossFireball>();
        if (fireballScript != null)
        {
            fireballScript.Launch(direction, fireballSpeed);
        }
        else
        {
            Debug.LogWarning("Fireball prefab missing BossFireball script.");
        }
    }

    void SpawnEnemies()
    {
        if (enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("No enemy prefabs assigned!");
            return;
        }

        for (int i = 0; i < enemiesPerSpawn; i++)
        {
            Vector3 offset = Random.insideUnitSphere * spawnRadius;
            offset.y = 0f;
            Vector3 spawnPosition = transform.position + offset;

            // Randomly pick an enemy type
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
    }
}
