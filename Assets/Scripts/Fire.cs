using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField]
    private string fireName;             // 불 이름 (난로, 모닥불, 화톳불등)
    [SerializeField]
    private int damage;                  // 불 데미지
    [SerializeField]
    private float damageTime;             // 데미지가 들어갈 딜레이 시간
    private float currentDamageTime;        // 실제 데미지 계산에 필요한 시간

    [SerializeField]
    private float durationTime;             // 불의 지속시간
    private float currentDurationTime;      // 실제 지속시간 계산에 필요한 시간

    [SerializeField]
    private ParticleSystem ps_Flame;        // 파티클 시스템 관리를 위한 변수

    // 상태변수
    private bool isFire = true;             // 불이 켜져있는지

    // 필요한 컴포넌트
    private StatusController thePlayerStatus;       // 플레이어 스테이터스 관리를 위한 변수

    void Start()
    {
        thePlayerStatus = FindObjectOfType<StatusController>();
        currentDurationTime = durationTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(isFire)          // 불이 켜져있으면
        {
            ElapseTime();       // 시간이 지나면서 발생하는 함수 실행
        }
    }

    private void ElapseTime()           // 시간이 지나면서 발생하는 함수
    {
        currentDurationTime -= Time.deltaTime;          // 불 지속시간 1초에 1씩감소

        if (currentDamageTime > 0)                      // 데미지가 활성화되면
            currentDamageTime -= Time.deltaTime;        // 데미지 적용 시간 1초에 1씩 감소

        if(currentDurationTime <= 0)        // 지속시간이 끝나면
        {
            Off();         // 불 끔
        }
    }

    private void Off()      // 불 끄는 함수
    {
        ps_Flame.Stop();
        isFire = false;
    }

    private void OnTriggerStay(Collider other)      // 콜라이더 안에 위치해있으면 발동하는 함수
    {
        if(isFire && other.transform.tag == "Player")       // 불이 켜져있을때 && 플레이어가 닿아있을때
        {
            if(currentDamageTime <= 0)          // 데미지 적용 시간이 되면
            {
                //other.GetComponent<Burn>().StartBurning();  // 닿은 객체의 Burn 컴포넌트를 가져와서 불 붙는 함수 실행
                thePlayerStatus.DecreaseHP(damage);     // 불의 데미지 만큼 플레이어 HP 감소
                currentDamageTime = damageTime;         // 데미지 적용 시간 활성화
            }
        }
    }
    private void OnTriggerExit(Collider other)      // 콜라이더 밖으로 나가면 발동하는 함수
    {
        if (isFire && other.transform.tag == "Player")       // 불이 켜져있을때 && 플레이어가 닿아있을때
        {
                other.GetComponent<Burn>().StartBurning();  // 닿은 객체의 Burn 컴포넌트를 가져와서 불 붙는 함수 실행
        }
    }

    public bool GetIsFire()         // 외부에서 private bool형인 isFire값을 얻기위한 함수 
    {
        return isFire;
    }
}
