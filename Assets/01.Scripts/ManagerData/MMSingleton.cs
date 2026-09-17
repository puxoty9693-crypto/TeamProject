using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 모든 매니저가 상속받는 싱글톤 베이스 클래스
// 씬에 인스턴스가 없으면 자동 생성, 씬 전환 시에도 파괴되지 않도록 처리
public class MMSingleton<T> : MonoBehaviour where T : Component
{
    protected static T _instance; // 실제 싱글톤 인스턴스

    public static bool HasInstance => _instance != null; // 인스턴스 존재 여부
    public static T TryGetInstance() => HasInstance ? _instance : null; // 자동 생성 없이 안전하게 가져오기
    public static T Current => _instance; // 현재 인스턴스 (없으면 null)

    // 싱글톤 인스턴스 접근 프로퍼티. 없으면 찾아보고, 그래도 없으면 자동 생성
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<T>(); // 씬에서 기존 인스턴스 탐색
                Create(true);                          // 없으면 자동 생성 (DontDestroyOnLoad 적용)
            }
            return _instance;
        }
    }

    // 인스턴스가 없으면 새로 생성 (씬 전환 시 유지 안 함)
    public static void Create()
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject(typeof(T).Name);
            obj.name = typeof(T).Name + "_AutoCreated";
            _instance = obj.AddComponent<T>();
        }
    }

    // 인스턴스가 없으면 새로 생성 (dontDestroy = true면 씬 전환 시에도 유지)
    public static void Create(bool dontDestroy)
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject(typeof(T).Name);
            obj.name = typeof(T).Name + "_AutoCreated";
            _instance = obj.AddComponent<T>();
            if (dontDestroy) DontDestroyOnLoad(obj);
        }
    }

    // Awake 시점에 싱글톤 초기화. 자식 클래스에서 재정의 시 base.Awake() 먼저 호출할 것
    protected virtual void Awake()
    {
        InitializeSingleton();
    }

    // 실제 인스턴스 할당. 에디터에서 플레이 중이 아니면 동작 안 함
    protected virtual void InitializeSingleton()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        _instance = this as T;
    }
}