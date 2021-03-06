using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{

//Comunicación básica entre scripts.
    public GameObject InitGame;
    private InitGame initGame;
    public GameObject UI;
    private UI ui;

//Variables útiles para la nave.
    public float speednave;
    [SerializeField] MeshRenderer visibilidadnave;
    [SerializeField] MeshRenderer visibilidadesfera;
    [SerializeField] MeshRenderer inmunidad;
    [SerializeField] BoxCollider boxcollidernave;
    [SerializeField] BoxCollider boxcollideresfera;
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject GameOverCanvas;
    [SerializeField] SpriteRenderer visibilidadExplosion;
    [SerializeField] GameObject explosionparticulas;
    [SerializeField] GameObject explosionparticulas2;
    public GameObject[] spritesVidas;
    public int variablemuerto;
    
//Componentes nave para destrucción.
    public AudioClip golpe;
    public AudioClip explosion;
    public AudioClip lowHp;
    [SerializeField] GameObject Lucesyparticulas;
    
    
    void Start()
    {
        ui = UI.GetComponent<UI>();
        initGame = InitGame.GetComponent<InitGame>();
        variablemuerto = 1;
        transform.position = new Vector3(0, 2, 0);
        speednave = 10;
        audioSource = GetComponent<AudioSource>();
        GameOverCanvas.SetActive (false);
        visibilidadExplosion.enabled = false;
        StartCoroutine("lowHPSound");
        explosionparticulas.SetActive(false);
        explosionparticulas2.SetActive(false);
        
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

        //Destruir Sprites de los escudos
        DestruirVidas();
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
//
    //Restringir movimiento en el eje Y

        if (posY < 10 && posY > 1 || posY < 1 && desplY > 0 || posY > 10 && desplY < 0)
        {
            transform.Translate(Vector3.up * Time.deltaTime * speednave * desplY, Space.World);
        }
    //
    //Rotación nave

         transform.rotation = Quaternion.Euler(desplY * -10 * variablemuerto, 0 , desplX * -20 * variablemuerto);
         
        }
//

//Detección de colisión solo con los coches, restar las vidas del jugador y sonidos de choque y explosión.

   
    void OnTriggerEnter(Collider target)
        {

            if(target.gameObject.tag == "Enemigo")
            {

            initGame.vidas--;
            print(initGame.vidas + (" son tus vidas"));
            


            if (initGame.vidas >= 1)
            {
                audioSource.PlayOneShot(golpe, 0.7f);
                StartCoroutine("ParpadeoNave");
                
            }
            else if (initGame.vidas == 0)
            {

                ui.puntuacionfinal = ui.puntuacion;
                StopCoroutine("lowHPSound");
                audioSource.PlayOneShot(explosion, 0.3f);
                explosionparticulas.SetActive(true);
                explosionparticulas2.SetActive(true);

            }
        }         
        }
//

//Confirma la muerte del personaje y cambia booleana.
   
    void RestarVidas()
        {

         if (initGame.vidas == 0)
        {
        
         initGame.alive = false;
                 
        }
        }
//

//Pregunta al juego si estas vivo o muerto para actuar.
   
    void AliveOrDead()
        {
        if (initGame.alive == false)
        { 
            variablemuerto = 0;
            speednave = 0;
            visibilidadnave.enabled = false;
            visibilidadesfera.enabled = false;
            Destroy(Lucesyparticulas);    
            visibilidadExplosion.enabled = true;
            Invoke ("UIGameOver", 3f);
           
        }
         }    

//

//Parpadeo de nave mediante animación y corrutina.

    IEnumerator ParpadeoNave()
    {
        for (int n = 0; n < initGame.vidas; n++)
        {           
            boxcollideresfera.enabled = false;
            inmunidad.enabled = true;
            visibilidadnave.enabled = false;
            visibilidadesfera.enabled = false;
            yield return new WaitForSeconds(0.1f);
            visibilidadnave.enabled = true;
            visibilidadesfera.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2f);
        boxcollideresfera.enabled = true;
        inmunidad.enabled = false;
    }
//

// Ayuda al jugador para poder esquivar mejor los obstaculos más rápidos.
    void AyudaJugadror()
    {

        if (initGame.velocidadnaves == 50)
        {
            speednave = 75;
        }
    }
//

//Codigo de Daniel Ocasar // Destrucción de Sprites al golpear.

    void DestruirVidas()
    {
        if (initGame.vidas < 4 && initGame.vidas >= 3 )
        {
            Destroy(spritesVidas[0].gameObject);
        
        }

        else if (initGame.vidas < 3 && initGame.vidas >= 2)
        {
            Destroy(spritesVidas[1].gameObject);
        }

        else if (initGame.vidas < 2 && initGame.vidas >= 1)
        {
            Destroy(spritesVidas[2].gameObject);
            
        }
        
    }
//

// Sonido cuando te queda 1HP (Corrutina)

    IEnumerator lowHPSound()
    {
        for (int i = 0; ; i++)
        {
            if (initGame.vidas < 2)
                {
            
                    audioSource.PlayOneShot(lowHp,3f);
                    print("FuncionoAudioLow");
                 } 
                 yield return new WaitForSeconds(0.65f);
        }
        yield return new WaitForSeconds(0.5f);
    }
//   

// Activación del canvas GAME OVER

    void UIGameOver(){

       GameOverCanvas.SetActive(true);
    }

}
/

