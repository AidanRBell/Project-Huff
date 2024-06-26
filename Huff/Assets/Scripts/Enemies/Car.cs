using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] GameObject huff;
    [SerializeField] GameObject respawnY;
    private const int HUFF_LAYER_NO = 6;
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == HUFF_LAYER_NO)
        {
            huff.GetComponent<HuffMovement>().takeDamage();
        }
    }

    public void idle()
    {
        anim.SetTrigger("idle");
        transform.position = new Vector2(transform.position.x, respawnY.transform.position.y);
    }

    public void chargeUp()
    {
        anim.SetTrigger("charge");
    }

    private void drive()
    {
        anim.SetTrigger("drive");
    }



}
