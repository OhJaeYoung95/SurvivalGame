using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTrap : MonoBehaviour
{
    private Animator anim;                  // �ִϸ��̼� ���
    private AudioSource theAudio;           // ȿ���� ���

    private bool isActivated = false;       // Ʈ�� �۵� ����(�ߺ� ����)

    [SerializeField]
    private AudioClip sound_Activate;       // Ʈ�� �ߵ� ȿ����
    [SerializeField]
    private TrapDamage theTrapDamage;       // Ʈ�� �������� �����ֱ� ���� ������Ʈ

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        theAudio = GetComponent<AudioSource>();
    }

    public bool GetIsActivated()        // �ܺο� Ʈ�� �۵� ���� ��ȯ���ִ� �Լ�
    {
        return isActivated;
    }

    public void ReInstall()     // Ʈ�� �缳ġ �Լ�
    {
        isActivated = false;
        anim.SetTrigger("DeActivate");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isActivated)        // Ʈ���� �۵��� ���� �ʾ�����
        {       // �����ȿ� ���� ������Ʈ�� �±װ� ������ && ���� �±װ� �ƴҶ�
            if(other.transform.tag != "Untagged" && other.transform.tag != "Trap")
            {   // Ʈ�� �۵�, Ʈ�� ������ ����, ���ϸ��̼� ȿ���� ���
                StartCoroutine(theTrapDamage.ActivatedTrapCoroutine());
                isActivated = true;
                anim.SetTrigger("Activate");
                theAudio.clip = sound_Activate;
                theAudio.Play();
            }
        }
    }
}
