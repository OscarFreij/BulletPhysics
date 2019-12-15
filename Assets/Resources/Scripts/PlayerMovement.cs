using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Vector3 headAngle;
    public Vector3 headEuler;
    public float xSensitivity = 1;
    public bool xSensitivityIsInverted = false;
    public float ySensitivity = 1;
    public bool ySensitivityIsInverted = false;
    public float speed = 4.0f;
    public float runModifier = 1.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 9.82f;
    private CharacterController characterController;

    public Vector3 MovementVector = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        headAngle = Vector3.zero;
        headEuler = Vector3.zero;
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");


        transform.eulerAngles = new Vector3(0,transform.eulerAngles.y + (mouseX * xSensitivity),0);
        transform.Find("Head").transform.eulerAngles = new Vector3(transform.Find("Head").transform.eulerAngles.x + (mouseY * ySensitivity)*-1,transform.eulerAngles.y,0);

        headAngle.Set(transform.Find("Head").transform.rotation.x, transform.rotation.y, transform.rotation.z);
        headEuler.Set(transform.Find("Head").transform.eulerAngles.x, transform.eulerAngles.y,0);



        GameObject.Find("Canvas").transform.Find("Vector").transform.Find("X").GetComponent<Text>().text = Math.Round(Mathf.Cos(headEuler.y * Mathf.PI / 180), 2).ToString();
        GameObject.Find("Canvas").transform.Find("Vector").transform.Find("Y").GetComponent<Text>().text = Math.Round(Mathf.Sin(headEuler.y * Mathf.PI / 180), 2).ToString();

        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes


            MovementVector.z = Mathf.Cos(headEuler.y * Mathf.PI / 180) * Input.GetAxis("Vertical");
            MovementVector.x = Mathf.Sin(headEuler.y * Mathf.PI / 180) * Input.GetAxis("Vertical");

            MovementVector.z += Mathf.Sin(headEuler.y * Mathf.PI / -180) * Input.GetAxis("Horizontal");
            MovementVector.x += Mathf.Cos(headEuler.y * Mathf.PI / -180) * Input.GetAxis("Horizontal");


            MovementVector.x *= speed;
            MovementVector.z *= speed;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                MovementVector.x *= speed * runModifier;
                MovementVector.z *= speed * runModifier;
            }
            else
            {
                MovementVector.x *= speed;
                MovementVector.z *= speed;
            }

            if (Input.GetButton("Jump"))
            {
                MovementVector.y = jumpSpeed;
            }
        }
        MovementVector.y -= gravity * Time.deltaTime;
        characterController.Move(MovementVector * Time.deltaTime);
    }
}
