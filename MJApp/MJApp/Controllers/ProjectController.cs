using MJApp.Context;
using MJApp.MJLib;
using MJApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace MJApp.Controllers
{
    public class ProjectController : Controller
    {
        private MYDBContext db = new MYDBContext();
        // GET: Project
        public ActionResult Index()
        {
            int starting = 0;
            if(Request.Form["starting"]!=null)
            {
                starting = Convert.ToInt32(Request.Form["starting"]);
            }
            string strpost ="&ajax=1";
            //var data = db.Projects.ToList();
            //var query3 = (from _customerMaster in _customerMasterList select _customerMaster).Skip(start).Take(offset).ToList();
            var q_total = (from t in db.Projects select t);
            int numrows = q_total.Count();
            //var query = from c in db.Projects.Take(2).Skip(2) orderby c.ProjectId descending select c;
            var query = (from c in db.Projects orderby c.ProjectId descending select c).AsEnumerable().Skip(starting).Take(2);
            
            //int count = data.Count;
            string links = Pagination.paginate(numrows,starting,3,"","page",strpost);
           // ViewBag.link = links.Replace("\"","");
            //string test = "<ul><li>Welcome</li><li>TES</li></ul>";
            ViewBag.link = links;
            ViewBag.query = query;
            if (Request.IsAjaxRequest())
                return PartialView("Index2");
            return View();
        }

        public ActionResult get_search()
        {
            int Pro_id = 0;
            if(Request.Form["project_id"]!=null)
            {
                 Pro_id = Convert.ToInt32(Request.Form["project_id"]);
            }
            int starting = 0;
            if (Request.Form["starting"] != null)
            {
                starting = Convert.ToInt32(Request.Form["starting"]);
            }
            string strpost = "&ajax=1";
            strpost += "&project_id=" + Pro_id;
         
            var q_total = (from c in db.Projects select c);
            int numrows = q_total.Count();
            //var query = from c in db.Projects.Take(2).Skip(2) orderby c.ProjectId descending select c;
            string pname = Request.Form["project_name"];
            string pdate = Request.Form["project_date"];
           // var query = (from c in db.Projects  orderby c.ProjectId descending select c).AsEnumerable().Skip(starting).Take(3);
            /*
            if(Request.Form["project_name"]!=null)
            {
                query = query.Where(c => c.ProjectName.Contains(Request.Form["project_name"].Trim()));
                 numrows = query.Count();
            }
             */
            var query = (from c in db.Projects where((string.IsNullOrEmpty(pname)? true : c.ProjectName.Contains(pname))
                             && (string.IsNullOrEmpty(pdate) ? true:c.ProjectDate.Contains(pdate))) orderby c.ProjectId descending select c).AsEnumerable().Skip(starting).Take(3);
            
            //int count = data.Count;
            string links = Pagination.paginate(numrows, starting, 3, "", "page", strpost);
            // ViewBag.link = links.Replace("\"","");
            //string test = "<ul><li>Welcome</li><li>TES</li></ul>";
            ViewBag.link = links;
            ViewBag.query = query;
            if (Request.IsAjaxRequest())
                return PartialView("get_search2");
            return View();
        }

        // GET: Project/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var mydata = db.Projects.Find(id);
            if (mydata == null)
            {
                return HttpNotFound();
            }
  
            var prov_id =  from o in db.ProRel where o.ProjectId == id select o.ProvinceIds;

            List<int> mList = new List<int>();
            foreach (var p_id in prov_id)
            {
                //int[] PIds = new int[] {p_id};
                mList.Add(p_id);
            }

            var Prov = from c in db.Provinces where (mList.Contains(c.ID)) select c;
            ViewBag.prov = Prov;
            return View(mydata);
        }

        // GET: Project/Create
        public ActionResult Create()
        {

            IEnumerable<SelectListItem> items = db.Provinces.Select(c => new SelectListItem { Value = c.ID.ToString(), Text = c.Name });
            ViewBag.ProvinceId = items;
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(Project project, int[] ProvinceId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Projects.Add(project);
                    db.SaveChanges();

                    int Project_id = project.ProjectId;
                    var PBObject = new ProjectProvRel();
                    //---adding the project_id to this object---//
                    PBObject.ProjectId = Project_id;
                    // TODO: Add insert logic here
                    foreach (int P_id in ProvinceId)
                    {
                        PBObject.ProvinceIds = P_id;
                        db.ProRel.Add(PBObject);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(project);
            }
            catch
            {
                return View();
            }
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var edata = db.Projects.Find(id);
            //check if data
            if (edata == null)
                return HttpNotFound();

            //IEnumerable<SelectListItem> items = db.Provinces.Select(c => new SelectListItem { Value = c.ID.ToString(), Text = c.Name });
            //ViewBag.ProvinceId = items;

            //---Get all provice-----
            var Prov = from c in db.Provinces select c;
            //------get province ids---------------
            var prov_id = from o in db.ProRel where o.ProjectId == id select o.ProvinceIds;
            List<int> mid_list = new List<int>();
            foreach (var p_ids in prov_id)
            {
                mid_list.Add(p_ids);
            }
            /*
            var option_vals = "";
            var selected = string.Empty;
            foreach (var p_itmes in Prov)
            {
                if (mid_list.Contains(p_itmes.ID))
                {
                    selected = "selected='selected'";
                }
                else
                {
                    selected = "";
                }
                option_vals += "<option "+selected+" value=\""+p_itmes.ID+"\">"+p_itmes.Name+"</option>";
            }
            string test = option_vals.ToString();
            string test2 = test.Replace("\"", "");
            ViewBag.options = test2;
             */
            var options = new List<SelectListItem>();
            foreach (var p_itmes in Prov)
            {
                var item = new SelectListItem();
                if (mid_list.Contains(p_itmes.ID))
                    item.Selected = true;
                item.Value = p_itmes.ID.ToString();
                item.Text = p_itmes.Name;
                options.Add(item);
            }
            ViewBag.options = options;
            return View(edata);
          
        }

        // POST: Project/Edit/5
        [HttpPost]
        public ActionResult Edit(Project project, int[] prov)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(project).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    //-------update project province-------//
                    int Project_id = project.ProjectId;
                    db.ProRel.RemoveRange(db.ProRel.Where(c => c.ProjectId == Project_id));
                    var PBObject = new ProjectProvRel();
                    
                    //---adding the project_id to this object---//
                    PBObject.ProjectId = Project_id;
                    // TODO: Add insert logic here
                    foreach (int P_id in prov)
                    {
                        PBObject.ProvinceIds = P_id;
                        db.ProRel.Add(PBObject);
                        db.SaveChanges();
                    }

                    // TODO: Add update logic here

                    return RedirectToAction("Index");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Project/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
