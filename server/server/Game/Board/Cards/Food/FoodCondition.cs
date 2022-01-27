namespace server.Game.Board.Cards.Food;

public class FoodCondition
{
    private readonly int _greenAmount;
    private readonly int _yellowAmount;
    private readonly int _pinkAmount;

    public FoodCondition(int green, int yellow, int pink)
    {
        _greenAmount  = green;
        _yellowAmount = yellow;
        _pinkAmount   = pink;
    }

    public bool Match(FoodStorage storage)
    {
        return    storage.GetGreenAmount()  >= _greenAmount
               && storage.GetYellowAmount() >= _yellowAmount
               && storage.GetPinkAmount()   >= _pinkAmount;
    }
}