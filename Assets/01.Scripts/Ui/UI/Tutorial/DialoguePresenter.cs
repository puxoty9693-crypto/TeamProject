using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IDialoguePresenter
{
    event Action OnNextRequested;
    void Show(string text);
    void Hide();
}


public class DialoguePresenter : MonoBehaviour, IDialoguePresenter
{
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject npcImagePanel;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] Button nextButton;

    public event Action OnNextRequested;

    private void Awake()
    {
        nextButton.onClick.AddListener(() => OnNextRequested?.Invoke());
    }
    public void Hide()
    {
        dialoguePanel.SetActive(false);
        npcImagePanel.SetActive(false);
    }

    public void Show(string text)
    {
        dialogueText.text = text;
        dialoguePanel.SetActive(true);
        npcImagePanel.SetActive(true);
    }

}
