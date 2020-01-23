using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Skrypt odpowiadający za zdobycie punktu w grze
public class HoleScript : MonoBehaviour {

    private GameController gameController;
    private float TimeToWinData;
    private bool holeInOne = false;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }
    private void Start()
    {       
        TimeToWinData = gameController.TimeToWin;
    }

    private void Update()
    {
        HoleInOne();
    }

    void HoleInOne()
    {
        if (holeInOne)
        {
            if (TimeToWinData > 0)
            {
                //Debug.Log("reducing Time" + TimeToWinData);
                TimeToWinData -= Time.deltaTime;
            }
            else
            {
                //Debug.Log("Score!");
                gameController.AddScore();
                TimeToWinData = gameController.TimeToWin;
                holeInOne = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            holeInOne = true;
        }       
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("No score!");
        TimeToWinData = gameController.TimeToWin;
        holeInOne = false;
    }
}
