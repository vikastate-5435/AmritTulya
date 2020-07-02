using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using AmritTulya.EntityLayer;
using AmritTulya.Web.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AmritTulya.Web.Controllers
{
    public class InventoryController : Controller
    {

        private readonly IConfiguration _oConfiguration;
        private APICommunicationResponseModel<string> _oApiResponse;
        private APIRepository _oApiRepository;

        string sFilterData = string.Empty, sJson = string.Empty;
        List<Inventory> sResponse = new List<Inventory>();
        string url = string.Empty;
        private string _sToken = string.Empty;
        public InventoryController()
        {

        }
        public InventoryController(IConfiguration configuration)
        {
            _sToken = HomeController.token;
            _oConfiguration = configuration;
        }

        // GET: Inventory
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetValues(string sidx, string sord, int page, int rows) //Gets the todo Lists.  
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            url = "api/Inventories/GetProducts";
            var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            DataTableAjaxPostModel dModel = new DataTableAjaxPostModel();
            sJson = JsonConvert.SerializeObject(dModel, Formatting.Indented).ToString();
            byte[] bContent = Encoding.ASCII.GetBytes(sJson);
            var sBytes = new ByteArrayContent(bContent);

            _oApiRepository = new APIRepository(_oConfiguration);
            _oApiResponse = new APICommunicationResponseModel<string>();
            _oApiResponse = _oApiRepository.APICommunication(url, System.Net.Http.HttpMethod.Get, sBytes, HomeController.token);

            if (_oApiResponse.statusCode == HttpStatusCode.OK)
            {
                sFilterData = _oApiResponse.data;
                sResponse = JsonConvert.DeserializeObject<List<Inventory>>(sFilterData);
            }

            var Results = sResponse.Select(
            a => new
            {
                a.Id,
                a.Name,
                a.Description,
                a.Price,
                a.InventoryImage

            });
            int totalRecords = Results.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sord.ToUpper() == "DESC")
            {
                Results = Results.OrderByDescending(s => s.Id);
                Results = Results.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                Results = Results.OrderBy(s => s.Id);
                Results = Results.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = Results
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        // TODO:insert a new row to the grid logic here  

        [HttpPost]
        public string Create([Bind(Exclude = "Id")] Inventory obj, HttpPostedFileBase InventoryImage)
        {
            url = "api/Inventories/AddProduct";
            string msg;
            try
            {
                if (ModelState.IsValid)
                {

                    _oApiRepository = new APIRepository(_oConfiguration);
                    _oApiResponse = new APICommunicationResponseModel<string>();

                    var json = JsonConvert.SerializeObject(obj, Formatting.Indented).ToString();

                    byte[] content = Encoding.ASCII.GetBytes(json);
                    var bytes = new ByteArrayContent(content);
                    _oApiResponse = _oApiRepository.APICommunication(url, HttpMethod.Post, bytes, _sToken);

                    msg = "Saved Successfully";
                    //db.Inventories.Add(obj);
                    //db.SaveChanges();
                    //msg = "Saved Successfully";
                }
                else
                {
                    msg = "Validation data not successful";
                }
            }
            catch (Exception ex)
            {
                msg = "Error occured:" + ex.Message;
            }
            return msg;
        }
        public string Edit(Inventory obj)
        {
            url = "api/Inventories/UpdateProduct/";
            string msg;
            try
            {
                if (ModelState.IsValid)
                {

                    _oApiRepository = new APIRepository(_oConfiguration);
                    _oApiResponse = new APICommunicationResponseModel<string>();
                    var json = JsonConvert.SerializeObject(obj, Formatting.Indented).ToString();

                    byte[] content = Encoding.ASCII.GetBytes(json);
                    var bytes = new ByteArrayContent(content);

                    _oApiResponse = _oApiRepository.APICommunication(string.Format(url), HttpMethod.Put, bytes, HomeController.token);

                    msg = "Saved Successfully";
                    
                    //db.Entry(obj).State = EntityState.Modified;
                    //db.SaveChanges();
                    //msg = "Saved Successfully";
                }
                else
                {
                    msg = "Validation data not successfull";
                }
            }
            catch (Exception ex)
            {
                msg = "Error occured:" + ex.Message;
            }
            return msg;
        }

        #region Upload Images
        [HttpPost]
        public JsonResult UploadImage(int id, HttpPostedFileBase InventoryImage)
        {
            var _Id = Request["Id"];
            var _InventoryImage = Request["InventoryImage"];
            string directory = "~/Images/";

            if (InventoryImage != null && InventoryImage.ContentLength > 0)
            {
                var fileName = System.IO.Path.GetFileName(InventoryImage.FileName);
                InventoryImage.SaveAs(Server.MapPath(System.IO.Path.Combine(directory, fileName)));
                //ProductImage.SaveAs(Path.Combine(directory, fileName));
            }


            string _imgname = string.Empty;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["MyImages"];
                if (pic.ContentLength > 0)
                {
                    var fileName = System.IO.Path.GetFileName(pic.FileName);
                    var _ext = System.IO.Path.GetExtension(pic.FileName);

                    _imgname = Guid.NewGuid().ToString();
                    var _comPath = Server.MapPath("/Images") + _imgname + _ext;
                    _imgname = "Inv" + _imgname + _ext;

                    ViewBag.Msg = _comPath;
                    var path = _comPath;

                    // Saving Image in Original Mode
                    pic.SaveAs(path);

                    // resizing image
                    MemoryStream ms = new MemoryStream();
                    WebImage img = new WebImage(_comPath);

                    if (img.Width > 200)
                        img.Resize(200, 200);
                    img.Save(_comPath);
                    // end resize
                }
            }

            return Json(new { isUploaded = true, message = "Uploaded Successfully" }, "text/html");
        }

        #endregion
        public string Delete(int Id)
        {
            string url = "api/Inventories/DeleteProduct/";

            //Inventory list = db.Inventories.Find(Id);
            //db.Inventories.Remove(list);
            //db.SaveChanges();
            //return "Deleted successfully";

            try
            {
                _oApiRepository = new APIRepository(_oConfiguration);
                _oApiResponse = new APICommunicationResponseModel<string>();

                _oApiResponse = _oApiRepository.APICommunication(string.Format(url + Id), HttpMethod.Delete, null, _sToken);
                    return "Deleted successfully";
            }
            catch (Exception e)
            {
                return "error occured" + e.Message;
            }

        }
    }
}