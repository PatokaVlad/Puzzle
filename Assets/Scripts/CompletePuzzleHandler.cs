using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletePuzzleHandler : MonoBehaviour
{
    private Collider2D _collider2D;
    private ShadowAnimationHandler _animationHandler;
    private Transform _transform;
    private SoundHandler _soundHandler;
    private BalloonsHandler _balloonsHandler;

    private Vector2 startPosition = new Vector2();

    [SerializeField]
    private float shadowAnimationDuration = 0.5f;
    [SerializeField]
    private float animationDuration = 1f;

    private bool isPlaying = false;

    [SerializeField]
    private bool useMouse = false;

    private void OnEnable()
    {
        _balloonsHandler.onBalloonsDestroyed += PlayAnimation;
    }

    private void Awake()
    {
        _balloonsHandler = FindObjectOfType<BalloonsHandler>();
        _soundHandler = FindObjectOfType<SoundHandler>();
    }

    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        _animationHandler = GetComponent<ShadowAnimationHandler>();
        _transform = GetComponent<Transform>();

        startPosition = _transform.position;
    }

    private void Update()
    {
        if(!isPlaying)
        {
            if (useMouse)
                MouseHandle();
            else
                TouchHandle();
        }
    }

    private void OnDisable()
    {
        _balloonsHandler.onBalloonsDestroyed -= PlayAnimation;
    }

    private void CheckInput(Vector2 position)
    {
        if (_collider2D == Physics2D.OverlapPoint(position))
        {
            StartCoroutine(StartAnimation());
        }
    }

    private void TouchHandle()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            CheckInput(touchPosition);
        }
    }

    private void MouseHandle()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            CheckInput(touchPosition);
        }
    }

    private IEnumerator StartAnimation()
    {
        yield return new WaitForFixedUpdate();
        _soundHandler.PlayCompleteClip();

        isPlaying = true;
        _animationHandler.StartAnimation(shadowAnimationDuration, 1, false, Vector2.zero);

        yield return new WaitForSeconds(animationDuration);
        
        _transform.position = startPosition + new Vector2(-0.1f, 0.1f);
        _animationHandler.StartAnimation(shadowAnimationDuration, -1, true, new Vector2(0.1f, -0.1f));
        isPlaying = false;
    }

    private void PlayAnimation()
    {
        StartCoroutine(StartAnimation());
    }
}
