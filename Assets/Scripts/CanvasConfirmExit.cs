using UnityEngine;

public class CanvasConfirmExit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Desactivamos el cuadro de confirmación al iniciar
        gameObject.SetActive(false);   
    }
}
