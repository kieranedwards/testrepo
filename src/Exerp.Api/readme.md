
This project wrapps Exerp's Java SOAP API. 
No classes directly from the API should be used outside of this project (can not mark them as internal due to the way web services work).
No business logic is help in this project. This is eaither in Exerp or the Application Layer

DataTransfer - Public Exposed DTO 
Interfaces - Used to abstract the API and Services
Services - Public exposed methods that wrap up the calls to the API. 
WebServices - Auto Generated classes via the WSDL.exe tool. Once imported interfaces have been added to these classes so the services can be tested. Namespaces are also added to prevent collisions. 
To add new web referance, save file locally then run the commandline tool: wsdl /l:CS personapi.wsdl /n:Exerp.Api.WebServices.PersonAPI

All terms and language used maps to the terms in Exerp as these are shown in the admin UI and the business are famillair with them.

##Technologies##
AutoMap - Used to map the API objects to the DTO. 

###Exerp###

Contact:
Jonathan E. Midttun
Business Consultant – Hardware Expert | Exerp - Fitness Chain Management Systems
+45 33 32 45 45 | support@clublead.com | http://www.exerp.com

SiteID's 100 = HQ, 700+ are Pure Ride
PureRide and PureGym are setup as 2 diffrent countries, this allows same system but diffrent rules and members
Location Details of the Cntres in the API are under the Person API for historic reasons
Access to the Exerp systems is restricted via IP address using their firewall. On option to get a fixed ip is to use EC2 Proxy - http://nimrodflores.com/web-design-and-development/create-private-proxy-amazon-ec2-putty-windows/

Whitelisted IP
54.174.149.234 - JK EC2 Instance Used as Proxy
90.208.127.87 - JK Home IP (dynamic)
90.152.30.161-90.152.30.163 - Regus Office
104.47.150.114,104.47.148.54,104.47.149.26,104.47.151.211 - Azure Alpha Servers

Test Environment
All emails go to live addresses in the test environment
Copy from live to test is Manual process that takes a day and is requested on demand
All personal data is removed on test such as email, phone etc.



