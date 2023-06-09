﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SS.Entity.Context.Models;
using SS.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;
using SS.Service.DTO;
using Microsoft.AspNetCore.Authorization;

namespace SSWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ProductSellController : ControllerBase
    {
        private readonly IProductSellRepository _productSellRepo;

        public ProductSellController(IProductSellRepository productSellRepo)
        {
            _productSellRepo = productSellRepo;
        }

        [Authorize("CSUser")]
        [HttpPost]
        [Route("billCreate")]
        public IActionResult ProductBillCreate(BillDTO bill)
        {
            bool modelInvalid = false;
            try
            {
                if (bill.Payment.PaymentType == 1)
                {
                    // cash
                    if (bill.Payment.AmountPaid <= 0)
                    {
                        modelInvalid = true;
                        ModelState.AddModelError("Amount Paid", "is Required !");
                        return BadRequest(ModelState);
                    }
                    bill = _productSellRepo.ProductBillCreate(bill);
                    return Ok(bill);
                }
                else
                {
                    // cc
                    if (bill.Payment.CardNumber == null)
                    {
                        modelInvalid = true;
                        ModelState.AddModelError("Card Number", "is Required !");
                        // return BadRequest(ModelState);
                    }
                    if (bill.Payment.CardCVV <= 0)
                    {
                        modelInvalid = true;
                        ModelState.AddModelError("Card CVV", "is Required !");
                        // return BadRequest(ModelState);
                    }
                    if (bill.Payment.ValidMonth <= 0)
                    {
                        modelInvalid = true;
                        ModelState.AddModelError("Month", "is Required !");
                        // return BadRequest(ModelState);
                    }
                    if (bill.Payment.ValidYear <= 0)
                    {
                        modelInvalid = true;
                        ModelState.AddModelError("Year", "is Required !");
                        // return BadRequest(ModelState);
                    }
                    if (!modelInvalid)
                    {
                        bill = _productSellRepo.ProductBillCreate(bill);
                        return Ok(bill);
                    }
                    else
                    {
                        return BadRequest(ModelState);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
