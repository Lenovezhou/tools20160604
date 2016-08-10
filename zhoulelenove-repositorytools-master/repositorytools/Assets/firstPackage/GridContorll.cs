using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Sprites;

public class GridContorll : MonoBehaviour {

	public GameObject[] cells;
	public GameObject instantiate;
	GameObject item,lastparent;
	Image imagesingel;
	Text indextext;
	bool isfind = false;
	bool isselect;
	int randomM,indexint;
	string randomstr="";
	string overridename,oldridename;

	void Start () {
		
	}
	public void Pickup()
	{
		randomM = Random.Range (0,6);
		randomstr = "Picture/" + randomM.ToString();
		item= Instantiate (instantiate ,transform.position,transform.rotation)as GameObject;
		imagesingel =item.transform.GetComponent<Image> ();


		//		load 内是文件名所以，路径下的randomM代表的是文件名0，1，2，3，4，5----------Picture的名字
		imagesingel.overrideSprite = Resources.Load (randomstr, typeof(Sprite))as Sprite;
		overridename = imagesingel.overrideSprite.name;
		Debug.Log (overridename);
		isfind = false;
		for (int i = 0; i < cells.Length; i++) 
		{
			if (cells[i].transform.childCount>0) 
			{
				if (overridename==cells[i].transform.GetChild(0).transform.GetComponent<Image>().overrideSprite.name)
				{
					isfind = true;
					indextext = cells [i].transform.GetChild (0).transform.GetChild(0).GetComponent<Text> ();
					indexint = int.Parse (indextext.text);
					indexint += 1;
					indextext.text = indexint.ToString ();
					Destroy (item);

				}
			}
		}
		if (isfind==false) {
			for (int i = 0; i < cells.Length; i++) 
			{
				if (cells[i].transform.childCount==0)
				{
					item.transform.SetParent (cells[i].transform);
					item.transform.localPosition = Vector3.zero;
				
					break;
				}
			}
		}
	}




	public void Select( GameObject castedobj,GameObject movedobj, GameObject oldparent)
	{
		
		if (castedobj.tag=="Uitem") {
			

			//	当交换item时movedobj在这里设置父节点
			lastparent = castedobj.transform.parent.gameObject;
			movedobj.transform.SetParent (lastparent.transform);
			movedobj.transform.localPosition = Vector3.zero;
			castedobj.transform.SetParent (oldparent.transform);
			castedobj.transform.localPosition = Vector3.zero;
		}

			//  当拖拽到空的Ucells上时,movedobj的父节点在这里设置
			if (castedobj.transform.childCount == 0 ) {
				movedobj.transform.SetParent (castedobj.transform);
				movedobj.transform.localPosition = Vector3.zero;
				
			}

		for (int i = 0; i < cells.Length; i++) {
				if (cells [i].transform.childCount > 0) {
					cells [i].transform.GetChild (0).transform.localPosition = Vector3.zero;
				}

			}

		
	}


	void Update () {
		if (Input.GetKeyDown(KeyCode.G))
		{
			Pickup ();
		}
	}
}
