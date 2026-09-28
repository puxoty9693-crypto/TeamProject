// 특정 NPC 역할의 현재 업그레이드 레벨
[System.Serializable]
public class NPCUpgradeSave
{
    public NPCRole role; // 업그레이드 대상 역할
    public int level;    // 현재 레벨
}
