using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotController : MonoBehaviour
{
    [SerializeField]
    private Slot[] quickSlots;      // 퀵슬롯 배열
    [SerializeField]
    private Transform tf_parent;    // 퀵슬롯 부모 객체

    private int selectedSlot;       // 선택된 퀵슬롯 번호 (0~7), 8개

    // 필요한 컴포넌트
    [SerializeField]
    private GameObject go_SelectedImage;    // 선택된 퀵슬롯의 이미지
    [SerializeField]
    private WeaponManager theWeaponManager;     // 무기 교체를 위한 변수

    // Start is called before the first frame update
    void Start()
    {
        quickSlots = tf_parent.GetComponentsInChildren<Slot>();     // 퀵슬롯 부모 객체 밑에 자식 객체들을 할당
        selectedSlot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        TryInputNumber();
    }

    private void TryInputNumber()       // 퀵슬롯 선택
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

    private void ChangeSlot(int _num)       // 퀵슬롯 번호 교체
    {
        SelectedSlot(_num);

        Execute();
    }

    private void SelectedSlot(int _num)     // 퀵슬롯 번호 선택
    {
        selectedSlot = _num;        // 선택된 슬롯 번호
        go_SelectedImage.transform.position = quickSlots[selectedSlot].transform.position;      // 선택된 슬롯 번호에 해당하는 위치로 이미지 이동
    }

    private void Execute()
    {
        if(quickSlots[selectedSlot].item != null)       // 선택된 퀵슬롯에 아이템이 있다면
        {
            if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Equipment)      // 퀵슬롯에 아이템이 장비라면
                StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(quickSlots[selectedSlot].item.weaponType, quickSlots[selectedSlot].item.itemName));       // 퀵슬롯에 해당하는 무기로 교체
            else if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Used)      // 퀵슬롯에 아이템이 소모품이라면
                StartCoroutine(theWeaponManager.ChangeWeaponCoroutine("HAND", "맨손"));   // 맨손으로 교체
            else
                StartCoroutine(theWeaponManager.ChangeWeaponCoroutine("HAND", "맨손"));   // 맨손으로 교체
        }
        else                            // 선택된 퀵슬롯에 아이템이 없다면
        {
            StartCoroutine(theWeaponManager.ChangeWeaponCoroutine("HAND", "맨손"));   // 맨손으로 교체
        }
    }
}