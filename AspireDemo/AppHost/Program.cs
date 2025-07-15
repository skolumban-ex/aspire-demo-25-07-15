var builder = DistributedApplication.CreateBuilder(args);

var dbForWorkerApi = builder.AddPostgres("dbForWorkerApi");

var workerApi = builder.AddProject<Projects.Worker_Api>("workerApi")
    .WithReference(dbForWorkerApi);

var dbForWorkManagementApi = builder.AddPostgres("dbForWorkManagementApi");

var workManagementApi = builder.AddProject<Projects.WorkManagement_Api>("workManagementApi")
    .WithReference(dbForWorkManagementApi);

var workSubmissionPage = builder.AddProject<Projects.WorkSubmissionPage>("workSubmissionPage");

builder.Build().Run();
