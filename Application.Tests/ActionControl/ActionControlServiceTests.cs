using Application.ActionControl;
using Domain.Entities;
using Domain.Entities.Common;
using Domain.Models;
using Domain.Repositories.Common;
using Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.ActionControl;

public class ActionControlServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IActionRuleProcessor> _actionRuleProcessor = new();
    private readonly Mock<IOutletService> _outletService = new();
    private readonly Mock<ILogger<ActionControlService>> _logger = new();
    
    private readonly ActionControlService _actionControlService;

    public ActionControlServiceTests()
    {
        _actionControlService = new ActionControlService(
            _unitOfWork.Object,
            _actionRuleProcessor.Object,
            _outletService.Object,
            _logger.Object);
    }

    [Fact]
    public async Task TakeActions_CorrectData_OutletServiceActionsAndSaveTakenActions()
    {
        // Arrange
        var rules = new List<ActionRule>
        {
            new ActionRule()
        };

        var measure = new EnvironmentMeasure();

        var actionRuleProcessResult = new ActionRuleProcessResult
        {
            Outlet = new OutletConfiguration
            {
                Id = 1,
                Name = "Test",
                OutletId = 2,
                TimeStamp = DateTime.Now
            },
            Action = OutletAction.On,
            RuleId = 6,
            MeasureId = 9
        };

        var cancellationToken = new CancellationToken();

        _unitOfWork
            .Setup(u => u.ActionRules.GetAllAsync(cancellationToken))
            .ReturnsAsync(rules);

        _unitOfWork
            .Setup(u => u.Measures.GetLastOrDefaultAsync(cancellationToken))
            .ReturnsAsync(measure);

        _unitOfWork
            .Setup(u => u.TakenActions.Add(It.IsAny<TakenAction>()));

        _actionRuleProcessor
            .Setup(arp => arp.ProcessRule(measure, rules.First()))
            .Returns(actionRuleProcessResult);

        _outletService
            .Setup(os => os.IsOutletOn(actionRuleProcessResult.Outlet.OutletId))
            .Returns(false);
        
        // Act
        await _actionControlService.TakeActionsAsync(cancellationToken);
        
        // Assert
        _outletService
            .Verify(os => os.TurnOutletOn(actionRuleProcessResult.Outlet.PinId), Times.Once);
        
        _unitOfWork.Verify(uow => uow.TakenActions.Add(It.IsAny<TakenAction>()), Times.Once);
        
        _unitOfWork.Verify(uow => uow.CompleteAsync(cancellationToken), Times.Once);
    }
    
    [Fact]
    public async Task TakeActions_EmptyRules_DoNothing()
    {
        // Arrange
        var rules = new List<ActionRule>();

        var cancellationToken = new CancellationToken();

        _unitOfWork
            .Setup(u => u.ActionRules.GetAllAsync(cancellationToken))
            .ReturnsAsync(rules);

        // Act
        await _actionControlService.TakeActionsAsync(cancellationToken);
        
        // Assert
        _outletService
            .Verify(os => os.TurnOutletOn(It.IsAny<int>()), Times.Never);
        
        _unitOfWork.Verify(uow => uow.TakenActions.Add(It.IsAny<TakenAction>()), Times.Never);
        
        _unitOfWork.Verify(uow => uow.CompleteAsync(cancellationToken), Times.Never);
    }
    
    [Fact]
    public async Task TakeActions_EmptyMeasures_ThrowArgumentException()
    {
        // Arrange
        var rules = new List<ActionRule>
        {
            new ActionRule()
        };

        EnvironmentMeasure measure = null!;


        var cancellationToken = new CancellationToken();

        _unitOfWork
            .Setup(u => u.ActionRules.GetAllAsync(cancellationToken))
            .ReturnsAsync(rules);

        _unitOfWork
            .Setup(u => u.Measures.GetLastOrDefaultAsync(cancellationToken))
            .ReturnsAsync(measure);

        _unitOfWork
            .Setup(u => u.TakenActions.Add(It.IsAny<TakenAction>()));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(()=>_actionControlService.TakeActionsAsync(cancellationToken));
    }
    
    [Fact]
    public async Task TakeActions_DeviceAlreadyOn_DoNothing()
    {
        // Arrange
        var rules = new List<ActionRule>
        {
            new ActionRule()
        };

        var measure = new EnvironmentMeasure();

        var actionRuleProcessResult = new ActionRuleProcessResult
        {
            Outlet = new OutletConfiguration
            {
                Id = 1,
                Name = "Test",
                OutletId = 2,
                TimeStamp = DateTime.Now
            },
            Action = OutletAction.On,
            RuleId = 6,
            MeasureId = 9
        };

        var cancellationToken = new CancellationToken();

        _unitOfWork
            .Setup(u => u.ActionRules.GetAllAsync(cancellationToken))
            .ReturnsAsync(rules);

        _unitOfWork
            .Setup(u => u.Measures.GetLastOrDefaultAsync(cancellationToken))
            .ReturnsAsync(measure);

        _unitOfWork
            .Setup(u => u.TakenActions.Add(It.IsAny<TakenAction>()));

        _actionRuleProcessor
            .Setup(arp => arp.ProcessRule(measure, rules.First()))
            .Returns(actionRuleProcessResult);

        _outletService
            .Setup(os => os.IsOutletOn(actionRuleProcessResult.Outlet.PinId))
            .Returns(true);
        
        // Act
        await _actionControlService.TakeActionsAsync(cancellationToken);
        
        // Assert
        _outletService
            .Verify(os => os.TurnOutletOn(actionRuleProcessResult.Outlet.PinId), Times.Never);
        
        _unitOfWork.Verify(uow => uow.TakenActions.Add(It.IsAny<TakenAction>()), Times.Never);
        
        _unitOfWork.Verify(uow => uow.CompleteAsync(cancellationToken), Times.Once);
    }
    
    [Fact]
    public async Task TakeActions_DeviceAlreadyOff_DoNothing()
    {
        // Arrange
        var rules = new List<ActionRule>
        {
            new ActionRule()
        };

        var measure = new EnvironmentMeasure();

        var actionRuleProcessResult = new ActionRuleProcessResult
        {
            Outlet = new OutletConfiguration
            {
                Id = 1,
                Name = "Test",
                OutletId = 2,
                TimeStamp = DateTime.Now
            },
            Action = OutletAction.Off,
            RuleId = 6,
            MeasureId = 9
        };

        var cancellationToken = new CancellationToken();

        _unitOfWork
            .Setup(u => u.ActionRules.GetAllAsync(cancellationToken))
            .ReturnsAsync(rules);

        _unitOfWork
            .Setup(u => u.Measures.GetLastOrDefaultAsync(cancellationToken))
            .ReturnsAsync(measure);

        _unitOfWork
            .Setup(u => u.TakenActions.Add(It.IsAny<TakenAction>()));

        _actionRuleProcessor
            .Setup(arp => arp.ProcessRule(measure, rules.First()))
            .Returns(actionRuleProcessResult);

        _outletService
            .Setup(os => os.IsOutletOn(actionRuleProcessResult.Outlet.OutletId))
            .Returns(false);
        
        // Act
        await _actionControlService.TakeActionsAsync(cancellationToken);
        
        // Assert
        _outletService
            .Verify(os => os.TurnOutletOn(actionRuleProcessResult.Outlet.OutletId), Times.Never);
        
        _unitOfWork.Verify(uow => uow.TakenActions.Add(It.IsAny<TakenAction>()), Times.Never);
        
        _unitOfWork.Verify(uow => uow.CompleteAsync(cancellationToken), Times.Once);
    }
}