using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public GameObject crewMenu;
    public GameObject shipMenu;
    public GameObject achievementMenu;
    public GameObject prestigeMenu;

    void Start()
    {
        crewMenu.gameObject.SetActive(true);
        shipMenu.gameObject.SetActive(false);
        achievementMenu.gameObject.SetActive(false);
        prestigeMenu.gameObject.SetActive(false);
    }

    public void Ship()
    {
        crewMenu.gameObject.SetActive(false);
        shipMenu.gameObject.SetActive(true);
    }

    public void Crew()
    {
        crewMenu.gameObject.SetActive(true);
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
        }
    }
}
