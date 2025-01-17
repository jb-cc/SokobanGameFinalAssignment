﻿using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
namespace libs;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

public sealed class GameEngine
{

    private static GameEngine? _instance;
        private IGameObjectFactory gameObjectFactory;

        public static GameEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameEngine();
                }
                return _instance;
            }
        }

        private GameEngine()
        {
            gameObjectFactory = new GameObjectFactory();
            gameObjects = new List<GameObject>();
            stateHistory = new Stack<List<GameObject>>();
            map = new Map();
            playerHistory = new Stack<(int, int)>();  // Stack to store player positions for undo functionality
        }

        public void Clear() {
            gameObjectFactory = new GameObjectFactory();
            gameObjects = new List<GameObject>();
            stateHistory = new Stack<List<GameObject>>();
            map = new Map();
            playerHistory = new Stack<(int, int)>();  // Stack to store player positions for undo functionality
        }

        private GameObject? _focusedObject;
        private List<GameObject> gameObjects;
        private readonly List<GameObject> _gameObjects;
        private Map map;
        private Stack<List<GameObject>> stateHistory;  // Stack to store map states for undo functionality

        private Stack<(int, int)> playerHistory;

        public bool isMainMenu = false;
        public bool isDialog = false;
        public bool isIntro = false;
        private dynamic _mainMenuData = FileHandler.ReadMainMenu();
        private dynamic _pauseMenuData = FileHandler.ReadPauseMenu();
        private dynamic _introData = FileHandler.ReadIntro();

        public void SaveCurrentState()
        {
            List<GameObject> gameObjectsCopy = new List<GameObject>();
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.Type != GameObjectType.Player)
                {
                    gameObjectsCopy.Add((GameObject)gameObject.Clone());
                }
            }
            foreach (GameObject gameObject in gameObjects)
            {
                Console.WriteLine(gameObject.Type + " " + gameObject.PosX + " " + gameObject.PosY + " " + gameObject.CharRepresentation + " " + gameObject.Color);
            }
            stateHistory.Push(gameObjectsCopy);
            playerHistory.Push((_focusedObject.PosX, _focusedObject.PosY));
            
        }

        public void SaveProgress(){
            
            JObject mapObject = new JObject(
                new JProperty("map", new JObject(
                new JProperty("width", map.MapWidth),
                new JProperty("height", map.MapHeight)
                ))
            );
            string json = mapObject.ToString(Newtonsoft.Json.Formatting.Indented);

            string json1 = "{\"gameObjects\":"+JsonConvert.SerializeObject(gameObjects)+"}";
            //string json1 = "{""map"":{""width"":"+map.MapWidth+",""height"":"+map.MapHeight+"}, ""gameObjects"":";
            
            JObject obj1 = JObject.Parse(json);
            JObject obj2 = JObject.Parse(json1);
            obj1.Merge(obj2);

            // Convert merged JObject back to JSON string
            string mergedJson = obj1.ToString();

            FileHandler.saveJson(mergedJson);
            //File.WriteAllText(@"D:\Schule\FH\4.Semester\CD\Soko\SokobanGame\Save.json", mergedJson);

        }

        
        public void UndoMove()
        {
            Console.WriteLine("UndoMove" + stateHistory.Count);
            if (stateHistory.Count > 0)
            {
                var gameObjectsCopy = new List<GameObject>(gameObjects); // Create a copy of gameObjects

                foreach (GameObject gameObject in gameObjectsCopy)
                {
                    if (gameObject.Type != GameObjectType.Player)
                    {
                        gameObjects.Remove(gameObject); // Remove elements from the original list
                    }
                }
                
                // Add the gameObjects from the previous state to the gameObjects list
                foreach (GameObject gameObject in stateHistory.Pop())
                {
                    gameObjects.Add(gameObject);
                }

                // Restore the player position from playerHistory
                (int x, int y) = playerHistory.Pop();
                _focusedObject.PosX = x;
                _focusedObject.PosY = y;
                Render(); 
            }
            else
            {
                Console.WriteLine("No more moves to undo.");
            }
        }


        public bool CanUndo()
        {
            return stateHistory.Count > 0;
        }

        public Map GetMap()
        {
            return map;
        }

        public GameObject GetFocusedObject()
        {
            return _focusedObject;
        }

        public void Setup()
        {
            Clear();
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            dynamic gameData = FileHandler.ReadJson();  // Load game data
            if (gameData.mainMenu != null)
                isMainMenu = true;
            if (isMainMenu)
            {
                map.MapWidth = 0;
                map.MapHeight = 0;
            }
            else
            {
                isMainMenu = false;
                map.MapWidth = gameData.map.width;
                map.MapHeight = gameData.map.height;

                foreach (var gameObject in gameData.gameObjects)
                {
                    AddGameObject(CreateGameObject(gameObject));
                }

                _focusedObject = gameObjects.OfType<Player>().FirstOrDefault();  // Ensure there is a player
            }
        }

        public void SetFocused(GameObject gameObject)
        {
            _focusedObject = gameObject;
        }

        public void Render()
        {
            if (isMainMenu)
            {
                
                Console.Clear();
                Console.WriteLine(_mainMenuData.title);
                Console.WriteLine(_mainMenuData.welcomeMessage);
                
                if (FileHandler.saveExists)
                {
                    foreach (string option in _mainMenuData.saveExistsOptions)
                    {
                        Console.WriteLine(option);
                    }
                }
                else
                {
                    foreach (string option in _mainMenuData.noSaveOptions)
                    {
                        Console.WriteLine(option);
                    }
                }
                Console.WriteLine(_mainMenuData.footer);
                
                /*if (!FileHandler.saveExists)
                {
                    Console.Clear();
                    Console.WriteLine("====== Main Menu ======");
                    Console.WriteLine("Welcome to Sokoban!");
                    Console.WriteLine("Press 1 to start the game");
                    Console.WriteLine("Press 2 to exit");
                    Console.WriteLine("=======================");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("====== Main Menu ======");
                    Console.WriteLine("Welcome to Sokoban!");
                    Console.WriteLine("Press 1 to start the game");
                    Console.WriteLine("Press 2 to continue");
                    Console.WriteLine("Press 3 to exit");
                    Console.WriteLine("=======================");
                
                }
                */

            }
            else if (isIntro)
            {
                Console.Clear();
                Console.WriteLine(_introData.title);
                Console.WriteLine(_introData.description);
                Console.WriteLine(_introData.gameplay);
                Console.WriteLine(_introData.callToAction);
                Console.WriteLine(_introData.startPrompt);
            }
            else if (isDialog)
            {
                Console.Clear();
                Console.WriteLine(_pauseMenuData.title);
                foreach (string option in _pauseMenuData.options)
                {
                    Console.WriteLine(option);
                }
                Console.WriteLine(_pauseMenuData.footer);
            }
            else
            {
                Console.Clear();
                map.Initialize();
                PlaceGameObjects();


                for (int i = 0; i < map.MapHeight; i++)
                {
                    for (int j = 0; j < map.MapWidth; j++)
                    {
                        DrawObject(map.Get(i, j));
                    }
                    Console.WriteLine();
                }
            }
        }

        


        public GameObject CreateGameObject(dynamic obj)
        {
            return gameObjectFactory.CreateGameObject(obj);
        }

        // THIS IS LINQ, REQUIREMENT FULFILLED GG EZ
        public void AddGameObject(GameObject gameObject)
        {
            if (!gameObjects.Any(p => p.Type == GameObjectType.Player && gameObject.Type == GameObjectType.Player))
            {
                gameObjects.Add(gameObject);
            }
        }

        private void PlaceGameObjects()
        {
            foreach (GameObject obj in gameObjects)
            {
                map.Set(obj);
            }
        }

        private void DrawObject(GameObject gameObject)
        {
            Console.ResetColor();
            if (gameObject != null)
            {
                Console.ForegroundColor = gameObject.Color;
                Console.Write(gameObject.CharRepresentation);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(' ');
            }
        }


    public bool CheckWinCondition() {
        List<GameObject> goals = new List<GameObject>();
        List<GameObject> boxes = new List<GameObject>();


        foreach (GameObject gameObject in gameObjects) {
            if (gameObject.Type == GameObjectType.Goal) {
                goals.Add(gameObject);
            }
            else if (gameObject.Type == GameObjectType.Box) {
                boxes.Add(gameObject);
            }
        }

        // Check if each goal is covered by at least one box
        foreach (GameObject goal in goals) {
            bool goalIsCovered = false;
            foreach (GameObject box in boxes) {
                if (goal.PosX == box.PosX && goal.PosY == box.PosY) {
                    goalIsCovered = true;
                    break; // Stop checking other boxes once a covering box is found
                }
            }
            if (!goalIsCovered) {
                return false; // Return false as soon as an uncovered goal is found
            }
        }

        return true; // Return true if all goals are covered
    }
}