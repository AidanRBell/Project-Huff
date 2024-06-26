/**
 * Tornado jump sometimes activates even when not already having starting jumped
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

    // the direction huff is facing
    bool facingRight = true;

    // running variables
    [SerializeField] private float groundSpeed;
    private bool holdingLeft = false, holdingRight = false, holdingUp = false, holdingDown = false;
    private bool holdingJump = false, holdingShoot = false, holdingCharge = false, holdingGrip = false, holdingPath = false, holdingInteract = false;

    // airdrifting variables
    [SerializeField] private float airStrafeFactor;
    [SerializeField] private float maxAirStrafeSpeed;

    // Jumping variables
    private bool isJumping = false;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpDragFactor;
    [SerializeField] private float maxHoldTime;
    [SerializeField] private float jumpHeldForce;
    private float jumpHeldTimer;

    // tornado jump
    private bool canTornadoJump = false;
    private bool tornadoJumping = false;
    [SerializeField] private float tornadoJumpingFactor;
    const float TORNADO_JUMP_MAX_TIME = 1.5f;
    private float tornadoJumpTimer;

    // sliding variables
    private bool isSliding = false, keepSliding = false;
    [SerializeField] private float slideFactor;
    public Transform slideAttackPoint;
    public float slideAttackRadius = 0.5f;



    private HuffControls controls;

    private Rigidbody2D body;
    private CircleCollider2D legsCrclCollider;
    private Animator animator;

    [SerializeField] private LayerMask groundLayer, enemyLayer, hazardLayer, breakableLayer;


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

        controls.Huff.Up.performed += ctx => holdingUp = true;
        controls.Huff.Up.canceled += ctx => holdingUp = false;

        controls.Huff.Down.performed += ctx => holdingDown = true;
        controls.Huff.Down.canceled += ctx => holdingDown = false;

        controls.Huff.Jump.performed += ctx => holdingJump = true;
        controls.Huff.Jump.canceled += ctx => holdingJump = false;

        controls.Huff.Shoot.performed += ctx => holdingShoot = true;
        controls.Huff.Shoot.canceled += ctx => holdingShoot = false;

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
        if (onGround()) // if player is on the ground
        {
            canTornadoJump = false;
            tornadoJumping = false;
            animator.SetBool("MidAir", false);
            animator.SetBool("TornadoJumping", false);
            tornadoJumpTimer = TORNADO_JUMP_MAX_TIME;

            if (holdingLeft && !holdingRight) // run left
                runLeft();
            else if (holdingRight) // run right (or both)
                runRight();
            else // not running
                animator.SetBool("Running", false);

            if (holdingJump) // initalizing ground jump
            {
                isJumping = true;
                jumpHeldTimer = 0;
                groundJump(jumpForce);
            }

            if (!hasVoltage() && holdingCharge && !isSliding && keepSliding) { 
                isSliding = true;
                animator.SetBool("Sliding", true);
                slide();
            }
            else if (!hasVoltage() && holdingCharge && isSliding && keepSliding) {
                slide();
            }
            else
            {
                isSliding = false;
                animator.SetBool("Sliding", false);
            }



        }
        else // if player is in mid air
        {
            animator.SetBool("Running", false);
            animator.SetBool("Sliding", false);
            isSliding = false;
            animator.SetBool("MidAir", true);

            // holding jump
            if (holdingJump && isJumping && jumpHeldTimer < maxHoldTime)
            {
                body.velocity = new Vector2(body.velocity.x, body.velocity.y + jumpHeldForce * Time.deltaTime);
                jumpHeldTimer += Time.deltaTime;
            }

            if (holdingLeft && !holdingRight)
                airStrafe(-1);
            else if (holdingRight)
                airStrafe(1);

            // base form and in mid air
            if (!hasVoltage())
            {
                if (holdingJump && canTornadoJump) { // just staring to tornado jump
                    tornadoJump();
                }
            }

            if (isJumping && !holdingJump && !tornadoJumping && tornadoJumpTimer > 0)
            {
                isJumping = false;
                canTornadoJump = true;
            }

            
        }

        if (holdingJump && jumpHeldTimer >= maxHoldTime)
            isJumping = false;


        if (!holdingCharge)
            keepSliding = true;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == groundLayer)
        {
            animator.SetBool("MidAir", false);
            animator.SetBool("TornadoJumping", false);
        }
    }



    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    //}


    void groundJump(float jumpingForce)
    {
        animator.SetBool("MidAir", true);
        animator.SetBool("TornadoJumping", false);

        body.velocity = new Vector2(body.velocity.x / jumpDragFactor, jumpForce);
    }

    void tornadoJump()
    {
        if (!tornadoJumping)
        {
            tornadoJumping = true;
            animator.SetBool("TornadoJumping", true);
        }
        if (tornadoJumpTimer > 0)
        {
            
            tornadoJumpTimer -= Time.deltaTime;
            body.velocity = new Vector2(body.velocity.x, tornadoJumpingFactor);
        }
        else
            canTornadoJump = false;

    }

    void slide()
    {
        keepSliding = true;
        int dir = 0;
        if (facingRight)
            dir = 1;
        else
            dir = -1;
        body.velocity = new Vector2(dir * slideFactor, body.velocity.y);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(slideAttackPoint.position, slideAttackRadius, enemyLayer);
        Collider2D[] hitBreakables = Physics2D.OverlapCircleAll(slideAttackPoint.position, slideAttackRadius, breakableLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
            enemy.GetComponent<EnemyHealth>().takeDamage(1);
        }
    }

    public void stopSliding()
    {
        keepSliding = false;
        isSliding = false;
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.DrawWireSphere(slideAttackPoint.position, slideAttackRadius);
    //}


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

    public void respawn(float x, float y)
    {
        body.velocity = Vector3.zero;
        transform.position = new Vector3(x,y,0);
    }


    // Voltage

    public bool hasVoltage()
    {
        return voltageTimer > 0;
    }


    // Other

    public void setAnimationBool(string name, bool value)
    {
        animator.SetBool(name, value);
    }



}