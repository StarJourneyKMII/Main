using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiProduction.Extension;

public class JumpTutorial : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigibody = null;
    [SerializeField]
    private SpriteRenderer _renderer = null;
    [SerializeField]
    private Vector2 _direction = Vector2.one;
    [SerializeField]
    private float _jumpForce = 5f;
    [SerializeField]
    private bool _isDoubleJump = false;

    private Coroutine _coroutine = null;
    private Vector2 _originPos = Vector2.zero;
    private bool _isGrounded = false;
    private int _groundLayerIndex = 0;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Tutorial"));
        _groundLayerIndex = LayerMask.NameToLayer("Ground");
    }

    private void OnEnable()
    {
        _renderer.color = new Color(1f, 1f, 1f, 0f);
        _coroutine = StartCoroutine(PlayTutorial());
        _originPos = transform.position;
    }

    private void Update()
    {
        if(Physics2D.gravity.y > 0)
        {
            _rigibody.gravityScale = -1f;
        }
        else
        {
            _rigibody.gravityScale = 1f;
        }
        
    }

    private IEnumerator PlayTutorial()
    {
        while (true)
        {
            yield return StartCoroutine(AlphaFading(1f));
            _rigibody.AddForce(_direction * _jumpForce,ForceMode2D.Impulse);
            yield return new WaitUntil(() => !_isGrounded);
            if(_isDoubleJump)
            {
                yield return new WaitUntil(() => _rigibody.velocity.y <= -0.5f);
                _rigibody.AddForce(_direction * _jumpForce, ForceMode2D.Impulse);
            }
            yield return new WaitUntil(() => _isGrounded);
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(AlphaFading(0f));
            transform.position = _originPos;
        }
    }

    private IEnumerator AlphaFading(float targetAlpha)
    {
        float origin = _renderer.color.a;
        float t = 0f;
        while(t < 1f)
        {
            _renderer.color = new Color(1f,1f,1f, Mathf.Lerp(origin, targetAlpha, t));
            t += Time.deltaTime;
            yield return null;
        }
        _renderer.color = new Color(1f, 1f, 1f, targetAlpha);
    }

    private void OnDisable()
    {
        _coroutine.Stop(this);
        transform.position = _originPos;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == _groundLayerIndex)
        {
            _isGrounded = true;
            _rigibody.velocity = Vector3.zero;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == _groundLayerIndex)
        {
            _isGrounded = false;
        }
    }
}
