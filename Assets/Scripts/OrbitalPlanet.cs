using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalPlanet : MonoBehaviour
{
    [SerializeField] GameObject _rotationPivot;
    [SerializeField] float _speed = 5f;
    [SerializeField] Sprite _previewSprite;
    [SerializeField] Color32 _previewColor;
    [SerializeField] GameObject _deliveryZone;

    public Sprite PreviewSprite { get => _previewSprite; set => _previewSprite = value; }
    public Color32 PreviewColor { get => _previewColor; set => _previewColor = value; }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(_rotationPivot.transform.position, Vector3.forward, _speed * Time.deltaTime);
    }

    public void EnableDeliveryZone()
    {
        _deliveryZone.gameObject.SetActive(true);
    }

    public void DisableDeliveryZone()
    {
        _deliveryZone.gameObject.SetActive(false);
    }
}
