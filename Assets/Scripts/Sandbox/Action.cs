using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    public enum eActionType
    {
        Creator,
        Connector,
        Force,
        Selector
    }

    public abstract eActionType actionType { get; }

    public bool active { get; set; } = false;

    public abstract void StartAction();
    public abstract void StopAction();
}
