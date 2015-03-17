using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ResourcePool : MonoBehaviour 
{
    public static ResourcePool Instance { get { return _instance; } }
    private static ResourcePool _instance;

    private List<Resource> _resources;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (this != _instance)
            Destroy(this.gameObject);

        var resourceTypes = System.Enum.GetValues(typeof(Resource.ResourceType)).Cast<Resource.ResourceType>();
        _resources = new List<Resource>();
        foreach(var resourceType in resourceTypes)
        {
            _resources.Add(new Resource(resourceType));
        }
        DebugHUD.MessagesCleared += DebugHUD_MessagesCleared;
        Gameboard.Instance.GameboardReset += Instance_GameStarted;
    }

    private void Instance_GameStarted()
    {
        ClearResources();
    }

    void Update()
    {
        UpdateResourceCount();
    }

    public Resource GetResource(Resource.ResourceType resourceType)
    {
        return _resources[(int)resourceType];
    }
    private void DebugHUD_MessagesCleared(object sender, System.EventArgs e)
    {
        //foreach(var resource in _resources)
        //{
        //    DebugHUD.Add(string.Format("{0}: {1}", resource.Type.ToString(), resource.Value));
        //}
    }
    
    private void ClearResources()
    {
        foreach (var resource in _resources)
            resource.Value = 0;
    }

    public void UpdateResourceCount()
    {
        ClearResources();
        foreach (var tile in Gameboard.Instance.gameTiles)
        {
            foreach (var resource in tile.TokenProperties.Resources)
            {
                _resources[(int)resource.Type].Value += resource.Value;
            }
        }
    }
}
