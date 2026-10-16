using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    [Header("Cài đặt sát thương")]
    public float damage = 20f;               // Sát thương gây ra
    public float damageInterval = 1f;        // Nhịp gây sát thương liên tục nếu đứng dính vào nhau (giây)

    private float nextDamageTime = 0f;

    // Trường hợp 1: Vật thể dùng Collider thường (không tick Is Trigger)
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TryDealDamage(collision.gameObject);
        }
    }

    // Trường hợp 2: Vật thể dùng Trigger (có tick Is Trigger - ví dụ bẫy chông, vùng độc)
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TryDealDamage(collision.gameObject);
        }
    }

    private void TryDealDamage(GameObject target)
    {
        if (Time.time >= nextDamageTime)
        {
            PlayerStats stats = target.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(damage);
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }
}