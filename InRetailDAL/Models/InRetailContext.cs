using InRetailDAL.Dtos;
using InRetailDAL.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Models
{
    public class InRetailContext : DbContext
    {
        public InRetailContext(DbContextOptions<InRetailContext> options) : base(options)
        {
        }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<SaleOrder> SaleOrders { get; set; }
        public DbSet<SaleOrderDetail> SaleOrderDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<EncCedentials> EncCedentials { get; set; }
        public DbSet<DeviceRegistration> DeviceRegistrations { get; set; }
        public DbSet<UsersOTP> UsersOTPs { get; set; }


        //View Models
        public DbSet<OrganizationModel> OrganizationModels { get; set; }
        public DbSet<BranchModel> BranchModels { get; set; }
        public DbSet<ItemModel> ItemModels { get; set; }
        public DbSet<ItemNameDto> ItemNames { get; set; }
        public DbSet<UserModel> UserModels { get; set; }
        public DbSet<SaleOrderModel> SaleOrderModels { get; set; }
        public DbSet<SaleOrderSummary> SaleOrderSummaries { get; set; }
        public DbSet<SaleOrderDetailModel> SaleOrderDetailModels { get; set; }
        public DbSet<ItemCountModel> ItemCountModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // mapping entities for the tables
            modelBuilder.Entity<Organization>().ToTable("Organization");
            modelBuilder.Entity<Branch>().ToTable("Branch");
            modelBuilder.Entity<Users>().ToTable("Users");
            modelBuilder.Entity<Item>().ToTable("Item");
            modelBuilder.Entity<SaleOrder>().ToTable("SaleOrder");
            modelBuilder.Entity<SaleOrderDetail>().ToTable("SaleOrderDetail");
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<RefreshToken>().ToTable("RefreshToken");
            modelBuilder.Entity<EncCedentials>().HasNoKey().ToTable("EncCedentials");
            modelBuilder.Entity<DeviceRegistration>().ToTable("DeviceRegistration");
            modelBuilder.Entity<UsersOTP>().ToTable("UsersOTP");


            //mapping netities for the stored procidures
            modelBuilder.Entity<OrganizationModel>().HasNoKey().ToView(null);
            modelBuilder.Entity<BranchModel>().HasNoKey().ToView(null);
            modelBuilder.Entity<ItemModel>().HasNoKey().ToView(null);
            modelBuilder.Entity<UserModel>().HasNoKey().ToView(null);
            modelBuilder.Entity<ItemNameDto>().HasNoKey().ToView(null);
            modelBuilder.Entity<SaleOrderModel>().HasNoKey().ToView(null);
            modelBuilder.Entity<SaleOrderSummary>().HasNoKey().ToView(null);
            modelBuilder.Entity<ItemCountModel>().HasNoKey().ToView(null);

        }
    }
}
