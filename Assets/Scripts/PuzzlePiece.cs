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
    private float animationAccuracy = 0.01f;
    [SerializeField]
    private float smoothDragMultiplier = 4f;
    [SerializeField]
    private float minSmoothAnimationMultiplier = 1.5f;
    [SerializeField]
    private float maxSmoothAnimationMultiplier = 3f;

    private bool inInitialPlace = false;
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

        StartCoroutine(VisitStartPosition());
    }

    private void Update()
    {
        if(inInitialPlace)
        {
            if (!_puzzleHandler.UseMouse)
            {
                if (Input.touchCount > 0 && !inRightPlace)
                {
                    HandlePressing();
                }
            }
            else
            {
                if (!inRightPlace)
                    MouseMove();
            }
        }
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
                    MovePiece(touchPosition, smoothDragMultiplier);
                }
                break;
            case TouchPhase.Stationary:
                if (isTouched)
                {
                    MovePiece(touchPosition, smoothDragMultiplier);
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

        StickToRightPlace(_rightPlaceTransform.position, PositionsTypes.Initial, correctPositionAccuracy);
    }

    private void MovePiece(Vector2 targetPosition, float smoothMultiplier)
    {
        Vector2 motionPosition = Vector2.Lerp(_transform.position, targetPosition, smoothMultiplier * Time.deltaTime);
        _transform.position = motionPosition;
    }

    private void StickToRightPlace(Vector2 targetPlace, PositionsTypes type, float accuracy)
    {
        if (Mathf.Abs(_transform.position.x - targetPlace.x) <= accuracy &&
            Mathf.Abs(_transform.position.y - targetPlace.y) <= accuracy)
        {
            _transform.position = targetPlace;

            switch (type)
            {
                case PositionsTypes.Start:
                    inInitialPlace = true;
                    break;
                case PositionsTypes.Initial:
                    _puzzleHandler.DecreasePiecesCount();
                    _puzzleHandler.PlayObjectParticle(_transform.position);
                    _soundHandler.PlayWinClip();
                    inRightPlace = true;
                    break;
                default:
                    break;
            }
                
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
                    MovePiece(mousePosition, smoothDragMultiplier);
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

        StickToRightPlace(_rightPlaceTransform.position, PositionsTypes.Initial, correctPositionAccuracy);
    }

    private IEnumerator VisitStartPosition()
    {
        float smooth = Random.Range(minSmoothAnimationMultiplier, maxSmoothAnimationMultiplier);
        _transform.position = _rightPlaceTransform.position;
        while(!inInitialPlace)
        {
            smooth += Time.deltaTime;
            MovePiece(initialPosition, smooth);
            StickToRightPlace(initialPosition, PositionsTypes.Start, animationAccuracy);

            yield return null;
        }
    }
}

enum PositionsTypes
{
    Start,
    Initial
}