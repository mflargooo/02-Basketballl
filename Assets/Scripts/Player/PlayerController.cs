using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 30f; 
    [SerializeField] private float rotateSpeedMultiplier = 10f;
    [Min(min: 1)]
    [SerializeField] private float shootPower = 1;

    [SerializeField] private PlayerStats ps;
    [SerializeField] private GameObject hoop;
    [SerializeField] private GameObject ballPrefab;
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
        else rb.transform.Rotate(Vector3.zero);

        rb.velocity = input.normalized * moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && ps.eggCt > 0)
        {
            ShootAt();
            UIManager.UpdateEggs(ps.id, --ps.eggCt);
        }
    }

    void ShootAt()
    {
        Projectile ball = Instantiate(ballPrefab, transform.position, transform.rotation).GetComponent<Projectile>();
        ball.LaunchAt(ps.id, hoop, shootPower);
    }
}

