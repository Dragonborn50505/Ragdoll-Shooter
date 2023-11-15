using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GunScript : MonoBehaviour
{
    [Header("Bullets Stats")]
    [SerializeField] private GameObject m_bullet;
    [SerializeField] private float m_shootForce, m_upwardForce;
    
    [Header("Gun Stats")]
    [SerializeField] private float m_timeBetweenShooting, m_spread, m_realoadTime, m_timeBwtweenShots;
    [SerializeField] private int m_magazineSize, m_bulletsPerTap;
    [SerializeField] private bool m_canHoldSpray;
    [SerializeField] private float m_RangeOrWhatIDK = 70f;

    [Header("References")]
    [SerializeField] private Camera m_playerCamera;
    [SerializeField] private Transform m_attackPoint;
    
    private int m_bulletsLeft, m_bulletsShot;
    private bool m_shooting, m_readyToShoot, m_realoading;
    private float m_spreadX;
    private float m_spreadY;
    private KeyCode m_shootKey = KeyCode.Mouse0;
    private KeyCode m_reloadKey = KeyCode.R;
    private Vector3 m_targetPoint;

    private void Awake()
    {
        m_bulletsLeft = m_magazineSize;
        m_readyToShoot = true;
    }

    private void Update()
    {
        playerInput();
    }

    private void playerInput()
    {
        if (m_canHoldSpray)
        {
            m_shooting = Input.GetKey(m_shootKey);
        }
        else
        {
            m_shooting = Input.GetKeyDown(m_shootKey);
        }

        if (m_readyToShoot && m_shooting && !m_realoading && m_bulletsLeft > 0)
        {
            m_bulletsShot = 0;
            Shoot();
        }

        if (Input.GetKeyDown(m_reloadKey) && m_bulletsLeft < m_magazineSize && !m_realoading)
        {
            Reload();
        }
    }

    private void Shoot()
    {
        m_readyToShoot = false;

        Ray ray = m_playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit m_hit;

        if (Physics.Raycast(ray, out m_hit))
        {
            m_targetPoint = m_hit.point;
        }
        else
        {
            m_targetPoint = ray.GetPoint(m_RangeOrWhatIDK);
        }
        Vector3 m_directionWithoutSpread = m_targetPoint - m_attackPoint.position;

        m_spreadX = Random.Range(-m_spread, m_spread);
        m_spreadY = Random.Range(-m_spread, m_spread);

        Vector3 m_directionWithSpread = m_directionWithoutSpread + new Vector3(m_spreadX, m_spreadY, 0);
        GameObject m_currentBullet = Instantiate(m_bullet, m_attackPoint.position, Quaternion.identity);

        m_currentBullet.transform.forward = m_directionWithSpread.normalized;
        
        m_currentBullet.GetComponent<Rigidbody>().AddForce(m_directionWithSpread.normalized * m_shootForce, ForceMode.Impulse);
        m_currentBullet.GetComponent<Rigidbody>().AddForce(m_playerCamera.transform.up * m_upwardForce, ForceMode.Impulse);
        
        m_bulletsLeft--;
        m_bulletsShot++;

        if (m_bulletsShot < m_bulletsPerTap && m_bulletsLeft > 0)
        {
            Invoke("Shoot", m_timeBwtweenShots);
        }
    }
    
    private void ResetShot()
    {
        m_readyToShoot = true;
    }

    private void Reload()
    {
        m_realoading = true;
        Invoke("ReloadDone", m_realoadTime);
    }
    private void ReloadDone()
    {
        m_bulletsLeft = m_magazineSize;
        m_realoading = false;
    }
}