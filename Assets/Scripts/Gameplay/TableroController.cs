using UnityEngine;

[System.Serializable]
public class DatosCasilla {
    public string nombreCasilla; // Opcional, para identificarla en el Inspector
    public Sprite[] variantes;   // Aquí guardas tus 4 sprites
}

public class TableroController : MonoBehaviour
{
    [SerializeField] private GameObject padreCasillas;
    [SerializeField] private GameObject padreLuces;
    [SerializeField] private GameObject padreBordes;

    [Header("Sprites para las casillas y las luces bordes de los cuadros")]
    // Definimos una matriz de 9 casillas (3x3)
    // Usamos un array simple de 9 elementos para que sea fácil de ver en el Inspector,
    // o una clase que agrupe filas si prefieres orden visual total.
    [SerializeField] private DatosCasilla[] spritesCasillas = new DatosCasilla[9];

    // Ejemplo para acceder a un sprite específico:
    // Sprite s = casillas[indice].variantes[numeroDeSprite];
    [SerializeField] private DatosCasilla[] spritesBordes = new DatosCasilla[9];

    private GameObject[] _casillas;
    private SpriteRenderer[] _renderersCasillas; // Caché de renderers para optimizar
    private GameObject[] _luces;
    private GameObject[] _bordes;

    private void Awake()
    {
        // Inicializamos arrays y guardamos referencias a los SpriteRenderers
        _casillas = new GameObject[padreCasillas.transform.childCount];
        _renderersCasillas = new SpriteRenderer[_casillas.Length];
        
        for (int i = 0; i < _casillas.Length; i++)
        {
            _casillas[i] = padreCasillas.transform.GetChild(i).gameObject;
            _renderersCasillas[i] = _casillas[i].GetComponent<SpriteRenderer>();
            // Empezamos con todas las casillas activas pero el sprite 0
        }

        _luces = new GameObject[padreLuces.transform.childCount];
        for (int i = 0; i < padreLuces.transform.childCount; i++)
        {
            _luces[i] = padreLuces.transform.GetChild(i).gameObject;
            _luces[i].SetActive(false);
        }

        _bordes = new GameObject[padreBordes.transform.childCount];
        for (int i = 0; i < padreBordes.transform.childCount; i++)
        {
            _bordes[i] = padreBordes.transform.GetChild(i).gameObject;
            _bordes[i].SetActive(false);
        }
    }

    private void OnEnable()
    {
        // Nos suscribimos al evento del Compositor
        Composer.OnGridUpdate += ActualizarTablero;
    }

    private void OnDisable()
    {
        // Siempre desuscribirse para evitar errores
        Composer.OnGridUpdate -= ActualizarTablero;
    }

    private void ActualizarTablero(int[] estados)
    {
        for (int i = 0; i < estados.Length; i++)
        {
            int estado = estados[i];

            // --- LÓGICA DE CASILLAS (5 SPRITES) ---
            _casillas[i].SetActive(true); // Siempre activas para ver el gris
            
            if (estado == 0) {
                _renderersCasillas[i].sprite = spritesCasillas[i].variantes[0]; // Sprite Gris
            } 
            else if (estado % 2 != 0) { 
                // Si es 1, 3, 5 o 7, calculamos el índice del sprite (1, 2, 3, 4)
                int spriteIdx = (estado / 2) + 1; 
                if (spriteIdx < spritesCasillas[i].variantes.Length)
                    _renderersCasillas[i].sprite = spritesCasillas[i].variantes[spriteIdx];
            }

            // --- LÓGICA DE BORDES (4 SPRITES) ---
            if (estado >= 1) {
                _bordes[i].SetActive(true);
                // Mapeamos los estados (1-2), (3-4), (5-6), (7) a los 4 sprites de bordes
                int bordeIdx = Mathf.Clamp((estado - 1) / 2, 0, 3);
                _bordes[i].GetComponent<SpriteRenderer>().sprite = spritesBordes[i].variantes[bordeIdx];
            } else {
                _bordes[i].SetActive(false);
            }

            // --- LUCES (SOLO ESTADO 7) ---
            _luces[i].SetActive(estado == 7);
        }
    }
}
