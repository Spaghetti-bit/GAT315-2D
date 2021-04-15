using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public BoolData simulate;
    public FloatData mass;
    public FloatData gravity;
    public FloatData gravitation;
    public StringData FPSText;

    public FloatData fixedFPS;

    float timeAccumulator;
    public float fixedDeltaTime { get { return 1 / fixedFPS.value; } }
    //

    static World instance;
    public static World Instance { get { return instance; } }

    public Vector2 Gravity { get { return new Vector2(0, gravity.value); } }
    public List<Body> bodies { get; set; } = new List<Body>();


    //float fps = 0;
    float fpsAverage = 0;
    float smoothing = 0.975f;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        
        float dt = Time.deltaTime;
        fpsAverage = (fpsAverage * smoothing) + ((1.0f/dt) * (1.0f - smoothing));
        FPSText.value = $"FPS: {fpsAverage.ToString("F2")} : {(dt * 1000.0f).ToString("F1")} ms";

        if (!simulate) return;


        bodies.ForEach(body => body.shape.color = new Color(Random.value * 255, Random.value * 255, Random.value * 255));


        timeAccumulator += dt;
        //Debug.Log(1.0f / dt);
        GravitationalForce.ApplyForce(bodies, gravitation.value);

        while(timeAccumulator >= fixedDeltaTime)
        {
            bodies.ForEach(body => body.Step(fixedDeltaTime));
            bodies.ForEach(body => Integrator.SemiImplicitEuler(body, fixedDeltaTime));


            timeAccumulator -= fixedDeltaTime;
        }

        bodies.ForEach(body => body.force = Vector2.zero);
        bodies.ForEach(body => body.acceleration = Vector2.zero);

    }
}
