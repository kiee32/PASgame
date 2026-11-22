using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxEnemies = 5;
    [SerializeField] private Vector2 areaSize = new Vector2(10f, 6f);
    [SerializeField] private float minDistanceFromPlayer = 3f;

    private Transform player;
    private PlayerHealth playerHealth;
    private float timer;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        // Berhenti spawn kalau player sudah mati
        if (playerHealth != null && playerHealth.CurrentHealth <= 0) return;

        timer += Time.deltaTime;
        if (timer < spawnInterval) return;
        timer = 0f;

        // Enemy yang sudah ada di scene juga ikut dihitung
        int alive = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
        if (alive >= maxEnemies) return;

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        if (prefab == null) return;

        Instantiate(prefab, RandomSpawnPoint(), Quaternion.identity);
    }

    Vector3 RandomSpawnPoint()
    {
        Vector2 point = transform.position;

        // Coba beberapa kali supaya tidak muncul tepat di dekat player
        for (int i = 0; i < 10; i++)
        {
            point = (Vector2)transform.position + new Vector2(
                Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
                Random.Range(-areaSize.y / 2f, areaSize.y / 2f));

            if (player == null || Vector2.Distance(point, player.position) >= minDistanceFromPlayer)
                break;
        }

        return new Vector3(point.x, point.y, 0f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }
}