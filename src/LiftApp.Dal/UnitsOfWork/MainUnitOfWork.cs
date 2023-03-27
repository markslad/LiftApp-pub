using LiftApp.Dal.Contexts;
using LiftApp.Dal.Interfaces;
using LiftApp.Dal.Models;
using LiftApp.Dal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Dal.UnitsOfWork
{
    public class MainUnitOfWork : IMainUnitOfWork
    {
        private readonly AppDbContext _context;

        public GenericRepository<Address> AddressRepository { get; }

        public GenericRepository<Borrowal> BorrowalRepository { get; }

        public GenericRepository<Customer> CustomerRepository { get; }

        public GenericRepository<Invoice> InvoiceRepository { get; }

        public GenericRepository<Lift> LiftRepository { get; }

        public GenericRepository<Maintenance> MaintenanceRepository { get; }

        public GenericRepository<Office> OfficeRepository { get; }

        public GenericRepository<TimeInterval> TimeIntervalRepository { get; }


        public MainUnitOfWork(AppDbContext context)
        {
            _context = context;
            AddressRepository = new GenericRepository<Address>(context);
            BorrowalRepository = new GenericRepository<Borrowal>(context);
            CustomerRepository = new GenericRepository<Customer>(context);
            InvoiceRepository = new GenericRepository<Invoice>(context);
            LiftRepository = new GenericRepository<Lift>(context);
            MaintenanceRepository = new GenericRepository<Maintenance>(context);
            OfficeRepository = new GenericRepository<Office>(context);
            TimeIntervalRepository = new GenericRepository<TimeInterval>(context);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
