using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingScript : MonoBehaviour
{
	public GameObject prefab;
	private float RateofSpawnz = 15f;
	private float nextSpawn = 15f;
	public float timeLeft;
	public Text winText;
	public Text timerText;
    // Use this for initialization
    public float speed = 100;
    private Rigidbody rb;

    int streak_red = 0;
    int streak_blue = 0;

    int score;
    [SerializeField]
    Text score_txt;

    const string KEY_PLAYER_SCORE = "player_score";
    int[] RedBallRewaedPoint = { 15, 30, 45 };
    int[] BlueBallRewaedPoint = { 20, 40, 60 };
    void Start()
    {
        rb = GetComponent<Rigidbody>();
		timeLeft = 60f;
		updateScore (0);
    }
   
    void FixedUpdate()
    {
		
        // Below are moving concept you can move player as required

        if (Input.GetKey(KeyCode.UpArrow))
        {
            //rb.AddForce(Vector3.forward * Time.deltaTime * speed);
            rb.velocity = Vector3.forward * Time.deltaTime * speed;
            //rb.MovePosition(Vector3.forward * Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            // rb.MovePosition(Vector3.back * Time.deltaTime * speed);
           // rb.AddForce(Vector3.back * Time.deltaTime * speed);
            rb.velocity = Vector3.back * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //  rb.MovePosition(Vector3.left * Time.deltaTime * speed);
           // rb.AddForce(Vector3.left * Time.deltaTime * speed);
            rb.velocity = Vector3.left * Time.deltaTime * speed;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // rb.AddForce(Vector3.right * Time.deltaTime * speed);
            // rb.MovePosition(Vector3.right * Time.deltaTime * speed);
             rb.velocity = Vector3.right * Time.deltaTime * speed;
        }
			
			timeLeft -= Time.deltaTime;

		if (timeLeft < 0f) 
		{
			timeLeft = 0;
			Destroy (gameObject);
			winText.text = "Congratulation Your Gain  " + score.ToString ();
		} 

		
			UpdateText ();
    }
	void Update()
	{
		Spawn ();

	}
		
	void UpdateText()
	{
		timerText.text = "Time Left : " + Mathf.RoundToInt (timeLeft);

	}
	private void Spawn()
	{
		if (Time.time > nextSpawn)
		{
			
			nextSpawn = Time.time + RateofSpawnz;
			Vector3 pos = new Vector3 (Random.Range(-3f,2f),Random.Range(0f,0f), Random.Range(-2f,2.7f));
			pos = transform.TransformPoint (pos * 0.5f);
			Instantiate (prefab, pos, Quaternion.identity);
		}
	}

    BallColor DefaultBallColor;
    int ballIndex = 0;
	void OnTriggerEnter(Collider col)
	{

        if (col.gameObject.GetComponent<Ball>().color == BallColor.RED)
		{
            if (streak_red < 3)
			{
                score = score+(RedBallRewaedPoint[streak_red]);
                streak_red ++;
            }
            else
            {
                streak_red = 0;
                score = score + (RedBallRewaedPoint[streak_red]);
            }
        }
		else if(col.gameObject.GetComponent<Ball>().color==BallColor.BLUE)
		{
            if (streak_red < 3)
            {
                score = score + (BlueBallRewaedPoint[streak_red]);
                streak_red++;
            }
            else
            {
                streak_red = 0;
                score = score + (BlueBallRewaedPoint[streak_red]);
            }
        }

        if (ballIndex == 0)
        {
            DefaultBallColor = col.gameObject.GetComponent<Ball>().color;
            Debug.Log("Set DefaultBallColor:::" + DefaultBallColor.ToString());
            updateScore(score);
        }
        else
        {
            if (DefaultBallColor != col.gameObject.GetComponent<Ball>().color)
            {
                Debug.Log("Score Reset:::" + DefaultBallColor.ToString());
                updateScore(0);
                streak_red = 0;
                DefaultBallColor = col.gameObject.GetComponent<Ball>().color;
                Debug.Log("Set DefaultBallColor:::" + DefaultBallColor.ToString());
            }
            else
			{
                updateScore(score);
            }
        }
	    Destroy(col.gameObject);
        ballIndex++; 
	}

    private void DestroyBall(GameObject ball)
	{
        if (ball != null)
            Destroy(ball);
 	}

    void updateScore(int score)
    {
        this.score = score;
        PlayerPrefs.SetInt(KEY_PLAYER_SCORE, score);
        score_txt.text = "Score : " +score.ToString();
    }
}
public enum BallColor
{
RED,
BLUE
}

	