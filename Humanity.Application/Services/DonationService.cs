﻿using AutoMapper;
using Humanity.Application.DTOs;
using Humanity.Application.Interfaces;
using Humanity.Repository;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Humanity.API.Extensions;
using Humanity.Domain.Models;
using System.Linq;
using Humanity.Domain.Enums;

namespace Humanity.Application.Services
{
    public class DonationService : IDonationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DonationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DonationDto> GetDonationByIdAsync(int id)
        {
            // Koristimo relacijsku bazu podataka za preuzimanje donacije po ID-u
            var donation = await _unitOfWork.DonationRepository.GetById(id);
            if (donation == null)
            {
                throw new Exception("Donation not found.");
            }
            return _mapper.Map<DonationDto>(donation);
        }

        // Metoda za preuzimanje svih donacija sa mogućnošću filtriranja, sortiranja i paginacije
        public async Task<IEnumerable<DonationDto>> GetAllDonationsAsync(DonationQueryDto queryDto)
        {
            // Koristimo relacijsku bazu podataka za preuzimanje donacija
            var query = _unitOfWork.DonationRepository.GetAll();

            // Definišemo mape za sortiranje
            // Ključ predstavlja naziv kolone po kojoj se vrši sortiranje (npr. "DateReceived"),
            // a vrednost predstavlja lambda izraz za sortiranje (npr. d => d.DateReceived)
            var sortColumns = new Dictionary<string, Expression<Func<Donation, object>>>
            {
                // Sortiranje po datumu primanja donacije
                { "DateReceived", d => d.DateReceived },
                // Sortiranje po vrednosti donacije
                { "Value", d => d.Value }
            };

            // Primena sortiranja
            if (!string.IsNullOrEmpty(queryDto.SortBy) && sortColumns.ContainsKey(queryDto.SortBy))
            {
                // Ako je u queryDto specificirana kolona za sortiranje i ta kolona postoji u sortColumns mapi,
                // pozivamo ekstenziju ApplySorting koja primenjuje sortiranje
                query = query.ApplySorting(queryDto, sortColumns);
            }
            else
            {
                // Ako nije specificirano sortiranje, podrazumevano sortiramo prema datumu primanja donacije
                query = query.OrderBy(d => d.DateReceived);
            }

            // Filtriranje po datumu primanja donacije
            if (queryDto.DateReceived.HasValue)
            {
                // Ako queryDto sadrži vrednost za DateReceived, filtriramo donacije prema datumu.
                // Poređenje koristi samo datum (bez vremena)
                query = query.Where(d => d.DateReceived.Date == queryDto.DateReceived.Value.Date);
            }

            // Filtriranje po kategoriji
            if (queryDto.Category.HasValue)
            {
                // Ako queryDto sadrži vrednost za kategoriju, pretvaramo tu vrednost iz int u odgovarajući enum
                var categoryEnum = (DonationCategory)queryDto.Category.Value;
                // Filtriramo donacije tako da odgovaraju specificiranoj kategoriji
                query = query.Where(d => d.Category == categoryEnum);
            }

            // Filtriranje po imenu ili prezimenu donatora
            if (!string.IsNullOrEmpty(queryDto.DonorName))
            {
                // Ako je u queryDto specificirano ime ili prezime donatora, konvertujemo ga u mala slova za poređenje
                var donorName = queryDto.DonorName.ToLower();

                // Filtriramo donacije prema imenu ili prezimenu donatora.
                // Ignorišemo anonimne donacije (d.IsAnonymous).
                // Koristimo Contains za delimično poređenje (npr. pretraga za "John" pronalazi "Johnson").
                query = query.Where(d =>
                    !d.IsAnonymous &&
                    (d.Donor.FirstName.ToLower().Contains(donorName) ||
                     d.Donor.LastName.ToLower().Contains(donorName)));
            }

            // Primenjujemo straničenje
            query = query.ApplyPaging(queryDto);

            // Mapiranje donacija u DonationDto
            // Izvršavamo upit sa ToListAsync kako bismo preuzeli rezultate iz baze podataka.
            // Rezultat (lista donacija) mapiramo u DTO (DonationDto)
            return _mapper.Map<IEnumerable<DonationDto>>(await query.ToListAsync());
        }

        public async Task<DonationDto> CreateDonationAsync(CreateDonationDto createDonationDto)
        {
            // Dohvatamo informacije o donoru
            var donor = await _unitOfWork.UserRepository.GetById(createDonationDto.DonorId);
            if (donor == null)
            {
                throw new Exception("Donor not found.");
            }

            // Kreiramo donaciju u relacijskoj bazi podataka
            var donation = _mapper.Map<Donation>(createDonationDto);
            donation.IsAnonymous = donor.IsAnonymous; // Postavljamo IsAnonymous prema donoru
            donation.IsDistributed = false; // Inicijalno postavljamo IsDistributed na false
            donation.DateReceived = DateTime.UtcNow;

            var addedDonation = await _unitOfWork.DonationRepository.Add(donation);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<DonationDto>(addedDonation);
        }

        public async Task<bool> UpdateDonationAsync(int id, UpdateDonationDto updateDonationDto)
        {
            // Ažuriramo donaciju u relacijskoj bazi podataka
            var donation = await _unitOfWork.DonationRepository.GetById(id);
            if (donation == null) return false;
           
            _mapper.Map(updateDonationDto, donation);
            await _unitOfWork.DonationRepository.Update(donation); 
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteDonationAsync(int id)
        {
            // Brišemo donaciju iz relacijske baze podataka
            var donation = await _unitOfWork.DonationRepository.GetById(id);
            if (donation == null) return false;
            
            await _unitOfWork.DonationRepository.Delete(donation);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
