using AutoMapper;
using Domain.DTOs;
using Domain.DTOs.ActionRules;
using Domain.DTOs.Measures;
using Domain.DTOs.TakenActions;
using Domain.Entities;

namespace Domain.Profiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<OutletConfiguration, OutletDto>();
        CreateMap<EnvironmentMeasure, MeasureDto>();

        CreateMap<TakenAction, TakenActionDto>()
            .ForMember(dest => dest.ComparisonType, opt => opt.MapFrom(src => src.ActionRule.ComparisonType.ToString()))
            .ForMember(dest => dest.MeasureProperty, opt => opt.MapFrom(src => src.ActionRule.MeasureProperty))
            .ForMember(dest => dest.Action, opt => opt.MapFrom(src => src.ActionRule.OutletAction.ToString()))
            .ForMember(dest => dest.ComparisonValue, opt => opt.MapFrom(src => src.ActionRule.ComparisonValue))
            .ForMember(dest => dest.OutletId, opt => opt.MapFrom(src => src.ActionRule.Outlet.OutletId))
            .ForMember(dest => dest.MeasureValue,
                opt => opt.MapFrom(src => GetMeasureValue(src.EnvironmentMeasure, src.ActionRule.MeasureProperty)));

        CreateMap<ActionRule, ActionRuleDto>()
            .ForMember(dest => dest.OutletId, opt => opt.MapFrom(src => src.Outlet.OutletId))
            .ForMember(dest => dest.OutletAction, opt => opt.MapFrom(src => src.OutletAction.ToString()))
            .ForMember(dest => dest.ComparisonType, opt => opt.MapFrom(src => src.ComparisonType.ToString()))
            .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.IsSchedule ? $"{src.ScheduledTime!.Hour}:{src.ScheduledTime.Minute}:{src.ScheduledTime.Second}" : string.Empty));

        CreateMap<CreateActionRuleDto, ActionRule>()
            .ForMember(dest => dest.ScheduledTime,
                opt => opt.MapFrom(src => new ScheduledTime(src.Hour!.Value, src.Minute!.Value, src.Second!.Value)));
    }

    private static string GetMeasureValue(EnvironmentMeasure measure, string measureProperty)
    {
        return measureProperty switch
        {
            nameof(measure.TimeStamp) => measure.TimeStamp.ToString("dd-MM-yyyy HH:mm:ss"),
            nameof(measure.GroundHumidity) => measure.GroundHumidity.ToString("0.##"),
            nameof(measure.Humidity) => measure.Humidity.ToString("0.##"),
            nameof(measure.Temperature) => measure.Temperature.ToString("0.##"),
            nameof(measure.Pressure) => measure.Pressure.ToString("0.##"),
            _ => throw new ArgumentException($"Unknown measure property: {measureProperty}")
        };
    }
}