using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : MonoBehaviour
{
    // 상태변수
    private bool isBurning = false;     // 불이 붙었는지

    [SerializeField]
    private int damage;                 // 불 데미지
    [SerializeField]
    private float damageTime;           // 불 데미지 지속시간
    private float currentDamageTime;    // 불 데미지 적용시간

    [SerializeField]
    private float durationTIme;         // 불 지속시간
    private float currentDurationTime;  // 불 지속 적용시간

    [SerializeField]
    private GameObject flame_prefab;    // 불이 붙었을때 발생하는 프리팹
    private GameObject go_tempFlame;    // 그릇

    // 필요한 컴포넌트
    private StatusController thePlayerStatus;   // 플레이어 체력관리를 위한 변수

    void Start()
    {
        thePlayerStatus = FindObjectOfType<StatusController>();
    }
    public void StartBurning()                  // 불이 옮겨 붙었을때 발생하는 함수
    {
        if(!isBurning)      // 불이 안붙었을때만 생성되도록, 한번만 생성되도록
        {
            // 그릇에 불이 붙는 프리팹을 채워주는 작업
            go_tempFlame = Instantiate(flame_prefab, transform.position, Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
            go_tempFlame.transform.SetParent(transform);        // 플레이어를 부모객체로 설정
        }
        isBurning = true;                       // 불이 붙음
        currentDurationTime = durationTIme;     // 불 지속시간 적용
    }


    // Update is called once per frame
    void Update()
    {
        if(isBurning)       // 불이 붙었을때
        {
            ElapseTime();           // 시간이 지나면서 발생하는 함수
        }
    }

    private void ElapseTime()       // 시간이 지나면서 발생하는 함수
    {
        if(isBurning)       // 불이 붙었을때
        {
            currentDurationTime -= Time.deltaTime;      // 불 지속시간 1초에 1씩감소

            if (currentDamageTime > 0)                  // 불 데미지가 활성화되면
                currentDamageTime -= Time.deltaTime;    // 불 데미지 적용 시간 1초에 1씩감소
            
            if(currentDamageTime <= 0)          // 불 데미지 적용시간이 끝나면
            {
                Damage();               // 데미지 입힘
            }

            if(currentDurationTime <= 0)        // 불 지속시간이 끝나면
            {
                Off();                   // 불을 끔
            }
        }
    }

    private void Damage()           // 불 데미지 입는 함수
    {
        currentDamageTime = damageTime;             // 불 데미지 적용시간 활성화
        //GetComponent<StatusController>().DecreaseHP(damage);        // 플레이어 HP 감소
        thePlayerStatus.DecreaseHP(damage);                           // 플레이어 HP 감소
    }

    private void Off()              // 불을 꺼주는 함수
    {
        isBurning = false;          // 불을 끔
        Destroy(go_tempFlame);      // 불 붙은거 삭제
    }
}
