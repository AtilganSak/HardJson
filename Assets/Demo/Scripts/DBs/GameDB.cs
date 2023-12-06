using System.Collections.Generic;

[System.Serializable]
public class GameDB : DB<GameDB>
{    
    public string testString;

    public GameDB()
    {
        testString = string.Empty;
    }
}