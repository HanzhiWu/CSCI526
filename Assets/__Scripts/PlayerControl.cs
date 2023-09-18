using System;
using UnityEngine;

namespace __Scripts
{
    public class PlayerControl : MonoBehaviour
    {
        public float freezeInterval = 0;
        public float normalSpeed = 0;
        public float flyingSpeed = 0;
        
        
        private CheckPoint _checkPoint;
        private Vector3 _originalScale; // to record the original scale to use in the resize process
        private State _curState; // record the state 
        private bool _isFreeze;
        private DateTime _startFreezeTime; // record the time when get the freezeCollection
        
            

        // Start is called before the first frame update
        void Start()
        {
            var trans = transform;
            _checkPoint = new CheckPoint(trans);
            _originalScale = trans.localScale;
            _curState = State.Normal;
        }

        // Update is called once per frame
        void Update()
        {
            var trans = transform;
            float diminishingSpeed;
            if (_isFreeze)
            {
                TimeSpan span = DateTime.UtcNow - _startFreezeTime;
                if (span.TotalSeconds > freezeInterval)
                {
                    _isFreeze = false;
                }
                else
                {
                    diminishingSpeed = 0;
                }
            }
            switch (_curState)
            {
                case State.Dead:
                    Debug.Log("Dead process and change to Normal");
                    trans.position = _checkPoint.Position;
                    trans.localScale = _checkPoint.Scale;
                    _curState = State.Normal;
                    return;
                case State.Normal:
                    Debug.Log("Normal Speed");
                    break;
                case State.Flying:
                    Debug.Log("Flying Speed");
                    break;
                default:
                    return;
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
                    _curState = State.Dead;
                    break;
                case "Poison":
                    Debug.Log("Player is hit by poison");
                    _curState = State.Dead;
                    break;
                case "CheckPoint":
                    Debug.Log("Player reach the CheckPoint");
                    _checkPoint.DoCheckPoint(transform);
                    other.SetActive(false);
                    break;
                case "Collection-Resize":
                    Debug.Log("Reset to Original Size");
                    transform.localScale = _originalScale;
                    other.SetActive(false);
                    break;
                case "Collection-Freeze":
                    Debug.Log("Stop diminishing");
                    _isFreeze = true;
                    _startFreezeTime = DateTime.UtcNow;
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

    internal enum State
    {
        Normal, Dead, Flying
    }
}