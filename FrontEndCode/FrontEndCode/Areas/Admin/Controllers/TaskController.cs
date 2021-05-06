using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ShopOnClick.Areas.Admin.Controllers
{
    [Area("Admin")]
   
    public class TaskController : Controller
    {
        private readonly ILogger<TaskController> _logger;
        private readonly string BaseUrl = "http://localhost:61955/api/TaskByUsers/";


        private dynamic expObj;
        public TaskController(ILogger<TaskController> logger)
        {
            _logger = logger;
         

            expObj = new ExpandoObject();
        }


        public IActionResult Index()
        {
            return View(expObj);
        }

        public async Task <IActionResult> _list(int id)
        {
            var client = new HttpClient();

            var token = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await client.GetAsync(BaseUrl);

            var stringJson = await response.Content.ReadAsStringAsync();
            var listTask = JsonConvert.DeserializeObject<List<ShopOnClick.Models.Task>>(stringJson);
            expObj.listTask = listTask;

            return PartialView(expObj);
        }


        public async Task<IActionResult> edit(int id)
        {
            var client = new HttpClient();
            ShopOnClick.Models.Task IdDetail = null;
            if (id!=0)
            {

                var token = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var response = await client.GetAsync(BaseUrl+""+id);
                var stringJson = await response.Content.ReadAsStringAsync();
                 IdDetail = JsonConvert.DeserializeObject<ShopOnClick.Models.Task>(stringJson);
                expObj.IdDetail = IdDetail;


            }
            expObj.IdDetail = IdDetail;
            return View(expObj);
        }


        [HttpPost]
        public async Task<ActionResult> save(ShopOnClick.Models.Task postedData)
        {
            var client = new HttpClient();
            var token = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var data = JsonConvert.SerializeObject(postedData);
            var stringContent = new StringContent(data, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync(BaseUrl, stringContent);

            if(response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                return Json(new { result = "Failed" });
            }

            var stringJson = await response.Content.ReadAsStringAsync();
            var listTask = JsonConvert.DeserializeObject<ShopOnClick.Models.Task>(stringJson);


            return Json(new { result = "" });
        }


        [HttpPost]
        public async Task<ActionResult> update(ShopOnClick.Models.Task postedData)
        {
            var client = new HttpClient();
            var token = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var data = JsonConvert.SerializeObject(postedData);
            var stringContent = new StringContent(data, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PutAsync(BaseUrl+""+postedData.Id, stringContent);
           
            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                return Json(new { result = "Failed" });
            }

            var stringJson = await response.Content.ReadAsStringAsync();
            var listTask = JsonConvert.DeserializeObject<ShopOnClick.Models.Task>(stringJson);

            return Json(new { result = "" });
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var client = new HttpClient();
            var token = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await client.DeleteAsync(BaseUrl + "" + id);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return Json(new { result = "Not Found" });
            }

            return Json(new { result = "" });
        }

    }
}
