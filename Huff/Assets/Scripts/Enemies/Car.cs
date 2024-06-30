using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Car : MonoBehaviour
{
    const int UP = 0, DOWN = 1, RIGHT = 2, LEFT = 3;

    [SerializeField] GameObject huff;
    [SerializeField] float speed;
    [SerializeField] private int direction = 0;       

    private Rigidbody2D body;

    Vector3 respawnPoint;
    bool driving = false;
    private const int HUFF_LAYER_NO = 6;

    private void Start()
    {


        body = GetComponent<Rigidbody2D>();
        respawnPoint = transform.position;

        //if (direction == 0 || direction == 1)
        //    body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        //else if (direction == 2 || direction == 3)
        //    body.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        //else
        //    Debug.Log(gameObject.name + " car is given no direction");
    }

    // Update is called once per frame
    void Update()
    {
        switch (direction)
        {
            case UP:
                body.velocity = new Vector2(0, speed);
                break;
            case DOWN:
                body.velocity = new Vector2(0, -speed);
                break;
            case RIGHT:
                body.velocity = new Vector2(speed, 0);
                break;
            case LEFT:
                body.velocity = new Vector2(-speed, 0);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == HUFF_LAYER_NO)
        {
            huff.GetComponent<HuffMovement>().die();
        }
    }
    

    public void respawnOffScreen()
    {
        transform.position = respawnPoint;
    }


}
