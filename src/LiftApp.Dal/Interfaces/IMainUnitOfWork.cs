using LiftApp.Dal.Models;
using LiftApp.Dal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Dal.Interfaces
{
    public interface IMainUnitOfWork
    {
        GenericRepository<Address> AddressRepository { get; }

        GenericRepository<Borrowal> BorrowalRepository { get; }

        GenericRepository<Customer> CustomerRepository { get; }

        GenericRepository<Invoice> InvoiceRepository { get; }

        GenericRepository<Lift> LiftRepository { get; }

        GenericRepository<Maintenance> MaintenanceRepository { get; }

        GenericRepository<Office> OfficeRepository { get; }

        GenericRepository<TimeInterval> TimeIntervalRepository { get; }

        Task SaveChangesAsync();
    }
}
