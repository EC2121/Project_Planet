using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

public class ControlsTest : MonoBehaviour
{
    public GameObject SettingsPanel, MainButtonsPanel;
    private OnScreenButton backButton;
    private OnScreenControl button;

    private void Start()
    {
        backButton = GetComponent<OnScreenButton>();
    }

    private void Update()
    {
        if (backButton.control.IsPressed())
        {
            //GetComponent<EventTrigger>().OnPointerClick(BaseEventData);
            Debug.Log("Pippo");
        }
    }
}
