using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrap : MonoBehaviour
{
    private Rigidbody[] rigid;      // Ʈ���� �������� �ϱ� ���� �ڽİ�ü�� Rigidbody
    [SerializeField] private GameObject go_Meat;        // Ʈ���� �̳�(���)

    [SerializeField]
    private int damage;         // ������

    private bool isActivated = false;       // Ʈ�� �ߵ� ����

    private AudioSource theAudio;           // ȿ���� ����� ���� ������Ʈ
    [SerializeField]
    private AudioClip sound_Activate;       // Ʈ�� �ߵ� ȿ����

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponentsInChildren<Rigidbody>();
        theAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {   // �ݶ��̴� �ȿ� ������
        if(!isActivated)    // Ʈ���� �۵������� ������
        {
            if(other.transform.tag != "Untagged")   // �浹�� ������Ʈ�� �±װ� �ִٸ�
            {
                isActivated = true;     // Ʈ�� �ߵ�
                // Ʈ�� �ߵ� ȿ���� ���
                theAudio.clip = sound_Activate;
                theAudio.Play();

                Destroy(go_Meat);       // �̳�(���) ����

                for (int i = 0; i < rigid.Length; i++)
                {   // �߷� ����(Ʈ�� �ߵ�)
                    rigid[i].useGravity = true;
                    rigid[i].isKinematic = false;
                }

                if(other.transform.name == "Player")    // �÷��̾ �ε����ٸ�
                {       // ������ ��ŭ �÷��̾� ü�� ����
                    other.transform.GetComponent<StatusController>().DecreaseHP(damage);
                }
            }
        }
    }
}
