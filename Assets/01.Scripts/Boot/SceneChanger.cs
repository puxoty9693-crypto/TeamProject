using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




public class SceneChanger : MMSingleton<SceneChanger>
{
    [SerializeField] private CanvasGroup fadeCanvasGroup; //화면 전체를 덮는 이미지 캔버스
    [SerializeField] private GameObject loadingScreenRoot; // 로딩바 등을 담는 루트 오브젝트
    [SerializeField] private Slider progressBar;

    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private float minLoadingScreen = 0.5f; // 로딩이 너무 빨라서 사라지는 거 방지

    // 지금 추가로 로드돼 있는 게임 씬 이름
    private string currentSceneName = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
