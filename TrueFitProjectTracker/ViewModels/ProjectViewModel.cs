﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atlassian.Jira;
using TrueFitProjectTracker.Factories.Dashboard;

namespace TrueFitProjectTracker.ViewModels
{
    public class ProjectViewModel
    {
        public ProjectViewModel(string key, Jira jira)
        {
            var currentProject = jira.GetProjects().First(); //for initialization
            var projects = jira.GetProjects();
            for (int i = 0; i < projects.Count(); ++i)
            {
                var tempProject = projects.ElementAt(i);
                if (tempProject.Key == key)
                {
                    currentProject = tempProject;
                    break;
                }
            }
            var issues = from i in jira.Issues
                         where i.Project == currentProject.Key
                         select i;
            int count = 0;
            for (int i = 0; i < issues.Count(); ++i)
            {
                var newIssue = issues.ElementAt(i);

                count++;
            }

            
            // initialize the properties from the model just... like... this!
            Title = currentProject.Name;

            // summing logic goes here.
            ProjectCompletion = count; // %, 0 through 100
            
            // TODO: write logic - for each (task in project) ...
            RecentTasksCompletedCount = 42;
            TasksCompletedCount = 392;
            RemainingTasksCount = 105;

            RecentBugsCompletedCount = 42;
            BugsCompletedCount = 267;
            RemainingBugsCount = 83;

            // tasks

            AllTasks = new List<TaskEntryViewModel>();
            CompletedTasks = new List<TaskEntryViewModel>();
            OneWeekTasks = new List<TaskEntryViewModel>();
            DistantTasks = new List<TaskEntryViewModel>();


            TasksFactory tasks = new TasksFactory(key);
            string taskName;
            DateTime dueDate;
            for (int i = 0; i < tasks.list.Count; i++)
            {
                for (int j = 0; j < tasks.list[i].Tasks.Count; j++)
                {
                    taskName = tasks.list[i].Tasks[j].Name;
                    dueDate = tasks.list[i].Tasks[j].DueDate;
                    AllTasks.Add(new TaskEntryViewModel(taskName, dueDate));
                }
            }

            CompletedTasks = AllTasks.Where(task =>
                DateTime.Compare(task.CompletionDate, DateTime.Now) < 0
                ).OrderBy(task => task.CompletionDate).ToList();

            OneWeekTasks = AllTasks.Where(task =>
                DateTime.Compare(task.CompletionDate, DateTime.Now.AddDays(7)) < 0
                && DateTime.Compare(task.CompletionDate, DateTime.Now) > 0
                ).OrderBy(task => task.CompletionDate).ToList();
            
            DistantTasks = AllTasks.Where(task =>
                DateTime.Compare(task.CompletionDate, DateTime.Now.AddDays(7)) > 0
                ).OrderBy(task => task.CompletionDate).ToList();


        TaskProgress = 79;
        TaskBurndownChart = new List<double> {100.0, 86.9, 73.5, 67.5, 55.2, 47.5, 40.2, 31.5, 27.3, 20.3, 13.9, 9.6};
        TaskRecentChart = new List<double> {5, 3, 4, 7, 2, 1, 6};

        BugProgress = 76;
        BugBurndownChart = new List<double> {100.0, 86.9, 73.5, 67.5, 55.2, 47.5, 40.2, 31.5, 27.3, 20.3, 13.9, 9.6};
        BugRecentChart = new List<double> { 5, 3, 4, 7, 2, 1, 6 };




        } // more obvious: End of Constructor


        
        public string Title { get; set; }
        public int ProjectCompletion { get; set; } // percentage

        public int RecentTasksCompletedCount { get; set; } // number
        public int TasksCompletedCount { get; set; } // number
        public int RemainingTasksCount { get; set; } // number

        public int RecentBugsCompletedCount { get; set; } // number
        public int BugsCompletedCount { get; set; } // number
        public int RemainingBugsCount { get; set; } // number

        // tasks
        public List<TaskEntryViewModel> AllTasks { get; set; }
        public List<TaskEntryViewModel> CompletedTasks { get; set; }
        public List<TaskEntryViewModel> OneWeekTasks { get; set; }
        public List<TaskEntryViewModel> DistantTasks { get; set; }

        public double TaskProgress { get; set; }
        public List<double> TaskBurndownChart { get; set; } // looks like 12 numbers, 1 for each month
        public List<double> TaskRecentChart { get; set; } // looks like 1 week frame

        public double BugProgress { get; set; }
        public List<double> BugBurndownChart { get; set; } 
        public List<double> BugRecentChart { get; set; } 
       

    }

    //list classes

    public class TaskEntryViewModel
    {
        public TaskEntryViewModel(string title, DateTime completionDate){
            Title = title;
            CompletionDate = completionDate;
        }


        public string Title { get; set; }
        public DateTime CompletionDate { get; set; }
    }
}