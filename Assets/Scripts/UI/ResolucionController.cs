using UnityEngine;
using TMPro;

public class ResolucionController : MonoBehaviour
{
    public  TextMeshProUGUI textoResolucion;
    private Resolution[] resoluciones;
    private int indiceActual = 0;

    void Start()
    {
        // Obtiene las resoluciones disponibles del monitor
        resoluciones = Screen.resolutions;
        // Busca cuál es la resolución actual para empezar desde ahí
        for (int i = 0; i < resoluciones.Length; i++) {
            if (resoluciones[i].width == Screen.currentResolution.width && 
                resoluciones[i].height == Screen.currentResolution.height) {
                indiceActual = i;
            }
            // Debug.Log("Resolsución añadida: " + resoluciones[i].width + " x " + resoluciones[i].height);
        }
        ActualizarInterfaz();
    }

    public void Siguiente() {
        indiceActual = (indiceActual + 1) % resoluciones.Length;
        AplicarYActualizar();
    }

    public void Anterior() {
        indiceActual--;
        if (indiceActual < 0) indiceActual = resoluciones.Length - 1;
        AplicarYActualizar();
    }

    void AplicarYActualizar() {
        Resolution res = resoluciones[indiceActual];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        Debug.Log("Resolución aplicada: " + res.width + " x " + res.height);
        ActualizarInterfaz();
    }

    void ActualizarInterfaz() {
        textoResolucion.text = resoluciones[indiceActual].width + " x " + resoluciones[indiceActual].height;
    }
}
