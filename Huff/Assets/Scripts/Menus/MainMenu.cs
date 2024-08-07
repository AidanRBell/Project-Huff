using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    HuffControls controls;


    // Start is called before the first frame update
    void Start()
    {
        controls = new HuffControls();

        controls.Menu.Up.performed += ctx => upPressed();
        controls.Menu.Down.performed += ctx => downPressed();
        controls.Menu.Left.performed += ctx => leftPressed();
        controls.Menu.Right.performed += ctx => rightPressed();
        controls.Menu.Select.performed += ctx => selectPressed();
        controls.Menu.Down.performed += ctx => cancelPressed();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startButton()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void quitButton()
    {
        Application.Quit();
    }

    public void upPressed()
    {

    }
    
    public void downPressed()
    {

    }

    public void leftPressed()
    {

    }
    public void rightPressed()
    {

    }
    public void selectPressed()
    {

    }

    public void cancelPressed()
    {

    }

}
