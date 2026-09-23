using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("DialoguePresenter 연결")]
    [SerializeField] MonoBehaviour dialoguePresenterBehaviour;

    [Header("튜토리얼 캔버스 연결")]
    [SerializeField] Canvas tutorialCanvas;

    [Header("튜토리얼 시퀀스 데이터")]
    [SerializeField] List<Tutorialnode> nodes;

    private IDialoguePresenter dialoguePresenter;

    private int currentIndex = -1;
    private HashSet<EventType> subscribedTypes = new();

    private void Awake()
    {
        dialoguePresenter = dialoguePresenterBehaviour as IDialoguePresenter;
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
        }
        else
        {
            dialoguePresenter.Hide();
        }
    }

    private void Complete()
    {
        dialoguePresenter.Hide();

        SaveManager.Instance.CurrentData.CompleteTutorial();
        SaveManager.Instance.SaveGame();

        tutorialCanvas.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}