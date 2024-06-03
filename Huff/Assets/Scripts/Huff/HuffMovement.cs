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

    [SerializeField] private float groundSpeed;

    [SerializeField] private float jumpForce;
    [SerializeField] private float maxHoldTime;
    [SerializeField] private float jumpHeldForce;
    [SerializeField] private float jumpHeldTimer;


    Boolean facingRight = true;
    Boolean runningLeft = false, runningRight = false;

    Vector2 horizontalInput;

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

        controls.Huff.Left.performed += ctx => runningLeft = true;
        controls.Huff.Left.canceled += ctx => runningLeft = false;

        controls.Huff.Right.performed += ctx => runningRight = true;
        controls.Huff.Right.canceled += ctx => runningRight = false;

        controls.Huff.Jump.performed += jump;

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
        if (onGround())
        {
            animator.SetBool("MidAir", false);
            
            if (runningLeft && !runningRight)
                runLeft();
            else if (runningRight)
                runRight();
            else
                animator.SetBool("Running", false);
        }
        else
        {
            if (true) // !!! change this based on  if other mid air states are being performed
            {
                animator.SetBool("MidAir", true);
            }
        }


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
        if (facingRight && runningLeft || !facingRight && runningRight)
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

    void jump(InputAction.CallbackContext context)
    {
        if (onGround()) // grounded jump
            groundJump();
        else
        {
            if (hasVoltage()) // thunder strike
            {

            }
            else // tornado jump
            {

            }
        }
    }

    void groundJump()
    {
        animator.SetBool("MidAir", true);
        body.velocity = new Vector2(body.velocity.x, jumpForce);
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