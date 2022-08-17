# e-pine

## Project description
ePine is a platform where you can find diverse services for diverse needs anywhere in the world. The platform, besides searching for services, provides an easier and unique experience to book, see details and cancel the appointments that you can create by choosing the best option for you.

## Implementation details

The platform has implemented the following functionalities:

* Account management: register, login, logout, etc.
* Search for services (merchants) - using
* See details of a service (using Locations API, TeamMembers API, Catalog API)
* Create a booking (using Locations API, TeamMembers API, Catalog API, Bookings API, Customer API)
* See list of the appointments created
* See details of an appointment (Booking API)
* Cancel a booking (Bookings API)
* Dashboard - see appointments
* Onboard a new merchant

From the architecture point of view, here is the diagrem of the solution:
![ePine - General architecture of the components](https://user-images.githubusercontent.com/17809789/185145097-dbfd5824-bd07-4710-ab1f-40a831436735.png)

The platform is created using:
a Server Blazor application (.Net 6.0) that connects to a database and to the Square API
a SQL Server database where minimal information about users, merchants and appointments is kept

## How to run it

1. Create a database, get the connection string and update it in appsettings.json
2. Make sure that ePine project is the startup project
3. Run the solution
4. Create an account for [Square](https://developer.squareup.com/us/en) and get the needed credentials

## Links
Application: https://epine.azurewebsites.net/

Demo: https://www.youtube.com/watch?v=Kopeo_5Uk8w
