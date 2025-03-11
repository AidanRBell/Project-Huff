using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    HuffControls controls;
    [SerializeField] private GameObject[] buttons;
    private int currButton = 0, numOfButtons;

    [SerializeField] GameObject transitionObject;
    private Animator transition;
    public float transitionTime = 1f;
    
    // player press level variables
    const int NOT_UNLOCKED = 0, UNLOCKED_NO_PEICE = 1, UNLOCKED_WITH_PEICE = 2, CLEARED_NO_PEICE = 3, CLEARED_WITH_PEICE = 4;



    // Start is called before the first frame update
    void Start()
    {
        transition = transitionObject.GetComponent<Animator>();
        
        controls = new HuffControls();
        controls.Menu.Enable();
        controls.Huff.Disable();

        controls.Menu.Up.performed += ctx => upPressed();
        controls.Menu.Down.performed += ctx => downPressed();
        controls.Menu.Left.performed += ctx => leftPressed();
        controls.Menu.Right.performed += ctx => rightPressed();
        controls.Menu.Select.performed += ctx => selectPressed();
        controls.Menu.Cancel.performed += ctx => cancelPressed();

        highlightButton(0);
        numOfButtons = buttons.Length;

        playerPrefsDefaults();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startButton()
    {
        StartCoroutine(loadLevel(1)); // transitions and loads the world map level
    }

    public void quitButton()
    {
        Application.Quit();
    }

    public void upPressed()
    {
        Debug.Log("here");
        unhighlightButton(currButton);
        highlightButton(--currButton % numOfButtons);
    }

    public void downPressed()
    {
        unhighlightButton(currButton);
        highlightButton(++currButton % numOfButtons);
    }

    public void leftPressed()
    {

    }
    public void rightPressed()
    {

    }

    public void selectPressed()
    {
        pressButton(currButton);
    }

    public void cancelPressed()
    {

    }

    private void highlightButton(int index)
    {
        buttons[index].GetComponent<Button>().Select();
    }

    private void unhighlightButton(int index)
    {
        buttons[index].GetComponent<Button>().OnDeselect(null);
    }

    public void pressButton(int index)
    {
        buttons[index].GetComponent<Button>().OnPointerDown(new PointerEventData(EventSystem.current));
    }

    IEnumerator loadLevel(int levelIndex)
    {
        // start the transition animation
        transition.SetTrigger("exit");

        // Wait a certain amount of time for the transition to do it's thing
        yield return new WaitForSeconds(transitionTime);

        // Load scene
        SceneManager.LoadScene(levelIndex);
    }

    private void playerPrefsDefaults()
    {
        int levelStatus = PlayerPrefs.GetInt("lvl1", NOT_UNLOCKED);
        for (int i = 2; i <= 8; i++)
        {
            levelStatus = PlayerPrefs.GetInt("lvl" + i.ToString(), UNLOCKED_NO_PEICE);
        }
    }

}
