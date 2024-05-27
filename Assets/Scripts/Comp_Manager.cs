using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Comp_Manager : MonoBehaviour
{
    public GameObject GreenSlime;
    public GameObject RedSlime;
    private GameObject currentCompanion;
    public Canvas DialogueBox;
    public GameObject popUpBox;
    private TMP_Text popUpText;
    private bool wait;
    private bool yes;

    // Start is called before the first frame update
    void Start()
    {
        wait = true;
    }

    // Update is called once per frame
    void Update()
    {

    }


    private IEnumerator updater(string companionName){
        popUpBox.SetActive(true);
        Debug.Log("PopUp");
        Time.timeScale = 0f;

        while(wait){
            Debug.Log("Waiting");
            yield return new WaitForSecondsRealtime(0.1f);
            Debug.Log("Done Waiting");
        }

        if(companionName=="GreenSlime"){
            if(yes){
                Destroy(currentCompanion);
                Debug.Log("green Slime");
                currentCompanion = Instantiate(GreenSlime, transform.position, transform.rotation);
                wait = true;
                Time.timeScale = 1f;
                popUpBox.SetActive(false);
            }
            else{
                wait = true;
                Time.timeScale = 1f;
                popUpBox.SetActive(false);
            }
            
        }

        if(companionName=="RedSlime"){
            if(yes){
                Destroy(currentCompanion);
                Debug.Log("red Slime");
                currentCompanion = Instantiate(RedSlime, transform.position, transform.rotation);
                wait = true;
                Time.timeScale = 1f;
                popUpBox.SetActive(false);
            }
            else{
                wait = true;
                Time.timeScale = 1f;
                popUpBox.SetActive(false);
            }
            
        }
        Debug.Log("EOF");
        yield return null;

    }

    public void updateCompanion(string companionName){

        StartCoroutine(updater(companionName));
        
    }

    public void selectYes(){
        Debug.Log("pressed Yes");
        yes = true;
        wait = false;
    }

    public void selectNo(){
        Debug.Log("pressed no");
        yes = false;
        wait = false;
    }


}
