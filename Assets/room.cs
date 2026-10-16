using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Các mảng tường dùng để bịt cửa")]
    public GameObject wallUp;
    public GameObject wallDown;
    public GameObject wallLeft;
    public GameObject wallRight;

    [Header("Cấu hình sinh Quái vật")]
    public GameObject[] enemyPrefabs;
    public int minEnemies = 2;
    public int maxEnemies = 5;

    [Header("Cấu hình Phòng Thử Thách")]
    public bool isChallengeRoom = false; // Tích vào ô này trên Inspector nếu đây là phòng sinh tồn
    public float survivalTime = 30f;     // Thời gian phải sống sót (giây)
    public float enemySpawnInterval = 3f; // Cứ bao lâu sinh thêm 1 đợt quái

    private float currentSurvivalTime;
    private float spawnTimer;

    private bool pathUp, pathDown, pathLeft, pathRight;
    private bool isCleared = false;
    private bool hasTriggered = false;

    // Active enemies in this room
    private List<GameObject> activeEnemies = new List<GameObject>();

    public void UpdateDoors(bool hasUp, bool hasDown, bool hasLeft, bool hasRight)
    {
        pathUp = hasUp; pathDown = hasDown; pathLeft = hasLeft; pathRight = hasRight;
        UnlockDoors();

        currentSurvivalTime = survivalTime;
    }

    public void SpawnEnemies()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            isCleared = true;
            return;
        }

        // Sinh đợt quái đầu tiên (ẩn đi chờ bẫy sập)
        SpawnWave();
    }

    void SpawnWave()
    {
        int enemyCount = Random.Range(minEnemies, maxEnemies + 1);
        for (int i = 0; i < enemyCount; i++)
        {
            float randomX = Random.Range(-7f, 7f);
            float randomY = Random.Range(-3f, 3f);
            Vector3 spawnPos = transform.position + new Vector3(randomX, randomY, 0);

            GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            GameObject spawnedEnemy = Instantiate(randomEnemy, spawnPos, Quaternion.identity, transform);

            if (!hasTriggered) spawnedEnemy.SetActive(false); // Ẩn đi nếu bẫy chưa sập
            activeEnemies.Add(spawnedEnemy);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCleared && !hasTriggered)
        {
            StartEncounter();
        }
    }

    void StartEncounter()
    {
        hasTriggered = true;
        LockDoors();

        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null) enemy.SetActive(true);
        }
    }

    void Update()
    {
        if (!hasTriggered || isCleared) return;

        activeEnemies.RemoveAll(item => item == null);

        // 1. LOGIC PHÒNG THỬ THÁCH (Đếm ngược)
        if (isChallengeRoom)
        {
            currentSurvivalTime -= Time.deltaTime;
            spawnTimer -= Time.deltaTime;

            // Đẻ thêm quái liên tục
            if (spawnTimer <= 0)
            {
                SpawnWave();
                spawnTimer = enemySpawnInterval;
            }

            // Hết giờ -> Chiến thắng
            if (currentSurvivalTime <= 0)
            {
                ClearRoom();
                // Hủy toàn bộ quái còn sót lại
                foreach (GameObject enemy in activeEnemies) Destroy(enemy);
                activeEnemies.Clear();
            }
        }
        // 2. LOGIC PHÒNG QUÁI THƯỜNG (Giết sạch)
        else
        {
            if (activeEnemies.Count == 0)
            {
                ClearRoom();
            }
        }
    }

    void ClearRoom()
    {
        isCleared = true;
        UnlockDoors();
        Debug.Log("Phòng đã được dọn sạch!");
    }

    void LockDoors()
    {
        if (wallUp != null) wallUp.SetActive(true);
        if (wallDown != null) wallDown.SetActive(true);
        if (wallLeft != null) wallLeft.SetActive(true);
        if (wallRight != null) wallRight.SetActive(true);
    }

    void UnlockDoors()
    {
        if (wallUp != null) wallUp.SetActive(!pathUp);
        if (wallDown != null) wallDown.SetActive(!pathDown);
        if (wallLeft != null) wallLeft.SetActive(!pathLeft);
        if (wallRight != null) wallRight.SetActive(!pathRight);
    }
}