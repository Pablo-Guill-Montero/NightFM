using System; // <--- ¡Importante!

[System.Serializable]
public class BeatStep
{
    // Esto es lo único que el JSON escribirá: una línea corta de texto
    public string layout = "000000000";

    // Esto NO se guarda en el JSON ([NonSerialized]), solo vive en Unity
    [System.NonSerialized] 
    public int[] gridStates = new int[9];

    // Convierte el texto "000070000" al array de 9 números
    public void Import()
    {
        for (int i = 0; i < 9; i++)
        {
            gridStates[i] = (int)char.GetNumericValue(layout[i]);
        }
    }

    // Convierte el array a texto para cuando quieras exportar
    public void Export()
    {
        layout = "";
        for (int i = 0; i < 9; i++) layout += gridStates[i].ToString();
    }
}