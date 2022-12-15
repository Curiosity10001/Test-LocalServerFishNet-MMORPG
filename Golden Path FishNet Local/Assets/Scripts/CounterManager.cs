using FishNet.Object;
using FishNet.Object.Synchronizing;
using TMPro;
using UnityEngine;

public class CounterManager : NetworkBehaviour
{
    [SyncVar]
    int counter = 0;
    [SerializeField] TMP_Text counterText;

    private void Update()
    {
        counterText.text = counter.ToString();

    }
    [ServerRpc(RequireOwnership =false)]
    private void IncrementCounterServerRPC()
    {
        counter++;
    }

    public void Handleclick()
    {
        if(IsClient)
        {
            IncrementCounterServerRPC();
        }
    }
}
