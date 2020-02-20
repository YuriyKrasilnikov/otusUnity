using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private LoadingLogicPage LoadingLogic;
    private bool GameIsPaused = false;
    public string MenuSceneName;
    private InGameMenu GameMenu;
    private InPauseMenu PauseMenu;
    public Character[] playerCharacters;
    public Character[] enemyCharacters;
    Character currentTarget;
    bool waitingPlayerInput;

    [ContextMenu("Player Move")]
    public void PlayerMove()
    {
        if (waitingPlayerInput)
            waitingPlayerInput = false;
    }

    [ContextMenu("Switch character")]
    public void SwitchCharacter()
    {
        if (waitingPlayerInput)
        {
            var nextTarget = Array.IndexOf(enemyCharacters, currentTarget) + 1;
            var lenEnemy = enemyCharacters.Length - 1;
            currentTarget.GetComponentInChildren<TargetIndicator>(true).gameObject.SetActive(false);
            while (nextTarget > lenEnemy || enemyCharacters[nextTarget].IsDead())
            {
                nextTarget++;
                if (nextTarget > lenEnemy)
                    nextTarget = 0;
            }
            currentTarget = enemyCharacters[nextTarget];
            currentTarget.GetComponentInChildren<TargetIndicator>(true).gameObject.SetActive(true);
        }
    }

    private void Awake()
    {
        LoadingLogic = GameObject.Find("LoadingLogic").GetComponent<LoadingLogicPage>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GameLoop());
    }

    public void SetMenu(InGameMenu gameMenu, InPauseMenu pauseMenu)
    {
        GameMenu = gameMenu;
        PauseMenu = pauseMenu;
    }

    public bool Resume()
    {
        if (GameIsPaused) {
            PauseMenu.gameObject.SetActive(false);
            GameMenu.gameObject.SetActive(true);
            Time.timeScale = 1f;
            GameIsPaused = false;
            return GameIsPaused;
        }
        GameMenu.gameObject.SetActive(false);
        PauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        return GameIsPaused;
    }

    public void Restart()
    {
        StopAllAnimator();
        StopAllCharacter();
        LoadingLogic.LoadScene(SceneManager.GetActiveScene().name, new GameObject[] { GameMenu.gameObject, PauseMenu.gameObject });
    }

    public void ToMenu()
    {
        StopAllAnimator();
        StopAllCharacter();
        LoadingLogic.LoadScene(MenuSceneName, new GameObject[] { GameMenu.gameObject, PauseMenu.gameObject });
    }

    private void StopAllAnimator()
    {
        var allAnimations = FindObjectsOfType<Animator>();
        foreach (var animation in allAnimations)
        {
            animation.speed = 0;
        }
    }

    private void StopAllCharacter() {
        var allCharacters = FindObjectsOfType<Character>();
        foreach (var character in allCharacters)
        {
            character.SetState(Character.State.Death);
        }

    }

    void PlayerWon()
    {
        Resume();
        PauseMenu.ResumeButton.gameObject.SetActive(false);
        PauseMenu.WinText.SetActive(true);
        Debug.Log("Player won");
    }

    void PlayerLost()
    {
        Resume();
        PauseMenu.ResumeButton.gameObject.SetActive(false);
        PauseMenu.LostText.SetActive(true);
        Debug.Log("Player lost");
    }

    Character FirstAliveCharacter(Character[] characters)
    {
        foreach (var character in characters)
        {
            if (!character.IsDead())
            {
                return character;
            }
        }
        return null;
    }

    bool CheckEndGame()
    {
        if (FirstAliveCharacter(playerCharacters) == null)
        {
            PlayerLost();
            return true;
        }

        if (FirstAliveCharacter(enemyCharacters) == null)
        {
            PlayerWon();
            return true;
        }

        return false;
    }

    IEnumerator GameLoop()
    {
        while (!CheckEndGame()) {
            foreach (var player in playerCharacters)
            {
                if (player.IsDead())
                    continue;

                currentTarget = player.target;

                if (currentTarget.IsDead() || currentTarget == null)
                    currentTarget = FirstAliveCharacter(enemyCharacters);

                currentTarget.GetComponentInChildren<TargetIndicator>(true).gameObject.SetActive(true);

                waitingPlayerInput = true;
                while (waitingPlayerInput)
                    yield return null;

                currentTarget.GetComponentInChildren<TargetIndicator>().gameObject.SetActive(false);
                player.target = currentTarget;

                player.AttackEnemy();
                while (!player.IsIdle())
                    yield return null;
            }

            foreach (var enemy in enemyCharacters)
            {
                if (enemy.IsDead())
                    continue;

                currentTarget = enemy.target;

                if (currentTarget.IsDead() || currentTarget == null)
                    currentTarget = FirstAliveCharacter(playerCharacters);

                enemy.target = currentTarget;

                enemy.AttackEnemy();
                while (!enemy.IsIdle())
                    yield return null;
            }
        }
    }
}
