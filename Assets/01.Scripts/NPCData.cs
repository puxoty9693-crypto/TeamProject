using UnityEngine;

//public enum NPCRole { Chef, Server, Cashier } // 요리사, 서빙원, 계산원

// NPC 한 명의 기본 정보
[CreateAssetMenu(fileName = "NPC_", menuName = "Data/NPC")]
public class NPCData : ScriptableObject
{

    [Header("Identity")]
    [SerializeField] private string npcId;      // NPC 넘버링
    [SerializeField] private string npcName;    // NPC 이름
    [SerializeField] private Sprite npcImage;   // NPC 이미지

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;

    // 해당 property들은 get만
    public string NpcId => npcId;
    public string NpcName => npcName;
    public Sprite NpcImage => npcImage;
    public float MoveSpeed => moveSpeed;


}
