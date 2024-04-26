using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDisconnect : MonoBehaviour
{
    public static List<GameObject> list = new List<GameObject>();

    private void Start()
    {
        list.Add(this.gameObject);
    }
}
