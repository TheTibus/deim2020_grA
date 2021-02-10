﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{


    public GameObject InitGame;
    private InitGame initGame;
    public float speednave;
    [SerializeField] MeshRenderer visibilidadnave;
    [SerializeField] MeshRenderer visibilidadesfera;
    [SerializeField] MeshRenderer inmunidad;
    [SerializeField] BoxCollider boxcollidernave;
    [SerializeField] BoxCollider boxcollideresfera;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Animator animator;

    //Componentes nave para destrucción.
    public AudioClip golpe;
    public AudioClip explosion;
    [SerializeField] GameObject Lucesyparticulas;
    
    void Start()
    {
        initGame = InitGame.GetComponent<InitGame>();
        transform.position = new Vector3(0, 2, 0);
        speednave = 10;
        audioSource = GetComponent<AudioSource>();
        
        
        
    }

    
    void Update()
    {
        //Método para mover la nave con el joystick
        MoverNave();
       
       //Restar vidas al golpear.
       RestarVidas();

       //Parar el juego al llevar 0 vidas.
        AliveOrDead();

        //Ayuda al jugador si llega a más de 500 de puntuación.
        AyudaJugadror();
    }

    
//Movimiento de la nave con el mando. Restricciones y rotación.
    void MoverNave()
        {
       
        float posX = transform.position.x;
        float posY = transform.position.y;
        float desplY = Input.GetAxis("Vertical");
        float desplX = Input.GetAxis("Horizontal");

        //Restringir movimiento en el eje X y parte del codigo de Iris (SpaceWorld y rotación)
        if (posX < 14 && posX > -14 || posX < -14 && desplX > 0 || posX > 14 && desplX < 0)
        {
             transform.Translate(Vector3.right * Time.deltaTime * speednave * desplX, Space.World);
        }

//Restringir movimiento en el eje Y
        if (posY < 10 && posY > 1 || posY < 1 && desplY > 0 || posY > 10 && desplY < 0)
        {
            transform.Translate(Vector3.up * Time.deltaTime * speednave * desplY, Space.World);
        }
         
//Rotación nave
         transform.rotation = Quaternion.Euler(desplY * -10, 0 , desplX * -20);
         
        }

//Detección de colisión solo con los coches, restar las vidas del jugador y sonidos de choque y explosión.
   
    void OnTriggerEnter(Collider target)
        {

            if(target.gameObject.tag == "Enemigo")
            {

            initGame.vidas--;
            print(initGame.vidas);



            if (initGame.vidas >= 1)
            {
                audioSource.PlayOneShot(golpe, 0.7f);
                StartCoroutine("ParpadeoNave");
            }
            else if (initGame.vidas == 0)
            {
                audioSource.PlayOneShot(explosion, 0.3f);
            }
        }
            
           
        }
//Confirma la muerte del personaje  
   
    void RestarVidas()
        {

         if (initGame.vidas == 0)
        {
        
         initGame.alive = false;
                 
        }
        }
//Pregunta al juego si estas vivo o muerto para actuar.
   
    void AliveOrDead()
        {
        if (initGame.alive == false)
        { 
            speednave = 0;
            visibilidadnave.enabled = false;
            visibilidadesfera.enabled = false;
            Destroy(Lucesyparticulas);    
        }
         }    


//Parpadeo de nave mediante animación.

    IEnumerator ParpadeoNave()
    {
        for (int n = 0; n < 3; n++)
        {           
            boxcollideresfera.enabled = false;
            inmunidad.enabled = true;
            visibilidadnave.enabled = false;
            visibilidadesfera.enabled = false;
            yield return new WaitForSeconds(0.1f);
            visibilidadnave.enabled = true;
            visibilidadesfera.enabled = true;
            yield return new WaitForSeconds(0.1f);
            print("funciono");
        }
        yield return new WaitForSeconds(2f);
        boxcollideresfera.enabled = true;
        inmunidad.enabled = false;
    }
    void AyudaJugadror()
    {

        if (initGame.velocidadnaves == 50)
        {
            speednave = 15;
        }
    }

}

