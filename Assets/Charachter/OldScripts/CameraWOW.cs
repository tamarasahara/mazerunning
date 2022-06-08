// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CameraWOW : MonoBehaviour
// {
//     var target : Transform;
//     var distance = 10.0;
     
//     var xSpeed = 250.0;
//     var ySpeed = 120.0;
     
//     var yMinLimit = -20;
//     var yMaxLimit = 80;
     
//     var zoomRate = 20;
     
//     private var x = 0.0;
//     private var y = 0.0;
     
//     @script AddComponentMenu("Camera-Control/Mouse Orbit")
     
//     void Start () {
//         var angles = transform.eulerAngles;
//         x = angles.y;
//         y = angles.x;
     
//         // Make the rigid body not change rotation
//         if (rigidbody)
//             rigidbody.freezeRotation = true;
//     }
     
//     void LateUpdate () {
     
//         if (target) {
//             if (Input.GetMouseButton(0))
//             {
//             x += Input.GetAxis("Mouse X") * xSpeed * 0.02;
//             y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02;
//             var test = 0;
//             test = y;
//             }
//             distance += -(Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime) * zoomRate * Mathf.Abs(distance);
//             if (distance < 2.5)
//             {
//                 distance = 2.5;
//             }
//             if (distance > 20)
//             {
//                 distance = 20;
//             }
           
           
//             y = ClampAngle(y, yMinLimit, yMaxLimit);
                   
//             //Debug.Log("y: "+y+" test: "+test);
           
//             if( y == yMinLimit  test == yMinLimit)
//             {
//                 // This is to allow the camera to slide across the bottom if the player is too low in the y
//                 distance += -(Input.GetAxis("Mouse Y") * Time.deltaTime) * 10 * Mathf.Abs(distance);
//             }
           
//             var rotation = Quaternion.Euler(y, x, 0);
//             var position = rotation * Vector3(0.0, 2.0, -distance) + target.position;
           
//             //Debug.Log("Distance: "+distance);
//             transform.rotation = rotation;
//             transform.position = position;
//         }
//     }
     
//     static function ClampAngle (angle : float, min : float, max : float) {
//         if (angle < -360)
//             angle += 360;
//         if (angle > 360)
//             angle -= 360;
//         return Mathf.Clamp (angle, min, max);
//     }

// }
