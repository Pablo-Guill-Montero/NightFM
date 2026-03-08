using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class MenusController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject _confirmExitCanvas; // GameObject para poder activarlo y desactivarlo
    [SerializeField] private CanvasGroup _mainMenuCanvas; // CanvasGroup para controlar la interacción del fondo
    [SerializeField] private GameObject _idiomasCanvas; 

    private CanvasGroup _ConfirmExitCanvasGroup; // CanvasGroup del cuadro de confirmación para controlar su interacción
    private CanvasGroup _IdiomasCanvasGroup; 

    // Arrastra el Snapshot desde la ventana de FMOD Event Browser al Inspector
    public EventReference radioSnapshot; 
    public EventReference normalSnapshot;
    public EventReference pauseSnapshot;

    private EventInstance radioSnapshotInstance;
    private EventInstance pauseSnapshotInstance;


    void Start()
    {
        // Aseguramos que el cuadro de confirmación esté oculto al iniciar
        _confirmExitCanvas.SetActive(false);
        _ConfirmExitCanvasGroup = _confirmExitCanvas.GetComponent<CanvasGroup>();
        _idiomasCanvas.SetActive(false);
        _IdiomasCanvasGroup = _idiomasCanvas.GetComponent<CanvasGroup>();
    
        // Creamos la instancia del snapshot para poder controlarlo
        radioSnapshotInstance = RuntimeManager.CreateInstance(radioSnapshot);
        pauseSnapshotInstance = RuntimeManager.CreateInstance(pauseSnapshot);
    }

    private void OnDestroy()
    {
        // Liberamos la memoria de FMOD al destruir el script
        radioSnapshotInstance.release();
        pauseSnapshotInstance.release();
    }

    public void ActivarModoRadio(bool activar)
    {
        if (activar)
            radioSnapshotInstance.start(); // Inicia el snapshot de radio
        else
            radioSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); // Detiene el snapshot de radio con fade out
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
        pauseSnapshotInstance.start();

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
        pauseSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

    }

    public void AbrirIdiomas()
    {
        // 1. Mostramos el cuadro de idiomas
        _idiomasCanvas.SetActive(true);
        _IdiomasCanvasGroup.alpha = 1f;
        _IdiomasCanvasGroup.interactable = true; 
        // 2. Bloqueamos la interacción del fondo
        _mainMenuCanvas.interactable = false;
        _mainMenuCanvas.blocksRaycasts = false;

        pauseSnapshotInstance.start(); 
    }

    public void CerrarIdiomas()
    {
        // 1. Ocultamos el cuadro de idiomas
        _idiomasCanvas.SetActive(false);
        _IdiomasCanvasGroup.alpha = 0f;
        _IdiomasCanvasGroup.interactable = false; 

        // 2. Devolvemos la interacción al fondo
        _mainMenuCanvas.interactable = true;
        _mainMenuCanvas.blocksRaycasts = true;

        pauseSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

}
