using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InBattlePage : MonoBehaviour
{

    public GameController Controller;

    public InGameMenu GameMenu;

    public InPauseMenu PauseMenu;

    private void Awake()
    {

        Controller.SetMenu(GameMenu, PauseMenu);

        PauseMenu.gameObject.SetActive(false);

        PauseMenu.PauseScreen.SetActive(true);

        PauseMenu.WinText.SetActive(false);
        PauseMenu.LostText.SetActive(false);

        PauseMenu.SettingScreen.SetActive(false);

        GameMenu.AttackButton.onClick.AddListener(() => Controller.PlayerMove());
        GameMenu.SwitchButton.onClick.AddListener(() => Controller.SwitchCharacter());
        GameMenu.PauseButton.onClick.AddListener(() => Controller.Resume());

        PauseMenu.ResumeButton.onClick.AddListener(() => Controller.Resume());
        PauseMenu.SettingButton.onClick.AddListener(() => Controller.Setting());
        PauseMenu.RestartButton.onClick.AddListener(() => Controller.Restart());
        PauseMenu.MenuButton.onClick.AddListener(() => Controller.ToMenu());

        PauseMenu.BackButton.onClick.AddListener(() => Controller.Setting());

    }

}
