using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level1Camera : MonoBehaviour
{
    private Camera camera;
    [SerializeField] GameObject huff;
    [SerializeField] GameObject[] boundaries;
    

    const float ZERO = 0;
    const float Z_DEFUALT = -10;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        camera.transform.position = newCameraPos();
    }

    private Vector3 newCameraPos()
    {
        float x = huff.transform.position.x;
        float y = huff.transform.position.y;
        
        if (x < 0)
        {
            return new Vector3(0, ZERO, Z_DEFUALT);
        }
        else if (x > 0 && x < boundaries[0].transform.position.x)
        {
            return new Vector3(x, 0, Z_DEFUALT);
        }
        else if (x >= boundaries[0].transform.position.x) //  edit this when adding another xStart
        {
            if (y > 0 && y < boundaries[1].transform.position.y)
                return new Vector3(x, y, Z_DEFUALT);
            else if (y >= boundaries[1].transform.position.y)
                return new Vector3(x, boundaries[1].transform.position.y, Z_DEFUALT);
            else
                return new Vector3(x, 0, Z_DEFUALT);
        }

        return new Vector3(x, 0, Z_DEFUALT);

    }
}
