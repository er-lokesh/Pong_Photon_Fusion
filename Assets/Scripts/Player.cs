using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private int factor;
    [SerializeField] private float movementSpeed;
    [SerializeField] private NetworkRigidbody2D nrb;
    //private Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        //nrb = GetComponent<NetworkRigidbody2D>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData moveInput))
        {
            nrb.Rigidbody.velocity = moveInput.direction * factor * movementSpeed * Runner.DeltaTime;
        }
    }
}
