using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    private Transform myRotation;
    [SerializeField] Transform playersRotation;
    void Start()
    {
        myRotation = GetComponent<Transform>();
    }

    void Update()
    {
        myRotation.Rotate(Vector3.zero);
        //myRotation.rotation = Quaternion.identity; //keep the sprite original rotation
    }
}
