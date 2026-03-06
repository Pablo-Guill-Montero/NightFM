using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenusController_Gameplay : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject _confirmExitCanvas; 
    [SerializeField] private GameObject _pauseCanvas; 
    [SerializeField] private GameObject _finPartidaCanvas; 
    private CanvasGroup _pauseCanvasGroup;
    private CanvasGroup _ConfirmExitCanvasGroup; 
    private CanvasGroup _FinPartidaCanvasGroup;
    private FinPartidaCanvasController _finPartidaController;

    [SerializeField] private AudioMixer _audioMixer; // Referencia al AudioMixer para controlar el volumen
    private AudioMixerSnapshot _snapshotPausa; // Snapshot para aplicar al abrir el menú de confirmación
    private AudioMixerSnapshot _snapshotGameplay; // Snapshot para volver a la normalidad al cerrar el menú de confirmación
    private AudioMixerSnapshot _snapshotMenu; // Snapshot para aplicar al salir al menú principal

    void Start()
    {
        // Aseguramos que el cuadro de confirmación esté oculto al iniciar
        _confirmExitCanvas.SetActive(false);
        _pauseCanvas.SetActive(false);
        _finPartidaCanvas.SetActive(false);
        _ConfirmExitCanvasGroup = _confirmExitCanvas.GetComponent<CanvasGroup>();
        _pauseCanvasGroup = _pauseCanvas.GetComponent<CanvasGroup>();
        _FinPartidaCanvasGroup = _finPartidaCanvas.GetComponent<CanvasGroup>();

        _finPartidaController = _finPartidaCanvas.GetComponent<FinPartidaCanvasController>();

        // Obtenemos los snapshots del AudioMixer
        _snapshotPausa = _audioMixer.FindSnapshot("Pausa");
        _snapshotGameplay = _audioMixer.FindSnapshot("Gameplay");
        _snapshotMenu = _audioMixer.FindSnapshot("Menu");

    }

    public void AbrirFin()
    {
        _finPartidaCanvas.gameObject.SetActive(true);
        _FinPartidaCanvasGroup.alpha = 1f;
        _FinPartidaCanvasGroup.interactable = true;
        _FinPartidaCanvasGroup.blocksRaycasts = true;
        _snapshotPausa.TransitionTo(0.4f);
        // Mostramos el canvas de fin de partida y pausamos el juego
        _finPartidaController.MostrarFinPartida();
    }

    public void AbrirMenuPausa()
    {
        _pauseCanvas.gameObject.SetActive(true);
        _pauseCanvasGroup.alpha = 1f;
        _pauseCanvasGroup.interactable = true;
        _pauseCanvasGroup.blocksRaycasts = true;
        _snapshotPausa.TransitionTo(0.4f);
        // Aquí podrías agregar lógica adicional para pausar el juego, como detener el tiempo
        Time.timeScale = 0f; // Pausa el juego
    }

    public void CerrarMenuPausa()
    {
        _pauseCanvas.gameObject.SetActive(false);
        _pauseCanvasGroup.alpha = 0f;
        _pauseCanvasGroup.interactable = false;
        _pauseCanvasGroup.blocksRaycasts = false;

        _snapshotGameplay.TransitionTo(0.4f); // Transición rápida para volver a la normalidad
        // Aquí podrías agregar lógica adicional para reanudar el juego, como reanudar el tiempo
        Time.timeScale = 1f; // Reanuda el juego
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
        _pauseCanvasGroup.interactable = false;
        // 'blocksRaycasts' falso hace que el ratón "atraviese" el fondo si fuera necesario
        _pauseCanvasGroup.blocksRaycasts = false;
    }

    // Esta función cierra la confirmación y desbloquea el fondo
    public void CerrarConfirmacion()
    {
        // 1. Ocultamos el cuadro de confirmación
        _confirmExitCanvas.SetActive(false);
        _ConfirmExitCanvasGroup.alpha = 0f; // Aseguramos que el cuadro de confirmación sea invisible
        _ConfirmExitCanvasGroup.interactable = false; // Desactivamos la interacción con el

        // 2. Devolvemos la interacción al fondo
        _pauseCanvasGroup.interactable = true;
        _pauseCanvasGroup.blocksRaycasts = true;
    }

    public void SalirAlMenuPrincipal()
    {
        Debug.Log("Saliendo al menú principal...");
        Time.timeScale = 1f;
        _snapshotMenu.TransitionTo(0f); 
        SceneLoader.Instance.LoadScene("Menu_Main");
    }

}
