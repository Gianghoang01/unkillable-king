using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Mục Tiêu")]
    public Transform player;

    [Header("Kích Thước Phòng (Phải khớp với bản đồ)")]
    public float roomWidth = 20f;
    public float roomHeight = 12f;

    [Header("Tốc Độ Chuyển Cảnh")]
    public float moveSpeed = 10f; // Để tốc độ cao nếu muốn lướt nhanh, hoặc dùng transform.position thẳng nếu muốn chuyển tức thời

    void Update()
    {
        if (player == null) return;

        // Tính toán tọa độ tâm của phòng mà Player đang đứng
        float targetX = Mathf.Round(player.position.x / roomWidth) * roomWidth;
        float targetY = Mathf.Round(player.position.y / roomHeight) * roomHeight;

        // Giữ nguyên trục Z của Camera (thường là -10)
        Vector3 targetPosition = new Vector3(targetX, targetY, transform.position.z);

        // Di chuyển Camera mượt mà từ vị trí hiện tại sang phòng mới
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}