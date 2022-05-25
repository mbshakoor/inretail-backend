using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.ConstFiles
{
    public class ConstHelper
    {
        public const int ORGANIZATION_CODE_LENGTH = 6;
        public const int BRANCH_CODE_LENGTH = 6;
        public const int ORDER_NO_LENGTH = 10;
        public const int ITEM_CODE_LENGTH = 6;
        public const int RESET_USER_ID = -300;
        public const int APP_ADMIN_USER_ID = -100;
        public const int PASSWORD_CHANGE_USER_ID = -200;
        public const int TYPE_CATEGORY = 1;
        public const int TYPE_PRODUCT = 2;
        public const int PWD_RANDOM_MIN_RANGE = 100000;
        public const int PWD_RANDOM_MAX_RANGE = 999999;
        public const string PWD_STRING = "000000";
        public const int CODE_RANDOM_MIN_RANGE = 100000;
        public const int CODE_RANDOM_MAX_RANGE = 999999;
        public const string CODE_STRING = "000000";

        public const int BRANCH_ALREADY_EXIST = -1;
        public const int INVALID_ORGANIZATION = -1;
        public const int INVALID_BRANCH = -1;
        public const int INVALID_PARENT_CATEGORY = -1;
        public const int USER_ALREADY_EXIST = -1;
        public const int INVALID_USER = -1;
        public const int PWD_MISMATCHED = -1;
        public const int OTHER_ORG_WITH_SAME_NAME = -1;
        public const int OTHER_BRANCH_WITH_SAME_NAME = -1;
        public const int ITEM_ALREADY_EXIST = -1;
        public const int CATEGORY_ALREADY_EXIST = -1;


        public const string DummyCustomerName = "InRetail Customer";
        public const string DummyContactNo = "+923331234567";

        // const for sp names
        public const string spGetAllOrganizations = "spGetAllOrganizations";
        public const string spGetAllBranches = "spGetAllBranches";
        public const string spSearchBranches = "spSearchBranches";
        public const string spGetAllItems = "spGetAllItems";
        public const string spGetItemCount = "spGetItemCount";
        public const string spSearchItems = "spSearchItems";
        public const string spGetAllUsers = "spGetAllUsers";
        public const string spSearchUsers = "spSearchUsers";
        public const string spGetAllCatgories = "spGetAllCatgories";
        public const string spSearchCatgories = "spSearchCatgories";
        public const string spGetAllItemNames = "spGetAllItemNames";
        public const string spSearchItemsByName = "spSearchItemsByName"; 
        public const string spSearchSaleOrders = "spSearchSaleOrders"; 
        public const string spGetSaleOrderSummary = "spGetSaleOrderSummary"; 
        public const string spDeleteSaleOrderDetail = "spDeleteSaleOrderDetail"; 
        public const string spGetSaleOrderDetailBySOId = "spGetSaleOrderDetailBySOId"; 

        // const for sp params names
        public const string spParamOrganizationId = "@OrganizationId";
        public const string spParamBranchId = "@BranchId";
        public const string spParamFromDate = "@FromDate";
        public const string spParamToDate = "@ToDate";
        public const string spParamParentId = "@ParentId";
        public const string spParamContactNo = "@ContactNo";
        public const string spParamUserName = "@Username";
        public const string spParamPrice = "@Price";
        public const string spParamItemName = "@ItemName";
        public const string spParamCategoryName = "@CategoryName";
        public const string spParamCustomerName = "@CustomerName";
        public const string spParamOrderNo = "@OrderNo";
        public const string spParamMinPrice = "@MinPrice";
        public const string spParamMaxPrice = "@MaxPrice";
        public const string spParamSaleOrderId = "@SaleOrderId";
        public const string TokenTag = "refreshToken";
        public const string IpTag = "X-Forwarded-For";
        public const string ADMIN_BRANCH = "Admin Branch";
        public const string ADMIN_USER = "Super Admin";
        public const string ADMIN_USER_ID = "superadmin";

        //Errors
        public const string ERR_INVALID_TOKEN = "Invalid token";
        public const string ERR_EMPTY_TOKEN = "No token is found in request";
        public const string ERR_TOKEN_REQUIRED = "Token is required";
        public const string ERR_TOKEN_NOT_FOUND = "Token not found";
        public const string ERR_TOKEN_REVOKED = "Token revoked";

        public const string chaveAlpor = "chaveAlpor";

        //Import Consts
        public const string CategoryName = "CategoryName";
        public const string ItemName = "ItemName";
        public const int ImportSuccesful = 200;
        public const int ImpInvalidColumns = 421;
        public const int ImpZeroCategory = 422;
        public const int ImpEmptyFile = 423;
        public const int ImportError = 420;


        public static DateTime GetToDate(DateTime? fromDate)
        {
            fromDate ??= DateTime.Now;
            DateTime ToDate = new DateTime(fromDate.Value.Year, fromDate.Value.Month, fromDate.Value.Day, 23, 59, 59);

            return ToDate;
        }

        public static DateTime GetFromDate(DateTime? toDate)
        {
            toDate ??= DateTime.Now;
            DateTime FromDate = new DateTime(toDate.Value.Year, toDate.Value.Month, toDate.Value.Day, 0, 0, 0);

            return FromDate;
        }
    }
}
