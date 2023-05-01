using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class VesselController : MonoBehaviour
{
    Vector2 _rotationInput;
    Vector2 _precisionMoveInput;
    Rigidbody2D _rb;
    bool _isThrusting = false;

    [SerializeField] float _rotationPower = 5f;
    [SerializeField] float _precisionThrustForce = 3f;
    [SerializeField] float _thrustForce = 5f;

    [SerializeField] TextMeshProUGUI _thurstText;
    [SerializeField] ParticleSystem _thurstVFX;

    [SerializeField] int _hitPoints = 10;
    [SerializeField] TextMeshProUGUI _hitPointText;
    [SerializeField] AudioClip[] _crashAudioClips;
    [SerializeField] AudioClip _lossAudioClip;

    [SerializeField] GameObject _restartBtn;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        UpdateHPDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.AddTorque(_rotationInput.x * -1 * Time.deltaTime * _rotationPower);
        _rb.AddRelativeForce(_precisionMoveInput * _precisionThrustForce * Time.deltaTime);

        if (_isThrusting)
        {
            _rb.AddRelativeForce(Vector2.up * _thrustForce * Time.deltaTime);
        }
    }

    public void OnRotate(InputValue value)
    {
        _rotationInput = value.Get<Vector2>();
    }

    public void OnFire(InputValue value)
    {
        _isThrusting = value.Get<float>() != 0f;
        if (_isThrusting)
        {
            _thurstVFX.Play();
            // _thurstText.gameObject.SetActive(true);
        }
        else
        {
            _thurstVFX.Stop();
            // _thurstText.gameObject.SetActive(false);
        }
    }

    public void OnMove(InputValue value)
    {
        _precisionMoveInput = value.Get<Vector2>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        _hitPoints--;
        UpdateHPDisplay();

        if (_hitPoints == 0)
        {
            // Time.timeScale = 0;
            _restartBtn.SetActive(true);
            GetComponent<AudioSource>().PlayOneShot(_lossAudioClip);
            Destroy(gameObject, 2f);
        }
        else
        {
            AudioClip crashAudioClip = _crashAudioClips[Random.Range(0, _crashAudioClips.Length)];
            GetComponent<AudioSource>().PlayOneShot(crashAudioClip);
        }
    }

    private void UpdateHPDisplay()
    {
        _hitPointText.text = "Hull Points : " + _hitPoints.ToString();
    }
}
