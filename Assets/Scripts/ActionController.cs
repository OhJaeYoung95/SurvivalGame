using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range;    // 습득 가능한 최대 거리

    // 상태변수
    private bool pickupActivated = false;   // 아이템 습득 가능할 시 true
    private bool dissolveActivated = false;     // 고기 해체 가능할 시 true
    private bool isDissolving = false;          // 고기 해체 중에는 true
    private bool fireLookActivated = false;     // 불을 근접해서 바라볼 시 true
    private bool lookArchemyTable = false;      // 연금 테이블을 바라볼 시 true
    private bool lookComputer = false;          // 컴퓨터를 바라볼 시 true

    private RaycastHit hitInfo;         // 충돌체 정보 저장

    [SerializeField]
    private LayerMask layerMask;        // 아이템 레이어에만 반응하도록 레이어 마스크 설정

    // 필요한 컴포넌트
    [SerializeField]
    private Text actionText;            // 액션에 쓰이는 텍스트
    [SerializeField]
    private Inventory theInventory;     // 아이템 획득에 쓰일 인벤토리
    [SerializeField]
    private WeaponManager theWeaponManager;     // 무기 정보를 다루기 위한 변수
    [SerializeField]
    private QuickSlotController theQuickSlot;   // 퀵슬롯 정보를 다루기 위한 변수
    [SerializeField]
    private Transform tf_MeatDissolveTool;      // 고기 해체 툴
    [SerializeField]
    private ComputerKit theComputer;            // 컴퓨터 전원 On/Off를 위한 컴포넌트

    [SerializeField]
    private string sound_meat;              // 고기 해체 효과음 재생


    // Update is called once per frame
    void Update()
    {
        CheckAction();
        TryAction();
    }

    private void TryAction()            // 액션 실행
    {
        if(Input.GetKeyDown(KeyCode.E))     // E키를 누른다면
        {
            CheckAction();                  // 액션 확인 함수
            CanPickUp();                    // 아이템 습득 함수
            CanMeat();                      // 고기 해체 함수
            CanDropFire();                  // 불에 고기 떨구는 함수
            CanComputerPowerOn();           // 컴퓨터 전원 On 해주는 함수
            CanArchemyTableOpen();          // 연금창 열어주는 함수
        }
    }

    private void CanPickUp()                // 아이템 습득함수
    {
        if(pickupActivated)         // 아이템 습득 가능할시
        {
            if(hitInfo.transform != null)       // Raycast광선에 충돌 물체가 있다면
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득했습니다");      // 아이템 획득여부 콘솔창에 띄어줌
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);        // 충돌 물체 아이템 인벤토리에 획득
                Destroy(hitInfo.transform.gameObject);          // 아이템 게임오브젝트 제거
                InfoDisappear();                                // 아이템 획득시 아이템 정보 비활성화
            }
        }
    }

    private void CanComputerPowerOn()       // 컴퓨터 전원 On 해주는 함수
    {
        if (lookComputer)         // 컴퓨터를 바라볼 시
        {
            if (hitInfo.transform != null)       // Raycast광선에 충돌 물체가 있다면
            {
                if(!hitInfo.transform.GetComponent<ComputerKit>().isPowerOn)    // 컴퓨터 전원이 꺼져있을때
                {
                    hitInfo.transform.GetComponent<ComputerKit>().PowerOn();        // 컴퓨터 전원 On
                    InfoDisappear();                     // 컴퓨터 전원 정보 비활성화
                }
            }
        }
    }

    private void CanArchemyTableOpen()       // 연금창 열어주는 함수
    {
        if (lookArchemyTable)         // 연금 테이블을 바라볼 시
        {
            if (hitInfo.transform != null)       // Raycast광선에 충돌 물체가 있다면
            {
                hitInfo.transform.GetComponent<ArchemyTable>().Window();      // 아이템 연금창 활성화, 비활성화
                InfoDisappear();                     // 아이템 연금창 정보 비활성화

            }
        }
    }


    private void CanMeat()      // 고기 해체 함수
    {
        if(dissolveActivated)       // 고기 해체 가능할떄
        {       // 동물일때 && 동물이 죽어있을때 && 고기 해체중이 아닐때
            if((hitInfo.transform.tag == "WeakAnimal" || hitInfo.transform.tag == "StrongAnimal") 
                && hitInfo.transform.GetComponent<Animal>().isDead && !isDissolving)
            {
                isDissolving = true;    // 고기 해체중
                InfoDisappear();        // 액션정보창 비활성화
                StartCoroutine(MeatCoroutine());    // 고기 해체
            }
        }
    }

    IEnumerator MeatCoroutine()         // 고기 해체 코루틴
    {
        WeaponManager.isChangeWeapon = true;            // 고기 해체중에는 무기 교체가 안되도록
        WeaponSway.isActivated = false;                 // 무기 흔들림 비활성화
        WeaponManager.currentWeaponAnim.SetTrigger("Weapon_Out");       // 무기 무장해제 애니메이션
        PlayerController.isActivated = false;           // 플레이어 조종불가능

        yield return new WaitForSeconds(0.2f);
        
        WeaponManager.currentWeapon.gameObject.SetActive(false);        // 무기 오브젝트 비활성화
        tf_MeatDissolveTool.gameObject.SetActive(true);                 // 고기 해체 무기 활성화 동시에 애니메이션 실행

        yield return new WaitForSeconds(0.2f);
        SoundManager.instance.PlaySE(sound_meat);           // 고기 해체 효과음 재생
        yield return new WaitForSeconds(1.8f);

        // 인벤토리에 아이템 획득(동물아이템, 아이템 수량)
        theInventory.AcquireItem(hitInfo.transform.GetComponent<Animal>().GetItem(), hitInfo.transform.GetComponent<Animal>().itemNumber);

        tf_MeatDissolveTool.gameObject.SetActive(false);               // 고기 해체 무기 비활성화
        WeaponManager.currentWeapon.gameObject.SetActive(true);        // 무기 오브젝트 활성화

        PlayerController.isActivated = true;           // 플레이어 조종가능
        WeaponSway.isActivated = true;                 // 무기 흔들림 활성화
        WeaponManager.isChangeWeapon = false;       // 고기 해체끝 무기 교체 가능하도록 설정
        isDissolving = false;           // 고기 해체중이 아닐때
    
    }

    private void CanDropFire()      // 불에 고기 떨구는 함수
    {
        if(fireLookActivated)       // 불을 보고 있다면
        {
            if(hitInfo.transform.tag == "Fire" && hitInfo.transform.GetComponent<Fire>().GetIsFire())       // 불을 보고 있고 && 불이 켜져있다면
            {
                Slot _selectedSlot = theQuickSlot.GetSelectedSlot();    // 선택된 퀵슬롯 정보를 가져옴
                if(_selectedSlot.item != null)       // 선택된 퀵슬롯에 아이템이 있다면
                {
                    DropAnItem(_selectedSlot);               // 아이템 떨구기
                }
            }
        }
    }

    private void DropAnItem(Slot _selectedSlot)           // 아이템 떨구기 함수
    {
        switch(_selectedSlot.item.itemType)         // 선택된 퀵슬롯의 아이템 타입
        {
            case Item.ItemType.Used:                // 소모품이라면
                if(_selectedSlot.item.itemName.Contains("고기"))      // 아이템이름에 "고기"기 포함되어 있다면, Contains는 문자열 자체의 메소드
                {
                    Instantiate(_selectedSlot.item.itemPrefab, hitInfo.transform.position + Vector3.up, Quaternion.identity);       // 불위에 퀵슬롯에 선택된 아이템 생성
                    theQuickSlot.DecreaseSelectedItem();            // 선택된 퀵슬롯 아이템 소모 함수
                }
                break;
            case Item.ItemType.Ingredient:          // 재료라면
                break;

        }
    }

    private void CheckAction()            // 확인하는 액션 함수
    {
        // 광선을 플레이어에 위치에서 플레이어가 바라보는 방향으로 쏘아 레이어 마스크가 닿는지 확인
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, layerMask))
        {
            if (hitInfo.transform.tag == "Item")                        // 아이템이면
                ItemInfoAppear();                           // 아이템 정보 활성화
            else if (hitInfo.transform.tag == "WeakAnimal" || hitInfo.transform.tag == "StrongAnimal")      // 동물이면
                MeatInfoAppear();                           // 고기 해체 정보 활성화
            else if (hitInfo.transform.tag == "Fire")       // 불이면
                FireInfoAppear();       //불 조리 액션정보창 활성화
            else if (hitInfo.transform.tag == "Computer")
                ComputerInfoAppear();   // 컴퓨터 액션정보창 활성화
            else if (hitInfo.transform.tag == "ArchemyTable")
                ArchemyInfoAppear();    // 연금술 액션정보창 활성화
            else
                InfoDisappear();        // 액션정보창 비활성화
        }
        else                            // 아이템이 광선에 닿지 않았을때
        {
            InfoDisappear();            // 액션정보창 비활성화
        }
    }

    private void Reset()            // 상태변수 false로 리셋
    {
        pickupActivated = false;
        dissolveActivated = false;
        fireLookActivated = false;
        lookComputer = false;
    }

    private void ItemInfoAppear()           // 아이템 액션정보창 활성화
    {
        Reset();                            // 상태변수 false로 리셋
        pickupActivated = true;                      // 아이템 줍기 가능
        actionText.gameObject.SetActive(true);       // 아이템 정보 활성화
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득" + "<color=yellow>" + "(E)" + "</color>";        // 해당 아이템 이름 획득표시, 텍스트에 색 입힘
    }
    private void MeatInfoAppear()           // 고기해체 액션정보창 활성화
    {
        if(hitInfo.transform.GetComponent<Animal>().isDead)     // 동물이 죽어있을때
        {
            Reset();                                // 상태변수 false로 리셋
            dissolveActivated = true;                      // 고기 해체 가능
            actionText.gameObject.SetActive(true);       // 아이템 정보 활성화
            actionText.text = hitInfo.transform.GetComponent<Animal>().animalName + " 해체하기 " + "<color=yellow>" + "(E)" + "</color>";        // 해당 동물 이름 해체표시, 텍스트에 색 입힘
        }
    }

    private void FireInfoAppear()       // 불 조리 액션정보창 활성화
    {
        Reset();                            // 상태변수 false로 리셋
        fireLookActivated = true;           // 불을 바라보고 있음
        if (hitInfo.transform.GetComponent<Fire>().GetIsFire())      // 불 오브젝트에서 bool형인 isFire값을 받아서 불이 켜져있는지 확인
        {
            actionText.gameObject.SetActive(true);       // 불 조리 정보 활성화
            actionText.text = "선택된 아이템 불에 넣기 " + "<color=yellow>" + "(E)" + "</color>";     // 텍스트에 색 입힘
        }
    }

    private void ComputerInfoAppear()           // 컴퓨터 액션정보창 활성화
    {
        // 컴퓨터 전원이 꺼져있을때
        if(!hitInfo.transform.GetComponent<ComputerKit>().isPowerOn)
        {
            Reset();                            // 상태변수 false로 리셋
            lookComputer = true;                      //  컴퓨터를 바라보고 있을때
            actionText.gameObject.SetActive(true);       // 컴퓨터 전원 정보 활성화
            actionText.text = "컴퓨터 가동 " + "<color=yellow>" + "(E)" + "</color>";        // 해당 아이템 이름 획득표시, 텍스트에 색 입힘
        }
    }
    private void ArchemyInfoAppear()           // 연금 테이블 액션정보창 활성화
    {
        // 연금 테이블 액션정보창이 비활성화일때 
        if(!hitInfo.transform.GetComponent<ArchemyTable>().GetIsOpen())
        {
            Reset();                                     // 상태변수 false로 리셋
            lookArchemyTable = true;                     //  연금 테이블을 바라보고 있을때
            actionText.gameObject.SetActive(true);       // 연금 테이블 정보 활성화
            actionText.text = "연금 테이블 조작 " + "<color=yellow>" + "(E)" + "</color>";        // 해당 아이템 이름 획득표시, 텍스트에 색 입힘
        }
    }


    private void InfoDisappear()            // (아이템 , 고기해체)액션정보창 비활성화
    {
        pickupActivated = false;                    // 아이템 줍기 불가능
        dissolveActivated = false;                  // 고기 해체 불가능
        fireLookActivated = false;                  // 불을 바라보고 있음
        lookComputer = false;                       //  컴퓨터를 바라보지 않을때
        lookArchemyTable = false;                   //  연금 테이블을 바라보지 않을때
        actionText.gameObject.SetActive(false);           // 아이템 정보 비활성화
    }
}
