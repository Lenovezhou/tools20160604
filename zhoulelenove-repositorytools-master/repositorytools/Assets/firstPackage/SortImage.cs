using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SortImage : MonoBehaviour,IDragHandler,IPointerDownHandler,IPointerUpHandler
{	
	public GameObject background;
	public GameObject _canvas;
	public GameObject grid, panel;
	public GameObject oldparent;
	public GridContorll gridcontroll;
	Vector2 startpos;
	RectTransform rect;
	CanvasGroup canvas;
	void Start () {
		rect = gameObject.GetComponent<RectTransform> ();
		canvas = gameObject.GetComponent<CanvasGroup> ();
		_canvas = GameObject.FindWithTag ("Finish");
		grid = FindInChild (_canvas,"Grid");
		panel = FindInChild (_canvas,"Panel");
		background = FindInChild (_canvas,"BackGround");
		gridcontroll = grid.GetComponent<GridContorll> ();

	}
		
	public GameObject FindInChild(GameObject Go,string name)
	{
		foreach (RectTransform obj in Go.GetComponentsInChildren<RectTransform>()) {
			if (obj.name==name) {
				return obj.gameObject;
			}
		}
		return null;
	}

	public void OnPointerDown(PointerEventData data)
	{
		oldparent = this.transform.parent.gameObject;
		canvas.blocksRaycasts = false;
		startpos = data.position;
		rect.transform.localScale = new Vector3 (0.3f, 0.7f, 0.7f);
		Debug.Log ("pointerdown"+data.pointerCurrentRaycast.gameObject.name);
		rect.transform.SetParent (background.transform);
	}

	public void OnPointerUp(PointerEventData data)
	{
		rect.transform.localScale = new Vector3 (0.5f,1f,1f);
		if (data.pointerCurrentRaycast.gameObject!=null) 
		{
			Debug.Log ("pointerup"+data.pointerCurrentRaycast.gameObject.name);
			gridcontroll.Select ( data.pointerCurrentRaycast.gameObject, this.gameObject,oldparent);
		}
		canvas.blocksRaycasts = true;
	}

	public void OnDrag(PointerEventData data)
	{
		rect.position = data.position;
	}

	void Update () {
	
	}
}
