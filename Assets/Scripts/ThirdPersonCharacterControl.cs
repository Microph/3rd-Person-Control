using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCharacterControl : MonoBehaviour
{
    public Transform cameraTransform;

    public float ROTATION_TIME = 0.3f;

    private Transform characterTransform;

    // Start is called before the first frame update
    void Start()
    {
        characterTransform = transform;
    }

    void FixedUpdate()
    {
        //Get input -> transform to vector direction (according to camera's forward direction)
        //a = camera direction vector (now treated as (0, 1) )
        //b = input direction vector (in a circle of 1 radius)
        //Angle between vectors _tobeAngle = cos2(dot(a, b) / mag(a) * mag(b))
        //let's say we input (0, 1) then, _tobeAngle = 0
        //it means character have to face to current camera angle before moving!
        //So, character need to rotate with angles of...
        //characterTargetRotation = currentCameraAngleInDegree + _tobeAngle

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector2 inputDir = new Vector2(h, v);
        //Debug.Log("input dir: " + inputDir);
        if(inputDir.magnitude == 0)
        {
            return;
        }

        float cameraAngleDiffWithInput = Mathf.Acos(Vector2.Dot(new Vector2(0, 1), inputDir) / inputDir.magnitude) * Mathf.Rad2Deg;
        float targetRotationDegree = cameraTransform.eulerAngles.y + (h > 0 ? cameraAngleDiffWithInput : -cameraAngleDiffWithInput);
        Quaternion targetRotationQuarternion = Quaternion.Euler(0, characterTransform.rotation.y + targetRotationDegree, 0);
        characterTransform.rotation = Quaternion.Lerp(characterTransform.rotation, targetRotationQuarternion, Time.fixedDeltaTime / ROTATION_TIME);
    }
}
