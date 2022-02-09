namespace server.Game.Board.Decks;

public class Deck<T>
{
    protected readonly List<T> Items = new();
    private readonly Random _rng = new(DateTime.Now.Millisecond);

    protected void Shuffle()
    {
        for (int i = Items.Count-1; i > 0; i--) {  
            int k = _rng.Next(i + 1);
            (Items[k], Items[i]) = (Items[i], Items[k]);
        } 
    }
    
    public T? Pick()
    {
        if (Items.Count <= 0) return default;
        T picked = Items[0];
        Items.Remove(picked);
        return picked;
    }

    public void Return(T toReturn)
    {
        Items.Add(toReturn);
    }
}