using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET_FinalTermExam.Controllers
{
    public class CustomersController : Controller
    {
        private List<SelectListItem> returnContactTitleSelect()
        {
            List<SelectListItem> contactTitleSelect = new List<SelectListItem>();
            List<Models.Cust> contactTitleData = Models.FinalCustomers.GetContactTitle();
            contactTitleSelect.Add(new SelectListItem()
            {
                Text = "",
                Value = "",
                Selected = true
            });
            foreach (var item in contactTitleData)
            {
                contactTitleSelect.Add(new SelectListItem()
                {
                    Text = item.ContactTitle,
                    Value = item.ContactTitle
                });
            }
            return contactTitleSelect;
        }
        // GET: Customers
        [HttpGet()]
        public ActionResult Index()
        {
            Models.Cust cust = new Models.Cust();

            ViewBag.contactTitleSelect = returnContactTitleSelect();

            Models.FinalCustomers FinalCustomers = new Models.FinalCustomers();
            List<Models.Cust> searchCust = FinalCustomers.SearchCustomers(cust);
            ViewBag.searchResult = searchCust;
            return View();
        }
        [HttpPost()]
        public ActionResult Index(Models.Cust cust)
        {
            ViewBag.contactTitleSelect = returnContactTitleSelect();

            Models.FinalCustomers FinalCustomers = new Models.FinalCustomers();
            List<Models.Cust> searchCust = FinalCustomers.SearchCustomers(cust);
            ViewBag.searchResult = searchCust;
            return View();
        }

        public ActionResult NewCustomers()
        {
            ViewBag.contactTitleSelect = returnContactTitleSelect();
            return View();
        }

        public ActionResult NewCustomersFinish(Models.Cust cust)
        {
            Models.FinalCustomers.NewCustomers(cust);
            return View("Index");
        }
    }
    
}