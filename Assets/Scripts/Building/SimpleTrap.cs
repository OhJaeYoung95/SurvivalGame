using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrap : MonoBehaviour
{
    private Rigidbody[] rigid;      // 트랩을 쓰러지게 하기 위한 자식객체의 Rigidbody
    [SerializeField] private GameObject go_Meat;        // 트랩속 미끼(고기)

    [SerializeField]
    private int damage;         // 데미지

    private bool isActivated = false;       // 트랩 발동 여부

    private AudioSource theAudio;           // 효과음 재생을 위한 컴포넌트
    [SerializeField]
    private AudioClip sound_Activate;       // 트랩 발동 효과음

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponentsInChildren<Rigidbody>();
        theAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {   // 콜라이더 안에 들어오면
        if(!isActivated)    // 트랩이 작동중이지 않을때
        {
            if(other.transform.tag != "Untagged")   // 충돌한 오브젝트에 태그가 있다면
            {
                isActivated = true;     // 트랩 발동
                // 트랩 발동 효과음 재생
                theAudio.clip = sound_Activate;
                theAudio.Play();

                Destroy(go_Meat);       // 미끼(고기) 제거

                for (int i = 0; i < rigid.Length; i++)
                {   // 중력 생성(트랩 발동)
                    rigid[i].useGravity = true;
                    rigid[i].isKinematic = false;
                }

                if(other.transform.name == "Player")    // 플레이어가 부딪혔다면
                {       // 데미지 만큼 플레이어 체력 감소
                    other.transform.GetComponent<StatusController>().DecreaseHP(damage);
                }
            }
        }
    }
}
