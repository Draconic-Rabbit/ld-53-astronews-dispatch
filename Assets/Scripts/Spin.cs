using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] Vector3 _speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_speed * Time.deltaTime);
    }
}
