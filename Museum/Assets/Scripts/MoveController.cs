using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float sensitivity = 1f;
    
    public float minimumVert = -45.0f;
    public float maximumVert = 45.0f;

    private float _rotationX = 0;
    private CharacterController controller;
    private float playerSpeed = 20.0f;
    private Camera mainCam;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = gameObject.AddComponent<CharacterController>();
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        _rotationX -= Input.GetAxis("Mouse Y") * sensitivity;
        _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);
        float delta = Input.GetAxis("Mouse X") * sensitivity;
        float rotationY = transform.localEulerAngles.y + delta;
        transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
        Vector3 frontBackMove = new Vector3(mainCam.transform.forward.x, 0, mainCam.transform.forward.z);
        Vector3 leftRightMove = new Vector3(frontBackMove.z, 0, -frontBackMove.x);
        controller.Move(frontBackMove * Input.GetAxis("Vertical") * Time.deltaTime * playerSpeed);
        controller.Move(leftRightMove * Input.GetAxis("Horizontal") * Time.deltaTime * playerSpeed);

    }
}
