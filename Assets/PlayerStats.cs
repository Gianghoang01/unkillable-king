using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("UI Sliders")]
    public Slider healthSlider;
    public Slider armorSlider;
    public Slider manaSlider;

    [Header("Máu (Health)")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Giáp (Armor)")]
    public float maxArmor = 50f;
    public float currentArmor;
    public float armorRegenRate = 5f;       // Tốc độ hồi (điểm/giây)
    public float armorRegenDelay = 3f;      // Thời gian chờ hồi sau khi bị bắn
    private float armorRegenTimer;

    [Header("Mana")]
    public float maxMana = 100f;
    public float currentMana;

    [Header("Tài nguyên")]
    public int currentGold = 0;
    public int currentXP = 0;

    void Start()
    {
        currentHealth = maxHealth;
        currentArmor = maxArmor;
        currentMana = maxMana;

        UpdateAllUI();
    }

    void Update()
    {
        HandleArmorRegen();
    }

    // Logic hồi giáp tự động theo thời gian
    void HandleArmorRegen()
    {
        if (currentArmor < maxArmor)
        {
            if (armorRegenTimer > 0)
            {
                armorRegenTimer -= Time.deltaTime;
            }
            else
            {
                currentArmor += armorRegenRate * Time.deltaTime;
                currentArmor = Mathf.Min(currentArmor, maxArmor);
                if (armorSlider) armorSlider.value = currentArmor;
            }
        }
    }

    // Hàm nhận sát thương: Trừ giáp trước, hết giáp mới trừ vào máu
    public void TakeDamage(float damage)
    {
        player p = GetComponent<player>();
        if (p != null && p.isInvulnerable)
        {
            return; // Đang lướt thì miễn nhiễm mọi sát thương
        }

        armorRegenTimer = armorRegenDelay; // Reset bộ đếm thời gian hồi giáp

        if (currentArmor > 0)
        {
            if (currentArmor >= damage)
            {
                currentArmor -= damage;
                damage = 0;
            }
            else
            {
                damage -= currentArmor;
                currentArmor = 0;
            }
        }

        if (damage > 0)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
        }

        UpdateAllUI();
    }

    // Hàm tiêu tốn Mana khi bắn súng (trả về true nếu đủ mana)
    public bool UseMana(float amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            if (manaSlider) manaSlider.value = currentMana;
            return true;
        }
        return false;
    }

    // --- CÁC HÀM XỬ LÝ NHẶT VẬT PHẨM TỪ ITEMPICKUP ---

    // Nhận mana từ bình hoặc khi diệt quái (SỬA: Đã gộp và cập nhật UI)
    public void AddMana(float amount)
    {
        currentMana = Mathf.Min(currentMana + amount, maxMana);
        if (manaSlider) manaSlider.value = currentMana;
        Debug.Log("Hồi Mana! Mana hiện tại: " + currentMana);
    }

    // Hồi máu qua bình máu hoặc buff (SỬA: Đã xóa hàm trùng lặp)
    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        if (healthSlider) healthSlider.value = currentHealth;
        Debug.Log("Hồi máu! Máu hiện tại: " + currentHealth);
    }

    // Nhận vàng
    public void AddGold(int amount)
    {
        currentGold += amount;
        Debug.Log("Vàng hiện tại: " + currentGold);
    }

    // Nhận kinh nghiệm
    public void AddXP(int amount)
    {
        currentXP += amount;
        Debug.Log("XP hiện tại: " + currentXP);
    }

    // Cập nhật toàn bộ thanh UI
    void UpdateAllUI()
    {
        if (healthSlider) { healthSlider.maxValue = maxHealth; healthSlider.value = currentHealth; }
        if (armorSlider) { armorSlider.maxValue = maxArmor; armorSlider.value = currentArmor; }
        if (manaSlider) { manaSlider.maxValue = maxMana; manaSlider.value = currentMana; }
    }

    void Die()
    {
        Debug.Log("Player đã chết! Hiện popup tổng kết hoặc hồi sinh.");
        // Xử lý logic chết ở đây
    }
}