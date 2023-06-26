using AutoMapper;
using Domain.DTOs.AppConfiguration;
using Domain.DTOs.Devices;
using Domain.DTOs.Measurements;
using Domain.DTOs.Sensors;
using Domain.Entities;

namespace Domain.Profiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Device, DeviceDto>();
        CreateMap<CreateDeviceDto, Device>();
        CreateMap<UpdateDeviceDto, Device>();

        CreateMap<Sensor, SensorDto>();
        CreateMap<CreateSensorDto, Sensor>();
        CreateMap<UpdateSensorDto, Sensor>();

        CreateMap<Measurement, MeasurementDto>();
        CreateMap<CreateMeasurementDto, Measurement>();

        CreateMap<AppConfigurationDto, AppConfiguration>().ReverseMap();
        CreateMap<MqttConfigurationDto, MqttConfiguration>().ReverseMap();
    }
}
