using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    [SerializeField] GameObject _rotationPivot;
    [SerializeField] float _speed = 5f;
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(_rotationPivot.transform.position, Vector3.forward, _speed * Time.deltaTime);
    }
}
