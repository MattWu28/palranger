using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Open : MonoBehaviour
{
    public bool isOpen = false;    


    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision){
        Debug.Log("Collision");
        if(collision.gameObject.tag=="Player" && isOpen)
            {
                SceneManager.LoadScene("House");
            }
    }

}
