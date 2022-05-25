using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InRetailDAL.Dtos;
using InRetailDAL.Models;
using InRetailDAL.Services.IService;
using InRetailDAL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using InRetailDAL.ConstFiles;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.IO;

namespace InRetail.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SaleOrderController : ControllerBase
    {
        private readonly ISaleOrderService _saleOrderService;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        private readonly IConfiguration configuration;
        public SaleOrderController(ISaleOrderService saleOrderService, ICustomerService customerService, IMapper mapper, IConfiguration _configuration)
        {
            _saleOrderService = saleOrderService;
            _customerService = customerService;
            _mapper = mapper;
            configuration = _configuration;
        }

        [HttpGet("getAllSaleOrders")]
        public async Task<SaleOrderModelResponse> GetAllSaleOrders()
        {
            SaleOrderModelResponse response = new SaleOrderModelResponse();
            try
            {
                var result = await _saleOrderService.GetAllSaleOrderAsync();
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_SALE_ORDER_LIST;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                    response.SaleOrderModel = JsonConvert.DeserializeObject<List<SaleOrderModel>>(result);
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpGet("getSaleOrderById")]
        public async Task<SaleOrderUpdateDto> GetSaleOrderById(int Id)
        {
            SaleOrderUpdateDto response = new SaleOrderUpdateDto();
            try
            {
                var saleOrder = await _saleOrderService.GetSaleOrderByIdAsync(Id);
                if (saleOrder == null)
                    response.ErrorMessage = ErrorHelper.NO_SALE_ORDER_FOUND;

                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    SaleOrderUpdateDto order = _mapper.Map<SaleOrderUpdateDto>(saleOrder);
                    var customer = await _customerService.GetCustomerByIdAsync(saleOrder.CustomerId);
                    order.CustomerName = customer.CustomerName;
                    order.ContactNo = customer.ContactNo;
                    order.SaleOrderDetail = await _saleOrderService.GetSaleOrderDetailBySOIdAsync(order.Id);
                    response = order;
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }
                
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpPost("createSaleOrder")]
        public async Task<ActionResult<SaleOrderResponseDto>> CreateSaleOrder( SaleOrderInsertDto saleOrder)
        {
            SaleOrderResponseDto response = new SaleOrderResponseDto();
            try
            {
                SaleOrder order = _mapper.Map<SaleOrder>(saleOrder);
                var customer = await _customerService.GetCustomerId(saleOrder.CustomerName, saleOrder.ContactNo, saleOrder.BranchId);
                order.CustomerId = customer.Id;

                order = await _saleOrderService.AddSaleOrderAsync(order);
                if (order == null)
                    response.ErrorMessage = ErrorHelper.NO_SALE_ORDER_CREATE;
                decimal amount = 0;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    //var detailList = _mapper.Map<List<SaleOrderDetail>>(saleOrder.SaleOrderDetail);
                    foreach (var sdetail in saleOrder.SaleOrderDetail)
                    {
                        var detail = _mapper.Map<SaleOrderDetail>(sdetail);
                        detail.SaleOrderId = order.Id;
                        await _saleOrderService.AddSaleOrderDetailAsync(detail);
                        amount += sdetail.Quantity * sdetail.Price;
                    }

                    response = _mapper.Map<SaleOrderResponseDto>(order);
                    response.Amount = amount;
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpPost("updateSaleOrder")] // date formate : 2020-03-07T14:49:48.549Z
        public async Task<ActionResult<ResponseDto>> UpdateSaleOrder( UpdateSaleOrderDto saleOrder)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                SaleOrder order = _mapper.Map<SaleOrder>(saleOrder);
                var customer = await _customerService.GetCustomerId(saleOrder.CustomerName, saleOrder.ContactNo, saleOrder.BranchId);
                order.CustomerId = customer.Id;

                var result = await _saleOrderService.UpdateSaleOrderAsync(order);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_SALE_ORDER_UPDATE;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    //var orderDetail = _mapper.Map<List<SaleOrderDetail>>(saleOrder.SaleOrderDetail);
                    var orderDetail = new List<SaleOrderDetail>();
                    foreach (var sdetail in saleOrder.SaleOrderDetail)
                    {
                        var detail = _mapper.Map<SaleOrderDetail>(sdetail);
                        orderDetail.Add(detail);
                    }
                    await _saleOrderService.UpateSaleOrderDetail(order.Id, orderDetail);

                    response = _mapper.Map<ResponseDto>(result);
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpGet("getSaleOrderSummaryDateWise")]
        public async Task<SaleOrderSummaryResponse> GetSaleOrderSummaryDateWise(int? OrganizationId, DateTime? FromDate, DateTime? ToDate)
        {
            SaleOrderSummaryResponse response = new SaleOrderSummaryResponse();
            try
            {
                if (FromDate == null || ToDate == null)
                {
                    ToDate = DateTime.Now;
                    FromDate = ToDate.Value.AddDays(-30);
                }
                FromDate = ConstHelper.GetFromDate(FromDate);
                ToDate = ConstHelper.GetToDate(ToDate);

                var result = await _saleOrderService.GetSaleOrderSummary(OrganizationId, FromDate, ToDate);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_SALE_ORDER_SUMMARY_LIST;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                    var summary = JsonConvert.DeserializeObject<SaleOrderSummary>(result);
                    response.SaleOrderSummary = new List<SaleOrderSummary>();
                    response.SaleOrderSummary.Add(summary);

                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpGet("getSaleOrderSummary")]
        public async Task<SaleOrderSummaryResponse> GetSaleOrderSummary(int? OrganizationId)
        {
            SaleOrderSummaryResponse response = new SaleOrderSummaryResponse();
            try
            {
                DateTime FromDate = DateTime.Now;
                DateTime ToDate = DateTime.Now;

                FromDate = ToDate.AddDays(-7);
                var result7Days = await _saleOrderService.GetSaleOrderSummary(OrganizationId, FromDate, ToDate);
                if (result7Days == null)
                    response.ErrorMessage = ErrorHelper.NO_SALE_ORDER_SUMMARY_LIST_7DAYS;

                FromDate = ToDate.AddDays(-30);
                var result30Days = await _saleOrderService.GetSaleOrderSummary(OrganizationId, FromDate, ToDate);
                if (result30Days == null)
                    response.ErrorMessage = ErrorHelper.NO_SALE_ORDER_SUMMARY_LIST_30DAYS;

                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                    var summary7Days = JsonConvert.DeserializeObject<SaleOrderSummary>(result7Days);
                    var summary30Days = JsonConvert.DeserializeObject<SaleOrderSummary>(result30Days);

                    response.SaleOrderSummary = new List<SaleOrderSummary>();
                    response.SaleOrderSummary.Add(summary7Days);
                    response.SaleOrderSummary.Add(summary30Days);
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpGet("searchSaleOrders")]
        public async Task<SaleOrderModelResponse> SearchSaleOrders(int? BranchId, string? ContactNo, string? CustomerName, DateTime? FromDate,
            DateTime? ToDate, string? OrderNo, string? ItemName, decimal? MinPrice, decimal? MaxPrice)
        {
            SaleOrderModelResponse response = new SaleOrderModelResponse();
            try
            {
                FromDate = ConstHelper.GetFromDate(FromDate);
                ToDate = ConstHelper.GetToDate(ToDate);
                var result = await _saleOrderService.SearchSaleOrderAsync(BranchId, ContactNo, CustomerName, FromDate,
                ToDate, OrderNo, ItemName, MinPrice, MaxPrice);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_SALE_ORDER_LIST;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                    response.SaleOrderModel = JsonConvert.DeserializeObject<List<SaleOrderModel>>(result);
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpGet("printSaleOrder")]
        public async Task<string> PrintSaleOrder(int SaleOrderId)
        {
            string URl = "";
            try
            {
                string printingAppUrl = configuration.GetSection("InRetailConfig").GetSection("PrintingAppURL").Value;
                printingAppUrl += SaleOrderId;
                WebRequest request = HttpWebRequest.Create(printingAppUrl);
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                URl = reader.ReadToEnd();
            }
            catch (Exception exc)
            {
                URl = ErrorHelper.ExceptionError(exc);
            }
            return URl;
        }
    }
}
