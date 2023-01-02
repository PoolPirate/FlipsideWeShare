namespace FlipsideWeShare.Models.Raw;
public class RawCurrency
{
    public string Symbol { get; private set; }

    public RawCurrency(string symbol)
    {
        Symbol = symbol;
    }
}
