using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Prosty skrypt, który odpowiada za Wywołanie funkcji GameOver() w przypadku wypadnięcia poza mape
public class BorderRip : MonoBehaviour {

    private GameController gameController;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            gameController.GameOver();
        }
    }
}
