using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InRetailDAL.Dtos;
using InRetailDAL.Models;

namespace InRetail.InRetailMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<OrganizationInsertDto, Organization>();
            CreateMap<OrganizationUpdateDto, Organization>();
            CreateMap<OrganizationUpdateObj, Organization>();
            CreateMap<Organization, OrganizationUpdateDto>();
            CreateMap<Organization, OrganizationUpdateObj>();
            CreateMap<Organization, ResponseDto>();
            CreateMap<Organization, CreateOrgResponseDto>();
            

            CreateMap<BranchInsertDto, Branch>();
            CreateMap<BranchUpdateDto, Branch>();
            CreateMap<BranchUpdateObj, Branch>();
            CreateMap<Branch, BranchUpdateDto>();
            CreateMap<Branch, BranchUpdateObj>();
            CreateMap<Branch, ResponseDto>(); 
            CreateMap<Branch, CreateBrachResponseDto>(); 

            CreateMap<UserInsertDto, Users>();
            CreateMap<UserUpdateDto, Users>();
            CreateMap<UserUpdateObj, Users>();
            CreateMap<UserChangePasswordDto, Users>();
            CreateMap<UserIMEIChangePasswordDto, Users>();
            CreateMap<UserResetPasswordDto, Users>();
            CreateMap<UserUpdateDto, Users>();
            CreateMap<Users, UserUpdateDto>();
            CreateMap<Users, UserUpdateObj>();
            CreateMap<Users, ResponseDto>();

            CreateMap<ItemInsertDto, Item>();
            CreateMap<ItemUpdateDto, Item>();
            CreateMap<ItemUpdateObj, Item>();
            CreateMap<Item, ItemUpdateDto>();
            CreateMap<Item, ItemUpdateObj>();
            CreateMap<CategoryInsertModel, Item>();
            CreateMap<CategoryUpdateDto, Item>();
            CreateMap<CategoryUpdateObj, Item>();
            CreateMap<Item, CategoryUpdateDto>();
            CreateMap<Item, CategoryUpdateObj>();
            CreateMap<Item, ResponseDto>();

            CreateMap<SaleOrderDetailInsertDto, SaleOrderDetail>();
            CreateMap<List<SaleOrderDetailInsertDto>, List<SaleOrderDetail>>();
            CreateMap<List<SaleOrderDetailUpdateDto>, List<SaleOrderDetail>>();
            CreateMap<SaleOrderDetailUpdateDto, SaleOrderDetail>();
            CreateMap<SaleOrderDetailInsertDto, SaleOrderDetail>();
            CreateMap<UpdateSaleOrderDto, SaleOrder>();
            CreateMap<SaleOrder, UpdateSaleOrderDto>();
            CreateMap<SaleOrderInsertDto, SaleOrder>();
            CreateMap<SaleOrder, SaleOrderInsertDto>();
            CreateMap<SaleOrderUpdateDto, SaleOrder>();
            CreateMap<SaleOrder, SaleOrderUpdateDto>();
            CreateMap<SaleOrderDetailUpdateDto, SaleOrderDetailInsertDto>();
            CreateMap<SaleOrderDetail, SaleOrderDetailInsertDto>();
            CreateMap<SaleOrderDetail, SaleOrderDetailUpdateDto>();
            CreateMap<SaleOrder, SaleOrderResponseDto>();
            CreateMap<DeviceRegistration, DeviceRegisterationDto>();
        }

    }
}
