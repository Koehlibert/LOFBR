using UnityEngine;
using System.Collections.Generic;

namespace Extensions
{
    public static class TagExtensions
    {
        public static bool HasAnyTag(this GameObject obj, IEnumerable<string> tags)
        {
            foreach (var tag in tags)
            {
                if (obj.CompareTag(tag))
                    return true;
            }
            return false;
        }
    }
    public static class CollisionExtensions
    {
        public static bool HasAnyTag(this Collision collision, IEnumerable<string> tags)
        {
            return collision.gameObject.HasAnyTag(tags);
        }
    }

    public static class ColliderExtensions
    {
        public static bool HasAnyTag(this Collider collider, IEnumerable<string> tags)
        {
            return collider.gameObject.HasAnyTag(tags);
        }
    }
}