using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField]
    private string animalName;          // ���� �̸�
    [SerializeField]
    private int hp;                     // ���� ü��

    [SerializeField]
    private float walkSpeed;            // �ȱ� ���ǵ�
    [SerializeField]
    private float runSpeed;             // �ٱ� ���ǵ�
    private float applySpeed;           // ���� ���� ���ǵ�

    private Vector3 direction;          // ����

    // ���º���
    private bool isAction;              // �ൿ������ �ƴ���
    private bool isWalking;             // �ȴ��� �Ȱȴ���
    private bool isRunning;             // �ٴ���
    private bool isDead;                // �׾�����

    [SerializeField]
    private float walkTime;             // �ȱ� �ð�
    [SerializeField]
    private float waitTime;             // ��� �ð�
    [SerializeField]
    private float runTime;              // �ٱ� �ð�
    private float currentTime;          // ��� ���ð�

    // �ʿ��� ������Ʈ
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Rigidbody rigid;
    [SerializeField]
    private BoxCollider boxCol;
    private AudioSource theAudio;

    [SerializeField]
    private AudioClip[] sound_pig_Normal;   // ��� ����
    [SerializeField]
    private AudioClip sound_pig_Hurt;       // ��ĥ�� ����
    [SerializeField]
    private AudioClip sound_pig_Dead;       // ������ ����


    // Start is called before the first frame update
    void Start()
    {
        theAudio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDead)             // ������ ���� �ʾ�����
        {
            Move();
            Rotation();
            ElapseTime();
        }
    }

    private void Move()         // ������ ���� �Լ�
    {
        if (isWalking || isRunning)      // �ȱ����̸� 1�ʿ� walkSpeed ��ŭ ������ ���ư���
            rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
    }

    private void Rotation()     // ���⼳�� �Լ�
    {
        if(isWalking || isRunning)       // ��������
        {
            // ���� ȸ�������� ���ϴ� ���� ȸ�������� �ڿ������� ���� �����
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotation));    // Vector3 => Quaternion���� ����, ���� ȸ���� ����κ�
        }
    }

    private void ElapseTime()
    {
        if(isAction)            // �ൿ���϶���
        {
            currentTime -= Time.deltaTime;          // 1�ʿ� 1�� ����
            if (currentTime <= 0)        // ���� �ൿ�ð��� ������       
                ReSet();        // ���� ������ ���� ���� �ൿ
        }
    }

    private void ReSet()            // ���� �ൿ�� ���� ���Ǹ����Լ�
    {
        isWalking = false;
        isRunning = false;
        isAction = true;
        applySpeed = walkSpeed;                 // �⺻ �ȱ� �ӵ��� �ʱ�ȭ
        anim.SetBool("Walking", isWalking);     // �ȱ� �ִϸ��̼� ���
        anim.SetBool("Running", isRunning);     // �ٱ� �ִϸ��̼� ���
        direction.Set(0f, Random.Range(0f, 360f), 0f);      // �̵����� ����
        RandomAction();             // ���� �ൿ
    }

    private void RandomAction()             // ���� ���� �ൿ
    {
        RandomSound();      // �ϻ� ���� ȿ���� ��� �Լ�����

        // Random.Range(x,y) int�� ��� x�� ���� y�� �������� �ʴ� ������ ���� �ش�.
        int _random = Random.Range(0, 4);       // ���, Ǯ���, �θ���, �ȱ�

        if (_random == 0)
            Wait();
        else if (_random == 1)
            Eat();
        else if (_random == 2)
            Peek();
        else if (_random == 3)
            TryWalk();
    }

    private void Wait()         // ��� �Լ�
    {
        currentTime = waitTime;     // ���� �ൿ �ð� ����
        Debug.Log("���");
    }
    private void Eat()          // Ǯ ��� �Լ�
    {
        currentTime = waitTime;     // ���� �ൿ �ð� ����
        anim.SetTrigger("Eat");     // Ǯ��� �ִϸ��̼� ����
        Debug.Log("Ǯ ���");
    }
    private void Peek()         // �θ��� �Ÿ��� �Լ�
    {
        currentTime = waitTime;     // ���� �ൿ �ð� ����
        anim.SetTrigger("Peek");    // �θ��� �ִϸ��̼� ����
        Debug.Log("�θ���");
    }
    private void TryWalk()      // �ȱ� �õ� �Լ�
    {
        isWalking = true;
        currentTime = walkTime;     // ���� �ൿ �ð� ����
        anim.SetBool("Walking", isWalking);     // �ȱ� �ִϸ��̼� ����
        applySpeed = walkSpeed;     // �ȱ� �ӵ� ����
        Debug.Log("�ȱ�");
    }
    private void Run(Vector3 _targetPos)      // �÷��̾�� �°� �پ �������� �Լ�
    {
        // ���� ��ġ���� Ÿ����ġ(�÷��̾�)�� ���� �ݴ�������� ����ġ�� ������ ���´�
        direction = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles;

        currentTime = runTime;      // ���� �ൿ �ð� ����
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed;     // �ٱ� �ӵ� ����
        anim.SetBool("Running", isRunning);     // �ٱ� �ִϸ��̼� ����
    }

    public void Damage(int _dmg, Vector3 _targetPos)        // ������ �������� �Դ� �Լ�
    {
        if(!isDead)
        {
            hp -= _dmg;                     // HP �������ŭ ����

            if (hp <= 0)                     //  HP �� 0, ������
            {
                Dead();                     // ���� ���
                return;
            }

            PlaySE(sound_pig_Hurt);         // �������� ȿ���� ���
            anim.SetTrigger("Hurt");        // �������� �ִϸ��̼� ����
            Run(_targetPos);                // �°� �پ �÷��̾� �ݴ�������� ���������� ����
        }
    }

    private void Dead()                  // ���� ��� �Լ�
    {
        PlaySE(sound_pig_Dead);         // �׾����� ȿ���� ���
        // �׾������� ���� (������X, ����ִϸ��̼�)
        isWalking = false;
        isRunning = false;
        isDead = true;
        anim.SetTrigger("Dead");
    }

    private void RandomSound()                  // �ϻ� ȿ���� ������� �Լ�
    {
        int _random = Random.Range(0, 3);       // �ϻ� ȿ���� 3�� 0,1,2
        PlaySE(sound_pig_Normal[_random]);      // �ϻ� ȿ���� �������
    }

    private void PlaySE(AudioClip _clip)        //  ȿ���� ��� �Լ�
    {
        theAudio.clip = _clip;                  // �ش� Ŭ�� ����
        theAudio.Play();                        // �ش� Ŭ�� ���
    }
}
