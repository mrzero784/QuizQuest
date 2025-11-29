using UnityEngine;
using System.Collections.Generic;
using System;

// PlayerStats.cs (ควรแนบกับ GameObject ใน Scene ที่มี DontDestroyOnLoad)

public class PlayerStats : MonoBehaviour
{
    // --- 1. Singleton Access ---
    public static PlayerStats Instance { get; private set; }

    [Header("Data References")]
    [SerializeField] private PlayerBaseStatsSO _baseStats; 

    // --- 2. Runtime State Storage ---
    // Dictionary สำหรับเก็บค่ารวมของ Stat ที่ถูกคำนวณแล้ว (RUN-TIME STATE)
    private Dictionary<StatType, float> _currentStats = new Dictionary<StatType, float>();

    // --- Unity Lifecycle ---

    private void Awake()
    {
        // Singleton Initialization and Check
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // ทำให้ Instance นี้คงอยู่ตลอดการเปลี่ยน Scene
        DontDestroyOnLoad(gameObject); 
        
        // โหลดค่า Stats เริ่มต้นเมื่อ Manager ถูกสร้าง
        InitializeStats(); 
    }

    // --- Core Logic ---
    
    // เมธอดเริ่มต้นและรีเซ็ตค่า Stats (ถูกเรียกครั้งเดียวใน Awake)
    private void InitializeStats()
    {
        if (_baseStats == null)
        {
            Debug.LogError("ERROR: Base Stats SO is not assigned to PlayerStats!");
            return;
        }
        
        // 1. ล้างค่าเดิมทั้งหมด
        _currentStats.Clear();

        // 2. กำหนดค่าเริ่มต้นจาก Base Stats SO เข้าสู่ Dictionary
        _currentStats[StatType.MaxHealth] = _baseStats.baseMaxHealth;
        _currentStats[StatType.CurrentHealth] = _baseStats.baseMaxHealth; // เริ่มต้นด้วย HP เต็ม
        _currentStats[StatType.Attack] = _baseStats.baseAttack;
        // *** TODO: เพิ่มการกำหนด Base Stat อื่นๆ ที่เหลือจาก SO ที่นี่ ***

        // ณ จุดนี้ Current Stats คือ Base Stats (ก่อน Item/Buff)
        Debug.Log("Player Stats Initialized. Max Health: " + GetCurrentStat(StatType.MaxHealth));
    }

    // --- Public Methods for Other Systems ---

    // 1. Reading Data: เมธอดสำหรับระบบอื่น (เช่น ระบบโจมตี, UI) เรียกใช้เพื่อดึงค่า Stat
    public float GetCurrentStat(StatType type)
    {
        if (_currentStats.TryGetValue(type, out float value))
        {
            return value;
        }
        Debug.LogWarning($"StatType {type} not found in current stats dictionary. Returning 0.");
        return 0f; 
    }
    
    // 2. State Manipulation: เมธอดสำหรับระบบต่อสู้/Question Manager เรียกใช้เมื่อถูกโจมตี
    public void ApplyDamage(float rawDamage)
    {
        // 1. ดึงค่า Armor ปัจจุบันมาใช้ในการลดความเสียหาย
        float armor = GetCurrentStat(StatType.Armor); // ต้องมั่นใจว่ามีการกำหนดค่า Armor เริ่มต้นแล้ว
        float finalDamage = Mathf.Max(0, rawDamage - armor); 

        // 2. อัปเดตค่า HP
        float newHealth = GetCurrentStat(StatType.CurrentHealth) - finalDamage;
        _currentStats[StatType.CurrentHealth] = Mathf.Max(0, newHealth); 

        Debug.Log($"Player took {finalDamage} final damage. Current HP: {_currentStats[StatType.CurrentHealth]}");
        
        if (_currentStats[StatType.CurrentHealth] <= 0)
        {
            Debug.Log("Player Defeated! Game Over.");
            // TODO: เรียกเมธอด Game Over/Defeat State
        }
    }
    
    // 3. Calculation Logic: (จะใช้ใน Phase 3)
    // เมธอดนี้จะถูกเรียกโดย Equipment Manager เมื่อ Player สวมใส่/ถอด Item
    public void RecalculateStats(/* TODO: รับ List ของ StatModifier จาก Item สวมใส่ */)
    {
        // **TODO: เขียน Logic ใน Phase 3**
        // 1. เรียก InitializeStats() เพื่อโหลด Base Stats ใหม่
        // 2. วนซ้ำ Stat Modifier ทั้งหมดจาก Item และบวกค่าเข้าสู่ _currentStats
    }
}