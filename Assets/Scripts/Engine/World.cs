using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public BoolData simulate;
    public FloatData mass;
    public FloatData gravity;
    public StringData fps;

    public FloatData fixedFPS;

    float timeAccumulator;
    public float fixedDeltaTime { get { return 1 / fixedFPS.value; } }
    //

    static World instance;
    public static World Instance { get { return instance; } }

    public Vector2 Gravity { get { return new Vector2(0, gravity.value); } }
    public List<Body> bodies { get; set; } = new List<Body>();

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        
        if (!simulate.value) return;



        float dt = Time.deltaTime;
        timeAccumulator += dt;
        Debug.Log(1.0f / dt);
        fps.value = (1.0f / dt).ToString();

        while(timeAccumulator > fixedDeltaTime)
        {
            bodies.ForEach(body => body.mass = mass.value);
            bodies.ForEach(body => body.Step(fixedDeltaTime));
            bodies.ForEach(body => Integrator.SemiImplicitEuler(body, fixedDeltaTime));

            bodies.ForEach(body => body.force = Vector2.zero);
            bodies.ForEach(body => body.acceleration = Vector2.zero);

            timeAccumulator -= fixedDeltaTime;
        }


    }
}
