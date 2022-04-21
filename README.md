# LetsGoBiking
The objective of this project is to create all the parts of an application which would allow the user to find its way from any location to any other location (in a same city for a first release) by using as much as possible the bikes offered by JC Decaux.


## This repository contains 4 projects:
<h4 align="left">1) A Server to compute the routing and return the results to the clients. This server should be a WCF server (as done in previous labs), receiving requests in both REST and SOAP. This server has 2 jobs:</h4>

   -  Get the resources (List of stations, bikes availability, geo data from OpenStreetMap). The list of stations is easy to manage because it doesn't change. So, the call to JCDecaux's stations needs to be done only once (at the initialization of the server for instance). The bikes availability however is dynamic and changes often. To retrieve those, we will use a Proxy implementing a cache (detailed below). Finally, for the OpenStreetMap data, it's rare to request the exact same information twice, so there is no real interest to use a Cache, and so the server will call OpenStreetMap directly when needed.
  -   Compute the routing and returns the result to the Client. To do so, the server should first sort the stations to find the closest to the start point, and the closest to the end point. Then, call the Proxy to get up-to-data information about the selected stations: if there are no available bikes in a station, another one should be tried, until we found the closest stations with available bikes. Then, with those 4 points (start, station1, station2, end), call OpenStreetMap to retrieve Geo data and compute the route (detailed below). Finally, return the details to the Client. 
  -   Optional: To illustrate the relevance of a SOAP heavy client to this service, you may add a log of the use of each station. Thanks to an API extension the SOAP heavy client will be able to process this information and even provide an excel sheet as a synthesis.
  -   Optional: In case of a route between different cities, the main challenge is to find successive paths to go to the first station in the first city, to find the following city with JCDecaux bikes, to find the nearest station in the second city, etc. 

<h4 align="left">2) A Proxy implementing a Cache to centralize all the external calls to JC Decaux API.</h4>
    This proxy should be a WCF server (as done in previous labs), receiving requests in either REST or SOAP (you can choose how to communicate between the server described above and this server).
Whenever a request is received, the Proxy should check its cache to check if the resource has already been saved: if so, it will return the cached value. If not, it will call the requested resource, store the response in its cache, and then return the response.


<h4 align="left">3) A Heavy C# Client, which will follow the Client Flow defined above.  To illustrate the relevance of a SOAP heavy client, we : </h4>
  - Test the same requests as for the Thin web client 
  - Get the optional log of the use of each station, provided by an API extension of the routing service. Then the heavy client will be able to process this information and provide statistics and an excel sheet as a synthesis.
  - 
<h4 align="left">4) A Thin Web (HTML/CSS/JS) Client, which will follow the Client Flow defined below. The requests to the Proxy will be REST requests.</h4>


## Flow between Clients and Web services :
<p align="center">
    <img src="https://user-images.githubusercontent.com/54988059/164542067-44ff7b9f-c15c-4bf2-8921-1eaa4caad655.png"/>
</p>
