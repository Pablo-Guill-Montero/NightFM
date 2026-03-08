using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using FMODUnity;
using FMOD.Studio;
// using System; // Para usar Action

public class MenusController_Gameplay : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject _confirmExitCanvas; 
    [SerializeField] private GameObject _pauseCanvas; 
    [SerializeField] private GameObject _finPartidaCanvas; 
    [SerializeField] private GameObject _idiomasCanvas; 

    [SerializeField] private GameObject _reiniciarCanvas; 

    private CanvasGroup _pauseCanvasGroup;
    private CanvasGroup _ConfirmExitCanvasGroup; 
    private CanvasGroup _FinPartidaCanvasGroup;
    private CanvasGroup _IdiomasCanvasGroup; 
    private CanvasGroup _ReiniciarCanvasGroup; 
    
    private FinPartidaCanvasController _finPartidaController;


    // Arrastra el Snapshot desde la ventana de FMOD Event Browser al Inspector
    // public EventReference radioSnapshot; 
    // public EventReference normalSnapshot;
    // public EventReference pauseSnapshot;
    // private EventInstance radioSnapshotInstance;
    // private EventInstance normalSnapshotInstance;
    // private EventInstance pauseSnapshotInstance;

    public MusicPlayer musicPlayer; // Arrastra aquí tu MusicPlayer para poder pausar la música desde este script

    private int _menuState = 0; // 0 = Sin menú, 1 = Menú de pausa, 2 = Confirmación de salida, 3 = Fin de partida

    private void OnEnable()
    {
        // Empezamos a escuchar la señal
        PlayerInput.OnInputPressed += ResponderAlPlayerInput;
        Referee.GameEndEvent += AbrirFin; // Nos suscribimos al evento de fin de juego para abrir el canvas de fin de partida
    }

    private void OnDisable()
    {
        // Dejamos de escuchar para evitar errores de memoria
        PlayerInput.OnInputPressed -= ResponderAlPlayerInput;
        Referee.GameEndEvent -= AbrirFin; // Nos desuscribimos del evento de fin de juego al desactivar el script
    }

    // private void OnDestroy()
    // {
    //     // Liberamos la memoria de FMOD al destruir el script
    //     radioSnapshotInstance.release();
    //     normalSnapshotInstance.release();
    //     pauseSnapshotInstance.release();
    // }

    void Start()
    {
        // Aseguramos que el cuadro de confirmación esté oculto al iniciar
        _confirmExitCanvas.SetActive(false);
        _pauseCanvas.SetActive(false);
        _finPartidaCanvas.SetActive(false);
        _idiomasCanvas.SetActive(false);
        _reiniciarCanvas.SetActive(false);

        _ConfirmExitCanvasGroup = _confirmExitCanvas.GetComponent<CanvasGroup>();
        _pauseCanvasGroup = _pauseCanvas.GetComponent<CanvasGroup>();
        _FinPartidaCanvasGroup = _finPartidaCanvas.GetComponent<CanvasGroup>();
        _IdiomasCanvasGroup = _idiomasCanvas.GetComponent<CanvasGroup>();
        _ReiniciarCanvasGroup = _reiniciarCanvas.GetComponent<CanvasGroup>();

        _finPartidaController = _finPartidaCanvas.GetComponent<FinPartidaCanvasController>();

        // Creamos la instancia del snapshot para poder controlarlo
        // radioSnapshotInstance = RuntimeManager.CreateInstance(radioSnapshot);
        // normalSnapshotInstance = RuntimeManager.CreateInstance(normalSnapshot);
        // pauseSnapshotInstance = RuntimeManager.CreateInstance(pauseSnapshot);

        // normalSnapshotInstance.start(); // Empezamos con el snapshot normal al iniciar el juego
    }

    void ResponderAlPlayerInput(string accion)
    {
        switch (accion)
        {
            case "Escape":
                if (_menuState == 0)
                {
                    AbrirMenuPausa();
                }
                // else 
                // if (_menuState == 1)
                // {
                //     CerrarMenuPausa();
                // }
                else if (_menuState == 2)
                {
                    CerrarConfirmacion();
                }
                break;
        }
    }

    // // Método auxiliar para limpiar snapshots antes de poner una nueva
    // private void LimpiarSnapshots()
    // {
    //     radioSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    //     normalSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    //     pauseSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    // }

    public void AbrirFin(bool playerWon)
    {
        _finPartidaCanvas.gameObject.SetActive(true);
        _FinPartidaCanvasGroup.alpha = 1f;
        _FinPartidaCanvasGroup.interactable = true;
        _FinPartidaCanvasGroup.blocksRaycasts = true;
        // Mostramos el canvas de fin de partida y pausamos el juego
        _finPartidaController.MostrarFinPartida();

        // LimpiarSnapshots();
        // pauseSnapshotInstance.start(); // Enchufar la snapshot de pausa al abrir el canvas de fin de partida

    }


    // Menús


    public void AbrirMenuPausa()
    {
        _pauseCanvas.gameObject.SetActive(true);
        _pauseCanvasGroup.alpha = 1f;
        DevolverInteraccionFondo(); // Aseguramos que el fondo sea interactuable para el menú de pausa
        // Aquí podrías agregar lógica adicional para pausar el juego, como detener el tiempo
        Time.timeScale = 0f; // Pausa el juego
        
        // LimpiarSnapshots();
        // pauseSnapshotInstance.start(); // Enchufar la snapshot de pausa al abrir el menú de pausa

        musicPlayer.pauseMusic(); // Pausamos la música al abrir el menú de pausa

        _menuState = 1;
    }

    public void CerrarMenuPausa()
    {
        _pauseCanvas.gameObject.SetActive(false);
        _pauseCanvasGroup.alpha = 0f;
        QuitarInteraccionFondo(); // Aseguramos que el fondo no sea interactuable mientras el menú está cerrado

        // Aquí podrías agregar lógica adicional para reanudar el juego, como reanudar el tiempo
        Time.timeScale = 1f; // Reanuda el juego

        // LimpiarSnapshots();
        // normalSnapshotInstance.start(); // Vuelve al snapshot normal al cerrar el menú de pausa

        musicPlayer.unPauseMusic(); // Reanudamos la música al cerrar el menú de pausa  

        _menuState = 0;
    }

    // Esta función activa la confirmación y bloquea el fondo
    public void AbrirConfirmacion()
    {
        // 1. Mostramos el cuadro de confirmación
        _confirmExitCanvas.SetActive(true);
        _ConfirmExitCanvasGroup.alpha = 1f; // Aseguramos que el cuadro de confirmación sea visible
        _ConfirmExitCanvasGroup.interactable = true; // Permitimos la interacción con el cuadro

        // 2. Bloqueamos la interacción del fondo
        QuitarInteraccionFondo();

        _menuState = 2;
    }

    // Esta función cierra la confirmación y desbloquea el fondo
    public void CerrarConfirmacion()
    {
        // 1. Ocultamos el cuadro de confirmación
        _confirmExitCanvas.SetActive(false);
        _ConfirmExitCanvasGroup.alpha = 0f; // Aseguramos que el cuadro de confirmación sea invisible
        _ConfirmExitCanvasGroup.interactable = false; // Desactivamos la interacción con el

        // 2. Devolvemos la interacción al fondo
        DevolverInteraccionFondo();

        _menuState = 1;
    }

    public void AbrirIdiomas()
    {
        // 1. Mostramos el cuadro de idiomas
        _idiomasCanvas.SetActive(true);
        _IdiomasCanvasGroup.alpha = 1f;
        _IdiomasCanvasGroup.interactable = true; 
        // 2. Bloqueamos la interacción del fondo
        QuitarInteraccionFondo();

        // pauseSnapshotInstance.start(); 
    }

    public void CerrarIdiomas()
    {
        // 1. Ocultamos el cuadro de idiomas
        _idiomasCanvas.SetActive(false);
        _IdiomasCanvasGroup.alpha = 0f;
        _IdiomasCanvasGroup.interactable = false; 

        // 2. Devolvemos la interacción al fondo
        DevolverInteraccionFondo();
        // pauseSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void AbrirReiniciar()
    {
        // 1. Mostramos el cuadro de idiomas
        _reiniciarCanvas.SetActive(true);
        _ReiniciarCanvasGroup.alpha = 1f;
        _ReiniciarCanvasGroup.interactable = true; 

        // 2. Bloqueamos la interacción del fondo
        QuitarInteraccionFondo();
        // pauseSnapshotInstance.start(); 
    }

    public void CerrarReiniciar()
    {
        // 1. Ocultamos el cuadro de idiomas
        _reiniciarCanvas.SetActive(false);
        _ReiniciarCanvasGroup.alpha = 0f;
        _ReiniciarCanvasGroup.interactable = false; 

        // 2. Devolvemos la interacción al fondo
        DevolverInteraccionFondo();

        // pauseSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void SalirAlMenuPrincipal()
    {
        Debug.Log("Saliendo al menú principal...");
        Time.timeScale = 1f;
        // LimpiarSnapshots();
        // radioSnapshotInstance.start();
        SceneLoader.Instance.LoadScene("Menu_Main");
        _menuState = 0;
    }

    private void QuitarInteraccionFondo()
    {
        _pauseCanvasGroup.interactable = false;
        _pauseCanvasGroup.blocksRaycasts = false;
    }

    private void DevolverInteraccionFondo()
    {
        _pauseCanvasGroup.interactable = true;
        _pauseCanvasGroup.blocksRaycasts = true;
    }

    public void ReiniciarNivel()
    {
        Debug.Log("Reiniciando nivel...");
        Time.timeScale = 1f;
        DesactivarTodosCanvas();
        SceneLoader.Instance.LoadScene("Lvl_01_Gameplay");
    }

    private void DesactivarTodosCanvas()
    {
        DesactivarPauseCanvas();
        DesactivarConfirmExitCanvas();
        DesactivarIdiomaCanvas();
        DesactivarReiniciarCanvas();
        DesactivarFinPartidaCanvas();
    }

    private void DesactivarPauseCanvas()
    {
        _pauseCanvasGroup.alpha = 0f; // Hace el canvas invisible
        _pauseCanvasGroup.interactable = false; // Desactiva la interacción
        _pauseCanvasGroup.blocksRaycasts = false; // Desactiva el bloqueo de raycasts
        _pauseCanvas.gameObject.SetActive(false); // Desactiva el GameObject del canvas
    }

    private void DesactivarConfirmExitCanvas()
    {
        _ConfirmExitCanvasGroup.alpha = 0f;
        _ConfirmExitCanvasGroup.interactable = false;
        _ConfirmExitCanvasGroup.blocksRaycasts = false;
        _confirmExitCanvas.SetActive(false);
    }

    private void DesactivarIdiomaCanvas()
    {
        _IdiomasCanvasGroup.alpha = 0f;
        _IdiomasCanvasGroup.interactable = false;
        _IdiomasCanvasGroup.blocksRaycasts = false;
        _idiomasCanvas.SetActive(false);
    }

    private void DesactivarReiniciarCanvas()
    {
        _ReiniciarCanvasGroup.alpha = 0f;
        _ReiniciarCanvasGroup.interactable = false;
        _ReiniciarCanvasGroup.blocksRaycasts = false;
        _reiniciarCanvas.SetActive(false);
    }

    private void DesactivarFinPartidaCanvas()
    {
        _FinPartidaCanvasGroup.alpha = 0f;
        _FinPartidaCanvasGroup.interactable = false;
        _FinPartidaCanvasGroup.blocksRaycasts = false;
        _finPartidaCanvas.SetActive(false);
    }

}
