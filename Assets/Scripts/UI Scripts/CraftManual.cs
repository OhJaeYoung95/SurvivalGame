using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]   // 직렬화
public class Craft      // 건축물 클래스
{
    public string craftName;              // 건축물 이름
    public Sprite craftImage;             // 건축물 이미지
    public string craftDesc;              // 건축물 설명
    public string[] craftNeedItem;        // 건축시 필요한 아이템
    public int[] craftNeedItemCount;      // 건축시 필요한 아이템의 개수
    public GameObject go_Prefab;          // 실제 설치될 프리팹
    public GameObject go_PreviewPrefab;   // 미리보기 프리팹
}

public class CraftManual : MonoBehaviour
{
    // 상태변수
    private bool isActiavted = false;           // 건축 메뉴얼 활성화 여부
    private bool isPreviewActivated = false;    // 미리보기 활성화 여부

    [SerializeField]
    private GameObject go_BaseUI;           // 기본 베이스 UI

    private int tabNumber = 0;              // 탭 번호
    private int page = 1;                   // 첫번째 탭 페이지 숫자
    private int selectedSlotNumber;         // 선택한 슬롯 번호
    private Craft[] craft_SelectedTab;      // 선택한 건축 분류 탭


    [SerializeField]
    private Craft[] craft_fire;             // 모닥불용 탭
    [SerializeField]
    private Craft[] craft_build;            // 건축용 탭

    private GameObject go_Preview;          // 미리보기 프리팹을 담을 변수
    private GameObject go_Prefab;           // 실제 생성될 프리팹을 담을 변수

    [SerializeField]
    private Transform tf_Player;            // 플레이어 위치를 받아올 변수

    // Raycast 필요 변수 선언
    private RaycastHit hitInfo;         // 레이저에 맞는 물체 정보를 다루기 위한 변수
    [SerializeField]
    private LayerMask layerMask;        // 레이저에 맞을 레이어 설정
    [SerializeField]
    private float range;                // 레이저 사정거리

    // 필요한 UI Slot 요소
    [SerializeField]
    private GameObject[] go_Slots;          // 슬롯을 담을 변수
    [SerializeField]
    private Image[] image_Slot;             // 슬롯 이미지
    [SerializeField]
    private Text[] text_SlotName;           // 슬롯 이름
    [SerializeField]
    private Text[] text_SlotDesc;           // 슬롯 설명
    [SerializeField]
    private Text[] text_SlotNeedItem;       // 건축시 필요한 아이템 이름

    // 필요한 컴포넌트
    private Inventory theInventory;         // 재료 개수 정보관리를 위한 변수


    void Start()
    {
        theInventory = FindObjectOfType<Inventory>();
        tabNumber = 0;
        page = 1;
        TabSlotSetting(craft_fire);     // 시작할때 모닥불 관련 탭으로 설정, 안할시 처음시작시 오류 발생
    }

    // 버튼클릭은 pulbic 으로 설정
    public void TabSetting(int _tabNumber)
    {
        tabNumber = _tabNumber;
        page = 1;

        switch(tabNumber)       // 탭 숫자에 따른 건축물 종류 선정
        {
            case 0:
                TabSlotSetting(craft_fire);   // 불 세팅
                break;
            case 1:
                TabSlotSetting(craft_build);   // 건물 세팅
                break;
        }
    }

    private void ClearSlot()        // 슬롯 관련 정보 비활성화, 슬롯이 바뀔때마다 해주어야 한다
    {
        for (int i = 0; i < go_Slots.Length; i++)
        {
            image_Slot[i].sprite = null;
            text_SlotName[i].text = "";
            text_SlotDesc[i].text = "";
            text_SlotNeedItem[i].text = "";
            go_Slots[i].SetActive(false);
        }
    }

    public void RightPageSetting()      // 우측 화살표 클릭시 페이지 설정함수
    {
        // 최대 페이지를 넘지 않도록 (건축물길이 / 슬롯길이 = 페이지, +1 은 page 초기값이 1이여서)
        if (page < (craft_SelectedTab.Length / go_Slots.Length) + 1)
            page++;
        else        // 최대 페이지 넘으면
            page = 1;       // 첫 페이지로 이동

        TabSlotSetting(craft_SelectedTab);      // 탭슬롯 설정
    }
    public void LeftPageSetting()       // 좌측 화살표 클릭시 페이지 설정함수
    {
        // 첫번째 페이가 아닐경우 
        if (page != 1)
            page--;
        else        // 첫번째 페이지일 경우  , 최대 페이지(건축물길이 / 슬롯길이 = 페이지, +1 은 page 초기값이 1이여서) 로 설정
            page = (craft_SelectedTab.Length / go_Slots.Length) + 1;       // 최대 페이지로 이동

        TabSlotSetting(craft_SelectedTab);      // 탭슬롯 설정
    }

    private void TabSlotSetting(Craft[] _craft_tab)     // 탭슬롯 설정, 탭 클릭시 해당 건축 분류 슬롯으로 설정
    {
        ClearSlot();            // 슬롯 관련 정보 비활성화
        craft_SelectedTab = _craft_tab;     // 선택한 건축 분류 탭
        // 페이지 마다 첫번째의 슬롯 번호를 나타냄 , 4의 배수
        int startSlotNumber = (page - 1) * go_Slots.Length;

        // 건축물 개수만큼 반복
        for (int i = startSlotNumber; i < craft_SelectedTab.Length; i++)
        {
            if (i == page * go_Slots.Length)    // 다음페이지로 바뀔때(다음 페이지 슬롯을 탐색할때)
                break;
            // 앞슬롯부터 순차적으로 활성화
            go_Slots[i - startSlotNumber].SetActive(true);

            // 앞슬롯부터 순차적으로 해당 건축물 이미지 대입, 이름 대입, 설명, 필요한 아이템 대입
            image_Slot[i - startSlotNumber].sprite = craft_SelectedTab[i].craftImage;
            text_SlotName[i - startSlotNumber].text = craft_SelectedTab[i].craftName;
            text_SlotDesc[i - startSlotNumber].text = craft_SelectedTab[i].craftDesc;
            // 필요한 아이템은 여러개일 수도 있어서 for문으로 돌려서 다 넣어준다
            for (int x = 0; x < craft_SelectedTab[i].craftNeedItem.Length; x++)
            {
                // 아이템이 x개 이상일때는 합해서 추가해준다
                // 아이템 이름 x 아이템 수량 형태로 텍스트가 나온다
                text_SlotNeedItem[i - startSlotNumber].text += craft_SelectedTab[i].craftNeedItem[x];
                text_SlotNeedItem[i - startSlotNumber].text += " x " + craft_SelectedTab[i].craftNeedItemCount[x] + "\n";
            }
        }
    }

    public void SlotClick(int _slotNumber)      // 슬롯을 클릭하면 발생하는 함수
    {
        // 페이지 마다 클릭한 슬롯의 번호를 나타냄
        selectedSlotNumber = _slotNumber + (page - 1) * go_Slots.Length;

        if (!CheckIngredient())     // 재료가 없다면
            return;                 // 함수 종료

        // 미리보기 프리팹 플레이어기준 정면에 생성후,  관리를 하기 위한 변수에 대입
        go_Preview = Instantiate(craft_SelectedTab[selectedSlotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        // 건축 메뉴얼에서 클릭한 슬롯에 대한 실제 프리팹에 대한 정보를 대입
        go_Prefab = craft_SelectedTab[selectedSlotNumber].go_Prefab;
        isPreviewActivated = true;          // 미리보기 활성화
        go_BaseUI.SetActive(false);         // 기본 베이스 UI 비활성화
    }

    private bool CheckIngredient()      // 재료 개수 확인 함수
    {
        // 선택한 슬롯의 아이템 개수만큼 반복
        for (int i = 0; i < craft_SelectedTab[selectedSlotNumber].craftNeedItem.Length; i++)
        {
            // 선택된 건축탭 슬롯의 필요한 아이템의 수량이 실제 인벤토리 or 퀵슬롯에 있는 아이템의 수량보다 적다면
            if(theInventory.GetItemCount(craft_SelectedTab[selectedSlotNumber].craftNeedItem[i])  
                < craft_SelectedTab[selectedSlotNumber].craftNeedItemCount[i])
            return false;
        }
        return true;
    }

    private void UseIngredient()
    {
        // 선택한 슬롯의 아이템 개수만큼 반복
        for (int i = 0; i < craft_SelectedTab[selectedSlotNumber].craftNeedItem.Length; i++)
        {
            // 선택된 건축탭 슬롯의 필요한 아이템의 수량만큼 인벤토리 or 퀵슬롯의 아이템재료의 수량을 차감
            theInventory.SetItemCount(craft_SelectedTab[selectedSlotNumber].craftNeedItem[i], 
                craft_SelectedTab[selectedSlotNumber].craftNeedItemCount[i]);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)        // 탭키를 눌렀을때 && 미리보기 비활성일때
            Window();           // 창 제어 함수(열기, 닫기)

        if (isPreviewActivated)             // 미리보기 활성화시
            PreviewPositionUpdate();        // 미리보기 프리팹 위치 업데이트

        if (Input.GetButtonDown("Fire1"))    // 마우스 좌버튼 클릭시
            Build();        // 실제 프리팹 생성

        if (Input.GetKeyDown(KeyCode.Escape))
            Cancel();           // 취소 함수
    }

    private void Build()            // 실제 프리팹 생성
    {
        // 미리보기 활성화시 && 건물을 지을 수 있을때
        if (isPreviewActivated && go_Preview.GetComponent<PreviewObject>().IsBuildable())      
        {
            UseIngredient();
            // 레이저가 닿는 좌표에 실제 프리팹 생성, 미리보기 프리팹의 회전값을 적용
            Instantiate(go_Prefab, go_Preview.transform.position, go_Preview.transform.rotation);
            Destroy(go_Preview);        // 미리보기 프리팹 삭제
            // 상태변수, 프리팹, UI 비활성화
            isActiavted = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
        }
    }

    private void PreviewPositionUpdate()        // 미리보기 프리팹 위치 업데이트 함수
    {
        // 매 프레임 레이저 생성
        if (Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask))
        {
            if(hitInfo.transform != null)       // 충돌물체가 있을때
            {
                // hitInfo.point => 레이저를 쏴서 맞은 곳의 좌표를 반환
                Vector3 _location = hitInfo.point;

                if(Input.GetKeyDown(KeyCode.Q))
                    go_Preview.transform.Rotate(0f, -90f, 0f);      // 미리보기 프리팹 반시계 방향으로 돌리기
                else if(Input.GetKeyDown(KeyCode.E))
                    go_Preview.transform.Rotate(0f, +90f, 0f);      // 미리보기 프리팹 시계 방향으로 돌리기

                // x, z 좌표를 반올림하여 설정하고, y 좌표는 더욱 세분화해서 나눠주도록 설정
                _location.Set(Mathf.Round(_location.x), Mathf.Round(_location.y / 0.1f) * 0.1f, Mathf.Round(_location.z));
                go_Preview.transform.position = _location;      // 레이저의 좌표를 따라서 미리보기 프리팹 움직임 => 마우스를 따라가는 거처럼 보임
            }
        }
    }

    private void Cancel()               // 취소 함수
    {
        if(isPreviewActivated)              // 미리보기 활성화시
            Destroy(go_Preview);            // 미리보기 프리팹 삭제

        // 상태변수, 프리팹, UI 비활성화
        isActiavted = false;
        isPreviewActivated = false;
        go_Preview = null;
        go_Prefab = null;
        go_BaseUI.SetActive(false);

    }

    private void Window()           // 창 제어 함수(열기, 닫기)
    {
        if (!isActiavted)               // 크래프트 메뉴얼이 비활성화일때
            OpenWindow();           // 창을 여는 함수
        else                            // 크래프트 메뉴얼이 활성화일때
            CloseWindow();          // 창을 닫는 함수
    }

    private void OpenWindow()                    // 창을 여는 함수
    {
        isActiavted = true;                      // 상태변수 활성화
        go_BaseUI.SetActive(true);               // 오브젝트 활성화
    }

    private void CloseWindow()                  // 창을 닫는 함수
    {
        isActiavted = false;                    // 상태변수 비활성화
        go_BaseUI.SetActive(false);             // 오브젝트 비활성화
    }
}
