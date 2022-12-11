using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;      // 인벤토리 활성화 bool 형 변수

    // 필요한 컴포넌트
    [SerializeField]
    private GameObject go_InventoryBase;            // 인벤토리 창
    [SerializeField]
    private GameObject go_SlotsParent;              //  슬롯 그리드 세팅
    [SerializeField]
    private GameObject go_QuickSlotParent;          // 퀵슬롯 부모
    [SerializeField]
    private QuickSlotController theQuickSlot;       // 퀵슬롯 정보

    private Slot[] slots;                           // 인벤토리 슬롯 배열
    private Slot[] quickslots;                      // 퀵 슬롯 배열
    private bool isNotPut;                          // 인벤토리든 퀵슬롯이든 꽉 찼을경우
    private int slotNumber;                         // 슬롯넘버

    public Slot[] GetSlots() { return slots; }      // 인벤토리 슬롯정보 받기
    public Slot[] GetQuickSlots() { return quickslots; }    // 퀵슬롯정보 받기

    [SerializeField]
    private Item[] items;       // 아이템 정보 받기

    public void LoadToInvenSlots(int _arrayNum, string _itemName, int _itemNum)      // 인벤에 아이템 저장해주는 함수
    {       // 아이템을 전부 비교해 인벤토리에 아이템 넣어준다
        for (int i = 0; i < items.Length; i++)
            if (items[i].itemName == _itemName)
                slots[_arrayNum].AddItem(items[i], _itemNum);
    }    
    public void LoadToQuickSlots(int _arrayNum, string _itemName, int _itemNum)      // 퀵슬롯에 아이템 저장해주는 함수
    {       // 아이템을 전부 비교해 인벤토리에 아이템 넣어준다
        for (int i = 0; i < items.Length; i++)
            if (items[i].itemName == _itemName)
                quickslots[_arrayNum].AddItem(items[i], _itemNum);
    }

    // Start is called before the first frame update
    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();     // 슬롯 배열에 그리드 세팅 자식 슬롯들 설정
        quickslots = go_QuickSlotParent.GetComponentsInChildren<Slot>();     // 퀵슬롯 배열에 Contents자식 슬롯들 설정
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()     // 인벤토리 열기 시도
    {
        if (Input.GetKeyDown(KeyCode.I))    // I 키를 누르면 인벤토리 활성화, 비활성화
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
                OpenInventory();
            else
                CloseInventory();
        }
        if(Input.GetKeyDown(KeyCode.Escape))        // ESC 누르면 인벤토리 비활성화
        {
            inventoryActivated = false;
            CloseInventory();
        }
    }

    private void OpenInventory()        // 인벤토리 활성화
    {
        GameManager.isOpenInventory = true;     // 커서 활성화
        go_InventoryBase.SetActive(true);
    }

    private void CloseInventory()       // 인벤토리 비활성화
    {
        GameManager.isOpenInventory = false;     // 커서 비활성화
        go_InventoryBase.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count = 1)     // 아이템 획득, _count값 없으면 default값으로 1이 들어간다
    {
            // 첫번째 인자에 quickslots(퀵슬롯), slots(인벤토리)에 넣을건지 정한다
            PutSlot(quickslots, _item, _count);
        if (!isNotPut)      // 퀵슬롯이 꽉 안찼을경우
            theQuickSlot.IsActivatedQuickSlot(slotNumber);      // 퀵슬롯에 대해서 실행해주는 함수 실행

            if (isNotPut)   // 인벤토리든 퀵슬롯이든 꽉 찼을경우 
                PutSlot(slots, _item, _count);
            if (isNotPut)       // 인벤토리 퀵슬롯 둘다 꽉 찼을경우
                Debug.Log("퀵슬롯과 인벤토리가 꽉찼습니다");
    }
    public void PutSlot(Slot[] _slots, Item _item, int _count)     // 아이템 획득, _count값 없으면 default값으로 1이 들어간다
    {
        if (Item.ItemType.Equipment != _item.itemType     // 아이템 타입이 장비가 아니라면
            && Item.ItemType.Kit != _item.itemType)       // 아이템 타입이 키트가 아니라면
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].item != null)                   // 슬롯이 빈칸이 아니라면
                {
                    if (_slots[i].item.itemName == _item.itemName)        // 슬롯의 아이템과 얻은 아이템의 이름이 같다면
                    {
                        slotNumber = i;
                        _slots[i].SetSlotCount(_count);      // 아이템 개수 설정
                        isNotPut = false;           // 인벤토리든 퀵슬롯이든 덜 찼을경우
                        return;
                    }
                }
            }
        }
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].item == null)        // 슬롯이 빈칸이면
            {
                _slots[i].AddItem(_item, _count);    // 아이템 획득
                isNotPut = false;           // 인벤토리든 퀵슬롯이든 덜 찼을경우
                return;
            }
        }
        isNotPut = true;            // 인벤토리든 퀵슬롯이든 꽉 찼을경우
    }

     // 인벤토리 퀵슬롯 둘다 확인해서 아이템 개수 반환해주는 함수
    public int GetItemCount(string _itemName)      
    {
        // 임시 변수에 인벤토리에서 가져온 아이템 개수를 대입
        int temp = SearchSlotItem(slots, _itemName);

        // 인벤토리 슬롯의 아이템이 없다면 퀵슬롯에서 찾게 해준다
        return temp != 0 ? temp : SearchSlotItem(quickslots, _itemName);
    }

    // 인벤토리 or 퀵슬롯에서 슬롯에 있는 아이템의 개수를 반환해 주는 함수
    private int SearchSlotItem(Slot[]_slots, string _itemName)
    {
        // (인벤토리 or 퀵슬롯)슬롯 길이만큼 반복
        for(int i =0; i < _slots.Length; i++)
        {
            if(_slots[i].item != null)      // 슬롯의 아이템이 있을경우
            {
                // 슬롯의 아이템 이름과 같다면
                if (_itemName == _slots[i].item.itemName)
                    return _slots[i].itemCount;     // 슬롯의 아이템 개수 반환
            }
        }
        return 0;       // 없으면 0반환
    }

    public void SetItemCount(string _itemName, int _itemCount)      // 아이템 개수 조정
    {
        // 인벤토리 아이템 개수 조정
        if (!ItemCountAdjust(slots, _itemName, _itemCount))
            ItemCountAdjust(quickslots, _itemName, _itemCount); // 퀵슬롯 아이템 개수 조정
    }

    // 아이템 개수 조정 Bool형 함수
    private bool ItemCountAdjust(Slot[] _slots, string _itemName, int _itemCount)
    {
        // (인벤토리 or 퀵슬롯)슬롯 길이만큼 반복
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].item != null)      // 슬롯의 아이템이 있을경우
            {
                // 아이템이름이 슬롯 아이템이름과 동일하다면
                if (_itemName == _slots[i].item.itemName)
                {
                    // 슬롯 아이템 개수 설정
                    _slots[i].SetSlotCount(-_itemCount);
                    return true;    // Bool형이라 true 반환
                }
            }
        }
        return false;
    }
}
