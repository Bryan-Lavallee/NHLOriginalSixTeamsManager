function togglePosition(PositionId, Selected) {
    var players = document.querySelectorAll('div.' + PositionId);

    if (Selected) {
        for (var i = 0; i < players.length; i++) {
            players[i].style.display = 'block';
        }
    } else {
        for (var i = 0; i < players.length; i++) {
            players[i].style.display = 'none';
        }
    }
}