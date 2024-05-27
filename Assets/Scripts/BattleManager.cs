using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public HealthBar monsterHealthBar;
    public GameObject keepPalUI;
    [SerializeField] int monsterMaxHealth;
    private int monsterHealth;
    private LoopCapture loopCapture;

    void Start()
    {
        monsterHealth = monsterMaxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        monsterHealth -= damage;
        monsterHealthBar.UpdateHealthBar(monsterHealth, monsterMaxHealth);
        if (monsterHealth <= 0)
        {
            Debug.Log("Captured!");
            keepPalUI.SetActive(true);
            DestroyMonsters();
        }
    }

    // Move to game manager later to restore player, player position
    public void Run()
    {
        DestroyMonsters();
        GameManager.instance.ExitBattle(false);
    }

    public void Assist()
    {
        if (GameManager.instance.GetPal() == null)
        {
            Debug.Log("You do not have a Pal!");
        }
        else
        {
            Debug.Log("Calling Pal for assistance!");
        }
    }

    private void DestroyMonsters()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject monster in monsters)
        {
            Destroy(monster);
        }
    }

    public void KeepPal()
    {
        Debug.Log("New Pal added to party!");
        keepPalUI.SetActive(false);
        GameManager.instance.ExitBattle(true);
    }

    public void ReleasePal()
    {
        Debug.Log("New Pal has been released");
        keepPalUI.SetActive(false);
        GameManager.instance.ExitBattle(true);
    }
}
