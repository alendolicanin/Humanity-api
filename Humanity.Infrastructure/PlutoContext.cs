using Humanity.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Humanity.Infrastructure
{
    public class PlutoContext : IdentityDbContext<User>
    {
        public PlutoContext(DbContextOptions<PlutoContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Pozivanje osnovne implementacije metode
            base.OnModelCreating(modelBuilder);

            // Definisanje relacije između User i Donation entiteta
            modelBuilder.Entity<User>()
                .HasMany(u => u.Donations) // Jedan korisnik ima mnogo donacija
                .WithOne(d => d.Donor) // Svaka donacija ima jednog donora
                .HasForeignKey(d => d.DonorId) // Strani ključ je DonorId
                .OnDelete(DeleteBehavior.Cascade); // Brisanje korisnika briše sve njegove donacije

            // Definisanje relacije između User i DistributedDonation entiteta
            modelBuilder.Entity<User>()
                .HasMany(u => u.ReceivedDonations) // Jedan korisnik može primiti mnogo distribuisanih donacija
                .WithOne(dd => dd.Recipient) // Svaka distribuisana donacija ima jednog primaoca
                .HasForeignKey(dd => dd.RecipientId) // Strani ključ je RecipientId
                .OnDelete(DeleteBehavior.Restrict); // Onemogućava brisanje korisnika ako postoje povezane distribuisane donacije

            // Definisanje relacije između User i ThankYouNote (poslate zahvalnice) entiteta
            modelBuilder.Entity<User>()
                .HasMany(u => u.SentThankYouNotes) // Jedan korisnik može poslati mnogo zahvalnica
                .WithOne(tn => tn.Sender) // Svaka zahvalnica ima jednog pošiljaoca
                .HasForeignKey(tn => tn.SenderId) // Strani ključ je SenderId
                .OnDelete(DeleteBehavior.Restrict); // Onemogućava brisanje korisnika ako postoje povezane zahvalnice

            // Definisanje relacije između User i ThankYouNote (primljene zahvalnice) entiteta
            modelBuilder.Entity<User>()
                .HasMany(u => u.ReceivedThankYouNotes) // Jedan korisnik može primiti mnogo zahvalnica
                .WithOne(tn => tn.Donor) // Svaka zahvalnica ima jednog donora
                .HasForeignKey(tn => tn.DonorId) // Strani ključ je DonorId
                .OnDelete(DeleteBehavior.Restrict); // Onemogućava brisanje korisnika ako postoje povezane zahvalnice

            // Definisanje relacije između Donation i DistributedDonation entiteta
            modelBuilder.Entity<Donation>()
                .HasMany(d => d.DistributedDonations) // Jedna donacija može imati mnogo distribuisanih donacija
                .WithOne(dd => dd.Donation) // Svaka distribuisana donacija je povezana sa jednom donacijom
                .HasForeignKey(dd => dd.DonationId) // Strani ključ je DonationId
                .OnDelete(DeleteBehavior.Cascade); // Brisanje donacije briše sve povezane distribuisane donacije

            // Definisanje relacije između DistributedDonation i User (primaoca) entiteta
            modelBuilder.Entity<DistributedDonation>()
                .HasOne(dd => dd.Recipient) // Svaka distribuisana donacija ima jednog primaoca
                .WithMany(u => u.ReceivedDonations) // Jedan korisnik može primiti mnogo distribuisanih donacija
                .HasForeignKey(dd => dd.RecipientId) // Strani ključ je RecipientId
                .OnDelete(DeleteBehavior.Restrict); // Onemogućava brisanje korisnika ako postoje povezane distribuisane donacije

            // Definisanje relacije između DistributedDonation i Receipt entiteta
            modelBuilder.Entity<DistributedDonation>()
                .HasOne(dd => dd.Receipt) // Svaka distribuisana donacija ima jedan račun
                .WithOne(r => r.DistributedDonation) // Jedan račun je povezan sa jednom distribuisanom donacijom
                .HasForeignKey<Receipt>(r => r.DistributedDonationId) // Strani ključ u tabeli Receipt je DistributedDonationId
                .OnDelete(DeleteBehavior.Cascade); // Brisanje distribuisane donacije briše i povezani račun

            // Definisanje relacije između ThankYouNote i User (donora) entiteta
            modelBuilder.Entity<ThankYouNote>()
                .HasOne(tn => tn.Donor) // Svaka zahvalnica ima jednog donora
                .WithMany(u => u.ReceivedThankYouNotes) // Jedan korisnik može primiti mnogo zahvalnica
                .HasForeignKey(tn => tn.DonorId) // Strani ključ je DonorId
                .OnDelete(DeleteBehavior.Restrict); // Onemogućava brisanje korisnika ako postoje povezane zahvalnice

            // Definisanje relacije između ThankYouNote i User (pošiljaoca) entiteta
            modelBuilder.Entity<ThankYouNote>()
                .HasOne(tn => tn.Sender) // Svaka zahvalnica ima jednog pošiljaoca
                .WithMany(u => u.SentThankYouNotes) // Jedan korisnik može poslati mnogo zahvalnica
                .HasForeignKey(tn => tn.SenderId) // Strani ključ je SenderId
                .OnDelete(DeleteBehavior.Restrict); // Onemogućava brisanje korisnika ako postoje povezane zahvalnice

            // Podešavanje preciznosti decimalnih vrednosti u svim entitetima
            var decimalProps = modelBuilder.Model
                .GetEntityTypes() // Dobijanje svih entiteta u modelu
                .SelectMany(t => t.GetProperties()) // Dobijanje svih svojstava entiteta
                .Where(p => (System.Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType) == typeof(decimal)); // Filtriranje svojstava koja su tipa decimal

            // Postavljanje preciznosti i skale za decimalna svojstva
            foreach (var property in decimalProps)
            {
                property.SetPrecision(18); // Postavljanje preciznosti na 18
                property.SetScale(2); // Postavljanje skale na 2
            }
        }

        public DbSet<Donation> Donations { get; set; }
        public DbSet<DistributedDonation> DistributedDonations { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ThankYouNote> ThankYouNotes { get; set; }
    }
}
