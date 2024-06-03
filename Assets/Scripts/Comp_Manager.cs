using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Comp_Manager : MonoBehaviour
{
    public GameObject greenSlime;
    public GameObject redSlime;
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
                currentCompanion = Instantiate(greenSlime, transform.position, transform.rotation);
            } else if (monsterName.Contains("RedSlime")) {
                currentCompanion = Instantiate(redSlime, transform.position, transform.rotation);
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
        Debug.Log("EOF");
        yield return null;
    }

    public void updateCompanion(string monsterName){
        StartCoroutine(updater(monsterName));
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
