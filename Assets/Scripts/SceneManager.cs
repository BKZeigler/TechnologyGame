using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton with GRASP "Controller" role
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Loads a scene by name. Centralized entry point.
    /// </summary>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Loads a scene based on tile event type.
    /// </summary>
    public void LoadEventScene(TileEventType eventType)
    {
        switch (eventType)
        {
            case TileEventType.Battle:
                LoadScene("BattleOne"); // for testing
                //string battleScene = BattlePoolManager.Instance.GetNextBattle();
                //LoadScene(battleScene);
                break;

            case TileEventType.TechReward:
                LoadScene("BattleOne"); // For testing
                //LoadScene("TechRewardScene");
                break;

            case TileEventType.Boss:
                LoadScene("BossScene");
                break;

            default:
                Debug.LogWarning("Tile event has no associated scene.");
                break;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MapScene")
            StartCoroutine(DelayedLoadGame());
    }   

    private IEnumerator DelayedLoadGame()
    {
        // Wait one frame so GridManager.Awake() and Start() finish
        yield return null;

        PlayerManager.Instance.LoadGame();
    }
}
