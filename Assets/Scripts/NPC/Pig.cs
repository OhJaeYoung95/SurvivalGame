using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField]
    private string animalName;          // 동물 이름
    [SerializeField]
    private int hp;                     // 동물 체력

    [SerializeField]
    private float walkSpeed;            // 걷기 스피드
    [SerializeField]
    private float runSpeed;             // 뛰기 스피드
    private float applySpeed;           // 실제 적용 스피드

    private Vector3 direction;          // 방향

    // 상태변수
    private bool isAction;              // 행동중인지 아닌지
    private bool isWalking;             // 걷는지 안걷는지
    private bool isRunning;             // 뛰는지
    private bool isDead;                // 죽었는지

    [SerializeField]
    private float walkTime;             // 걷기 시간
    [SerializeField]
    private float waitTime;             // 대기 시간
    [SerializeField]
    private float runTime;              // 뛰기 시간
    private float currentTime;          // 대기 계산시간

    // 필요한 컴포넌트
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Rigidbody rigid;
    [SerializeField]
    private BoxCollider boxCol;
    private AudioSource theAudio;

    [SerializeField]
    private AudioClip[] sound_pig_Normal;   // 평소 사운드
    [SerializeField]
    private AudioClip sound_pig_Hurt;       // 다칠때 사운드
    [SerializeField]
    private AudioClip sound_pig_Dead;       // 죽을때 사운드


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
        if(!isDead)             // 돼지가 죽지 않았으면
        {
            Move();
            Rotation();
            ElapseTime();
        }
    }

    private void Move()         // 움직임 설정 함수
    {
        if (isWalking || isRunning)      // 걷기중이면 1초에 walkSpeed 만큼 앞으로 나아가기
            rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
    }

    private void Rotation()     // 방향설정 함수
    {
        if(isWalking || isRunning)       // 걸을때만
        {
            // 원래 회전값에서 원하는 랜덤 회전값으로 자연스럽게 돌게 만들기
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotation));    // Vector3 => Quaternion으로 변경, 실제 회전값 적용부분
        }
    }

    private void ElapseTime()
    {
        if(isAction)            // 행동중일때만
        {
            currentTime -= Time.deltaTime;          // 1초에 1씩 감소
            if (currentTime <= 0)        // 현재 행동시간이 끝나면       
                ReSet();        // 조건 리셋후 다음 랜덤 행동
        }
    }

    private void ReSet()            // 다음 행동을 위한 조건리셋함수
    {
        isWalking = false;
        isRunning = false;
        isAction = true;
        applySpeed = walkSpeed;                 // 기본 걷기 속도로 초기화
        anim.SetBool("Walking", isWalking);     // 걷기 애니메이션 취소
        anim.SetBool("Running", isRunning);     // 뛰기 애니메이션 취소
        direction.Set(0f, Random.Range(0f, 360f), 0f);      // 이동방향 랜덤
        RandomAction();             // 랜덤 행동
    }

    private void RandomAction()             // 다음 랜덤 행동
    {
        RandomSound();      // 일상 랜덤 효과음 재생 함수실행

        // Random.Range(x,y) int일 경우 x는 포함 y는 포함하지 않는 랜덤한 수를 준다.
        int _random = Random.Range(0, 4);       // 대기, 풀뜯기, 두리번, 걷기

        if (_random == 0)
            Wait();
        else if (_random == 1)
            Eat();
        else if (_random == 2)
            Peek();
        else if (_random == 3)
            TryWalk();
    }

    private void Wait()         // 대기 함수
    {
        currentTime = waitTime;     // 현재 행동 시간 설정
        Debug.Log("대기");
    }
    private void Eat()          // 풀 뜯기 함수
    {
        currentTime = waitTime;     // 현재 행동 시간 설정
        anim.SetTrigger("Eat");     // 풀뜯기 애니메이션 실행
        Debug.Log("풀 뜯기");
    }
    private void Peek()         // 두리번 거리는 함수
    {
        currentTime = waitTime;     // 현재 행동 시간 설정
        anim.SetTrigger("Peek");    // 두리번 애니메이션 실행
        Debug.Log("두리번");
    }
    private void TryWalk()      // 걷기 시도 함수
    {
        isWalking = true;
        currentTime = walkTime;     // 현재 행동 시간 설정
        anim.SetBool("Walking", isWalking);     // 걷기 애니메이션 실행
        applySpeed = walkSpeed;     // 걷기 속도 대입
        Debug.Log("걷기");
    }
    private void Run(Vector3 _targetPos)      // 플레이어에게 맞고 뛰어서 도망가는 함수
    {
        // 돼지 위치에서 타겟위치(플레이어)를 빼면 반대방향으로 도망치는 방향이 나온다
        direction = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles;

        currentTime = runTime;      // 현재 행동 시간 설정
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed;     // 뛰기 속도 대입
        anim.SetBool("Running", isRunning);     // 뛰기 애니메이션 실행
    }

    public void Damage(int _dmg, Vector3 _targetPos)        // 돼지가 데미지를 입는 함수
    {
        if(!isDead)
        {
            hp -= _dmg;                     // HP 대미지만큼 차감

            if (hp <= 0)                     //  HP 가 0, 죽을때
            {
                Dead();                     // 돼지 사망
                return;
            }

            PlaySE(sound_pig_Hurt);         // 다쳤을때 효과음 재생
            anim.SetTrigger("Hurt");        // 다쳤을때 애니메이션 실행
            Run(_targetPos);                // 맞고 뛰어서 플레이어 반대방향으로 도망가도록 설정
        }
    }

    private void Dead()                  // 돼지 사망 함수
    {
        PlaySE(sound_pig_Dead);         // 죽었을때 효과음 재생
        // 죽었을때의 상태 (움직임X, 사망애니메이션)
        isWalking = false;
        isRunning = false;
        isDead = true;
        anim.SetTrigger("Dead");
    }

    private void RandomSound()                  // 일상 효과음 랜덤재생 함수
    {
        int _random = Random.Range(0, 3);       // 일상 효과음 3개 0,1,2
        PlaySE(sound_pig_Normal[_random]);      // 일상 효과음 랜덤재생
    }

    private void PlaySE(AudioClip _clip)        //  효과음 재생 함수
    {
        theAudio.clip = _clip;                  // 해당 클립 대입
        theAudio.Play();                        // 해당 클립 재생
    }
}
