using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    private Collider2D _collider2D;
    private Transform _transform;
    private BalloonsHandler _balloonsHandler;

    [SerializeField]
    private float minSpeed = 4;
    [SerializeField]
    private float maxSpeed = 7;

    private float speed;

    private bool isDestroyed = false;

    private void Start()
    {
        _balloonsHandler = FindObjectOfType<BalloonsHandler>();

        _transform = GetComponent<Transform>();
        _collider2D = GetComponent<Collider2D>();

        speed = Random.Range(minSpeed, maxSpeed);
    }

    private void Update()
    {
        MoveUp();
        CheckPressing();
        //CheckMousePressing();
    }

    private void MoveUp()
    {
        Vector3 delta = Vector3.up * speed * Time.deltaTime;
        _transform.position += delta;
    }

    private void CheckPressing()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (_collider2D == Physics2D.OverlapPoint(touchPosition))
            {
                DestroyGameObject();
            }
        }
    }

    private void CheckMousePressing()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (_collider2D == Physics2D.OverlapPoint(touchPosition))
            {
                _balloonsHandler.PlayObjectParticle(_transform.position);
                DestroyGameObject();
            }
        }
    }

    private void DestroyGameObject()
    {
        if (!isDestroyed)
        {
            isDestroyed = true;
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        DestroyGameObject();
    }
}
