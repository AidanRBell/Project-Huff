using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squirrel : MonoBehaviour
{
    [SerializeField] GameObject acorn;
    [SerializeField] GameObject acornSpawnPoint;
    //SquirrelAcorn acornScript;
    Animator animator;
    const int MAX_CYCLES = 1;
    int numOfCycles = 0;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //acorn.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void oneCycle()
    {
        numOfCycles++;
        if (numOfCycles == MAX_CYCLES)
        {
            numOfCycles = 0;
            animator.SetTrigger("Throw");
        }
    }

    void throwAcorn()
    {
        if (acorn == null)
        {
            Debug.Log("acorn is null");
        }
        acorn.SetActive(true);
        acorn.GetComponent<SquirrelAcorn>().getThrown();
        
        
    }

    public void killAcorn()
    {
        acorn.transform.position = acornSpawnPoint.transform.position;
        acorn.SetActive(false);
    }

    

}
