using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAnimal : Animal
{
    public void Chase(Vector3 _targetPos)      // 플레이어에게 맞으면 추격하는 함수
    {
        isChasing = true;           // 추격중
        destination = _targetPos;   // 플레이어 위치 설정
        nav.speed = runSpeed;       // 뛰기 속도 대입
        isRunning = true;           // 뛰는중
        anim.SetBool("Running", isRunning);     // 뛰기 애니메이션 실행
        nav.SetDestination(destination);        // 네비매쉬 목적지 설정
    }

    // 부모클래스 함수를 수정해서 사용할때 override 해서 사용
    public override void Damage(int _dmg, Vector3 _targetPos)       // 약한 동물은 데미지를 받으면 도망
    {
        base.Damage(_dmg, _targetPos);
        if (!isDead)                          // 동물이 죽지 않았으면
            Chase(_targetPos);                // 맞으면 플레이어를 추격하는 함수

    }
}
