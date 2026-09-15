using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CustomerTrait_", menuName = "Data/NPC/Customer Trait")]

public class CustomerTraitData : MonoBehaviour
{
    [Header("Trait")]
    [SerializeField] private CustomerTrait trait = CustomerTrait.Normal;

    [SerializeField] private CustomerTraitGroup traitGroup = CustomerTraitGroup.None;

    [SerializeField] private bool canExclusiveInGroup; // 그룹 내 중복 가능 여부

    [Header("Conflict")]
    [SerializeField] private List<CustomerTraitData> incompatibleTraits = new();

    [Header("Multiplier")] // trait별 배율
    [SerializeField] private float patienceMultiplier = 1f;
    [SerializeField] private float eatingSpeedMultiplier = 1f;
    [SerializeField] private float tipMultiplier = 1f;

    public CustomerTrait Trait => trait;
    public CustomerTraitGroup TraitGroup => traitGroup;
    public bool CanExclusiveInGroup => canExclusiveInGroup;
    public float PatienceMultiplier => patienceMultiplier;

    public float EatingSpeedMultiplier => eatingSpeedMultiplier;
    public float TipMultiplier => tipMultiplier;


    /// <summary>
    /// 
    /// 기존 trait에 추가하려는 other의 trait과 비교하여 충돌 발생 시 적용x
    /// 
    /// true == 충돌,  false != 충돌
    /// 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool ConflictsCompare(CustomerTraitData other)
    {

        if (other == null) return false;
        if (other == this) return true;

        // Same Group, but any other one has unique Trait will be CONFLICTED
        // 동일 그룹 내에 존재하나 하나라도 독립 Trait이면 충돌 발생
        if (traitGroup != CustomerTraitGroup.None && traitGroup == other.traitGroup && (canExclusiveInGroup || other.canExclusiveInGroup)) return true;

        // 직접 설정된 충돌 관계일 시 충돌 발생
        if (incompatibleTraits.Contains(other)) return true;

        // other의 trait이 해당 trait과 동일할 시 충돌 발생
        if (other.incompatibleTraits.Contains(this)) return true;

        // 검증절차를 끝내고 triat 적용 가능 판정
        return false;
    }

}
