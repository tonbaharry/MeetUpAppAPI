# MeetUpAppAPI
source code for the API that powers the angular based MeetUpAPP
<<<<<<< HEAD
# MeetUpApp
An application that enables people within a local community connect and communicate with a possiblity of 
forming long term bonds and partnerships.The application is still currently in development mode with new features
tested and pushed to source control every 2-3 days. 

Technical Description

The application's backend was developed using .NET Core Web API
framework using the Repository design pettern which abstracts away the implementation of each of the API's methods
from the calling party through the use of appropriate interfaces. In addition the implementation data models are
not directly exposed to the caller but rather calls to the API (HTTP POST) are achieved using Data Transfer Objects
(DTO) which are later mapped to the implementation data models using a custom AutoMapper helper class. Authorization
requirement was enabled thus a calls to the API will required a valid bearer token which is generated at login
using JSON Web Token (JWT) authentication. The API is supported by SQL server database which holds all member records 
and a cloud based service (Cloudinary.com) which holds user photos and returns a reference to the picture file for
reference purpose.

On the other hand the front end of the application was developed using the Angular platform. The front end consists
of multiple compoenents with each components containing a html file, an associated code behind typescripts class and
a .css stylin file. The components are put together in a pre-planned order to make up the view component for 
interection to users. In addition, a service component was created to manage all calls to the support WEB API for
data retreival, submission and updates. 

Users have the option of Signing up on the application and becoming members of the application's network. As an
enrolled member, the user can add photos, view other members and connect with other members. At the moment work is ongoing 
on the messaging feature which will enable members to exchange messages to one another on the application.


=======
# MeetUpApp API
>>>>>>> 3fe83b1eb279417ed2e12bfdf0de4edb9958673c
