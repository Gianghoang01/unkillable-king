using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 15f; // Sát thương của mỗi viên đạn

    // Dùng OnTriggerEnter2D nếu Collider của đạn đang tích ô 'Is Trigger'
    // Dùng OnCollisionEnter2D nếu Collider của đạn KHÔNG tích 'Is Trigger'
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // 1. Nếu đạn chạm vào người chơi -> Bỏ qua, để đạn bay tiếp
        if (hitInfo.CompareTag("Player")) return;

        // 2. Nếu đạn chạm vào vùng bẫy của phòng (Box Collider trên roomTemplate) -> Bỏ qua
        if (hitInfo.GetComponent<Room>() != null) return;

        // 3. Nếu đạn trúng quái vật -> Gây sát thương
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // 4. Nếu chạm tường, quái, hoặc bất cứ thứ gì khác ngoài 2 trường hợp trên -> Hủy đạn
        Destroy(gameObject);
    }
}