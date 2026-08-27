using System.Collections.Generic;
using UnityEngine;

public enum DialogueOwner { Server, Chef, Cashier, Waiter } // 서버, 요리사, 계산원, 서빙원

// 특정 역할이 사용하는 대사 목록
[CreateAssetMenu(fileName = "Dialogue_", menuName = "Data/Dialogue")]
public class DialogueData : ScriptableObject
{
    public DialogueOwner owner;   // 대사 소유 주체
    public List<string> lines;    // 랜덤 출력용 대사 목록
}