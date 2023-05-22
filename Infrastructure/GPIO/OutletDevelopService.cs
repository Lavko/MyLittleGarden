using Domain.Services;

namespace Infrastructure.GPIO;

public class OutletDevelopService : IOutletService
{
    private readonly Dictionary<int, bool> _outlets;

    public OutletDevelopService()
    {
        _outlets = new Dictionary<int, bool>();
    }

    public void TurnOutletOn(int outletPin)
    {
        InitializeOutlet(outletPin);
        _outlets[outletPin] = true;
    }

    public void TurnOutletOff(int outletPin)
    {
        InitializeOutlet(outletPin);
        _outlets[outletPin] = false;
    }

    public bool IsOutletOn(int outletPin)
    {
        InitializeOutlet(outletPin);
        return _outlets[outletPin];
    }

    private void InitializeOutlet(int outletPin)
    {
        if (!_outlets.ContainsKey(outletPin))
        {
            _outlets.Add(outletPin, false);
        }
    }
}