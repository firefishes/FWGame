using ShipDock.Applications;
using UnityEngine;

public class TkzsStartUp : MonoBehaviour
{
    void Start()
    {
        ShipDockApp.StartUp(120);
    }

    private void OnDestroy()
    {
        ShipDockApp.Close();
    }
}
