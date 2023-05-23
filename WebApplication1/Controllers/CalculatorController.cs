using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CalculatorController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            List<string> history = new List<string>();

            if (Request.QueryString["history"] != null)
            {
                history = Request.QueryString["history"].Split(',').ToList();
            }

            ViewBag.History = history;

            return View();
        }

        [HttpPost]
        public ActionResult Calculate(CalculatorModel model, string action)
        {
            bool status = false;

            if (action == "calculate")
            {
                if (ModelState.IsValid)
                {
                    switch (model.Operation)
                    {
                        case "+":
                            model.Result = model.FstNumber + model.SndNumber;
                            break;
                        case "-":
                            model.Result = model.FstNumber - model.SndNumber;
                            break;
                        case "/":
                            if (model.SndNumber == 0)
                            {
                                ViewBag.Message = "Нельзя делить на 0!";
                                status = true;
                            }
                            else
                            {
                                model.Result = model.FstNumber / model.SndNumber;
                            }
                            break;
                        case "*":
                            model.Result = model.FstNumber * model.SndNumber;
                            break;
                    }

                    if (status == false)
                    {
                        ViewBag.Message = "Вычисление произведено успешно!";
                    }

                    // Создаем строку истории
                    string historyItem = $"{model.FstNumber},{model.Operation},{model.SndNumber},{model.Result}";

                    // Получаем текущую историю из QueryString
                    string currentHistory = Request.QueryString["history"];

                    // Объединяем текущую историю с новым элементом истории
                    string updatedHistory = string.IsNullOrEmpty(currentHistory) ? historyItem : currentHistory + "," + historyItem;

                    // Передаем историю через QueryString
                    return RedirectToAction("Index", new { history = updatedHistory });
                }
                else
                {
                    // Получаем текущую историю из QueryString
                    string currentHistory = Request.QueryString["history"];

                    // Создаем строку истории
                    string historyItem = $"{model.FstNumber},{model.Operation},{model.SndNumber},{model.Result}";

                    // Объединяем текущую историю с новым элементом истории
                    string updatedHistory = string.IsNullOrEmpty(currentHistory)
                        ? historyItem
                        : currentHistory + "," + historyItem;

                    // Получаем текущий URL
                    string currentUrl = Request.Url.ToString();

                    // Определяем позицию символа '?' в URL
                    int queryStringIndex = currentUrl.LastIndexOf('?');

                    // Обновляем URL с новым QueryString
                    string updatedUrl = queryStringIndex != -1
                        ? currentUrl.Substring(0, queryStringIndex + 1) + "history=" + HttpUtility.UrlEncode(updatedHistory)
                        : currentUrl + "?history=" + HttpUtility.UrlEncode(updatedHistory);

                    // Перенаправляем на обновленный URL
                    return Redirect(updatedUrl);
                }

                
            }
            else if (action == "clear")
            {
                // Очищаем историю
                return RedirectToAction("Index", new { history = "" });
            }
            else
            {
                return View("Index", model);
            }
        }

        [HttpGet]
        public ActionResult Result()
        {
            List<string> history = new List<string>();

            if (Request.QueryString["history"] != null)
            {
                history = Request.QueryString["history"].Split(',').ToList();
            }

            return View(history);
        }
    }
}