using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController player;
    public float speed = 10f;
    public float gravity = -9.8f;
    public float groundDistance = 0.4f;
    public Transform groundChecker;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        
        var move = transform.right * x + transform.forward * z;
        player.Move(move * Time.deltaTime * speed);

        velocity.y += gravity * Time.deltaTime;
        player.Move(velocity * Time.deltaTime);


    }
}
