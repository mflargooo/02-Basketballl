using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 30f; 
    [SerializeField] private float rotateSpeedMultiplier = 10f; 
    private Rigidbody rb;

    Vector3 input;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        if (input.magnitude > 0)
        {
            float angle = Vector3.SignedAngle(transform.forward, input.normalized, transform.up);
            rb.transform.Rotate(Vector3.up * angle * rotateSpeedMultiplier * Time.deltaTime);
        }
        rb.velocity = input.normalized * moveSpeed * Time.deltaTime;

    }
}

