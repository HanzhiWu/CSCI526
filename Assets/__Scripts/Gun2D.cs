using UnityEngine;

namespace __Scripts
{
    public class Gun2D : MonoBehaviour
    {
        // Start is called before the first frame update
        public Transform bulletSpawnPoint;
        public GameObject bulletPrefab;
        public float bulletSpeed;
        public float moveSpeed;
        public float Ymax;
        public float Ymin;
        public float spawnInterval;
        private bool movingUp = true;
        private float bulletTimer = 0f;

        void Update()
        {
            bulletTimer += Time.deltaTime;

            if (bulletTimer >= spawnInterval)
            {
                var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                bullet.GetComponent<Rigidbody2D>().velocity = (-1) * bulletSpawnPoint.right * bulletSpeed;
                bulletTimer = 0f;
            }

            Vector2 currentPosition = transform.position;

            if (movingUp)
            {
                currentPosition.y += moveSpeed * Time.deltaTime;
                if (currentPosition.y >= Ymax)
                {
                    movingUp = false;
                }
            }
            else
            {
                currentPosition.y -= moveSpeed * Time.deltaTime;
                if (currentPosition.y <= Ymin)
                {
                    movingUp = true;
                }
            }
            transform.position = currentPosition;
        }

        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    Debug.Log("collision occured");
        //    movingUp = !movingUp;
        //}
    }
}
