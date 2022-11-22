using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    protected StatusController thePlayerStatus;     // 플레이어 스테이터스 정보



    [SerializeField]
    public string animalName;          // 동물 이름
    [SerializeField]
    protected int hp;                     // 동물 체력

    [SerializeField]
    protected Item item_Prefab;           // 동물 아이템
    [SerializeField]
    public int itemNumber;            // 동물 아이템 획득 수량


    [SerializeField]
    protected float walkSpeed;            // 걷기 스피드
    [SerializeField]
    protected float runSpeed;             // 뛰기 스피드

    protected Vector3 destination;          // 목적지

    // 상태변수
    protected bool isAction;              // 행동중인지 아닌지
    protected bool isWalking;             // 걷는지 안걷는지
    protected bool isRunning;             // 뛰는지
    protected bool isChasing;             // 추격중인지
    protected bool isAttacking;           // 공격중인지
    public bool isDead;                // 죽었는지

    [SerializeField]
    protected float walkTime;             // 걷기 시간
    [SerializeField]
    protected float waitTime;             // 대기 시간
    [SerializeField]
    protected float runTime;              // 뛰기 시간
    protected float currentTime;          // 대기 계산시간

    // 필요한 컴포넌트
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
    protected AudioClip[] sound_Normal;   // 평소 사운드
    [SerializeField]
    protected AudioClip sound_Hurt;       // 다칠때 사운드
    [SerializeField]
    protected AudioClip sound_Dead;       // 죽을때 사운드



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
        if (!isDead)             // 동물이 죽지 않았으면
        {
            Move();
            ElapseTime();
        }
    }

    protected void Move()         // 움직임 설정 함수
    {
        if (isWalking || isRunning)
        {
            // 걷기중이면 1초에 walkSpeed 만큼 앞으로 나아가기
            //rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
            // 네비메쉬가 리지드내용을 동결시킴 그래서 따로 설정
            // *5를 한 이유는 destination변수를 정규화 시켜 값이 1이나와서 멀리 가지 않기에
            nav.SetDestination(transform.position + destination * 5f);      // 목적지 까지 이동시킴
        }
    }

    protected void ElapseTime()
    {
        if (isAction)            // 행동중일때만
        {
            currentTime -= Time.deltaTime;          // 1초에 1씩 감소
            if (currentTime <= 0 && !isChasing && !isAttacking)        // 현재 행동시간이 끝나면, 추격중이 아니면, 공격중이 아니면 
                ReSet();        // 조건 리셋후 다음 랜덤 행동
        }
    }

    protected virtual void ReSet()            // 다음 행동을 위한 조건리셋함수
    {
        isWalking = false;
        isRunning = false;
        isAction = true;
        nav.speed = walkSpeed;                 // 기본 걷기 속도로 초기화
        nav.ResetPath();                        // 목적지 초기화
        anim.SetBool("Walking", isWalking);     // 걷기 애니메이션 취소
        anim.SetBool("Running", isRunning);     // 뛰기 애니메이션 취소
        destination.Set(Random.Range(-0.2f, 0.2f), 0f, Random.Range(0.5f, 1f));      // 이동 목적지 랜덤
    }


    protected void TryWalk()      // 걷기 시도 함수
    {
        isWalking = true;
        currentTime = walkTime;     // 현재 행동 시간 설정
        anim.SetBool("Walking", isWalking);     // 걷기 애니메이션 실행
        nav.speed = walkSpeed;     // 걷기 속도 대입
        Debug.Log("걷기");
    }

    // 자식클래스에서 수정할때 virtual로 설정
    public virtual void Damage(int _dmg, Vector3 _targetPos)        // 동물이 데미지를 입는 함수
    {
        if (!isDead)
        {
            hp -= _dmg;                     // HP 대미지만큼 차감

            if (hp <= 0)                     //  HP 가 0, 죽을때
            {
                Dead();                     // 동물 사망
                return;
            }

            PlaySE(sound_Hurt);         // 다쳤을때 효과음 재생
            anim.SetTrigger("Hurt");        // 다쳤을때 애니메이션 실행
        }
    }

    protected void Dead()                  // 동물 사망 함수
    {
        PlaySE(sound_Dead);         // 죽었을때 효과음 재생
        // 죽었을때의 상태 (움직임X, 사망애니메이션)
        isWalking = false;
        isRunning = false;
        isChasing = false;
        isAttacking = false;
        isDead = true;
        nav.ResetPath();        // 움직임 정지, 초기화
        anim.SetTrigger("Dead");
    }

    protected void RandomSound()                  // 일상 효과음 랜덤재생 함수
    {
        int _random = Random.Range(0, 3);       // 일상 효과음 3개 0,1,2
        PlaySE(sound_Normal[_random]);      // 일상 효과음 랜덤재생
    }

    protected void PlaySE(AudioClip _clip)        //  효과음 재생 함수
    {
        theAudio.clip = _clip;                  // 해당 클립 대입
        theAudio.Play();                        // 해당 클립 재생
    }

    public Item GetItem()                   // 아이템 획득 함수
    {
        this.gameObject.tag = "Untagged";
        Destroy(this.gameObject, 3f);
        return item_Prefab;
    }
}
