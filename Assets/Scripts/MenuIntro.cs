using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuIntro : MonoBehaviour
{

    int dernierScore;

    [SerializeField] TMP_Text texteScore;
    private void Start()
    {
        if (PlayerPrefs.HasKey("score"))
        {
            dernierScore = PlayerPrefs.GetInt("score");
        }
        else
        {
            dernierScore = 0;
        }
        texteScore.text = $"Dernier score: {dernierScore} coups";   
    }
    public void Demarrer()
    {
        SceneManager.LoadScene("Jeu");
    }
}
