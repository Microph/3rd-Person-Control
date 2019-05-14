using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCharacterControl : MonoBehaviour
{
    public Transform cameraTransform;

    public float ROTATION_TIME = 0.15f;
    public float RUNNING_SPEED = 5f;

    private Transform characterTransform;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        characterTransform = transform;
        anim = GetComponent<Animator>();
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
        if(inputDir.magnitude == 0)
        {
            return;
        }

        float inputMagnitude = Mathf.Clamp(inputDir.magnitude, 0, 1);
        anim.SetFloat("Speed", inputMagnitude);
        anim.SetFloat("Direction", Mathf.Sin(Vector3.Angle(Vector3.zero, inputDir) * Mathf.Deg2Rad));

        //Rotation
        float cameraAngleDiffWithInput = Mathf.Acos(Vector2.Dot(new Vector2(0, 1), inputDir) / inputDir.magnitude) * Mathf.Rad2Deg; //Vector(0, 1) represents camera direction
        float targetRotationDegree = cameraTransform.eulerAngles.y + (h > 0 ? cameraAngleDiffWithInput : -cameraAngleDiffWithInput);
        Quaternion targetRotationQuarternion = Quaternion.Euler(0, characterTransform.rotation.y + targetRotationDegree, 0);
        characterTransform.rotation = Quaternion.Lerp(characterTransform.rotation, targetRotationQuarternion, Time.fixedDeltaTime / ROTATION_TIME);

        //Movement
        Vector3 velocity = new Vector3(0, 0, inputMagnitude);
        velocity = transform.TransformDirection(velocity);
        transform.localPosition += velocity * RUNNING_SPEED * Time.fixedDeltaTime;
    }
}
