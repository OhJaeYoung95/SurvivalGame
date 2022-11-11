using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // Ȱ��ȭ ����
    public static bool isActivate = false;

    [SerializeField]
    private Gun currentGun;     // ���� ����ϰ� �ִ� �ѱ�

    private float currentFireRate;      // ���� �� ����ӵ� ���

    // ���º���
    private bool isReload = false;              // ������ ����, false�� ���� �Ѿ˹߻� �����ϰ� ����� ���� Bool�� ����
    [HideInInspector]
    public bool isFineSightMode = false;      // ������ ����, True�� �� ������ ����

    private Vector3 originPos;          // ������ �� �ٽ� �������� ���ƿö� �ʿ��� ����, ���� ������ ��

    private AudioSource audioSource;    // ����� �ҽ� (����ȿ����)

    private RaycastHit hitInfo;         // �������� �浹�� ��ü�� ������ �������� ����

    // �ʿ��� ������Ʈ
    [SerializeField]
    private Camera theCam;              // ī�޶� ����, ȭ��� ����� ��ǥ�� �������� ���ؼ� ����ϴ� ����
    private Crosshair theCrosshair;     // ũ�ν���� �ִϸ��̼��� �����ϱ� ���� ����

    [SerializeField]
    private GameObject hit_effect_prefab;   // �ǰ� ����Ʈ ������

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

    private void GunFireRateCalc()      // ���� ����ӵ��� �����ϱ� ���� �Լ�
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;  // �뷫 60���� 1, �뷫 1�ʿ� 1�� ���� ��Ŵ
    }

    private void TryFire()  // �� �߻� �õ�
    {
        if(Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)   // �� ����ð��� �ǰ� ���콺 ���ʹ�ư�� ������ �ִٸ� �� �߻�
        {
            Fire();
        }
    }

    private void Fire()     // �� �߻縦 ���� ����, �߻� �� ���
    {
        if(!isReload)
        {
            if (currentGun.currentBulletCount > 0)      // ź������ �����ִ� �Ѿ��� �ִٸ�
                Shoot();
            else
            {
                CancelFineSight();                      // ������ �����ظ�尡 ����Ǹ� �����ظ�� �������ִ� �Լ�
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    private void Shoot()    // �Ѿ� �߻�, �߻� �� ��� �Լ�
    {
        theCrosshair.FireAnimation();           // �Ѿ� �߻��Ҷ� ����ڼ��� ���� ũ�ν���� �ִϸ��̼� ����
        currentGun.currentBulletCount--;        // �Ѿ� �߻�� źâ�� �����ִ� ź�� ����
        currentFireRate = currentGun.fireRate;  // �� ����ӵ� �ʱ�ȭ
        PlaySE(currentGun.fire_Sound);  // �Ѿ� �߻� ���� ���
        currentGun.muzzleFlash.Play();  // �Ѿ� ��������Ʈ ����
        Hit();                          // �Ѿ˿� �ǰݽ� �߻��ϴ� �Լ�

        // �ѱ� �ݵ� �ڷ�ƾ ����
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
    }

    private void Hit()      // �ǰݽ��� �Լ�
    {
        if(Physics.Raycast(theCam.transform.position, theCam.transform.forward + 
            new Vector3(Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                        Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                        0), out hitInfo, currentGun.range)) // ȭ�� ī�޶� ���� �߾ӿ��� �������� ���� �����Ÿ���ŭ ������ ��Ƽ� ������ �����´�.// �������� ���� ��Ȯ���� ������ش�.
        {
            GameObject clone = Instantiate(hit_effect_prefab,hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(clone, 2f);
        }
    }

    private void TryReload()    // ������ �õ� �Լ�
    {
        if(Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)    // RŰ�� ������ ���������� �ƴϰ� ������ źâ���� ���� źâ�� �۴ٸ� ��������
        {
            CancelFineSight();                          // ������ �����ظ�尡 ����Ǹ� �����ظ�� �������ִ� �Լ�
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
    IEnumerator ReloadCoroutine()           // ������
    {
        if(currentGun.carryBulletCount > 0)         // ���� �����ϰ� �ִ� ź���� ����(�� �ȿ� ���� �ʰ� ���ٴϰ� �ִ�)
        {
            isReload = true;

            currentGun.anim.SetTrigger("Reload");   // ������ �ִϸ��̼� ����

            currentGun.carryBulletCount += currentGun.currentBulletCount;   // ���� �����Ǿ� �ִ� �Ѿ��� ���������� ������ ������ �Ѿ��� �����ϰ� �ִ� �Ѿ˿� �־��ְ� �ؿ��� �������ϴ� ���
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);             // ������ ���ð� ���

            if(currentGun.carryBulletCount >= currentGun.reloadBulletCount)     // ���� �����ϰ� �ִ� ź���� ������ �������Ҷ� ��� �Ѿ��� ������ ��
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;   // ���� �������� �̷������ ��
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;    // ������ �� �����ϰ� �ִ� ź���� ���� ����
            }
            else                                                                // ���� �����ϰ� �ִ� ź���� ������ �������Ҷ� ��� �Ѿ˺��� ������
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;    // ��� �ִ� �Ѿ��� ������ ���� �Ѿȿ� ����
                currentGun.carryBulletCount = 0;
            }

            isReload = false;
        }
        else
        {
            Debug.Log("�����ϰ� �ִ� �Ѿ��� �����ϴ�.");
        }
    }

    private void TryFineSight()     // ������ �õ� �Լ�
    {
        if(Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }

    public void CancelFineSight()  // �����ظ�忡�� ������ �����ظ�尡 �����Ǵ� �Լ�, ������ ��� �Լ�
    {
        if (isFineSightMode)
            FineSight();
    }

    private void FineSight()        // ������ �Լ� (������ ���� ����)
    {
        isFineSightMode = !isFineSightMode;                          // ����ġó�� ���������� ������ ���°� �����ȴ�.
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);   // �ִϸ��̼� ������ �� �ְ� FineSightMode bool �Ķ���Ϳ� true, false��ȯ
        theCrosshair.FineSightAnimation(isFineSightMode);            // �����ؽ� ũ�ν���� �ִϸ��̼� ����

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

    IEnumerator FineSightActivateCoroutine()    // �����ظ�� ����, Ȱ��ȭ
    {
        while(currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }

    IEnumerator FineSightDeactivateCoroutine()  // �����ظ�� ����, ��Ȱ��ȭ
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }

    IEnumerator RetroActionCoroutine()          // �ѽ�(�� ��ü��) �ݵ����� (�� �ڷ� ��鸮�� ���)
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);                // ������ ���������� �ִ� �ݵ�
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);        // ������ �������� �ִ� �ݵ�
    
        if(!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos;

            // �ݵ� ����
            while(currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            // �� ��ġ
            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            // �ݵ� ����
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            // �� ��ġ
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }

    private void PlaySE(AudioClip _clip)   // ȿ���� ���, �Ű������� �ش� �����Ŭ���� �־ �����Ű�� �Լ�
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
