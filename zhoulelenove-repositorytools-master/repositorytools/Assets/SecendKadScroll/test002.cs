using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class test002 : MonoBehaviour {



	public float v,s;
	private test001 parent;
	private RectTransform rect;
	private Vector3 p, sv;
	private Image image;
	private Color color;

	public void Init(test001 _parent,float value)  	//	在parent中调用这个方法来实现初始化就可以避免先后问题
	{												//	尽量把所有的初始化放在这里
		rect = gameObject.GetComponent<RectTransform> ();
		image = gameObject.GetComponent<Image> ();
		parent = _parent;
		Drag (value);
	}

	public void Drag(float value)
	{
		v += value;
		p = rect.localPosition;				//	ui用localposition，
		p.x = parent.GetPosition (v);		//	这里要求图片的父的中心点是以你想要的位置开始的点如（0，0.5）
		rect.localPosition = p;				//	就可以从屏幕的左侧开始计算

		color = image.color;
		color.a = parent.GetAlpha (v);
		image.color = color;
		sv = rect.transform.localScale;
		s = parent.GetScale (v);
		sv.x = s;
		sv.y = s;
		sv.z = 1f;
		rect.localScale = sv;

	}

	void Start () {
	
	}
	

	void Update () {
	
	}
}
