using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * When running we bump into it and it stops
 * Some, always including the first one, randomly are like 1/2 the speed
 */


public class ZapShot : MonoBehaviour
{
    [SerializeField] private GameObject huffObject;
    [SerializeField] private int index;
    private Rigidbody2D body;

    private bool beenShot;
    private Vector3 trejectroy;
    private float timeOut = 0;
    const float maxTime = 1;
    const int ENEMY_LAYER_NO = 7, BREAKABLE_LAYER_NO = 9;

    // Start is called before the first frame update
    void Start()
    {
        beenShot = true;
        body = GetComponent<Rigidbody2D>();
        timeOut = 0;
        trejectroy = new Vector3(20, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (beenShot)
        {
            body.velocity = trejectroy;
            if (timeOut >= maxTime)
                die();
            else
                timeOut += Time.deltaTime;
        }
        else
        {
            body.velocity = Vector3.zero;
            timeOut = 0;
        }
        
    }

    private void OnEnable()
    {
        beenShot = true;
        timeOut = 0;
    }

    private void OnDisable()
    {
        beenShot = false;
        timeOut = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        beenShot = false;
        if (collision.gameObject.layer == ENEMY_LAYER_NO)
        {
            collision.gameObject.GetComponent<EnemyHealth>().takeDamage(1);
            die();
        }
        else if (collision.gameObject.layer == BREAKABLE_LAYER_NO)
        {
            // breakable.takeHit()
            die();
        }
        else
            die();
    }

    public void shoot(float speed, Vector3 pos)
    {
        Debug.Log(speed);
        beenShot = true;
        trejectroy = new Vector3(speed, 0, 0);
        transform.position = pos;
        
    }

    public void die()
    {
        beenShot = false;
        huffObject.GetComponent<HuffMovement>().disableZapShot(index);
    }

}
