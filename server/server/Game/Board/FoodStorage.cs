namespace server.Game.Board;

public class FoodStorage
{
    private int _greenAmount;
    private int _yellowAmount;
    private int _pinkAmount;
    
    public FoodStorage() {}

    public FoodStorage(int green, int yellow, int pink)
    {
        _greenAmount  = green;
        _yellowAmount = yellow;
        _pinkAmount   = pink;
    }

    public bool Match(FoodStorage storage)
    {
        return storage.GetGreenAmount()  >= _greenAmount
            && storage.GetYellowAmount() >= _yellowAmount
            && storage.GetPinkAmount()   >= _pinkAmount;
    }

    public void Store(FoodStorage amount)
    {
        Store(amount._greenAmount, amount._yellowAmount, amount._pinkAmount);
    }

    public void Store(int greenAmount, int yellowAmount, int pinkAmount)
    {
        _greenAmount  += greenAmount;
        _yellowAmount += yellowAmount;
        _pinkAmount   += pinkAmount;
    }

    public void Dispose(FoodStorage amount)
    {
        Store(amount._greenAmount, amount._yellowAmount, amount._pinkAmount);
    }
    
    public void Dispose(int greenAmount, int yellowAmount, int pinkAmount)
    {
        _greenAmount  -= greenAmount;
        _yellowAmount -= yellowAmount;
        _pinkAmount   -= pinkAmount;
    }

    public int GetGreenAmount()
    {
        return _greenAmount;
    } 

    public int GetYellowAmount()
    {
        return _yellowAmount;
    } 

    public int GetPinkAmount()
    {
        return _pinkAmount;
    } 
}