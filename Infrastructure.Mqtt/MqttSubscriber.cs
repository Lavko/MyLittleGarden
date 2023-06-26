using Domain.Entities;
using Domain.Entities.Common;
using Domain.Repositories.Common;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;

namespace Infrastructure.Mqtt;

public class MqttSubscriber
{
    private readonly MqttFactory _mqttFactory;
    private readonly IMqttClient _mqttClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<MqttSubscriber> _logger;

    public MqttSubscriber(IUnitOfWork unitOfWork, ILogger<MqttSubscriber> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mqttFactory = new MqttFactory();
        _mqttClient = _mqttFactory.CreateMqttClient();
    }

    public async Task SubscribeToMqtt()
    {
        _logger.LogDebug("Starting MQTT subscriber");

        var config = await _unitOfWork.AppConfiguration.GetCurrent();
        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithClientId(config!.MqttConfiguration.ClientId)
            .WithTcpServer(config.MqttConfiguration.TcpServer)
            .WithCredentials(config.MqttConfiguration.Login, config.MqttConfiguration.Password)
            .Build();

        _mqttClient.ApplicationMessageReceivedAsync += HandleMessageReceivedEvent;

        _logger.LogDebug(
            "Event handler subscribed. Connecting to MQTT server at: {Server}",
            config.MqttConfiguration.TcpServer
        );
        await _mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

        var mqttSubscribeOptions = _mqttFactory.CreateSubscribeOptionsBuilder().Build();

        await _mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
    }

    public async Task UnsubscribeFromMqtt()
    {
        _logger.LogDebug("Disabling MQTT subscriber");
        await _mqttClient.UnsubscribeAsync(new MqttClientUnsubscribeOptions());
        await _mqttClient.DisconnectAsync();
        _logger.LogDebug("MQTT subscriber disabled");
    }

    private async Task HandleMessageReceivedEvent(MqttApplicationMessageReceivedEventArgs e)
    {
        _logger.LogDebug("Handling MQTT event with topic: {Topic}", e.ApplicationMessage.Topic);

        var mqttSensors = await _unitOfWork.Sensors.GetAllByTypeAsync(DeviceType.Mqtt);

        _logger.LogDebug("Found {SensorsCount} MQTT sensors", mqttSensors.Count);

        var isHandled = false;
        foreach (var mqttSensor in mqttSensors)
        {
            if (e.ApplicationMessage.Topic == mqttSensor.MqttPath)
            {
                var measurement = new Measurement
                {
                    Result = BitConverter.ToString(e.ApplicationMessage.PayloadSegment.ToArray()),
                    SensorId = mqttSensor.Id
                };

                _unitOfWork.Measurements.Add(measurement);
                isHandled = true;
            }
        }

        if (!isHandled)
        {
            _logger.LogWarning(
                "MQTT event with topic '{Topic}' wasn't handled by any of the sensors",
                e.ApplicationMessage.Topic
            );
        }

        await _unitOfWork.CompleteAsync();
    }
}
