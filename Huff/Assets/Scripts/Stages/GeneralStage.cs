using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralStage : MonoBehaviour
{

    [SerializeField] GameObject huffObject;
    HuffMovement huffScript;

    int huffCurrentHealth;

    [SerializeField] GameObject[] huffHealthUIs;
    



    // Start is called before the first frame update
    void Start()
    {
        
        huffScript = huffObject.GetComponent<HuffMovement>();
        huffCurrentHealth = huffScript.getHealth();
        setHuffHealthUI(huffCurrentHealth);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (huffScript.getHealth() != huffCurrentHealth)
        {
            huffCurrentHealth = huffScript.getHealth();
            setHuffHealthUI(huffCurrentHealth);
        }
    }

    // do this
    void setHuffHealthUI(int huffHealth)
    {
        
        for (int i = 0; i < 4; i++)
        {
            if (i == huffHealth)
                huffHealthUIs[i].SetActive(true);
            else
                huffHealthUIs[i].SetActive(false);

        }
    }



    
}
