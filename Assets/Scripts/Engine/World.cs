using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public StringData FPSText;
    public StringData collisionText;
    public BoolData simulate;
    public BoolData collisionDebug;
    public BoolData collision;
    public BoolData wrap;
    public FloatData mass;
    public FloatData gravity;
    public FloatData gravitation;

    public FloatData fixedFPS;
    public VectorField vectorField;


    public BroadphaseTypeData bodyphaseType;

    float timeAccumulator;

    BroadPhase broadPhase = new BVH();
    BroadPhase[] broadPhases = { new NullBroadPhase(), new Quadtree(), new BVH() };
    public AABB AABB { get => aabb; }
    AABB aabb;

    public Vector2 WorldSize { get => size * 2; }
    Vector2 size;
    public float fixedDeltaTime { get { return 1 / fixedFPS.value; } }
    //

    static World instance;
    public static World Instance { get { return instance; } }

    public Vector2 Gravity { get { return new Vector2(0, gravity.value); } }
    public List<Body> bodies { get; set; } = new List<Body>();
    public List<Spring> springs { get; set; } = new List<Spring>();
    public List<Force> forces { get; set; } = new List<Force>();


    //float fps = 0;
    float fpsAverage = 0;
    float smoothing = 0.975f;

    private void Awake()
    {
        instance = this;
        size = Camera.main.ViewportToWorldPoint(Vector2.one);
        aabb = new AABB(Vector2.zero, size * 2);
    }

    void Update()
    {
        
        float dt = Time.deltaTime;
        fpsAverage = (fpsAverage * smoothing) + ((1.0f/dt) * (1.0f - smoothing));
        FPSText.value = $"FPS: {fpsAverage.ToString("F2")} : {(dt * 1000.0f).ToString("F1")} ms";


        // Draw springs connected between bodies. (A->B)
        springs.ForEach(spring => spring.Draw());

        broadPhase = broadPhases[bodyphaseType.index];

        if (!simulate) return;




        // Forces
        GravitationalForce.ApplyForce(bodies, gravitation.value);
        forces.ForEach(force => bodies.ForEach(body => force.ApplyForce(body)));
        springs.ForEach(spring => spring.ApplyForce());
        bodies.ForEach(body => vectorField.ApplyForce(body));

        timeAccumulator += dt;

        while(timeAccumulator >= fixedDeltaTime)
        {
            bodies.ForEach(body => body.Step(fixedDeltaTime));
            bodies.ForEach(body => Integrator.SemiImplicitEuler(body, fixedDeltaTime));


            // if collision
            if(collision)
            {
                bodies.ForEach(body => body.shape.color = Color.white );
                broadPhase.Build(aabb, bodies);

                Collision.CreateBroadPhaseContacts(broadPhase, bodies, out List<Contact> contacts);
                Collision.CreateNarrowPhaseContacts(ref contacts);
                contacts.ForEach(contact => Collision.UpdateContactInfo(ref contact));

                ContactSolver.Resolve(contacts);

                if(collisionDebug)
                {
                    contacts.ForEach(contact =>
                    {
                        contact.bodyA.shape.color = Color.red;
                        contact.bodyB.shape.color = Color.red;
                    });
                }
            }
            //

            timeAccumulator -= fixedDeltaTime;
        }


        collisionText.value = $"Broad Phase: {BroadPhase.potentialCollisionCount.ToString()}";
        if(collisionDebug) broadPhase.Draw();

        if(wrap)
        {
            bodies.ForEach(body => body.position = Utilities.Wrap(body.position, -size, size));
        }

        bodies.ForEach(body => body.force = Vector2.zero);
        bodies.ForEach(body => body.acceleration = Vector2.zero);

    }
}
