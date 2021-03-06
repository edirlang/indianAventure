﻿using UnityEngine;
using System.Collections;

public class Maleta : MonoBehaviour {

	// Use this for initialization
	public Texture[] imagenes;
	public bool mostarMaleta = false;
	public static bool vaciar = false;
	string[,] objeto;
	void Start () {
		imagenes = new Texture[6];
		objeto = new string[6,2];
		if(General.misionActual[0]=="2"){
			vaciar = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(vaciar){
			imagenes = new Texture[6];
			vaciar = false;
		}

	}

	void OnGUI(){
		if(mostarMaleta)
		{
			GUI.Box (new Rect (0, 0, Screen.width, Screen.height), "Mi maleta");
			for(int i = 0; i < imagenes.Length; i++)
			{
				if(objeto[i,0] != null)
				{
					if(i < 3)
					{
						GUI.Label(new Rect((i+1)*(Screen.width/4), Screen.height/6,Screen.width/4, Screen.height/4), imagenes[i]);
						GUI.Label(new Rect((i+1)*(Screen.width/4), 2*Screen.height/6,Screen.width/4, Screen.height/10), objeto[i,0]+" x "+objeto[i,1]);
					}
					else
					{
						GUI.Label(new Rect((i-2)*(Screen.width/4), 2*(Screen.height/4),Screen.width/4, Screen.height/4), imagenes[i]);
						GUI.Label(new Rect((i+1)*(Screen.width/4), 3*Screen.height/4,Screen.width/4, Screen.height/10), objeto[i,0]+" x "+objeto[i,1]);
					}
				}
			}
			
			if (GUI.Button (new Rect (Screen.width/2 - Screen.width / 12, 5*(Screen.height/6),Screen.width / 4, Screen.height / 10), "Volver")) {
				mostarMaleta = false;
				MoverMouse.movimiento = true;
			}
		}
	}

	public void agregarTextura(Texture textura){
		for(int i = 0; i < imagenes.Length; i++)
		{
			if(objeto[i,0] == textura.name)
			{
				int cantidad = int.Parse(objeto[i,1]);
				cantidad = cantidad + 1;
				objeto[i,1] = cantidad+"";
				break;
			}else if(objeto[i,0] == null)
			{
				objeto[i,0] = textura.name;
				objeto[i,1] = "1";
				imagenes[i] = textura;
				break;
			}
		}
	}
}