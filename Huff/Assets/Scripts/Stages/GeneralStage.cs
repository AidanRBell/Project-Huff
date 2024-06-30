using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralStage : MonoBehaviour
{

    [SerializeField] GameObject huffObject;

    int huffCurrentHealth;

    [SerializeField] GameObject[] huffHealthUIs;
    [SerializeField] Transform[] respawnPoints;

    private Vector2 currRespawnPos;
    



    // Start is called before the first frame update
    void Start()
    {
        
        huffCurrentHealth = huffObject.GetComponent<HuffMovement>().getHealth();
        setHuffHealthUI(huffCurrentHealth);
        currRespawnPos = new Vector2(respawnPoints[0].position.x, respawnPoints[0].position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (huffObject.GetComponent<HuffMovement>().getHealth() != huffCurrentHealth)
        {
            huffCurrentHealth = huffObject.GetComponent<HuffMovement>().getHealth();
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

    public void huffHitDeathPit()
    {
        huffObject.GetComponent<HuffMovement>().die();
    }


    // this need to probably be changed to reset the scene
    public void resetScene()
    {
        huffObject.GetComponent<HuffMovement>().respawn(currRespawnPos.x, currRespawnPos.y);
    }


    public Vector2 getCurrentRespawnPoint()
    {
        return currRespawnPos;
    }

    public void setCurrentRespawnPoint(int index)
    {
        
    }



    
}
