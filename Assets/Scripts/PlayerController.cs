using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    static public bool isActivated = true;      // �÷��̾� �����Ҽ� �ִ���

    // ���ǵ� ���� ����
    [SerializeField]
    private float walkSpeed;            // �ȴ� �ӵ� ����
    [SerializeField]
    private float runSpeed;             // �޸��� �ӵ� ����
    [SerializeField]
    private float crouchSpeed;          // �ɾƼ� �����̴� �ӵ� ����
    private float applySpeed;           // ���� �ӵ��� �����ϴ� ����

    // ���� ����
    [SerializeField]
    private float jumpForce;            // ���� ����

    // ���� ����
    private bool isWalk = false;        // �ȱ� ����
    private bool isRun = false;         // �޸��� ����
    private bool isCrouch = false;      // �ɱ� ����
    private bool isGround = true;       // �������� ����

    // ������ üũ ����
    private Vector3 lastPos;

    // �ɾ��� �� �󸶳� ������ �����ϴ� ����
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;


    // ī�޶� ȸ�� �ΰ���
    [SerializeField]
    private float lookSensitivity;               // �ΰ���

    // ī�޶� ���� (����, �Ѱ�)
    [SerializeField]
    private float cameraRotationLimit;          // ī�޶� �Ѱ� ����
    private float currentCameraRotationX = 0f;  // ���� ī�޶� X�� ȸ������

    // �ʿ��� ������Ʈ
    [SerializeField]
    private Camera theCamera;                   // ����ī�޶�

    private Rigidbody myRigid;                  // �÷��̾� ����

    private CapsuleCollider capsuleCollider;    // �� ���� ���� Ȯ��

    //[SerializeField]                              // [SerializField] ��� Start()���� FIndObjectOfType���� ã���ִ� ����� ����
    private GunController theGunController;        // GunController ��ũ��Ʈ���� ������ �������� ���ؼ� ���

    private Crosshair theCrosshair;             // Crosshair ��ũ��Ʈ���� ������ �����ͼ� ���º����� ���� ũ�ν���� �ִϸ��̼��� ����ϱ� ���� ���

    private StatusController theStatusController;   // Status ���� ������ ���� ������Ʈ

    //[SerializeField]
    //private Gun theGun;
    //[SerializeField]
    //private Hand theHand;


    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        theGunController = FindObjectOfType<GunController>();
        theCrosshair = FindObjectOfType<Crosshair>();
        theStatusController = FindObjectOfType<StatusController>();

        // �ʱ�ȭ
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActivated && GameManager.canPlayerMove)         // Ȱ��ȭ, �÷��̾� ������ ���� Ȱ��ȭ
        {
            IsGround();
            TryJump();
            TryRun();
            TryCrouch();
            Move();
            MoveCheck();
            CameraRotation();
            CharacterRotation();
        }
    }

    private void TryCrouch() // �ɱ� �õ�
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    private void Crouch() // �ɱ� ����
    {
        /* �Ʒ��� ������
         if (isCrouch)
            isCrouch = false;
          else
            isCrouch = true;
         */
        isCrouch = !isCrouch;  // bool ���� �Լ��� �Ҹ������� ����ġ �������� True, False ���� ����Ī�ϵ��� ��
        theCrosshair.CrouchingAnimation(isCrouch);  // isCrouch�� Bool���� ���� Crouchũ�ν���� �ִϸ��̼� ����

        if (isCrouch) // ������
        {
            applySpeed = crouchSpeed;       // �ɾƼ� �����̴� �ӵ� ����
            applyCrouchPosY = crouchPosY;   // �ɾ��� �� ���� ����
        }
        else
        {
            applySpeed = walkSpeed;         // �ɾҴٰ� �Ͼ�� �� �����̴� �ӵ� ����
            applyCrouchPosY = originPosY;   // �ɾҴٰ� �Ͼ�� �� ���� ����
        }
        // ĳ���Ͱ� �ɾ��� �� ī�޶��� ���� ����
        StartCoroutine(CrouchCoroutine());
    }

    IEnumerator CrouchCoroutine() // ĳ���Ͱ� ������ �ε巯�� ī�޶� �������� �ڷ�ƾ �Լ�
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while(_posY != applyCrouchPosY)
        {
            count++;
            // Lerp �� ���������� ����, ī�޶� �������� ������ �ε巴�� �ٲ��ش�.
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
                break;
            yield return null;
        }
        // ������ �� ���� �������� �ʱ⿡ count�� while���� �������ͼ� ���� �ٽ� �������ش�.
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }
    private void IsGround() // �ٴڿ� ����ִ��� Ȯ���ϴ� �Լ�
    {
        // ������ �ٴ����� ��Ƽ� ĳ���Ͱ� �ٴڿ� ��Ҵ��� Ȯ��
        // bounds�� �ݶ��̴� �ٴڸ��� ���ϰ� extents�� �ݶ��̴��� ������ ���̸� ����
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        theCrosshair.JumpingAnimation(!isGround);       // ���� �پ����� ������ �޸����� ũ�ν���� �ִϸ��̼��� ����
    }
    private void TryJump() // ���� �Է� & ���� �����Լ�
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround && theStatusController.GetCurrentSP() > 0)
        {
            Jump();
        }
    }

    private void Jump() // ����
    {
        // �������¿��� ������ ���� ���� ����
        if (isCrouch)
            Crouch();
        theStatusController.DecreaseStamina(100);           // ���׹̳� 100 ����
        myRigid.velocity = transform.up * jumpForce;
    }

    private void TryRun() // �޸��� �Է� & �޸��� ���ǰ˻� �Լ�
    {
        if(Input.GetKey(KeyCode.LeftShift) && theStatusController.GetCurrentSP() > 0)
        {
            Running();
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift) || theStatusController.GetCurrentSP() <= 0)
        {
            RunningCancel();
        }
    }

    private void Running() // �޸��� �Լ�
    {
        // �������¿��� �޸��� ���� ���� ����
        if (isCrouch)
            Crouch();

        theGunController.CancelFineSight();         // �޸��� ������ ���
        
        isRun = true;
        //if (GunController.isActivate)
        //    theGun.anim.SetBool("Run", true);
        //else if (HandController.isActivate)
        //    theHand.anim.SetBool("Run", true);

        theCrosshair.RunningAnimation(isRun);       // �޸��� Running ũ�ν���� �ִϸ��̼� True�� ����
        theStatusController.DecreaseStamina(10);    // ���׹̳� 10 ����
        applySpeed = runSpeed;
    }

    private void RunningCancel() // �޸��� ��� �Լ�
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun); //�޸��� ������ Running ũ�ν���� �ִϸ��̼� False�� ����
        applySpeed = walkSpeed;
    }

    private void Move()  // ĳ���� ������ ���� �Լ�
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal"); // ĳ���� �¿� �̵� Ű �Է�
        float _moveDirZ = Input.GetAxisRaw("Vertical");   // ĳ���� �յ� �̵� Ű �Է�

        Vector3 _moveHorizontal = transform.right * _moveDirX; // Ű ���� ���� ĳ���� �¿� �̵� ����
        Vector3 _moveVertical = transform.forward * _moveDirZ; // Ű ���� ���� ĳ���� �յ� �̵� ����

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed; // ����� �ӵ� ����

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime); // ���� ĳ���Ͱ� �̵��� �ϰ� ���ش�.
        transform.position = myRigid.position;
    }

    private void MoveCheck()    // ĳ���� ������ üũ �Լ�
    {
        if(!isRun && !isCrouch && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f)     // ���� �������� ��ġ�� ���������� ��ġ�� ��
                isWalk = true;
            else
                isWalk = false;

            theCrosshair.WalkingAnimation(isWalk);      // �������� ũ�ν���� �ִϸ��̼� ����
            lastPos = transform.position;               // ���� �������� �������� lastPos������ ����
        }
    }
    private void CharacterRotation() // ĳ���� �¿� ȸ�� �Լ�
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");             // ���콺 �¿� ������ ���� Y�� ȸ�� �� �Է�
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;  // ĳ��ó Y�࿡ ȸ���� ����
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));   // ĳ������ Y �� ȸ������ŭ ȸ����Ų��.
    }

    private void CameraRotation() // ī�޶� ���� ȸ�� �Լ�
    {
        if(!pauseCameraRotation)                // ī�޶� ȸ���� ������ �ƴ϶��
        {
            float _xRotation = Input.GetAxisRaw("Mouse Y");             // ���콺 ���� ������ ���� X�� ȸ�� �� �Է�
            float _cameraRotationX = _xRotation * lookSensitivity;      // ī�޶�X�࿡ ȸ���� ����

            currentCameraRotationX -= _cameraRotationX;                 // ���� ī�޶� X�� ȸ�� ��  ps. X��ȸ���� �Ʒ��� ���ϸ� ���� + ���� ���ϸ� ���� -�� �ȴ�
                                                                        // ������ ���콺�� ���� �̰��� �ݴ��̹Ƿ� ������Ѵ�.
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);// ���� ī�޶� X�� ȸ�� ���� cameraRortationLimit ����ŭ �����Ѵ�.

            theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f); // ����ī�޶� ȸ�������� X�� ȸ�� ����ŭ ȸ����Ų��.
        }
    }

    private bool pauseCameraRotation = false;       // ī�޶� ȸ�� ���� bool ����

    public IEnumerator TreeLookCoroutine(Vector3 _target)       // ������ �ٶ󺸴� �ڷ�ƾ �Լ�
    {
        pauseCameraRotation = true;                 // ī�޶� ȸ�� ����

        Quaternion direction = Quaternion.LookRotation(_target - theCamera.transform.position);         // ��� ��ġ���� ī�޶� ��ġ(�÷��̾� ��ġ)�� ���� ������ ���´�
        Vector3 eulerValue = direction.eulerAngles;             // eulerAngles������ ��ȭ ��Ų�� , eulerValue = ī�޶� ���ؾ��� ���� ������
        float destinationX = eulerValue.x;                      // eulerAngles �� ����

        while(Mathf.Abs(destinationX - currentCameraRotationX) >= 0.5f)    // ������ �ٷδ� ������ ���� ī�޶� �ٶ󺸴� ���� ���� 0.5���� Ŭ ����        
        {
            eulerValue = Quaternion.Lerp(theCamera.transform.localRotation, direction, 0.3f).eulerAngles;   // ī�޶� õõ�� ���ϴ� ���� �ٶ󺸵��� �������ش�
            theCamera.transform.localRotation = Quaternion.Euler(eulerValue.x, 0f, 0f);             // ī�޶� ��ġ�� �������ش�
            currentCameraRotationX = theCamera.transform.localEulerAngles.x;                        // ���� ī�޶� �����̼ǰ��� �������ش�
            yield return null;
        }

        pauseCameraRotation = false;                 // ī�޶� ȸ�� ����
    }

    public bool GetRun()
    {
        return isRun;
    }
    public bool GetWalk ()
    {
        return isWalk;
    }
    public bool GetCrouch()
    {
        return isCrouch;
    }
    public bool GetIsGround()
    {
        return isGround;
    }
}
