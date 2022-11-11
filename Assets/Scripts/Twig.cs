using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twig : MonoBehaviour
{
    [SerializeField]
    private int hp;         // 나뭇가지 체력

    [SerializeField]
    private float destroyTime;          // 이펙트 제거 대기시간

    [SerializeField]
    private GameObject go_little_Twig;              // 나뭇가지 파괴시 나오는 작은 나뭇가지 조각프리팹

    [SerializeField]
    private GameObject go_hit_effect_prefab;        // 타격 이펙트

    // 회전값 변수
    private Vector3 originRot;                      // 원래 방향
    private Vector3 wantedRot;                      // 회전되기 원하는 방향
    private Vector3 currentRot;                     // 현재 방향


    // 필요한 사운드 이름
    [SerializeField]
    private string hit_Sound;                       // 타격 효과음
    [SerializeField]
    private string broken_Sound;                    // 파손 효과음

    // Start is called before the first frame update
    void Start()
    {
        originRot = this.transform.rotation.eulerAngles;        // 일반 transform.rotation은 Quaternion값이고 Vector3는 eulurAngles 값이기에 변환시켜준다
        currentRot = originRot;                                 // 원래 방향을 현재 방향에 초기화
    }

    // 플레이어가 나뭇가지을 때릴때의 함수
    public void Damage(Transform _playerTf)         // 플레이어가 때린 반대 방향으로 튀게 하기 위한 위치값(파라미터)
    {
        hp--;

        Hit();
        StartCoroutine(HitSwayCoroutine(_playerTf));

        if(hp<=0)
        {
            Destruction();                   // 파괴함수
        }
    }

    private void Hit()      // 나뭇가지 타격시 효과 함수
    {
        SoundManager.instance.PlaySE(hit_Sound);                // 타격 효과음 재생

        GameObject clone = Instantiate(go_hit_effect_prefab,                                        // 타격 이펙트 생성
                                       gameObject.GetComponent<BoxCollider>().bounds.center + (Vector3.up * 0.5f),        // 나뭇가지 박스콜라이더의 중앙에서 살짝 위쪽에서 생성
                                       Quaternion.identity);

        Destroy(clone, destroyTime);        // destroyTime 시간 후에 타격 이펙트 삭제
    }

    IEnumerator HitSwayCoroutine(Transform _target)
    {
        Vector3 direction = (_target.position - transform.position).normalized;     // 나뭇가지와 플레이어가 바라보는 방향, normalized = 정규화

        Vector3 rotationDir = Quaternion.LookRotation(direction).eulerAngles;       // direction 방향을 바라보게 만들어주는 함수

        CheckDirection(rotationDir);

        while(!CheckThreshold())            // 나뭇가지가 쓰러지는 과정
        {
            currentRot = Vector3.Lerp(currentRot, wantedRot, 0.04f);            // 원하는 방향으로 부드럽게 이동
            transform.rotation = Quaternion.Euler(currentRot);                  // 현재 방향에 대입
            yield return null;
        }

        wantedRot = originRot;                  // 다시 돌아가도록 값 설정

        while (!CheckThreshold())           // // 나뭇가지가 돌아가는 과정
        {
            currentRot = Vector3.Lerp(currentRot, wantedRot, 0.02f);            // 원하는 방향으로 부드럽게 이동
            transform.rotation = Quaternion.Euler(currentRot);                  // 현재 방향에 대입
            yield return null;
        }
    }

    private bool CheckThreshold()           // 임계점 확인 함수
    {
        if(Mathf.Abs(wantedRot.x - currentRot.x) <= 0.5f && Mathf.Abs(wantedRot.z - currentRot.z) <= 0.5f)  // 원하는 값에 근접했는지 확인
            return true;
        return false;
    }

    private void CheckDirection(Vector3 _rotationDir)           // 나뭇가지의 쓰러지는 방향을 구해주는 함수
    {
        Debug.Log(_rotationDir);

        if(_rotationDir.y > 180)                                // 플레이어가 치는 방향에 따라서 나뭇가지가 쓰러지는 방향 설정
        {
            if (_rotationDir.y > 300)
                wantedRot = new Vector3(-50f,0,-50f);
            else if (_rotationDir.y > 240)
                wantedRot = new Vector3(0, 0, -50f);
            else
                wantedRot = new Vector3(50f, 0, -50f);
        }
        else if(_rotationDir.y <= 180)
        {
            if (_rotationDir.y < 60)
                wantedRot = new Vector3(-50f,0, 50f);
            else if (_rotationDir.y < 120)
                wantedRot = new Vector3(0, 0, 50f);
            else
                wantedRot = new Vector3(50f, 0, 50f);
        }
    }

    private void Destruction()      // 나뭇가지 파손시 나오는 함수
    {
        SoundManager.instance.PlaySE(broken_Sound);         // 파손 효과음 재생

        GameObject clone1 = Instantiate(go_little_Twig,                                                     // 파손 이펙트 생성
                               gameObject.GetComponent<BoxCollider>().bounds.center + (Vector3.up * 0.5f),        // 나뭇가지 박스콜라이더의 중앙에서 살짝 위쪽에서 생성
                               Quaternion.identity);
        GameObject clone2 = Instantiate(go_little_Twig,                                                     // 파손 이펙트 생성
                               gameObject.GetComponent<BoxCollider>().bounds.center - (Vector3.up * 0.5f),        // 나뭇가지 박스콜라이더의 중앙에서 살짝 위쪽에서 생성
                               Quaternion.identity);

        Destroy(clone1,destroyTime);            // 나뭇가지 파편 2개 생성 후 destriyTime 이후 제거
        Destroy(clone2, destroyTime);   
        Destroy(gameObject);                    // 기존 나뭇가지 제거
    }
}
