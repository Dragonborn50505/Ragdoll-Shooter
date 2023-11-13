using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float m_cameraSensetivityX;
    [SerializeField] private float m_cameraSensetivityY;
    private float m_RotationX;
    private float m_RotationY;
    private float m_mouseX;
    private float m_mouseY;

    public Transform m_playerOrientation;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        m_mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * m_cameraSensetivityX;
        m_mouseX = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * m_cameraSensetivityY;

        m_RotationY += m_mouseX;
        m_RotationX -= m_mouseY;

        m_RotationX = Mathf.Clamp(m_RotationX, -90f, 90f);

        transform.rotation = Quaternion.Euler(m_RotationX, m_RotationY, 0);
        m_playerOrientation.rotation = Quaternion.Euler(0, m_RotationY, 0);
    }
}
