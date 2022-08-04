using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerMovementButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    #region Private Attributes

    [SerializeField] float movementFactor;
    
    private UnityAction<float> OnButtonPressed;
    private bool buttonPressed;

    #endregion

    #region Main Methods

    public UnityAction<float> OnPressed
    {
        set { OnButtonPressed = value; }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }

    private void FixedUpdate()
    {
        if (buttonPressed)
            OnButtonPressed?.Invoke(movementFactor);
    }

    #endregion

}