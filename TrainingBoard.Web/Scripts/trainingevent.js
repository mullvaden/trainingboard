// A simple templating method for replacing placeholders enclosed in curly braces.
if (!String.prototype.supplant) {
    String.prototype.supplant = function (o) {
        return this.replace(/{([^{}]*)}/g,
            function (a, b) {
                var r = o[b];
                return typeof r === 'string' || typeof r === 'number' ? r : a;
            }
        );
    };
}

$(function () {

    var ticker = $.connection.trainingHub, // the generated client-side hub proxy
        up = '▲',
        down = '▼',
        $trainingTable = $('#trainingTable'),
        $trainingTableBody = $trainingTable.find('tbody'),
        rowTemplate = '<tr data-symbol="{Symbol}"><td>{Symbol}</td><td>{DayName}</td><td>{DayOpen}</td><td>{Direction} {Change}</td><td>{Day}</td></tr>';

    function formatEvent(trainingEvent) {
        return $.extend(trainingEvent, {
            DayName: trainingEvent.DayName,
            Day: (trainingEvent.Day * 100).toFixed(2) + '%'
            //,Direction: trainingEvent.Change === 0 ? '' : trainingEvent.Change >= 0 ? up : down
        });
    }

    function init() {
        ticker.server.getAllTrainingEvents().done(function (trainingEvents) {
            $trainingTableBody.empty();
            $.each(trainingEvents, function () {
                var stock = formatEvent(this);
                $trainingTableBody.append(rowTemplate.supplant(stock));
            });
        });
    }

    //// Add a client-side hub method that the server will call
    //ticker.client.updateStockDayName = function (stock) {
    //    var displayStock = formatStock(stock),
    //        $row = $(rowTemplate.supplant(displayStock));

    //    $trainingTableBody.find('tr[data-symbol=' + stock.Symbol + ']')
    //        .replaceWith($row);
    //};

    // Start the connection
    $.connection.hub.start().done(init);

});