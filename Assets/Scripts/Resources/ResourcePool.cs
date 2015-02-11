using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ResourcePool : MonoBehaviour 
{
    private List<Resource> _resources;

    void Awake()
    {
        var resourceTypes = System.Enum.GetValues(typeof(Resource.ResourceType)).Cast<Resource.ResourceType>();

        _resources = new List<Resource>();

        foreach(var resourceType in resourceTypes)
        {
            _resources.Add(new Resource(resourceType));
        }
        DebugHUD.MessagesCleared += DebugHUD_MessagesCleared;
    }

    private void DebugHUD_MessagesCleared(object sender, System.EventArgs e)
    {
        foreach (var resource in _resources)
            resource.Value = 0;

        foreach(var tile in Gameboard.Instance.gameTiles)
        {
            foreach(var resource in tile.TokenProperties.Resources)
            {
                _resources[(int)resource.Type].Value += resource.Value;
            }
        }

        foreach(var resource in _resources)
        {
            DebugHUD.Add(string.Format("{0}: {1}", resource.Type.ToString(), resource.Value));
        }
    }
}
