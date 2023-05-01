using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetLocator : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] float _distanceFromPlayer = 2.5f;

    [SerializeField] float _distanceOfFullRedGradient = 10f;
    Gradient _gradient;
    [SerializeField] Color32[] _colors;


    GameObject _planetTarget;
    SpriteRenderer _previewSR;

    private void Awake()
    {
        _previewSR = GetComponent<SpriteRenderer>();
        DisableTargeting();
        PrepareGradient();
    }

    private void PrepareGradient()
    {
        _gradient = new Gradient();
        int colorCount = _colors.Length;

        GradientColorKey[] colorKeys = new GradientColorKey[colorCount];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[colorCount];
        int curId = 0;
        foreach (Color32 color in _colors)
        {
            colorKeys[curId].color = color;
            colorKeys[curId].time = 1f / colorCount * curId;
            // badly done but quicker than thinking
            alphaKeys[curId].alpha = 1.0f;
            alphaKeys[curId].time = 1f / colorCount * curId;
            curId++;
        }
        _gradient.SetKeys(colorKeys, alphaKeys);
    }

    private void LateUpdate()
    {
        // No mission or player lose
        if (_planetTarget == null || _player == null)
        {
            return;
        }
        Vector2 direction = _planetTarget.transform.position - _player.transform.position;
        if (direction.magnitude < 5)
        {
            _previewSR.enabled = false;
            return;
        }
        if (_previewSR.enabled != true)
        {
            _previewSR.enabled = true;
        }
        float colorTime = Mathf.Clamp(direction.magnitude / _distanceOfFullRedGradient, 0, 1f);
        _previewSR.color = _gradient.Evaluate(colorTime);
        transform.position = _player.transform.position + (Vector3)direction.normalized * _distanceFromPlayer;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
    }

    public void SetNewTarget(GameObject target)
    {
        _planetTarget = target;
        _previewSR.enabled = true;
    }

    public void DisableTargeting()
    {
        _planetTarget = null;
        _previewSR.enabled = false;
    }
}
