using UnityEngine;

public class CanvasConfirmExit : MonoBehaviour
{

    void Awake() {
        // Nos aseguramos de que el cuadro de confirmación esté desactivado al cargar la escena
        gameObject.SetActive(false);
    }

}
