using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using Domain;

namespace WebAppMVC.Controllers
{
    public class NhanSuController : Controller
    {

        //
        // GET: /NhanSu/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult LoadData()
        {
            Employee_basic objSearch = new Employee_basic();
            objSearch.ORG_ID = 1;
            List<Employee_basic> model =
                OracleHelper.ExcuteSelectMultiObject<Employee_basic>(PKG_LOAD_DATA.NAME,
                    PKG_LOAD_DATA.GET_EMPLOYEE_BY_ORG, objSearch);
            IQueryable<Employee_basic> query = model.AsQueryable();
            return Json(new
            {
                data = query,
                status = true

            }, JsonRequestBehavior.AllowGet);
        }
    }
}