using UnityEngine;
using System;
using System.Collections.Generic;


[Serializable]
public class CustomerTraitSet
{
    // 읽기 전용으로 list 생성, get 허용
    private readonly List<CustomerTraitData> traits = new();
    public IReadOnlyList<CustomerTraitData> Traits => traits;

    public bool TryAdd(CustomerTraitData newTrait)
    {
        if (newTrait == null) return false;
        foreach(CustomerTraitData currentTrait in traits)
        {
            if (currentTrait.ConflictsCompare(newTrait)) return false;
        }

        traits.Add(newTrait);
        return true;
    }


    /// <summary>
    /// 
    /// 이하는 List 제어용
    /// 
    /// </summary>
    /// <param name="trait"></param>
    /// <returns></returns>
    public bool Remove(CustomerTraitData trait)
    {
        return traits.Remove(trait);
    }

    public void Clear()
    {
        traits.Clear();
    }

    public void ApplyModifiers(CustomerModifier modifier)
    {
        if (modifier == null) return;   // null 방어

        modifier.Reset();

        foreach(CustomerTraitData trait in traits)
        {
            modifier.Apply(trait);
        }

    }

}
