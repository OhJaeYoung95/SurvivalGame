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

    private Slot[] slots;                           // �κ��丮 ���� �迭
    private Slot[] quickslots;                      // �� ���� �迭
    private bool isNotPut;                          // �κ��丮�� �������̵� �� á�����


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
        go_InventoryBase.SetActive(true);
    }

    private void CloseInventory()       // �κ��丮 ��Ȱ��ȭ
    {
        go_InventoryBase.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count = 1)     // ������ ȹ��, _count�� ������ default������ 1�� ����
    {
            // ù��° ���ڿ� quickslots(������), slots(�κ��丮)�� �������� ���Ѵ�
            PutSlot(quickslots, _item, _count);
            if (isNotPut)   // �κ��丮�� �������̵� �� á����� 
            {
                PutSlot(slots, _item, _count);
            }
            if (isNotPut)       // �κ��丮 ������ �Ѵ� �� á�����
                Debug.Log("�����԰� �κ��丮�� ��á���ϴ�");
    }
    public void PutSlot(Slot[] _slots, Item _item, int _count)     // ������ ȹ��, _count�� ������ default������ 1�� ����
    {
        if (Item.ItemType.Equipment != _item.itemType)       // ������ Ÿ���� ��� �ƴ϶��
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].item != null)                   // ������ ��ĭ�� �ƴ϶��
                {
                    if (_slots[i].item.itemName == _item.itemName)        // ������ �����۰� ���� �������� �̸��� ���ٸ�
                    {
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
}
