var map = new ol.Map({
    target: 'map', // <-- This is the id of the div in which the map will be built.
    layers: [
        new ol.layer.Tile({
            source: new ol.source.OSM()
        })
    ],

    view: new ol.View({
        center: ol.proj.fromLonLat([7.0985774, 43.6365619]), // <-- Those are the GPS coordinates to center the map to.
        zoom: 10 // You can adjust the default zoom.
    })
});
/**
 *  Center the map to the given coordinates.
 * @param longitude the longitude of the new center point of the map.
 * @param latitude the latitude of the new center point of the map.
 */
function CenterMap(longitude, latitude) {
    console.log("Longitude: " + longitude + " Lat: " + latitude);
    map.getView().setCenter(ol.proj.transform([longitude, latitude], 'EPSG:4326', 'EPSG:3857'));
    map.getView().setZoom(10);
}

function computePath() {
    console.log('Saving the information');
    //  get the location
    var location = document.getElementById('location').value;
    //  get the destination
    var destination = document.getElementById('destination').value;

    console.log('The current location is : ' + location + ' and the destination is : ' + destination);

    //TODO : change the latitude and longitude to the real ones
    CenterMap(12.9, 55.7);

    // get the router server targetUrl
    var targetUrl = "http://localhost:8733/Design_Time_Addresses/RoutingService/Service1/itinerary?location="
        + location + "&destination=" + destination;
    var requestType = "GET";

    var caller = new XMLHttpRequest(); // create a new XHR object
    caller.open(requestType, targetUrl, true); // open the connection to the server with the targetUrl and the requestType (GET) and asynchronous (true)

    // The header set below limits the elements we are OK to retrieve from the server.
    caller.setRequestHeader("Accept", 'application/json; charset=utf-8');
    // onload shall contain the function that will be called when the call is finished.
    caller.onload = showTripPath;
    caller.send();
}

/**
 * Drawing the path on the map.
 */
function showTripPath() {
    console.log('Computing the trip');
}
