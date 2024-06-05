using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : BaseScreen
{
    [SerializeField] Button restartButton;
    [SerializeField] Button homeButton;
    [SerializeField] Button exitButton;


    private void Start()
    {
        restartButton.onClick.AddListener(OnRetry);
        homeButton.onClick.AddListener(OnHome);
        exitButton.onClick.AddListener(OnExit);
    }

    public override void ActivateScreen()
    {
        base.ActivateScreen();
        GameManager.isInputOn = false;
    }

    public override void DeActivateScreen()
    {
        base.DeActivateScreen();
    }

    void OnRetry()
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

}
