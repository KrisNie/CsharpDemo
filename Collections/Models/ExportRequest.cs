using System.Collections.Generic;

namespace Collections.Models;

public struct ExportRequest
{
    public string requestId { get; set; }
    public UserContext userContext { get; set; }
    public string packageDatasourceType { get; set; }
    public string basePackageLocation { get; set; }
    public string sourceTenantID { get; set; }
    public List<Organization> organizations { get; set; }
}

public struct UserContext
{
    public string emailId { get; set; }
}

public struct Organization
{
    public string uniqueName { get; set; }
    public List<Application> applications { get; set; }
}

// TODO displayName
public struct Application
{
    public string uniqueName { get; set; }
    public List<Module> modules { get; set; }
}

public struct Module
{
    public string irn { get; set; }
    public string uniqueName { get; set; }

    public List<Configuration> configurations { get; set; }
    public List<Content> contents { get; set; }
    public List<Extension> extensions { get; set; }
    public List<SecurityAuthzData> securityAuthzData { get; set; }
    public List<UserData> userData { get; set; }
}

public struct Configuration
{
    public string irn { get; set; }
    public string uniqueName { get; set; }
    public List<Configuration> configurations { get; set; }
    public List<string> instances { get; set; }
}

// TODO
public struct Content
{
}

// TODO
public struct Extension
{
}

// TODO
public struct SecurityAuthzData
{
}

// TODO
public struct UserData
{
}