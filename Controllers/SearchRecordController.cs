using Search_Filter.DataContext;
using Search_Filter.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using PagedList;

namespace Search_Filter.Controllers
{
    public class SearchRecordController : Controller
    {

        public ActionResult ViewSearchRecord(string search, int? i, string SearchText)
        {
            // Set the SearchText in session
            Session["SearchText"] = SearchText;

            DatabaseHelper db = new DatabaseHelper();

            List<Product> ec = db.GetAllRecords();

            ModelState.Clear();
            return View(ec.ToPagedList(i ?? 1, 4));
        }

        public ActionResult SearchRecord(string search, int? i, string SearchText, string conjunction,
       string criteriaField1, string criteriaValue1Text, string criteriaValue1Date,
       string criteriaField2, string criteriaValue2Text, string criteriaValue2Date,
       string criteriaField3, string criteriaValue3Text, string criteriaValue3Date,
       string criteriaField4, string criteriaValue4Text, string criteriaValue4Date,
       string criteriaField5, string criteriaValue5Text, string criteriaValue5Date,
       string criteriaField6, string criteriaValue6Text, string criteriaValue6Date)
        {
            // Build the filter conditions based on criteria fields and values
            List<string> filterConditions = new List<string>();
            string filterQuery = null;

            AddFilterCondition(criteriaField1, criteriaField1 == "MfgDate" ? criteriaValue1Date : criteriaValue1Text, filterConditions);
            AddFilterCondition(criteriaField2, criteriaField2 == "MfgDate" ? criteriaValue2Date : criteriaValue2Text, filterConditions);
          
            AddFilterCondition(criteriaField3, criteriaField3 == "MfgDate" ? criteriaValue3Date : criteriaValue3Text, filterConditions);
            AddFilterCondition(criteriaField4, criteriaField4 == "MfgDate" ? criteriaValue4Date : criteriaValue4Text, filterConditions);
            AddFilterCondition(criteriaField5, criteriaField5 == "MfgDate" ? criteriaValue5Date : criteriaValue5Text, filterConditions);
            AddFilterCondition(criteriaField6, criteriaField6 == "MfgDate" ? criteriaValue6Date : criteriaValue6Text, filterConditions);

            if (conjunction == "AND")
            {
                 filterQuery = string.Join(" AND ", filterConditions);
            }
            else
            {
                 filterQuery = string.Join(" OR ", filterConditions);
            }

            // Store the selected option in the session
            Session["SelectedOption"] = SearchText;

            DatabaseHelper db = new DatabaseHelper();
            List<Product> ec = db.SearchedRecord(filterQuery);

            return View("ViewSearchRecord", ec.ToPagedList(i ?? 1, 4));
        }

       



        private void AddFilterCondition(string criteriaField, string criteriaValue, List<string> filterConditions)
        {
            if (!string.IsNullOrEmpty(criteriaField) && !string.IsNullOrEmpty(criteriaValue))
            {
                switch (criteriaField)
                {
                    case "ProductId":
                        filterConditions.Add("ProductId = '" + criteriaValue + "'");
                        break;
                    case "ProductName":
                        filterConditions.Add("ProductName LIKE '%" + criteriaValue + "%'");
                        break;
                    case "Size":
                        filterConditions.Add("Size = '" + criteriaValue + "'");
                        break;
                    case "Price":
                        filterConditions.Add("Price = '" + criteriaValue + "'");
                        break;
                    case "MfgDate":
                        filterConditions.Add("MfgDate = '" + criteriaValue + "'");
                        break;
                    case "Category":
                        filterConditions.Add("Category LIKE '%" + criteriaValue + "%'");
                        break;
                    default:
                        // Handle the default case or set a default filter query
                        filterConditions.Add("ProductName LIKE '%" + criteriaValue + "%'"); // Change this to your desired default query
                        break;
                }
            }
        }



    }
}