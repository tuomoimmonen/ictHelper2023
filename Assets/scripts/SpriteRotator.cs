using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    private Transform myRotation;
    void Start()
    {
        myRotation = GetComponent<Transform>();
    }

    void Update()
    {
        myRotation.rotation = Quaternion.identity; //keep the sprite original rotation
    }
}
