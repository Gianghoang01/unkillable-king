using UnityEngine;

public class shooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        PlayerStats stats = GetComponent<PlayerStats>();
        if (stats != null && stats.UseMana(5f)) // Tốn 5 mana mỗi viên
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = firePoint.up * bulletSpeed;
        }
    }
}
