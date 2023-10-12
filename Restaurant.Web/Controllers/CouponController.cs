﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Web.Models;
using Restaurant.Web.Service.IService;

namespace Restaurant.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto?> couponList = new();
            var response = await _couponService.GetAllCouponsAsync();

            if (response != null && response.IsSuccess)
            {
                couponList = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(couponList);
        }

        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _couponService.CreateCouponsAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Coupon created successfully";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
			var response = await _couponService.GetCouponByIdAsync(couponId);

			if (response != null && response.IsSuccess)
			{
				CouponDto? model = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
				return View(model);
			}
            else
            {
                TempData["error"] = response?.Message;
            }
			return NotFound();
        }

        [HttpPost]
		public async Task<IActionResult> CouponDelete(CouponDto couponDto)
		{
            var response = await _couponService.DeleteCouponsAsync(couponDto.CouponId);

			if (response != null && response.IsSuccess)
			{
                TempData["success"] = "Coupon deleted successfully";
                return RedirectToAction(nameof(CouponIndex));
			}
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(couponDto);
		}
	}
}
