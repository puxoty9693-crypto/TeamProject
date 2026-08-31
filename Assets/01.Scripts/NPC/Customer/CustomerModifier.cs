using System;

/// <summary>
/// struct 대신 class 사용. 현재는 기능 구현만 되어있음.
/// </summary>
[Serializable]
public class CustomerModifier
{
    public float PatienceMultiplier { get; private set; } = 1f;
    public float EatingSpeedMultiplier { get; private set; } = 1f;
    public float TipMultiplier { get; private set; } = 1f;

    public void Reset()
    {
        // 현재는 trait이 존재하지 않으므로 하드코딩
        PatienceMultiplier = 1f;
        EatingSpeedMultiplier = 1f;
        TipMultiplier = 1f;
    }
    public void Apply(CustomerTraitData traitData)
    {
        if (traitData == null) return;  // CustomerTraitData 스크립트 존재하지 않을 시 return. 방어용

        PatienceMultiplier *= traitData.PatienceMultiplier;
        EatingSpeedMultiplier *= traitData.EatingSpeedMultiplier;
        TipMultiplier *= traitData.TipMultiplier;
    }

    
}
