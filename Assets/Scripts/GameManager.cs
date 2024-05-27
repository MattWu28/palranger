using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] emotes;
    public Vector3 playerPosition;
    public GameObject encounteredMonster;
    public Material[] battleTransitionMaterials;
    private Material currentMaterial;
    private GameObject mainCamera;
    private GameObject playerPal;
    private SimpleBlit simpleBlitScript;
    private bool isTransitioning = false;
    private bool isPaused = false;

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
    }

    void Start()
    {
        SetMainCamera();
    }

    private void SetMainCamera()
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

    IEnumerator TransitionIn(string sceneName)
    {
        isTransitioning = true;

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

        // Load the next scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the next scene is fully loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        currentMaterial.SetFloat("_Cutoff", 0f);
        SetMainCamera();
        StartCoroutine(TransitionOut(sceneName));
    }

    IEnumerator TransitionOut(string sceneName)
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
        if (!isTransitioning)
        {
            StartCoroutine(TransitionIn("MonsterBattle"));
        }
        encounteredMonster = monster;
        DontDestroyOnLoad(monster);
    }

    public void ExitBattle(bool wasCaptured)
    {
        SceneManager.LoadSceneAsync("MainGame").completed += operation =>
        {
            SetMainCamera();
            if (wasCaptured)
            {
                StartCoroutine(ShowEmote(emotes[0]));
            }
        };
    }

    public GameObject GetPal()
    {
        return playerPal;
    }

    public void SetPal(GameObject pal)
    {
        playerPal = pal;
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
