﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vale.Geographic.Api.Filters;
using Vale.Geographic.Application.Dto.Notification;
using Vale.Geographic.Application.Services;

namespace Vale.Geographic.Api.Controllers
{
    /// <summary>
    /// Controller to Notification
    /// </summary>
    [Route("api/Applications")]
    [Authorize]
    public class NotificationController : Controller
    {
        private INotificationAppService _notificationAppService { get; }
        /// <summary>
        /// Constructor to Notification Controller 
        /// </summary>
        /// <param name="notificationAppService"></param>
        public NotificationController(INotificationAppService notificationAppService)
        {
            this._notificationAppService = notificationAppService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        [HttpPost("{applicationId}/Devices")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public async Task<IActionResult> RegisterDevice(string applicationId, [FromBody] DeviceAddDto device)
        {
            var ret = await this._notificationAppService.RegisterDevice(applicationId, device);
            return Created("", ret);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="installationId"></param>
        /// <param name="deviceUpdate"></param>
        /// <returns></returns>
        [HttpPut("{applicationId}/Devices/installations/{installationId}")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public async Task<IActionResult> InstalationDevice(string applicationId, string installationId, [FromBody]DeviceUpdateDto deviceUpdate)
        {
            await this._notificationAppService.InstalationDevice(applicationId, installationId, deviceUpdate);
            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="notificationAddDto"></param>
        /// <returns></returns>
        [HttpPost("{applicationId}/Notifications")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public async Task<IActionResult> RegisterNotification(string applicationId, [FromBody] NotificationAddDto notificationAddDto)
        {
            var ret = await this._notificationAppService.RegisterNotification(applicationId, notificationAddDto);
            return Ok(ret);
        }
    }
}