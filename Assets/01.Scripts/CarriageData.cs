using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarriageLevel 
{
    public float gartherTime;
    public int ingredientAmount;
    public int upgradeGoldCost;
}

[CreateAssetMenu(fileName = "Carrige_", menuName = "Data/Carrige")]

public class CarriageData : ScriptableObject
{
    public List<CarriageLevel> levels;
}
