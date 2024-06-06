using UnityEngine;
using UnityEngine.UI;

public class PlayScreen : BaseScreen
{
    [SerializeField] Button settingButton;
    [SerializeField] Button musicButton;
    [SerializeField] Button pauseButton;
    [SerializeField] Button soundButton;
    [SerializeField] Button iButton;

    [SerializeField] private Image musicBtnImage;
    [SerializeField] private Sprite musicOnButton;
    [SerializeField] private Sprite musicOffButoon;

    [SerializeField] private Image soundBtnImage;
    [SerializeField] private Sprite soundOnButton;
    [SerializeField] private Sprite soundOffButoon;

    [SerializeField] private Image pauseBtnImage;
    [SerializeField] private Sprite pauseOnButton;
    [SerializeField] private Sprite pauseOffButoon;

    bool isMusicOn = true;
    bool isSoundOn = true;
    bool isPause = false;

    private void Start()
    {
        settingButton.onClick.AddListener(OnSetting);
        musicButton.onClick.AddListener(OnMusicBtnPressed);
        soundButton.onClick.AddListener(OnSoundBtnPressed);
        iButton.onClick.AddListener(OniPressed);
        pauseButton.onClick.AddListener(OnPause);
    }

    void OnSetting()
    {
        SoundManager.inst.PlaySound(SoundName.BtnClick);
    }


    private void OnMusicBtnPressed()
    {
        isMusicOn = !isMusicOn;
        SoundManager.inst.PlaySound(SoundName.BtnClick);
        if (isMusicOn)
        {
            SoundManager.inst.MusicMute(true);
            musicBtnImage.sprite = musicOffButoon;
        }
        else
        {
            SoundManager.inst.MusicMute(false);
            musicBtnImage.sprite = musicOnButton;
        }
    }

    private void OnSoundBtnPressed()
    {
        isSoundOn = !isSoundOn;
        SoundManager.inst.PlaySound(SoundName.BtnClick);
        if (isSoundOn)
        {
            SoundManager.inst.SoundMute(true);
            soundBtnImage.sprite = soundOffButoon;
        }
        else
        {
            SoundManager.inst.SoundMute(false);
            soundBtnImage.sprite = soundOnButton;
        }
    }

    private void OniPressed()
    {
        SoundManager.inst.PlaySound(SoundName.BtnClick);
        UIManager.instance.SwitchScreen(GameScreens.Info);
    }

    private void OnPause()
    {

       // isPause = !isPause;
        SoundManager.inst.PlaySound(SoundName.BtnClick);
        //if (isPause)
        //{
            UIManager.instance.SwitchScreen(GameScreens.Pause);
         //   pauseBtnImage.sprite = pauseOffButoon;
        //}
        //else
        //{
        //    pauseBtnImage.sprite = pauseOnButton;
        //}
        Time.timeScale = 0;
    }



    public override void ActivateScreen()
    {
        base.ActivateScreen();
        GameManager.instance.isInputOn = true;
    }
}


