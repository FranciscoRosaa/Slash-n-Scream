using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
