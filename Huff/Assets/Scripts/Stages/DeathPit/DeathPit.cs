using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPit : MonoBehaviour
{
    [SerializeField] private GameObject stage;
    const int HUFF_LAYER_NO = 6;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == HUFF_LAYER_NO)
            stage.GetComponent<GeneralStage>().huffHitDeathPit();
    }


}
