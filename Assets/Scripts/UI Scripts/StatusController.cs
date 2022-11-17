using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    // 체력
    [SerializeField]
    private int hp;                 // 최대 체력
    private int currentHp;          // 현재 체력

    // 스테미너
    [SerializeField]
    private int sp;                 // 최대 스테미너
    private int currentSp;          // 현재 스테미너
    [SerializeField]
    private int spIncreaseSpeed;    // 스테미너 회복량
    [SerializeField]
    private int spRechargeTime;             // 스테미너 재회복 딜레이 시간
    public int currentSpRechargeTime;      // 현재 스테미너 재회복 딜레이 시간

    // 스테미너 감소 여부
    private bool spUsed;

    // 방어력
    [SerializeField]
    private int dp;                 // 기본 방어력
    private int currentDp;          // 현재 방어력

    // 배고픔
    [SerializeField]
    private int hungry;             // 배고픔
    private int currentHungry;      // 현재 배고픔
    [SerializeField]
    private int hungryDecreaseTime;                 // 배고픔 감소 시간
    private int currentHungryDecreaseTime;          // 현재 배고픔 감소 시간

    // 목마름
    [SerializeField]
    private int thirsty;            // 목마름
    private int currentThirsty;     // 현재 목마름
    [SerializeField]
    private int thirstyDecreaseTime;                 // 목마름 감소 시간
    private int currentThirstyDecreaseTime;          // 현재 목마름 감소 시간

    // 만족도
    [SerializeField]
    private int satisfy;               // 최대 만족도
    private int currentSatisfy;        // 현재 만족도


    [SerializeField]
    private Image[] images_Gauge;       // 필요한 이미지

    // 이미지 배열 변수에 숫자대신 문자를 넣어서 알기 쉽게 하기 위한 상수
    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;


    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;     // 초기값 초기화
        currentDp = dp;
        currentSp = sp;
        currentHungry = hungry;
        currentThirsty = thirsty;
        currentSatisfy = satisfy;
    }

    // Update is called once per frame
    void Update()
    {
        Hungry();
        Thirsty();
        SPRechargeTime();
        SPRecover();
        GaugeUpdate();
    }

    private void SPRechargeTime()       // SP 자연 치료 딜레이 계산 함수
    {
        if(spUsed)                      // SP 사용중이면
        {
            if (currentSpRechargeTime < spRechargeTime)         
                currentSpRechargeTime++;
            else
                spUsed = false;
        }
    }

    private void SPRecover()            // SP 자연 치료
    {
        if(!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed;
        }
    }

    private void Hungry()               // 배고픔 구현
    {
        if (currentHungry > 0)
        {
            if (currentHungryDecreaseTime <= hungryDecreaseTime)
                currentHungryDecreaseTime++;
            else
            {
                currentHungry--;
                currentHungryDecreaseTime = 0;
            }
        }
        else
            Debug.Log("배고픔 수치가 0이 되었습니다");
    }
    private void Thirsty()              // 목마름 구현
    {
        if (currentThirsty > 0)
        {
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
                currentThirstyDecreaseTime++;
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;
            }
        }
        else
            Debug.Log("목마름 수치가 0이 되었습니다");
    }

    private void GaugeUpdate()          // 게이지 UI 에 수치화
    {
        images_Gauge[HP].fillAmount = (float)currentHp / hp;                // 기본 값에 현재 값을 나누어서 적용
        images_Gauge[SP].fillAmount = (float)currentSp / sp;
        images_Gauge[DP].fillAmount = (float)currentDp / dp;
        images_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        images_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        images_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }

    public void IncreaseHP(int _count)          // HP 증가 함수
    {
        if (currentHp + _count < hp)
            currentHp += _count;
        else
            currentHp = hp;
    }

    public void DecreaseHP(int _count)          // HP 감소 함수 (DP 먼저 감소후) 
    {
        if(currentDp >= 0)                       // 방어력이 있다면
        {
            DecreaseDP(_count);
            return;                         // 뒤에 남은 내용을 거치지 않고 함수를 끝내줌
        }
            currentHp -= _count;

        if (currentHp <= 0)
            Debug.Log("캐릭터의 HP가 0이 되었습니다!!");
    }
    public void IncreaseDP(int _count)          // DP 증가 함수
    {
        if (currentDp + _count < dp)
            currentDp += _count;
        else
            currentDp = dp;
    }   
    public void IncreaseSP(int _count)          // SP 증가 함수
    {
        if (currentSp + _count < sp)
            currentSp += _count;
        else
            currentSp = sp;
    }

    public void DecreaseDP(int _count)          // DP 감소 함수
    {
        currentDp -= _count;

        if (currentDp <= 0)
            Debug.Log("방어력이 0이 되었습니다!!");
    }
    public void IncreaseHungry(int _count)          // 배고픔수치 증가 함수
    {
        if (currentHungry + _count < hungry)
            currentHungry += _count;
        else
            currentHungry = hungry;
    }

    public void DecreaseHungry(int _count)          // 배고픔수치 감소 함수
    {
        if(currentHungry - _count < 0)
            currentHungry = 0;
        else
            currentHungry -= _count;
    }

    public void IncreaseThirsty(int _count)          // 배고픔수치 증가 함수
    {
        if (currentThirsty + _count < thirsty)
            currentThirsty += _count;
        else
            currentThirsty = thirsty;
    }

    public void DecreaseThirsty(int _count)          // 목마름수치 감소 함수
    {
        if(currentThirsty - _count < 0)
            currentThirsty = 0;
        else
            currentThirsty -= _count;
    }

    public void DecreaseStamina(int _count)     // 스테미너 감소 함수
    {
        spUsed = true;
        currentSpRechargeTime = 0;

        if (currentSp - _count > 0)
            currentSp -= _count;
        else
            currentSp = 0;
    }

    public int GetCurrentSP()                   // 외부에서 SP수치 얻어가는 함수
    {
        return currentSp;
    }
}
