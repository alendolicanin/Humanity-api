using Humanity.Application.DTOs;
using Humanity.Domain.Models;
using AutoMapper;

namespace Humanity.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(dto => dto.Email));
            CreateMap<UpdateUserDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<RegisterDto, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(dto => dto.Email))
                .ForMember(u => u.Email, opt => opt.MapFrom(dto => dto.Email));

            // Donation mappings
            CreateMap<CreateDonationDto, Donation>();
            CreateMap<Donation, DonationDto>()
                .ForMember(dest => dest.DonorFirstName, opt => opt.MapFrom(src =>
                    src.IsAnonymous || src.Donor == null ? null : src.Donor.FirstName))
                .ForMember(dest => dest.DonorLastName, opt => opt.MapFrom(src =>
                    src.IsAnonymous || src.Donor == null ? null : src.Donor.LastName));
            CreateMap<UpdateDonationDto, Donation>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // DistributedDonation mappings
            CreateMap<CreateDistributedDonationDto, DistributedDonation>()
                .ForMember(dd => dd.DateDistributed, opt => opt.MapFrom(src => src.DateDistributed == default ? DateTime.UtcNow : src.DateDistributed));
            CreateMap<DistributedDonation, DistributedDonationDto>()
                .ForMember(dest => dest.RecipientFirstName, opt => opt.MapFrom(src => src.Recipient.FirstName))
                .ForMember(dest => dest.RecipientLastName, opt => opt.MapFrom(src => src.Recipient.LastName))
                .ForMember(dest => dest.DonationId, opt => opt.MapFrom(src => src.Donation.Id))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Donation.Category))
                .ForMember(dest => dest.DonorId, opt => opt.MapFrom(src => src.Donation.DonorId))
                .ForMember(dest => dest.Receipt, opt => opt.MapFrom(src => src.Receipt));

            // Receipt mappings
            CreateMap<Receipt, ReceiptDto>()
                .ForMember(dest => dest.Signature, opt => opt.MapFrom(src => src.Signature));

            // ThankYouNote mappings
            CreateMap<CreateThankYouNoteDto, ThankYouNote>();
            CreateMap<ThankYouNote, ThankYouNoteDto>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => $"{src.Sender.FirstName} {src.Sender.LastName}"))
                .ForMember(dest => dest.DonorName, opt => opt.MapFrom(src => $"{src.Donor.FirstName} {src.Donor.LastName}"));
            CreateMap<UpdateThankYouNoteDto, ThankYouNote>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
