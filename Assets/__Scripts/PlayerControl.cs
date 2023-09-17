using UnityEngine;

namespace __Scripts
{
    public class PlayerControl : MonoBehaviour
    {
        private static CheckPoint _checkPoint;

        // temp
        private static bool _isDead;

        // Start is called before the first frame update
        void Start()
        {
            _checkPoint = new CheckPoint(transform);
        }

        // Update is called once per frame
        void Update()
        {
            if (_isDead)
            {
                transform.position = _checkPoint.Position;
                transform.localScale = _checkPoint.Scale;
                _isDead = false;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            var player = gameObject;
            var other = collision.gameObject;
            switch (collision.gameObject.tag)
            {
                case "Bullet":
                    Debug.Log("Player is hit by bullet");
                    _isDead = true;
                    break;
                case "Poison":
                    Debug.Log("Player is hit by poison");
                    _isDead = true;
                    break;
                case "CheckPoint":
                    Debug.Log("Player reach the CheckPoint");
                    _checkPoint.DoCheckPoint(transform);
                    other.SetActive(false);
                    break;
                default:
                    return;
            }
        }
    }

    internal class CheckPoint
    {
        public Vector3 Scale;
        public Vector3 Position;

        public CheckPoint(Transform transform)
        {
            Scale = transform.localScale;
            Position = transform.position;
            Debug.LogFormat("Initial CheckPoint, scale: {0}, position: {1}", Scale, Position);
        }

        public void DoCheckPoint(Transform transform)
        {
            Scale = transform.localScale;
            Position = transform.position;
            Debug.LogFormat("scale: {0}, position: {1}", Scale, Position);
        }
    }
}