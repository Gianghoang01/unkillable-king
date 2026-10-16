using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    [Header("Phần thưởng rương")]
    public GameObject[] rewardPrefabs; // Các vật phẩm quý (Vàng lớn, Vũ khí, Buff...)
    public int minDrop = 3;
    public int maxDrop = 6;

    private bool isOpen = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Khi người chơi chạm vào và rương chưa mở
        if (collision.CompareTag("Player") && !isOpen)
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        isOpen = true;

        // TODO: Bạn có thể đổi hình ảnh rương mở ở đây bằng GetComponent().sprite

        int dropCount = Random.Range(minDrop, maxDrop + 1);
        for (int i = 0; i < dropCount; i++)
        {
            if (rewardPrefabs.Length == 0) break;

            GameObject reward = rewardPrefabs[Random.Range(0, rewardPrefabs.Length)];

            // Tính toán vị trí văng ra ngẫu nhiên xung quanh rương
            Vector3 randomOffset = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0);
            Instantiate(reward, transform.position + randomOffset, Quaternion.identity);
        }

        Debug.Log("Đã mở rương báu!");
    }
}