using System.Collections.Generic;

public class ScoreDTO
{
    public string Name { get; set; }
    public int Score { get; set; }
}

public class HighScoresDTO
{
    public List<ScoreDTO> Scores { get; set; }
}

