﻿using UnityEngine;
using System.Collections;

public class Trasportador : MonoBehaviour {
	public string scena;
	public Vector3 posicion;
	float tiempo = 0;
	bool inciarTiempo, inciarTiempoInicio = false;
	// Use this for initialization
	void Start () {
		inciarTiempo = false;
	}
	
	// Update is called once per frame
	void Update () {
		tiempo -= Time.deltaTime;
		if(tiempo < 0 && inciarTiempo){
			if(scena != "level1"){
				GameObject.Find("PlayerJuego").transform.position = GameObject.Find("PlayerJuego2").transform.position;
				GameObject.Find(Network.player.ipAddress).transform.position = GameObject.Find("PlayerJuego").transform.position;
				Destroy(GameObject.Find("Main Camera2"));
				Destroy(GameObject.Find("PlayerJuego2"));
				Destroy(GameObject.Find("Luz2"));
			}else{
				GameObject.Find(Network.player.ipAddress).transform.position = GameObject.Find("PlayerJuego").transform.position;
				Destroy(GameObject.Find("Main Camera"));
			}
			MoverMouse.movimiento = true;
			inciarTiempo = false;
			Destroy(gameObject);
		}

		if(tiempo < 0 && inciarTiempoInicio){
			if(scena != "level1"){
				GameObject.Find("PlayerJuego").transform.position = GameObject.Find("PlayerJuego2").transform.position;
				Destroy(GameObject.Find("Main Camera2"));
				Destroy(GameObject.Find("PlayerJuego2"));
				Destroy(GameObject.Find("Luz2"));
			}else{
				GameObject.Find(Network.player.ipAddress).transform.position = GameObject.Find("PlayerJuego").transform.position;
				Destroy(GameObject.Find("Main Camera"));
			}
			MoverMouse.movimiento = true;
			inciarTiempo = false;
			Destroy(gameObject);
		}
	}

	public void OnTriggerEnter(Collider colision){
		if (colision.tag == "Player") {
			cambiarEscena();
		}
	}

	private void cambiarEscena(){
		MoverMouse.movimiento = false;
		Application.LoadLevelAdditive(scena);
		Destroy(GameObject.Find("Escenario"));
		if(scena != "level1"){
			Destroy(GameObject.Find("fogata"));
			Destroy(GameObject.Find("micos"));
			Destroy(GameObject.Find("chozas"));
			Destroy(GameObject.Find("PlayerJuego"));
		}else{
			
		}
		
		tiempo = 2;
		inciarTiempo = true;
	}

	public void cambiarEscenaSpaw(string scenaCambio){
		scena = scenaCambio;
		Application.LoadLevelAdditive(scenaCambio);
		Destroy(GameObject.Find("Escenario"));
		if(scenaCambio != "level1"){
			Destroy(GameObject.Find("fogata"));
			Destroy(GameObject.Find("micos"));
			Destroy(GameObject.Find("chozas"));

		}
		tiempo = 2;
		inciarTiempoInicio = true;
	}
}
