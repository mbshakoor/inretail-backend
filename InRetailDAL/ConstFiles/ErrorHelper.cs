using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.ConstFiles
{
    public class ErrorHelper
    {
        public const string SUCCESS = "SUCCESS";
        public const string FATAL_ERROR = "Some Fatal Error Occured, Details are below";

        public const string NO_ORGANIZATION_LIST = "No organization list is found";
        public const string NO_ORGANIZATION_FOUND = "No organization is found";
        public const string NO_ORGANIZATION_CREATE = "Unable to create organization";
        public const string ORGANIZATION_ALREADY_EXIST = "Orgnization with the name is already exist";
        public const string NO_ORGANIZATION_UPDATE = "Unable to update organization";

        public const string NO_BRANCH_LIST = "No branch list is found";
        public const string NO_BRANCH_FOUND = "No branch is found";
        public const string NO_BRANCH_CREATE = "Unable to create branch";
        public const string NO_BRANCH_UPDATE = "Unable to update branch";
        public const string BRANCH_ALREADY_EXIST = "Branch with the name is already exist";

        public const string NO_USER_LIST = "No user list is found";
        public const string NO_USER_LOGIN = "User name or password is invalid";
        public const string INCORRECT_PASSWORD = "Password is incorrect";
        public const string NO_USER_FOUND = "No user is found";
        public const string NO_USER_CREATE = "Unable to create user";
        public const string NO_USER_UPDATE = "Unable to update user";
        public const string NO_REGISTER_USER_DEVICE = "Unable to refister user device";
        public const string NO_PWD_RESET = "Unable to reset user password";
        public const string NO_PWD_CHANGE = "Unable to change user password";

        public const string NO_ITEM_LIST = "No item list is found";
        public const string NO_ITEM_COUNT = "No item count is found";
        public const string NO_ITEM_NAME_LIST = "No item name list is found";
        public const string NO_ITEM_FOUND = "No item is found";
        public const string NO_ITEM_CREATE = "Unable to create item";
        public const string NO_ITEM_UPDATE = "Unable to update item";

        public const string NO_CATEGORY_LIST = "No category list is found";
        public const string NO_CATEGORY_NAME_LIST = "No category name list is found";
        public const string NO_CATEGORY_FOUND = "No category is found";
        public const string NO_CATEGORY_CREATE = "Unable to create category";
        public const string NO_CATEGORY_UPDATE = "Unable to update category";

        public const string NO_SALE_ORDER_LIST = "No sale order list is found";
        public const string NO_SALE_ORDER_SUMMARY_LIST_30DAYS = "No sale order 30 days summary is found";
        public const string NO_SALE_ORDER_SUMMARY_LIST_7DAYS = "No sale order 7 days summary is found";
        public const string NO_SALE_ORDER_SUMMARY_LIST = "No sale order summary is found";
        public const string NO_SALE_ORDER_FOUND = "No sale order is found";
        public const string NO_SALE_ORDER_CREATE = "Unable to create sale order";
        public const string NO_SALE_ORDER_UPDATE = "Unable to update sale order";
        public const string ERROR_INVALID_ORGANIZATION = "Organization is invalid";
        public const string ERROR_INVALID_BRANCH = "Branch is invalid";
        public const string ERROR_INVALID_PARENT_CATEGORY = "Parent category is invalid";
        public const string USER_ALREADY_EXIST = "User with the login id is already exist";
        public const string ITEM_ALREADY_EXIST = "Item with the name is already exist";
        public const string CATEGORY_ALREADY_EXIST = "Category with the name is already exist";
        public const string ERROR_INVALID_CREDENTIALS = "Username or password is incorrect, Login Failed!";
        public const string IMEI_NOT_REGISTERED = "IMEI is not registered";
        public const string IMEI_INVALID = "IMEI is empty or invalid length";
        public const string ERROR_PWD_MISMATCHED = "Old password is not matched, Unable to change password";

        //Item Errors
        public const string IMP_INVALID_COLS = "Columns name specified in the file are invalid, Correct names : CategoryName, ItemName";
        public const string IMP_NO_CATEGORY = "No Category is added in file, please add at least one category";
        public const string IMP_GENERIC_ERROR = "Unable to import the file, please try again later";
        public const string IMP_EMPTY_FILE = "Uploaded file is empty, please uploade the file with the items data";

        public static string ExceptionError(Exception exception)
        {
            return FATAL_ERROR + "\n"
               + "Exception Message : " + exception.Message + "\n"
               + "Exception StackTrace : " + exception.StackTrace;
        }


    }
}
