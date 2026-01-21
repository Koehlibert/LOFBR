using UnityEngine;

public static class StackingHandler
{
    public static void PushAwayFromNearbyObjects(GameObject gameObject)
    {
        Transform transform = gameObject.transform;
        float radius = 1f;
        float pushStrength = 5f;
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in hits)
        {
            if (hit == null || hit.gameObject == gameObject) continue;
            if (!hit.gameObject.GetComponent<Health>()) continue;
            //Debug.Log("Pushing away from " + hit.gameObject.tag);
            Vector3 dir = transform.position - hit.transform.position;
            dir.y = 0f;
            float dist = dir.magnitude;
            if (dist < 0.001f) continue;
            transform.position += dir.normalized * pushStrength * Time.deltaTime;
        }
    }
}