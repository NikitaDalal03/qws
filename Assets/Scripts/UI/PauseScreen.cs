using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : BaseScreen
{

    [SerializeField] Button restartButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button exitButton;


    private void Start()
    {
        restartButton.onClick.AddListener(OnRestart);
        quitButton.onClick.AddListener(OnQuit);
        exitButton.onClick.AddListener(OnExit);
    }

    public override void ActivateScreen()
    {
        base.ActivateScreen();
        GameManager.instance.isInputOn = false;
    }

  
    void OnRestart()
    {
        Time.timeScale = 1;
        SoundManager.inst.PlaySound(SoundName.BtnClick);
        UIManager.instance.SwitchScreen(GameScreens.Play);
        GameManager.instance.RestartGame();
    }

    void OnQuit()
    {
        Time.timeScale = 1;
        SoundManager.inst.PlaySound(SoundName.BtnClick);
        UIManager.instance.SwitchScreen(GameScreens.Home);
        GameManager.instance.RestartGame();
    }

    void OnExit()
    {
        Time.timeScale = 1;
        SoundManager.inst.PlaySound(SoundName.BtnClick);
        UIManager.instance.SwitchScreen(GameScreens.Play);
    }
}
