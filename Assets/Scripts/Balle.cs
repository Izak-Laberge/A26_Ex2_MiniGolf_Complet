using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.U2D;
using Unity.Mathematics;

public class Balle : MonoBehaviour
{

    [Header("État de jeu")]

    public int nbCoups;
    Vector3 positionBalle;




    [Header("Paramètres de tir")]
    Rigidbody rigidbodyBalle;
    [SerializeField] float angleTir;
    public bool peutJouer;

    [Header("Gauge de force")]
    [SerializeField] float forceTir;
    

    [Header("Input Actions")]
    [SerializeField] float accumulateurForce = 0.1f;

    [SerializeField] Slider jaugeForce;

    

    [Header("InputActions")]

    [SerializeField] InputAction tirAction;
    [SerializeField] InputAction angleAction;


    [Header("Composant")]

    LineRenderer lineRendererBalle;
    [SerializeField] TMP_Text texteCoups;
    AudioSource audioSourceBalle;
    [SerializeField] AudioClip sonErreur;
    [SerializeField] AudioClip sonFin;


    void Start()
    {
        audioSourceBalle = GetComponent<AudioSource>();
        lineRendererBalle = GetComponent<LineRenderer>();
        rigidbodyBalle = GetComponent<Rigidbody>();
        nbCoups = 0;
        MettreAJourUI();
        if (PlayerPrefs.HasKey("dernierePosition"))
        {
            string positionJSON = PlayerPrefs.GetString("dernierePosition");
            transform.position = JsonUtility.FromJson<Vector3>(positionJSON);
        }
        peutJouer = true;
    }

    void Update()
    {

        if (peutJouer == true && GestionnaireJeu.instance.etat == EtatJeu.jeu)
        {
            angleTir += angleAction.ReadValue<float>();
            Vector3 direction = Quaternion.Euler(0, angleTir, 0) * Vector3.forward;
            lineRendererBalle.SetPosition(0, transform.position);
            lineRendererBalle.SetPosition(1, transform.position + direction);

            if (tirAction.WasPressedThisFrame())
            {
                forceTir = 0;
                jaugeForce.value = forceTir;
            }
            if (tirAction.IsPressed())
            {
                forceTir += accumulateurForce;
                forceTir = Mathf.Clamp(forceTir, jaugeForce.minValue, jaugeForce.maxValue);
                jaugeForce.value = forceTir;

            }
            if (tirAction.WasReleasedThisFrame())
            {
                positionBalle = transform.position;
                rigidbodyBalle.AddForce(direction * forceTir * Time.deltaTime, ForceMode.Impulse);
                forceTir = 0;
                jaugeForce.value = forceTir;
                nbCoups++;
                MettreAJourUI();

                string positionJSON = JsonUtility.ToJson(transform.position);
                PlayerPrefs.SetString("dernierePosition", positionJSON);

                StartCoroutine(AttendreFinCoup());
            }
        }


    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "horsParcours")
       {
            rigidbodyBalle.linearVelocity = Vector3.zero;
            transform.position = positionBalle;
            audioSourceBalle.PlayOneShot(sonErreur);
            

        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "trou")
        {
            rigidbodyBalle.linearVelocity = Vector3.zero;
            rigidbodyBalle.angularVelocity = Vector3.zero;
            rigidbodyBalle.useGravity = false;
            audioSourceBalle.PlayOneShot(sonFin);
            transform.position = collision.transform.position;
            PlayerPrefs.SetInt("score",nbCoups);
            GestionnaireJeu.instance.TerminerJeu();
            PlayerPrefs.DeleteKey("dernierePosition");
            SceneManager.LoadScene("Intro");
            Debug.Log("fin");
        }
    }

    // ===================

    IEnumerator AttendreFinCoup()
    {
        peutJouer = false;
        Debug.Log("DÉBUT");
        lineRendererBalle.enabled = false;
        //yield return new WaitForSeconds(2);
        yield return new WaitForFixedUpdate();//  Attend de calculer la physique
        float vitesse = rigidbodyBalle.linearVelocity.magnitude; // Calculer la vitesse présente
        while (vitesse > 0.1f)
        {
            vitesse = rigidbodyBalle.linearVelocity.magnitude;
            yield return null; // Attend à la prochaine frame
        }
        Debug.Log("FIN");
        peutJouer = true;
        lineRendererBalle.enabled = true;
    }

    void FrapperBalle()
    {

    }

    void MettreAJourUI()
    {
        texteCoups.text = $"(Coups: {nbCoups})";
    }

    // IEnumerator FinJeu()
    // {

    // }

    void SauvegarderScore()
    {

    }

    //=================================
    // Gestion des inputs actions
    void OnEnable()
    {
        tirAction.Enable();
        angleAction.Enable();
    }

    void OnDisable()
    {
        tirAction.Disable();
        angleAction.Disable();
    }
}
