using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private Rigidbody2D rb;
    //private Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData moveInput))
        {
            rb.velocity = moveInput.direction * movementSpeed;
        }
    }
}
