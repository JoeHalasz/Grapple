using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// loading and unloading levels
// should allow for levels to be loaded next to each other on the fly
public class LevelLoader : MonoBehaviour
{

    [SerializeField]
    public string LoadLevel = "";
    public string UnloadLevel = "";

    LevelManager levelManager;
    void Start()
    {
        levelManager = GetComponent<LevelManager>();
        if (levelManager == null)
        {
            Debug.LogError("Could not find levelManager on this object");
            return;
        }
    }

    // we dont need to do this every frame... so just do it every physics frame
    void FixedUpdate()
    {
        if (LoadLevel.Contains(' '))
        {
            LoadLevel = LoadLevel.Remove(LoadLevel.Length - 1);
            levelManager.loadLevel(LoadLevel, new Vector2(0,0));
            LoadLevel = "";
        }

        if (UnloadLevel.Contains(' '))
        {
            UnloadLevel = UnloadLevel.Remove(UnloadLevel.Length - 1);
            levelManager.unloadLevel(UnloadLevel);
            UnloadLevel = "";
        }
    }
}