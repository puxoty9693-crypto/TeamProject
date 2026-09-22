using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


// Boot씬에만 존재하는 오브젝트에 붙임
// 전역 매니저들이 전부 초기화 된 걸 확인 후, 첫 게임 씬을 SceneChanger로 로드.
// 여기 나열한 매니저들은 전부 MMS 의 persistAcrossScenes = true 여야 합니다.
public class BootStrapper : MonoBehaviour
{

    [SerializeField] private string firstSceneName = "MainScene";

    private IEnumerator Start()
    {
        // 혹시 안 놔뒀으면 자동 생성 + DonDestroyOnLoad 까지 자동 처리
        // 여기 나열한 순서가 존재를 보장하는 순서 (필요하면 추가/순서 조정)
        _ = DataManager.Instance;
        _ = SaveManager.Instance;
        _ = SoundManager.Instance;
        _ = EventManager.Instance;

        yield return null; // 한 프레임 대기 (씬에 있던 매니저들의 Awake가 확실히 다 끝나도록)
        

        //메인 메뉴에서 새 게임을 눌렀으면, 기존 세이브를 지우고 데이터를 새로 초기화함.
        // 불러오기를 눌렀을 때는 그대로 기존 세이브 유지.
        if (MainMenuController.PendingNewGame) 
        {
            MainMenuController.PendingNewGame = false;
            SaveManager.Instance.StartNewGame();
        }


        SceneChanger.Instance.ChangeScene(firstSceneName);

    }
}

