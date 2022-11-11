using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// MonoBehaviour를 붙이지 않으면 다른 객체에 Sound를 컴포넌트로 활용할 수 없다.
[System.Serializable]           // 클래스 자체 데이터 직렬화, [SerializeField]변수 와 같은 효과
public class Sound              // 효과음 관리 클래스
{
    public string name;         // 사운드의 이름
    public AudioClip clip;      // 사운드(클립)
}

public class SoundManager : MonoBehaviour
{
    //싱글톤 singleton 1개. 씬이동이 이뤄져도 한개만 존재하도록 해줌
    static public SoundManager instance;

    #region singleton
    void Awake()    // 객체 생성시 최초 실행
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    #endregion singleton

    void OnEnable() // 매번 활성화시 실행. 코루틴 실행 X
    {
        
    }

    // 매번 활성화시 실행 ex) SetActive(false) -> SetActive(true) 시 실행, 코루틴 실행 o
    void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }


    public AudioSource[] audioSourceEffects;        // 효과음 관리
    public AudioSource audioSourceBgm;              // 배경음 관리

    public string[] playSoundName;

    public Sound[] effectSounds;                    // 효과음 정보
    public Sound[] bgmSounds;                       // 배경음 정보

    public void PlaySE(string _name)        // 효과음 재생 함수
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if(_name  == effectSounds[i].name)              // 효과음 정보에 있는 이름과 일치하는게 있는지 확인
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if(!audioSourceEffects[j].isPlaying)                // 효과음 관리에서 재생중이지 않은 효과음을 확인
                    {
                        playSoundName[j] = effectSounds[i].name;                // 효과음 이름 저장
                        audioSourceEffects[j].clip = effectSounds[i].clip;      // 파라미터 _name과 일치하는 효과음을 넣어준다
                        audioSourceEffects[j].Play();                           // 효과음 재생
                        return;                                                 // return 으로 함수를 끝내서 for문을 빠져나옴
                    }
                }
                Debug.Log("모든 가용 AudioSource가 사용중입니다");
                return;
            }
        }
        Debug.Log(_name + "사운드가 SoundManager에 등록되지 않았습니다");
    }

    public void StopAllSE()               // 모든 효과음 재생 취소 함수
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();                       // 탐색하는 효과음 재생 취소
        }
    }

    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (playSoundName[i] == _name)                  // 파라미터 _name과 같은 효과음인지 확인
            {
                audioSourceEffects[i].Stop();               // 효과음 재생 취소       
                return;
            }
        }
        Debug.Log("재생 중인" + _name + "사운드가 없습니다");
    }
}
