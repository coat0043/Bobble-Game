using UnityEngine;

public class Bullet : MonoBehaviour
{
    public uint ColorID;

    private bool m_bShoot = false;
    Vector3 m_direction;
    float m_speed;

    public BobbleGrid Grid;

    private bool bCollided = false;
    private float lifeTime = 2.0f;

    private void Update()
    {
        if(m_bShoot)
        {
            transform.position += m_direction * m_speed * Time.deltaTime;
            lifeTime -= Time.deltaTime;

            if(lifeTime < 0) { Destroy(gameObject); }
        }
    }

    public void SetMaterialColor(Color color, uint colorID)
    {
        gameObject.GetComponent<MeshRenderer>().material.color = color;
        ColorID = colorID;
    }

    public void Shoot(Vector3 direction, float speed)
    {
        m_bShoot = true;
        m_direction = direction;
        m_speed = speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (bCollided) { return; }

        if (collision.collider.GetComponent<Bobble>() != null)
        {
            bCollided = true;
            Destroy(gameObject);

            Vector3 direction = transform.position - collision.contacts[0].point;
            Index index = collision.collider.GetComponent<Bobble>().Index;

            if (direction.y < -0.3f) { index.y += 1; }
            else if (direction.y > 0.3f) { index.y -= 1; }
            else if (direction.x < -0.3f) { index.x -= 1; }
            else if (direction.x > 0.3f) { index.x += 1; }

            Grid.UpdateGrid(index, ColorID);
        }


        //Not actual calculation, just rotated the vector by hardcoded 90 degrees.
        else if (collision.collider.GetComponent<Wall>() != null)
        {
            Wall wall = collision.collider.GetComponent<Wall>();

            switch(wall.Side)
            {
                case "Left":
                    m_direction = new Vector3(m_direction.y, -m_direction.x, m_direction.z);
                break;

                case "Right":
                    m_direction = -new Vector3(m_direction.y, -m_direction.x, m_direction.z);
                break;
            }
        }
    }
}
