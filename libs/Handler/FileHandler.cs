﻿using System.Reflection.Metadata.Ecma335;

namespace libs;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

public static class FileHandler
{
    private static string filePath;
    private static string defaultPath;
    private static int currLevel = 0;
    private static int maxLevel = 4;
    private readonly static string envVar = "GAME_SETUP_PATH";
    

    static FileHandler()
    {
        Initialize();
    }

    public static void setPath(){
        if(currLevel > maxLevel){
            Console.Clear();
            Console.WriteLine("\u2800\u2800\u2800\u2800\u2800\u2800\u2880\u28e0\u28f4\u2836\u2836\u28e6\u28e4\u28c0\u28e4\u28f6\u28f6\u28f6\u2876\u2836\u2836\u2836\u2836\u2836\u2836\u2836\u2836\u2836\u2836\u2836\u2836\u2836\u28f6\u28f6\u28f6\u28e6\u28c4\u28e4\u28f4\u2836\u2836\u28a6\u28e4\u2840\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u28a0\u287f\u2809\u2880\u28e0\u28c0\u28c0\u2808\u28fd\u280f\u2809\u2809\u2819\u281b\u281b\u281b\u281b\u281b\u281b\u281b\u281b\u281b\u281b\u281b\u281b\u281b\u281b\u281b\u2809\u2809\u2809\u28ff\u2801\u28c4\u28e6\u28e4\u28c0\u2808\u283b\u28e6\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u28ff\u2800\u28a0\u285f\u2809\u2809\u2809\u281b\u28ff\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u28b9\u285f\u280b\u2809\u2809\u28bb\u2846\u2800\u28bb\u2844\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u28ff\u2800\u28b8\u2847\u2800\u2800\u2800\u2800\u28ff\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u28b8\u2847\u2800\u2800\u2800\u2808\u28f7\u2800\u28b8\u2847\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2820\u28ff\u2800\u2838\u28c7\u2800\u2800\u2800\u2800\u28ff\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u28b8\u2847\u2800\u2800\u2800\u28b0\u284f\u2800\u28f8\u2807\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u28bb\u28c6\u2800\u28bb\u28c4\u2800\u2800\u2800\u28bb\u2846\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u28ff\u2800\u2800\u2800\u2880\u287e\u2801\u2880\u287f\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u2800\u28bb\u28c6\u2800\u2839\u28e7\u28c4\u2800\u2808\u28b7\u2840\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u28fc\u2803\u2800\u2880\u28f4\u281f\u2801\u28a0\u287e\u2801\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2819\u28b7\u28c4\u2800\u2819\u283b\u28a6\u28fc\u28f7\u2840\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2880\u28fc\u28ef\u28f4\u283e\u280b\u2801\u28c0\u28f4\u281f\u2801\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2809\u283b\u28a6\u28c4\u28c0\u2808\u2809\u283b\u28c6\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u28e0\u287e\u280b\u2801\u2880\u28e0\u28e4\u283e\u280b\u2801\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2808\u2819\u281b\u2832\u2836\u283c\u28b7\u28e4\u2840\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u28e0\u28fe\u283f\u2836\u2836\u281b\u280b\u2809\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2808\u283b\u28e6\u2840\u2800\u2800\u2800\u2800\u2800\u28e0\u287e\u280b\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2808\u28b7\u2844\u2800\u2800\u2880\u287e\u280b\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u28ff\u2840\u2800\u28fe\u2803\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2880\u287f\u2800\u2800\u28bb\u2846\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2880\u28fe\u2803\u2800\u2800\u2808\u28bf\u2844\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2880\u287e\u2801\u2800\u2800\u2800\u2800\u2800\u28bb\u2844\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2880\u28fe\u28c1\u28c0\u28c0\u28c0\u28c0\u28c0\u28c0\u28c0\u28ff\u28c4\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u28a0\u285f\u2809\u2809\u2809\u2809\u2809\u2809\u2809\u2809\u2809\u2809\u2839\u2847\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u28b8\u2847\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2847\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u28fc\u283f\u2836\u2836\u2836\u2836\u2836\u2836\u2836\u2836\u2836\u2836\u283e\u28b7\u2844\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u28bf\u28e4\u28e4\u28e4\u28e4\u28e4\u28e4\u28e4\u28e4\u28e4\u28e4\u28e4\u28e4\u28fc\u2807\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\n\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\u2800\n\u28f6\u28f6\u2876\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\u28f6\n\u28ff\u28ff\u28f6\u28f6\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\u28ff\n __     __                    _       \n \\ \\   / /                   (_)      \n  \\ \\_/ /__  _   _  __      ___ _ __  \n   \\   / _ \\| | | | \\ \\ /\\ / / | '_ \\ \n    | | (_) | |_| |  \\ V  V /| | | | |\n    |_|\\___/ \\__,_|   \\_/\\_/ |_|_| |_|\n                                      \n                                      ");
            Environment.Exit(0); // Exit the game after displaying the win message.
        }

        filePath = defaultPath +$"{currLevel}.json";
            
        
    }
    public static void nextLevel(){
        currLevel++;
        setPath();
    }

    public static void saveJson(string mergedJson){

        JObject currentLevelObj = new JObject(
                new JProperty("level", currLevel)
        );
        string leveltoJ = currentLevelObj.ToString(Newtonsoft.Json.Formatting.Indented);
        JObject obj1 = JObject.Parse(mergedJson);
        JObject obj2 = JObject.Parse(leveltoJ);
        obj1.Merge(obj2);
        string mJ = obj1.ToString();

        File.WriteAllText(defaultPath + "Save.json", mJ);
    }

    private static void Initialize()
    {
        
        //currLevel = 0;
        if(Environment.GetEnvironmentVariable(envVar) != null){

            defaultPath = Environment.GetEnvironmentVariable(envVar);

            if(File.Exists(defaultPath + "Save.json")){
                filePath = defaultPath + "Save.json";
                dynamic data = ReadJson();
                currLevel = data.level;
            }else{
                filePath = defaultPath +$"{currLevel}.json";
             }
        }; 

    }

    public static dynamic ReadJson()
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new InvalidOperationException("JSON file path not provided in environment variable");
        }

        try
        {
            string jsonContent = File.ReadAllText(filePath);
            dynamic jsonData = JsonConvert.DeserializeObject(jsonContent);
            return jsonData;
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException($"JSON file not found at path: {filePath}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error reading JSON file: {ex.Message}");
        }
    }

    
}