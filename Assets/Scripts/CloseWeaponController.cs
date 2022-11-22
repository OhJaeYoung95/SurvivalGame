using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 추상 코루틴 함수가 포함된 클래스에도 abstract를 붙여줘야 한다
// 미완성 클래스 = 추상 클래스
// 추상 클래스는 다른 객체에 추가 시킬수 없다, 컴포넌트로 사용x
public abstract class CloseWeaponController : MonoBehaviour
{


    // 현재 장착된 근접무기 타입
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    // 공격중
    protected bool isAttack = false;  // 공격중인지 체크하는 변수
    protected bool isSwing = false;   // 팔을 휘두르는 중인지 체크하는 변수

    protected RaycastHit hitInfo;     // 광선을 쏘았을때 닿은 녀석의 정보를 얻어올 수 있는 변수
    [SerializeField]
    protected LayerMask layerMask;      // 영향 받지 않는 레이어를 선택하기 위한 변수

    // 필요한 컴포넌트
    private PlayerController thePlayerController;       // 카메라 로테이션 값 설정을 위한 컴포넌트

    void Start()
    {
        thePlayerController = FindObjectOfType<PlayerController>();
    }
    protected void TryAttack()    // 공격 시도하는 함수
    {
        if (!Inventory.inventoryActivated)       // 인벤토리 비활성화시에만
        {
            if (Input.GetButton("Fire1"))       // 마우스 좌클릭
            {
                if (!isAttack)
                {
                    if (CheckObject())           // 공격이 닿았는지 확인,
                    {                                                                           //(작업인지 확인)
                        if (currentCloseWeapon.isAxe && hitInfo.transform.tag == "Tree")        // 휘두르는 무기가 도끼이고, 공격이 닿은 물체가 나무인지 확인
                        {
                            //StartCoroutine(thePlayerController.TreeLookCoroutine(hitInfo.transform.position));      // 나무를 칠때 나무를 바라보게 해줌
                            StartCoroutine(thePlayerController.TreeLookCoroutine(hitInfo.transform.GetComponent<TreeComponent>().GetTreeCenterPosition()));      // 나무를 칠때 나무를 바라보게 해줌
                            StartCoroutine(AttackCoroutine("Chop", currentCloseWeapon.workDelayA,         // 맞다면 Chop 애니메이션 실행
                            currentCloseWeapon.workDelayB,
                            currentCloseWeapon.workDelay - currentCloseWeapon.workDelayA - currentCloseWeapon.workDelayB));
                            return;         // 밑에 코루틴이 중복실행될 수 있기에 return으로 함수 종료
                        }
                    }
                    StartCoroutine(AttackCoroutine("Attack", currentCloseWeapon.attackDelayA,   // 공격
                        currentCloseWeapon.attackDelayB,
                        currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB));
                }
            }
        }
    }

    protected IEnumerator AttackCoroutine(string swingType, float _delayA, float _delayB, float _delayC)   // 공격(swing)코루틴
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger(swingType);

        yield return new WaitForSeconds(_delayA);  // 딜레이 이후에 공격활성화 시점
        isSwing = true;
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(_delayB);  // 딜레이 이후에 공격비활성화 시점
        isSwing = false;

        yield return new WaitForSeconds(_delayC - _delayA - _delayB); // 공격후 총 딜레이 시점
        isAttack = false;
    }

    // abstract = 추상 코루틴 = 미완성(자식 클래스에서 완성시켜야함)
    protected abstract IEnumerator HitCoroutine();  // 공격 코루틴 내부 타격구현 코루틴 함수


    protected bool CheckObject()  // 공격이 닿았는지 확인해주는 Bool형 함수
    {
        // 캐릭터에서 광선을 쏘아 범위안에 물체가 있다면 닿은 물체의 정보를 불러오게 해준다.
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range, layerMask))
        {
            return true; // 물체가 닿으면 True를 반환한다.
        }
        return false;   // 감지되는 물체가 없으면 False를 반환한다.
    }

    // 가상 함수 : 완성함수이지만, 추가 편집 가능한 함수
    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)     // 근접무기 변화 함수
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentCloseWeapon = _closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
    }
}
