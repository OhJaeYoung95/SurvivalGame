using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;          // 총 이름
    public float range;             // 총 사정거리
    public float accuracy;          // 총 정확도
    public float fireRate;          // 연사 속도 (높을수록 느려짐)
    public float reloadTime;        // 재장전 속도

    public int damage;                // 총 데미지

    public int reloadBulletCount;      // 총알 재장전 개수
    public int currentBulletCount;    // 현재 탄알집에 남아있는 총알의 개수
    public int maxBulletCount;        // 최대 소유 가능 총알 개수
    public int carryBulletCount;      // 현재 소유하고 있는 탄알 개수(총안에 넣지 않은, 들고다니고 있는)

    public float retroActionForce;    // 총 반동 세기
    public float retroActionFineSightForce; // 정조준시의 반동 세기

    public Vector3 fineSightOriginPos;      // 정조준 했을때의 위치
    public Animator anim;              // 애니메이션
    public ParticleSystem muzzleFlash;  //  총구 화염 이펙트

    public AudioClip fire_Sound;         // 총 사운드

}
