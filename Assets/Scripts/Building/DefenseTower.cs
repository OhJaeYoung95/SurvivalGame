using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseTower : MonoBehaviour
{
    [SerializeField]
    private string towerName;           // ���Ÿ�� �̸�
    [SerializeField]
    private float range;                // ���Ÿ�� ���� �Ÿ�
    [SerializeField]
    private int damage;                 // ���Ÿ�� ���ݷ�
    [SerializeField]
    private float rateOfAccuracy;       // ���Ÿ�� ��Ȯ��
    [SerializeField]
    private float rateOfFire;           // ���Ÿ�� ����ӵ�
    private float currentRateOfFire;    // ���Ÿ�� ����ӵ� ���
    [SerializeField]
    private float viewAngle;            // ���Ÿ�� �þ߰�
    [SerializeField]
    private float spinSpeed;            // ���Ÿ�� ���� ȸ�� �ӵ�
    [SerializeField]
    private LayerMask layerMask;        // �����̴� ��� Ÿ������ ����
    [SerializeField]
    private Transform tf_TopGun;        // ���Ÿ�� ��ž��ġ ����
    [SerializeField]
    private ParticleSystem particle_MuzzleFlash;    // ���Ÿ�� �ѱ� ����
    [SerializeField]
    private GameObject go_HitEffect_Prefab;     // ���� ����Ʈ

    private RaycastHit hitInfo;     // ���� �浹 ��ü�� ���� ����
    private Animator anim;          // ���Ÿ�� �ѱ� �ִϸ��̼�
    private AudioSource theAudio;   // ���Ÿ�� ȿ����

    private bool isFindTarget = false;  // �� Ÿ�� �߽߰� true
    private bool isAttack = false;      // �ѱ� ����� �� ������ ��ġ�� �� true

    private Transform tf_Target;        // ���� ������ Ÿ����ġ ����

    [SerializeField]
    private AudioClip sound_Fire;   // ���� ȿ����

    // Start is called before the first frame update
    void Start()
    {
        theAudio = GetComponent<AudioSource>();
        theAudio.clip = sound_Fire;
        anim = GetComponent<Animator>();
    }

    // Update���� ���� ������, ��Ȯ�ϴ�
    // �� �����Ӹ��� x ������ �ʸ��� ����
    void FixedUpdate()
    {
        Spin();             // ��ž ȸ��
        SearchEnemy();      // �� Ž��
        LookTarget();       // �� �ܴ���(�ٷκ���)
        Attack();           // ����
    }

    private void Spin()     // ��ž ȸ���Լ�
    {   // ���߰� �������� && �������� �ƴҶ�
        if(!isFindTarget && !isAttack)
        {
            // Quaternion�� ������ spinSpeed��ŭ ȸ���ϴ� ȸ������ ����
            Quaternion _spin = Quaternion.Euler(0f, tf_TopGun.eulerAngles.y + (1f * spinSpeed * Time.deltaTime), 0f);
            tf_TopGun.rotation = _spin;     // �� �����Ӹ��� ȸ��
        }
    }

    private void SearchEnemy()      // �� Ž���Լ�
    {   // �ݰ� �ȿ� �ִ� �ݶ��̴��� �ִ� ������Ʈ���� Collider�迭 ������ _targets�� ����
        // ������ ��ġ�κ��� ��ž�����Ÿ� �ݰ�ȿ� �ش� ���̾��ũ�� �ݶ��̴��� ���� ������Ʈ������ _targets ������ ��´�
        Collider[] _targets = Physics.OverlapSphere(tf_TopGun.position, range, layerMask);

        for (int i = 0; i < _targets.Length; i++)
        {   // _target�� ��ġ������ ����
            Transform _targetTf = _targets[i].transform;
            // Ÿ���� �÷��̾���
            if(_targetTf.name == "Player")
            {
                // Ÿ���� ���� ����
                Vector3 _direction = (_targetTf.position - tf_TopGun.position).normalized;
                float _anlge = Vector3.Angle(_direction, tf_TopGun.forward);        // ����

                // ������ �ڽ� ���� �������� �������� �ȿ� ���´ٸ�
                if (_anlge < viewAngle * 0.5f)
                {   // Ÿ�� ���� ����, �� Ÿ�� �߰�
                    tf_Target = _targetTf;
                    isFindTarget = true;

                    if (_anlge < 5f)    // Ÿ�ٰ��� ������ ������
                        isAttack = true;        // ���� ����
                    else
                        isAttack = false;       // ���� �Ұ���
                    return;
                }
            }
        }
        // Ÿ�� Ž�� ���н� Ÿ�� �ʱ�ȭ
        tf_Target = null;
        isAttack = false;
        isFindTarget = false;
    }

    private void LookTarget()       // �� �ܴ���(�ٷκ���)�Լ�
    {
        if(isFindTarget)        // �� �߽߰�
        {       // ���Ű� ������ ����
            Vector3 _direction = (tf_Target.position - tf_TopGun.position).normalized;
            Quaternion _lookRotation = Quaternion.LookRotation(_direction); // �� ���� �ٶ󺸱�
            Quaternion _rotation = Quaternion.Lerp(tf_TopGun.rotation, _lookRotation, 0.2f);   // õõ�� �ٶ󺸱�(����)
            tf_TopGun.rotation = _rotation;     // ���� ȸ������ ����
        }
    }

    private void Attack()   // ���� �Լ�
    {
        if(isAttack)        // ���� ���ɽ�(�ѱ� ����� �� ����� ��ġ��)
        {   // ����ӵ� ���
            currentRateOfFire += Time.deltaTime;        // 1�ʿ� 1�� ����
            if(currentRateOfFire >= rateOfFire)     // ����ӵ� ����� ����ӵ��� ������
            {   // ��� �ʱ�ȭ, �߻�, �߻� �ִϸ��̼�, �߻� ȿ����, �ѱ��߻� ��ƼŬ
                currentRateOfFire = 0;
                anim.SetTrigger("Fire");
                theAudio.Play();
                particle_MuzzleFlash.Play();
                // ������ �߻�, �ѹ߻�
                if(Physics.Raycast(tf_TopGun.position,      // �ѱ���ġ����
                    tf_TopGun.forward + new Vector3(Random.Range(-1f, 1f) * rateOfAccuracy, Random.Range(-1f, 1f) * rateOfAccuracy, 0f),    // �ѱ��ݵ� ��������
                    out hitInfo, range, layerMask))     // ������ŭ, �ش緹�̾
                {   // Ÿ�� ����Ʈ ����, ����
                    GameObject _temp = Instantiate(go_HitEffect_Prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    Destroy(_temp, 1f);
                    if(hitInfo.transform.tag == "Player")
                    {   // �÷��̾� ü�� ����
                        hitInfo.transform.GetComponent<StatusController>().DecreaseHP(damage);
                    }
                }
            }
        }
    }
}
