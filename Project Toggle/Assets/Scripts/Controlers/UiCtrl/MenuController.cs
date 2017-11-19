using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    [SerializeField]
    Sprite pauseSprite, playSprite;

    [SerializeField]
    GameObject optionsPanel, gameOverPanel;
    [SerializeField]
    Button pauseButton;


    private int minScoreSize;
    private bool isPaused;
    public bool GameOver { get; set; }
	// Use this for initialization
	void Start () {
        minScoreSize = 15;
        GameOver = false;
        isPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Time.timeScale = 1;
                pauseButton.image.sprite = pauseSprite;
            }
            else
            {
                pauseButton.image.sprite = playSprite;
                Time.timeScale = 0;
            }

            isPaused = !isPaused;
            optionsPanel.SetActive(isPaused);
        }

        // When the game is over
        if (GameOver)
        {
            gameOverPanel.SetActive(true);
            GameObject goScore = GameObject.FindGameObjectWithTag("GameOverScore");
            string score = Time.timeSinceLevelLoad.ToString("0");
            goScore.GetComponent<Text>().text = score;
            goScore.GetComponent<RectTransform>().sizeDelta = new Vector2(System.Math.Max(System.Convert.ToInt32(score), minScoreSize), System.Math.Max(System.Convert.ToInt32(score), minScoreSize));
            Time.timeScale = 0;
        }
    }

    public void OnReloadClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnExitClicked()
    {
        Time.timeScale = 1; // Reset the timeScale to unfreeze the game after loading the new scene
        isPaused = false;
        SceneManager.LoadScene("MenuScene");
    }

    public void OnPauseClick(Button button)
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            button.image.sprite = pauseSprite;
        }
        else
        {
            button.image.sprite = playSprite;
            Time.timeScale = 0;
        }
        
        isPaused = !isPaused;
        optionsPanel.SetActive(isPaused);
    }
}
