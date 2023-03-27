using LiftApp.Dal.Models;
using LiftApp.Dal.Tests.Contexts;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

[assembly: LightBddScope]
namespace LiftApp.Dal.Tests.Features
{
    [FeatureDescription("Database context tests")]
    [Collection("Sequential")]
    public class Db_Context_Communication_Feature : FeatureFixture
    {
        [Scenario]
        [ClassData(typeof(TestBorrowalClassData))]
        public async Task Test_Insert_Borrowal(Office office, Borrowal borrowal)
        {
            await Runner.WithContext<DbContextContext>()
                .AddAsyncSteps(
                _ => _.Given_Reset_Db(),
                _ => _.And_Add_Office_With_Address_And_Single_Lift(office),
                _ => _.When_Borrowal_Is_Added(borrowal)
                ).AddSteps(
                _ => _.Then_Office_Has_Lift_That_Has_Borrowal(borrowal)
                )
                .RunAsync();
        }

        [Scenario]
        [ClassData(typeof(TestInvalidEntitiesClassData))]
        public async Task Test_That_It_Is_Unable_To_Add_Some_Entities_Without_Association(Maintenance maintenance, Address address, Invoice invoice)
        {
            await Runner.WithContext<DbContextContext>()
                .AddAsyncSteps(
                _ => _.Given_Reset_Db(),
                _ => _.Then_Adding_Maintenance_Without_Association_Throws_Exception(maintenance),
                _ => _.Then_Adding_Address_Without_Association_Throws_Exception(address),
                _ => _.Then_Adding_Invoice_Without_Association_Throws_Exception(invoice)
                )
                .RunAsync();
        }

        public class TestBorrowalClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] 
                {
                    new Office()
                    {
                        Address = new Address()
                        {
                            Country = "Èeská Republika",
                            City = "Praha",
                            Street = "Testovací",
                            HouseNumber = 1,
                            ZipCode = "100 20",
                        },
                        Lifts = new List<Lift>()
                        {
                            new Lift()
                            {
                                Manufacturer = "Haulotte",
                                MaximumHeight = 12,
                                SerialNumber = "123456789",
                                PowerSource = Enums.PowerSource.Electric,
                                Eliminated = false,
                            }
                        }
                    },
                    new Borrowal()
                    {
                        PriceADay = 200,
                        TimeInterval = new TimeInterval()
                        {
                            DateFrom = new DateOnly(2023, 1, 31),
                            DateTo = new DateOnly(2023, 2, 3)
                        },
                        LiftSerialNumber = "123456789",
                        Customer = new LegalEntity()
                        {
                            Identifier = "001234567",
                            Email = "company@gmail.com",
                            PhoneNumber = "00420713456789",
                            Name = "Company",
                            Address = new Address()
                            {
                                City = "Most",
                                Street = "Horní",
                                HouseNumber = 50,
                                ZipCode = "434 20",
                                Country = "Èeská republika",
                            }
                        },
                    }
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class TestInvalidEntitiesClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Maintenance()
                    {
                        Description = "desc",
                        Price = 5000
                    },
                    new Address()
                    {
                        City = "Praha",
                        Street = "Ulice",
                        Country = "Èeská republika",
                        HouseNumber = 50,
                        ZipCode = "100 21"
                    },
                    new Invoice()
                    {
                        DateOfIssue = new DateOnly(2023, 1, 1),
                        DueDate = new DateOnly(2023, 1, 1),
                        DateOfTaxableSupply = new DateOnly(2023, 1, 1),
                        Price = 5000,
                        Bank = "Bank",
                        BankAccount = "101-1215455/2032",
                        Identifier = "2023-1",
                        ValueAddedTaxRate = 0.21f,
                        Paid = false,
                        VariableSymbol = "1234",
                        Description = "Popis",
                        IsExtra = false
                    }
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}