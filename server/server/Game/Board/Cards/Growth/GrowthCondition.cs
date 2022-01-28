namespace server.Game.Board.Cards.Growth;

public class GrowthCondition
{
    private readonly List<SingleBambooCondition> _conditions;

    public GrowthCondition(List<SingleBambooCondition> conditions)
    {
        _conditions = conditions;
    }

    public bool Match(Board.Field.Field field)
    {
        // TODO !
        return true;
    }
}