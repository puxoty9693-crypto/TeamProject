using System;
using UnityEngine;
using UnityEngine.Events;

public enum TutorialNodeType
{
    Dialogue,
    WaitForEvent
}

[Serializable]
public class Tutorialnode
{
    public TutorialNodeType type;

    [Header("Dialogue 타입일 때만 사용")]
    [TextArea] public string dialogueText;

    [Header("WaitForEvent 타입일 때만 사용")]
    public EventType waitEventType;
    public string highlightKey;

    [Header("이 단계 진입 시 실행 (팝업 열기 등)")]
    public UnityEvent onEnterActions;

    [Header("이 단계 벗어날 때 실행 (팝업 닫기 등)")]
    public UnityEvent onExitActions;
}
