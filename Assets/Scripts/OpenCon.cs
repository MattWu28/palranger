using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCon : MonoBehaviour
{
    public GameObject door;
    public Open house;

    // Start is called before the first frame update
    void Start()
    {
        door.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        door.SetActive(true);
        house.isOpen = true;
    }

}
