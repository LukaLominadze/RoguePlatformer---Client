using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private static GameLogic _singleton;
    public static GameLogic Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
                if (_singleton != value)
                {
                    Destroy(value);
                }
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
        foreach(GameObject weapon in weaponPrefabs)
        {
            weaponList.Add(CharValues.GetValueOfString(weapon.name), weapon);
        }
    }

    public GameObject LocalPlayerPrefab => localPlayerPrefab;
    public GameObject PlayerPrefab => playerPrefab;
    public GameObject EnemyPrefab => enemyPrefab;
    public GameObject IndicatorPrefab => indicatorPrefab;

    public Dictionary<ushort, GameObject> weaponList = new Dictionary<ushort, GameObject>();

    [Header("Prefabs")]
    [SerializeField] GameObject localPlayerPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject indicatorPrefab;
    [Space(3)]
    [SerializeField] List<GameObject> weaponPrefabs = new List<GameObject>();
}
