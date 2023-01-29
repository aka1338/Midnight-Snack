using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public InputAction playerControls;
    public Vector2 moveDirection = Vector2.zero;
    public float moveSpeed = 5f;

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        moveDirection = playerControls.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed * Time.deltaTime,
                                  moveDirection.y * moveSpeed * Time.deltaTime);
    }
}
