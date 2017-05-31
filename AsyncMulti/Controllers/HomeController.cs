using AsyncMulti.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AsyncMulti.Controllers
{
	public class Result {
		public List<Staging> Staging { get; set; }
		public List<StagingOne> StagingOne { get; set; }

	}


    public class HomeController : Controller
    {
        public ActionResult Index()
        {

			   return View();
        }


		  [HttpGet]
		  public async Task<JsonResult> GetData() {

			  //var tasks = new Task[2]{
			  //	new QueryService<Staging>().GetResults(),
			  //	new QueryService<StagingOne>().GetResults()
			  //};

			  //Task.WaitAll(tasks);

			  //List<Staging> result1 = await tasks[0];

			  var result =  new QueryService<Staging>().GetResults();
			  var result1 =  new QueryService<StagingOne>().GetResults();

			  await Task.WhenAll(result, result1);

			  var ret = new Result() {
				  Staging = result.Result,
				  StagingOne = result1.Result
			  };

			  return Json(ret, JsonRequestBehavior.AllowGet);

		  }



    }
}
