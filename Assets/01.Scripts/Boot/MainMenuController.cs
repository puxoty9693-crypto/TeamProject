using UnityEngine;
using UnityEngine.SceneManagement;


// 메인 메뉴 씬에만 존재
public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string bootSceneName = "Boot";

    public static bool PendingNewGame;

    // 새 게임 버튼에 연결 - 기존 세이브 지우기
    public void OnClickStatGame() 
    {
        PendingNewGame = true;
        SceneManager.LoadScene(bootSceneName);

    }

    // 불러오기 버튼에 연결
    public void OnClickLoadGame() 
    {
        PendingNewGame = false; 
        SceneManager.LoadScene(bootSceneName);
    }

    // 게임 종료 버튼에 연결
    public void OnClickQuit() 
    {
        Application.Quit();
    }
    
}
