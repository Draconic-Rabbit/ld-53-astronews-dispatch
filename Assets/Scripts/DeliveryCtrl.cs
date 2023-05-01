using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryCtrl : MonoBehaviour
{
    GameObject _deliveryZoneGO;
    float _deliveryDuration = 5f;
    float _enterDeliveryZoneTime;

    [SerializeField] ParticleSystem _deliveryVFX;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip[] _deliveryAudioClips;


    private void Update()
    {
        if (isDelivering())
        {
            OrientDeliveryVFX();
            if (Time.time - _enterDeliveryZoneTime > _deliveryDuration)
            {
                // Delivery completed
                MissionManager.Instance.ValidateDelivery();
                StopDelivery();
            }
        }
    }

    private void OrientDeliveryVFX()
    {
        Vector2 direction = _deliveryZoneGO.transform.position - gameObject.transform.position;
        _deliveryVFX.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
    }

    // Demarre la livraison quand on entre dans le trigger de la planet de la mission
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == MissionManager.Instance._currentTargetMission)
        {
            StartDelivery(other.gameObject);
        }
    }

    void StartDelivery(GameObject target)
    {
        _deliveryZoneGO = target;
        _enterDeliveryZoneTime = Time.time;
        _deliveryVFX.Play();
        AudioClip audioClip = _deliveryAudioClips[Random.Range(0, _deliveryAudioClips.Length)];
        _audioSource.PlayOneShot(audioClip);
    }

    void StopDelivery()
    {
        _deliveryZoneGO = null;
        _deliveryVFX.Stop();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == _deliveryZoneGO)
        {
            StopDelivery();
        }
    }

    bool isDelivering()
    {
        return _deliveryZoneGO != null;
    }
}
