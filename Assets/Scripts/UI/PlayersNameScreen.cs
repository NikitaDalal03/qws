using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayersNameScreen : BaseScreen
{
    
    [SerializeField] Button submitButton;
    [SerializeField] Button backButton;

    //Player names input
    public string player1Name = "Player 1";
    public string player2Name = "Player 2";

    public TMP_InputField player1NameInput;
    public TMP_InputField player2NameInput;

    public TextMeshProUGUI player1NameText;
    public TextMeshProUGUI player2NameText;

    private void Start()
    {
        submitButton.onClick.AddListener(OnSubmit);
        backButton.onClick.AddListener(OnBack);
        player1NameText.text = player1Name;
        player2NameText.text = player2Name;
    }
    private void UpdatePlayerNameText()
    {
        player1NameText.text = player1Name;
        player2NameText.text = player2Name;
    }
    void OnSubmit()
    {

        player1NameText.text = player1NameInput.text + "1's Turn";
        player2NameText.text = player2NameInput.text + "2's Turn";
        SoundManager.inst.PlaySound(SoundName.BtnClick);
        UIManager.instance.SwitchScreen(GameScreens.Play);

    }

    void OnBack()
    {
        SoundManager.inst.PlaySound(SoundName.BtnClick);
        UIManager.instance.SwitchScreen(GameScreens.Vs);
    }

    public void Reset()
    {
        player1NameInput.text = string.Empty;
        player2NameInput.text = string.Empty;
    }
}
