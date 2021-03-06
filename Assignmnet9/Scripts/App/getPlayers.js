﻿function getPlayers(teamId, queryID) {

    if (queryID == 1) {
        var e = document.querySelector('#teamList1');
    } else {
        var e = document.querySelector('#teamList2');
    }
    // Get a reference to the DOM element
   

    // create an xhr object
    var xhr = new XMLHttpRequest();

    // configure its handler
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
            // request-response cycle has been completed, so continue

            if (xhr.status === 200) {
                // request was successfully completed, and data was received, so continue

                // update the user interface
                e.innerHTML = xhr.responseText;

            } else {
                e.innerHTML = "<p>Request was not successful<br>(" + xhr.statusText + ")</p>";
            }
        } else {
            e.innerHTML = "<p>Loading...</p>";
        }
        // show the content
        e.style.display = 'block';
    }

    // configure the xhr object to fetch content
    xhr.open('get', 'playertrade/' + teamId, true);
    // fetch the content
    xhr.send(null);
}