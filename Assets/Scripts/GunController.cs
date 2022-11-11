using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // 활성화 여부
    public static bool isActivate = false;

    [SerializeField]
    private Gun currentGun;     // 현재 사용하고 있는 총기

    private float currentFireRate;      // 현재 총 연사속도 계산

    // 상태변수
    private bool isReload = false;              // 재장전 여부, false일 때만 총알발사 가능하게 만들기 위한 Bool형 변수
    [HideInInspector]
    public bool isFineSightMode = false;      // 정조준 여부, True일 때 정조준 상태

    private Vector3 originPos;          // 정조준 후 다시 시점으로 돌아올때 필요한 변수, 본래 포지션 값

    private AudioSource audioSource;    // 오디오 소스 (음향효과음)

    private RaycastHit hitInfo;         // 레이저에 충돌한 객체의 정보를 가져오는 변수

    // 필요한 컴포넌트
    [SerializeField]
    private Camera theCam;              // 카메라 정보, 화면상 가운데의 좌표를 가져오기 위해서 사용하는 변수
    private Crosshair theCrosshair;     // 크로스헤어 애니메이션을 제어하기 위한 변수

    [SerializeField]
    private GameObject hit_effect_prefab;   // 피격 이펙트 프리팹

    void Start()
    {
        originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
        theCrosshair = FindObjectOfType<Crosshair>();

    }
    // Update is called once per frame
    void Update()
    {
        if(isActivate)
        {
            GunFireRateCalc();
            TryFire();
            TryReload();
            TryFineSight();
        }
    }

    private void GunFireRateCalc()      // 총의 연사속도를 재계산하기 위한 함수
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;  // 대략 60분의 1, 대략 1초에 1씩 감소 시킴
    }

    private void TryFire()  // 총 발사 시도
    {
        if(Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)   // 총 연사시간이 되고 마우스 왼쪽버튼을 누르고 있다면 총 발사
        {
            Fire();
        }
    }

    private void Fire()     // 총 발사를 위한 과정, 발사 전 계산
    {
        if(!isReload)
        {
            if (currentGun.currentBulletCount > 0)      // 탄알집에 남아있는 총알이 있다면
                Shoot();
            else
            {
                CancelFineSight();                      // 장전시 정조준모드가 실행되면 정조준모드 해제해주는 함수
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    private void Shoot()    // 총알 발사, 발사 후 계산 함수
    {
        theCrosshair.FireAnimation();           // 총알 발사할때 사격자세에 따른 크로스헤어 애니메이션 실행
        currentGun.currentBulletCount--;        // 총알 발사시 탄창에 남아있는 탄알 감소
        currentFireRate = currentGun.fireRate;  // 총 연사속도 초기화
        PlaySE(currentGun.fire_Sound);  // 총알 발사 사운드 재생
        currentGun.muzzleFlash.Play();  // 총알 머즐이펙트 생성
        Hit();                          // 총알에 피격시 발생하는 함수

        // 총기 반동 코루틴 실행
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
    }

    private void Hit()      // 피격시의 함수
    {
        if(Physics.Raycast(theCam.transform.position, theCam.transform.forward + 
            new Vector3(Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                        Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                        0), out hitInfo, currentGun.range)) // 화면 카메라 기준 중앙에서 레이저를 총의 사정거리만큼 앞으로 쏘아서 정보를 가져온다.// 랜덤값을 통해 정확도를 계산해준다.
        {
            GameObject clone = Instantiate(hit_effect_prefab,hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(clone, 2f);
        }
    }

    private void TryReload()    // 재장전 시도 함수
    {
        if(Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)    // R키를 누르고 재장전중이 아니고 재장전 탄창보다 현재 탄창이 작다면 장전실행
        {
            CancelFineSight();                          // 장전시 정조준모드가 실행되면 정조준모드 해제해주는 함수
            StartCoroutine(ReloadCoroutine());
        }
    }

    public void CancelReload()
    {
        if(isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }
    IEnumerator ReloadCoroutine()           // 재장전
    {
        if(currentGun.carryBulletCount > 0)         // 현재 소유하고 있는 탄알의 개수(총 안에 넣지 않고 들고다니고 있는)
        {
            isReload = true;

            currentGun.anim.SetTrigger("Reload");   // 재장전 애니메이션 실행

            currentGun.carryBulletCount += currentGun.currentBulletCount;   // 현재 장전되어 있는 총알이 남아있을때 장전시 장전된 총알을 소유하고 있는 총알에 넣어주고 밑에서 재장전하는 방식
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);             // 재장전 대기시간 대기

            if(currentGun.carryBulletCount >= currentGun.reloadBulletCount)     // 현재 소유하고 있는 탄알의 개수가 재장전할때 드는 총알의 개수와 비교
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;   // 실제 재장전이 이루어지는 곳
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;    // 재장전 후 소유하고 있는 탄알의 개수 차감
            }
            else                                                                // 현재 소유하고 있는 탄알의 개수가 재장전할때 드는 총알보다 적을때
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;    // 들고 있는 총알의 개수를 전부 총안에 장전
                currentGun.carryBulletCount = 0;
            }

            isReload = false;
        }
        else
        {
            Debug.Log("소유하고 있는 총알이 없습니다.");
        }
    }

    private void TryFineSight()     // 정조준 시도 함수
    {
        if(Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }

    public void CancelFineSight()  // 정조준모드에서 장전시 정조준모드가 해제되는 함수, 정조준 취소 함수
    {
        if (isFineSightMode)
            FineSight();
    }

    private void FineSight()        // 정조준 함수 (정조준 로직 가동)
    {
        isFineSightMode = !isFineSightMode;                          // 스위치처럼 누를때마다 변수의 상태가 반전된다.
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);   // 애니메이션 실행할 수 있게 FineSightMode bool 파라미터에 true, false반환
        theCrosshair.FineSightAnimation(isFineSightMode);            // 정조준시 크로스헤어 애니메이션 실행

        if(isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactivateCoroutine());
        }
    }

    IEnumerator FineSightActivateCoroutine()    // 정조준모드 실행, 활성화
    {
        while(currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }

    IEnumerator FineSightDeactivateCoroutine()  // 정조준모드 해제, 비활성화
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }

    IEnumerator RetroActionCoroutine()          // 총신(총 자체의) 반동구현 (앞 뒤로 흔들리는 모션)
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);                // 정조준 안했을때의 최대 반동
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);        // 정조준 했을때의 최대 반동
    
        if(!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos;

            // 반동 시작
            while(currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            // 원 위치
            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            // 반동 시작
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            // 원 위치
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }

    private void PlaySE(AudioClip _clip)   // 효과음 재생, 매개변수에 해당 오디오클립을 넣어서 재생시키는 함수
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    public Gun GetGun()
    {
        return currentGun;
    }

    public bool GetFineSightMode()
    {
        return isFineSightMode;
    }

    public void GunChange(Gun _gun)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentGun = _gun;
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;

        currentGun.transform.localPosition = Vector3.zero;
        currentGun.gameObject.SetActive(true);

        isActivate = true;
    }
}
