using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public GameObject crewMenu;
    public GameObject shipMenu;
    public GameObject permMenu;
    public GameObject achievementMenu;
    public GameObject prestigeMenu;
    public GameObject settingsMenu;
    public GameObject idleMenu;
    public GameObject breakdownMenu;

    void Start()
    {
        crewMenu.gameObject.SetActive(true);
        shipMenu.gameObject.SetActive(false);
        permMenu.gameObject.SetActive(false);
        achievementMenu.gameObject.SetActive(false);
        prestigeMenu.gameObject.SetActive(false);
        breakdownMenu.gameObject.SetActive(false);
    }

    public void Ship()
    {
        crewMenu.gameObject.SetActive(false);
        shipMenu.gameObject.SetActive(true);
        permMenu.gameObject.SetActive(false);
    }

    public void Crew()
    {
        crewMenu.gameObject.SetActive(true);
        shipMenu.gameObject.SetActive(false);
        permMenu.gameObject.SetActive(false);
    }

    public void Perm()
    {
        permMenu.gameObject.SetActive(true);
        crewMenu.gameObject.SetActive(false);
        shipMenu.gameObject.SetActive(false);
    }

    public void Achievments()
    {
        if(achievementMenu.gameObject.activeSelf)
            achievementMenu.gameObject.SetActive(false);
        else
        {
            achievementMenu.gameObject.SetActive(true);
            prestigeMenu.gameObject.SetActive(false);
            breakdownMenu.gameObject.SetActive(false);
        }
    }

    public void Prestige()
    {
        if(prestigeMenu.gameObject.activeSelf)
            prestigeMenu.gameObject.SetActive(false);
        else
        {
            prestigeMenu.gameObject.SetActive(true);
            achievementMenu.gameObject.SetActive(false);
            breakdownMenu.gameObject.SetActive(false);
        }
    }

    public void Settings()
    {
        if(settingsMenu.gameObject.activeSelf)
            settingsMenu.gameObject.SetActive(false);
        else
        {
            settingsMenu.gameObject.SetActive(true);
        }
    }

    public void Breakdown()
    {
        if(breakdownMenu.gameObject.activeSelf)
            breakdownMenu.gameObject.SetActive(false);
        else
        {
            breakdownMenu.gameObject.SetActive(true);
            achievementMenu.gameObject.SetActive(false);
            prestigeMenu.gameObject.SetActive(false);
        }
    }

    public void LeaveIdle()
    {
        idleMenu.gameObject.SetActive(false);
    }
}
