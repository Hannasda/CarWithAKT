using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CarWithAKT.Models
{
    public class CarWithAKTContext : DbContext
    {
        public CarWithAKTContext()//:base("Data source=cars.db")
        {

        }
        public DbSet<Client> Client { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Worker> Worker { get; set; }
        public DbSet<Firm> Firm { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Car> Car { get; set; }
        public DbSet<Location> Locations { set; get; }
        public DbSet<Status> Status { set; get; }
        public DbSet<Order> Order { set; get; }
        public DbSet<Basket> Basket { set; get; }
        public DbSet<CarsOnOrder> CarsOnOrder { set; get; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //var initilazi = new SqliteCreateDatabaseIfNotExists<CarWithAKTContext>(modelBuilder);
            //Database.SetInitializer(initilazi);
        }
    }

    public class CarWithAKTDbInitializer : DropCreateDatabaseAlways<CarWithAKTContext>
    {
        protected override void Seed(CarWithAKTContext db)
        {
            db.Role.Add(new Role { Id = 1, Name = "client" });
            db.Role.Add(new Role { Id = 2, Name = "worker" });
            db.Role.Add(new Role { Id = 3, Name = "admin" });
            db.Client.Add(new Client { Id = 1, Email = "Ben.Ten@gmail.com", Name = "Ben", Password = "123", Phone = "3822222222222", Surname = "Ten", RoleId = 2 });
            db.Client.Add(new Client { Id = 2, Email = "John.Josh@gmail.com", Name = "John", Password = "12345", Phone = "3811111111111", Surname = "Josh", RoleId = 1 });
            db.Client.Add(new Client { Id = 3, Email = "Admin.Admin@gmail.com", Name = "Admin", Password = "12345", Phone = "3833333333333", Surname = "Admin", RoleId = 3 });
            db.Worker.Add(new Worker { Id = 1, ClientId = 1, Middlename = "BenTen", Start = DateTime.Now });

            db.Firm.Add(new Firm { Id = 1, Name = "BMW" });
            db.Firm.Add(new Firm { Id = 2, Name = "Mercedes" });
            db.Firm.Add(new Firm { Id = 3, Name = "Toyota" });

            db.Country.Add(new Country { Id = 1, Name = "Германия", Shortname = "de" });
            db.Country.Add(new Country { Id = 2, Name = "Франция", Shortname= "fr" });
            db.Country.Add(new Country { Id = 3, Name = "Япония", Shortname = "jp" });

            db.Car.Add(new Car { Img = "../Img/Cars/1.jpg", Id = 1, FirmId = 1, CountryId = 1, Body_type = "Внедорожник", Color = "Чёрный", Door_num = 4, Driver_type = "Левый", Engine_cap = 2926, Engine_type = "L6", Fuel_type = "Дизель", Fuel_use = 10, Mass = 2065, Power = 184, Price = 405000, Seats_num = 5, Transmission_type = "Полный", Modelname = "X5 3.0d" });
            db.Car.Add(new Car { Img = "../Img/Cars/2.jpg", Id = 2, FirmId = 3, CountryId = 3, Body_type = "Седан", Color = "Чёрный", Door_num = 4, Driver_type = "Левый", Engine_cap = 3456, Engine_type = "3.5 Dual VVT-i", Fuel_type = "Бензин", Fuel_use = 10, Mass = 2050, Power = 277, Price = 671000, Seats_num = 5, Transmission_type = "Передний", Modelname = "Camry 3.5" });

            db.Locations.Add(new Location { Id = 1, Name = "Главный салон", Adress = "г.Харьков, ул.Сумская, 1242352" });
            db.Locations.Add(new Location { Id = 1, Name = "Второй салон", Adress = "г.Харьков, пр.Науки, 1244123" });

            db.Status.Add(new Status { Id = 1, Name = "Новый" });
            db.Status.Add(new Status { Id = 2, Name = "Принят" });
            db.Status.Add(new Status { Id = 3, Name = "Отклонён" });
            db.Status.Add(new Status { Id = 4, Name = "Выполнен" });
            
            base.Seed(db);
        }
    }
}