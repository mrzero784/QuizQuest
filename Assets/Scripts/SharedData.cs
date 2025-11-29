using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum StatType 
{
    MaxHealth,
    CurrentHealth,
    Attack,
    MagicAttack,
    Armor,
    MagicArmor,
    Speed,
    CriticalDamage,
    CriticalChance
}

[Serializable]
public struct StatModifier
{
    public StatType type;
    public float value;
}
