using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    [SerializeField]
    private Transform _rightPlaceTransform;
    private Transform _transform;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _shadowSpriteRenderer;

    private Collider2D _collider2D;

    private PuzzleHandler _puzzleHandler;
    private SoundHandler _soundHandler;
    private ShadowAnimationHandler _animationHandler;

    private Vector2 initialPosition = new Vector2();

    private float correctPositionAccuracy;
    private float animationAccuracy;
    private float smoothDragMultiplier;
    private float minSmoothAnimationMultiplier;
    private float maxSmoothAnimationMultiplier;
    private float shadowAnimationDuration;

    private bool inInitialPlace = false;
    private bool inRightPlace = false;
    private bool isTouched = false;

    private bool firstPress = false;

    private void Awake()
    {
        _puzzleHandler = FindObjectOfType<PuzzleHandler>();
        _transform = GetComponent<Transform>();

        initialPosition = _transform.position;
        _puzzleHandler.IncreasePiecesCount(initialPosition);
    }

    private void OnEnable()
    {
        _puzzleHandler.onPlayerWin += Disactivate;
    }

    private void Start()
    {
        _soundHandler = FindObjectOfType<SoundHandler>();

        _animationHandler = GetComponent<ShadowAnimationHandler>();
        
        _collider2D = GetComponent<Collider2D>();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _shadowSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[1];

        Initialize();
        
        initialPosition = _puzzleHandler.GenerateInitialPosition();
        StartCoroutine(VisitStartPosition(_rightPlaceTransform.position));
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

    private void OnDisable()
    {
        _puzzleHandler.onPlayerWin -= Disactivate;
    }

    private void Initialize()
    {
        correctPositionAccuracy = _puzzleHandler.CorrectPositionAccuracy;
        animationAccuracy = _puzzleHandler.AnimationAccuracy;
        smoothDragMultiplier = _puzzleHandler.SmoothDragMultiplier;
        minSmoothAnimationMultiplier = _puzzleHandler.MinSmoothAnimationMultiplier;
        maxSmoothAnimationMultiplier = _puzzleHandler.MaxSmoothAnimationMultiplier;
        shadowAnimationDuration = _puzzleHandler.ShadowAnimationDuration;
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
                    ChangeSortingOrder(2);
                    _animationHandler.StartAnimation(shadowAnimationDuration, 1, false, Vector2.zero);
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
                    ChangeSortingOrder(-2);
                    DownPuzzlePiece();
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
            switch (type)
            {
                case PositionsTypes.Start:
                    inInitialPlace = true;
                    break;
                case PositionsTypes.Initial:
                    ChangeSortingOrder(-2);

                    _transform.position = targetPlace + new Vector2(-0.1f, 0.1f);
                    _animationHandler.StartAnimation(shadowAnimationDuration, -1, true, new Vector2(0.1f, -0.1f));

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
                    ChangeSortingOrder(2);
                    _animationHandler.StartAnimation(shadowAnimationDuration, 1, false, Vector2.zero);

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
                    ChangeSortingOrder(-2);
                    DownPuzzlePiece();
                    _soundHandler.PlayFailClip();
                }
                isTouched = false;
                break;
            default:
                break;
        }

        StickToRightPlace(_rightPlaceTransform.position, PositionsTypes.Initial, correctPositionAccuracy);
    }

    private IEnumerator VisitStartPosition(Vector2 targetPosition)
    {
        float smooth = Random.Range(minSmoothAnimationMultiplier, maxSmoothAnimationMultiplier);
        _transform.position = targetPosition;
        yield return new WaitForSeconds(0.2f);

        while(!inInitialPlace)
        {
            smooth += Time.deltaTime;
            MovePiece(initialPosition, smooth);
            StickToRightPlace(initialPosition, PositionsTypes.Start, animationAccuracy);

            yield return null;
        }
    }

    private void Disactivate() => gameObject.SetActive(false);

    private void ChangeSortingOrder(int count)
    {
        _spriteRenderer.sortingOrder += count;
        _shadowSpriteRenderer.sortingOrder += count;
    }

    private void DownPuzzlePiece()
    {
        _transform.position = initialPosition + new Vector2(-0.1f, 0.1f);
        _animationHandler.StartAnimation(shadowAnimationDuration, -1, true, new Vector2(0.1f, -0.1f));
    }
}

enum PositionsTypes
{
    Start,
    Initial
}