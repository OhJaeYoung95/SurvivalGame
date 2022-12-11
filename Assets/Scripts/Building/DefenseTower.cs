using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseTower : MonoBehaviour
{
    [SerializeField]
    private string towerName;           // 방어타워 이름
    [SerializeField]
    private float range;                // 방어타워 사정 거리
    [SerializeField]
    private int damage;                 // 방어타워 공격력
    [SerializeField]
    private float rateOfAccuracy;       // 방어타워 정확도
    [SerializeField]
    private float rateOfFire;           // 방어타워 연사속도
    private float currentRateOfFire;    // 방어타워 연사속도 계산
    [SerializeField]
    private float viewAngle;            // 방어타워 시야각
    [SerializeField]
    private float spinSpeed;            // 방어타워 포신 회전 속도
    [SerializeField]
    private LayerMask layerMask;        // 움직이는 대상만 타겟으로 설정
    [SerializeField]
    private Transform tf_TopGun;        // 방어타워 포탑위치 정보
    [SerializeField]
    private ParticleSystem particle_MuzzleFlash;    // 방어타워 총구 섬광
    [SerializeField]
    private GameObject go_HitEffect_Prefab;     // 적중 이펙트

    private RaycastHit hitInfo;     // 광선 충돌 객체의 정보 저장
    private Animator anim;          // 방어타워 총구 애니메이션
    private AudioSource theAudio;   // 방어타워 효과음

    private bool isFindTarget = false;  // 적 타겟 발견시 true
    private bool isAttack = false;      // 총구 방향과 적 방향이 일치할 시 true

    private Transform tf_Target;        // 현재 설정된 타겟위치 정보

    [SerializeField]
    private AudioClip sound_Fire;   // 발포 효과음

    // Start is called before the first frame update
    void Start()
    {
        theAudio = GetComponent<AudioSource>();
        theAudio.clip = sound_Fire;
        anim = GetComponent<Animator>();
    }

    // Update보다 조금 느리다, 정확하다
    // 매 프레임마다 x 정해진 초마다 실행
    void FixedUpdate()
    {
        Spin();             // 포탑 회전
        SearchEnemy();      // 적 탐색
        LookTarget();       // 적 겨누기(바로보기)
        Attack();           // 공격
    }

    private void Spin()     // 포탑 회전함수
    {   // 적발견 못했을때 && 공격중이 아닐때
        if(!isFindTarget && !isAttack)
        {
            // Quaternion형 변수에 spinSpeed만큼 회전하는 회전값을 대입
            Quaternion _spin = Quaternion.Euler(0f, tf_TopGun.eulerAngles.y + (1f * spinSpeed * Time.deltaTime), 0f);
            tf_TopGun.rotation = _spin;     // 매 프레임마다 회전
        }
    }

    private void SearchEnemy()      // 적 탐색함수
    {   // 반경 안에 있는 콜라이더가 있는 오브젝트들을 Collider배열 변수인 _targets에 대입
        // 포신의 위치로부터 포탑사정거리 반경안에 해당 레이어마스크와 콜라이더를 지닌 오브젝트정보를 _targets 변수에 담는다
        Collider[] _targets = Physics.OverlapSphere(tf_TopGun.position, range, layerMask);

        for (int i = 0; i < _targets.Length; i++)
        {   // _target의 위치정보를 대입
            Transform _targetTf = _targets[i].transform;
            // 타겟이 플레이어라면
            if(_targetTf.name == "Player")
            {
                // 타겟을 향한 방향
                Vector3 _direction = (_targetTf.position - tf_TopGun.position).normalized;
                float _anlge = Vector3.Angle(_direction, tf_TopGun.forward);        // 각도

                // 각도가 자신 기준 좌측각도 우측각도 안에 들어온다면
                if (_anlge < viewAngle * 0.5f)
                {   // 타겟 정보 대입, 적 타겟 발견
                    tf_Target = _targetTf;
                    isFindTarget = true;

                    if (_anlge < 5f)    // 타겟과의 각도가 작을때
                        isAttack = true;        // 공격 가능
                    else
                        isAttack = false;       // 공격 불가능
                    return;
                }
            }
        }
        // 타겟 탐색 실패시 타겟 초기화
        tf_Target = null;
        isAttack = false;
        isFindTarget = false;
    }

    private void LookTarget()       // 적 겨누기(바로보기)함수
    {
        if(isFindTarget)        // 적 발견시
        {       // 포신과 적과의 방향
            Vector3 _direction = (tf_Target.position - tf_TopGun.position).normalized;
            Quaternion _lookRotation = Quaternion.LookRotation(_direction); // 적 방향 바라보기
            Quaternion _rotation = Quaternion.Lerp(tf_TopGun.rotation, _lookRotation, 0.2f);   // 천천히 바라보기(보간)
            tf_TopGun.rotation = _rotation;     // 실제 회전값에 대입
        }
    }

    private void Attack()   // 공격 함수
    {
        if(isAttack)        // 공격 가능시(총구 방향과 적 방향과 일치시)
        {   // 연사속도 계산
            currentRateOfFire += Time.deltaTime;        // 1초에 1씩 증가
            if(currentRateOfFire >= rateOfFire)     // 연사속도 계산이 연사속도를 넘으면
            {   // 계산 초기화, 발사, 발사 애니메이션, 발사 효과음, 총구발사 파티클
                currentRateOfFire = 0;
                anim.SetTrigger("Fire");
                theAudio.Play();
                particle_MuzzleFlash.Play();
                // 레이저 발사, 총발사
                if(Physics.Raycast(tf_TopGun.position,      // 총구위치에서
                    tf_TopGun.forward + new Vector3(Random.Range(-1f, 1f) * rateOfAccuracy, Random.Range(-1f, 1f) * rateOfAccuracy, 0f),    // 총구반동 방향으로
                    out hitInfo, range, layerMask))     // 범위만큼, 해당레이어만
                {   // 타격 이펙트 생성, 제거
                    GameObject _temp = Instantiate(go_HitEffect_Prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    Destroy(_temp, 1f);
                    if(hitInfo.transform.tag == "Player")
                    {   // 플레이어 체력 감소
                        hitInfo.transform.GetComponent<StatusController>().DecreaseHP(damage);
                    }
                }
            }
        }
    }
}
