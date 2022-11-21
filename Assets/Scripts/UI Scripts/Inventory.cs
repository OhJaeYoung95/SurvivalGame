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

    private Slot[] slots;                           // 인벤토리 슬롯 배열
    private Slot[] quickslots;                      // 퀵 슬롯 배열
    private bool isNotPut;                          // 인벤토리든 퀵슬롯이든 꽉 찼을경우


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
        go_InventoryBase.SetActive(true);
    }

    private void CloseInventory()       // 인벤토리 비활성화
    {
        go_InventoryBase.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count = 1)     // 아이템 획득, _count값 없으면 default값으로 1이 들어간다
    {
            // 첫번째 인자에 quickslots(퀵슬롯), slots(인벤토리)에 넣을건지 정한다
            PutSlot(quickslots, _item, _count);
            if (isNotPut)   // 인벤토리든 퀵슬롯이든 꽉 찼을경우 
            {
                PutSlot(slots, _item, _count);
            }
            if (isNotPut)       // 인벤토리 퀵슬롯 둘다 꽉 찼을경우
                Debug.Log("퀵슬롯과 인벤토리가 꽉찼습니다");
    }
    public void PutSlot(Slot[] _slots, Item _item, int _count)     // 아이템 획득, _count값 없으면 default값으로 1이 들어간다
    {
        if (Item.ItemType.Equipment != _item.itemType)       // 아이템 타입이 장비가 아니라면
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].item != null)                   // 슬롯이 빈칸이 아니라면
                {
                    if (_slots[i].item.itemName == _item.itemName)        // 슬롯의 아이템과 얻은 아이템의 이름이 같다면
                    {
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
}
