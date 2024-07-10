using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMaxPoint : MonoBehaviour
{
    const int HAZARD_LAYER_NO = 8;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == HAZARD_LAYER_NO)
            collision.gameObject.GetComponent<Car>().respawnOffScreen();
    }
}
