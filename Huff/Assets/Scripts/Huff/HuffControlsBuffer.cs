using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HuffControlsBuffer : MonoBehaviour
{
    private HuffControls controls;
    private HuffMovement movementScript;

    private bool holdingLeft, holdingRight, holdingUp, holdingDown, holdingJump,
        holdingShoot, holdingCharge, holdingGrip, holdingPath, holdingInteract;

    // states
    const int HITSTUN = 0, IDLE_GROUND = 1, MIDAIR = 2, RUNNING_LEFT = 3, RUNNING_RIGHT = 4,
        ZAPSHOT_IDLE = 5, ZAPSHOT_RUNNING = 6, ZAPSHOT_MIDAIR = 7, GRIP_GRABBING = 8,
        GRIP_HOLDING_IDLE = 9, GRIP_HOLDING_RUNNING = 10, GRIP_HOLDING_MIDAIR = 11, GRIP_THROWING = 12,
        PATH_CHARGING = 13, PATH_DASHING = 14, TORNADO_JUMPING = 15, THUNDER_JUMPING = 16,
        INTERACTING = 17, CUTSCENE = 18, KARTING = 19;

        
    // Start is called before the first frame update
    void Start()
    {
        controls = new HuffControls();
        movementScript = GetComponent<HuffMovement>();

        controls.Huff.Left.performed += ctx => holdingLeft = true;
        controls.Huff.Left.canceled += ctx => holdingLeft = false;

        controls.Huff.Right.performed += ctx => holdingRight = true;
        controls.Huff.Right.canceled += ctx => holdingRight = false;

        controls.Huff.Up.performed += ctx => holdingUp = true;
        controls.Huff.Up.canceled += ctx => holdingUp = false;

        controls.Huff.Down.performed += ctx => holdingDown = true;
        controls.Huff.Down.canceled += ctx => holdingDown = false;

        controls.Huff.Jump.performed += ctx => holdingJump = true;
        controls.Huff.Jump.canceled += ctx => holdingJump = false;

        controls.Huff.Shoot.performed += ctx => holdingShoot = true;
        controls.Huff.Shoot.canceled += ctx => holdingShoot = false;

        // probably gonna scrap
        controls.Huff.Charge.performed += ctx => holdingCharge = true;
        controls.Huff.Charge.canceled += ctx => holdingCharge = false;

        controls.Huff.Grip.performed += ctx => holdingGrip = true;
        controls.Huff.Grip.canceled += ctx => holdingGrip = false;

        controls.Huff.Path.performed += ctx => holdingPath = true;
        controls.Huff.Path.canceled += ctx => holdingPath = false;

        controls.Huff.Interact.performed += ctx => holdingInteract = true;
        controls.Huff.Interact.canceled += ctx => holdingInteract = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int getState()
    {
        //return movementScript.getState();
        return 0;
    }
}
