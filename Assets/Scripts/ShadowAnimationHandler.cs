using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowAnimationHandler : MonoBehaviour
{
    private Transform _transform;
    private Transform _shadowTransform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _shadowTransform = GetComponentsInChildren<Transform>()[1];
    }

    public void StartAnimation(float duration, int direction, bool isBack, Vector2 shadowMoveVector)
    {
        StartCoroutine(AnimateGrab(duration, direction, isBack, shadowMoveVector));
    }

    private IEnumerator AnimateGrab(float duration, int direction, bool isBack, Vector2 shadowMoveVector)
    {
        float time = 0;

        Vector2 startPiecePosition = _transform.position;
        Vector2 targetPiecePosition = new Vector2(startPiecePosition.x - 0.1f * direction, startPiecePosition.y + 0.1f * direction);

        while (time < duration)
        {
            _transform.position = Vector2.Lerp(startPiecePosition, targetPiecePosition, time / duration);
            _shadowTransform.position = Vector2.Lerp(_shadowTransform.position, startPiecePosition + shadowMoveVector, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        _transform.position = targetPiecePosition;
        if(!isBack)
        {
            _shadowTransform.position = startPiecePosition;
        }

    }
}
