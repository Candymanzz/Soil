using AutoMapper;
using server.Models;
using server.DTOs;

namespace server.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();

            // Crop mappings
            CreateMap<Crop, CropDto>();
            CreateMap<CreateCropDto, Crop>();
            CreateMap<UpdateCropDto, Crop>();

            // Field mappings
            CreateMap<Field, FieldDto>();
            CreateMap<CreateFieldDto, Field>();
            CreateMap<UpdateFieldDto, Field>();

            // SoilType mappings
            CreateMap<SoilType, SoilTypeDto>();
            CreateMap<CreateSoilTypeDto, SoilType>();
            CreateMap<UpdateSoilTypeDto, SoilType>();

            // Fertilizer mappings
            CreateMap<Fertilizer, FertilizerDto>();
            CreateMap<CreateFertilizerDto, Fertilizer>();
            CreateMap<UpdateFertilizerDto, Fertilizer>();

            // Equipment mappings
            CreateMap<Equipment, EquipmentDto>();
            CreateMap<CreateEquipmentDto, Equipment>();
            CreateMap<UpdateEquipmentDto, Equipment>();
        }
    }
}