using Riptide;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public short horizontalInput;

    public bool[] inputArray;

    public bool rangedInput;
    private bool abilityInput;

    private void Start()
    {
        inputArray = new bool[2];
    }

    private void Update()
    {
        horizontalInput = (short)Input.GetAxisRaw("Horizontal");

        inputArray[0] = Input.GetKey(KeyCode.Space); //jump boolean
        inputArray[1] = Input.GetKey(KeyCode.LeftShift); //dash boolean
        rangedInput = Input.GetKey(KeyCode.Mouse1);
        abilityInput = Input.GetKey(KeyCode.P);
    }

    private void FixedUpdate()
    {
        Message sendInputs = Message.Create(MessageSendMode.Unreliable, ClientToServerID.input);
        sendInputs.AddShort(horizontalInput);
        sendInputs.AddBools(inputArray);
        sendInputs.AddBool(rangedInput);
        sendInputs.AddBool(abilityInput);
        NetworkManager.Singleton.Client.Send(sendInputs);

        for(ushort i = 0; i < inputArray.Length; i++)
        {
            inputArray[i] = false;
        }
    }
}
