using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

// This is CONTEXT for Players state machine
// CONTEXT - stores the persistent state data that is passsed to the active CONCRETE STATES.
// This data is used for their logic and switching between states.
// To understand state machines. Watch first https://www.youtube.com/watch?v=Vt8aZDPzRjI and then
// https://www.youtube.com/watch?v=kV06GiJgFhc
// https://youtu.be/bXNFxQpp2qk?si=H8u4lwnXdm_IRqMP&t=689 to understand how to use inputs / why we use them like this

public class PlayerStateMachine : MonoBehaviour
{
    // Here are all vairiables
        // _ -> stands for private variables
        [Header("Settings")] // makes the header in the inspectore

        // references
        private PlayerInputManager _GetInput;
        public PlayerInput _playerInput;
        private Rigidbody _rb; // use e.g rb.AddForce
        public GameObject model;
        public Animator animator;

        // Player state variables
        private bool _isGrounded = true; // Is the player on the ground?
        private bool _isCrouching = false; // Is the player crouching?
        public float walkSpeed = 5f;
        public float runSpeedultiplier = 1.5f;
        public float jumpForce = 6f; // Speed of the player
        public float gravity = -9.8f; // Gravity force

        // Player input variables
        Vector2 _currentMovementInput; // Current movement input from the player
        Vector3 _currentMovement;
        bool _isMovementPressed;

    private void Awake()
    {
        //_playerInput = GetComponent<PlayerInput>();
        _GetInput = GetComponent<PlayerInputManager>();
        _rb = gameObject.GetComponent<Rigidbody>();
        _playerInput = new PlayerInput();

        /* 
        Callback Functions:
        A callback is a function passed to be executed when a specific event occurs

        Debug: _playerInput.Player.MovementInput.started += context => { Debug.Log(context.ReadValue<Vector2>()); };

        _playerInput.Player.MovementInput.started += context => {}; // Navigates to: CHARACTERCONTROLS -> MOVE ->
        Then tells unity: LISTEN FOR WHEN THE PLAYER STARTS USING THIS ACTION
        */
        
        // Log when movement starts (e.g., joystick or key pressed).
        _playerInput.Player.MovementInput.started += context => { 
            Debug.Log(context.ReadValue<Vector2>());
        };

        // Log when movement stops (e.g., joystick or key released).
        _playerInput.Player.MovementInput.canceled += context => { 
            Debug.Log(context.ReadValue<Vector2>());
        };

        // Log ongoing movement (e.g., joystick or key held down).
        _playerInput.Player.MovementInput.performed += context => { 
            Debug.Log(context.ReadValue<Vector2>());
        }; // For controller input handling.

    }
    void onMovementInput (InputAction.CallbackContext context)
    {
        // This function is called when the player starts using the movement input action.
        // It reads the value of the input and sets the current movement vector accordingly.
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.z = _currentMovementInput.y;
        _isMovementPressed = _currentMovement.x != 0 || _currentMovementInput.y != 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        // Enable the player input actions when the object is enabled,
        // ensuring that input can be processed while the object is active.
        _playerInput.Player.Enable(); 
    }

    void OnDisable()
    {
        // Disable the player input actions when the object is disabled,
        // preventing input from being processed when the object is inactive.
        _playerInput.Player.Disable(); 
    }
}
