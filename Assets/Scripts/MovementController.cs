using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MovementController : MonoBehaviour {
    
    Rigidbody2D rb;
    [SerializeField] int speed;
    float speedMultiplier;
    bool btnPressed;
    [Range(1, 10)]
    [SerializeField] float acceleration;

    bool isWallTouch;
    public LayerMask wallLayer;
    public Transform wallCheckPoint;

    Vector2 relativeTransform;

    public bool isOnPlatform;
    public Rigidbody2D platformRb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        UpdateRelativeTransform();
    }

    private void FixedUpdate()
    {
        UpdateSpeedMultiplier();

        float targetSpeed = speed * speedMultiplier * relativeTransform.x;

        if (isOnPlatform){
            rb.velocity = new Vector2(targetSpeed+platformRb.velocity.x, rb.velocity.y);
        } else {
            rb.velocity = new Vector2(targetSpeed, rb.velocity.y);
        }


        isWallTouch = Physics2D.OverlapBox(wallCheckPoint.position, new Vector2(0.05f, 0.2f), 0, wallLayer);
        if (isWallTouch)
        {
            Flip();
        }
    }

    public void Flip()
    {
        
            transform.Rotate(0, 180, 0);
            UpdateRelativeTransform();

            // Reset the horizontal velocity to allow the player to continue moving after flipping
            rb.velocity = new Vector2(0f, rb.velocity.y);
    }

    void UpdateRelativeTransform()
    {
        relativeTransform = transform.InverseTransformVector(Vector2.one);
    }

    public void Move(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            btnPressed = true;
        }
        else if (value.canceled)
        {
            btnPressed = false;
        }
    }

    void UpdateSpeedMultiplier()
    {
        if (btnPressed && speedMultiplier < 1)
        {
            speedMultiplier += Time.deltaTime * acceleration;
        }
        else if (!btnPressed && speedMultiplier > 0)
        {
            speedMultiplier -= Time.deltaTime * acceleration;
            if (speedMultiplier < 0) speedMultiplier = 0;
        }
    }
}
