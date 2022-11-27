using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField]
    private string fireName;             // �� �̸� (����, ��ں�, ȭ��ҵ�)
    [SerializeField]
    private int damage;                  // �� ������
    [SerializeField]
    private float damageTime;             // �������� �� ������ �ð�
    private float currentDamageTime;        // ���� ������ ��꿡 �ʿ��� �ð�

    [SerializeField]
    private float durationTime;             // ���� ���ӽð�
    private float currentDurationTime;      // ���� ���ӽð� ��꿡 �ʿ��� �ð�

    [SerializeField]
    private ParticleSystem ps_Flame;        // ��ƼŬ �ý��� ������ ���� ����

    // ���º���
    private bool isFire = true;             // ���� �����ִ���

    // �ʿ��� ������Ʈ
    private StatusController thePlayerStatus;       // �÷��̾� �������ͽ� ������ ���� ����

    void Start()
    {
        thePlayerStatus = FindObjectOfType<StatusController>();
        currentDurationTime = durationTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(isFire)          // ���� ����������
        {
            ElapseTime();       // �ð��� �����鼭 �߻��ϴ� �Լ� ����
        }
    }

    private void ElapseTime()           // �ð��� �����鼭 �߻��ϴ� �Լ�
    {
        currentDurationTime -= Time.deltaTime;          // �� ���ӽð� 1�ʿ� 1������

        if (currentDamageTime > 0)                      // �������� Ȱ��ȭ�Ǹ�
            currentDamageTime -= Time.deltaTime;        // ������ ���� �ð� 1�ʿ� 1�� ����

        if(currentDurationTime <= 0)        // ���ӽð��� ������
        {
            Off();         // �� ��
        }
    }

    private void Off()      // �� ���� �Լ�
    {
        ps_Flame.Stop();
        isFire = false;
    }

    private void OnTriggerStay(Collider other)      // �ݶ��̴� �ȿ� ��ġ�������� �ߵ��ϴ� �Լ�
    {
        if(isFire && other.transform.tag == "Player")       // ���� ���������� && �÷��̾ ���������
        {
            if(currentDamageTime <= 0)          // ������ ���� �ð��� �Ǹ�
            {
                //other.GetComponent<Burn>().StartBurning();  // ���� ��ü�� Burn ������Ʈ�� �����ͼ� �� �ٴ� �Լ� ����
                thePlayerStatus.DecreaseHP(damage);     // ���� ������ ��ŭ �÷��̾� HP ����
                currentDamageTime = damageTime;         // ������ ���� �ð� Ȱ��ȭ
            }
        }
    }
    private void OnTriggerExit(Collider other)      // �ݶ��̴� ������ ������ �ߵ��ϴ� �Լ�
    {
        if (isFire && other.transform.tag == "Player")       // ���� ���������� && �÷��̾ ���������
        {
                other.GetComponent<Burn>().StartBurning();  // ���� ��ü�� Burn ������Ʈ�� �����ͼ� �� �ٴ� �Լ� ����
        }
    }

    public bool GetIsFire()         // �ܺο��� private bool���� isFire���� ������� �Լ� 
    {
        return isFire;
    }
}
