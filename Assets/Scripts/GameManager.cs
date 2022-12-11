using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true;        // 플레이어 움직임 제어 활성화 여부

    public static bool isOpenInventory = false;     // 인벤토리 활성화 여부
    public static bool isOpenCraftManual = false;   // 건축 메뉴창 활성화 여부
    public static bool isOpenArchemyTable = false;  // 연금 테이블 창 활성화 여부
    public static bool isOnComputer = false;     // 컴퓨터 창 활성화 여부

    public static bool isNight = false;         // 밤인지 낮인지에 대한 여부
    public static bool isWater = false;         // 물속인지에 대한 여부

    public static bool isPause = false;         // 퍼즈메뉴 활성화 여부

    private WeaponManager theWM;
    private bool flag = false;      // 물속에서 장비를 집어넣는지에 대한 여부

    void Update()
    {
        // 인벤토리 활성화시, 컴퓨터 전원 On시, 건축 메뉴창 활성화시, 연금 테이블 창 활성화시, 퍼즈메뉴 활성화시
        if (isOpenInventory || isOnComputer || isOpenCraftManual || isOpenArchemyTable || isPause)
        {
            // 마우스 커서 원래대로, Cursor.visible = true 도 포함
            Cursor.lockState = CursorLockMode.None;
            // 마우스 커서 비활성화(사라짐)
            Cursor.visible = true;
            canPlayerMove = false;  // 움직임 제어 비활성화
        }
        else                        // 인벤토리 비활성화시
        {
            // 마우스 커서 잠그기, Cursor.visible = false 도 포함
            Cursor.lockState = CursorLockMode.Locked;
            // 마우스 커서 비활성화(사라짐)
            Cursor.visible = false;
            canPlayerMove = true;   // 움직임 제어 활성화
        }

        if (isWater)        // 물속일때
        {
            if (!flag)      // 무기를 집어넣지 않았다면
            {
                StopAllCoroutines();
                StartCoroutine(theWM.WeaponInCoroutine());      // 무기 집어넣기
                flag = true;        // 무기 집어넣음
            }
        }
        else                // 물 속이 아닐때
        {
            if (flag)       // 무기를 집어넣었다면
            {
                flag = false;   // 무기 집어넣지 않음
                theWM.WeaponOut();          // 무기 꺼내기
            }
        }
        
    }

    void Start()
    {
        // 마우스 커서 잠그기, Cursor.visible = false 도 포함
        Cursor.lockState = CursorLockMode.Locked;
        // 마우스 커서 비활성화(사라짐)
        Cursor.visible = false;
        theWM = FindObjectOfType<WeaponManager>();
    }

}
