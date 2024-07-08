using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using Unity.Mathematics;

namespace BlobActor.Runtime
{
    public partial struct ActorRuntime : IComponentData
    {
        [Flags]
        public enum Flag : byte
        {
            Nothing = 0,
            Reserved = 1,
            Reached = 2,
            AutoRepath = 4,
            ForceRepath = 8,
            AllowTurning = 16,
            AllowMove = 32,
            AvoidanceEnabled = 64,
            PathSeeekerEnabled = 128,
        }

        public Flag flags;
        public float maxSpeed;
        public float3 velocity;
        public float repath;
        public int currentPathIndex;
        public bool isReached => (flags & Flag.Reached) != 0;
        public bool autoRepath => (flags & Flag.AutoRepath) != 0;
        public float3 destination;
        public bool requireRepath => (flags & Flag.ForceRepath) != 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ComputeNewVelocity(ref Actor.Blob blob, ref ActorRuntime runtime, in float3 newVelocity, in float deltaTime)
        {
            runtime.velocity = math.lerp(runtime.velocity, new float3(newVelocity.x, 0f, newVelocity.z), blob.acceleration * deltaTime);
        }

        public void Stop()
        {
            velocity = float3.zero;
            flags &= ~(Flag.AllowMove | Flag.AllowTurning | Flag.PathSeeekerEnabled | Flag.AvoidanceEnabled);
        }

        public void Release()
        {
            flags |= Flag.AllowMove | Flag.AllowTurning | Flag.PathSeeekerEnabled | Flag.AvoidanceEnabled | Flag.AutoRepath | Flag.ForceRepath;
        }

        public void Set(in Flag flag)
        {
            flags |= flag;
        }

        public void Unset(in Flag flag)
        {
            flags &= ~flag;
        }
    }

    public partial struct Actor : IComponentData
    {
        [Serializable]
        public struct Blob
        {
            public ActorRuntime.Flag runtimeFlags;
            public float turningSpeedImpact;
            public float rotationSpeed;
            public float acceleration;
            public float stoppingDistanceSq;
            public float baseSpeed;
            public float repathRate;
            public float3 extents;
            public float3 spawnExtents;
            public int agentTypeId;
            public int areaMask;
            public float timeHorizon;
            public float timeHorizonObst;

            public float timeStep;

            // public float timeStepObs;
            public int maxNeighbours;
            public float3 neighborsExtents;
            public float radius;
            public float radiusObst;

            public static Blob Default => new Blob()
            {
                runtimeFlags = ActorRuntime.Flag.AutoRepath & ActorRuntime.Flag.AllowMove & ActorRuntime.Flag.AllowTurning & ActorRuntime.Flag.PathSeeekerEnabled,
                agentTypeId = 0,
                repathRate = 0.5f,
                baseSpeed = 5,
                acceleration = 1f,
                extents = new float3(5, 1, 5),
                spawnExtents = new float3(5, 5, 5),
                areaMask = -1,
                stoppingDistanceSq = 0.1f,
                turningSpeedImpact = 1f,
                rotationSpeed = 360f,
                radius = 0.25f,
                radiusObst = 0.25f,
                neighborsExtents = new float3(5, 1, 5),
                // timeStep = 0.01f,
                // timeStepObs = 0.01f,
                timeHorizon = 2,
                timeHorizonObst = 2,
                maxNeighbours = 10,
            };
        }

        public BlobAssetReference<Blob> blob;
    }
}