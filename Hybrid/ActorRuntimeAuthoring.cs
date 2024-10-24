using BlobActor.Runtime;
using Unity.Entities;
using UnityEngine;

namespace BlobActor.Hybrid
{
    public class ActorRuntimeAuthoring : MonoBehaviour
    {
        private class ActorRuntimeBaker : Baker<ActorRuntimeAuthoring>
        {
            public override void Bake(ActorRuntimeAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.None);
                AddComponent<ActorRuntime>(e);
            }
        }
    }
}