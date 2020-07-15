using FWGame;
using ShipDock.Applications;
using UnityEngine;

public class TestComponent : MonoBehaviour
{
    private RoleEntitas role;

    // Start is called before the first frame update
    void Start()
    {
        //role = new RoleEntitas
        //{
        //    Position = transform.localPosition,
        //    Direction = new Vector3(0, 0, UnityEngine.Random.Range(0.1f, 5f))
        //};
        ShipDockApp.Instance.AddStart(OnAppStart);
    }

    private void OnAppStart()
    {
        ShipDockApp.Instance.Servers.AddOnServerFinished(OnServerStart);
    }

    private void OnServerStart()
    {
        //var component = ShipDockApp.Instance.Components.GetComponentByAID(1);
        //role.AddComponent(component);
    }

    // Update is called once per frame
    void Update()
    {
        if(role == default)
        {
            return;
        }
        transform.localPosition += role.Direction;
        //var component = ShipDockApp.AppInstance.Components.GetComponentByAID(1);
        //role.RemoveComponent(component);
        //role.Direction = Vector3.zero;
    }
}