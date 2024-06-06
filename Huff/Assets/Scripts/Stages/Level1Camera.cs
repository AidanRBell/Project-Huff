using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Camera : MonoBehaviour
{
    private Camera camera;
    [SerializeField] GameObject huff;
    [SerializeField] float yOffset = 0; // don't keep in long run
    Vector2 huffPos;

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
            return new Vector3(0, 0 + yOffset, Z_DEFUALT);
        }
        
        return new Vector3(x, 0, Z_DEFUALT);
        
    }
}
