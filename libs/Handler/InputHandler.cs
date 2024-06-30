namespace libs;

public sealed class InputHandler{

    private static InputHandler? _instance;
    private GameEngine engine;

    public static InputHandler Instance {
        get{
            if(_instance == null)
            {
                _instance = new InputHandler();
            }
            return _instance;
        }
    }

    private InputHandler() {
        // Initialize properties if needed
        engine = GameEngine.Instance;
    }

    public void Handle(ConsoleKeyInfo keyInfo)
    {
        Console.WriteLine($"Key pressed: {keyInfo.Key} with modifiers {keyInfo.Modifiers}");  // This will show what key is pressed

        GameObject focusedObject = engine.GetFocusedObject();
        if (engine.isMainMenu)
        {
            if (!FileHandler.saveExists)
                switch (keyInfo.Key)
                {
                    case ConsoleKey.D1:
                        engine.isMainMenu = false;
                        engine.isIntro = true;
                        break;
                    case ConsoleKey.D2:
                        engine.isMainMenu = false;
                        Environment.Exit(0);
                        break;
                }
            else
            {
                switch (keyInfo.Key)
                {
                    case ConsoleKey.D1:
                        engine.isMainMenu = false;
                        engine.isIntro = true;
                        break;
                    case ConsoleKey.D2:
                        engine.isMainMenu = false;
                        FileHandler.LoadGame();
                        break;
                    case ConsoleKey.D3:
                        Environment.Exit(0);
                        break;
                }
            
            }
        }
        else if(engine.isIntro)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.D1:
                    engine.isIntro = false;
                    engine.isMainMenu = false;
                    FileHandler.FirstLevel();
                    break;
            }
        }
        else if (engine.isDialog)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.D1:
                    engine.isDialog = false;
                    break;
                case ConsoleKey.D2:
                    Environment.Exit(0);
                    break;
            }
        }
        else
        {
            if (focusedObject != null) {
                // Handle keyboard input to move the player and save the state before the move
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        engine.SaveCurrentState();
                        focusedObject.Move(0, -1);
                        break;
                    case ConsoleKey.DownArrow:
                        engine.SaveCurrentState();
                        focusedObject.Move(0, 1);
                        break;
                    case ConsoleKey.LeftArrow:
                        engine.SaveCurrentState();
                        focusedObject.Move(-1, 0);
                        break;
                    case ConsoleKey.RightArrow:
                        engine.SaveCurrentState();
                        focusedObject.Move(1, 0);
                        break;
                    case ConsoleKey.Z:
                        if (keyInfo.Modifiers == ConsoleModifiers.Control) {
                            Console.WriteLine("Undoing last move...");
                            engine.UndoMove();
                        }
                        break;
                    case ConsoleKey.S:
                    if (keyInfo.Modifiers == ConsoleModifiers.Control) {
                        Console.WriteLine("Saving States...");
                        engine.SaveProgress();
                    }
                    break;
                    case ConsoleKey.P:
                        engine.isDialog = true;
                        break;
                    default:
                        Console.WriteLine("No action assigned for this key.");
                        break;
                }
            }
            else 
            { 
                Console.WriteLine("No focused object available.");
            }
        } 
    }

}