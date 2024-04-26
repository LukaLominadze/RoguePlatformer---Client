using Riptide;
using System.Collections.Generic;
using UnityEngine;

public class ShadowStrikeFlurry : MonoBehaviour
{
    private static List<GameObject> indicators = new List<GameObject>();

    private static void SpawnIndicators(Vector2 spawnPosition)
    {
        GameObject indicator = Instantiate(GameLogic.Singleton.IndicatorPrefab, spawnPosition, Quaternion.identity);
        indicators.Add(indicator);
    }

    private static void DestroyIndicators()
    {
        for(int i = 0; i <  indicators.Count; i++)
        {
            Destroy(indicators[i]);
            indicators.Remove(indicators[i]);
        }
    }

    [MessageHandler((ushort)ServerToClientID.abilityEvent)]
    private static void GetPosition(Message message)
    {
        SpawnIndicators(message.GetVector2());
    }

    [MessageHandler((ushort)ServerToClientID.destroyIndicators)]
    private static void DestroyIndicatorsActivator(Message message)
    {
        DestroyIndicators();
    }
}
