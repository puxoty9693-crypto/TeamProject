using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;




public class SceneChanger : MMSingleton<SceneChanger>
{
    [SerializeField] private CanvasGroup fadeCanvasGroup; //화면 전체를 덮는 이미지 캔버스
    [SerializeField] private GameObject loadingScreenRoot; // 로딩바 등을 담는 루트 오브젝트, 없어도 동작은 함
    [SerializeField] private Slider progressBar;           // 로딩 진행 률 표시 슬라이더, 없어도 동작은 함

    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private float minLoadingScreen = 0.5f; // 로딩이 너무 빨라서 사라지는 거 방지


    // 지금 Additive로 얹혀있는 게임 씬 이름
    // 지금 추가로 로드돼 있는 게임 씬 이름
    private string currentSceneName = "";

    public bool IsChangingScene {  get; private set; }

    // 씬이 완전히 바뀌고 활성화 됐을 직후에 발동됨.
    // 씬 전용 매니저는 이 값 필요하면 구독하고 재 초기화
    public event Action<string> OnSceneChanged;


    // 외부(BootStrapper나 버튼 등) 에서는 이 함수 하나만 호출하면 됨.
    public void ChangeScene(string sceneName) 
    {
        if (IsChangingScene) 
        {
            GameLogOnlyEditor.Log("[SceneChanger] 씬 전환 중... 중복 호출 무시");
            return;
        }

        StartCoroutine(ChangeSceneRoutine(sceneName));

    }

    // 씬 전환 전체 흐름을 순서대로 담당하는 코루틴
    private IEnumerator ChangeSceneRoutine(string sceneName) 
    {
        IsChangingScene = true;

        yield return FadeCanvas(0f, 1f);  // 화면 검게 덮기

        if (loadingScreenRoot != null)
            loadingScreenRoot.SetActive(true);


        // 기존 게임 씬이 있으면 먼저 떼어냄
        if (!string.IsNullOrEmpty(currentSceneName)) 
        {
            var unload = SceneManager.UnloadSceneAsync(currentSceneName);
            while (unload != null && !unload.isDone)
                yield return null;
        }

        float startTime = Time.unscaledTime;  //최소 로딩 시간 계산용

        // 새 씬을 비동기로 로드, Boot 씬은 유지되고 덧붙여짐.
        var load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        load.allowSceneActivation = false;


        while (load.progress < 0.9f) 
        {
            if (progressBar != null)
                progressBar.value = load.progress / 0.9f;

            yield return null;  

        }

        if (progressBar != null)
            progressBar.value = 1f;

        // 최소 로딩화면 유지시간 보장
        float elapsed = Time.unscaledTime - startTime;
        if (elapsed < minLoadingScreen)
            yield return new WaitForSecondsRealtime(minLoadingScreen - elapsed);

        load.allowSceneActivation =true; //씬 활성화
        yield return load;  // 씬 활성화 완전히 끝날 때까지 대기

        var loadedScene = SceneManager.GetSceneByName(sceneName); //방금 로드된 씬을 활성화 된 씬으로 지정
        SceneManager.SetActiveScene(loadedScene); // Lighting 이나 Skybox 기준 씬으로 지정

        currentSceneName = sceneName;

        if (loadingScreenRoot != null)
            loadingScreenRoot.SetActive(false);

        yield return FadeCanvas(1f, 0f); // 화면 다시 밝게

        IsChangingScene = false;

        OnSceneChanged?.Invoke(sceneName);


    }

    //  화면 전체를 덮는 이미지의 투명도를  서서히 바꿈
    private IEnumerator FadeCanvas(float from, float to) 
    {
        if (fadeCanvasGroup == null)
            yield break;

        fadeCanvasGroup.gameObject.SetActive(true);
        fadeCanvasGroup.alpha = from;

        float t = 0f;
        while (t < fadeDuration) 
        {
            t += Time.unscaledDeltaTime; // 일시 정지중에도 Fade는 동작하게
            fadeCanvasGroup.alpha = Mathf.Lerp(from, to, t/ fadeDuration);
            yield return null;


        }

        fadeCanvasGroup.alpha = to;

        if (Mathf.Approximately(to, 0f))
            fadeCanvasGroup.gameObject.SetActive(false);
    }

}
