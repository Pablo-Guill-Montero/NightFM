using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenusController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject _confirmExitCanvas; // GameObject para poder activarlo y desactivarlo
    [SerializeField] private CanvasGroup _mainMenuCanvas; // CanvasGroup para controlar la interacción del fondo
    private CanvasGroup _ConfirmExitCanvasGroup; // CanvasGroup del cuadro de confirmación para controlar su interacción

    [SerializeField] private AudioMixer _audioMixer; // Referencia al AudioMixer para controlar el volumen
    private AudioMixerSnapshot _snapshotExit; // Snapshot para aplicar al abrir el menú de confirmación
    private AudioMixerSnapshot _snapshotNormal; // Snapshot para volver a la normalidad al cerrar el menú de confirmación

    void Start()
    {
        // Aseguramos que el cuadro de confirmación esté oculto al iniciar
        _confirmExitCanvas.SetActive(false);
        // Obtenemos los snapshots del AudioMixer
        _snapshotExit = _audioMixer.FindSnapshot("ConfirmExit");
        _snapshotNormal = _audioMixer.FindSnapshot("Menu");
        _ConfirmExitCanvasGroup = _confirmExitCanvas.GetComponent<CanvasGroup>();
    }

    // Esta función activa la confirmación y bloquea el fondo
    public void AbrirConfirmacion()
    {
        // 1. Mostramos el cuadro de confirmación
        _confirmExitCanvas.SetActive(true);
        _ConfirmExitCanvasGroup.alpha = 1f; // Aseguramos que el cuadro de confirmación sea visible
        _ConfirmExitCanvasGroup.interactable = true; // Permitimos la interacción con el cuadro

        // 2. Bloqueamos la interacción del fondo
        // 'interactable' falso evita clicks en botones
        _mainMenuCanvas.interactable = false;
        // 'blocksRaycasts' falso hace que el ratón "atraviese" el fondo si fuera necesario
        _mainMenuCanvas.blocksRaycasts = false;

        // Enchufar la snapshot de pausa
        _snapshotExit.TransitionTo(0.4f); // Transición rápida para sentir el cambio inmediato
    }

    // Esta función cierra la confirmación y desbloquea el fondo
    public void CerrarConfirmacion()
    {
        // 1. Ocultamos el cuadro de confirmación
        _confirmExitCanvas.SetActive(false);
        _ConfirmExitCanvasGroup.alpha = 0f; // Aseguramos que el cuadro de confirmación sea invisible
        _ConfirmExitCanvasGroup.interactable = false; // Desactivamos la interacción con el

        // 2. Devolvemos la interacción al fondo
        _mainMenuCanvas.interactable = true;
        _mainMenuCanvas.blocksRaycasts = true;

        // Volver a la snapshot normal
        _snapshotNormal.TransitionTo(0.4f); // Transición rápida para volver


    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

}
