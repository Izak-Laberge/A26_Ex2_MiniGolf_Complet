using UnityEngine;

public class TourneMoulin : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    [SerializeField] private Vector3 axeRotation = Vector3.up;
    [SerializeField] private float vitesseRotation = 90f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axeRotation * vitesseRotation * Time.deltaTime);
    }
}