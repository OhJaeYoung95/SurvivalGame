using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAnimal : Animal
{
    [SerializeField]
    protected int attackDamage;               // 공격 데미지
    [SerializeField]
    protected float attackDelay;                // 공격 딜레이
    [SerializeField]
    protected LayerMask targetMask;             // 타켓 마스크(플레이어)

    [SerializeField]
    protected float ChaseTime;                  // 총 추격 시간
    protected float currentChaseTime;           // 추격 계산시간
    [SerializeField]
    protected float ChaseDelayTime;             // 추격 딜레이 시간

    public void Chase(Vector3 _targetPos)       // 플레이어에게 맞으면 추격하는 함수
    {
        isChasing = true;                       // 추격중
        destination = _targetPos;               // 플레이어 위치 설정
        nav.speed = runSpeed;                   // 뛰기 속도 대입
        isRunning = true;                       // 뛰는중
        anim.SetBool("Running", isRunning);     // 뛰기 애니메이션 실행
        nav.SetDestination(destination);        // 네비매쉬 목적지 설정, 이동
    }

    // 부모클래스 함수를 수정해서 사용할때 override 해서 사용
    public override void Damage(int _dmg, Vector3 _targetPos)       // 약한 동물은 데미지를 받으면 도망
    {
        base.Damage(_dmg, _targetPos);
        if (!isDead)                          // 동물이 죽지 않았으면
            Chase(_targetPos);                // 맞으면 플레이어를 추격하는 함수

    }

    protected IEnumerator ChaseTargetCoroutine()      // 플레이어 추격 코루틴
    {
        currentChaseTime = 0;               // 추격 시간 초기화

        while (currentChaseTime < ChaseTime)
        {
            Chase(theViewAngle.GetTargetPos());                     // 플레이어 추격
            if (Vector3.Distance(transform.position, theViewAngle.GetTargetPos()) <= 3f)     // 난폭한 돼지와 타겟(플레이어)간의 거리가 3보다 작을시, 근접해 있고
            {
                if (theViewAngle.View())     // 눈 앞에 있을 경우
                {
                    Debug.Log("플레이어 공격 시도");
                    StartCoroutine(AttackCoroutine());      // 난폭한 동물 공격 시작
                }
            }

            yield return new WaitForSeconds(ChaseDelayTime);        // 추격 딜레이 시간 대기
            currentChaseTime += ChaseDelayTime;                     // 점점 추격중인 시간이 쌓이다가
                                                                    // 총 추격시간을 벗어나서 코루틴에서 벗아남
        }
        isChasing = false;
        isRunning = false;
        anim.SetBool("Running", isRunning);     // 달리기 멈추기
        nav.ResetPath();        // 목적지 경로 초기화, 추격 포기
    }

    protected IEnumerator AttackCoroutine()   // 난폭한 동물 공격 코루틴
    {
        isAttacking = true;     // 공격중
        nav.ResetPath();        // 움직임정지
        currentChaseTime = ChaseTime;       // 추격 중지하도록 설정

        yield return new WaitForSeconds(0.5f);
        transform.LookAt(theViewAngle.GetTargetPos());      // 타겟(플레이어)를 바라본다
        anim.SetTrigger("Attack");                          // 공격 애니메이션 실행
        yield return new WaitForSeconds(0.5f);
        RaycastHit _hit;
        // Physics.Raycast(광선발사 위치, 광선발사 방향, 광선에 부딪힌 물체정보, 광선사거리, 광선에 부딪힐 레이어
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out _hit, 3, targetMask))
        {
            Debug.Log("플레이어 적중!!");
            thePlayerStatus.DecreaseHP(attackDamage);
        }
        else
        {
            Debug.Log("플레이어 빗나감!!");
        }
        yield return new WaitForSeconds(attackDelay);       // 공격 딜레이 시간동안 대기
        isAttacking = false;                                // 공격 종료
        StartCoroutine(ChaseTargetCoroutine());             // 추격 시작
    }

}
