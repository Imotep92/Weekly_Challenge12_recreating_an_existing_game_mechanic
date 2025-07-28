using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PlayerControllerScript : MonoBehaviour
{
    public static PlayerControllerScript _PlayerController;

    #region character movement variables
    //This gives us the ability to manipulate the FPS Camera
    private Camera _playerCamera;

    //This is how fast the player moves
    [SerializeField] float _movementSpeed = 6f;

    //This is how high the player jumps
    [SerializeField] float jumpForce = 7f;

    //This is the gravity being applied to the player when they are not on the ground 
    [SerializeField] float _gravity = 9.8f;

    [SerializeField] float _doubleJumpMultiplier;

    //This is how sensitive the camera movement is based on the mouse input
    public float _mouseSensitivity = 2f;

    //This is how high or low the player can look
    [SerializeField] float _lookXLimit = 45f;

    //This stores the X rotation of the camera
    float rotationX = 0;

    //This represents the direction the player is moving in any given point
    Vector3 _moveDirection;

    private bool _canDoubleJump = false;


    //Gives us access to the player's character controller
    CharacterController _characterController;
    #endregion

    //Called before start
    void Awake()
    {
        _PlayerController = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerCamera = Camera.main;
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * _mouseSensitivity);

        //Rotating the player camera in the X axis
        rotationX += -Input.GetAxis("Mouse Y") * _mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX, -_lookXLimit, _lookXLimit);
        _playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);


        //Checks if the player is on the ground
        if (_characterController.isGrounded)
        {
            //Getting the player inputs
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            //Preserving the Y velocity of the player
            float movementDirectionY = _moveDirection.y;

            //combining the player's local directions with player inputs
            _moveDirection = (horizontalInput * transform.right) + (verticalInput * transform.forward);

            _canDoubleJump = true;


            // //Jumping mechanic
            if (Input.GetButtonDown("Jump"))
            {
                _moveDirection.y = jumpForce; // player jumps into the air
                Debug.Log("Player has jumped and can double jump");

            }
            else
            {
                _moveDirection.y = movementDirectionY;
                Debug.Log("Player is on ground");

                _canDoubleJump = false;
            }
        }
        else
        {
             if (Input.GetButtonDown("Jump") && _canDoubleJump)
                {
                    _moveDirection.y = jumpForce * _doubleJumpMultiplier; // player jumps into the air again
                    Debug.Log("Player has double jumped");

                    _canDoubleJump = false;
                }
            _moveDirection.y -= _gravity * Time.deltaTime; // Player falls
            
        }
        //Moves the character based on inputs
        _characterController.Move(_moveDirection * _movementSpeed * Time.deltaTime);
    }

}




