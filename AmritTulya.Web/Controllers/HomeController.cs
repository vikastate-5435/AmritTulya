using AmritTulya.EntityLayer;
using AmritTulya.Web.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace AmritTulya.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _oConfiguration;
        private APICommunicationResponseModel<string> _oApiResponse;
        private APIRepository _oApiRepository;

        string sFilterData = string.Empty, sJson = string.Empty;
        List<Inventory> sResponse = new List<Inventory>();
        string url = string.Empty;
        public static string token = string.Empty;


        public HomeController()
        {
        }
        public HomeController(IConfiguration oConfiguration)
        {
            _oConfiguration = oConfiguration;
        }
        public ActionResult Index()
        {
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

            return View(sResponse);
        }




        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public ActionResult Login()
        { return View(); }

        [HttpPost]
        public ActionResult Login(UsersEntity objuser)
        {
            if (ModelState.IsValid)
            {

                string tokenUrl = "api/account/login";
                try
                {
                    _oApiRepository = new APIRepository(_oConfiguration);

                    AccessTokenModel oResponse = _oApiRepository.GetToken(objuser, tokenUrl);

                    if (!string.IsNullOrEmpty(oResponse.token))
                    {
                        var claims = new List<Claim>
                        {
                        new Claim(ClaimTypes.NameIdentifier, "token"),
                        new Claim(ClaimTypes.Authentication, oResponse.token)
                    };
                        token = oResponse.token;

                        FormsAuthentication.SetAuthCookie(oResponse.Username, true);
                        var authTicket = new FormsAuthenticationTicket(1, oResponse.Username, DateTime.Now, DateTime.Now.AddMinutes(20), false, "Admin");
                        HttpCookie tokenCookie = new HttpCookie(token);
                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                        var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                        HttpContext.Response.Cookies.Add(authCookie);

                        return RedirectToAction("Index", "Inventory", null);

                    }
                    else
                    {
                        ViewBag.errormessage = "Invalid login details";
                        return View("Login");
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            else
            {
                return View("Login");
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}