using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This class makes so when the player click on the screen with the mouse (on the menu)
/// the selected button on unity Event System resets to the one in the firstButtonSelected variable.
/// </summary>
public class FirstButtonSelected : MonoBehaviour
{
    [SerializeField]
    private Button firstButtonSelected;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButtonSelected.gameObject);
    }

        void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(firstButtonSelected.gameObject);
        }
    }
}
