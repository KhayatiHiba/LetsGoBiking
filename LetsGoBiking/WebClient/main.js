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
    // Calculate the center of the two points.
   // var center = ol.proj.fromLonLat([(longitude + longitude2) / 2, (latitude + latitude2) / 2]);
    console.log("Longitude: " + longitude + " Lat: " + latitude);
    map.getView().setCenter(ol.proj.transform([longitude,latitude], 'EPSG:4326', 'EPSG:3857'));
    map.getView().setZoom(15);
}

function computePath() {
    console.log('Saving the information');
    //  get the location
    var location = document.getElementById('location').value;
    //  get the destination
    var destination = document.getElementById('destination').value;

    if(location == "" || destination == ""){
        setTimeout(
            () => document.getElementById("instructions").textContent = "Please fill both start and end destination fields",
            100);
        return;
    }

    console.log('The current location is : ' + location + ' and the destination is : ' + destination);

    // get the router server targetUrl
    var targetUrl = "http://localhost:8733/Design_Time_Addresses/RoutingService/Service1/rest/ComputePath?location="
        + location + "&destination=" + destination;
    var requestType = "GET";

    var caller = new XMLHttpRequest(); // create a new XHR object
    caller.open(requestType, targetUrl, true); // open the connection to the server with the targetUrl and the requestType (GET) and asynchronous (true)

    // The header set below limits the elements we are OK to retrieve from the server.
    caller.setRequestHeader("Accept", 'application/json; charset=utf-8');
    // onload shall contain the function that will be called when the call is finished.
    caller.onload = showTripPath;
    caller.responseType = 'json';
    caller.send();
}

let layers = [];

/**
 * Drawing the path on the map.
 */
function showTripPath() {
    console.log('Computing the trip');
    while(layers.length) {
        map.removeLayer(layers.pop())
    }
    if (this.status !== 200) {
        console.log("Contracts not retrieved. Check the error in the Network or Console tab.");
    } else {
        const responseObject = this.response;
        console.log(responseObject);

        if (responseObject['hasError'] === 'true') {
            document.getElementById("instruct").textContent = responseObject['message'];
            return;
        }
        posInitialex = responseObject['routes'][0]['geometry']['coordinates'][0][0];
        posInitialey = responseObject['routes'][0]['geometry']['coordinates'][0][1];
        
        for (let i = 0; i < responseObject['routes'].length; i++) {
            let color = i % 2 === 0 ? '#edae49' : '#7c616c'
            let coords = responseObject['routes'][i]['geometry']['coordinates'];
            let lineString = new ol.geom.LineString(coords);
            
            //posFinalex = coords[0][0]; 
            //posFinaley = coords[1][1];

            lineString.transform('EPSG:4326', 'EPSG:3857');

            let feature = new ol.Feature({
                geometry: lineString,
                name: 'Line'
            });

            let lineStyle = new ol.style.Style({
                stroke: new ol.style.Stroke({
                    color: color,
                    width: 3
                })
            });

            let source = new ol.source.Vector({
                features: [feature]
            });

            let vector = new ol.layer.Vector({
                source: source,
                style: [lineStyle]
            });

            layers.push(vector);
            map.addLayer(vector);
        }
        CenterMap(posInitialex, posInitialey);
        showInstructions(responseObject);
    }
}

function showInstructions(responseObject) {
    let steps = "";
    for (let i = 0; i < responseObject['routes'].length; i++) {
        let instructions = responseObject['routes'][i]['properties']['segments'][0]['steps']
        for (let j = 0; j < instructions.length; j++) {
            steps += "- " + instructions[j]['instruction'] + "\n";
        }
    }
    console.log(steps);
    let paragraph = document.getElementById("instruct");
    // Replace the content of the span with the new text content of steps
    paragraph.textContent = steps;
    paragraph.setAttribute('style', 'white-space: pre-line;');

}
