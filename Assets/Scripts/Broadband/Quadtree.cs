using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quadtree : BroadPhase
{
    public int capacity { get; set; } = 4;

    QuadtreeNode rootNode;

    public override void Build(AABB aabb, List<Body> bodies)
    {
        potentialCollisionCount = 0;
        rootNode = new QuadtreeNode(aabb, capacity);
        bodies.ForEach(body => rootNode.Insert(body));
    }


    public override void Query(AABB aabb, List<Body> bodies)
    {
        rootNode.Query(aabb, bodies);
        potentialCollisionCount = potentialCollisionCount + bodies.Count;
    }

    public override void Query(Body body, List<Body> bodies)
    {
        Query(body.shape.aabb, bodies);
    }
    public override void Draw()
    {
        rootNode?.Draw();
    }
}
