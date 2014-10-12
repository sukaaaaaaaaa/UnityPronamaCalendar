using UnityEngine;
using System.Collections;
using System;

public class Calendar : MonoBehaviour {
	GameObject todayLight;
	GameObject cubeLight;
	GameObject myCamera;
	Vector3 cubeAngle = new Vector3(6,60,6);
	Vector3 angle = new Vector3(0,360,0);
	GameObject cube;
	GameObject plane;
	Font arial;
	DateTime dt;

	IEnumerator Download(GameObject target,String url){
		var myImage = new WWW (url);
		yield return myImage;
		Texture2D tex = myImage.texture;
		if (tex != null) {
			target.renderer.material.mainTexture = tex;
			float aspect = (float)tex.width/(float)tex.height;
			Vector3 s = target.transform.localScale;
			target.transform.localScale = new Vector3(s.x*aspect,s.y,s.z);
		}
	}

	// Use this for initialization
	void Start () {

		RenderSettings.ambientLight = Color.gray;

		todayLight = new GameObject("TodayLight");
		todayLight.AddComponent<Light>();
		todayLight.light.type = LightType.Spot;
		todayLight.light.color = new Color(0.5f,0.5f,1.0f);
		todayLight.light.intensity = 8.0f;
		todayLight.light.spotAngle = 5f;
		todayLight.light.range = 400;
		todayLight.light.transform.position = new Vector3(0, 1, -300);
		todayLight.light.transform.LookAt (new Vector3(0,0,0));

		cubeLight = new GameObject("BackLight");
		cubeLight.AddComponent<Light>();
		cubeLight.light.type = LightType.Spot;
		cubeLight.light.color = Color.white;
		cubeLight.light.intensity = 1.0f;
		cubeLight.light.spotAngle = 100f;
		cubeLight.light.range = 400;
		cubeLight.light.transform.position = new Vector3(-100, 50, -50);
		cubeLight.light.transform.LookAt (new Vector3(-70,0,0));

		myCamera = new GameObject ("MyCamera");
		myCamera.transform.position = new Vector3(0, 1, -300);
		myCamera.transform.rotation = new Quaternion (0, 0, 0, 1);
		myCamera.AddComponent<Camera>();

		cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.name = "Cube_script";
		cube.transform.Translate (-70, 0, 0);
		cube.transform.localScale = new Vector3(50,50,50);

		plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
		plane.name = "Plane_script";

		plane.transform.localScale = new Vector3(20,1,20);
		plane.transform.position = new Vector3(0, 0, 0);
		plane.transform.rotation = Quaternion.Euler (90, -180, 0);

		arial = (Font)Resources.GetBuiltinResource<Font> ("Arial.ttf");

		StartCoroutine (Download (plane,"http://pronama.github.io/mascot-apps-contest/2014/images/characters/all.jpg"));
		StartCoroutine (Download (cube,"http://pronama.github.io/mascot-apps-contest/2014/images/image-cut2.png"));

		StartCoroutine ("SpawnCalendar");
	}
	
	// Update is called once per frame
	void Update () {
		cube.transform.Rotate (cubeAngle * Time.deltaTime);

		int i = dt.Day;
		GameObject obj = GameObject.Find ("word"+i);
		if (obj != null) {
			obj.transform.Rotate (angle*Time.deltaTime);
			todayLight.light.transform.LookAt(obj.transform.position);
			float j = (float)Math.Abs(Math.Cos(Time.realtimeSinceStartup)*8);
			todayLight.light.intensity = j;
		}
	}

	GameObject CreateWord(string str,Vector3 pos,Quaternion q){
		GameObject word = new GameObject("word"+str);
		
		word.AddComponent<TextMesh>();
		MeshRenderer meshRender = word.GetComponent<MeshRenderer>();
		meshRender.material = arial.material;
		
		TextMesh textMesh = word.GetComponent<TextMesh>();
		textMesh.font = arial;
		textMesh.text = str;
		textMesh.color = Color.green;
		textMesh.characterSize = 1;
		textMesh.anchor = TextAnchor.MiddleCenter;
		textMesh.alignment = TextAlignment.Left;
		textMesh.fontSize = 150;
		textMesh.fontStyle = FontStyle.Normal;
		word.transform.localScale = new Vector3(1,1,1);
		word.transform.position = pos;
		word.transform.rotation = q;

		return word;
	}


	void SpawnCalendar(){
		var deltaX = 20;
		var deltaY = -20;
		dt = DateTime.Today;
		var head = new DateTime(dt.Year,dt.Month,1);
		var offset = (int)head.DayOfWeek;
		var count = DateTime.DaysInMonth(dt.Year, dt.Month);
		var x = deltaX*offset;
		var y = 50;

		for (var i = 1; i <= count; i++){
			Vector3 pos = new Vector3(x, y, 0 );
			Quaternion q = new Quaternion(0, 0, 0, 1);
			CreateWord(i.ToString(),pos,q);

			if ((i+offset)%7==0){
				x = 0;
				y += deltaY;
			} else {
				x += deltaX;
			}
		}
		{
			Vector3 pos = new Vector3 (deltaX*3, 80, 0);
			Quaternion q = new Quaternion (0, 0, 0, 1);
			CreateWord(dt.ToString("MMMM"),pos,q);

		}
	}
}
