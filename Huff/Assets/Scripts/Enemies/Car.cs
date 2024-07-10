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
    [SerializeField] private bool hasOffset = false;
    [SerializeField] private Vector2 offset;

    private Rigidbody2D body;

    Vector3 respawnPoint;
    bool driving = false;
    private const int HUFF_LAYER_NO = 6;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        if (hasOffset)
            return;
        else
            respawnPoint = new Vector2(transform.position.x + offset.x, transform.position.y + offset.y);
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
