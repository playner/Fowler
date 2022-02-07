using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveVerticalAxisName = "Vertical";
    public string moveHorizontalAxisName = "Horizontal";
    public string mouseXRotaionName = "Mouse Y";
    public string mouseYRotaionName = "Mouse X";
    public string fireButtonName = "Fire1";
    public string reloadButtonName = "Reload";
    public string runButtonName = "Run";
    public string crouchButtonName = "Crouch";
    public string jumpButtonName = "Jump";

    public float moveVertical { get; private set; }
    public float moveHorizontal { get; private set; }
    public float mouseXRotaion { get; private set; }
    public float mouseYRotaion { get; private set; }
    public bool fire { get; private set; }
    public bool reload { get; private set; }
    public bool run { get; private set; }
    public bool crouch { get; private set; }
    public bool jump { get; private set; }

    // Update is called once per frame
    void Update()
    {
        //if(GameManager.instance.isGameOver())
        //{
        //    moveHorizon = 0;
        //    moveVertical = 0;
        //    mouseXrotaion = 0;
        //    mouseYrotaion = 0;
        //    fire = false;
        //    reload = false;
        //    return 0;
        //}

        moveVertical = Input.GetAxisRaw(moveVerticalAxisName);
        moveHorizontal = Input.GetAxisRaw(moveHorizontalAxisName);
        mouseXRotaion = Input.GetAxisRaw(mouseYRotaionName);
        mouseYRotaion = Input.GetAxisRaw(mouseXRotaionName);
        fire = Input.GetButton(fireButtonName);
        reload = Input.GetButton(reloadButtonName);
        run = Input.GetButton(runButtonName);
        crouch = Input.GetButton(crouchButtonName);
        jump = Input.GetButton(jumpButtonName);
    }
}
