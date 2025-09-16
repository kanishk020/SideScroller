using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text highScore, currentScore; // UI scores

    [SerializeField] private Button startReset,red,blue,green; // all Buttons

    [SerializeField] SpriteRenderer indicatorSprite; // box over character
    [SerializeField] Animator charcterAnimator;  // character animator

    [SerializeField] private BgPoolScroller bgScroller; // scroller script object
    [SerializeField] private GameObject gameOver; // gameover Panel

    [SerializeField] Sprite start, reset; // sprites to replace startButton
    
    public bool isGameOVer; // checks if game is over for start button fucntion 

    public string currentColor; // current color selected

    public static GameManager instance; // singleton instance for global acess

    private int score;
    private int hiScore;  // scores for calculation
    private void Awake()
    {
        score = 0;
        if(instance == null)
        {
            instance = this;
        }
        hiScore = PlayerPrefs.GetInt("HiScore", 00);
        gameOver.SetActive(false);
        highScore.text = hiScore.ToString();
    }
    private void Start()
    {
        isGameOVer = false;
        currentColor = "white";
        indicatorSprite.color = Color.white;
    }
    

    private void OnEnable()
    {
        startReset.onClick.AddListener(StartReset);
        red.onClick.AddListener(delegate { SetColour("red",red); SoundManager.instance.PlaySFX("Button"); });
        green.onClick.AddListener(delegate { SetColour("green", green); SoundManager.instance.PlaySFX("Button"); });
        blue.onClick.AddListener(delegate { SetColour("blue", blue); SoundManager.instance.PlaySFX("Button"); });
    }
    private void OnDisable()
    {
        startReset.onClick.RemoveAllListeners();
        red.onClick.RemoveAllListeners();
        green.onClick.RemoveAllListeners();
        blue.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// Sets the color according to button
    /// </summary>
    /// <param name="colour"></param>
    /// <param name="btn"></param>
    void SetColour(string colour,Button btn)
    {
        currentColor = colour;
        indicatorSprite.color = btn.GetComponent<Image>().color;
        StartCoroutine(IndicatorPulse(indicatorSprite.gameObject.transform));
    }
    /// <summary>
    /// Indicator animation
    /// </summary>
    /// <param name="t"> transform of indicator </param>
    /// <returns></returns>
    public IEnumerator IndicatorPulse(Transform t)
    {
        Vector3 originalScale = t.localScale;
        Vector3 targetScale = originalScale * 2;

        float timer = 0f;
        while (timer < 0.2f)
        {
            t.localScale = Vector3.Lerp(originalScale, targetScale, timer / 0.2f);
            timer += Time.deltaTime;
            yield return null;
        }
        
        timer = 0f;
        while (timer < 0.5f)
        {
            t.localScale = Vector3.Lerp(targetScale, originalScale, timer / 0.5f);
            timer += Time.deltaTime;
            yield return null;
        }
        t.localScale = originalScale;
    }
    /// <summary>
    /// Switch Start reset function applied on same button
    /// </summary>
    void StartReset()
    {
        if (isGameOVer)
        {
            ResetGame();
        }
        else
        {
            StartGame();
        }
    }
    /// <summary>
    /// Reset and get highscore if any
    /// </summary>
    void ResetScore()
    {
        highScore.text = PlayerPrefs.GetInt("HiScore", 00).ToString();
        currentScore.text = "00";
    }
    /// <summary>
    /// update score if passed gate
    /// </summary>
    void UpdateScore()
    {
        SoundManager.instance.PlaySFX("Gate");
        score += 1;
        if(score > hiScore)
        {
            hiScore = score;
            PlayerPrefs.SetInt("HiScore",hiScore);
            highScore.text = hiScore.ToString();
        }
        if (score % 4 == 0)
        {
            bgScroller.speed += 1f;
        }
        currentScore.text = score.ToString();
    }
    /// <summary>
    /// Start game with base values and animation
    /// </summary>
    void StartGame()
    {
        SoundManager.instance.PlaySFX("Button");
        ResetScore();
        charcterAnimator.SetFloat("Speed", 2f);
        bgScroller.speed = 4f;
        startReset.gameObject.SetActive(false);
        startReset.image.sprite = reset;
    }
    /// <summary>
    /// Resets game with base values and animations
    /// </summary>
    void ResetGame()
    {
        SoundManager.instance.PlaySFX("Button");
        gameOver.SetActive(false);
        isGameOVer = false;
        score = 0;
        ResetScore();
        charcterAnimator.Rebind();
        charcterAnimator.SetFloat("Speed", 0f);
        startReset.image.sprite = start;
        bgScroller.ResetAllBg();
    }

    
    /// <summary>
    /// public function to be accessed by gate script
    /// </summary>
    public void ScoreUP()
    {
        UpdateScore();
    }
    /// <summary>
    /// Game over with sounds and stops scrolling
    /// </summary>
    private void GameFinished()
    {
        SoundManager.instance.PlaySFX("GameOver");
        bgScroller.speed = 0f;
        charcterAnimator.SetTrigger("Death");
        gameOver.SetActive(true);
        isGameOVer = true;
        startReset.gameObject.SetActive(true);
    }
    /// <summary>
    /// public function to be accessed by gate script
    /// </summary>
    public void GameOver()
    {
        GameFinished();
    }
}
