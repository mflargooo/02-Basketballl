using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 0f; 
    public float lookSpeed = 0f; 
    private CharacterController controller;
    private float pitch = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed;
        float moveVertical = Input.GetAxis("Vertical") * moveSpeed;
        Vector3 move = transform.right * moveHorizontal + transform.forward * moveVertical;
        controller.Move(move * Time.deltaTime);

        transform.Rotate(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

        pitch -= Input.GetAxis("Mouse Y") * lookSpeed;
        pitch = Mathf.Clamp(pitch, -45.0f, 45.0f);
        Camera.main.transform.localEulerAngles = new Vector3(pitch, transform.eulerAngles.y, 0);
    }
}

