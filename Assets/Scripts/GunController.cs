using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;     // ���� ����ϰ� �ִ� �ѱ�

    private float currentFireRate;      // ���� �� ����ӵ�

    private bool isReload = false;              // ������ ����, false�� ���� �Ѿ˹߻� �����ϰ� ����� ���� Bool�� ����
    private bool isFineSightMode = false;      // ������ ����, True�� �� ������ ����

    [SerializeField]
    private Vector3 originPos;          // ������ �� �ٽ� �������� ���ƿö� �ʿ��� ����, ���� ������ ��

    private AudioSource audioSource;    // ����� �ҽ� (����ȿ��)

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
        TryFineSight();
    }

    private void GunFireRateCalc()      // ���� ����ӵ��� ����ϱ� ���� �Լ�
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

    private void Fire()     // �� �߻縦 ���� ����
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

    private void Shoot()    // �Ѿ� �߻�
    {
        currentGun.currentBulletCount--;        // �Ѿ� �߻�� źâ�� �����ִ� ź�� ����
        currentFireRate = currentGun.fireRate;  // �� ����ӵ� �ʱ�ȭ
        PlaySE(currentGun.fire_Sound);  // �Ѿ� �߻� ���� ���
        currentGun.muzzleFlash.Play();  // �Ѿ� ��������Ʈ ����

        // �ѱ� �ݵ� �ڷ�ƾ ����
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());

        Debug.Log("�Ѿ� �߻���");
    }

    private void TryReload()
    {
        if(Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)    // RŰ�� ������ ���������� �ƴϰ� ������ źâ���� ���� źâ�� �۴ٸ� ��������
        {
            CancelFineSight();                          // ������ �����ظ�尡 ����Ǹ� �����ظ�� �������ִ� �Լ�
            StartCoroutine(ReloadCoroutine());
        }
    }
    IEnumerator ReloadCoroutine()
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

    public void CancelFineSight()  // �����ظ�忡�� ������ �����ظ�尡 �����Ǵ� �Լ�
    {
        if (isFineSightMode)
            FineSight();
    }

    private void FineSight()        // ������ �Լ�
    {
        isFineSightMode = !isFineSightMode;                          // ����ġó�� ���������� ������ ���°� �����ȴ�.
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);   // �ִϸ��̼� ������ �� �ְ� FineSightMode bool �Ķ���Ϳ� true, false��ȯ

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

    IEnumerator FineSightActivateCoroutine()    // �����ظ�� ����
    {
        while(currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }

    IEnumerator FineSightDeactivateCoroutine()  // �����ظ�� ����
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
}
