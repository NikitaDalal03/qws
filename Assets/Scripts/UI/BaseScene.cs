using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScreen : MonoBehaviour
{
    Canvas canvas;
    public GameScreens screen;

    protected void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    public virtual void ActivateScreen()
    {
        canvas.enabled = true;
    }

    public virtual void DeActivateScreen()
    {
        canvas.enabled = false;
    }


}
