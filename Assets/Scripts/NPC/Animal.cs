using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    protected StatusController thePlayerStatus;     // �÷��̾� �������ͽ� ����



    [SerializeField]
    public string animalName;          // ���� �̸�
    [SerializeField]
    protected int hp;                     // ���� ü��

    [SerializeField]
    protected Item item_Prefab;           // ���� ������
    [SerializeField]
    public int itemNumber;            // ���� ������ ȹ�� ����


    [SerializeField]
    protected float walkSpeed;            // �ȱ� ���ǵ�
    [SerializeField]
    protected float runSpeed;             // �ٱ� ���ǵ�

    protected Vector3 destination;          // ������

    // ���º���
    protected bool isAction;              // �ൿ������ �ƴ���
    protected bool isWalking;             // �ȴ��� �Ȱȴ���
    protected bool isRunning;             // �ٴ���
    protected bool isChasing;             // �߰�������
    protected bool isAttacking;           // ����������
    public bool isDead;                // �׾�����

    [SerializeField]
    protected float walkTime;             // �ȱ� �ð�
    [SerializeField]
    protected float waitTime;             // ��� �ð�
    [SerializeField]
    protected float runTime;              // �ٱ� �ð�
    protected float currentTime;          // ��� ���ð�

    // �ʿ��� ������Ʈ
    [SerializeField]
    protected Animator anim;
    [SerializeField]
    protected Rigidbody rigid;
    [SerializeField]
    protected BoxCollider boxCol;
    protected AudioSource theAudio;
    protected NavMeshAgent nav;
    protected FieldOfViewAngle theViewAngle;

    [SerializeField]
    protected AudioClip[] sound_Normal;   // ��� ����
    [SerializeField]
    protected AudioClip sound_Hurt;       // ��ĥ�� ����
    [SerializeField]
    protected AudioClip sound_Dead;       // ������ ����



    // Start is called before the first frame update
    void Start()
    {
        thePlayerStatus = FindObjectOfType<StatusController>();
        theViewAngle = GetComponent<FieldOfViewAngle>();
        nav = GetComponent<NavMeshAgent>();
        theAudio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!isDead)             // ������ ���� �ʾ�����
        {
            Move();
            ElapseTime();
        }
    }

    protected void Move()         // ������ ���� �Լ�
    {
        if (isWalking || isRunning)
        {
            // �ȱ����̸� 1�ʿ� walkSpeed ��ŭ ������ ���ư���
            //rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
            // �׺�޽��� �����峻���� �����Ŵ �׷��� ���� ����
            // *5�� �� ������ destination������ ����ȭ ���� ���� 1�̳��ͼ� �ָ� ���� �ʱ⿡
            nav.SetDestination(transform.position + destination * 5f);      // ������ ���� �̵���Ŵ
        }
    }

    protected void ElapseTime()
    {
        if (isAction)            // �ൿ���϶���
        {
            currentTime -= Time.deltaTime;          // 1�ʿ� 1�� ����
            if (currentTime <= 0 && !isChasing && !isAttacking)        // ���� �ൿ�ð��� ������, �߰����� �ƴϸ�, �������� �ƴϸ� 
                ReSet();        // ���� ������ ���� ���� �ൿ
        }
    }

    protected virtual void ReSet()            // ���� �ൿ�� ���� ���Ǹ����Լ�
    {
        isWalking = false;
        isRunning = false;
        isAction = true;
        nav.speed = walkSpeed;                 // �⺻ �ȱ� �ӵ��� �ʱ�ȭ
        nav.ResetPath();                        // ������ �ʱ�ȭ
        anim.SetBool("Walking", isWalking);     // �ȱ� �ִϸ��̼� ���
        anim.SetBool("Running", isRunning);     // �ٱ� �ִϸ��̼� ���
        destination.Set(Random.Range(-0.2f, 0.2f), 0f, Random.Range(0.5f, 1f));      // �̵� ������ ����
    }


    protected void TryWalk()      // �ȱ� �õ� �Լ�
    {
        isWalking = true;
        currentTime = walkTime;     // ���� �ൿ �ð� ����
        anim.SetBool("Walking", isWalking);     // �ȱ� �ִϸ��̼� ����
        nav.speed = walkSpeed;     // �ȱ� �ӵ� ����
        Debug.Log("�ȱ�");
    }

    // �ڽ�Ŭ�������� �����Ҷ� virtual�� ����
    public virtual void Damage(int _dmg, Vector3 _targetPos)        // ������ �������� �Դ� �Լ�
    {
        if (!isDead)
        {
            hp -= _dmg;                     // HP �������ŭ ����

            if (hp <= 0)                     //  HP �� 0, ������
            {
                Dead();                     // ���� ���
                return;
            }

            PlaySE(sound_Hurt);         // �������� ȿ���� ���
            anim.SetTrigger("Hurt");        // �������� �ִϸ��̼� ����
        }
    }

    protected void Dead()                  // ���� ��� �Լ�
    {
        PlaySE(sound_Dead);         // �׾����� ȿ���� ���
        // �׾������� ���� (������X, ����ִϸ��̼�)
        isWalking = false;
        isRunning = false;
        isChasing = false;
        isAttacking = false;
        isDead = true;
        nav.ResetPath();        // ������ ����, �ʱ�ȭ
        anim.SetTrigger("Dead");
    }

    protected void RandomSound()                  // �ϻ� ȿ���� ������� �Լ�
    {
        int _random = Random.Range(0, 3);       // �ϻ� ȿ���� 3�� 0,1,2
        PlaySE(sound_Normal[_random]);      // �ϻ� ȿ���� �������
    }

    protected void PlaySE(AudioClip _clip)        //  ȿ���� ��� �Լ�
    {
        theAudio.clip = _clip;                  // �ش� Ŭ�� ����
        theAudio.Play();                        // �ش� Ŭ�� ���
    }

    public Item GetItem()                   // ������ ȹ�� �Լ�
    {
        this.gameObject.tag = "Untagged";
        Destroy(this.gameObject, 3f);
        return item_Prefab;
    }
}
