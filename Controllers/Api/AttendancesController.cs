﻿using System.Linq;
using System.Web.Http;
using BigSchool.DTOs;
using BigSchool.Models;
using Microsoft.AspNet.Identity;

namespace BigSchool.Controllers.Api
{
    public class AttendancesController : ApiController
    {
        private ApplicationDbContext _dbContext;
        public AttendancesController()
        {
            _dbContext = new ApplicationDbContext();
        }
        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto attendanceDto)
        {
            var userId = User.Identity.GetUserId();
            if (_dbContext.Attendances.Any(a => a.AttendeeId == userId
            && a.CourseId == attendanceDto.CourseId))
                return BadRequest("The Attendance already exists");
            var attendance = new Attendance
            {
                CourseId = attendanceDto.CourseId,
                AttendeeId = userId
            };
            _dbContext.Attendances.Add(attendance);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteAttendance(int id)
        {

            var userId = User.Identity.GetUserId();
            var attendance = _dbContext.Attendances
                .SingleOrDefault(a => a.AttendeeId == userId && a.CourseId == id);
            if (attendance == null)
                return NotFound();
            _dbContext.Attendances.Add(attendance);
            _dbContext.SaveChanges();
            return Ok(id);
        }
    }
}
