using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Huff_WorldMap : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("curr", 1);
        anim.SetInteger("goTo", -1);
    }

    public void setCurrLevel(int lvl)
    {
        anim.SetInteger("curr", lvl);
        anim.SetInteger("goTo", -1);
        anim.SetTrigger("idle");
    }

    public void goTo(int lvl)
    {
        anim.SetInteger("goTo", lvl);
    }

    public int currentLevel()
        { return anim.GetInteger("curr"); }
}
