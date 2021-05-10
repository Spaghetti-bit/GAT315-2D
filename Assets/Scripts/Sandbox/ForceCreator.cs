using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceCreator : Action
{
    public List<Force> forces { get; set; } = new List<Force>();

    public GameObject original;
    public FloatData size;
    public FloatData forceMagnitude;
    public ForceModeData forceMode;

    public GameObject objectContainer;

    public override eActionType actionType => eActionType.Creator;


    bool action { get; set; } = false;
    public bool oneTime { get; set; } = false;
    public override void StartAction()
    {
        action = true;
        oneTime = true;
    }

    public override void StopAction()
    {
        action = false;
        oneTime = false;
    }

    void Update()
    {
        if (action && (oneTime || Input.GetKey(KeyCode.LeftControl)))
        {
            oneTime = false;
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            GameObject gameObject = Instantiate(original, position, Quaternion.identity, objectContainer.transform);
            if (gameObject.TryGetComponent<PointEffector>(out PointEffector effector))
            {
                effector.forceMagnitude = forceMagnitude;
                effector.shape.size = size;
                effector.forceMode = forceMode.value;

                World.Instance.forces.Add(effector);
            }
        }


    }
}
