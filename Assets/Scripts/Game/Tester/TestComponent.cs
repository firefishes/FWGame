using FWGame;
using ShipDock.Applications;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestComponent : MonoBehaviour
{
    private FWRole role;

    // Start is called before the first frame update
    void Start()
    {
        role = new FWRole
        {
            Position = transform.localPosition,
            Direction = new Vector3(0, 0, UnityEngine.Random.Range(0.1f, 5f))
        };
        ShipDockApp.AppInstance.AddStart(OnAppStart);
    }

    private void OnAppStart()
    {
        ShipDockApp.AppInstance.Servers.AddOnServerFinished(OnServerStart);
    }

    private void OnServerStart()
    {
        var component = ShipDockApp.AppInstance.Components.GetComponentByAID(1);
        role.AddComponent(component);
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