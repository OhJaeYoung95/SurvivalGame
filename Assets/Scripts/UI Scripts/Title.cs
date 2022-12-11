using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "GameStage";        // 다음씬으로 이동할 이름

    private SaveNLoad theSaveNLoad;     // 저장 불러오기를 위한 컴포넌트

    public static Title instance;       // 싱글턴화


    private void Awake()        // 싱글턴화
    {
        if (instance == null)       // 객체가 없다면
        {
            instance = this;        // 객체생성
            DontDestroyOnLoad(gameObject);  // 씬이동 간에 객체 파괴되지 않게 설정
        }
        else            // 객체가 있다면
            Destroy(this.gameObject);       // 객체제거
    }

    public void ClickStart()        // 스타트 버튼 클릭 함수
    {
        Debug.Log("로딩");
        SceneManager.LoadScene(sceneName);      // 씬이동
    }

    public void ClickLoad()     // 로드 버튼 클릭 함수
    {
        Debug.Log("로드");

        StartCoroutine(LoadCoroutine());

    }

    IEnumerator LoadCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while(!operation.isDone)        // 씬을 다 불러올때까지 대기
        {
            yield return null;
        }

        theSaveNLoad = FindObjectOfType<SaveNLoad>();       // 다음씬에서 객체참조후
        theSaveNLoad.LoadData();                // 저장
        gameObject.SetActive(false);       // 타이틀 UI 비활성화
    }

    public void ClickExit()     // 게임종료 버튼 클릭 함수
    {
        Debug.Log("게임 종료");
        Application.Quit();     // 게임종료
    }
}
