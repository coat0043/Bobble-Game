using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private GameObject m_BulletPrefab;
    [SerializeField] private BobbleGrid m_grid;

    [SerializeField] private ColorPool m_ColorPool;
    [SerializeField] private Transform m_previewLocation;

    [SerializeField] private float m_turnAngle;
    [SerializeField] private float m_turnSpeed;
    [SerializeField] private float m_cooldown;

    private Bullet m_NextBullet;
    private float m_cooldownCounter;
    private bool bShoot = true;

    void Start()
    {
        SpawnBulletPreview();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) { Fire(); }
        if(Input.GetKey(KeyCode.A)) { Rotate(m_turnAngle, m_turnSpeed); }
        else if (Input.GetKey(KeyCode.D)) { Rotate(-m_turnAngle, m_turnSpeed); }

        if (!bShoot) { m_cooldownCounter -= Time.deltaTime; }
        if(m_cooldownCounter <= 0) { bShoot = true; }
    }

    void Rotate(float turnAngle, float turnSpeed)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, turnAngle),
                             turnSpeed * Time.deltaTime);
        UpdatePreviewBulletPosition();
    }

    void Fire()
    {
        if(bShoot)
        {
            m_NextBullet.Shoot(m_previewLocation.up, 15.0f);
            bShoot = false;
            m_cooldownCounter = m_cooldown;

            SpawnBulletPreview();
        }
    }

    void SpawnBulletPreview()
    {
        m_NextBullet = GameObject.Instantiate(m_BulletPrefab).GetComponent<Bullet>();
        m_NextBullet.Grid = m_grid;
        m_NextBullet.transform.position = m_previewLocation.position;

        uint randomColorIndex = (uint)Random.Range(0, (int)m_ColorPool.GetPoolSize());
        m_NextBullet.SetMaterialColor(m_ColorPool.GetColor(randomColorIndex), randomColorIndex);
    }

    void UpdatePreviewBulletPosition()
    {
        if (!m_NextBullet) { return; }
        m_NextBullet.transform.position = m_previewLocation.position;
    }
}