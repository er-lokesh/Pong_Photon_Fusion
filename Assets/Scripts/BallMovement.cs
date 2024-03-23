using Fusion;
using System.Collections;
using UnityEngine;

public class BallMovement : NetworkBehaviour
{
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float initialSpeed = 10f;
    [SerializeField] private float speedIncrease = 0.25f;
    [SerializeField] private SpriteRenderer ballImage;

    private int hitCounter;
    private Rigidbody2D rb;
    public float startAngle = 45f;

    public override void Spawned()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(WaitForSpawn());
    }

    private IEnumerator WaitForSpawn()
    {
        rb.velocity = Vector2.zero;
        rb.position = Vector2.zero;

        yield return new WaitForSeconds(startDelay / 2);
        ballImage.enabled = true;
        yield return new WaitForSeconds(startDelay / 2);
        rb.velocity = new Vector2(-1, 0) * (initialSpeed + (speedIncrease * hitCounter));
    }

    public void UpdateRBSpeed()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, initialSpeed + (speedIncrease * hitCounter)); // * speed;
    }

    private void Update()
    {
        if (rb.velocity.magnitude > 0)
            UpdateRBSpeed();
    }

    private void PlayerBounce(Transform myObject)
    {
        hitCounter++;

        Vector2 ballPos = transform.position;
        Vector2 playerPos = myObject.position;

        float xDirection, yDirection;

        xDirection = transform.position.x > 0 ? -1 : 1;
        yDirection = (ballPos.y - playerPos.y); // / myObject.GetComponent<Collider2D>().bounds.size.y;

        if (yDirection == 0)
            yDirection = 0.25f;

        rb.velocity = new Vector2(xDirection, yDirection) * (initialSpeed + (speedIncrease * hitCounter));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            PlayerBounce(collision.transform);
        }
        else if (collision.transform.TryGetComponent(out AddPoints addPoints))
        {
            addPoints.OnHit?.Invoke();
            ballImage.enabled = false;
            StartCoroutine(WaitForSpawn());
        }
    }
}
