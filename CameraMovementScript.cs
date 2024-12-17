using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    // mouse direction
    public Transform player;
    public float mouseSensitivity = 2f;
    public bool isMoving = false;
    public GameObject inventory_sprite;
    public GameObject help_obj;

    float cameraVerticalRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    { 
        if(inventory_sprite.activeSelf == false && help_obj.activeSelf == false){ // testing that they are not in inventory
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            // Collect Mouse Input

            float inputX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // Rotate the Camera around its local X axis

            cameraVerticalRotation -= inputY;
            cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
            transform.localEulerAngles = Vector3.right * cameraVerticalRotation;


            // Rotate the Player Object and the Camera around its Y axis

            player.Rotate(Vector3.up * inputX);
        } else{
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}