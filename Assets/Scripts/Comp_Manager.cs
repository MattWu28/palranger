using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Comp_Manager : MonoBehaviour
{
    public GameObject GreenSlime;
    public GameObject RedSlime;
    public Canvas DialogueBox;
    public GameObject popUpBox;
    private GameObject currentCompanion;
    private TMP_Text popUpText;
    private bool wait;
    private bool yes;
    


    // Start is called before the first frame update
    void Start()
    {
        wait = true;

        if(Comp_Data.hasComp){
            if(Comp_Data.currentComp == "green"){
                currentCompanion = Instantiate(GreenSlime, transform.position, transform.rotation);
            }
            if(Comp_Data.currentComp == "red"){
                currentCompanion = Instantiate(RedSlime, transform.position, transform.rotation);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Comp_Data.hasComp){
            if((currentCompanion.transform.position-GameObject.Find("Player").transform.position).magnitude > 1.3){
                Destroy(currentCompanion);
                if(Comp_Data.currentComp == "green"){
                    currentCompanion = Instantiate(GreenSlime, transform.position, transform.rotation);
                }
                if(Comp_Data.currentComp == "red"){
                    currentCompanion = Instantiate(RedSlime, transform.position, transform.rotation);
                }
            }
        }
    }

    private IEnumerator updater(string monsterName){
        popUpBox.SetActive(true);
        Debug.Log("PopUp");
        Time.timeScale = 0f;

        while(wait){
            Debug.Log("Waiting");
            yield return new WaitForSecondsRealtime(0.1f);
            Debug.Log("Done Waiting");
        }

        if(yes){
            Destroy(currentCompanion);
            if (monsterName.Contains("GreenSlime")) {
                currentCompanion = Instantiate(GreenSlime, transform.position, transform.rotation);
                Comp_Data.currentComp = "green";
            } else if (monsterName.Contains("RedSlime")) {
                currentCompanion = Instantiate(RedSlime, transform.position, transform.rotation);
                Comp_Data.currentComp = "red";
            }
            GameManager.instance.SetPal(currentCompanion);
            wait = true;
            Time.timeScale = 1f;
            popUpBox.SetActive(false);
        }
        else{
            wait = true;
            Time.timeScale = 1f;
            popUpBox.SetActive(false);
        }
        Debug.Log("Done");
        yield return null;
    }

    public void updateCompanion(string companionName){

        StartCoroutine(updater(companionName));
        
    }

    public void selectYes(){
        yes = true;
        wait = false;
    }

    public void selectNo(){
        yes = false;
        wait = false;
    }


}
