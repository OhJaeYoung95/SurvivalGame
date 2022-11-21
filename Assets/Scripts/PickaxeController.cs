using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
    // 활성화 여부
    public static bool isActivate = true;

    private void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }

    void Update()
    {
        if (isActivate)
            TryAttack();
    }

    // 공격 코루틴
    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                if (hitInfo.transform.tag == "Rock")                                          // 바위일시
                    hitInfo.transform.GetComponent<Rock>().Mining();                          // 채광
                else if (hitInfo.transform.tag == "Twig")                                     // 나뭇가지일시
                    hitInfo.transform.GetComponent<Twig>().Damage(this.transform);            // 대미지
                else if (hitInfo.transform.tag == "NPC")                                      // NPC일시
                {
                    SoundManager.instance.PlaySE("Animal_Hit");     // 동물 타격 효과음 재생
                    hitInfo.transform.GetComponent<Pig>().Damage(1, transform.position);      // 대미지
                }
                isSwing = false;        // 프레임 마다 호출하기에 false 값을 넣어 위 코드를 한번만 실행하도록 설정
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
