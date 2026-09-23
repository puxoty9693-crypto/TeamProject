using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("DialoguePresenter 연결")]
    [SerializeField] MonoBehaviour dialoguePresenterBehaviour;

    [Header("ScreenDimHighlighter 연결")]
    [SerializeField] MonoBehaviour highlightPresenterBehaviour;

    [Header("TutorialHighlightMap 연결")]
    [SerializeField] TutorialHighlightMap highlightMap;

    [Header("튜토리얼 캔버스 연결")]
    [SerializeField] Canvas tutorialCanvas;

    [Header("튜토리얼 시퀀스 데이터")]
    [SerializeField] List<Tutorialnode> nodes;

    private IDialoguePresenter dialoguePresenter;
    private IHighlightPresenter highlightPresenter;

    private int currentIndex = -1;
    private HashSet<EventType> subscribedTypes = new();

    private void Awake()
    {
        dialoguePresenter = dialoguePresenterBehaviour as IDialoguePresenter;
        highlightPresenter = highlightPresenterBehaviour as IHighlightPresenter;

        if (dialoguePresenter == null)
            Debug.LogError("dialoguePresenterBehaviour가 IDialoguePresenter를 구현하지 않습니다.");
        if (highlightPresenter == null)
            Debug.LogError("highlightPresenterBehaviour가 IHighlightPresenter를 구현하지 않습니다.");
    }

    private void OnEnable()
    {
        if (SaveManager.Instance.CurrentData.TutorialCompleted)
        {
            tutorialCanvas.gameObject.SetActive(false);
            gameObject.SetActive(false);
            return;
        }

        dialoguePresenter.OnNextRequested += HandleNext;
        SubscribeAllWaitEvents();
        currentIndex = -1;
        Advance();
    }

    private void OnDisable()
    {
        if (dialoguePresenter != null)
            dialoguePresenter.OnNextRequested -= HandleNext;

        UnsubscribeAllWaitEvents();
    }

    private void SubscribeAllWaitEvents()
    {
        foreach (var node in nodes)
        {
            if (node.type != TutorialNodeType.WaitForEvent)
                continue;

            if (subscribedTypes.Contains(node.waitEventType))
                continue;

            EventManager.Instance.AddListener(node.waitEventType, HandleTutorialEvent);
            subscribedTypes.Add(node.waitEventType);
        }
    }

    private void UnsubscribeAllWaitEvents()
    {
        if (!EventManager.HasInstance)
            return;

        foreach (var type in subscribedTypes)
            EventManager.Instance.RemoveListener(type, HandleTutorialEvent);

        subscribedTypes.Clear();
    }

    private void HandleTutorialEvent(Component sender, object param)
    {
        if (currentIndex < 0 || currentIndex >= nodes.Count) 
            return;

        var current = nodes[currentIndex];

        if (current.type != TutorialNodeType.WaitForEvent)
            return;

        Advance();
    }

    private void HandleNext()
    {
        if (currentIndex < 0 || currentIndex >= nodes.Count)
            return;

        var current = nodes[currentIndex];
        if (current.type != TutorialNodeType.Dialogue)
            return;

        Advance();
    }

    private void Advance()
    {

        if (currentIndex >= 0 && currentIndex < nodes.Count)
            nodes[currentIndex].onExitActions?.Invoke();
        
        currentIndex++;

        if (currentIndex >= nodes.Count)
        {
            Complete();
            return;
        }

        Render(nodes[currentIndex]);
    }

    private void Render(Tutorialnode node)
    {
        node.onEnterActions?.Invoke();

        if (node.type == TutorialNodeType.Dialogue)
        {
            dialoguePresenter.Show(node.dialogueText);
            highlightPresenter.ShowFullDim();
        }
        else
        {
            dialoguePresenter.Hide();

            if (highlightMap.TryGetTarget(node.highlightKey, out var target, out var padding))
                highlightPresenter.ShowHole(target, padding);
            else
                highlightPresenter.ShowFullDim();
        }
    }

    private void Complete()
    {
        dialoguePresenter.Hide();
        highlightPresenter.Hide();

        SaveManager.Instance.CurrentData.CompleteTutorial();
        SaveManager.Instance.SaveGame();

        gameObject.SetActive(false);
    }
}