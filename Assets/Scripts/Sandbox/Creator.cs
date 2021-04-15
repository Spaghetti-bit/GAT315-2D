using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : Action
{

    public GameObject original;
    public FloatData speed;
    public FloatData damping;
    public FloatData size;
    public FloatData density;

    public GameObject objectContainer;


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

            if(gameObject.TryGetComponent<Body>(out Body body))
            {
                Vector2 force = Random.insideUnitSphere.normalized * speed;
                body.AddForce(force, Body.eForceMode.Velocity);
                body.damping = damping;
                body.shape.size = size;
                body.shape.density = density;
                World.Instance.bodies.Add(body);
            }
        }
        
    }
}
