using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerBaseStats", menuName = "Stats/Player Base Stats Definition")]
public class PlayerBaseStatsSO : ScriptableObject
{
    [Header("Core Base Stats")]
    public float baseMaxHealth = 100f; 
    public float baseAttack = 10f;
    public float baseMagicAttack = 0f;
    public float baseArmor = 5f;
    public float baseMagicArmor = 5f;
    public float baseSpeed = 1f;
    public float baseCriticalDamage = 1.5f;
    public float baseCriticalChance = 0.05f;

    [Header("Default Modifiers (Optional)")]
    public StatModifier[] defaultModifiers;
}