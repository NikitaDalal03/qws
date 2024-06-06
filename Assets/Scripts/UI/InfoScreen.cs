using UnityEngine;
using UnityEngine.UI;

public class InfoScreen : BaseScreen
{
    [SerializeField] Button exitButton;


    private void Start()
    {
        exitButton.onClick.AddListener(OnExit);
    }

    public override void ActivateScreen()
    {
        base.ActivateScreen();
        GameManager.instance.isInputOn = false;
    }

    void OnExit()
    {
        SoundManager.inst.PlaySound(SoundName.BtnClick);
        UIManager.instance.SwitchScreen(GameScreens.Play);
    }

}


