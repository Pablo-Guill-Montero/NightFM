using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Necesario para el Slider
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    
    [Header("Referencias UI")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Slider progressBar; // Arrastra tu Slider aquí
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Importante: No uses SetActive(false) aquí nunca.
            // Usa el CanvasGroup para que el script siga vivo.
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            if(progressBar != null) progressBar.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Ya hay una instancia, destruyendo duplicado.");
            Destroy(gameObject);
            return; // <--- ESTO ES VITAL
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(TransitionCoroutine(sceneName));
    }

    private IEnumerator TransitionCoroutine(string sceneName)
    {
        yield return StartCoroutine(Fade(1));
        
        // Mostramos la barra de progreso al empezar la carga
        progressBar.gameObject.SetActive(true);
        progressBar.value = 0;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        
        // Evitamos que la escena se active sola para que el usuario vea el 100%
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Normalizamos el progreso: 0.9f en Unity es "listo"
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            // progressBar.value = progressValue;
            progressBar.value = Mathf.MoveTowards(progressBar.value, progressValue, Time.deltaTime);

            // Si llegó al 90% (que es nuestro 100%), permitimos la entrada
            if (operation.progress >= 0.9f)
            {
                progressBar.value = 1f;
                yield return new WaitForSeconds(0.2f); // Breve pausa estética
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        progressBar.gameObject.SetActive(false); // Ocultamos la barra
        yield return StartCoroutine(Fade(0));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
        canvasGroup.blocksRaycasts = (targetAlpha == 1);
    }
}