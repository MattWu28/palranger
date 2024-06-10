using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject battleManager;
    public GameObject battleUI;
    public GameObject[] emotes;
    public Vector3 playerPosition;
    public GameObject playerPal;
    public GameObject encounteredMonster;
    public Material[] battleTransitionMaterials;
    public Comp_Manager compManager;
    private GameObject[] monsters;
    private Material currentMaterial;
    private GameObject mainCamera;
    private SimpleBlit simpleBlitScript;
    private bool isTransitioning = false;
    private bool isPaused = false;
    public AudioSource audioSource;

    // Singleton pattern to ensure GameManager persists between scenes
    public static GameManager instance;

    private void Awake()
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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player.transform.position = new Vector2(0,0);
        if(playerPal != null){
            playerPal.transform.position = new Vector2(0,0);
        }
        
    }

    void Start()
    {
        monsters = GameObject.FindGameObjectsWithTag("Monster");
        audioSource.Play();
        player.transform.position = new Vector2(0,0);
    }

    private void ChooseTransition()
    {
        mainCamera = GameObject.Find("Main Camera");

        if (mainCamera != null)
        {
            simpleBlitScript = mainCamera.GetComponent<SimpleBlit>();
            if (simpleBlitScript != null)
            {
                int index = Random.Range(0, battleTransitionMaterials.Length);
                currentMaterial = battleTransitionMaterials[index];
                simpleBlitScript.SetTransitionMaterial(currentMaterial);
            }
            else
            {
                Debug.LogWarning("SimpleBlit script not found on Main Camera.");
            }
        }
        else
        {
            Debug.LogWarning("Main Camera not found in the scene.");
        }
    }

    IEnumerator TransitionIn(GameObject monster)
    {
        isTransitioning = true;
        ChooseTransition();
        // Increase cutoff from 0 to 1
        float timer = 0f;
        float transitionDuration = 1f;
        while (timer < transitionDuration)
        {
            float t = timer / transitionDuration;
            float cutoff = Mathf.Lerp(0f, 1f, t);

            // Set the cutoff value to the transition material
            currentMaterial.SetFloat("_Cutoff", cutoff);

            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure the cutoff value is exactly 1 when the coroutine finishes
        currentMaterial.SetFloat("_Cutoff", 1f);
        currentMaterial.SetFloat("_Cutoff", 0f);
        StartCoroutine(TransitionOut());
    }

    IEnumerator TransitionOut()
    {
        float timer = 0f;
        float transitionDuration = 1f;
        
        while (timer < transitionDuration)
        {
            float t = timer / transitionDuration;
            float cutoff = Mathf.Lerp(1f, 0f, t);

            // Set the cutoff value to the transition material
            currentMaterial.SetFloat("_Cutoff", cutoff);

            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure the cutoff value is exactly 0 when the coroutine finishes
        currentMaterial.SetFloat("_Cutoff", 0f);

        isTransitioning = false;
    }

    public void LoadBattle(GameObject monster)
    {
        if(!isTransitioning){
            encounteredMonster = monster;
            player.SetActive(false);
            if (playerPal) {
                playerPal.SetActive(false);
            }
            battleManager.SetActive(true);
            battleUI.SetActive(true);
            encounteredMonster.GetComponent<Slime>().currentState = Slime.State.battling;
            StartCoroutine(TransitionIn(encounteredMonster));
            audioSource.Stop();
        }
    }

    public void ExitBattle(bool wasCaptured)
    {
        player.SetActive(true);
        if (playerPal) {
            playerPal.SetActive(true);
        }
        battleManager.SetActive(false);
        battleUI.SetActive(false);
        if(!isTransitioning){
            StartCoroutine(TransitionOut());
        }
        if(wasCaptured){
            StartCoroutine(ShowEmote(emotes[0]));
            compManager.updateCompanion(encounteredMonster.name);
            Destroy(encounteredMonster);
        }else{
            encounteredMonster.GetComponent<Slime>().currentState = Slime.State.idle;
        }
        audioSource.Play();
    }

    public GameObject GetPal()
    {
        return playerPal;
    }

    public void SetPal(GameObject pal)
    {
        playerPal = pal;
        DontDestroyOnLoad(playerPal);
    }

    public void PauseGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }

    private IEnumerator ShowEmote(GameObject emote)
    {
        GameObject newEmote = Instantiate(emote);
        yield return new WaitForSeconds(8f);
        Destroy(newEmote);
    }
}