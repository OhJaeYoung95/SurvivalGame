using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp;                 // 바위의 체력

    [SerializeField]
    private float destroyTime;      // 바위 파괴시 날라가는 파편들 제거 시간

    [SerializeField]
    private SphereCollider col;     // 바위의 구체 콜라이더


    // 필요한 게임 오브젝트
    [SerializeField]
    private GameObject go_rock;     // 일반 바위
    [SerializeField]
    private GameObject go_debris;   // 깨진 바위 파편
    [SerializeField]
    private GameObject go_effect_prefabs;    // 바위 채굴시 나오는 이펙트
    [SerializeField]
    private GameObject go_rock_item_prefab;     // 돌맹이 아이템(재료)

    // 돌맹이 아이템(재료) 생성 개수
    [SerializeField]
    private int count;

    //// 사운드 관련 컴포넌트
    //[SerializeField]
    //private AudioSource audioSource;        // 바위 채굴 사운드
    //[SerializeField]
    //private AudioClip effect_sound;         // 바위 부서지기 전 채굴 사운드
    //[SerializeField]
    //private AudioClip effect_sound2;        // 바위 부서진 후 채굴 사운드 

    // 필요한 사운드 이름
    [SerializeField]
    private string strike_Sound;        // 바위 부서지기 전 채굴 사운드 이름
    [SerializeField]
    private string destroy_Sound;       // 바위 부서진 후 채굴 사운드 이름

    public void Mining()    // 바위 채굴 함수
    {
        //audioSource.clip = effect_sound;
        //audioSource.Play();

        SoundManager.instance.PlaySE(strike_Sound);             // 바위 부서지기 전 채굴 사운드 재생
        var clone = Instantiate(go_effect_prefabs, col.bounds.center, Quaternion.identity);         // 바위 채굴시 이펙트 프리팹 생성
        Destroy(clone, destroyTime);            // 변수에 프리팹을 넣어줌으로써, 생성된 이후에 프리팹을 다룰수 있게 해준다. 생성후 destroyTime뒤에 제거

        hp--;
        if (hp <= 0)
            Destruction();
    }

    private void Destruction()  // 바위 파괴 함수
    {
        //audioSource.clip = effect_sound2;
        //audioSource.Play();

        SoundManager.instance.PlaySE(destroy_Sound);            // 바위 부서진 후 채굴 사운드 재생
        col.enabled = false;                    // 콜라이더 제거
        
        for(int i =0; i <= count; i++)          // 돌맹이 아이템(재료)를 count 수 만큼 생성
        {
            Instantiate(go_rock_item_prefab, go_rock.transform.position, Quaternion.identity);
        }
        Destroy(go_rock);                       // 바위 본체 제거

        go_debris.SetActive(true);              // 바위 파편 생성
        Destroy(go_debris, destroyTime);        // 바위 파편 destroyTime 이후에 삭제
    }
}