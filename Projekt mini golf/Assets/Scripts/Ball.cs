using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Skrypt odpowiedzialny za ruch piłki
[RequireComponent(typeof(Rigidbody2D),typeof(CircleCollider2D),typeof(Dots))]
public class Ball : MonoBehaviour {

    public float angle;                                         //Kąt pod którym zostanie wystrzelona piłka. Funkcja jest używana przez skrypt Dots.cs
    [SerializeField] private float minSpeed;                    //Minimalna prędkość z jaką przemieści się piłka
    [SerializeField] private float maxSpeed;                    //Maksymalna prędkość z jaką przemieści się piłka po czym zostanie wypuszczona
    [HideInInspector] public float speed;                       //Prędkość z jaką przemieści się piłka. Jest zależne od "minSpeed", oraz "acceleration". Funkcja jest używana przez skrypt Dots.cs

    [SerializeField] private float acceleration;                //Wartość zwiększająca szybkość z jaką będzie się przemieszczała piłka
    [SerializeField] private float accMultiplier = 1.05f;       //Wartość przez którą będzie przemnożona zmienna "acceleration" z każdym kolejnym poziomem. Zalecane jest pozostanie przy wartościach od 1 do 1.5f
    [SerializeField] private float minGOSpeed = 0.1f;           //Minimalna prędkość przy której niezaliczany jest strzał piłką. Zalecane jest utrzymanie niskich wartości np. 0.1f
    [SerializeField] private KeyCode pressedKey;                //Klawisz odpowiedzialny za wystrzelenie piłki

    private bool isJumped = false;                              //Odpowiada za to czy piłka została wystrzelona
    private bool isPressed = false;                             //Odpowiada za to czy został wciśnięty klawisz, wcześniej używałem po prostu Input.GetKeyUp(), ale po testach zauważyłem, że gra nie zawsze zauważa że klawisz został podniesiony
    private float oldAcceleration;                              //Zapamiętuję wartość "acceleration" w przypadku resetu gry
    private float GOTimer = 1f;                                 //Timer po którym gracz ma game over
    private GameController gc;                                  //Przypisuje skrypt "GameController" do późniejszego użytku
    private Rigidbody2D rb;                                     //Przypisuje Rigidbody2D do późniejszego użytku
    private Dots dots;                                          //Przypisuje skrypt "Dots" do późniejszego użytku

	void Awake()
    {
        speed = minSpeed;
        oldAcceleration = acceleration;
        rb = GetComponent<Rigidbody2D>();
        dots = GetComponent<Dots>();

    }

    private void Start()
    {
        gc = FindObjectOfType<GameController>();  
    }

    void FixedUpdate () {
        BallMove();                                     
	}

    //Funkcja odpowiada za wystrzeliwanie piłki z odpowiednią siłą, oraz za wywoływanie funkcji RenderArc(), oraz DisableDots()
    void BallMove()
    {
        if (Input.GetKey(pressedKey) && !isJumped && speed < maxSpeed)                                          
        {
            speed += acceleration * Time.deltaTime;                                         //Zwiększanie prędkości z jaką będzie się poruszała piłka
            dots.RenderArc();                                                               //Rysowanie kropek pomocniczych, które pokazują łuk po którym się będzie poruszać
            isPressed = true;
        }
        else if (!isJumped && isPressed)
        {
            var forcedir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;    //Przeliczanie kątą, pod którym zostanie wystrzelona piłka

            rb.AddForce(forcedir.normalized * speed,ForceMode2D.Impulse);                   //Wystrzelenie piłki
            dots.DisableDots();                                                             //Wyłączenie kropek pomocniczych
            //Debug.Log("Relased!");
            //Debug.LogFormat("forcedir = {0}; speed = {1}", forcedir, speed);
            isJumped = true;                                                                //Oznaczenie że piłka została wystrzelona, uniemożliwiające ponowne wciśnięcie klawisza
        }
    }

    //Zrestartowanie Skoku, prędkości, oraz zwiększenie mnożnika "acceleration"
    public void ResetJump(bool accelerate)
    {
        isJumped = false;               //Zresetowanie skoku
        isPressed = false;              //Zresetowanie wciśnięcia klawisza
        speed = minSpeed;               //Zresetowanie prędkości
        rb.velocity = Vector2.zero;     //Zresetowanie prędkości piłki na wypadek, gdyby się ona poruszała
        rb.angularVelocity = 0;         //Zresetowanie rotacji piłki
        GOTimer = 1f;
        if(accelerate)
        {
            acceleration *= accMultiplier;  //Zwiększenie mnożnika prędkości
        }
    }

    //Sprawdza, czy przyznać GameOver() za zatrzymanie się piłki poza dołkiem
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && isJumped && rb.velocity.x < minGOSpeed)
        {
            GOTimer -= Time.deltaTime;
            if(GOTimer < 0)
            {
                gc.GameOver();
                ResetAcc();
                GOTimer = 1f;
            }
        }
    }

    //Resetuje narzucone wcześniej przyspieszenie
    public void ResetAcc()
    {
        acceleration = oldAcceleration;
    }
}
