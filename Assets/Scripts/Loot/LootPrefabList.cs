using Riptide;
using System.Collections.Generic;
using UnityEngine;

public class LootPrefabList : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();

    public static Dictionary<ushort, GameObject> itemPrefabs = new Dictionary<ushort, GameObject>();

    public static Dictionary<ushort, GameObject> droppedItems = new Dictionary<ushort, GameObject>();

    private void Start()
    {
        for(ushort i = 0; i < prefabs.Count; i++)
        {
            itemPrefabs.Add(CharValues.GetValueOfString(prefabs[i].name), prefabs[i]);
        }
    }

    private static void SpawnItem(ushort id, Vector2 spawnPosition, ushort droppedItemId)
    {
        GameObject droppedItem = Instantiate(itemPrefabs[id], spawnPosition, Quaternion.identity);
        droppedItems.Add(droppedItemId, droppedItem);
    }

    [MessageHandler((ushort)ServerToClientID.droppedItems)]
    private static void GetItemID(Message message)
    {
        SpawnItem(message.GetUShort(), message.GetVector2(), message.GetUShort());
    }
}
