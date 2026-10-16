using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType { Gold, XP, Health, Mana }

    [Header("Cài đặt Vật phẩm")]
    public ItemType type;   // Loại vật phẩm
    public int amount = 10; // Số lượng hoặc chỉ số hồi phục

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Khi người chơi chạm vào
        if (collision.CompareTag("Player"))
        {
            // Fix CS0411: specify type parameter for GetComponent
            PlayerStats stats = collision.GetComponent<PlayerStats>();

            if (stats != null)
            {
                // Cộng chỉ số tương ứng theo loại vật phẩm
                switch (type)
                {
                    case ItemType.Gold:
                        stats.AddGold(amount);
                        break;
                    case ItemType.XP:
                        stats.AddXP(amount);
                        break;
                    case ItemType.Health:
                        stats.Heal(amount);
                        break;
                    case ItemType.Mana:
                        stats.AddMana(amount); // Đồng bộ gọi hàm AddMana
                        break;
                }

                // Xóa vật phẩm khỏi map sau khi nhặt
                Destroy(gameObject);
            }
        }
    }
}