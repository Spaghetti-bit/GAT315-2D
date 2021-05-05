using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static Vector2 Wrap(Vector2 point, Vector2 min, Vector2 max)
    {
        if (point.x > max.x) point.x = min.x;
        else if (point.x < min.x) point.x = max.x;

        if (point.y > max.y) point.y = min.y;
        else if (point.y < min.y) point.y = max.y;


        return point;
    }
    public static Vector2 SpringForce(Vector2 source, Vector2 destination, float restLength, float k)
    {
        // destination -> source
        Vector2 direction = destination - source;
        float length = direction.magnitude;
        // Difference between current length and its' resting point
        float x = length - restLength;

        // force
        return direction.normalized * (-k * x);
    }

    public static Body GetBodyFromPosition(Vector2 position)
    {
        Body body = null;

        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (hit.collider)
        {
            body = hit.collider.gameObject.GetComponent<Body>();
        }

        return body;
    }
}
