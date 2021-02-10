using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    [SerializeField]
    private Transform _rightPlaceTransform;
    private Transform _transform;

    private Collider2D _collider2D;

    private PuzzleHandler _puzzleHandler;
    private SoundHandler _soundHandler;

    private Vector2 initialPosition = new Vector2();

    [SerializeField]
    private float correctPositionAccuracy = 0.5f;
    [SerializeField]
    private float smoothMultiplier = 4f;

    private bool inRightPlace = false;
    private bool isTouched = false;

    private bool firstPress = false;

    private void Start()
    {
        _puzzleHandler = FindObjectOfType<PuzzleHandler>();
        _soundHandler = FindObjectOfType<SoundHandler>();

        _transform = GetComponent<Transform>();
        _collider2D = GetComponent<Collider2D>();

        initialPosition = _transform.position;
        _puzzleHandler.IncreasePiecesCount();
    }

    private void Update()
    {
        if (Input.touchCount > 0 && !inRightPlace)
        {
            HandlePressing();
        }
        //if (!inRightPlace)
        //    MouseMove();
    }

    private void HandlePressing()
    {
        Touch touch = Input.GetTouch(0);
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                if(_collider2D == Physics2D.OverlapPoint(touchPosition))
                {
                    isTouched = true;
                }
                break;
            case TouchPhase.Moved:
                if (isTouched)
                {
                    MovePiece(touchPosition);
                }
                break;
            case TouchPhase.Stationary:
                if (isTouched)
                {
                    MovePiece(touchPosition);
                }
                break;
            case TouchPhase.Ended:
                if (!inRightPlace && isTouched)
                {
                    _soundHandler.PlayFailClip();
                    _transform.position = initialPosition;
                }
                isTouched = false;
                break;
            default:
                break;
        }

        StickToRightPlace();
    }

    private void MovePiece(Vector2 targetPosition)
    {
        Vector2 motionPosition = Vector2.Lerp(_transform.position, targetPosition, smoothMultiplier * Time.deltaTime);
        _transform.position = motionPosition;
    }

    private void StickToRightPlace()
    {
        if (Mathf.Abs(_transform.position.x - _rightPlaceTransform.position.x) <= correctPositionAccuracy &&
            Mathf.Abs(_transform.position.y - _rightPlaceTransform.position.y) <= correctPositionAccuracy)
        {
            _transform.position = _rightPlaceTransform.position;

            _puzzleHandler.DecreasePiecesCount();
            _puzzleHandler.PlayObjectParticle(_transform.position);
            _soundHandler.PlayWinClip();

            inRightPlace = true;
        }
    }

    private void MouseMove() // симуляция тачей для удобной работы в редакторе
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        TouchPhase touch = TouchPhase.Canceled;

        if (Input.touchCount == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                firstPress = true;
                touch = TouchPhase.Began;
            }
            if (Input.GetMouseButton(0) && !firstPress)
            {
                touch = TouchPhase.Moved;
            }
            if (Input.GetMouseButtonUp(0))
            {
                touch = TouchPhase.Ended;
            }
        }

        switch (touch)
        {
            case TouchPhase.Began:
                if (_collider2D == Physics2D.OverlapPoint(mousePosition))
                {
                    isTouched = true;
                    firstPress = false;
                }
                break;
            case TouchPhase.Moved:
                if (isTouched)
                {
                    MovePiece(mousePosition);
                }
                break;
            case TouchPhase.Ended:
                if (!inRightPlace && isTouched)
                {
                    _soundHandler.PlayFailClip();
                    _transform.position = initialPosition;
                }
                isTouched = false;
                break;
            default:
                break;
        }

        StickToRightPlace();
    }
}
