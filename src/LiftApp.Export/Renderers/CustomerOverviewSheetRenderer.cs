using LiftApp.Dal.Interfaces;
using LiftApp.Dal.Models;
using LiftApp.Export.Interfaces;
using LiftApp.Export.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Export.Renderers
{
    public class CustomerOverviewSheetRenderer : OverviewSheetRendererBase, IOverviewSheetRenderer
    {
        private readonly IOptions<CustomerOverviewOptions> _customerOverviewOptions;

        public CustomerOverviewSheetRenderer(IOptions<CustomerOverviewOptions> customerOverviewOptions,
            IMainUnitOfWork mainUnitOfWork)
            : base(mainUnitOfWork)
        {
            _customerOverviewOptions = customerOverviewOptions;
        }

        public override async Task RenderContentAsync((DateOnly dateFrom, DateOnly dateTo)? dateRange = null)
        {
            var customers = await _mainUnitOfWork.CustomerRepository.GetAsync(include: customers => customers
                .Include(customer => customer.Address),
                orderBy:
                customers => customers
                .OrderBy(customer => customer.Identifier.Length)
                .ThenBy(customer => customer.Identifier));

            if (customers is null)
                throw new NullReferenceException("Customers collection recieved null");
            if (_worksheet is null)
                throw new NullReferenceException("Worksheet is null");

            _worksheet.Cells[_customerOverviewOptions.Value.ExportDateRowIndex, _customerOverviewOptions.Value.ExportDateColumnIndex] = DateTime.Now;

            if (dateRange is not null)
            {
                _worksheet.Cells[_customerOverviewOptions.Value.DateRangeRowIndex, _customerOverviewOptions.Value.DateRangeColumnIndex] = $"{dateRange.Value.dateFrom} - {dateRange.Value.dateTo}";
            }
            else
            {
                _worksheet.Cells[_customerOverviewOptions.Value.DateRangeRowIndex, _customerOverviewOptions.Value.DateRangeColumnIndex] = "-";
            }

            var startRow = _customerOverviewOptions.Value.StartRowIndex;
            foreach (var customer in customers)
            {
                _worksheet.Cells[startRow, _customerOverviewOptions.Value.IdentifierColumnIndex] = customer.Identifier;
                _worksheet.Cells[startRow, _customerOverviewOptions.Value.PhoneNumberColumnIndex] = customer.PhoneNumber;
                _worksheet.Cells[startRow, _customerOverviewOptions.Value.EmailColumnIndex] = customer.Email;

                switch (customer)
                {
                    case NonEntrepreneurCustomer customerWithConcreteType:
                        _worksheet.Cells[startRow, _customerOverviewOptions.Value.FirstNameColumnIndex] = customerWithConcreteType.FirstName;
                        _worksheet.Cells[startRow, _customerOverviewOptions.Value.SurnameColumnIndex] = customerWithConcreteType.Surname;
                        break;
                    case OwnAccountWorker customerWithConcreteType:
                        _worksheet.Cells[startRow, _customerOverviewOptions.Value.TaxIdentificationNumberColumnIndex] = customerWithConcreteType.TaxIdentificationNumber;
                        _worksheet.Cells[startRow, _customerOverviewOptions.Value.FirstNameColumnIndex] = customerWithConcreteType.FirstName;
                        _worksheet.Cells[startRow, _customerOverviewOptions.Value.SurnameColumnIndex] = customerWithConcreteType.Surname;
                        break;
                    case LegalEntity customerWithConcreteType:
                        _worksheet.Cells[startRow, _customerOverviewOptions.Value.TaxIdentificationNumberColumnIndex] = customerWithConcreteType.TaxIdentificationNumber;
                        _worksheet.Cells[startRow, _customerOverviewOptions.Value.NameColumnIndex] = customerWithConcreteType.Name;
                        break;
                    default:
                        throw new NotImplementedException("Unkown customer type");
                }

                _worksheet.Cells[startRow, _customerOverviewOptions.Value.StreetColumnIndex] = customer.Address.Street;
                _worksheet.Cells[startRow, _customerOverviewOptions.Value.HouseNumberColumnIndex] = customer.Address.HouseNumber;
                _worksheet.Cells[startRow, _customerOverviewOptions.Value.CityColumnIndex] = customer.Address.City;
                _worksheet.Cells[startRow, _customerOverviewOptions.Value.ZipCodeColumnIndex] = customer.Address.ZipCode;
                _worksheet.Cells[startRow, _customerOverviewOptions.Value.CountryColumnIndex] = customer.Address.Country;

                startRow++;

            }
            _worksheet.Columns.AutoFit();
        }
    }
}
