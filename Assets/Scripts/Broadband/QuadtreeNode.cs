using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuadtreeNode
{
    AABB aabb;
    int capacity;
    List<Body> bodies;
    bool isSubdivided = false;

    QuadtreeNode northEast, northWest, southEast, southWest;

    public QuadtreeNode(AABB aabb, int capacity)
    {
        this.aabb = aabb;
        this.capacity = capacity;

        bodies = new List<Body>();

    }

    public void Insert(Body body)
    {
        if (!aabb.Contains(body.shape.aabb)) return;

        if (bodies.Count < capacity)
        {
            bodies.Add(body);
        }
        else
        {
            if (!isSubdivided)
            {
                // subdivide
                Subdivide();
            }
            northEast.Insert(body);
            northWest.Insert(body);
            southEast.Insert(body);
            southWest.Insert(body);
        }
    }

    public void Query(AABB aabb, List<Body> bodies)
    {
        if (!this.aabb.Contains(aabb)) return;

        bodies.AddRange(this.bodies.Where(body => body.shape.aabb.Contains(aabb)));

        //foreach (Body body in this.bodies)
        //{
        //    if(body.shape.aabb.Contains(aabb))
        //    {
        //        bodies.Add(body);
        //    }
        //}

        if(isSubdivided)
        {
            northEast.Query(aabb, bodies);
            northWest.Query(aabb, bodies);
            southEast.Query(aabb, bodies);
            southWest.Query(aabb, bodies);
        }
    }

    private void Subdivide()
    {
        // Calculate center
        float xo = aabb.extents.x * 0.5f;
        float yo = aabb.extents.y * 0.5f;

        northEast = new QuadtreeNode(new AABB(new Vector2(aabb.center.x - xo, aabb.center.y + yo), aabb.extents), capacity);
        northWest = new QuadtreeNode(new AABB(new Vector2(aabb.center.x + xo, aabb.center.y + yo), aabb.extents), capacity);
        southEast = new QuadtreeNode(new AABB(new Vector2(aabb.center.x - xo, aabb.center.y - yo), aabb.extents), capacity);
        southWest = new QuadtreeNode(new AABB(new Vector2(aabb.center.x + xo, aabb.center.y - yo), aabb.extents), capacity);

        isSubdivided = true;
    }

    public void Draw()
    {
        aabb.Draw(Color.white);

        if(isSubdivided)
        {
            northEast?.Draw();
            northWest?.Draw();
            southEast?.Draw();
            southWest?.Draw();
        }
    }
    
}
