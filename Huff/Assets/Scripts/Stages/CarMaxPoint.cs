using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMaxPoint : MonoBehaviour
{
    const int ENEMY_LAYER_NO = 7;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == ENEMY_LAYER_NO)
            collision.gameObject.GetComponent<Car>().respawnOffScreen();
    }
}
