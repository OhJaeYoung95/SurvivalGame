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

    private Slot[] slots;                           // ���� �迭



    // Start is called before the first frame update
    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();     // ���� �迭�� �׸��� ���� �ڽ� ���Ե� ����
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
        if(Item.ItemType.Equipment != _item.itemType)       // ������ Ÿ���� ��� �ƴ϶��
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if(slots[i].item != null)                   // ������ ��ĭ�� �ƴ϶��
                {
                    if (slots[i].item.itemName == _item.itemName)        // ������ �����۰� ���� �������� �̸��� ���ٸ�
                    {
                        slots[i].SetSlotCount(_count);      // ������ ���� ����
                        return;
                    }
                }
            }
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item == null)        // ������ ��ĭ�̸�
            {
                slots[i].AddItem(_item, _count);    // ������ ȹ��
                return;
            }
        }
    }
}
