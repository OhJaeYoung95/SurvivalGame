using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTrap : MonoBehaviour
{
    private Animator anim;                  // 애니메이션 재생
    private AudioSource theAudio;           // 효과음 재생

    private bool isActivated = false;       // 트랩 작동 여부(중복 방지)

    [SerializeField]
    private AudioClip sound_Activate;       // 트랩 발동 효과음
    [SerializeField]
    private TrapDamage theTrapDamage;       // 트랩 데미지를 가해주기 위한 컴포넌트

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        theAudio = GetComponent<AudioSource>();
    }

    public bool GetIsActivated()        // 외부에 트랩 작동 여부 반환해주는 함수
    {
        return isActivated;
    }

    public void ReInstall()     // 트랩 재설치 함수
    {
        isActivated = false;
        anim.SetTrigger("DeActivate");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isActivated)        // 트랩이 작동이 되지 않았을때
        {       // 영역안에 들어온 오브젝트의 태그가 있을때 && 함정 태그가 아닐때
            if(other.transform.tag != "Untagged" && other.transform.tag != "Trap")
            {   // 트랩 작동, 트랩 데미지 적용, 에니메이션 효과음 재생
                StartCoroutine(theTrapDamage.ActivatedTrapCoroutine());
                isActivated = true;
                anim.SetTrigger("Activate");
                theAudio.clip = sound_Activate;
                theAudio.Play();
            }
        }
    }
}
