using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBattlePage : MonoBehaviour
{

    public GameController Controller;

    public InGameMenu GameMenu;

    public InPauseMenu PauseMenu;

    private void Awake()
    {

        Controller.SetMenu(GameMenu, PauseMenu);

        PauseMenu.LostText.SetActive(false);
        PauseMenu.WinText.SetActive(false);
        PauseMenu.gameObject.SetActive(false);

        GameMenu.AttackButton.onClick.AddListener(() => Controller.PlayerMove());
        GameMenu.SwitchButton.onClick.AddListener(() => Controller.SwitchCharacter());
        GameMenu.PauseButton.onClick.AddListener(() => Controller.Resume());

        PauseMenu.ResumeButton.onClick.AddListener(() => Controller.Resume());
        PauseMenu.RestartButton.onClick.AddListener(() => Controller.Restart());
        PauseMenu.MenuButton.onClick.AddListener(() => Controller.ToMenu());


    }
}
