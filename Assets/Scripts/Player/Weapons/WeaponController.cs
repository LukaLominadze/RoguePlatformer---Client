using Riptide;
using UnityEngine;

public enum Weapon { melee, ranged }

public class WeaponController : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] Player player;
    [SerializeField] KeyCode _switchInput;

    public GameObject ranged;

    public ushort id;
    private ushort weaponId;
    private bool switchInput;
    private bool attackInput;

    private float angle;
    private Vector2 mousePosition;
    private Vector2 lookToDirection;


    private void Awake()
    {
        weaponId = (ushort)Weapon.melee;
    }

    void Update()
    {
        attackInput = Input.GetKey(KeyCode.Mouse0);

        if (ranged == null) return;

        if (Input.GetKeyDown(_switchInput))
        {
            switchInput = true;
        }

        mousePosition = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        lookToDirection = (Vector2)transform.position - mousePosition;
        angle = Mathf.Atan2(lookToDirection.y, lookToDirection.x) * Mathf.Rad2Deg - 180f;
        SendAngle(angle);
    }

    void FixedUpdate()
    {
        if (ranged == null) return;

        switch (weaponId)
        {
            case (ushort)Weapon.melee:
                Attack(ClientToServerID.meleeAttack);
                if (switchInput)
                {
                    SendMessage(Weapon.ranged);
                }
                break;
            case (ushort)Weapon.ranged:
                Attack(ClientToServerID.gunAttack);
                if (switchInput)
                {
                    SendMessage(Weapon.melee);
                }
                break;
        }
    }

    private void Attack(ClientToServerID id)
    {
        Message message = Message.Create(MessageSendMode.Reliable, id);
        message.AddUShort(this.id);
        message.AddBool(attackInput);
        NetworkManager.Singleton.Client.Send(message);
    }

    private void SendMessage(Weapon nextState)
    {
        Message message = Message.Create(MessageSendMode.Reliable, ClientToServerID.weaponState);
        message.AddUShort(id);
        message.AddUShort((ushort)nextState);
        Debug.Log(nextState);
        NetworkManager.Singleton.Client.Send(message);

        switchInput = false;
        weaponId = (ushort)nextState;
    }

    private void SendAngle(float angle)
    {
        Message message = Message.Create(MessageSendMode.Unreliable, ClientToServerID.gunAngle);
        message.AddUShort(id);
        message.AddFloat(angle);
        NetworkManager.Singleton.Client.Send(message);
    }

    public void SetId(ushort id)
    {
        this.id = id;
    }
}