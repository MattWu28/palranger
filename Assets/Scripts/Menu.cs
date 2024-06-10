using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void QuitGame(){
        Debug.Log("quitting");
        Application.Quit();
    }

    public void Play(){
        SceneManager.LoadScene("Overworld");
    }

    public void MainMenu(){
        SceneManager.LoadScene("Main Menu");
    }

    public void Tutorial(){
        SceneManager.LoadScene("Tutorial");
    }

}
