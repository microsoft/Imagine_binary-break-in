using UnityEngine;
using System.Collections;

public class WheelLightUp : MonoBehaviour {

	private Ray myRay;
	private RaycastHit myHit;
	private Collider wheelCollider;
	private MeshRenderer wheelRenderer;

	public Material OverWheel; //Material when mouse hits the sphere collider on the wheel
	public Material NotOverWheel; //Material set when mouse leaves the sphere collider

	void Start(){
		wheelCollider = gameObject.GetComponent<Collider> ();
		wheelRenderer = gameObject.GetComponent<MeshRenderer> ();
	}

	void Update(){
		myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
	

		if (Physics.Raycast (myRay,out myHit)){
			if(myHit.collider == wheelCollider)
				wheelRenderer.material =  OverWheel;
		}
			if(myHit.collider != wheelCollider)
				wheelRenderer.material =  NotOverWheel;
	}

}