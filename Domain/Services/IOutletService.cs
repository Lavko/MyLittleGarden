namespace Domain.Services;

public interface IOutletService
{
    void TurnOutletOn(int outletPin);
    void TurnOutletOff(int outletPin);
    bool IsOutletOn(int outletPin);
}