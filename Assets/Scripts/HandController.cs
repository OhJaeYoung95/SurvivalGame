using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{

    // 현재 장착된 Hand형 타입
    [SerializeField]
    private Hand currentHand;

    // 공격중
    private bool isAttack = false;  // 공격중인지 체크하는 변수
    private bool isSwing = false;   // 팔을 휘두르는 중인지 체크하는 변수

    private RaycastHit hitInfo;     // 광선을 쏘았을때 닿은 녀석의 정보를 얻어올 수 있는 변수

    // Update is called once per frame
    void Update()
    {
        TryAttack();
    }

    private void TryAttack()    // 공격 시도하는 함수
    {
        if(Input.GetButton("Fire1"))
        {
            if(!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()   // 공격 코루틴
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentHand.attackDelayA);  // 딜레이 이후에 공격활성화 시점
        isSwing = true;
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentHand.attackDelayB);  // 딜레이 이후에 공격비활성화 시점
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB); // 공격후 총 딜레이 시점
        isAttack = false;
    }

    IEnumerator HitCoroutine()  // 공격 코루틴 내부 타격구현 코루틴 함수
    {
        while(isSwing)
        {
            if(CheckObject())   // 공격범위에 있다면
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;  // 1프레임 대기
        }
    }

    private bool CheckObject()  // 공격이 닿았는지 확인해주는 Bool형 함수
    {
        // 캐릭터에서 광선을 쏘아 범위안에 물체가 있다면 닿은 물체의 정보를 불러오게 해준다.
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            return true; // 물체가 닿으면 True를 반환한다.
        }
        return false;   // 감지되는 물체가 없으면 False를 반환한다.
    }
}
