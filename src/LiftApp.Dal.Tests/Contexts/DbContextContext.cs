using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiftApp.Dal.Tests.Extensions;
using LiftApp.Dal.Contexts;
using LiftApp.Dal.Models;
using FluentAssertions;
using Xunit;
using LiftApp.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LiftApp.Dal.Tests.Contexts
{
    internal class DbContextContext
    {
        private IServiceProvider _serviceProvider;
        private AppDbContext _dbContext;

        public DbContextContext()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.RegisterServicesForDbContextTesting();
            _serviceProvider = serviceCollection.BuildServiceProvider();

            _dbContext = _serviceProvider.GetRequiredService<AppDbContext>();
        }


        internal async Task When_Borrowal_Is_Added(Borrowal borrowal)
        {
            await _dbContext.Borrowals.AddAsync(borrowal);
            await _dbContext.SaveChangesAsync();
        }

        internal void Then_Office_Has_Lift_That_Has_Borrowal(Borrowal borrowal)
        {
            var recievedBorrowal = _dbContext.Offices.Include("Lifts").Include("Lifts.Borrowals").First(o => o.Address.Street == "Testovací").Lifts.Single(l => l.SerialNumber == "123456789").Borrowals.Single();
            borrowal.Should().BeEquivalentTo(borrowal);
        }

        internal async Task And_Add_Office_With_Address_And_Single_Lift(Office office)
        {
            await _dbContext.AddAsync(office);
            await _dbContext.SaveChangesAsync();
        }

        internal async Task Then_Adding_Address_Without_Association_Throws_Exception(Address address)
        {
            Func<Task> act = async () => {
                await _dbContext.Addresses.AddAsync(address);
                await _dbContext.SaveChangesAsync();
            };
            await act.Should().ThrowAsync<DbUpdateException>();
        }

        internal async Task Then_Adding_Invoice_Without_Association_Throws_Exception(Invoice invoice)
        {
            Func<Task> act = async () => {
                await _dbContext.Invoices.AddAsync(invoice);
                await _dbContext.SaveChangesAsync();
            };
            await act.Should().ThrowAsync<DbUpdateException>();
        }

        internal async Task Then_Adding_Maintenance_Without_Association_Throws_Exception(Maintenance maintenance)
        {
            Func<Task> act = async () => {
                await _dbContext.Maintenances.AddAsync(maintenance);
                await _dbContext.SaveChangesAsync();
            };
            await act.Should().ThrowAsync<DbUpdateException>();
        }

        internal async Task Given_Reset_Db()
        {
            await _dbContext.Database.EnsureDeletedAsync();
            await _dbContext.Database.EnsureCreatedAsync();
        }
    }
}
