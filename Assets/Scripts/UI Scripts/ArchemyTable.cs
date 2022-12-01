using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ArchemyItem            // 연금 아이템
{
    public string itemName;         // 연금 아이템 이름
    public string itemDesc;         // 연금 아이템 설명
    public Sprite itemImage;        // 연금 아이템 이미지

    public float itemCraftingTime;      // 포션 제조에 걸리는 시간 (5초, 10초, 100초)

    public GameObject go_ItemPrefab;    // 연금 아이템 프리팹
}

public class ArchemyTable : MonoBehaviour
{
    public bool GetIsOpen()                 // 외부에서 아이템 연금창 활성화 여부를 얻게 해주는 함수
    {
        return isOpen;
    }
    private bool isOpen = false;            // 아이템 연금창 활성화 여부(중복 방지)
    private bool isCrafting = false;        // 아이템의 제작 시작 여부


    [SerializeField]
    private ArchemyItem[] archemyItems;         // 제작할 수 있는 연금 아이템 종류
    private Queue<ArchemyItem> archemyItemQueue = new Queue<ArchemyItem>();     // 연금 아이템 제작 대기열(큐)
    private ArchemyItem currentCraftingItem;        // 현재 제작중인 연금 아이템

    private float craftingTime;                 // 연금 아이템 제작 시간
    private float currentCraftingTime;          // 실제 제작 시간
    private int page = 1;                       // 연금 제작 테이블의 페지이 번호
    [SerializeField]
    private int theNumberOfSlot;                // 한 페이지당 슬롯의 최대 개수(4개)   

    [SerializeField]
    private Image[] image_ArchemyItems;     // 페이지에 따른 아이템 이미지들
    [SerializeField]
    private Text[] text_ArchemyItems;       // 페이지에 따른 아이템 텍스트들
    [SerializeField]
    private Button[] btn_ArchemyItems;      // 페이지에 따른 어아템 버튼들
    [SerializeField]
    private Slider slider_Gauge;            // 슬라이더 게이지
    [SerializeField]
    private Transform tf_BaseUI;            // 베이스 UI
    [SerializeField]
    private Transform tf_PotionAppearPos;   // 포션 생성 위치
    [SerializeField]
    private GameObject go_Liquid;           // 동작시키면 액체 등장
    [SerializeField]
    private Image[] image_CraftingItems;    // 대기열 슬롯에 있는 아이템 이미지들

    void Start()
    {
        ClaerSlot();        // 아이템 슬롯 이미지 초기화
        PageSetting();      // 페이지 슬롯 이미지 설정
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsFinish())     // 제작중일때
        {
            Crafting();
        }

        if (isOpen)     // 아이템 연금창 활성화시, 중복방지
            if (Input.GetKeyDown(KeyCode.Escape))       // ESC키를 누른다면
                CloseWindow();      // 창 닫기
    }

    private bool IsFinish()     // 제작이 끝났는지에 대한 여부
    {
        // 대기열에 없을때 && 제작중이 아닐때
        if(archemyItemQueue.Count == 0 && !isCrafting)
        {
            go_Liquid.SetActive(false);
            slider_Gauge.gameObject.SetActive(false);
            return true;
        }
        else
        {
            go_Liquid.SetActive(true);
            slider_Gauge.gameObject.SetActive(true);
            return false;
        }
    }

    private void Crafting()     // 연금 아이템 제작 함수
    {
        // 제작중이 아닐때 && 대기열이 있다면
        if (!isCrafting && archemyItemQueue.Count != 0)
            DequeueItem();      // 대기열에서 나온 아이템

        if(isCrafting)      // 제작 중일때
        {
            currentCraftingTime += Time.deltaTime;      // 실제 제작계산 시간에 1초씩 증가
            slider_Gauge.value = currentCraftingTime;   // 게이지 값에 실제 제작계산 시간을 대입(게이지가 실제로 차는 구간)

            if(currentCraftingTime >= craftingTime)     // 제작 시간이 되면
                ProductionComplete();         // 연금 아이템 생산완료

        }
    }

    private void DequeueItem()      // 대기열에서 나온 아이템 함수
    {
        isCrafting = true;        // 제작중
        // 현재 제작중인 연금 아이템에 대기열에서 나온 아이템을 대입
        currentCraftingItem = archemyItemQueue.Dequeue();       

        // 연금 아이템 제작 시간에 대기열에서 나온 아이템의 제작 시간을 대입
        craftingTime = currentCraftingItem.itemCraftingTime;
        currentCraftingTime = 0;        // 실제 제작시간 초기화
        slider_Gauge.maxValue = craftingTime;   // 슬라이더 게이지 최대값 대입

        CraftingImageChange();      // 대기열에 따른 이미지 교체
    }

    private void CraftingImageChange()      // 대기열에 따른 이미지 교체
    {
        image_CraftingItems[0].gameObject.SetActive(true);  // 첫번째 대기열 이미지 활성화

        // 이미지는 그대로 남아있는데 DequeueItem()함수에서 Dequeue를 해줘서 +1을 해준다
        for (int i = 0; i < archemyItemQueue.Count + 1; i++)
        {
            // 대기열 슬롯의 아이템 이미지 한칸씩 당겨오기
            image_CraftingItems[i].sprite = image_CraftingItems[i + 1].sprite;
            // 다음번째 것이 대기열의 길이보다 커질 경우
            if (i + 1 == archemyItemQueue.Count + 1)
                image_CraftingItems[i + 1].gameObject.SetActive(false); // 해당 이미지 비활성화
        }
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
        if (archemyItemQueue.Count < 3)         // 대기열 길이안이면
        {
            // 페이지에 해당하는 슬롯 버튼을 누르기 위한 변수
            // 최대페이지 + 버튼번호 = 현재 페이지에 해당하는 슬롯버튼 번호
            int archemyItemArrayNumber = _buttonNum + ((page - 1) * theNumberOfSlot);

            // 대기열에 버튼을 누른 아이템을 합류
            archemyItemQueue.Enqueue(archemyItems[archemyItemArrayNumber]);

            // 대기열 아이템 활성화, 이미지 대입
            image_CraftingItems[archemyItemQueue.Count].gameObject.SetActive(true);
            image_CraftingItems[archemyItemQueue.Count].sprite = archemyItems[archemyItemArrayNumber].itemImage;
        }
    }

    private void ProductionComplete()     // 연금 아이템 생산완료 함수
    {
        isCrafting = false;     // 제작이 끝남
        image_CraftingItems[0].gameObject.SetActive(false);     // (아이템이 생산완료)제작이 끝나면서 0번째 이미지 비활성화

        // 연금 테이블 생성 위치에 해당 연금 아이템 생성
        Instantiate(currentCraftingItem.go_ItemPrefab, tf_PotionAppearPos.position, Quaternion.identity);
    }

    public void UpButton()      // 페이지 업버튼 함수
    {
        if (page != 1)   // 페이지가 첫번째 장이 아니라면
            page--;     // 페이지 번호 -1
        else            // 아이템 길이 / 슬롯 최대 갯수 = 페이지 갯수 + 1 => 최대 페이지
            page = 1 + (archemyItems.Length / theNumberOfSlot);

        ClaerSlot();        // 아이템 슬롯 이미지 초기화
        PageSetting();      // 페이지 슬롯 이미지 설정
    }
    public void DownButton()    // 페이지 다운버튼 함수
    {
        if (page < 1 + (archemyItems.Length / theNumberOfSlot))  // 최대 페이지 보다 작을때
            page++;     // 페이지 번호 +1
        else        // 최대 페이지를 넘을때 첫번째 장으로 이동
            page = 1;

        ClaerSlot();        // 아이템 슬롯 이미지 초기화
        PageSetting();      // 페이지 슬롯 이미지 설정
    }

    private void ClaerSlot()    // 아이템 슬롯 이미지 초기화 함수
    {
        // 슬롯 아이템창 개수
        for (int i = 0; i < theNumberOfSlot; i++)
        {   // 연금 아이템 슬롯 비활성화, 이미지,버튼, 텍스트 초기화 작업
            image_ArchemyItems[i].sprite = null;
            image_ArchemyItems[i].gameObject.SetActive(false) ;
            btn_ArchemyItems[i].gameObject.SetActive(false);
            text_ArchemyItems[i].text = "";
        }
    }

    private void PageSetting()      // 페이지 슬롯 이미지 설정 함수
    {
        // 페이지당 첫번째 슬롯의 번호
        int pageArrayStartNumber = (page - 1) * theNumberOfSlot;    // 0, 4, 8, 12

        for (int i = pageArrayStartNumber; i < archemyItems.Length; i++)
        {
            // 끝번호의 슬롯을 넘어가면
            if (i == page * theNumberOfSlot)
                break;

            // 시작번호 ~ 끝번호의 슬롯 활성화, 이미지, 버튼, 텍스트 대입 활성화
            image_ArchemyItems[i - pageArrayStartNumber].sprite = archemyItems[i].itemImage;
            image_ArchemyItems[i - pageArrayStartNumber].gameObject.SetActive(true);
            btn_ArchemyItems[i - pageArrayStartNumber].gameObject.SetActive(true);
            text_ArchemyItems[i - pageArrayStartNumber].text =  archemyItems[i].itemName + "\n" + archemyItems[i].itemDesc;
        }
    }

}
