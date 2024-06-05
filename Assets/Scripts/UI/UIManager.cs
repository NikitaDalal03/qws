using System.Collections.Generic;
using UnityEngine;
public enum GameScreens
{
    Home,
    Play,
    Pause,
    Draw,
    Win1,
    Win2,
    Info,
    Vs,
    Names,
}
public enum GamePopUp
{
    SoundSetting,
}

public class UIManager : MonoBehaviour
{
    [SerializeField] BaseScreen currentScreen;
    public static UIManager instance;
    [SerializeField] List<BaseScreen> screens;

    private void Start()
    {
        instance = this;
        currentScreen.ActivateScreen();
    }

    public void SwitchScreen(GameScreens screen)
    {
        foreach (BaseScreen baseScreen in screens)
        {
            if (baseScreen.screen == screen)
            {
                baseScreen.ActivateScreen();
                currentScreen.DeActivateScreen();
                currentScreen = baseScreen;
            }
        }
    }


}