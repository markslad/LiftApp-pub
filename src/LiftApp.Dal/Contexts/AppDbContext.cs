using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiftApp.Dal.Models;
using LiftApp.Dal.Enums;
using LiftApp.Dal.EntityConfigurations;

namespace LiftApp.Dal.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; } = default!;

        public DbSet<Borrowal> Borrowals { get; set; } = default!;

        public DbSet<Customer> Customers { get; set; } = default!;

        public DbSet<Invoice> Invoices { get; set; } = default!;

        public DbSet<Lift> Lifts { get; set; } = default!;

        public DbSet<Maintenance> Maintenances { get; set; } = default!;

        public DbSet<Office> Offices { get; set; } = default!;

        public DbSet<TimeInterval> TimeIntervals { get; set; } = default!;


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AddressEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BorrowalEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerEntityConfiguration());
            modelBuilder.ApplyConfiguration(new EntrepreneurCustomerEntityConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceEntityConfiguration());
            modelBuilder.ApplyConfiguration(new LegalEntityEntityConfiguration());
            modelBuilder.ApplyConfiguration(new LiftEntityConfiguration());
            modelBuilder.ApplyConfiguration(new MaintenanceEntityConfiguration());
            modelBuilder.ApplyConfiguration(new NonEntrepreneurCustomerEntityConfiguration());
            modelBuilder.ApplyConfiguration(new OfficeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new OwnAccountWorkerEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TimeIntervalEntityConfiguration());

            // Data Seeding
            modelBuilder.Entity<Office>().HasData(
                new Office()
                {
                    Id = 1,
                    AddressId = 1,
                });
            modelBuilder.Entity<Address>().HasData(
                new Address()
                {
                    Id = 1,
                    Country = "Česká republika",
                    City = "Praha",
                    Street = "Kamýcká",
                    HouseNumber = 1,
                    ZipCode = "100 01",
                },
                new Address()
                {
                    Id = 2,
                    Country = "Česká republika",
                    City = "Kladno",
                    Street = "Pražská",
                    HouseNumber = 120,
                    ZipCode = "160 20"
                });
            modelBuilder.Entity<Lift>().HasData(
                new List<Lift>()
                    {
                        new Lift()
                        {
                            OfficeId = 1,
                            SerialNumber = "312054861",
                            Manufacturer = "Haulotte",
                            PowerSource = PowerSource.Electric,
                            MaximumHeight = 12,
                            Eliminated = false
                        }
                    });
            modelBuilder.Entity<Maintenance>().HasData(
                new List<Maintenance>()
                    {
                        new Maintenance()
                        {
                            Id = 1,
                            LiftSerialNumber = "312054861",
                            Description = "Pravidelná údržba",
                            Price = 1200,
                            TimeIntervalId = 1,
                        }
                    });
            modelBuilder.Entity<Borrowal>().HasData(
                new Borrowal()
                {
                    Id = 1,
                    LiftSerialNumber = "312054861",
                    CustomerIdentifier = "20315648",
                    TimeIntervalId = 2,
                    PriceADay = 1200,
                    
                });
            modelBuilder.Entity<OwnAccountWorker>().HasData(
                new OwnAccountWorker()
                {
                    AddressId = 2,
                    FirstName = "Jiří",
                    Surname = "Dlouhý",
                    Identifier = "20315648",
                    Email = "jiri.dlouhy@seznam.cz",
                    PhoneNumber = "00420641215987",
                    TaxIdentificationNumber = "CZ00420641215987"
                });
            modelBuilder.Entity<Invoice>().HasData(
                new Invoice()
                {
                    BorrowalId = 1,
                    Identifier = "2023-1",
                    DateOfIssue = new DateOnly(2022, 12, 15),
                    DueDate = new DateOnly(2022, 12, 31),
                    DateOfTaxableSupply = new DateOnly(2023, 1, 1),
                    Paid = true,
                    Price = 7200,
                    Bank = "KB",
                    BankAccount = "104-12345678/0100",
                    VariableSymbol = "12345",
                    Description = "Popis položky",
                    IsExtra = false,
                    ValueAddedTaxRate = 0.21f,
                });
            modelBuilder.Entity<TimeInterval>().HasData(
                new TimeInterval()
                {
                    Id = 1,
                    DateFrom = new DateOnly(2022, 12, 10),
                    DateTo = new DateOnly(2022, 12, 12)
                },
                new TimeInterval()
                {
                    Id = 2,
                    DateFrom = new DateOnly(2022, 12, 15),
                    DateTo = new DateOnly(2022, 12, 20)
                });


        }
    }
}
