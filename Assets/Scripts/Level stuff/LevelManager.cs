using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;



// loading and unloading levels
// should allow for levels to be loaded next to each other on the fly
public class LevelManager : MonoBehaviour
{
    // assume each level is its own game object under the AllLevels game object
    // so when we want to load a level just enable that level.
    // If you want to load levels next to each other and at the correct vertical offset
    // a levels length and vertical offset MUST be correctly saved in the Data/levels.data file

    GameObject LevelsParentObject;
    List<Level> AllLevels = new List<Level>();
    GameObject player;

    void Start()
    {
        LevelsParentObject = GameObject.Find("AllLevels");
        player = GameObject.Find("Player");
        if (LevelsParentObject == null)
        {
            Debug.LogError("Could not find AllLevels");
            return;
        }
        if (player == null)
        {
            Debug.LogError("Could not find Player");
            return;
        }
        List<GameObject> levelObjects = new List<GameObject>();
        // get each child from that and put it in the list 
        foreach(Transform child in LevelsParentObject.transform)
        {
            levelObjects.Add(child.gameObject);
        }
        loadAllLevelData(levelObjects);
    }

    // name of level to load, offset to move the level to, if we should move the player or not ( default true )
    // returns if the level is now loaded
    public bool loadLevel(string levelName, Vector2 offset, bool movePlayer=true)
    {
        Level level;
        try
        {
            level = AllLevels.Where(o => o.name == levelName).ElementAt(0);
        }
        catch(Exception e)
        {
            Debug.LogError("Could not find level " + levelName + " to load");
            return false;
        }
        if (level == null)
        {
            Debug.LogError("Could not find level " + levelName + " to load");
            return false;
        }
        if (level.levelObject.activeSelf)
        {
            Debug.Log("Level " + levelName + " is already loaded. Skipping");
            return true;
        }
        level.levelObject.SetActive(true);
        level.levelObject.transform.position = offset;
        if (movePlayer)
        {
            player.transform.position = level.startPos + offset;
        }
        return true;
    }

    // returns if that level is now unloaded
    public bool unloadLevel(string levelName)
    {
        Level level;
        try
        {
            level = AllLevels.Where(o => o.name == levelName).ElementAt(0);
        }
        catch(Exception e)
        {
            Debug.LogError("Could not find level " + levelName + " to load");
            return false;
        }
        if (level == null)
        {
            Debug.LogError("Could not find level " + levelName + " to load");
            return false;
        }
        if (!level.levelObject.activeSelf)
        {
            Debug.Log("Level " + levelName + " is already unloaded. Skipping");
            return true;
        }
        level.levelObject.SetActive(false);
        return true;
    }

    private void loadAllLevelData(List<GameObject> levelObjects)
    {
        int fileStartPos = 10;
        // open the file level.data
        string[] files = Directory.GetFiles("Assets/Data");
        string levelFile = null;
        foreach (string file in files)
        {
            if (file.Contains("levels.data"))
            {
                levelFile = file;
                break;
            }
        }
        if (levelFile == null)
        {
            Debug.LogError("Could not find levels.data");
            return;
        }

        string saveData = File.ReadAllText(levelFile);
        string[] splitByLine = saveData.Split('\n'); // \n is the character for new line
        for (int i = fileStartPos-1; i < splitByLine.Length; i++)
        {
            string name;
            int length;
            int xStartPos;
            int yStartPos;
            int endHeight;
            Debug.Log(splitByLine[i]);
            try
            {
                
                if (splitByLine[i] == "" || splitByLine[i] == " ") // skip if its an empty line
                {
                    continue;
                }
                name = splitByLine[i].Split(':')[0];
                string[] levelDataString = splitByLine[i].Split(':')[1].Split(',');
                length = int.Parse(levelDataString[0]);
                xStartPos = int.Parse(levelDataString[1]);
                yStartPos = int.Parse(levelDataString[2]);
                endHeight = int.Parse(levelDataString[3]);
            }
            catch(Exception e)
            {
                Debug.LogError("Could not parse line " + i + " in levels.data file");
                continue;
            }
            // if there was no error 
            GameObject matchingObj;
            try
            {
                matchingObj = levelObjects.Where(o => o.name == name).ElementAt(0);
            }
            catch(Exception e)
            {
                Debug.LogError("Could not find a level object named " + name);
                continue;
            }
            if (matchingObj == null)
            {
                // This level name was in the file but we couldn't find a matching level object in the scene with that name
                // TODO make error 
                Debug.LogError("Could not find a level object named " + name);
                continue;
            }
            matchingObj.SetActive(false);
            AllLevels.Add(new Level(matchingObj, length, new Vector2(xStartPos, yStartPos), endHeight));
            levelObjects.Remove(matchingObj); // take it out of the list so we know if any levels aren't loaded at the end

        }

        if (levelObjects.Count != 0)
        {
            foreach (GameObject obj in levelObjects)
            {
                Debug.LogError("Could not find level data in levels.data for " + obj.name); // ( probably rats fault tbh )
            }
        }

    }


    private class Level
    {
        public GameObject levelObject { get; }
        public string name { get; }
        public int length { get; }
        public Vector2 startPos { get; }
        public int endHeight { get; }

        public Level(GameObject obj, int length, Vector2 startPos, int endHeight)
        {
            this.levelObject = obj;
            this.name = obj.name;
            this.levelObject = obj;
            this.length = length;
            this.startPos = startPos;
            this.endHeight = endHeight;
        }

    }
}

