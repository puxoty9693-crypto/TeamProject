using UnityEngine;

public enum NPCRole { Chef, Server, Cashier } // 요리사, 서빙원, 계산원

// NPC 한 명의 기본 정보
[CreateAssetMenu(fileName = "NPC_", menuName = "Data/NPC")]
public class NPCData : ScriptableObject
{
    public NPCRole role;      // NPC 역할
    public string npcName;    // NPC 이름
    public Sprite npcImage;   // NPC 이미지
}
