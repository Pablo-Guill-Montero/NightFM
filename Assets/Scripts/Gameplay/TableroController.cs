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
    private GameObject[] _luces;
    private GameObject[] _bordes;

    private void Awake()
    {
        _casillas = new GameObject[padreCasillas.transform.childCount];
        for (int i = 0; i < padreCasillas.transform.childCount; i++)
        {
            _casillas[i] = padreCasillas.transform.GetChild(i).gameObject;
            _casillas[i].SetActive(false);
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
