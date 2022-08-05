using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerMovementButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    #region Private Attributes

    [SerializeField] float movementFactor;
    
    private UnityAction<float> OnButtonPressed;
    private bool buttonPressed;
    private bool buttonUp;

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
        buttonUp = true;
    }

    private void FixedUpdate()
    {
        if (buttonPressed)
            OnButtonPressed?.Invoke(movementFactor);
        else
        {
            if(buttonUp)
            {
                buttonUp = false;
                OnButtonPressed?.Invoke(0);
            }
        }
    }

    #endregion

}