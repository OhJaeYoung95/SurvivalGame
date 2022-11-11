using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CloseWeaponController 스크립트를 상속받는다
public class HandController : CloseWeaponController
{
    // 활성화 여부
    public static bool isActivate = false;

    void Update()
    {
        if (isActivate)
            TryAttack();
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
