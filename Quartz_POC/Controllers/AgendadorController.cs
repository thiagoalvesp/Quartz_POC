using Quartz;
using Quartz.Impl;
using Quartz_POC.Models;
using Quartz_POC.Service;
using System;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Quartz_POC.Controllers
{
    public class AgendadorController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgendarTarefa(Agendar model, string ButtonType)
        {
            if(ButtonType == "Agendar")
                Agendador.RunJob(model.Texto).GetAwaiter().GetResult();

            if(ButtonType == "Iniciar")
                Agendador.Start().GetAwaiter().GetResult();

            if (ButtonType == "Parar")
                Agendador.Stop().GetAwaiter().GetResult();

            return View("Index", model);

        }


    }
}
