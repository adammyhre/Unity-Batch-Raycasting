using System;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityUtils;

public class RaycastBatchProcessor : Singleton<RaycastBatchProcessor> {
    [SerializeField] int maxRaycastsPerJob = 10000;

    NativeArray<RaycastCommand> rayCommands;
    NativeArray<SpherecastCommand> sphereCommands;
    NativeArray<RaycastHit> hitResults;

    public void PerformRaycasts(
        Vector3[] origins,
        Vector3[] directions,
        int layerMask,
        bool hitBackfaces,
        bool hitTriggers,
        bool hitMultiFace,
        Action<RaycastHit[]> callback) {
        const float maxDistance = 1f;
        int rayCount = Mathf.Min(origins.Length, maxRaycastsPerJob);

        QueryTriggerInteraction queryTriggerInteraction = hitTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore;

        using (rayCommands = new NativeArray<RaycastCommand>(rayCount, Allocator.TempJob)) {
            QueryParameters parameters = new QueryParameters {
                layerMask = layerMask,
                hitBackfaces = hitBackfaces,
                hitTriggers = queryTriggerInteraction,
                hitMultipleFaces = hitMultiFace
            };

            for (int i = 0; i < rayCount; i++) {
                rayCommands[i] = new RaycastCommand(origins[i], directions[i], parameters, maxDistance);
            }

            ExecuteRaycasts(rayCommands, callback);
        }
    }

    void ExecuteRaycasts(NativeArray<RaycastCommand> raycastCommands, Action<RaycastHit[]> callback) {
        int maxHitsPerRaycast = 1;
        int totalHitsNeeded = raycastCommands.Length * maxHitsPerRaycast;

        using (hitResults = new NativeArray<RaycastHit>(totalHitsNeeded, Allocator.TempJob)) {
            foreach (RaycastCommand t in raycastCommands) {
                Debug.DrawLine(t.from, t.from + t.direction * 1f, Color.red, 0.5f);
            }

            JobHandle raycastJobHandle = RaycastCommand.ScheduleBatch(raycastCommands, hitResults, maxHitsPerRaycast);
            raycastJobHandle.Complete();

            if (hitResults.Length > 0) {
                RaycastHit[] results = hitResults.ToArray();

                // for (int i = 0; i < results.Length; i++) {
                //     if (results[i].collider != null) {
                //         Debug.Log($"Hit: {results[i].collider.name} at {results[i].point}");
                //         Debug.DrawLine(raycastCommands[i].from, results[i].point, Color.green, 1.0f);
                //     }
                // }

                callback?.Invoke(results);
            }
        }
    }
}