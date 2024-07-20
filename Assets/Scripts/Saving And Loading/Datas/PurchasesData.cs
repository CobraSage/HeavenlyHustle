using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PurchasesData
{
    public List<bool> BuildingsUnlocked = new List<bool> { true, false, false, false, false };
    public List<int> BuildingsLevel = new List<int> { 1, 1, 1, 1, 1 };
    public List<int> BuildingsCapacity = new List<int> { 1, 1, 1, 1, 1 };
    public List<int> BuildingsTimeLevel = new List<int> { 1, 1, 1, 1, 1 };
    public int HeavenTotalCapacityLevel = 1;
    public int SideGameCooldownReductionLevel = 1;
    public int SoulsMultiplierLevel = 1;
    public int HappinessPointsMultiplierLevel = 1;
}
