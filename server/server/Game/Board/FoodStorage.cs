namespace server.Game.Board;

public class FoodStorage
{
    private int _greenAmount;
    private int _yellowAmount;
    private int _pinkAmount;

    public void StoreFood(int greenAmount, int yellowAmount, int pinkAmount)
    {
        _greenAmount  += greenAmount;
        _yellowAmount += yellowAmount;
        _pinkAmount   += pinkAmount;
    }
    
    public void DisposeFood(int greenAmount, int yellowAmount, int pinkAmount)
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