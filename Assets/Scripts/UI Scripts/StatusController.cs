using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    // ü��
    [SerializeField]
    private int hp;                 // �ִ� ü��
    private int currentHp;          // ���� ü��

    // ���׹̳�
    [SerializeField]
    private int sp;                 // �ִ� ���׹̳�
    private int currentSp;          // ���� ���׹̳�
    [SerializeField]
    private int spIncreaseSpeed;    // ���׹̳� ȸ����
    [SerializeField]
    private int spRechargeTime;             // ���׹̳� ��ȸ�� ������ �ð�
    public int currentSpRechargeTime;      // ���� ���׹̳� ��ȸ�� ������ �ð�

    // ���׹̳� ���� ����
    private bool spUsed;

    // ����
    [SerializeField]
    private int dp;                 // �⺻ ����
    private int currentDp;          // ���� ����

    // �����
    [SerializeField]
    private int hungry;             // �����
    private int currentHungry;      // ���� �����
    [SerializeField]
    private int hungryDecreaseTime;                 // ����� ���� �ð�
    private int currentHungryDecreaseTime;          // ���� ����� ���� �ð�

    // �񸶸�
    [SerializeField]
    private int thirsty;            // �񸶸�
    private int currentThirsty;     // ���� �񸶸�
    [SerializeField]
    private int thirstyDecreaseTime;                 // �񸶸� ���� �ð�
    private int currentThirstyDecreaseTime;          // ���� �񸶸� ���� �ð�

    // ������
    [SerializeField]
    private int satisfy;               // �ִ� ������
    private int currentSatisfy;        // ���� ������


    [SerializeField]
    private Image[] images_Gauge;       // �ʿ��� �̹���

    // �̹��� �迭 ������ ���ڴ�� ���ڸ� �־ �˱� ���� �ϱ� ���� ���
    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;


    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;     // �ʱⰪ �ʱ�ȭ
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

    private void SPRechargeTime()       // SP �ڿ� ġ�� ������ ��� �Լ�
    {
        if(spUsed)                      // SP ������̸�
        {
            if (currentSpRechargeTime < spRechargeTime)         
                currentSpRechargeTime++;
            else
                spUsed = false;
        }
    }

    private void SPRecover()            // SP �ڿ� ġ��
    {
        if(!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed;
        }
    }

    private void Hungry()               // ����� ����
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
            Debug.Log("����� ��ġ�� 0�� �Ǿ����ϴ�");
    }
    private void Thirsty()              // �񸶸� ����
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
            Debug.Log("�񸶸� ��ġ�� 0�� �Ǿ����ϴ�");
    }

    private void GaugeUpdate()          // ������ UI �� ��ġȭ
    {
        images_Gauge[HP].fillAmount = (float)currentHp / hp;                // �⺻ ���� ���� ���� ����� ����
        images_Gauge[SP].fillAmount = (float)currentSp / sp;
        images_Gauge[DP].fillAmount = (float)currentDp / dp;
        images_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        images_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        images_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }

    public void IncreaseHP(int _count)          // HP ���� �Լ�
    {
        if (currentHp + _count < hp)
            currentHp += _count;
        else
            currentHp = hp;
    }

    public void DecreaseHP(int _count)          // HP ���� �Լ� (DP ���� ������) 
    {
        if(currentDp >= 0)                       // ������ �ִٸ�
        {
            DecreaseDP(_count);
            return;                         // �ڿ� ���� ������ ��ġ�� �ʰ� �Լ��� ������
        }
            currentHp -= _count;

        if (currentHp <= 0)
            Debug.Log("ĳ������ HP�� 0�� �Ǿ����ϴ�!!");
    }
    public void IncreaseDP(int _count)          // DP ���� �Լ�
    {
        if (currentDp + _count < dp)
            currentDp += _count;
        else
            currentDp = dp;
    }   
    public void IncreaseSP(int _count)          // SP ���� �Լ�
    {
        if (currentSp + _count < sp)
            currentSp += _count;
        else
            currentSp = sp;
    }

    public void DecreaseDP(int _count)          // DP ���� �Լ�
    {
        currentDp -= _count;

        if (currentDp <= 0)
            Debug.Log("������ 0�� �Ǿ����ϴ�!!");
    }
    public void IncreaseHungry(int _count)          // ����ļ�ġ ���� �Լ�
    {
        if (currentHungry + _count < hungry)
            currentHungry += _count;
        else
            currentHungry = hungry;
    }

    public void DecreaseHungry(int _count)          // ����ļ�ġ ���� �Լ�
    {
        if(currentHungry - _count < 0)
            currentHungry = 0;
        else
            currentHungry -= _count;
    }

    public void IncreaseThirsty(int _count)          // ����ļ�ġ ���� �Լ�
    {
        if (currentThirsty + _count < thirsty)
            currentThirsty += _count;
        else
            currentThirsty = thirsty;
    }

    public void DecreaseThirsty(int _count)          // �񸶸���ġ ���� �Լ�
    {
        if(currentThirsty - _count < 0)
            currentThirsty = 0;
        else
            currentThirsty -= _count;
    }

    public void DecreaseStamina(int _count)     // ���׹̳� ���� �Լ�
    {
        spUsed = true;
        currentSpRechargeTime = 0;

        if (currentSp - _count > 0)
            currentSp -= _count;
        else
            currentSp = 0;
    }

    public int GetCurrentSP()                   // �ܺο��� SP��ġ ���� �Լ�
    {
        return currentSp;
    }
}
