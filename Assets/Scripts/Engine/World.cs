using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public BoolData simulate;
    public BoolData collision;
    public BoolData wrap;
    public FloatData mass;
    public FloatData gravity;
    public FloatData gravitation;
    public StringData FPSText;

    public FloatData fixedFPS;

    float timeAccumulator;
    Vector2 size;
    public float fixedDeltaTime { get { return 1 / fixedFPS.value; } }
    //

    static World instance;
    public static World Instance { get { return instance; } }

    public Vector2 Gravity { get { return new Vector2(0, gravity.value); } }
    public List<Body> bodies { get; set; } = new List<Body>();
    public List<Spring> springs { get; set; } = new List<Spring>();


    //float fps = 0;
    float fpsAverage = 0;
    float smoothing = 0.975f;

    private void Awake()
    {
        instance = this;
        size = Camera.main.ViewportToWorldPoint(Vector2.one);
    }

    void Update()
    {
        
        float dt = Time.deltaTime;
        fpsAverage = (fpsAverage * smoothing) + ((1.0f/dt) * (1.0f - smoothing));
        FPSText.value = $"FPS: {fpsAverage.ToString("F2")} : {(dt * 1000.0f).ToString("F1")} ms";


        // Draw springs connected between bodies. (A->B)
        springs.ForEach(spring => spring.Draw());


        if (!simulate) return;




        //Debug.Log(1.0f / dt);
        GravitationalForce.ApplyForce(bodies, gravitation.value);
        springs.ForEach(spring => spring.ApplyForce());

        timeAccumulator += dt;

        while(timeAccumulator >= fixedDeltaTime)
        {
            bodies.ForEach(body => body.Step(fixedDeltaTime));
            bodies.ForEach(body => Integrator.SemiImplicitEuler(body, fixedDeltaTime));

            bodies.ForEach(body => body.shape.color = Color.white );

            // if collision
            if(collision)
            {
                Collision.CreateContacts(bodies, out List<Contact> contacts);
                contacts.ForEach(contact =>
                {
                    contact.bodyA.shape.color = Color.red;
                    contact.bodyB.shape.color = Color.red;
                });

                ContactSolver.Resolve(contacts);
            }
            //

            timeAccumulator -= fixedDeltaTime;
        }

        if(wrap)
        {
            bodies.ForEach(body => body.position = Utilities.Wrap(body.position, -size, size));
        }

        bodies.ForEach(body => body.force = Vector2.zero);
        bodies.ForEach(body => body.acceleration = Vector2.zero);

    }
}
