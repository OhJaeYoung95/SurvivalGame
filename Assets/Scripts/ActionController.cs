using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range;    // 습득 가능한 최대 거리

    private bool pickupActivated = false;   // 습득 가능할 시 true
    private bool dissolveActivated = false;     // 고기 해체 가능할 시 true
    private bool isDissolving = false;          // 고기 해체 중에는 true

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
    private Transform tf_MeatDissolveTool;      // 고기 해체 툴

    [SerializeField]
    private string sound_meat;              // 고기 해체 효과음 재생


    // Update is called once per frame
    void Update()
    {
        CheckAction();
        TryAction();
    }

    private void TryAction()            // 아이템 습득
    {
        if(Input.GetKeyDown(KeyCode.E))     // E키를 누른다면
        {
            CheckAction();
            CanPickUp();
            CanMeat();
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

    private void CanMeat()
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

    private void CheckAction()            // 확인하는 액션 함수
    {
        // 광선을 플레이어에 위치에서 플레이어가 바라보는 방향으로 쏘아 레이어 마스크가 닿는지 확인
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, layerMask))
        {
            if (hitInfo.transform.tag == "Item")                        // 아이템이면
                ItemInfoAppear();                           // 아이템 정보 활성화
            else if (hitInfo.transform.tag == "WeakAnimal" || hitInfo.transform.tag == "StrongAnimal")      // 동물이면
                MeatInfoAppear();                           // 고기 해체 정보 활성화
            else
                InfoDisappear();        // 액션정보창 비활성화
        }
        else                            // 아이템이 광선에 닿지 않았을때
        {
            InfoDisappear();            // 액션정보창 비활성화
        }
    }

    private void ItemInfoAppear()           // 아이템 액션정보창 활성화
    {
        pickupActivated = true;                      // 아이템 줍기 가능
        actionText.gameObject.SetActive(true);       // 아이템 정보 활성화
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득" + "<color=yellow>" + "(E)" + "</color>";        // 해당 아이템 이름 획득표시, 텍스트에 색 입힘
    }
    private void MeatInfoAppear()           // 고기해체 액션정보창 활성화
    {
        if(hitInfo.transform.GetComponent<Animal>().isDead)     // 동물이 죽어있을때
        {
            dissolveActivated = true;                      // 고기 해체 가능
            actionText.gameObject.SetActive(true);       // 아이템 정보 활성화
            actionText.text = hitInfo.transform.GetComponent<Animal>().animalName + " 해체하기 " + "<color=yellow>" + "(E)" + "</color>";        // 해당 동물 이름 해체표시, 텍스트에 색 입힘
        }
    }

    private void InfoDisappear()            // (아이템 , 고기해체)액션정보창 비활성화
    {
        pickupActivated = false;                          // 아이템 줍기 불가능
        dissolveActivated = false;                        // 고기 해체 불가능
        actionText.gameObject.SetActive(false);           // 아이템 정보 비활성화
    }
}
