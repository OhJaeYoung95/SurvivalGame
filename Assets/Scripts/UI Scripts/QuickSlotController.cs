using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotController : MonoBehaviour
{
    [SerializeField]
    private Slot[] quickSlots;      // 퀵슬롯 배열
    [SerializeField]
    private Image[] img_CoolTime;   // 퀵슬롯 쿨타임
    [SerializeField]
    private Transform tf_parent;    // 퀵슬롯 부모 객체

    [SerializeField]
    private Transform tf_ItemPos;   // 아이템이 위치할 손 끝
    public static GameObject go_HandItem;   // 손에 든 아이템

    // 쿨타임 내용
    [SerializeField]
    private float coolTime;         // 쿨타임
    private float currentCoolTime;  // 현재 쿨타임
    private bool isCoolTime;        // 쿨타임 진행중인지

    // 퀵슬롯 등장 내용
    [SerializeField]
    private float appearTime;           // 퀵슬롯이 등장하는 시간
    private float currentAppearTime;    // 현재 퀵슬롯이 등장해있는 시간
    private bool isAppear;              // 등장해 있는지


    private int selectedSlot;       // 선택된 퀵슬롯 번호 (0~7), 8개

    // 필요한 컴포넌트
    [SerializeField]
    private GameObject go_SelectedImage;    // 선택된 퀵슬롯의 이미지
    [SerializeField]
    private WeaponManager theWeaponManager;     // 무기 교체를 위한 변수
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        quickSlots = tf_parent.GetComponentsInChildren<Slot>();     // 퀵슬롯 부모 객체 밑에 자식 객체들을 할당
        anim = GetComponent<Animator>();
        selectedSlot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        TryInputNumber();
        CoolTimeCalc();
        AppearCalc();
    }

    private void AppearReset()      // 퀵슬롯 등장 시간 초기화
    {
        currentAppearTime = appearTime;     // 등장 시간 초기화
        isAppear = true;                    // 등장 가능하게 설정
        anim.SetBool("Appear", isAppear);   // 퀵슬롯이 등장 하는 애니메이션 실행
    }

    private void AppearCalc()           // 퀵슬롯 등장 시간 계산
    {
        if (Inventory.inventoryActivated)       // 인벤토리창이 활성화 되어있을땐
            AppearReset();                      // 퀵슬롯창 등장
        else
        {
            if (isAppear)            // 퀵슬롯이 등장해 있으면
            {
                currentAppearTime -= Time.deltaTime;        // 1초에 1 감소, 퀵슬롯이 등장해 있을수 있는 시간 감소
                if (currentAppearTime <= 0)                  // 등장시간이 끝나면
                {
                    isAppear = false;                       // 등장 못하게 설정
                    anim.SetBool("Appear", isAppear);       // 퀵슬롯이 사라지는 애니메이션 실행
                }
            }
        }
    }

    private void CoolTimeReset()        // 쿨타임 초기화
    {
        currentCoolTime = coolTime;     // 쿨타임 시간 초기화
        isCoolTime = true;              // 쿨타임 시작
    }

    private void CoolTimeCalc()     // 쿨타임 시간 계산함수
    {
        if(isCoolTime)          // 쿨타임 진행중일때
        {
            currentCoolTime -= Time.deltaTime;      // 1초에 1씩감소
            for (int i = 0; i < img_CoolTime.Length ; i++)
                img_CoolTime[i].fillAmount = currentCoolTime / coolTime;        // 쿨타임에 따른 이미지 채우기
            if (currentCoolTime <= 0)           // 현재 쿨타임이 0보다 작아지면
                isCoolTime = false;             // 쿨타임 끝
        }
    }

    private void TryInputNumber()       // 퀵슬롯 선택
    {
        if(!isCoolTime)     // 쿨타임이 아닐때만 슬롯선택 가능
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                ChangeSlot(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                ChangeSlot(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                ChangeSlot(2);
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                ChangeSlot(3);
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                ChangeSlot(4);
            else if (Input.GetKeyDown(KeyCode.Alpha6))
                ChangeSlot(5);
            else if (Input.GetKeyDown(KeyCode.Alpha7))
                ChangeSlot(6);
            else if (Input.GetKeyDown(KeyCode.Alpha8))
                ChangeSlot(7);
        }
    }

    public void IsActivatedQuickSlot(int _num)      // 퀵슬롯이 활성화 되어있을때
    {                                               
        if (selectedSlot == _num )    // 선택된 퀵슬롯 넘버와 일치한다면
        {
            Execute();      // 해당 퀵슬롯에 대해서 실행해주는 함수
            return;
        }

        if(DragSlot.instance != null)
        {
            if(DragSlot.instance.dragSlot != null)
            {
                // Slot에서 quickSlotNumber는 private이라 GetQuickSlotNumber()함수를 이용해 값을 가져와서 비교한다
                if (DragSlot.instance.dragSlot.GetQuickSlotNumber() == selectedSlot)
                {
                    Execute();      // 해당 퀵슬롯에 대해서 실행해주는 함수
                    return;
                }
            }
        }
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

    private void Execute()  //해당 퀵슬롯에 대해서 실행해주는 함수
    {
        CoolTimeReset();                // 쿨타임 초기화
        AppearReset();                  // 퀵슬롯 등장

        if (quickSlots[selectedSlot].item != null)       // 선택된 퀵슬롯에 아이템이 있다면
        {
            if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Equipment)      // 퀵슬롯에 아이템이 장비라면
                StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(quickSlots[selectedSlot].item.weaponType, quickSlots[selectedSlot].item.itemName));       // 퀵슬롯에 해당하는 무기로 교체
            else if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Used)      // 퀵슬롯에 아이템이 소모품이라면
                ChangeHand(quickSlots[selectedSlot].item);
            else
                ChangeHand();               // 맨손으로 교체
        }
        else                            // 선택된 퀵슬롯에 아이템이 없다면
        {
            ChangeHand();               // 맨손으로 교체
        }
    }

    private void ChangeHand(Item _item = null)         // 손모양 교체
    {
        StartCoroutine(theWeaponManager.ChangeWeaponCoroutine("HAND", "맨손"));   // 맨손으로 교체

        if (_item != null)
            StartCoroutine(HandItemCoroutine());
    }

    IEnumerator HandItemCoroutine()
    {
        // 람다식 : 다음 조건을 만족해야지 넘어간다
        // () => 
        HandController.isActivate = false;      // 손 비활성화
        yield return new WaitUntil(() => HandController.isActivate);      // 손이 활성화 될때까지 기다린다

        go_HandItem = Instantiate(quickSlots[selectedSlot].item.itemPrefab, tf_ItemPos.position, tf_ItemPos.rotation);
        go_HandItem.GetComponent<Rigidbody>().isKinematic = true;       // 중력 영향x
        go_HandItem.GetComponent<BoxCollider>().enabled = false;        // 충돌 영역 해제
        go_HandItem.tag = "Untagged";           // 태그 해제
        go_HandItem.layer = 6;                  // Weapon 레이어 6번
        go_HandItem.transform.SetParent(tf_ItemPos);        // 손에든 아이템을 아이템 생성위치 오브젝트를 부모로 설정
    }

    public void EatItem()           // 아이템 먹는 함수
    {
        CoolTimeReset();                // 쿨타임 초기화
        AppearReset();                  // 퀵슬롯 등장
        quickSlots[selectedSlot].SetSlotCount(-1);      // 퀵슬롯으로 사용하는 아이템 소모

        if (quickSlots[selectedSlot].itemCount <= 0)    // 남은 갯수가 0개면 
            Destroy(go_HandItem);                       // 손에 든 아이템 제거
    }

    public bool GetIsCoolTime()         // 외부에서 쿨타임여부를 확인하기 위한 변수
    {
        return isCoolTime;
    }
}