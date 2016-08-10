using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MoveTest : MonoBehaviour 
{

	private NavMeshAgent nav;
	private Vector3 target;
	private int floormask;
	private bool isset=false;


	void Start () {
		nav = GetComponent<NavMeshAgent> ();
		floormask = LayerMask.GetMask ("Floor");
	}

	public void GetDestination()
	{
		Ray camray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit camhit;
		if (Physics.Raycast(camray,out camhit,100f,floormask)) 
		{
			target = camhit.point;
		}
	}
//
//	public void OnPointerDown(PointerEventData data)
//	{
//		isset = true;
//	}


	void Update () {
		if (Input.GetMouseButtonUp(0)) 
		{
			isset = false;
		}
		if (Input.GetMouseButtonDown(0)&&!isset) {
			GetDestination ();
			nav.SetDestination (target);
			isset = true;
		}
	}
}
