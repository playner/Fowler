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
        set { }
    }

    [SerializeField]
    private float playerJumpForce;

    private bool isRun = false;
    private bool isWalk = false;
    private bool isGround = true;
    private bool isCrouch = false;

    private Vector3 lastPos;

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
    private CapsuleCollider capsuleCollider;
    private Rigidbody playerRigid;

    // Start is called before the first frame update
    void Start()
    {
        playerRigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        playerCrouchPosY = playerOriginPosY;
        playerOriginPosY = camera.transform.localPosition.y;
        isWalk = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckGround();
        Jump();
        Run();
        Crouch();

        CameraRotation();
        CharacterRoation();
    }

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
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
        if(Input.GetKeyDown(KeyCode.Space) && isGround && !isRun)
        {
            playerRigid.velocity = transform.up * playerJumpForce;
        }
    }

    void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRun = true;
            isWalk = false;
        }

        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
            isWalk = true;
        }
    }

    void Move()
    {
        float dirMoveX = Input.GetAxisRaw("Horizontal");
        float dirMoveZ = Input.GetAxisRaw("Vertical");

        Vector3 horizontalMove = transform.right * dirMoveX;
        Vector3 verticalMove = transform.forward * dirMoveZ;

        Vector3 playerVelocity = (horizontalMove + verticalMove).normalized * playerApplySpeed;

        playerRigid.MovePosition(transform.position + playerVelocity * Time.deltaTime);

    }

    //void moveCheck()
    //{
    //    if(!isRun && isGround)
    //    {
    //        if(Vector3.Distance(lastPos, transform.position) >= 0.01f)
    //        {
    //            isWalk = true;
    //        }
    //        else
    //        {
    //            isWalk = false;
    //        }
    //        lastPos = transform.position;
    //    }
    //}

    void CameraRotation()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRotation * playerLookSensitivity;
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        camera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    void CharacterRoation()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * playerLookSensitivity;
        playerRigid.MoveRotation(playerRigid.rotation * Quaternion.Euler(characterRotationY));
    }
}
