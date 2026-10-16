using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Cấu hình hầm ngục")]
    public int minRooms = 7;
    public int maxRooms = 10;

    // HashSet lưu tọa độ các phòng bằng Vector2 để tự động chặn trùng lặp
    private HashSet<Vector2> roomPositions = new HashSet<Vector2>();
    private List<Vector2> monsterRooms = new List<Vector2>(); // Thêm dòng này
    void Start()
    {
        GenerateRoomCoordinates();
        AssignRoomTypes();
        SpawnRoomsInWorld();

    }

    void GenerateRoomCoordinates()
    {
        int targetRoomCount = Random.Range(minRooms, maxRooms + 1);
        roomPositions.Add(Vector2.zero); // Luôn bắt đầu ở (0,0)

        while (roomPositions.Count < targetRoomCount)
        {
            // 1. Lấy ngẫu nhiên một phòng đã có trong danh sách
            Vector2 randomExistingRoom = roomPositions.ElementAt(Random.Range(0, roomPositions.Count));

            // 2. Lấy một hướng lân cận ngẫu nhiên
            Vector2 newRoomPos = randomExistingRoom + GetRandomDirection();

            // 3. Nếu tọa độ này chưa có phòng, thêm phòng mới vào
            if (!roomPositions.Contains(newRoomPos))
            {
                roomPositions.Add(newRoomPos);
            }
        }
    }

    Vector2 GetRandomDirection()
    {
        int rand = Random.Range(0, 4);
        if (rand == 0) return Vector2.up;    // (0, 1)
        if (rand == 1) return Vector2.down;  // (0, -1)
        if (rand == 2) return Vector2.left;  // (-1, 0)
        return Vector2.right;                // (1, 0)
    }

    void AssignRoomTypes()
    {
        List<Vector2> availableRooms = roomPositions.ToList();

        // 1. Xác định phòng Bắt đầu
        Vector2 startRoom = Vector2.zero;
        availableRooms.Remove(startRoom);

        // 2. Xác định phòng Boss (Phòng cách xa StartRoom nhất)
        Vector2 bossRoom = availableRooms.OrderByDescending(r => Vector2.Distance(startRoom, r)).First();
        availableRooms.Remove(bossRoom);
        Debug.Log("Boss đặt tại: " + bossRoom);

        // 3. Xác định Cửa hàng (1 phòng)
        Vector2 shopRoom = availableRooms[Random.Range(0, availableRooms.Count)];
        availableRooms.Remove(shopRoom);
        Debug.Log("Cửa hàng đặt tại: " + shopRoom);

        // 4. Xác định phòng Thưởng (1-2 phòng)
        int rewardCount = Random.Range(1, 3);
        for (int i = 0; i < rewardCount; i++)
        {
            if (availableRooms.Count == 0) break;
            Vector2 rewardRoom = availableRooms[Random.Range(0, availableRooms.Count)];
            availableRooms.Remove(rewardRoom);
            Debug.Log("Phòng thưởng đặt tại: " + rewardRoom);
        }

        // 5. Các phòng còn lại mặc định là Phòng Quái và Thử thách
        foreach (Vector2 monsterRoom in availableRooms)
        {
            monsterRooms.Add(monsterRoom);
        }
    }

    public GameObject roomPrefab; // Kéo Prefab phòng chuẩn vào đây trong Inspector
    public float roomWidth = 20f;
    public float roomHeight = 12f;

    void SpawnRoomsInWorld()
    {
        foreach (Vector2 pos in roomPositions)
        {
            // Nhân tọa độ lưới với kích thước thực tế của phòng
            Vector2 actualWorldPos = new Vector2(pos.x * roomWidth, pos.y * roomHeight);

            // Sinh phòng ra
            GameObject spawnedRoom = Instantiate(roomPrefab, actualWorldPos, Quaternion.identity);

            // Lấy script Room trên phòng vừa sinh
            Room roomScript = spawnedRoom.GetComponent<Room>();
            if (roomScript != null)
            {
                // Kiểm tra xem HashSet có chứa tọa độ của các phòng liền kề không
                bool hasUp = roomPositions.Contains(pos + Vector2.up);
                bool hasDown = roomPositions.Contains(pos + Vector2.down);
                bool hasLeft = roomPositions.Contains(pos + Vector2.left);
                bool hasRight = roomPositions.Contains(pos + Vector2.right);

                // Truyền thông tin cho phòng để nó tự động đập tường hoặc giữ lại tường
                roomScript.UpdateDoors(hasUp, hasDown, hasLeft, hasRight);

                // KIỂM TRA: Nếu tọa độ phòng này nằm trong danh sách phòng quái -> Sinh quái
                if (monsterRooms.Contains(pos))
                {
                    roomScript.SpawnEnemies();
                }
            }
        }
    }
}