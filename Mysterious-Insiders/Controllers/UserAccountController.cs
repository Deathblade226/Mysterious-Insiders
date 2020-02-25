using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mysterious_Insiders.Models;
using Mysterious_Insiders.Services;

namespace Mysterious_Insiders.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly UserAccountService accountService;

        public UserAccountController(UserAccountService service)
        {
            accountService = service;
        }

        [HttpGet]
        public ActionResult<List<UserAccount>> Get()
        {
            return accountService.Get();
        }

        [HttpGet("{id:length(24)}", Name = "GetAccount")]
        public ActionResult<UserAccount> Get(long id)
        {
            var ua = accountService.Get(id);

            if (ua == null)
            {
                return NotFound();
            }

            return ua;
        }

        //[HttpPost]
        public ActionResult<UserAccount> Create(UserAccount account)
        {
            accountService.Create(account);

            return CreatedAtRoute("GetAccount", new { id = account.UserAccountID.ToString() }, account);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(long id, UserAccount sheetIn)
        {
            var sheet = accountService.Get(id);

            if (sheet == null)
            {
                return NotFound();
            }

            accountService.Update(id, sheetIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(long id)
        {
            var ua = accountService.Get(id);

            if (ua == null)
            {
                return NotFound();
            }

            accountService.Remove(ua.UserAccountID);

            return NoContent();
        }


        //[HttpGet("testing")]
        //public IActionResult testing()
        //{
        //    return Ok(new { status = "success" });
        //}


        //[HttpPost("samplepostdata")]
        //public ActionResult samplepostdata(int x)
        //{
        //    //Console.WriteLine(value);
        //    return Ok(new { status = "0" });
        //}


        [HttpGet("createdatabase")]
        public IActionResult createdatabase()
        {
            //_dbc.Database.EnsureDeleted();
            accountService.CreateDatabase();

            return Ok(new { status = "db created" });
        }


        //[HttpPost("createuseraccount")]
        //public ActionResult createfoodorder([FromForm, Required, RegularExpression("^[A-Za-z1-9]+$")] string userName, [FromForm, Required, RegularExpression("^[A-Za-z1-9]+$")] string password)
        //{

        //    UserAccount ua = new UserAccount { UserName = userName, Password = password };

        //    _dbc.UserAccounts.Add(ua);
        //    int i = _dbc.SaveChanges();

        //    return Ok(new { status = "success", userName = ua.UserName });
        //}


        //[HttpPost("getorderbyid")]
        //public ActionResult getorderbyid([FromForm, Required, RegularExpression("^[A-Za-z1-9]+$")] string userName)
        //{
        //    var q = from _ua in _dbc.UserAccounts where _ua.UserName == userName select _ua;
        //    UserAccount ua = q.FirstOrDefault<UserAccount>();
        //    if (ua != null)
        //        return Ok(new { status = "success", userName = ua.UserName });
        //    else
        //        return Ok(new { status = "failure" });
        //}


        //[HttpPost("getorderbyitem")]
        //public ActionResult getorderbyitem([FromForm, Required] String item)
        //{
        //    var q = from _fo in _dbc.FoodOrders where _fo.Item == item select _fo;
        //    FoodOrder fo = q.FirstOrDefault<FoodOrder>();
        //    if (fo != null)
        //        return Ok(new { status = "success", id = fo.FoodOrderId, item = fo.Item, quantity = fo.Quantity, ordercomments = fo.OrderComments, price = fo.Price, totalprice = fo.TotalPrice, createdate = fo.CreateDate });
        //    else
        //        return Ok(new { status = "failure" });
        //}


        //[HttpGet("getallorders")]
        //public IQueryable<UserAccount> getallorders()
        //{
        //    var uas = from _ua in _dbc.UserAccounts select _ua;

        //    return uas;
        //}
    }
}