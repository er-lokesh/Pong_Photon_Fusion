using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallMovement : NetworkBehaviour
{
    [SerializeField] private float ballSpeed = 1f;
    [SerializeField] private float initialSpeed = 10f;
    [SerializeField] private float speedIncrease = 0.25f;

    [SerializeField] private TMP_Text teamAScoreTxt;
    [SerializeField] private TMP_Text teamBScoreTxt;

    private int hitCounter;
    private NetworkRigidbody2D nrb;
    //private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        nrb = GetComponent<NetworkRigidbody2D>();
        //rb = GetComponent<Rigidbody2D>();
        Invoke("StartBall", 2f);
    }

    public override void FixedUpdateNetwork()
    {
        nrb.Rigidbody.velocity = Vector2.ClampMagnitude(nrb.Rigidbody.velocity, initialSpeed + (speedIncrease * hitCounter));
        //rb.velocity = Vector2.ClampMagnitude(rb.velocity, initialSpeed + (speedIncrease * hitCounter)) * Runner.DeltaTime * ballSpeed;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    rb.velocity = Vector2.ClampMagnitude(rb.velocity, initialSpeed + (speedIncrease * hitCounter));
    //}

    private void StartBall()
    {
        nrb.Rigidbody.velocity = new Vector2(-1, 0) * (initialSpeed + (speedIncrease * hitCounter));
    }

    private void ResetBall()
    {
        nrb.Rigidbody.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        hitCounter = 0;
        Invoke("StartBall", 2f);
    }

    private void PlayerBounce(Transform myObject)
    {
        hitCounter++;

        Vector2 ballPos = transform.position;
        Vector2 playerPos = myObject.position;

        float xDirection, yDirection;

        xDirection = transform.position.x > 0 ? -1 : 1;

        Debug.Log($"ballPos.y ->{ballPos.y - playerPos.y} , bound -> {myObject.GetComponent<Collider2D>().bounds.size.y}");

        yDirection = (ballPos.y - playerPos.y); // / myObject.GetComponent<Collider2D>().bounds.size.y;
        if (yDirection == 0)
            yDirection = 0.25f;

        nrb.Rigidbody.velocity = new Vector2(xDirection, yDirection) * (initialSpeed + (speedIncrease * hitCounter));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            PlayerBounce(collision.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(transform.position.x > 0)
            teamAScoreTxt.text = (int.Parse(teamAScoreTxt.text) + 1).ToString();
        else
            teamBScoreTxt.text = (int.Parse(teamBScoreTxt.text) + 1).ToString();
        ResetBall();
    }
}
