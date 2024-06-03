using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public HealthBar monsterHealthBar;
    public HealthBar playerHealthBar;
    public GameObject keepPalUI;
    [SerializeField] int monsterMaxHealth;
    [SerializeField] int playerMaxHealth;
    private int monsterHealth, playerHealth;
    private LoopCapture loopCapture;

    // Singleton pattern to ensure GameManager persists between scenes
    public static BattleManager instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        monsterHealth = monsterMaxHealth;
        monsterHealthBar.UpdateHealthBar(monsterHealth, monsterMaxHealth);
    }
    
    //monster take damage
    public void TakeDamage(int damage)
    {
        monsterHealth -= damage;
        monsterHealthBar.UpdateHealthBar(monsterHealth, monsterMaxHealth);
        if (monsterHealth <= 0)
        {
            Debug.Log("Captured!");
            GameManager.instance.ExitBattle(true);
        }
    }

    public void PlayerTakeDamage(int damage)
    {
        playerHealth -= damage;
        playerHealthBar.UpdateHealthBar(playerHealth, playerMaxHealth);
        if (playerHealth <= 0)
        {
            Debug.Log("Defeated!");
            GameManager.instance.ExitBattle(false);
        }
    }

    // Move to game manager later to restore player, player position
    public void Run()
    {
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
}
