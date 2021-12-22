# BlazorGolf

## Summary 
A Blazor WebAssembly client application and a .NET 6 Web API sample application to learn/practice Blazor WebAssembly and Web API design and development.

The initial application will be based on the following:
- .NET 6 Web API project secured using Azure AD and Roles (for player vs admin features)
    - Use/support Open API from the start 
- .NET 6 Blazor WebAssembly project using MudBlazor components  (OSS and free)
- Deployed to Azure (targets TBD)


## Approach

Trying to use some best practices with API design first, then layering in the additional features. 


## Project setup checklist

1. [x] Start with an idea and draft a simple readme for the idea
2. [x] Choose a language(s) (C#)
3. [x] Choose an IDE (Visual Studio Code)
4. [x] Choose a cloud (Azure) 
    1. [ ] What architecture will we use (containers/PaaS/Serverless/?)
    2. [ ] What is the plan for resiliency
    3. [ ] What is the plan for backups 
5. Choose a DevOps platform (GitHub)
    1. [x] Create an organization (personal)
    2. [x] Create a repo (personal & private to start)
    3. [ ] Setup branch rules/policies (require a code review)
6. Creating the new/empty project
    1. [ ] Setup basic CI
    2. [ ] Creating basic empty projects (Rest API, Website, etc)
    3. [ ] Creating basic automated test projects - to prove that we are deploying working code 
      1. [ ] unit/integration tests 
      2. [ ] smoke/functional tests
      3. [ ] code coverage
    5. [ ] Add editor config file (to take the opinion out of code styling)
    6. [ ] Add infrastructure as code to deploy to each needed environment (Dev, QA, Prod?)
7. Back to GitHub
     1. [ ] Setup dependabot to automatically update dependencies
     2. [ ] Setup security to check for secrets and unsafe dependency
     3. [ ] Link to CD templates that build, test, or deploy as needed 
     4. [ ] Setup basic CD, to deploy to each cloud environment, with smoke tests and environment approvals as needed 
8. Project planning
     1. [ ] Set goals/roadmap. What does Alpha/Beta/GA look like?
     2. [x] Track PBI/ User Stories - Issues with Labels in Github 

Originally based on [Project Setup](https://github.com/samsmithnz/OpinionatedSoftwareAdvice/blob/main/98-Appendix-ProjectStartupList.md) from Sam Smith.
