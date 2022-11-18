using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item;           // 획득한 아이템의 정보를 가져오기 위한 Item 변수
    public int itemCount;       // 획득한 아이템 수량
    public Image itemImage;     // 아이템의 이미지
    
    // 필요한 컴포넌트
    [SerializeField]
    private Text text_Count;                    // 아이템 수량 나타내주는 텍스트
    [SerializeField]
    private GameObject go_CountImage;           // 아이템 수량 나타내는곳의 이미지 게임오브젝트

    // 프리팹으로 된 객체들은 자기 자신안의 있는 자식들만 [SerializeField]로 정보를 가져올 수 있다
    // 하지만 다른 객체들을 끌어올 수 는 없기에 이와 같이 Start 함수에서 초기화 하면서 대입해줘야 한다.
    // 이미 하이어라키에 꺼내 놓은 프리팹 객체는 개별적으로 넣을순 있지만
    // Instantiate()로 생성했을 경우에는 넣어지지 않는다.
    // 하이어라키에 꺼내 놓은 프리팹객체에 개별적으로 넣고 Aplly All을 해도
    // 다른 프리팹 객체에는 적용되지 않는다.
    [SerializeField]
    private RectTransform baseRect;                  // 인벤토리 영역
    private InputNumber theInputNumber;     // 아이템 버리는 갯수 창(인풋필드) 컨트롤을 위한 변수                                        
    private ItemEffectDatabase theItemEffectDatabase;       // 아이템 사용효과 이펙트를 위한 변수
    [SerializeField]
    private RectTransform quickSlotBaseRect;        // 퀵슬롯 영역
    void Start()
    {
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
        theInputNumber = FindObjectOfType<InputNumber>();
    }
    private void SetColor(float _alpha)         // 아이템 슬롯 활성화, 비활성화  / 투명도 조절
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }
                                                        // 아이템 획득
    public void AddItem(Item _item, int _count = 1)     // _count 값을 생략하면 기본값이 들어감
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;          // item클래스의 itemImage는 스프라이트이기에 .sprite로 형 변환

        if(item.itemType != Item.ItemType.Equipment)        // 획득 아이템이 장비가 아니라면
        {
            go_CountImage.SetActive(true);              // 획득한 아이템 수량 이미지 활성화
            text_Count.text = itemCount.ToString();     // int 타입은 text에 호환되지 않아 string 타입으로 형 변환
        }
        else                    // 획득 아이템이 장비라면
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }

        SetColor(1);            // 아이템 슬롯 활성화
    }

    public void SetSlotCount(int _count)        // 슬롯아이템 개수 설정
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }

    private void ClearSlot()            // 슬롯 초기화
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)  // 이 스크립트가 적용돤 객체에다가 마우스 우클릭시
        {
            if(item != null)            // 아이템이 있다면
            {
                    theItemEffectDatabase.UseItem(item);        // 아이템 사용 효과
                if(item.itemType == Item.ItemType.Used)         // 아이템 타입이 소모품이라면
                    SetSlotCount(-1);       // 아이템 수량 1 소모
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)         // 드래그 시작
    {
        if(item != null)
        {
            DragSlot.instance.dragSlot = this;               // DragSlot instance에 자기 자신을 대입
            DragSlot.instance.DragSetImage(itemImage);          // 드래그 하는 이미지 활성화
            DragSlot.instance.transform.position = eventData.position;        // 드래그 하는 슬롯의 위치를 이벤트가 발생한 위치로 바꾼다
        }
    }

    public void OnDrag(PointerEventData eventData)              // 드래그 중
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;     // 드래그 하는 슬롯의 위치를 이벤트가 발생한 위치로 바꾼다
        }

    }

    public void OnEndDrag(PointerEventData eventData)           // 드래그가 끝나기만 하면 호출됨
    {
        // !(인벤토리 영역 || // 퀵슬롯 영역), 인벤토리와 퀵슬롯 영역이 아니라면
        if (!(((DragSlot.instance.transform.localPosition.x > baseRect.rect.xMin && DragSlot.instance.dragSlot.transform.localPosition.x < baseRect.rect.xMax
            && DragSlot.instance.transform.localPosition.y > baseRect.rect.yMin && DragSlot.instance.dragSlot.transform.localPosition.y < baseRect.rect.yMax))
            ||   
            (DragSlot.instance.transform.localPosition.x > quickSlotBaseRect.rect.xMin && DragSlot.instance.dragSlot.transform.localPosition.x < quickSlotBaseRect.rect.xMax
            && DragSlot.instance.transform.localPosition.y < quickSlotBaseRect.transform.localPosition.y -quickSlotBaseRect.rect.yMin 
            && DragSlot.instance.dragSlot.transform.localPosition.y > quickSlotBaseRect.transform.localPosition.y - quickSlotBaseRect.rect.yMax)))
        {
            if(DragSlot.instance.dragSlot != null)      // 드래그한 슬롯이 빈칸이 아니라면
            {
                theInputNumber.Call();                  //아이템 버리는 갯수 창(인풋필드) 활성화      
            }
        }
        else        // 인벤토리 영역 안이라면
        {
            DragSlot.instance.SetColor(0);              // instance 객체 비활성화
            DragSlot.instance.dragSlot = null;          // dragSlot 객체 제거
        }
    }

    public void OnDrop(PointerEventData eventData)              // 다른 슬롯위에서 드래그가 끝났을때만 호출
    {
        if(DragSlot.instance.dragSlot != null)          // 드래그 슬롯에 아이템이 있을때
            ChangeSlot();                               // 슬롯 변경         
    }

    private void ChangeSlot()               // 슬롯 변경
    {
        // 슬롯 변경전 원래 있던 곳의 아이템 임시로 복사
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (_tempItem != null)      // 원래 있던곳에 아이템이 있다면
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);      // 임시 복사본 아이템 붙여넣기, 드래그 시작했던 자리로 옮기기
        else                        // 원래 있던곳에 아이템이 없다면
            DragSlot.instance.dragSlot.ClearSlot();         // 슬롯 초기화
    }

    public void OnPointerEnter(PointerEventData eventData)     // 마우스가 슬롯에 들어갈때 발동
    {
        if(item !=null)                 // 아이템이 있다면
            theItemEffectDatabase.ShowToolTip(item, transform.position);            // 아이템 툴팁 활성화
    }

    public void OnPointerExit(PointerEventData eventData)     // 마우스가 슬롯에서 빠져나갈 때 발동
    {
        theItemEffectDatabase.HideToolTip();            // 아이템 툴팁 비활성화
    }
}
