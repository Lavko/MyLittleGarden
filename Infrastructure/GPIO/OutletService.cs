using System.Device.Gpio;
using Domain.Services;

namespace Infrastructure.GPIO;

public class OutletService : IOutletService
{
    private readonly GpioController _controller;

    public OutletService()
    {
        _controller = new GpioController();
    }

    public void TurnOutletOn(int outletPin)
    {
        var isPinOpen = _controller.IsPinOpen(outletPin);
        if (!isPinOpen)
        {
            InitializePin(outletPin);
        }
        _controller.Write(outletPin, PinValue.Low);
    }

    public void TurnOutletOff(int outletPin)
    {
        var isPinOpen = _controller.IsPinOpen(outletPin);
        if (!isPinOpen)
        {
            InitializePin(outletPin);
        }
        _controller.Write(outletPin, PinValue.High);
    }

    public bool IsOutletOn(int outletPin)
    {
        var isPinOpen = _controller.IsPinOpen(outletPin);
        if (!isPinOpen)
        {
            InitializePin(outletPin);
        }
        return _controller.Read(outletPin) == PinValue.Low;
    }

    private void InitializePin(int outletPin)
    {
        _controller.OpenPin(outletPin, PinMode.Output);
    }
}