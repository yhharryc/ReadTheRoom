using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private PlayerCharacter playerCharacter;
    public PlayerCharacter PlayerCharacter{get{return playerCharacter;}}
    public float CurrentScore = 0;
    [SerializeField]
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        // Find the PlayerCharacter in the scene
        
        playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();

        if (playerCharacter == null)
        {
            Debug.LogError("PlayerCharacter not found in the scene. Make sure it has the tag 'Player'.");
        }
    }

    public void AddScore(float scoreToAdd)
    {
        CurrentScore+=scoreToAdd;
    }

    // This function can be called by SendMessage("OnRestartScene")
    public void OnRestartScene()
    {
        // Suppose scene index 0 is your main menu or initial scene:
        SceneManager.LoadScene(0, LoadSceneMode.Single);

        // Alternatively, if you have a scene name for the "full restart":
        // SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);

        // If you also want to reset certain static variables or singletons, do so here:
        CurrentScore = 0f;
        // Re-initialize anything else as needed
    }
}
