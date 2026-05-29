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
                string battleScene = BattlePoolManager.Instance.GetNextBattle();
                LoadScene(battleScene);
                break;

            case TileEventType.TechReward:
                LoadScene("TechRewardScene");
                break;

            case TileEventType.Boss:
                LoadScene("BossScene");
                break;

            default:
                Debug.LogWarning("Tile event has no associated scene.");
                break;
        }
    }
}
