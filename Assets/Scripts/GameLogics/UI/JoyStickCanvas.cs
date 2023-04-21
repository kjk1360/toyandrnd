using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStickCanvas : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    //public GameObject player;
    public RectTransform joystickBackground;
    public RectTransform joystickKnob;
    //private RectTransform joystickBackgroundRectTransform;
    //private RectTransform joystickKnobRectTransform;
    public Rigidbody2D playerRigidbody;
    public float moveSpeed = 5f;
    private Vector2 originalJoystickKnobPosition;
    public float maxJoystickDistance = 100f;
    //private void Start()
    //{
    //    playerRigidbody = player.GetComponent<Rigidbody>();
    //    joystickBackground = joystickBackground.GetComponent<RectTransform>();
    //    joystickKnob = joystickKnob.GetComponent<RectTransform>();
    //    originalJoystickKnobPosition = joystickKnob.anchoredPosition;
    //}

    public void OnPointerDown(PointerEventData eventData)
    {
        joystickBackground.gameObject.SetActive(true);
        joystickKnob.gameObject.SetActive(true);

        joystickBackground.anchoredPosition = eventData.position;
        joystickKnob.anchoredPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - joystickBackground.anchoredPosition;
        direction.Normalize();
        playerRigidbody.velocity = new Vector3(direction.x, direction.y, 0) * moveSpeed;

        Vector2 newJoystickKnobPosition = eventData.position;
        if (Vector2.Distance(newJoystickKnobPosition, joystickBackground.anchoredPosition) > maxJoystickDistance)
        {
            newJoystickKnobPosition = joystickBackground.anchoredPosition + direction * maxJoystickDistance;
        }
        joystickKnob.anchoredPosition = newJoystickKnobPosition;

        //joystickKnob.anchoredPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystickBackground.gameObject.SetActive(false);
        joystickKnob.gameObject.SetActive(false);

        playerRigidbody.velocity = Vector3.zero;
        joystickKnob.anchoredPosition = originalJoystickKnobPosition;
    }
}
