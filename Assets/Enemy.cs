using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Chỉ số Quái vật")]
    public float maxHealth = 50f;
    private float currentHealth;

    public float moveSpeed = 3f;
    public Transform player;
    public Rigidbody2D rb;

    private Vector2 moveDirection;

    void Start()
    {
        currentHealth = maxHealth; // Đổ đầy máu khi quái sinh ra

        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            moveDirection = (player.position - transform.position).normalized;
        }
    }

    void FixedUpdate()
    {
        if (player != null && rb != null)
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // --- PHẦN MỚI: Hàm xử lý nhận sát thương ---
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    [Header("Hệ thống rơi đồ")]
    public GameObject[] lootPrefabs;  // Kéo các Prefab Vàng, Máu, Mana vào đây
    [Range(0f, 1f)] public float dropChance = 0.5f; // Tỷ lệ rơi (0.5 = 50%)

    void Die()
    {
        // Kiểm tra tỷ lệ rơi và danh sách đồ
        if (Random.value <= dropChance && lootPrefabs != null && lootPrefabs.Length > 0)
        {
            // Chọn ngẫu nhiên 1 vật phẩm
            GameObject itemToDrop = lootPrefabs[Random.Range(0, lootPrefabs.Length)];
            Instantiate(itemToDrop, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(10f);
            }
        }
    }
}