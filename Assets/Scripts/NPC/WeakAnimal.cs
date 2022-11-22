using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAnimal : Animal
{
    public void Run(Vector3 _targetPos)      // 플레이어에게 맞고 뛰어서 도망가는 함수
    {
        // 동물 위치에서 타겟위치(플레이어)를 빼면 반대방향으로 도망치는 좌표가 나온다
        destination = new Vector3(transform.position.x - _targetPos.x, 0, transform.position.z - _targetPos.z).normalized;
        currentTime = runTime;      // 현재 행동 시간 설정
        isWalking = false;
        isRunning = true;
        nav.speed = runSpeed;     // 뛰기 속도 대입
        anim.SetBool("Running", isRunning);     // 뛰기 애니메이션 실행
    }

    // 부모클래스 함수를 수정해서 사용할때 override 해서 사용
    public override void Damage(int _dmg, Vector3 _targetPos)       // 약한 동물은 데미지를 받으면 도망
    {
        base.Damage(_dmg, _targetPos);
        if(!isDead)                         // 동물이 죽지 않았으면
            Run(_targetPos);                // 맞고 뛰어서 플레이어 반대방향으로 도망가도록 설정

    }
}
