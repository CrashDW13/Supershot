using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform cam;

    private void Start()
    {
        cam = Camera.main.transform; 
    }

    private void Update()
    {
        transform.rotation = cam.rotation;  
    }
}
