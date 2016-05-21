using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAQMS.DomainViewModel;
using DAQMS.Service;

namespace DAQMS.Web.Helper
{
    public static class PopulateDropdown
    {
        //Populate Dropdownlist
        public static List<SelectListItem> PopulateDropdownList<T>(this List<T> objectList, string valueField, string textField)
        {
            try
            {
                var selectedList = new SelectList(objectList, valueField, textField);
                List<SelectListItem> items;
                IEnumerable<SelectListItem> listOfItems;
                listOfItems = from obj in selectedList select new SelectListItem { Selected = false, Text = obj.Text, Value = obj.Value };
                items = listOfItems.ToList();

                return items.OrderBy(s => s.Text).OrderByDescending(x => x.Text).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<SelectListItem> PopulateDropdownListByTable<T>(string tableName)
        {
            string valueField="Id";
            string textField = "Name";

             List<CommonViewModel> objectList=new List<CommonViewModel>();
             DropDownService data = new DropDownService();
            try
            {
                objectList = data.GetItemByTableName(tableName);

                var selectedList = new SelectList(objectList, valueField, textField);
                List<SelectListItem> items;
                IEnumerable<SelectListItem> listOfItems;
                listOfItems = from obj in selectedList select new SelectListItem { Selected = false, Text = obj.Text, Value = obj.Value };
                items = listOfItems.ToList();

                return items.OrderBy(s => s.Text).OrderByDescending(x => x.Text).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}