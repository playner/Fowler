using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerWalkSpeed;
    [SerializeField]
    private float playerRunSpeed;

    private float playerApplySpeed
    {
        get
        {
            if (isWalk)
            {
                return playerWalkSpeed;
            }

            else if (isRun)
            {
                return playerRunSpeed;
            }

            return 0;
        }
    }

    [SerializeField]
    private float playerJumpForce;

    public bool isRun = false;
    public bool isWalk = false;
    public bool isGround = true;
    private bool isCrouch = false;

    private Vector3 lastPos = Vector3.zero;

    [SerializeField]
    private float playerCrouchPosY;
    private float playerOriginPosY;

    private float playerApplyCrouchPosY
    {
        get
        {
            if (isCrouch)
            {
                return playerCrouchPosY;
            }

            else
            {
                return playerOriginPosY;
            }
        }

        set { }
    }

    [SerializeField]
    private float playerLookSensitivity;

    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX;

    [SerializeField]
    private Camera camera;
    private PlayerInput playerInput;
    private CapsuleCollider capsuleCollider;
    private Rigidbody playerRigid;

    private GunController gunController;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        gunController = GetComponent<GunController>();

        playerCrouchPosY = playerOriginPosY;
        playerOriginPosY = camera.transform.localPosition.y;

        isWalk = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        moveCheck();
        CheckGround();
        Jump();
        Run();
        Crouch();

        CameraRotation();
        CharacterRoation();
    }

    void Crouch()
    {
        if (playerInput.crouch)
        {
            isCrouch = true;
            StartCoroutine(crouchCoroutine());
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouch = false;
            StartCoroutine(crouchCoroutine());
        }
    }

    IEnumerator crouchCoroutine()
    {
        float posY = camera.transform.localPosition.y;
        int count = 0;

        while(posY != playerApplyCrouchPosY)
        {
            count++;
            posY = Mathf.Lerp(posY, playerApplyCrouchPosY, 0.3f);
            camera.transform.localPosition = new Vector3(0f, posY, 0.35f);

            if (count > 15)
                break;

            yield return null;
        }

        camera.transform.localPosition = new Vector3(0f, playerApplyCrouchPosY, 0.35f);
    }

    void CheckGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y - 0.25f);
        //Debug.DrawRay(transform.position, new Vector3(0, -1 * (capsuleCollider.bounds.extents.y - 0.25f)));
    }

    void Jump()
    {
        if(playerInput.jump && isGround && !isRun)
        {
            playerRigid.velocity = transform.up * playerJumpForce;
        }
    }

    void Run()
    {
        if (playerInput.run)
        {
            isRun = true;
        }

        if(!playerInput.run)
        {
            isRun = false;
        }
    }

    void Move()
    {
        Vector3 horizontalMove = transform.right * playerInput.moveHorizontal;
        Vector3 verticalMove = transform.forward * playerInput.moveVertical;

        Vector3 playerVelocity = (horizontalMove + verticalMove).normalized * playerApplySpeed;

        playerRigid.MovePosition(transform.position + playerVelocity * Time.deltaTime);
        //gunController.animator.SetBool("Walk", isWalk);
    }

    void moveCheck()
    {
        if(!isRun && isGround)
        {
            if(Vector3.Distance(lastPos, transform.position) >= 0.01f)
            {
                isWalk = true;
            }

            else
            {
                isWalk = false;
            }

            lastPos = transform.position;

        }

        else
        {
            isWalk = false;
        }
    }

    void CameraRotation()
    {
        float xRotation = playerInput.mouseXRotaion;
        float cameraRotationX = xRotation * playerLookSensitivity;
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        camera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    void CharacterRoation()
    {
        float yRotation = playerInput.mouseYRotaion;
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * playerLookSensitivity;
        playerRigid.MoveRotation(playerRigid.rotation * Quaternion.Euler(characterRotationY));
    }
}
