using System;
using System.Collections;
using UnityEngine;

//Skrypt odpowiedzialny za celownik
public class Dots : MonoBehaviour {

    [SerializeField] private int setResolution;     //Ustawia ilość kropek wyświetlanych na ekranie. Nie zmienia to ilości kropek po uruchomieniu gry
    [SerializeField] private GameObject dotItem;    //Prefab kropki

    private float angle;                            //Kąt pod którym wystrzeliwywana jest piłka. Pobierane z "Ball.cs"
    private float radiantAngle;                     //Kąt przeliczony w radianach
    private float speed;                            //Prędkość pod którym wystrzeliwana jest piłka. Pobierane z "Ball.cs"
    private float g;                                //Grawitacja
    private int resolution;                         //Zapisana ilość kropek
    private Ball ball;                              //Skrót do skryptu piłki
    private Vector3[] dot;                          //Pozycje kropek
    private GameObject[] instance;                  //Instancje kropek

	void Awake () {
        resolution = setResolution;
        ball = GetComponent<Ball>();
        g = Mathf.Abs(Physics2D.gravity.y);
        speed = ball.speed;
        angle = ball.angle;
        instance = new GameObject[resolution];
	}

    private void Start()
    {
        GenerateDots();
    }
    
    //Tworzy wszystkie kropki na początku gry i ukrywa je
    void GenerateDots()
    {
        for (int i = 1; i < resolution; i++)
        {
            instance[i] = Instantiate(dotItem, ball.transform.position, Quaternion.identity, ball.transform) as GameObject;
            instance[i].SetActive(false);
        }
    }

    //Aktywuje kropki
    public void EnableDots()
    {
        for (int i = 1; i < resolution; i++)
        {
            instance[i].SetActive(true);
        }
    }

    //Dezaktywuje kropki
    public void DisableDots()
    {
        for (int i = 1; i < resolution; i++)
        {
            instance[i].SetActive(false);
        }
    }

    //Rozmieszczanie punktów na rozmieszczenie 
    public void RenderArc()
    {
        EnableDots();
        speed = ball.speed;
        angle = ball.angle;
        dot = new Vector3[resolution + 1];
        dot = CalculateArcArray();
        for(int i = 1; i < resolution; i++)
        {
            instance[i].transform.position = dot[i] + ball.transform.position;
        }
    }

    // tworzenie tablicy pozycji Vector3 dla RenderArc()
    Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];

        radiantAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (speed * speed * Mathf.Sin(2 * radiantAngle)) / g;

        for(int i= 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t,maxDistance);
        }

        return arcArray;
    }

    //obliczenie pozycji każdego punktu
    Vector3 CalculateArcPoint(float t, float maxDistance)
    {
        float x = t * maxDistance;
        float y = x * Mathf.Tan(radiantAngle) - ((g * x * x) / (2 * speed * speed * Mathf.Cos(radiantAngle) * Mathf.Cos(radiantAngle)));
        return new Vector3(x, y);
    }
}
