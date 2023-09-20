using System;
using UnityEngine;
using UnityEngine.UI;

namespace __Scripts
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private float freezeInterval = 0;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float shrinkSpeed;
        [SerializeField] private float flyShrinkSpeed;

        [SerializeField] private KeyCode leftInput;
        [SerializeField] private KeyCode rightInput;

        [SerializeField] private KeyCode upInput;
        [SerializeField] private KeyCode downInput;

        [SerializeField] private KeyCode flyModeSwitch;

        private Rigidbody2D _rigidbody;
        private static float min = 0.0000001f;
        private SpriteRenderer _mSpriteRenderer;

        [SerializeField] private Color normalColor;
        [SerializeField] private Color flyColor;

        [SerializeField] private int goneMax;
        private int _goneCount = 0;
        [SerializeField] private GameObject loseText;


        private CheckPoint _checkPoint;
        private Vector3 _originalScale; // to record the original scale to use in the resize process
        private State _curState; // record the state 
        private bool _isFreeze;
        private DateTime _startFreezeTime; // record the time when get the freezeCollection

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _mSpriteRenderer = GetComponent<SpriteRenderer>();
        }

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
            //float diminishingSpeed;
            ProcessInputs();
            FlyControl();
            SizeControl();

            if(_goneCount >= goneMax)
            {
                loseText.SetActive(true);
                Time.timeScale = 0.0f;
            }

            if(_curState == State.Flying)
            {
                _mSpriteRenderer.color = flyColor;
            }
            else
            {
                _mSpriteRenderer.color = normalColor;
            }

            if (_isFreeze)
            {
                TimeSpan span = DateTime.UtcNow - _startFreezeTime;
                if (span.TotalSeconds > freezeInterval)
                {
                    _isFreeze = false;
                }
                //else
                //{
                //    diminishingSpeed = 0;
                //}
            }
            switch (_curState)
            {
                case State.Dead:
                    Debug.Log("Dead process and change to Normal");
                    trans.position = _checkPoint.Position;
                    trans.localScale = _checkPoint.Scale;
                    _curState = State.Normal;
                    return;
                case State.Gone:
                    Debug.Log("Die of shrinking");
                    trans.position = _checkPoint.Position;
                    trans.localScale = _originalScale;
                    _curState = State.Normal;
                    break;
                case State.Normal:
                    //Debug.Log("Normal Speed");
                    break;
                case State.Flying:
                    //Debug.Log("Flying Speed");
                    break;
                default:
                    return;
            }
        }

        private void ProcessInputs()
        {
            if (Input.GetKey(leftInput))
            {
                transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);
            }
            if (Input.GetKey(rightInput))
            {
                transform.Translate(-1 * Vector2.left * Time.deltaTime * moveSpeed);
            }

            if (Input.GetKeyDown(flyModeSwitch))
            {
                if (_curState == State.Normal)
                {
                    _curState = State.Flying;
                    _mSpriteRenderer.color = Color.blue;
                }
                else if(_curState == State.Flying)
                {
                    _curState = State.Normal;
                    transform.Translate(new Vector2(min, min));
                }
            }

            if (_curState == State.Flying)
            {
                if (Input.GetKey(upInput))
                {
                    transform.Translate(Vector2.up * Time.deltaTime * moveSpeed);
                }
                if (Input.GetKey(downInput))
                {
                    transform.Translate(-1 * Vector2.up * Time.deltaTime * moveSpeed);
                }
            }
        }

        private void FlyControl()
        {
            if (_curState == State.Flying)
            {
                _rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
            }
            else
            {
                _rigidbody.constraints = RigidbodyConstraints2D.None;
            }
        }

        private void SizeControl()
        {
            if (!_isFreeze)
            {
                if(_curState == State.Flying)
                {
                    transform.localScale -= (Time.deltaTime * (new Vector3(flyShrinkSpeed, flyShrinkSpeed, 0.0f)));
                }
                else
                {
                    transform.localScale -= (Time.deltaTime * (new Vector3(shrinkSpeed, shrinkSpeed, 0.0f)));
                }

                if (transform.localScale.x <= min || transform.localScale.y <= min)
                {
                    _curState = State.Gone;
                    _goneCount++;
                    Debug.Log("gone count: " + _goneCount);
                }
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
                default:
                    return;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case "Collection-Freeze":
                    Debug.Log("Stop diminishing");
                    _isFreeze = true;
                    _startFreezeTime = DateTime.UtcNow;
                    other.gameObject.SetActive(false);
                    break;
                case "Collection-Resize":
                    Debug.Log("Reset to Original Size");
                    transform.localScale = _originalScale;
                    other.gameObject.SetActive(false);
                    break;
                case "CheckPoint":
                    Debug.Log("Player reach the CheckPoint");
                    _checkPoint.DoCheckPoint(transform);
                    other.gameObject.SetActive(false);
                    break;
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
        Normal, Dead, Flying, Gone
    }
}