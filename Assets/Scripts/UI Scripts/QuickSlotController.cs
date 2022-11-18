using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotController : MonoBehaviour
{
    [SerializeField]
    private Slot[] quickSlots;      // ������ �迭
    [SerializeField]
    private Transform tf_parent;    // ������ �θ� ��ü

    private int selectedSlot;       // ���õ� ������ ��ȣ (0~7), 8��

    // �ʿ��� ������Ʈ
    [SerializeField]
    private GameObject go_SelectedImage;    // ���õ� �������� �̹���
    [SerializeField]
    private WeaponManager theWeaponManager;     // ���� ��ü�� ���� ����

    // Start is called before the first frame update
    void Start()
    {
        quickSlots = tf_parent.GetComponentsInChildren<Slot>();     // ������ �θ� ��ü �ؿ� �ڽ� ��ü���� �Ҵ�
        selectedSlot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        TryInputNumber();
    }

    private void TryInputNumber()       // ������ ����
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeSlot(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeSlot(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeSlot(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            ChangeSlot(3);
        else if(Input.GetKeyDown(KeyCode.Alpha5))
            ChangeSlot(4);
        else if(Input.GetKeyDown(KeyCode.Alpha6))
            ChangeSlot(5);
        else if(Input.GetKeyDown(KeyCode.Alpha7))
            ChangeSlot(6);
        else if(Input.GetKeyDown(KeyCode.Alpha8))
            ChangeSlot(7);
    }

    private void ChangeSlot(int _num)       // ������ ��ȣ ��ü
    {
        SelectedSlot(_num);

        Execute();
    }

    private void SelectedSlot(int _num)     // ������ ��ȣ ����
    {
        selectedSlot = _num;        // ���õ� ���� ��ȣ
        go_SelectedImage.transform.position = quickSlots[selectedSlot].transform.position;      // ���õ� ���� ��ȣ�� �ش��ϴ� ��ġ�� �̹��� �̵�
    }

    private void Execute()
    {
        if(quickSlots[selectedSlot].item != null)       // ���õ� �����Կ� �������� �ִٸ�
        {
            if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Equipment)      // �����Կ� �������� �����
                StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(quickSlots[selectedSlot].item.weaponType, quickSlots[selectedSlot].item.itemName));       // �����Կ� �ش��ϴ� ����� ��ü
            else if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Used)      // �����Կ� �������� �Ҹ�ǰ�̶��
                StartCoroutine(theWeaponManager.ChangeWeaponCoroutine("HAND", "�Ǽ�"));   // �Ǽ����� ��ü
            else
                StartCoroutine(theWeaponManager.ChangeWeaponCoroutine("HAND", "�Ǽ�"));   // �Ǽ����� ��ü
        }
        else                            // ���õ� �����Կ� �������� ���ٸ�
        {
            StartCoroutine(theWeaponManager.ChangeWeaponCoroutine("HAND", "�Ǽ�"));   // �Ǽ����� ��ü
        }
    }
}