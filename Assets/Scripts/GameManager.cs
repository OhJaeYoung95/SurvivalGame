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

    void Update()
    {
        // 인벤토리 활성화시, 컴퓨터 전원 On시, 건축 메뉴창 활성화시, 연금 테이블 창 활성화시
        if (isOpenInventory || isOnComputer || isOpenCraftManual || isOpenArchemyTable)
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
    }

    void Start()
    {
        // 마우스 커서 잠그기, Cursor.visible = false 도 포함
        Cursor.lockState = CursorLockMode.Locked;
        // 마우스 커서 비활성화(사라짐)
        Cursor.visible = false;
    }

}
