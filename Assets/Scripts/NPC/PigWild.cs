using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigWild : StrongAnimal
{
    // 부모클래스 함수를 수정해서 사용할때 override 해서 사용
    protected override void Update()
    {
        base.Update();
        if (theViewAngle.View() && !isDead && !isAttacking)      // 시야안에 있으면, 죽지 않았으면, 공격중이 아니면
        {
            StopAllCoroutines();            // 코루틴 중복방지
            StartCoroutine(ChaseTargetCoroutine());     // 플레이어 추격 코루틴 실행
        }
    }
}
