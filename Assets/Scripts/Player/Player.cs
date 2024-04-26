using Riptide;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _username;
    [SerializeField] Interpolator interpolator;

    [SerializeField] WeaponController weaponController;
    public WeaponSwitch weaponSwitch;
    public WeaponScript meleeScript;
    public WeaponScript gunScript;
    [SerializeField] RangedFollowPlayer gunParent;
    [SerializeField] GameObject rangedMelee;

    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }
    public bool IsLocal { get; private set; }
    private string username;
    private ushort tick;

    public Vector2 oldPosition;
    public Vector2 newPosition;

    public static void Spawn(ushort id, string username, Vector2 position)
    {
        Player player;
        if (id == NetworkManager.Singleton.Client.Id)
        {
            player = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = true;
            player.weaponController.SetId(id);
        }
        else
        {
            player = Instantiate(GameLogic.Singleton.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = false;
        }

        player.name = $"{(string.IsNullOrEmpty(username) ? $"Guest {id}" : username)}";
        player.Id = id;
        player.username = username;
        player._username.text = player.name;

        list.Add(id, player);
    }

    private void MovePlayer(ushort tick, Vector2 newPosition)
    {
        this.tick = tick;
        oldPosition = transform.position;
        interpolator.NewUpdate(tick, newPosition);
        this.newPosition = newPosition;
    }

    private void MoveKnife(Vector2 newPosition)
    {
        rangedMelee.transform.position = newPosition;
    }

    public void SetWeapon(RangedFollowPlayer gunParent, GameObject gun, WeaponScript weaponScript, SpriteRenderer weaponSprite, Weapon weaponType)
    {
        //add gun parameters only if its a gun

        switch (weaponType)
        {
            case Weapon.melee:
                meleeScript = weaponScript;
                weaponSwitch.meleeSprite = weaponSprite;
                UIManager.Singleton.SetWeaponSlot(weaponSprite.sprite, Weapon.melee);
                break;
            case Weapon.ranged:
                gunScript = weaponScript;
                weaponSwitch.rangedSprite = weaponSprite;
                weaponController.ranged = gun;
                this.gunParent = gunParent;
                UIManager.Singleton.SetWeaponSlot(weaponSprite.sprite, Weapon.ranged);
                break;
        }
    }

    private void OnDestroy()
    {
        list.Remove(Id);
    }

    [MessageHandler((ushort)ServerToClientID.playerSpawned)]
    private static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetString(), message.GetVector2());
    }

    [MessageHandler((ushort)ServerToClientID.playerMovement)]
    private static void HorizontalMovement(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Player player))
        {
            player.MovePlayer(message.GetUShort(), message.GetVector2());
            if (player.TryGetComponent<DashEmission>(out DashEmission trail))
            {
                trail.EmittingTrail(message.GetBool());
            }
        }
    }

    [MessageHandler((ushort)ServerToClientID.serverEvent)]
    private static void OnDisconnect(Message message)
    {
        Destroy(list[message.GetUShort()].gameObject);
    }

    [MessageHandler((ushort)ServerToClientID.rangedEvent)]
    private static void GetKnifeState(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Player player))
        {
            player.MoveKnife(message.GetVector2());
            player.rangedMelee.SetActive(message.GetBool());
        }
    }

    [MessageHandler((ushort)ServerToClientID.weaponState)]
    private static void GetWeaponState(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Player player))
        {
            player.weaponSwitch.SetCurrentId(message.GetUShort());
        }
    }

    [MessageHandler((ushort)ServerToClientID.meleeAttack)]
    private static void MeleeAttack(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Player player))
        {
            player.meleeScript.Attack();
        }
    }

    [MessageHandler((ushort)ServerToClientID.gunAngle)]
    private static void SetAngle(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Player player))
        {
            player.gunParent.SetAngle(message.GetFloat());
        }
    }

    [MessageHandler((ushort)ServerToClientID.gunAttack)]
    private static void GunAttack(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Player player))
        {
            player.gunScript.Attack();
        }
    }

    [MessageHandler((ushort)ServerToClientID.pickUpMelee)]
    private static void PickUpWeapon(Message message)
    {
        ushort id = message.GetUShort();
        ushort nameId = message.GetUShort();
        ushort droppedItemId = message.GetUShort();
        if (Player.list.TryGetValue(id, out Player player))
        {
            if (player.meleeScript.nameId == nameId) return;

            GameObject newWeapon = Instantiate(GameLogic.Singleton.weaponList[nameId], player.transform.position, Quaternion.identity, player.transform);

            player.SetWeapon(null, null, newWeapon.GetComponent<WeaponScript>(), newWeapon.GetComponent<SpriteRenderer>(), Weapon.melee);

            Destroy(LootPrefabList.droppedItems[droppedItemId].gameObject);
            LootPrefabList.droppedItems.Remove(droppedItemId);
        }
    }
}