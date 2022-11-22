using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : WeakAnimal
{
    // 부모클래스 함수를 수정해서 사용할때 override 해서 사용
    protected override void ReSet()
    {
        base.ReSet();
        RandomAction();             // 돼지 랜덤행동
    }
    private void RandomAction()             // 다음 랜덤 행동
    {
        RandomSound();      // 일상 랜덤 효과음 재생 함수실행

        // Random.Range(x,y) int일 경우 x는 포함 y는 포함하지 않는 랜덤한 수를 준다.
        int _random = Random.Range(0, 4);       // 대기, 풀뜯기, 두리번, 걷기

        if (_random == 0)
            Wait();
        else if (_random == 1)
            Eat();
        else if (_random == 2)
            Peek();
        else if (_random == 3)
            TryWalk();
    }

    private void Wait()         // 대기 함수
    {
        currentTime = waitTime;     // 현재 행동 시간 설정
        Debug.Log("대기");
    }
    private void Eat()          // 풀 뜯기 함수
    {
        currentTime = waitTime;     // 현재 행동 시간 설정
        anim.SetTrigger("Eat");     // 풀뜯기 애니메이션 실행
        Debug.Log("풀 뜯기");
    }
    private void Peek()         // 두리번 거리는 함수
    {
        currentTime = waitTime;     // 현재 행동 시간 설정
        anim.SetTrigger("Peek");    // 두리번 애니메이션 실행
        Debug.Log("두리번");
    }
}
