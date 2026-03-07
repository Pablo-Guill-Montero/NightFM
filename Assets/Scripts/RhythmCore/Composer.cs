using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO; // Necesario para leer archivos

// <summary>
// Misión: Escribir las partituras.

// Define qué pasa en cada momento de la canción.
// Es el Level Design Output.
// Según nuestro juego necesitaremos que sea una partitura estática o dinámica (se construye, actualiza, procedural...)
// </summary>
public class Composer : MonoBehaviour
{
    public List<BeatStep> score = new List<BeatStep>();
    private const string FILE_NAME = "MusicScore.json";
    private const int TOTAL_BEATS = 409; // 3:12 a 128 BPM

    // Evento para que el tablero se actualice (pasa la matriz de 9 estados)
    public static event Action<int[]> OnGridUpdate;
    // Evento para indicar que el beat actual es puntuable (tiene un '7')
    public static event Action<int> OnPuntuableBeat;

    [ContextMenu("Generar Plantilla 409 Beats")]
    public void GenerarPlantillaVacia()
    {
        score.Clear();
        for (int i = 0; i < 409; i++)
        {
            BeatStep step = new BeatStep();
            // Inicializamos los 9 estados a 0
            step.gridStates = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            score.Add(step);
        }
        ExportToJSON(); // Esto crea el archivo MusicScore.json en tu carpeta Assets
        Debug.Log("Plantilla de 409 beats creada. ¡A editar!");
    }

    private void OnEnable()
    {
        // Escuchamos al metrónomo (solo ocurre una vez por beat)
        Metronom.BeatEvent += HandleBeat;
    }

    private void OnDisable()
    {
        Metronom.BeatEvent -= HandleBeat;
    }

    private void HandleBeat(int currentBeat)
    {
        int index = currentBeat - 1;

        if (index >= 0 && index < score.Count)
        {
            // Enviamos los 9 estados de golpe al GridManager
            OnGridUpdate?.Invoke(score[index].gridStates);
            
            // Si el beat actual tiene algún '7' lanzamos un evento de que es un beat puntuable
            if (Array.Exists(score[index].gridStates, state => state == 7))
            {
                OnPuntuableBeat?.Invoke(currentBeat);
            }
        }
    }

    void Awake()
    {
        // GenerarPartituraPrototipo();
        // ImportFromJSON(); // Carga la partitura desde el JSON al iniciar el juego
        GenerarPartituraPrototipoJSON(); // Genera una partitura de prueba y la exporta a JSON (sobrescribe el MusicScore.json) - ¡Descomenta para usar!
    }

    // --- SISTEMA DE DATOS JSON ---
    [ContextMenu("Exportar a JSON")]
    public void ExportToJSON()
    {
        // Antes de guardar, convertimos los arrays a los strings compactos
        foreach (var step in score) step.Export();

        FullScore data = new FullScore { steps = this.score };
        string json = JsonUtility.ToJson(data, true); // Ahora el 'true' solo separará los objetos, no los números
        File.WriteAllText(Path.Combine(Application.dataPath, "MusicScore.json"), json);
    }

    [ContextMenu("Importar desde JSON")]
    public void ImportFromJSON()
    {
        string path = Path.Combine(Application.dataPath, "MusicScore.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            FullScore data = JsonUtility.FromJson<FullScore>(json);
            this.score = data.steps;

            // Convertimos los strings del JSON a los arrays del juego
            foreach (var step in score) step.Import();
            Debug.Log("Partitura compacta cargada.");
        }
    }

    // Función para cargar una partitura desde un archivo JSON
    public void LoadScoreFromJSON(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            FullScore fullScore = JsonUtility.FromJson<FullScore>(json);
            score = fullScore.steps;
        }
        else
        {
            Debug.LogError("Archivo de partitura no encontrado: " + filePath);
        }
    }

    // Función para generar una partitura de prueba con algunos golpes aleatorios
    void GenerarPartituraPrototipo()
    {
        score.Clear();
        for (int i = 0; i < TOTAL_BEATS; i++) score.Add(new BeatStep());

        int beatActual = 10; // Empezamos en el beat 10

        while (beatActual < TOTAL_BEATS)
        {
            int casillaAleatoria = UnityEngine.Random.Range(0, 9);
            
            // Ponemos el Hit
            score[beatActual].gridStates[casillaAleatoria] = 7;

            // Rellenamos la anticipación (Estados 1-6) hacia atrás
            for (int ant = 1; ant <= 6; ant++)
            {
                int idx = beatActual - ant;
                if (idx >= 0) {
                    // Solo escribimos si no hay un estado mayor (para no pisar un 7 cercano)
                    if(score[idx].gridStates[casillaAleatoria] < (7 - ant))
                        score[idx].gridStates[casillaAleatoria] = 7 - ant;
                }
            }

            // Saltamos entre 2 y 6 beats para el siguiente Hit. 
            // Como la anticipación dura 6, si saltamos 2, veremos varias casillas 
            // encendiéndose a la vez, pero solo un "7" por beat.
            beatActual += UnityEngine.Random.Range(2, 7); 
        }
    }

    // // Función para generar una partitura de prueba sin solapamientos (cada casilla solo puede tener un golpe cada 6 beats)
    // [ContextMenu("Generar Partitura Dinámica JSON")]
    // public void GenerarPartituraPrototipoJSON()
    // {
    //     score.Clear();
    //     for (int i = 0; i < TOTAL_BEATS; i++)
    //     {
    //         score.Add(new BeatStep { layout = "000000000" });
    //         score[i].Import(); 
    //     }

    //     int[] estadoCasilla = new int[9];
    //     // Assets\Scripts\RhythmCore\Composer.cs(162,23): error CS1501: No overload for method 'Fill' takes 1 arguments
    //     Array.Fill(estadoCasilla, 0);
    //     int casillasOcupadas = 0;
    //     // lista dinámica con las casillas que se pueden usar
    //     List<int> casillasLibres = new List<int>();
    //     // Inicialmente todas las casillas están libres
    //     casillasLibres.AddRange(new int[] {0,1,2,3,4,5,6,7,8});
        

    //     for (int i = 10; i < TOTAL_BEATS; i++)
    //     {
            
    //         // 1. comprobamos si hay alguna casilla que aumentar
    //         for(int j = 0; j < estadoCasilla.Length; j++)
    //         {
    //             if(estadoCasilla[j] > 0 && estadoCasilla[j] < 7)
    //             {
    //                 estadoCasilla[j]++;
    //                 score[i].gridStates[j] = estadoCasilla[j];
    //             }
    //         }

    //         // 2. Sacamos un random de las casillas libres para poner un golpe
    //         if(i%2 == 0 // Solo cada 2 beats para no saturar, y que se vean varias casillas encendiéndose a la vez, pero sin solapamientos
    //         && casillasLibres.Count > 0 
    //         && casillasOcupadas < 4 // máximo casillas encendiéndose
    //         && UnityEngine.Random.value < 0.8f) // Solo ponemos golpe en el 30% de los beats para que no esté tan lleno
    //         {
    //             int idxCasilla = UnityEngine.Random.Range(0, casillasLibres.Count);
    //             int casillaSeleccionada = casillasLibres[idxCasilla];
    //             score[i].gridStates[casillaSeleccionada] =1;
    //             estadoCasilla[casillaSeleccionada] = 1; // Marcamos que esa casilla tiene un golpe activo
    //             casillasLibres.RemoveAt(idxCasilla); // La sacamos de las libres
    //             casillasOcupadas++;
    //         }

    //         // 3. Si la casilla se libera, la añadimos a las libres
    //         for(int j = 0; j < estadoCasilla.Length; j++)
    //         {
    //             if(estadoCasilla[j] == 7) 
    //             {
    //                 casillasLibres.Add(j); 
    //                 casillasOcupadas--;
    //             }
    //         }
    //     }


    //     foreach (var step in score) step.Export();
    //     ExportToJSON();
    //     Debug.Log("Partitura generada sin solapamientos.");
    // }

    // [ContextMenu("Generar Partitura Dinámica JSON")]
    // public void GenerarPartituraPrototipoJSON()
    // {
    //     score.Clear();
    //     for (int i = 0; i < TOTAL_BEATS; i++)
    //     {
    //         score.Add(new BeatStep { layout = "000000000" });
    //         score[i].Import(); 
    //     }

    //     int[] estadoCasilla = new int[9];
    //     for (int n = 0; n < 9; n++) estadoCasilla[n] = 0; // Manual fill para evitar errores de versión

    //     List<int> casillasLibres = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

    //     for (int i = 10; i < TOTAL_BEATS; i++)
    //     {
    //         // 1. ACTUALIZACIÓN DE ESTADOS EXISTENTES
    //         for (int j = 0; j < 9; j++)
    //         {
    //             if (estadoCasilla[j] > 0)
    //             {
    //                 // Si estaba en 7, ahora se libera (vuelve a 0)
    //                 if (estadoCasilla[j] == 7)
    //                 {
    //                     estadoCasilla[j] = 0;
    //                     score[i].gridStates[j] = 0;
    //                     casillasLibres.Add(j); // Se reintegra a las disponibles
    //                 }
    //                 else
    //                 {
    //                     // Incrementamos progreso
    //                     estadoCasilla[j]++;
    //                     score[i].gridStates[j] = estadoCasilla[j];
    //                 }
    //             }
    //         }

    //         // 2. LÓGICA DE INTENSIDAD (PICOS)
    //         // Cada 64 beats hay un "frenesí" de 16 beats donde la probabilidad es casi 100%
    //         bool esPicoIntensidad = (i % 64 >= 32 && i % 64 <= 48);
    //         float spawnChance = esPicoIntensidad ? 0.95f : 0.30f;
    //         int maxCasillas = esPicoIntensidad ? 6 : 3;

    //         // 3. GENERAR NUEVOS GOLPES
    //         // Solo intentamos spawnear si la casilla está en 0 (usando casillasLibres)
    //         if (i % 2 == 0 && casillasLibres.Count > 0 && (9 - casillasLibres.Count) < maxCasillas)
    //         {
    //             if (UnityEngine.Random.value < spawnChance)
    //             {
    //                 int rIndex = UnityEngine.Random.Range(0, casillasLibres.Count);
    //                 int casillaSeleccionada = casillasLibres[rIndex];

    //                 // Solo iniciamos si realmente está vacía en este frame
    //                 if (estadoCasilla[casillaSeleccionada] == 0)
    //                 {
    //                     estadoCasilla[casillaSeleccionada] = 1;
    //                     score[i].gridStates[casillaSeleccionada] = 1;
    //                     casillasLibres.RemoveAt(rIndex); // Ya no está libre
    //                 }
    //             }
    //         }
    //     }

    //     foreach (var step in score) step.Export();
    //     ExportToJSON();
    //     Debug.Log("Partitura generada con Picos de Intensidad.");
    // }

    [ContextMenu("Generar Partitura Dinámica JSON")]
    public void GenerarPartituraPrototipoJSON()
    {
        score.Clear();
        for (int i = 0; i < TOTAL_BEATS; i++)
        {
            score.Add(new BeatStep { layout = "000000000" });
            score[i].Import(); 
        }

        int[] estadoCasilla = new int[9];
        for (int n = 0; n < 9; n++) estadoCasilla[n] = 0;

        List<int> casillasLibres = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

        for (int i = 1; i < TOTAL_BEATS; i++) // Empezamos en 1 para facilitar el cálculo
        {
            // 1. Convertimos el Beat actual a Compás (Suponiendo 4/4)
            // Ejemplo: Beat 1-4 = Compás 1, Beat 5-8 = Compás 2...
            int compasActual = Mathf.CeilToInt(i / 4f);

            // 2. Obtenemos la configuración de intensidad para este compás
            IntensityConfig config = GetIntensityConfig(compasActual);

            // 3. ACTUALIZACIÓN DE ESTADOS EXISTENTES
            for (int j = 0; j < 9; j++)
            {
                if (estadoCasilla[j] > 0)
                {
                    if (estadoCasilla[j] == 7)
                    {
                        estadoCasilla[j] = 0;
                        score[i-1].gridStates[j] = 0;
                        casillasLibres.Add(j);
                    }
                    else
                    {
                        estadoCasilla[j]++;
                        score[i-1].gridStates[j] = estadoCasilla[j];
                    }
                }
            }

            // 4. GENERAR NUEVOS GOLPES (Basado en la configuración del compás)
            int casillasOcupadas = 9 - casillasLibres.Count;

            if (i % 2 == 0 && casillasLibres.Count > 0 && casillasOcupadas < config.maxCasillas)
            {
                if (UnityEngine.Random.value < config.spawnChance)
                {
                    int rIndex = UnityEngine.Random.Range(0, casillasLibres.Count);
                    int casillaSeleccionada = casillasLibres[rIndex];

                    if (estadoCasilla[casillaSeleccionada] == 0)
                    {
                        estadoCasilla[casillaSeleccionada] = 1;
                        score[i-1].gridStates[casillaSeleccionada] = 1;
                        casillasLibres.RemoveAt(rIndex);
                    }
                }
            }
        }

        foreach (var step in score) step.Export();
        ExportToJSON();
        Debug.Log("Partitura generada siguiendo la estructura de compases de FMOD.");
    }

    // Estructura para manejar los datos de intensidad
    private struct IntensityConfig {
        public float spawnChance;
        public int maxCasillas;
    }

    private IntensityConfig GetIntensityConfig(int compas)
    {
        // Definimos los rangos que me has pasado
        if (compas >= 1 && compas <= 10)   return new IntensityConfig { spawnChance = 0.2f, maxCasillas = 2 }; // Poca
        if (compas >= 11 && compas <= 15)  return new IntensityConfig { spawnChance = 0.4f, maxCasillas = 3 }; // Un poco más
        if (compas >= 16 && compas <= 20)  return new IntensityConfig { spawnChance = 0.6f, maxCasillas = 4 }; // Media
        if (compas >= 21 && compas <= 31)  return new IntensityConfig { spawnChance = 0.9f, maxCasillas = 6 }; // A TOPE
        if (compas >= 32 && compas <= 35)  return new IntensityConfig { spawnChance = 0.5f, maxCasillas = 4 }; // Relaja
        if (compas >= 36 && compas <= 50)  return new IntensityConfig { spawnChance = 0.95f, maxCasillas = 7 }; // A TOPE 2
        if (compas >= 51 && compas <= 60)  return new IntensityConfig { spawnChance = 0.2f, maxCasillas = 2 }; // Calma
        if (compas >= 61 && compas <= 65)  return new IntensityConfig { spawnChance = 0.4f, maxCasillas = 3 }; // Subiendo
        if (compas >= 66 && compas <= 75)  return new IntensityConfig { spawnChance = 0.6f, maxCasillas = 4 }; // Media
        if (compas >= 76 && compas <= 85)  return new IntensityConfig { spawnChance = 0.9f, maxCasillas = 6 }; // A TOPE 3
        if (compas >= 86 && compas <= 90)  return new IntensityConfig { spawnChance = 0.5f, maxCasillas = 4 }; // Relaja
        if (compas >= 91 && compas <= 100) return new IntensityConfig { spawnChance = 1.0f, maxCasillas = 8 }; // FINAL PEAK

        return new IntensityConfig { spawnChance = 0.1f, maxCasillas = 1 }; // Por defecto
    }
}
