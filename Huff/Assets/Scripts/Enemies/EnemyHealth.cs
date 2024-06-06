using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int MAX_HEALTH = 1; // not all enemies will have a max health of 1;
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        health = MAX_HEALTH;
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    void die()
    {
        gameObject.SetActive(false);
    }

    public void takeDamage(int damage)
    {
        if (health - damage >= 0)
            health -= damage;
        if (health <= 0)
            die();
    }
}
