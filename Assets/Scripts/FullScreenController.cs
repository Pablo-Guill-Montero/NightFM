using UnityEngine;
using UnityEngine.UI;

public class FullScreenController : MonoBehaviour
{
    [SerializeField] private Toggle toggle;

    public void Change()
    {
        Screen.fullScreen = toggle.isOn;
        Debug.Log("FullScreen: " + Screen.fullScreen);
    }
}
