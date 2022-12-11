using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject go_BaseUI;       // 퍼즈메뉴창 UI
    [SerializeField]
    private SaveNLoad theSaveNLoad;     // 데이터 세이브 로드를 위한 컴포넌트

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))     // P 키를 누른다면
        {
            if (!GameManager.isPause)        // 퍼즈메뉴창이 비활성화일떄
                CallMenu();         // 메뉴창 활성화
            else
                CloseMenu();        // 메뉴창 비활성화
        }
    }

    private void CallMenu()     // 메뉴창 활성화 함수
    {
        GameManager.isPause = true;     // 퍼즈메뉴창 여부 활성화
        go_BaseUI.SetActive(true);      // 퍼즈메뉴창 UI 활성화
        Time.timeScale = 0f;            // 시간의 흐름 멈춤
    }

    private void CloseMenu()    // 메뉴창 비활성화 함수
    {
        GameManager.isPause = false;    // 퍼즈메뉴창 여부 비활성화
        go_BaseUI.SetActive(false);     // 퍼즈메뉴창 UI 비활성화
        Time.timeScale = 1f;            // 시간의 흐름 정상적으로
    }

    public void ClickSave()     // 세이브 버튼 클릭 함수
    {
        Debug.Log("세이브");
        theSaveNLoad.SaveData();        // 데이터 저장
    }

    public void ClickLoad()     // 로드 버튼 클릭 함수
    {
        Debug.Log("로드");
        theSaveNLoad.LoadData();        // 데이터 불러오기
        CloseMenu();            // 메뉴창 비활성화
    }
    public void ClickExit()     // 게임종료 버튼 클릭 함수
    {
        Debug.Log("게임종료");
        Application.Quit();     // 게임종료
    }
}
