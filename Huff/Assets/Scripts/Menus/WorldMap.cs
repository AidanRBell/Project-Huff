/**
 * Player prefs are funky
 */


using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// Level Names: RODEO (intro), BELL (school), NULL (boredom), SHARD (heartbreak),
// COG (work), ZZZ (sleep), MUD (grief), PILOT (change)

public class WorldMap : MonoBehaviour
{
    const short LVL_RODEO = 1, LVL_BELL = 2, LVL_NULL = 3, LVL_SHARD = 4,
        LVL_COG = 5, LVL_MUD = 6, LVL_ZZZ = 7, LVL_PILOT = 8;

    // Level Status'
    const int NOT_UNLOCKED = 0, UNLOCKED_NO_PEICE = 1, UNLOCKED_WITH_PEICE = 2, CLEARED_NO_PEICE = 3, CLEARED_WITH_PEICE = 4;

    Color[] levelColors = 
    { 
        new Color(230, 135, 38),
        Color.white,
        Color.white,
        Color.white,
        Color.white,
        Color.white,
        Color.white,
        Color.white,
    };

    // Not unlocked = grey, unlocked no peice = red, unlocked w peice = orange, cleared no peice = blue, cleared w peice = purple
    Color[] lvlTileFillColors =
    {
        new Color(132,132,132),
        new Color(231,92,82),
        new Color(224,132,75),
        new Color(58,129,207),
        new Color(174,116,226)
    };

    bool hasSelectedLevel = false;

    float transitionTime = 1f;

    [SerializeField] GameObject huff;

    [SerializeField] GameObject circleTransitionObject;
    [SerializeField] GameObject[] levelFills;


    HuffControls controls;

    // Start is called before the first frame update
    void Start()
    {
        controls = new HuffControls();
        controls.Menu.Enable();
        controls.Huff.Disable();

        controls.Menu.Up.performed += ctx => upPressed();
        controls.Menu.Down.performed += ctx => downPressed();
        controls.Menu.Left.performed += ctx => leftPressed();
        controls.Menu.Right.performed += ctx => rightPressed();
        controls.Menu.Select.performed += ctx => selectPressed();
        controls.Menu.Cancel.performed += ctx => cancelPressed();

        setLevelFills();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void upPressed()
    {

    }

    void downPressed()
    {

    }

    void leftPressed()
    {
        int currLvl = huff.GetComponent<Huff_WorldMap>().currentLevel();
        if (currLvl > 1)
            huff.GetComponent<Huff_WorldMap>().goTo(currLvl-1);
    }

    void rightPressed()
    {
        int currLvl = huff.GetComponent<Huff_WorldMap>().currentLevel();
        Debug.Log(currLvl);
        if (currLvl < 8)
            huff.GetComponent<Huff_WorldMap>().goTo(currLvl + 1);
    }

    void selectPressed()
    {

    }

    void cancelPressed()
    {

    }

    IEnumerator loadLevel(int levelIndex)
    {
        // start the transition animation
        circleTransitionObject.GetComponent<Renderer>().material.color = levelColors[levelIndex]; // set circle to the color of the level
        circleTransitionObject.GetComponent<Animator>().SetTrigger("exit");

        // Wait a certain amount of time for the transition to do it's thing
        yield return new WaitForSeconds(transitionTime);

        // Load scene70
        SceneManager.LoadScene(levelIndex);
    }

    void setLevelFills() // do they need to be buttons?
    {
        for (int i = 0; i < 8; i++)
        {
            int lvlStatus = PlayerPrefs.GetInt("lvl" + (i+1).ToString());

            if (lvlStatus == NOT_UNLOCKED)
            {
                levelFills[i].GetComponent<UnityEngine.UI.Image>().color = levelColors[NOT_UNLOCKED];
            }
            else if (lvlStatus == UNLOCKED_NO_PEICE)
            {
                levelFills[i].GetComponent<UnityEngine.UI.Image>().color = levelColors[UNLOCKED_NO_PEICE];
            }
            else if (lvlStatus == UNLOCKED_WITH_PEICE)
            {
                levelFills[i].GetComponent<UnityEngine.UI.Image>().color = levelColors[UNLOCKED_WITH_PEICE];
            }
            else if (lvlStatus == CLEARED_NO_PEICE)
            {
                levelFills[i].GetComponent<UnityEngine.UI.Image>().color = levelColors[CLEARED_NO_PEICE];
            }
            else if (lvlStatus == CLEARED_WITH_PEICE)
            {
                levelFills[i].GetComponent<UnityEngine.UI.Image>().color = levelColors[CLEARED_WITH_PEICE];
            }
        }
    }
}
