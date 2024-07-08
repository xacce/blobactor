#if UNITY_EDITOR
using BlobActor.Runtime;
using Unity.Entities;
using UnityEngine;

namespace BlobActor.Hybrid
{
    public class BlobActorAuthoring : MonoBehaviour
    {
        [SerializeField] private BlobActorBaked blobActor_s;

        private class BlobActorAuthoringBaker : Baker<BlobActorAuthoring>
        {
            public override void Bake(BlobActorAuthoring authoring)
            {
                if (authoring.blobActor_s == null) return;
                var e = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(
                    e,
                    new Actor()
                    {
                        blob = authoring.blobActor_s.Bake(this),
                    });
                AddComponent(
                    e,
                    new ActorRuntime()
                    {
                        repath = authoring.blobActor_s.repathRate,
                        maxSpeed = authoring.blobActor_s.baseSpeed,
                        flags = authoring.blobActor_s.runtimeFlags,
                    });
            }
        }
    }
}
#endif