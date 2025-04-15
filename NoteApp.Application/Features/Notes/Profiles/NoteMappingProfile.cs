using AutoMapper;
using NoteApp.Application.Features.Notes.Dtos;
using NoteApp.Domain.Entities;

namespace NoteApp.Application.Features.Notes.Profiles;

public class NoteMappingProfile : Profile
{
    public NoteMappingProfile()
    {
        CreateMap<Note, NoteDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category!.Name))
            .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Ratings.Count > 0 
                ? src.Ratings.Average(r => r.Score) 
                : 0));
    }
}
