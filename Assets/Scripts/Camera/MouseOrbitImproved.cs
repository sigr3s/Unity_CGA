using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class MouseOrbitImproved : MonoBehaviour {
    public float xSpeed = 120.0f; 
    float x = 0.0f;

 
    void LateUpdate () 
    {
        if(Input.GetKey(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject()){
            x = Input.GetAxis("Mouse X") * xSpeed;           
			transform.RotateAround(Vector3.zero, Vector3.up, x);
		}
	}
 
}