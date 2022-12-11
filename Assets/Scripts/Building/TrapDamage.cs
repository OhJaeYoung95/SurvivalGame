using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private float finishTime;

    private bool isHurt = false;            // ���ظ� �־������� ���� ����(������ �ߺ� ����)
    private bool isActivated = false;       // Ʈ�� �۵� ���� 

    public IEnumerator ActivatedTrapCoroutine()
    {   // Ʈ�� �۵�
        isActivated = true;

        yield return new WaitForSeconds(finishTime);
        isActivated = false;
        isHurt = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isActivated)         // Ʈ���� �۵����϶�
        {
            if(!isHurt)         // ���ظ� ���־�����
            {
                isHurt = true;  // ���ظ� ����

                if(other.transform.name == "Player")        // �÷��̾ �ε����ٸ�
                {       // damage ��ŭ HP ����
                    other.transform.GetComponent<StatusController>().DecreaseHP(damage);
                }
            }
        }
    }
}
