using UnityEngine;

public enum WorkerRole { Chef, Server, Cashier } // 요리사, 서빙원, 계산원

// NPC 한 명의 기본 정보
[CreateAssetMenu(fileName = "NPC_", menuName = "Data/NPC")]
public class WokerData : ScriptableObject
{
    public WorkerRole role;      // NPC 역할
    public string workerName;    // NPC 이름
    public Sprite workerImage;   // NPC 이미지
}
