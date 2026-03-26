using System;
using System.Collections.Generic;

[Serializable]
public class GameRecord
{
    public int score;
    public string date;
}

[Serializable]
public class GameHistory
{
    public List<GameRecord> records = new List<GameRecord>();
}