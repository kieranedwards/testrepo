

###Technologies###
SagePay - Json.net
CMS - Umbraco
DI - AutoFaq - See https://our.umbraco.org/documentation/master/Reference/Mvc/using-ioc
FrontEnd UI - React.JS, reqwest (AJAX micro libary), WebApi (no Jquery), see https://developers.google.com/web/fundamentals/input/form/label-and-name-inputs?hl=en
DataAccess - AutoMapper, Dapper, Fluent Migrations
Unit Testing - Nunit, Moq and Fluent Assertions

###Dependancy Injection###


###Umbraco###
Model cannot be used with inhertis so we need to create forms as child actions.
Custom routes need to 
Surface Controllers - 
Route HiJacking - Inherits RenderMvcController allows for comply custom routes

#####Plugins#####
*  UmbracoAzureBlolbStorage (via Nuget)- https://our.umbraco.org/projects/backoffice-extensions/azure-blob-storage-provider
*  LockoutMembershipProvider (via Umbraco Package) - https://our.umbraco.org/projects/website-utilities/lockout-membership-provider

###Unit Tests###
Unit Tests are using Nunit, Moq and Fluent Assertions

When_MethodName_StateUnderTest e.g. When_ValidateLoginByEmailAddress_EmailIsNotValid

There are both unit tests and intergration tests.

##SagePay##
Sage offer either Direct, Forms or ServerIFrame intergration (we are using ServerIFrame)

Documention can be found here:
http://www.sagepay.co.uk/support/find-an-integration-document/server-inframe-integration-documents

Key Pages
P27 Test Card Numbers


The fields required to pass to us on transaction registration are in appendix A1 starting on page 42

https://test.sagepay.com/mysagepay/login.msp

Transactions	
Vendor Name: puregymltd1 
Username: puregymltd1 
password: Pur3r1de2015


pr-alpha.database.windows.net
pradmin
pr-admin-2015

##Azure##
https://azure.microsoft.com/en-gb/documentation/articles/powershell-install-configure/
http://mythoughtsonit.com/2014/09/step-by-step-reserve-a-public-ip-address-in-azure/ 