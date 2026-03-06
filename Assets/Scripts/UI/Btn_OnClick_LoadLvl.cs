// Lvl_01_Gameplay

using UnityEngine;
using UnityEngine.UI;

public class Btn_OnClick_LoadLvl : MonoBehaviour
{

    [SerializeField] private string _levelName;
    // [SerializeField] private Button _button;

    // private void Start()
    // {
    //     if (_button == null)
    //     {
    //         Debug.LogError("¡No se ha asignado el botón en el inspector!");
    //         return;
    //     }

    //     // Por ahora no lo quiero hacer así porque tengo un OnClick en editor para sonido
    //     // _button.onClick.RemoveAllListeners(); // Asegura que no haya listeners previos
    //     // _button.onClick.AddListener(() => LoadLevel(_levelName));
    // }

    public void LoadLevel(string levelName)
    {
        SceneLoader.Instance.LoadScene(levelName);
    }

    public void LoadLevel()
    {
        SceneLoader.Instance.LoadScene(_levelName);
    }

}