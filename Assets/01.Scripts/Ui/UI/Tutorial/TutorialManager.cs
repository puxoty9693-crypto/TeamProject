using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TutorialStep
{
    None,
    Dialogue_Intro,
    Wait_IngredientUnlock,
    Dialogue_AfterIngredient,
    Wait_RecipeUnlock,
    Dialogue_AfterRecipe,
    Wait_CookingStarted,
    Dialogue_AfterCooking,
    Wait_TableInstalled,
    Completed
}


public class TutorialManager : MonoBehaviour
{
    [Header("대사 UI")]
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject npcImagePanel;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] Button nextButton;

    [Header("단계별 대사")]
    [SerializeField] string introLine = "고향에서 무료로 토마토를 주기로 했어. 업그레이드에서 재료를 해금해보자.";
    [SerializeField] string afterIngredientLine = "토마토를 해금했으니, 이번엔 만들 요리를 해금해보자.";
    [SerializeField] string afterRecipeLine = "이제 요리를 해보자.";
    [SerializeField] string afterCookingLine = "손님을 받으려면 테이블이 있어야지. 테이블을 설치해보자.";


    public TutorialStep CurrentStep { get; private set; } = TutorialStep.None;

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventType.OnIngredientUnlocked, OnIngredientUnlocked);
        EventManager.Instance.AddListener(EventType.OnRecipeUnlocked, OnRecipeUnlocked);
        EventManager.Instance.AddListener(EventType.OnCookingStarted, OnCookingStarted);
        EventManager.Instance.AddListener(EventType.OnTableInstalled, OnTableInstalled);

        nextButton.onClick.AddListener(OnNextClicked);

        StartTutorial();

    }

    private void OnDisable()
    {
        if (!EventManager.HasInstance)
            return;
        EventManager.Instance.RemoveListener(EventType.OnIngredientUnlocked, OnIngredientUnlocked);
        EventManager.Instance.RemoveListener(EventType.OnRecipeUnlocked, OnRecipeUnlocked);
        EventManager.Instance.RemoveListener(EventType.OnCookingStarted, OnCookingStarted);
        EventManager.Instance.RemoveListener(EventType.OnTableInstalled, OnTableInstalled);

    }

    private void StartTutorial()
    {
        SetStep(TutorialStep.Dialogue_Intro);
    }

    private void OnIngredientUnlocked(Component sender, object param)
    {
        if (CurrentStep != TutorialStep.Wait_IngredientUnlock)
            return;

        SetStep(TutorialStep.Dialogue_AfterIngredient);
    }

    private void OnRecipeUnlocked(Component sender, object param)
    {
        if (CurrentStep != TutorialStep.Wait_RecipeUnlock)
            return;

        SetStep(TutorialStep.Dialogue_AfterRecipe);
    }

    private void OnCookingStarted(Component sender, object param)
    {
        if (CurrentStep != TutorialStep.Wait_CookingStarted)
            return;

        SetStep(TutorialStep.Dialogue_AfterCooking);
    }

    private void OnTableInstalled(Component sender, object param)
    {
        if (CurrentStep != TutorialStep.Wait_TableInstalled)
            return;

        SetStep(TutorialStep.Completed);
    }

    private void OnNextClicked()
    {
        switch (CurrentStep)
        {
            case TutorialStep.Dialogue_Intro:
                SetStep(TutorialStep.Wait_IngredientUnlock);
                break;
            case TutorialStep.Dialogue_AfterIngredient:
                SetStep(TutorialStep.Wait_RecipeUnlock);
                break;
            case TutorialStep.Dialogue_AfterRecipe:
                SetStep(TutorialStep.Wait_CookingStarted);
                break;
            case TutorialStep.Dialogue_AfterCooking:
                SetStep(TutorialStep.Wait_TableInstalled);
                break;
        }
    }

    private void SetStep(TutorialStep step)
    {
        CurrentStep = step;

        bool isDialogue = step == TutorialStep.Dialogue_Intro || step == TutorialStep.Dialogue_AfterIngredient || step == TutorialStep.Dialogue_AfterRecipe || step == TutorialStep.Dialogue_AfterCooking;

        dialoguePanel.SetActive(isDialogue);

        switch (step)
        {
            case TutorialStep.Dialogue_Intro: dialogueText.text = introLine;
                break;
            case TutorialStep.Dialogue_AfterIngredient: dialogueText.text = afterIngredientLine;
                break;
            case TutorialStep.Dialogue_AfterRecipe: dialogueText.text = afterRecipeLine;
                break;
            case TutorialStep.Dialogue_AfterCooking: dialogueText.text = afterCookingLine;
                break;
            case TutorialStep.Completed:
                gameObject.SetActive(false);
                break;
        }
    }
}
