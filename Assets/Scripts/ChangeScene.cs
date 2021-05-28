using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public GameObject crewMenu;
    public GameObject shipMenu;
    public GameObject achievementMenu;

    void Start()
    {
        crewMenu.gameObject.SetActive(true);
        shipMenu.gameObject.SetActive(false);
        achievementMenu.gameObject.SetActive(false);
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
            achievementMenu.gameObject.SetActive(true);
    }
}
