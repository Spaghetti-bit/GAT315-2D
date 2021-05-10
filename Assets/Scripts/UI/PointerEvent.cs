using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class PointerEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [System.Serializable]
    public enum eState
    {
        Up,
        Down
    }

    [System.Serializable]
    public struct EventInfo
    {
        public PointerEventData.InputButton button;
        public eState state;

        public UnityEvent uEvent;
    }

    public EventInfo[] eventInfos;

    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (EventInfo info in eventInfos)
        {
            if(info.button == eventData.button && info.state == eState.Down)
            {
                info.uEvent.Invoke();
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (EventInfo info in eventInfos)
        {
            if (info.button == eventData.button && info.state == eState.Up)
            {
                info.uEvent.Invoke();
            }
        }
    }

}
