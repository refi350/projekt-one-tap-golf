using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Główny skrypt odpowiadający za logike gry
public class GameController : MonoBehaviour
{

    [SerializeField] private GameObject ball;                       //GameObject piłki. Niezbędne dla "ballRef"
    [SerializeField] private float minPosBall;                      //Minimalna pozycja x dla piłki
    [SerializeField] private float maxPosBall;                      //Maksymalna pozycja x dla piłki
    [Space]
    [SerializeField] private GameObject hole;                       //GameObject dołka w grze. Niezbędne do ustalenia pozycji dołka, oraz kolizji(dołek w grze zawiera większość Collider2D)
    [SerializeField] private Collider2D holeWinZone;                //Trigger odpowiedzialny za wygraną w grze
                     public float TimeToWin;                        //Czas(liczony w sekundach) w dołku, który trzeba spędzić, aby został zaliczony punkt
    [SerializeField] private float minPosHole;                      //Minimalna pozycja x dla dołka
    [SerializeField] private float maxPosHole;                      //Maksymalna pozycja x dla dołka
    [Space]
    [SerializeField] private TextMeshProUGUI scoreText;             //Wynik widoczny w interfejsie w grze
    [SerializeField] private GameObject gameOverScreen;             //GameObject interfejsu "GameOver" wyświetlający koniec gry
    [SerializeField] private TextMeshProUGUI GOScoreText;           //Wynik widoczny w interfejsie "GameOver"
    [SerializeField] private TextMeshProUGUI GOHighScoreText;       //Najlepszy wynik widoczny w interfejsie "GameOver"

    private int score;                                              //Aktualny wynik w grze
    private int highScore = 0;                                          //Aktualny najlepszy wynik
    private Ball ballRef;                                           //Skrót do skryptu piłki. Uzyskuje go przy pomocy GameObject "ball"

    private void Awake()
    {
        ballRef = ball.GetComponent<Ball>();
    }
    void Start()
    {
        gameOverScreen.SetActive(false);
        Randomizer();
        UpdateText(scoreText, score);
        
    }

    //Dodaje punkt i przygotowuje do następnego poziomu
     public void AddScore()
    {
        score++;
        UpdateText(scoreText, score);
        Randomizer();
        ballRef.ResetJump(true);
    }

    //Resetuje grę
     public void ResetGame()
    {
        score = 0;
        UpdateText(scoreText, score);
        gameOverScreen.SetActive(false);
        ball.SetActive(true);
        Randomizer();
        ballRef.ResetJump(false);
    }

    //W przypadku uzyskania rekordu, zapisuje do pliku nowy rekord
    void HighScore()
    {
        if(score > highScore)
        {
            highScore = score;
        }
    }

    //Aktualizuje wynik w interfejsie
    void UpdateText(TextMeshProUGUI usedText,int value,string text = "")
    {
        string returnText = text + value;
        usedText.text = returnText;
    }

    //Kończy grę i wyświetla interfejs odpowiedzialny za koniec gry
    public void GameOver()
    {       
        gameOverScreen.SetActive(true);
        UpdateText(GOScoreText, score, "Score: ");
        HighScore();
        UpdateText(GOHighScoreText, highScore, "Best: ");
        ball.SetActive(false);               //Wyłączenie piłki, by nie spadała cały czas;
        ballRef.ResetAcc();
    }

    //Losuje pozycje piłki i dołka
    void Randomizer()
    {
        ball.transform.position = new Vector2(Random.Range(minPosBall, maxPosBall), -2.37f);
        hole.transform.position = new Vector2(Random.Range(minPosHole, maxPosHole), hole.transform.position.y);
    }   
}
