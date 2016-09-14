using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Transform initialPoint;
    public float speed;
    public static  float health;
    private AudioSource wallHitSFX;
    private float maxHealth = 100;
    public Image healthBar;
    private Rigidbody2D Player;
    private float sensitivity;
    private bool isTakingDamage;
    public static bool isGameLost;

    private Vector3 gotoPos;
    private float damageMultiplier;
    private float yAccOffset;


    void Start()
    {
       
        yAccOffset = ScreenCarryOverData.yAccOffset;
       
        initialPoint = GameObject.Find("InitialSpawn").transform;
        wallHitSFX = GameObject.Find("WallHitSFX").GetComponent<AudioSource>();
        damageMultiplier = 15f;
        isGameLost = false;
        maxHealth = 100f;
        health = maxHealth;
        isTakingDamage = false;
        sensitivity = ScreenCarryOverData.sensitivity;
        transform.position = initialPoint.position;
        Player = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        if (UIMainManager.canMovePlayer || UIforTutorial.canMovePlayer)
            Player.velocity = new Vector2(Input.acceleration.x * (sensitivity + 6), (Input.acceleration.y + yAccOffset) * (sensitivity + 6));          
       
        
 
        if (isTakingDamage)
        {
            health -= Time.deltaTime * damageMultiplier;
            healthBar.fillAmount = health / maxHealth;
            if (ScreenCarryOverData.isVibrationOn)
                Vibration.Vibrate(10);
        }


        if (Mathf.Abs(Player.velocity.x) > 2.3f)
        {
            if (Player.velocity.x > 0)
                Player.velocity = new Vector2(2.3f, Player.velocity.y);
            else
                Player.velocity = new Vector2(-2.3f, Player.velocity.y);
        }
        if (Mathf.Abs(Player.velocity.y) > 2.3f)
        {
            if (Player.velocity.y > 0)
                Player.velocity = new Vector2(Player.velocity.x, 2.3f);
            else
                Player.velocity = new Vector2(Player.velocity.x, -2.3f);
        }

        if (health <= 0 && !isGameLost)
        {
            isGameLost = true;
            Vibration.Cancel();     
        }
        if (UIMainManager.canMovePlayer || UIforTutorial.canMovePlayer)
        {
            if (Input.GetKey(KeyCode.W))
            {
                Player.AddForce(new Vector2(Player.velocity.x, speed * 8f), ForceMode2D.Force);
            }
            if (Input.GetKey(KeyCode.A))
            {
                Player.AddForce(new Vector2(-speed * 8f, Player.velocity.y), ForceMode2D.Force);
            }
            if (Input.GetKey(KeyCode.D))
            {
                Player.AddForce(new Vector2(speed * 8f, Player.velocity.y), ForceMode2D.Force);
            }
            if (Input.GetKey(KeyCode.S))
            {
                Player.AddForce(new Vector2(Player.velocity.x, -speed * 8f), ForceMode2D.Force);
            }
        }             

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Maze" || other.tag == "Obstacle" || other.tag == "Enemy")
        {
            isTakingDamage = true;
            if (!wallHitSFX.isPlaying)
                wallHitSFX.Play();
            if (other.tag == "Obstacle")
                damageMultiplier = LightningObstacle.damageCaused;
            else if (other.tag == "Maze")
                damageMultiplier = 15f;
            else if (other.tag == "Enemy")
                damageMultiplier = 25f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Maze" || other.tag == "Obstacle" || other.tag == "Enemy")
        {
            isTakingDamage = true;
            wallHitSFX.Play();
            if (other.tag == "Obstacle")
                damageMultiplier = LightningObstacle.damageCaused;
            else if (other.tag == "Maze")
                damageMultiplier = 15f;
            else if (other.tag == "Enemy")
                damageMultiplier = 25f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Maze" || other.tag == "Obstacle" || other.tag == "Enemy")
        {
            
            isTakingDamage = false;
            if (wallHitSFX.isPlaying)
                wallHitSFX.Pause();
            else if (other.tag == "Obstacle")
                damageMultiplier = LightningObstacle.damageCaused;
            else if (other.tag == "Maze")
                damageMultiplier = 15f;
            else if (other.tag == "Enemy")
                damageMultiplier = 25f;            
        }
    }
}
    