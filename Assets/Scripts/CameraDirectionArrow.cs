using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDirectionArrow : MonoBehaviour
{
    public Transform cameraTransform;

    private Transform arrowTransform;

    // Start is called before the first frame update
    void Start()
    {
        arrowTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        //update y rotation to align with camera facings
        float cameraRotationY = cameraTransform.eulerAngles.y;
        arrowTransform.rotation = Quaternion.Euler(0.0f, cameraRotationY, 0.0f);
    }
}
