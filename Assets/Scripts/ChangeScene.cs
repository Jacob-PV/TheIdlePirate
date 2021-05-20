using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    static ChangeScene instance;

    // void Awake()
    // {
    //     if(instance != null)
    //     {
    //         Destroy(gameObject);
    //     }
    //     else
    //     {
    //         instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    // }

    public void Ship()
    {
        SceneManager.LoadScene("ShipUpgrades");
    }

    public void Crew()
    {
        SceneManager.LoadScene("Crew");
    }
}
