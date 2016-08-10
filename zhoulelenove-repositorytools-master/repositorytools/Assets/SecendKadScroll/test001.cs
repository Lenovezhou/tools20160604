using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;


public class test001 : MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler,IPointerClickHandler
{


	public AnimationCurve positioncurve,scalecurve,alphacurve;
	public List<test002> items = new List<test002> ();
	public List<test002> gotofirstitems = new List<test002> ();
	public List<test002> gotolastitems=new List<test002>();
	public float width=500,scalemax=1f;
	private float startvalue = 0.1f, addvalue = 0.2f,v,vmin=0.1f,vmax=0.9f,k,vk,vrecode,animtoendV;
	private float animaspact,vT,vtaotal,currentv,animspeed=1f;
	private RectTransform myrect;
	private Vector2 startpos, addpos;
	private bool _anim = false;

	void Start () {
		myrect = gameObject.GetComponent<RectTransform> ();
		ReFresh ();
	}

	public void ReFresh()
	{
		
		for (int i = 0; i < myrect.childCount; i++) 		//	进入for循环的条件，给子的parent赋值
		{
			test002 item =myrect.transform.GetChild (i).transform.GetComponent<test002>();
			v = startvalue + i * addvalue;		//	每个图片将要移动的距离，通过for循环来叠加
			items.Add (item);					//	为了在check（）中使用
			item.Init (this,v);			//	初始化，test002中的drag在init中只调用一次
		}
	}

	public void OnBeginDrag(PointerEventData data)
	{
		startpos = data.position;				//	开始移动时记录下位置
		addpos = Vector3.zero;					//	开始移动时重置鼠标拖动距离
		_anim=false;
	}

	public void OnDrag(PointerEventData data)
	{
		addpos = data.position - startpos;		//	记录下移动的位置
		v = data.delta.x* 1f / width;			//	图片需要移动的距离（每帧）相对于屏幕的宽度（可设置）具体移动的方式在子中实现
		vrecode+=v;
		for (int i = 0; i < items.Count; i++)	//	需要将父的中心点放在（0，0.5）才能以最左边开始排序
		{
			items [i].Drag (v);
		}
		Check (v);
	}

	public void Check(float _v)
	{
		if (_v < 0)
		{//向左运动
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i].v < (vmin - addvalue / 2))		//		限制最小（比最小还要小1/2的addvalue）
				{
					gotolastitems.Add(items[i]);
				}
			}
			if (gotolastitems.Count > 0)
			{
				for (int i = 0; i < gotolastitems.Count; i++)
				{
					gotolastitems[i].v = items[items.Count - 1].v + addvalue;		//	易错,给超出最左端的图片赋新的位置信息
					items.Remove(gotolastitems[i]);
					items.Add(gotolastitems[i]);
				}
				gotolastitems.Clear();		//	易错for循环后就清空
			}
		}
		else if (_v > 0)
		{//向右运动，需要把右边的放到前面来

			for (int i = items.Count-1; i >0; i--)
			{
				if (items[i].v >= vmax)
				{
					gotofirstitems.Add(items[i]);
				}
			}
			if (gotofirstitems.Count > 0)
			{
				for (int i = 0; i < gotofirstitems.Count; i++)
				{
					gotofirstitems[i].v = items[0].v - addvalue;		//	易错,给超出最左端的图片赋新的位置信息
					items.Remove(gotofirstitems[i]);
					items.Insert(0, gotofirstitems[i]);
				}
				gotofirstitems.Clear();		//	易错for循环后就清空
			}
		}
	}

	public void OnEndDrag(PointerEventData data)
	{
		for (int i = 0; i < items.Count; i++) {
			if (items[i].v>=vmin)					//	易错，在屏幕内的
			{
				k = vrecode % addvalue;				// 	vrecode+=v;vrecode 就是鼠标移动的距离
				vrecode = 0;		//	重置			（对addvalue取余表示每移动addvalue就完成拖动对多出距离的判断				
				if (k>0) 
				{
					vk = addvalue- k;				//	剩余将要移动的位置	
				}
				if (k<0)
				{
					vk = -addvalue - k;				//	剩余将要移动的位置	
				}
				break;			//	易错
			}
		}
		addpos = Vector3.zero;
		AnimToEnd (vk);

	}
		

	public void OnPointerClick(PointerEventData data)
	{
		if (addpos.sqrMagnitude <= 1)
		{
			Debug.Log("============OnPointerClickOK============");

			//		通过射线检测获取被点击图片的脚本
			test002 script = data.pointerPressRaycast.gameObject.GetComponent<test002>();
			if(script!=null)
			{
				float k = script.v;
				k = 0.5f - k;				//	距离中间的位置
				AnimToEnd(k);
			}

		}
	}

	public void AnimToEnd(float value)
	{
		animtoendV = value;					//		调用此方法的的参数转化为方向；并记录该参数用于Updata的条件跳出循环
		if (animtoendV > 0) {
			animaspact = 1;
		} else if (animtoendV < 0) {
			animaspact = -1;
		} else {
			return;			//	易错
		}
		vtaotal = 0;
		_anim = true;
	}
	void Update () {
		if (_anim) 	//			激活该方法
		{
			currentv = Time.deltaTime *animspeed* animaspact;
			vT = currentv + vtaotal;
			if (animaspact>0&&vT>=animtoendV) {				//	animtoendv,是从animtoend的参数
				_anim = false;
				currentv = animtoendV - vtaotal;			//	要Drag的距离=剩余要移动的距离-已经更新的距离；
			}
			if (animaspact<0&&vT<=animtoendV) {				//	animaspact表示将移动的方向
				_anim = false;
				currentv = animtoendV - vtaotal;
			}
			for (int i = 0; i < items.Count; i++) {
				items [i].Drag (currentv);
			}
			Check(currentv);
			vtaotal = vT;
		}
	}


	//		三个反馈曲线，记住用法！！

	public float GetPosition(float value)
	{
		return positioncurve.Evaluate (value) * width;
	}


	public float GetScale(float value)
	{
		return scalecurve.Evaluate(value)*scalemax;
	}

	public float GetAlpha(float value)
	{
		return alphacurve.Evaluate (value);
	}




}
