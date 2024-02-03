using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 30f; 
    [SerializeField] private float rotateSpeedMultiplier = 10f; 
    private Rigidbody rb;

    Vector3 input;
    Vector3 rawInput;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rawInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        input = rawInput.magnitude > 0 ? new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) : Vector3.zero;

        if (input.magnitude > 0)
        {
            float angle = Vector3.SignedAngle(transform.forward, input.normalized, transform.up);
            rb.transform.Rotate(Vector3.up * angle * rotateSpeedMultiplier * Time.deltaTime);
        }
        rb.velocity = input.normalized * moveSpeed;

    }
}

