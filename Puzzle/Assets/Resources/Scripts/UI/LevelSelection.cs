﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    // Gameobject array to get reference of all the Coins in the scene
    private GameObject[] coins;
    public bool[] Unlocklevel;
    public AchievementManager achievementManager;
    public AchievementLeastRotations achievementLeastRotations;
    private GameObject UIEndScreen;

    public bool[] rewardLevels;

    void Awake()
    {
        Unlocklevel = new bool[SceneManager.sceneCountInBuildSettings - 1];
        rewardLevels = new bool[SceneManager.sceneCountInBuildSettings];
        LoadUnlockedLevel();
    }

    private void Start()
    {
        UnlockNewChapter();
        UIEndScreen = GameObject.Find("/GameManager/UIcanvas/End Screen");
    }

    void Update()
    {
        // check if we are not in the menu
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            // find all the gameobjects with the tag "coin" then load the next scene when they are no more
            coins = GameObject.FindGameObjectsWithTag("Coin");
            if (coins.Length == 0)
            {
                Unlocklevel[SceneManager.GetActiveScene().buildIndex - 1] = true;
                RewardPlayer();
                SaveUnlockedLevel();
                EndScreen();
                AchievementsGestion();
            }
        }
    }

    public void RewardPlayer()
    {
        if (!rewardLevels[SceneManager.GetActiveScene().buildIndex - 1])
            GameObject.FindGameObjectWithTag("User").GetComponent<User>().UpdateUserMoney(100);
        rewardLevels[SceneManager.GetActiveScene().buildIndex - 1] = true;
    }
    public void EndScreen()
    {
        GameObject.Find("/GameManager/LevelManager").SendMessage("StopTimer");
        UIEndScreen.SetActive(true);
    }

    public void AchievementsGestion()
    {
        if (achievementLeastRotations.rotationNumbers < 36 && achievementLeastRotations.achievement == true)
            achievementManager.UnlockAchievement(Achievements.MasterMind);
        if (SceneManager.GetActiveScene().buildIndex == 7)
            achievementManager.UnlockAchievement(Achievements.Chapter1);
        if (SceneManager.GetActiveScene().buildIndex == 11)
            achievementManager.UnlockAchievement(Achievements.Chapter2);
        if (SceneManager.GetActiveScene().buildIndex == 17)
            achievementManager.UnlockAchievement(Achievements.Chapter3);
        if (SceneManager.GetActiveScene().buildIndex == 21)
            achievementManager.UnlockAchievement(Achievements.Chapter4);
        if (SceneManager.GetActiveScene().buildIndex == 28)
            achievementManager.UnlockAchievement(Achievements.Chapter5);
    }

    private string GetLevelName(int chapterNumber, int i)
    {
        string tab = $"C{chapterNumber.ToString()}Level{i.ToString()}";
        return (tab);
    }
    public void LoadUnlockedLevel()
    {
        if (PlayerPrefs.HasKey("C2Level3"))
        {
            int chapterNumber = 1;
            int i = -1;
            for (int level = 0; level <= 27; level++)
            {
                i++;
                Unlocklevel[level] = (PlayerPrefs.GetInt(GetLevelName(chapterNumber, i + 1)) == 1 ? true : false);
                if (level == 6 || level == 10 || level == 16 || level == 20 || level == 27)
                {
                    i = -1;
                    chapterNumber++;
                }
            }
        }
        else
            Debug.Log("No Save");
    }
    public void SaveUnlockedLevel()
    {
        int chapterNumber = 1;
        int i = -1;
        for (int level = 0; level <= 27; level++)
        {
            i++;
            PlayerPrefs.SetInt(GetLevelName(chapterNumber, i + 1), (Unlocklevel[level] ? 1 : 0));
            if (level == 6 || level == 10 || level == 16 || level == 20 || level == 27)
            {
                i = -1;
                chapterNumber++;
            }
        }
        PlayerPrefs.Save();
    }
    public void UnlockNewChapter()
    {
        Unlocklevel[6] = true;
        Unlocklevel[10] = true;
        Unlocklevel[16] = true;
        Unlocklevel[20] = true;
        SaveUnlockedLevel();
    }
    public void GoToNexLevel()
    {

        if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
    public void GoToLevel(int i)
    {
        if (i == -1)
            SceneManager.LoadScene(1);
        else if (Unlocklevel[i] == true)
            SceneManager.LoadScene(i + 2);
    }
    public void UnlockLevelsTest()
    {
        int chapterNumber = 1;
        int i = -1;

        for (int level = 0; level < 27; level++)
        {
            i++;
            if (Unlocklevel[level] == false)
                Unlocklevel[level] = true;
            PlayerPrefs.SetInt(GetLevelName(chapterNumber, i + 1), (Unlocklevel[level] ? 1 : 0));
            Unlocklevel[level] = (PlayerPrefs.GetInt(GetLevelName(chapterNumber, i + 1)) == 1 ? true : false);
            if (level == 6 || level == 10 || level == 16 || level == 20 || level == 27)
            {
                i = -1;
                chapterNumber++;
            }
        }
        SceneManager.LoadScene(0);
    }
}