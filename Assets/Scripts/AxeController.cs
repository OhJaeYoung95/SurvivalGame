using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CloseWeaponController 스크립트를 상속받는다
public class AxeController : CloseWeaponController
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
                if(hitInfo.transform.tag == "Grass")                        // 타격 되는것의 태그가 Grass이면
                {
                    hitInfo.transform.GetComponent<Grass>().Damage();       // Grass에 대미지를 입히는 함수 실행
                }                
                else if(hitInfo.transform.tag == "Tree")                        // 타격 되는것의 태그가 Tree이면
                {
                    hitInfo.transform.GetComponent<TreeComponent>()
                        .Chop(hitInfo.point, transform.eulerAngles.y);       // Tree를 팰때의 함수 실행, hitInfo의 부딪힌 곳과 eulerAngles.y 값을 인자로 받음
                }
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
