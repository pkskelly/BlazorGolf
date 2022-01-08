using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using BlazorGolf.Client.Services;
using BlazorGolf.Core.Models;

namespace BlazorGolf.Client.Pages
{
    public class CourseBase: ComponentBase
    {
        [Inject]
        public ICourseService CourseService { get; set; }

        [Inject]
        public ILogger<CourseBase> Logger { get; set; }

        public List<Course> Courses { get; set; }

        public string Message { get; set; }

        public string[] Headings { get; set; } = new string[] { "", "Name", "City", "State", "" };


        // protected AddEmployeeDialog AddEmployeeDialog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Courses = (await CourseService.GetCourses()).ToList();                
            } 
            catch(Exception e)
            {
                Message = "Could not load courses!";
            }   
        }

        // public async void AddEmployeeDialog_OnDialogClose()
        // {
        //     Employees = (await EmployeeDataService.GetAllEmployees()).ToList();
        //     StateHasChanged();
        // }

        // protected void QuickAddEmployee()
        // {
        //     AddEmployeeDialog.Show();
        // }
    }
}