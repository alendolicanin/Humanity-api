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
            // Mapira `CreateUserDto` na `User`
            CreateMap<CreateUserDto, User>()
                // Dodatno prilagođava mapiranje tako da svojstvo `UserName` u `User` bude mapirano sa `Email` iz `CreateUserDto`
                .ForMember(u => u.UserName, opt => opt.MapFrom(dto => dto.Email));
            CreateMap<UpdateUserDto, User>()
                // Postavlja pravilo da se mapiraju samo ona svojstva koja nisu `null` (izbegava prepisivanje postojećih vrednosti `null` vrednostima)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<RegisterDto, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(dto => dto.Email))
                .ForMember(u => u.Email, opt => opt.MapFrom(dto => dto.Email));

            // Donation mappings
            CreateMap<CreateDonationDto, Donation>();
            CreateMap<Donation, DonationDto>()
                // Prilagođava mapiranje ime i prezime donora u slučaju anonimnih donacija
                .ForMember(dest => dest.DonorFirstName, opt => opt.MapFrom(src =>
                    src.IsAnonymous || src.Donor == null ? null : src.Donor.FirstName))
                .ForMember(dest => dest.DonorLastName, opt => opt.MapFrom(src =>
                    src.IsAnonymous || src.Donor == null ? null : src.Donor.LastName));
            CreateMap<UpdateDonationDto, Donation>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // DistributedDonation mappings
            CreateMap<CreateDistributedDonationDto, DistributedDonation>()
                // Postavlja `DateDistributed` na trenutni datum i vreme ako nije prosleđena vrednost
                .ForMember(dd => dd.DateDistributed, opt => opt.MapFrom(src => src.DateDistributed == default ? DateTime.UtcNow : src.DateDistributed));
            CreateMap<DistributedDonation, DistributedDonationDto>()
                // Prilagođava mapiranje podataka o primaocu
                .ForMember(dest => dest.RecipientFirstName, opt => opt.MapFrom(src => src.Recipient.FirstName))
                .ForMember(dest => dest.RecipientLastName, opt => opt.MapFrom(src => src.Recipient.LastName))
                // Mapira ID donacije i kategoriju iz povezane donacije
                .ForMember(dest => dest.DonationId, opt => opt.MapFrom(src => src.Donation.Id))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Donation.Category))
                // Mapira ID donora i račun iz povezanih entiteta
                .ForMember(dest => dest.DonorId, opt => opt.MapFrom(src => src.Donation.DonorId))
                .ForMember(dest => dest.Receipt, opt => opt.MapFrom(src => src.Receipt));

            // Receipt mappings
            CreateMap<Receipt, ReceiptDto>()
                // Mapira `Receipt` na `ReceiptDto` sa podrazumevanim mapiranjem, uz prilagođavanje za potpis
                .ForMember(dest => dest.Signature, opt => opt.MapFrom(src => src.Signature));

            // ThankYouNote mappings
            CreateMap<CreateThankYouNoteDto, ThankYouNote>();
            CreateMap<ThankYouNote, ThankYouNoteDto>()
                // Kombinuje ime i prezime pošiljaoca u jedno svojstvo
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => $"{src.Sender.FirstName} {src.Sender.LastName}"))
                // Kombinuje ime i prezime donora u jedno svojstvo
                .ForMember(dest => dest.DonorName, opt => opt.MapFrom(src => $"{src.Donor.FirstName} {src.Donor.LastName}"));
            CreateMap<UpdateThankYouNoteDto, ThankYouNote>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
