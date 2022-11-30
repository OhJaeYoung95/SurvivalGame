using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArchemyItem            // 연금 아이템
{
    public string itemName;         // 연금 아이템 이름
    public string itemDesc;         // 연금 아이템 설명
    public Sprite itemImage;        // 연금 아이템 이미지

    public GameObject go_ItemPrefab;    // 연금 아이템 프리팹
}

public class ArchemyTable : MonoBehaviour
{
    private bool isOpen = false;            // 아이템 연금창 활성화 여부(중복 방지)

    [SerializeField]
    private ArchemyItem[] archemyItems;     // 제작할 수 있는 연금 아이템 종류

    [SerializeField]
    private Transform tf_BaseUI;            // 베이스 UI
    [SerializeField]
    private Transform tf_PotionAppearPos;   // 포션 생성 위치

    // Update is called once per frame
    void Update()
    {
        if (isOpen)     // 아이템 연금창 활성화시, 중복방지
            if (Input.GetKeyDown(KeyCode.Escape))       // ESC키를 누른다면
                CloseWindow();      // 창 닫기
    }

    public void Window()        // 아이템 연금창 활성화, 비활성화
    {
        isOpen = !isOpen;       // 버튼식 처럼 true -> false, false -> true
        if (isOpen)             // 아이템 연금창 활성화시, 중복방지
            OpenWindow();
        else
            CloseWindow();
    }

    private void OpenWindow()       // 창 열기 함수
    {
        isOpen = true;          // 아이템 연금창 활성화, 중복방지
        GameManager.isOpenArchemyTable = true;      // 커서 활성화
        tf_BaseUI.localScale = new Vector3(1f, 1f, 1f);     // 크키가 1이 되어서 활성화된 것처럼 만든다
    }

    private void CloseWindow()      // 창 닫기 함수
    {
        isOpen = false;         // 아이템 연금창 비활성화, 중복방지
        GameManager.isOpenArchemyTable = false;     // 커서 비활성화
        tf_BaseUI.localScale = new Vector3(0f, 0f, 0f);     // 크키가 0이 되어서 비활성화된 것처럼 만든다
    }

    public void ButtonClick(int _buttonNum)         // 연금창 버튼 클릭시 발생하는 함수
    {
        ProductionComplete(_buttonNum);         // 연금 아이템 생산완료
    }

    private void ProductionComplete(int _buttonNum)     // 연금 아이템 생산완료 함수
    {
        // 연금 테이블 생성 위치에 해당 연금 아이템 생성
        Instantiate(archemyItems[_buttonNum].go_ItemPrefab, tf_PotionAppearPos.position, Quaternion.identity);
    }
}
