using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CloseWeaponController 스크립트를 상속받는다
public class HandController : CloseWeaponController
{
    // 활성화 여부
    public static bool isActivate = false;

    [SerializeField]
    private QuickSlotController theQuickSlot;   // 퀵슬롯 관련 정보 가져오기 위한 변수

    void Update()
    {
            if (isActivate && !Inventory.inventoryActivated) // 손 활성화 & 인벤토리 비활성화시
            {
                if (QuickSlotController.go_HandItem == null)        // 손에 든 아이템이 없다면
                    TryAttack();
                else                                                // 있다면
                    TryEating();
        }
    }

    private void TryEating()        // 소모품 사용(먹기)
    {
        if(Input.GetButtonDown("Fire1") && !theQuickSlot.GetIsCoolTime())        // 마우스 좌클릭시
        {
            currentCloseWeapon.anim.SetTrigger("Eat");      // 아이템 먹는 애니메이션 실행
            theQuickSlot.EatItem();             // 아이템 먹기
        }
    }

    // 공격 코루틴
    protected override IEnumerator HitCoroutine()
    {
        while(isSwing)
        {
            if(CheckObject())
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    // 근접 무기 변화 함수 덮어쓰기(부모 클래스에 있는 함수에서 자식 함수에 추가할 거 작성)
    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }

}
