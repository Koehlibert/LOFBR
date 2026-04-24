using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StackingHandler
{
    public static void PushAwayFromNearbyObjects(GameObject gameObject)
    {
        Transform transform = gameObject.transform;
        float radius = 1.75f;
        float pushStrength = 4f;
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        HashSet<GameObject> processed = new HashSet<GameObject>();
        foreach (Collider hit in hits)
        {
            if (hit == null) continue;
            GameObject root = hit.transform.root.gameObject;
            if (root == gameObject) continue;
            if (!processed.Add(root)) continue;
            if (!root.GetComponent<CharacterBehaviour>()) continue;
            if (root.GetComponent<TowerBehaviour>()) continue;
            Vector3 closest = hit.ClosestPoint(transform.position);
            Vector3 dir = transform.position - closest;
            dir.y = 0f;
            float dist = dir.magnitude;
            if (dist < 0.001f) continue;
            transform.position += dir.normalized * pushStrength * Time.deltaTime;
        }
    }
}