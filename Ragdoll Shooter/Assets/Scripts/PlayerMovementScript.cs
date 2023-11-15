using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float m_moveSpeed;
    [SerializeField] private float m_groundDrag;
    [SerializeField] private float m_jumpForce;
    [SerializeField] private float m_jumpCooldown;
    [SerializeField] private float m_airMultiplier;
    private bool m_readyToJump;
    
    [Header("Ground")]
    [SerializeField] private float m_playerHeight;
    [SerializeField] private LayerMask m_groundLayer;
    private bool m_grounded;

    [SerializeField] private Transform m_orientation;
    private KeyCode jumpKey = KeyCode.Space;
    private float m_horizontalInput;
    private float m_verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        m_readyToJump = true;
    }

    private void Update()
    {
        m_grounded = Physics.Raycast(transform.position, Vector3.down, m_playerHeight * 0.5f + 0.3f, m_groundLayer);
        PlayerInput();
        SpeedControl();
        if (m_grounded)
            rb.drag = m_groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void PlayerInput()
    {
        m_horizontalInput = Input.GetAxisRaw("Horizontal");
        m_verticalInput = Input.GetAxisRaw("Vertical");
        if(Input.GetKey(jumpKey) && m_readyToJump && m_grounded)
        {
            m_readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), m_jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = m_orientation.forward * m_verticalInput + m_orientation.right * m_horizontalInput;
        if(m_grounded)
            rb.AddForce(moveDirection.normalized * m_moveSpeed * 10f, ForceMode.Force);
        else if(!m_grounded)
            rb.AddForce(moveDirection.normalized * m_moveSpeed * 10f * m_airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(flatVel.magnitude > m_moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * m_moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * m_jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        m_readyToJump = true;
    }
}
