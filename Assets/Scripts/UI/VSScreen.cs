using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VSScreen : BaseScreen
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] Button AIVsUButton;
    [SerializeField] Button UVsUButton;
    [SerializeField] Button backButton; 


    private void Start()
    {
        AIVsUButton.onClick.AddListener(OnAI);
        UVsUButton.onClick.AddListener(OnPlayer);
        backButton.onClick.AddListener(OnBack);
    }

    public override void ActivateScreen()
    {
        base.ActivateScreen();
        GameManager.instance.isInputOn = false;
    }

    void OnAI()
    {
        SoundManager.inst.PlaySound(SoundName.BtnClick);
        gameManager.SetGameMode(true);
        UIManager.instance.SwitchScreen(GameScreens.Play);
    }

    void OnPlayer()
    {
        SoundManager.inst.PlaySound(SoundName.BtnClick);
        //gameManager.SetGameMode(false);
        UIManager.instance.SwitchScreen(GameScreens.Names);
        gameManager.SetGameMode(false);
    }

    void OnBack()
    {
        SoundManager.inst.PlaySound(SoundName.BtnClick); 
        UIManager.instance.SwitchScreen(GameScreens.Home);
    }
    
}
