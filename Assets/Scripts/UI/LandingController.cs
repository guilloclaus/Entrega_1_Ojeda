using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LandingController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LodadScena()
    {
        SceneManager.LoadScene("Level_one");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
