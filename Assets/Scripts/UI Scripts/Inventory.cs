using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;      // �κ��丮 Ȱ��ȭ bool �� ����

    // �ʿ��� ������Ʈ
    [SerializeField]
    private GameObject go_InventoryBase;            // �κ��丮 â
    [SerializeField]
    private GameObject go_SlotsParent;              //  ���� �׸��� ����
    [SerializeField]
    private GameObject go_QuickSlotParent;          // ������ �θ�
    [SerializeField]
    private QuickSlotController theQuickSlot;       // ������ ����

    private Slot[] slots;                           // �κ��丮 ���� �迭
    private Slot[] quickslots;                      // �� ���� �迭
    private bool isNotPut;                          // �κ��丮�� �������̵� �� á�����
    private int slotNumber;                         // ���Գѹ�

    public Slot[] GetSlots() { return slots; }      // �κ��丮 �������� �ޱ�
    public Slot[] GetQuickSlots() { return quickslots; }    // ���������� �ޱ�

    [SerializeField]
    private Item[] items;       // ������ ���� �ޱ�

    public void LoadToInvenSlots(int _arrayNum, string _itemName, int _itemNum)      // �κ��� ������ �������ִ� �Լ�
    {       // �������� ���� ���� �κ��丮�� ������ �־��ش�
        for (int i = 0; i < items.Length; i++)
            if (items[i].itemName == _itemName)
                slots[_arrayNum].AddItem(items[i], _itemNum);
    }    
    public void LoadToQuickSlots(int _arrayNum, string _itemName, int _itemNum)      // �����Կ� ������ �������ִ� �Լ�
    {       // �������� ���� ���� �κ��丮�� ������ �־��ش�
        for (int i = 0; i < items.Length; i++)
            if (items[i].itemName == _itemName)
                quickslots[_arrayNum].AddItem(items[i], _itemNum);
    }

    // Start is called before the first frame update
    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();     // ���� �迭�� �׸��� ���� �ڽ� ���Ե� ����
        quickslots = go_QuickSlotParent.GetComponentsInChildren<Slot>();     // ������ �迭�� Contents�ڽ� ���Ե� ����
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()     // �κ��丮 ���� �õ�
    {
        if (Input.GetKeyDown(KeyCode.I))    // I Ű�� ������ �κ��丮 Ȱ��ȭ, ��Ȱ��ȭ
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
                OpenInventory();
            else
                CloseInventory();
        }
        if(Input.GetKeyDown(KeyCode.Escape))        // ESC ������ �κ��丮 ��Ȱ��ȭ
        {
            inventoryActivated = false;
            CloseInventory();
        }
    }

    private void OpenInventory()        // �κ��丮 Ȱ��ȭ
    {
        GameManager.isOpenInventory = true;     // Ŀ�� Ȱ��ȭ
        go_InventoryBase.SetActive(true);
    }

    private void CloseInventory()       // �κ��丮 ��Ȱ��ȭ
    {
        GameManager.isOpenInventory = false;     // Ŀ�� ��Ȱ��ȭ
        go_InventoryBase.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count = 1)     // ������ ȹ��, _count�� ������ default������ 1�� ����
    {
            // ù��° ���ڿ� quickslots(������), slots(�κ��丮)�� �������� ���Ѵ�
            PutSlot(quickslots, _item, _count);
        if (!isNotPut)      // �������� �� ��á�����
            theQuickSlot.IsActivatedQuickSlot(slotNumber);      // �����Կ� ���ؼ� �������ִ� �Լ� ����

            if (isNotPut)   // �κ��丮�� �������̵� �� á����� 
                PutSlot(slots, _item, _count);
            if (isNotPut)       // �κ��丮 ������ �Ѵ� �� á�����
                Debug.Log("�����԰� �κ��丮�� ��á���ϴ�");
    }
    public void PutSlot(Slot[] _slots, Item _item, int _count)     // ������ ȹ��, _count�� ������ default������ 1�� ����
    {
        if (Item.ItemType.Equipment != _item.itemType     // ������ Ÿ���� ��� �ƴ϶��
            && Item.ItemType.Kit != _item.itemType)       // ������ Ÿ���� ŰƮ�� �ƴ϶��
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].item != null)                   // ������ ��ĭ�� �ƴ϶��
                {
                    if (_slots[i].item.itemName == _item.itemName)        // ������ �����۰� ���� �������� �̸��� ���ٸ�
                    {
                        slotNumber = i;
                        _slots[i].SetSlotCount(_count);      // ������ ���� ����
                        isNotPut = false;           // �κ��丮�� �������̵� �� á�����
                        return;
                    }
                }
            }
        }
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].item == null)        // ������ ��ĭ�̸�
            {
                _slots[i].AddItem(_item, _count);    // ������ ȹ��
                isNotPut = false;           // �κ��丮�� �������̵� �� á�����
                return;
            }
        }
        isNotPut = true;            // �κ��丮�� �������̵� �� á�����
    }

     // �κ��丮 ������ �Ѵ� Ȯ���ؼ� ������ ���� ��ȯ���ִ� �Լ�
    public int GetItemCount(string _itemName)      
    {
        // �ӽ� ������ �κ��丮���� ������ ������ ������ ����
        int temp = SearchSlotItem(slots, _itemName);

        // �κ��丮 ������ �������� ���ٸ� �����Կ��� ã�� ���ش�
        return temp != 0 ? temp : SearchSlotItem(quickslots, _itemName);
    }

    // �κ��丮 or �����Կ��� ���Կ� �ִ� �������� ������ ��ȯ�� �ִ� �Լ�
    private int SearchSlotItem(Slot[]_slots, string _itemName)
    {
        // (�κ��丮 or ������)���� ���̸�ŭ �ݺ�
        for(int i =0; i < _slots.Length; i++)
        {
            if(_slots[i].item != null)      // ������ �������� �������
            {
                // ������ ������ �̸��� ���ٸ�
                if (_itemName == _slots[i].item.itemName)
                    return _slots[i].itemCount;     // ������ ������ ���� ��ȯ
            }
        }
        return 0;       // ������ 0��ȯ
    }

    public void SetItemCount(string _itemName, int _itemCount)      // ������ ���� ����
    {
        // �κ��丮 ������ ���� ����
        if (!ItemCountAdjust(slots, _itemName, _itemCount))
            ItemCountAdjust(quickslots, _itemName, _itemCount); // ������ ������ ���� ����
    }

    // ������ ���� ���� Bool�� �Լ�
    private bool ItemCountAdjust(Slot[] _slots, string _itemName, int _itemCount)
    {
        // (�κ��丮 or ������)���� ���̸�ŭ �ݺ�
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].item != null)      // ������ �������� �������
            {
                // �������̸��� ���� �������̸��� �����ϴٸ�
                if (_itemName == _slots[i].item.itemName)
                {
                    // ���� ������ ���� ����
                    _slots[i].SetSlotCount(-_itemCount);
                    return true;    // Bool���̶� true ��ȯ
                }
            }
        }
        return false;
    }
}
