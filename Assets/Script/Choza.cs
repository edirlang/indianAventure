﻿using UnityEngine;
using System.Collections;

public class Choza : MonoBehaviour {
	public bool cosntrullendo=false, activarBoton = false;
	public float tiempo = 0;
	public AnimationClip crearChoza;
	public GameObject ubicar_camara, chozaFinal;
	private GameObject player;
	bool crearChozaMulti = false;
	Transform posicionInstanciar, camaraOriginal;
	Animator playerAnimator;
	float tiempoAnimacion;

	// Use this for initialization
	void Start () {
		posicionInstanciar = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(cosntrullendo){
			tiempo -= Time.deltaTime;
			activarBoton = false;
			Camera.main.transform.localPosition = new Vector3(1.71456f,3.25226f,1.54568f);
			Camera.main.transform.rotation = new Quaternion();
			Camera.main.transform.Rotate(50.517f,265.809f,14.878f);
			GameObject chozaLevel;
			if(tiempo > 18 && tiempo < 19)
			{
				chozaLevel = GameObject.Find("choza1");
				chozaLevel.transform.position = new Vector3(posicionInstanciar.position.x  - 1, posicionInstanciar.position.y, posicionInstanciar.position.z);
			}else if (tiempo > 10 && tiempo < 11){
				Destroy(GameObject.Find("choza1"));
				chozaLevel = GameObject.Find("choza2");
				chozaLevel.transform.position = chozaLevel.transform.position = new Vector3(posicionInstanciar.position.x  - 4, posicionInstanciar.position.y, posicionInstanciar.position.z);

			}else if(tiempo > 2 && tiempo < 3){
				Destroy(GameObject.Find("choza2"));
				chozaLevel = GameObject.Find("choza3");
				chozaLevel.transform.position = new Vector3(posicionInstanciar.position.x  - 4, posicionInstanciar.position.y - 2, posicionInstanciar.position.z);

				if(crearChozaMulti){
					NetworkView nw = Camera.main.GetComponent<NetworkView>();
					nw.RPC("crearChozaMultiplayer",RPCMode.OthersBuffered, player.name, posicionInstanciar.position, 2);
					crearChozaMulti = false;
					playerAnimator.SetBool("recojer",false);
					MoverMouse.cambioCamara = false;
				}
			}
		}

		if(tiempo < 0){
			if(cosntrullendo){
				Misiones mision = Camera.main.gameObject.GetComponent<Misiones>();
				mision.procesoMision1(General.paso_mision);
				MoverMouse.movimiento = true;
				Camera.main.transform.rotation = camaraOriginal.rotation;
			}
			cosntrullendo = false;
		}
	}

	void OnGUI(){
		if(activarBoton){
			if(GUI.Button(new Rect(Screen.width/2 - Screen.width/16, Screen.height/2 - Screen.height/32,Screen.width/8,Screen.height/16),"Construir")){
				cosntrullendo = true;
				crearChozaMulti = true;
				tiempo = 20;
				camaraOriginal = Camera.main.transform;
				MoverMouse.cambioCamara = true;
				MoverMouse.movimiento = false;
				playerAnimator.SetBool("recojer",true);
				posicionInstanciar = player.transform;
			}
		}
		if(crearChozaMulti){
			GUI.Label(new Rect(Screen.width/2 - Screen.width/16, Screen.height/2 - Screen.height/32,Screen.width/3,Screen.height/16),"Construyendo...");
		}
	}

	public void OnTriggerEnter(Collider colision){
		if (colision.tag == "Player") {
			player = colision.gameObject;
			playerAnimator = colision.gameObject.GetComponent<Animator>();
			
			if(General.paso_mision == 4 && General.misionActual[0] == "1"){
				activarBoton = true;
			}
		}
	}

	public void OnTriggerExit(Collider colision){
		if (colision.tag == "Player") {
			if(General.paso_mision == 4 && General.misionActual[0] == "1"){
				player = colision.gameObject;
				activarBoton = false;
			}
		}
	}
}