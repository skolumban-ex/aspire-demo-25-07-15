var builder = DistributedApplication.CreateBuilder(args);

var workerApi = builder.AddProject<Projects.Worker_Api>("workerApi");

var workManagementApi = builder.AddProject<Projects.WorkManagement_Api>("workManagementApi");

var workSubmissionPage = builder.AddProject<Projects.WorkSubmissionPage>("workSubmissionPage");

builder.Build().Run();
