using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private Vector3 originPos;  // 마우스 이벤트시 필요한 원래 위치

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
    // 이미 하이어라키에 꺼내 놓은 프리팹 객체는 가능하지만 Instantiate()로 생성했을 경우에 해당된다.
    private WeaponManager theWeaponManager;     // 총 장착을 위한 WeaponManager 변수

    void Start()
    {
        originPos = transform.position;
        theWeaponManager = FindObjectOfType<WeaponManager>();
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
                if(item.itemType == Item.ItemType.Equipment)        // 장비 아이템이라면
                {
                    // 장착
                    StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(item.weaponType, item.itemName)); // WeaponManager에 무기교체 코루틴 함수 실행
                }
                else                    // 장비 아이템이 아니라면
                {
                    // 소모
                    Debug.Log(item.itemName + " 을 사용했습니다");
                    SetSlotCount(-1);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item != null)
        {
            transform.position = eventData.position;        // 슬롯의 위치를 이벤트가 발생한 위치로 바꾼다
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            transform.position = eventData.position;        // 슬롯의 위치를 이벤트가 발생한 위치로 바꾼다
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = originPos;
    }

    public void OnDrop(PointerEventData eventData)
    {
    }
}
