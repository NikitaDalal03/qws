using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayersNameScreen : BaseScreen
{
    //[SerializeField] Text player1Name;
    //[SerializeField] Text player2Name;
    [SerializeField] Button submit;
   

    private void Start()
    {
        submit.onClick.AddListener(OnSubmit);
    }

    void OnSubmit()
    {
        SoundManager.inst.PlaySound(SoundName.BtnClick);
        UIManager.instance.SwitchScreen(GameScreens.Play);
    }

    void OnTakingUserInput()
    {
        string.Join(" , ", GameScreens.Play);
    }
   
}
