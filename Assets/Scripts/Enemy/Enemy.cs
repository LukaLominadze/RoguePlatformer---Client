using Riptide;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Interpolator interpolator;

    public static Dictionary<ushort, Enemy> enemyList = new Dictionary<ushort, Enemy>();

    public ushort Id { get; private set; }

    private void OnDestroy()
    {
        enemyList.Remove(Id);
    }

    private void MoveEnemy(ushort tick, Vector2 newPosition)
    {
        interpolator.NewUpdate(tick, newPosition);
    }

    private static void Spawn(ushort id, Vector2 spawnPosition)
    {
        Enemy enemy = Instantiate(GameLogic.Singleton.EnemyPrefab, spawnPosition, Quaternion.identity).GetComponent<Enemy>();
        enemy.Id = id;
        enemyList.Add(enemy.Id, enemy);
    }

    [MessageHandler((ushort)ServerToClientID.enemySpawned)]
    private static void GetId(Message message)
    {
        Spawn(message.GetUShort(), message.GetVector2());
    }

    [MessageHandler((ushort)ServerToClientID.enemyMovement)]
    private static void GetMovement(Message message)
    {
        if(enemyList.TryGetValue(message.GetUShort(), out Enemy enemy))
        {
            enemy.MoveEnemy(message.GetUShort(), message.GetVector2());
        }
    }

    [MessageHandler((ushort)ServerToClientID.enemyDied)]
    private static void EnemyDied(Message message)
    {
        if(enemyList.TryGetValue(message.GetUShort(), out Enemy enemy))
        {
            enemyList.Remove(enemy.Id);
            Destroy(enemy.gameObject);
        }
    }
}
