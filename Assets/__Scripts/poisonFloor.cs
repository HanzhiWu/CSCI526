using UnityEngine;

namespace __Scripts
{
    public class PoisonFloor : MonoBehaviour
    {
        public string playerObjectName = "player";
        public GameObject targetToDestroy;

        void Start()
        {
            targetToDestroy = GameObject.Find(playerObjectName);
        }
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject == targetToDestroy)
            {
                Destroy(targetToDestroy);
            }
        }
    }
}
