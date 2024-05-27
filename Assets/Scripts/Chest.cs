using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public bool isOpen;
    public Animator animator;
    public GameObject key;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag=="Player" && isOpen == false){
            StartCoroutine(OpenAnimation());
        }
    }

    private IEnumerator OpenAnimation(){
        isOpen = true;
        animator.SetBool("Opening", true);
        yield return new WaitForSecondsRealtime(0.5f);
        key.SetActive(true);
        yield return new WaitForSecondsRealtime(2.5f);
        key.SetActive(false);
        animator.SetBool("isOpen", true);
    }
}
