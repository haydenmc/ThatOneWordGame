﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ThatOneWordGame.Controllers
{
	public class GameController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}