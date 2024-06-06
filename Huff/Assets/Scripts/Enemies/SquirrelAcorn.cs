using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SquirrelAcorn : MonoBehaviour
{
    [SerializeField] GameObject squirrelParent;
    [SerializeField] float MAX_TIME = 5;
    float timeOutFor;

    [SerializeField] float xFactor, yFactor;
    [SerializeField] int playerLayerNumber, groundLayerNumber;    
    Rigidbody2D body;

    bool inAir = false;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        timeOutFor = 0;
    }

    private void Update()
    {
        if (inAir)
        {
            timeOutFor += timeOutFor;
            if (timeOutFor > MAX_TIME)
            {
                inAir = false;
                squirrelParent.GetComponent<Squirrel>().killAcorn();
            }
        }
    }

    public void getThrown()
    {
        body.velocity = new Vector2(xFactor, yFactor);
        inAir = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == playerLayerNumber) // Hitting huff interaction
        {
            collision.gameObject.GetComponent<HuffMovement>().takeDamage();
            inAir = false;
            squirrelParent.GetComponent<Squirrel>().killAcorn();
        }
        else if (collision.gameObject.layer == groundLayerNumber) // hitting the ground interaction
        {
            inAir = false;
            squirrelParent.GetComponent<Squirrel>().killAcorn();
        }

    }


}
