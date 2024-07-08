#if UNITY_EDITOR
using BlobActor.Runtime;
using Core.Hybrid;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace BlobActor.Hybrid
{
    [CreateAssetMenu(menuName = "BlobActor/Create actor")]
    public class BlobActorBaked : BakedScriptableObject<Actor.Blob>
    {
        [SerializeField] private Actor.Blob blob_s = Actor.Blob.Default;

        public float repathRate => blob_s.repathRate;
        public float baseSpeed => blob_s.baseSpeed;
        public ActorRuntime.Flag runtimeFlags => blob_s.runtimeFlags;

        public override void Bake(ref Actor.Blob data, ref BlobBuilder blobBuilder)
        {
            data = blob_s;
            data.stoppingDistanceSq = math.square(data.stoppingDistanceSq);
        }

    }
}
#endif