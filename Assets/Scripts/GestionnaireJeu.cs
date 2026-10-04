using UnityEditor.EditorTools;
using UnityEditor.Rendering;
using UnityEngine;

public enum EtatJeu
{
    debut, 

    jeu,

    fin,

    mort,

    pause
}
public class GestionnaireJeu : MonoBehaviour
{
    public static GestionnaireJeu instance;
    //public string etat;
    public EtatJeu etat;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(this.gameObject);
        }
        etat = EtatJeu.jeu;
    }

    public void TerminerJeu()
    {
        etat = EtatJeu.fin;
        //changer mon etat et bloquer la balle
        //Déclencher une couroutine animer
        //Changer Scene
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
