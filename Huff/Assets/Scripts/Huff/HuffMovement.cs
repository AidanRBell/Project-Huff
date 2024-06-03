/**
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

public class HuffMovement : MonoBehaviour
{
    const int IDLE = 0, MIDAIR = 1, RUNNING = 2, JUMPING = 3, TORNADO_JUMP = 4;
    private int currAction;

    private float voltageTimer = 0;

    // Jumping variables
    private bool jumpPressed = false;
    private bool isJumping = false;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxHoldTime;
    [SerializeField] private float jumpHeldForce;
    private float jumpHeldTimer;

    // the direction huff is facing
    Boolean facingRight = true;

    // running variables
    [SerializeField] private float groundSpeed;
    Boolean holdingLeft = false, holdingRight = false;

    // airdrifting variables
    [SerializeField] private float airStrafeFactor;
    [SerializeField] private float maxAirStrafeSpeed;
    

    private HuffControls controls;

    private Rigidbody2D body;
    private CircleCollider2D legsCrclCollider;
    private Animator animator;

    [SerializeField] private LayerMask groundLayer;

    public Vector2 boxSize;
    public float castDistance;

    const int MAX_HEALTH = 3;
    private int health = MAX_HEALTH;



    // Start is called before the first frame update
    private void Awake()

    {
        // intializes body, crcl colider, and animator as their components from Huff
        body = GetComponent<Rigidbody2D>();
        legsCrclCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();

        // initialize the controls adapter
        controls = new HuffControls();

        controls.Huff.Left.performed += ctx => holdingLeft = true;
        controls.Huff.Left.canceled += ctx => holdingLeft = false;

        controls.Huff.Right.performed += ctx => holdingRight = true;
        controls.Huff.Right.canceled += ctx => holdingRight = false;

        controls.Huff.Jump.performed += ctx => jumpPressed = true;
        controls.Huff.Jump.canceled += ctx => jumpPressed = false;

        controls.Huff.Up.performed += up;
        controls.Huff.Down.performed += down;
        controls.Huff.Shoot.performed += zipShot;
        controls.Huff.Charge.performed += superCharge;
        controls.Huff.Grip.performed += electricGrip;
        controls.Huff.Path.performed += pulsePath;
        controls.Huff.Interact.performed += interact;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(holdingLeft + ", " + holdingRight);

        if (onGround()) // if player is on the ground
        {
            animator.SetBool("MidAir", false);
            
            if (holdingLeft && !holdingRight) // run left
                runLeft();
            else if (holdingRight) // run right (or both)
                runRight();
            else // not running
                animator.SetBool("Running", false);

            if (jumpPressed) // initalizing ground jump
            {
                isJumping = true;
                jumpHeldTimer = 0;
                groundJump(jumpForce);
            }
        }
        else
        {
            
               

            if (jumpPressed && isJumping && jumpHeldTimer < maxHoldTime)
            {
                body.velocity = new Vector2(body.velocity.x, body.velocity.y + jumpHeldForce * Time.deltaTime);
                jumpHeldTimer += Time.deltaTime;
            }

            if (true) // !!! change this based on  if other mid air states are being performed
            {
                animator.SetBool("MidAir", true);

                if (holdingLeft && !holdingRight)
                    airStrafe(-1);
                else if (holdingRight)
                    airStrafe(1);
            }
        }

        if (jumpPressed && jumpHeldTimer >= maxHoldTime)
            isJumping = false;


    }

    private void OnEnable()
    {
        controls.Huff.Enable();

    }

    private void OnDisable()
    {
        controls.Huff.Disable();
    }

    // Movement

    void runLeft() // note for now, left() is just running left
    {
        animator.SetBool("Running", true);
        turnAround();
        body.velocity = new Vector2(-groundSpeed, body.velocity.y);
    }

    void runRight() // note for now, right() is just running right
    {
        animator.SetBool("Running", true);
        turnAround();
        body.velocity = new Vector2(groundSpeed, body.velocity.y);
    }

    void airStrafe(int direction)
    {
        body.velocity = new Vector2(Mathf.Clamp(body.velocity.x + direction * airStrafeFactor * Time.deltaTime, -maxAirStrafeSpeed, maxAirStrafeSpeed), body.velocity.y);
    }


    // for testing, pressing up makes Huff take 1 hp of damage
    void up(InputAction.CallbackContext context)
    {
        Debug.Log("in up, " + health);
        takeDamage();
    }

    void down(InputAction.CallbackContext context)
    {

    }


    void turnAround()
    {
        if (facingRight && holdingLeft || !facingRight && holdingRight)
        {
            facingRight = !facingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
        }
    }



    public bool onGround()
    {
        return Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    //}

    // Jumping

    //void jump()
    //{
    //    if (onGround()) // grounded jump
    //        groundJump();
    //    else
    //    {
    //        if (hasVoltage()) // thunder strike
    //        {

    //        }
    //        else // tornado jump
    //        {

    //        }
    //    }
    //}

    void groundJump(float jumpingForce)
    {
        animator.SetBool("MidAir", true);
        body.velocity = new Vector2(body.velocity.x, jumpForce);
    }

    void jumpHeld(float jumpingForce)
    {

    }


    // voltage moves

    void zipShot(InputAction.CallbackContext context)
    {

    }

    void landingShot()
    {

    }

    void superCharge(InputAction.CallbackContext context)
    {

    }

    void electricGrip(InputAction.CallbackContext context)
    {

    }

    void pulsePath(InputAction.CallbackContext context)
    {

    }

    void interact(InputAction.CallbackContext context)
    {

    }


    // Health

    public int getHealth()
    {
        return health;
    }

    public int takeDamage()
    {
        if (health > 0)
            health--;
        return health;
    }

    public int instantKill()
    {
        health = 0;
        return 0;
    }

    public bool hasVoltage()
    {
        return voltageTimer > 0;
    }

    public void setAnimationBool(string name, bool value)
    {
        animator.SetBool(name, value);
    }



}