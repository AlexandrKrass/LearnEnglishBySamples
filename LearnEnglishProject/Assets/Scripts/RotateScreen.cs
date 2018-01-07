using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LearningEnglishBySamples
{

public class RotateScreen : MonoBehaviour {
	/// <summary>
	/// 
	/// Slowly moves/rotates/scales camera to one of predefined transforms
	/// 
	/// </summary>
	public Camera cam;
	public Transform[] transforms;

	private Transform dest;
	private bool isProcessing = false;


	public void SetScreenToTransforn(int i) {
		dest = transforms[i];
		isProcessing = true;
	}


	private void SetCameraToTransform(Transform tr) {
		
		cam.transform.localPosition = tr.localPosition;
		cam.transform.localRotation = tr.localRotation;
		cam.transform.localScale    = tr.localScale;	
	}

	private float Dist (Transform a, Transform b) {
		return (a.localScale - b.localScale).magnitude + Mathf.Abs( Quaternion.Angle(a.localRotation, b.localRotation)) + (a.localPosition - b.localPosition).magnitude;
	
	}


	void Update() {
		if (isProcessing) {

			if (Dist(cam.transform, dest) < 0.01f) {
				SetCameraToTransform (dest);
				isProcessing = false;
			} else {
				cam.transform.localPosition = Vector3.Lerp (cam.transform.localPosition, dest.localPosition, Time.deltaTime*9.5f);
				cam.transform.localRotation = Quaternion.Lerp (cam.transform.localRotation, dest.localRotation, Time.deltaTime*9.5f);
				cam.transform.localScale    = Vector3.Lerp (cam.transform.localScale, dest.localScale, Time.deltaTime*9.5f);
			}		
		}		
	}
}

}