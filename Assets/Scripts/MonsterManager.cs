using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Store player state
            GameManager.instance.playerPosition = other.gameObject.transform.position;
            // GameManager.instance.playerHealth = collision.gameObject.GetComponent<PlayerHealth>().health;
            GameManager.instance.LoadBattle(gameObject);
        }
    }
}
