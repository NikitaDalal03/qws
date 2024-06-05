using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static PlayScreen;

public class WinScreen : BaseScreen
{
    [SerializeField] Button restartButton;
    [SerializeField] Button homeButton;
    [SerializeField] Button exitButton;

    private void Start()
    {
        restartButton.onClick.AddListener(OnRestart);
        homeButton.onClick.AddListener(OnHome);
        exitButton.onClick.AddListener(OnExit);
    }
 
    void OnRestart()
    {
        SoundManager.inst.PlaySound(SoundName.BtnClick);
        UIManager.instance.SwitchScreen(GameScreens.Play);
        GameManager.instance.RestartGame();
    }

    void OnHome()
    {
        SoundManager.inst.PlaySound(SoundName.BtnClick);
        UIManager.instance.SwitchScreen(GameScreens.Home);
        GameManager.instance.RestartGame();
    }

    void OnExit()
    {
        SoundManager.inst.PlaySound(SoundName.BtnClick);
        Application.Quit();
        
    }

    public override void ActivateScreen()
    {
        base.ActivateScreen();
        GameManager.isInputOn = false;
    }

}
