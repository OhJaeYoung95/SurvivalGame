using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : MonoBehaviour
{
    // ���º���
    private bool isBurning = false;     // ���� �پ�����

    [SerializeField]
    private int damage;                 // �� ������
    [SerializeField]
    private float damageTime;           // �� ������ ���ӽð�
    private float currentDamageTime;    // �� ������ ����ð�

    [SerializeField]
    private float durationTIme;         // �� ���ӽð�
    private float currentDurationTime;  // �� ���� ����ð�

    [SerializeField]
    private GameObject flame_prefab;    // ���� �پ����� �߻��ϴ� ������
    private GameObject go_tempFlame;    // �׸�

    // �ʿ��� ������Ʈ
    private StatusController thePlayerStatus;   // �÷��̾� ü�°����� ���� ����

    void Start()
    {
        thePlayerStatus = FindObjectOfType<StatusController>();
    }
    public void StartBurning()                  // ���� �Ű� �پ����� �߻��ϴ� �Լ�
    {
        if(!isBurning)      // ���� �Ⱥپ������� �����ǵ���, �ѹ��� �����ǵ���
        {
            // �׸��� ���� �ٴ� �������� ä���ִ� �۾�
            go_tempFlame = Instantiate(flame_prefab, transform.position, Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
            go_tempFlame.transform.SetParent(transform);        // �÷��̾ �θ�ü�� ����
        }
        isBurning = true;                       // ���� ����
        currentDurationTime = durationTIme;     // �� ���ӽð� ����
    }


    // Update is called once per frame
    void Update()
    {
        if(isBurning)       // ���� �پ�����
        {
            ElapseTime();           // �ð��� �����鼭 �߻��ϴ� �Լ�
        }
    }

    private void ElapseTime()       // �ð��� �����鼭 �߻��ϴ� �Լ�
    {
        if(isBurning)       // ���� �پ�����
        {
            currentDurationTime -= Time.deltaTime;      // �� ���ӽð� 1�ʿ� 1������

            if (currentDamageTime > 0)                  // �� �������� Ȱ��ȭ�Ǹ�
                currentDamageTime -= Time.deltaTime;    // �� ������ ���� �ð� 1�ʿ� 1������
            
            if(currentDamageTime <= 0)          // �� ������ ����ð��� ������
            {
                Damage();               // ������ ����
            }

            if(currentDurationTime <= 0)        // �� ���ӽð��� ������
            {
                Off();                   // ���� ��
            }
        }
    }

    private void Damage()           // �� ������ �Դ� �Լ�
    {
        currentDamageTime = damageTime;             // �� ������ ����ð� Ȱ��ȭ
        //GetComponent<StatusController>().DecreaseHP(damage);        // �÷��̾� HP ����
        thePlayerStatus.DecreaseHP(damage);                           // �÷��̾� HP ����
    }

    private void Off()              // ���� ���ִ� �Լ�
    {
        isBurning = false;          // ���� ��
        Destroy(go_tempFlame);      // �� ������ ����
    }
}
