var TrainingEventCtrl = function ($scope, trainingEventData) {

    $scope.trainingEvents = [];
    $scope.marketIsOpen = false;

    $scope.openMarket = function () {
        ops.openMarket();
    };
    $scope.closeMarket = function () {
        ops.closeMarket();
    };
    $scope.reset = function () {
        ops.reset();
    };

    function assignStocks(stocks) {
        $scope.trainingEvents = stocks;
    }

    function replaceStock(stock) {
        for (var count = 0; count < $scope.trainingEvents.length; count++) {
            if ($scope.trainingEvents[count].Symbol == stock.Symbol) {
                $scope.trainingEvents[count] = stock;
            }
        }
    }

    function setMarketState(isOpen) {
        $scope.marketIsOpen = isOpen;
    }

    var ops = trainingEventData();
    ops.setCallbacks(setMarketState, assignStocks, replaceStock);
    ops.initializeClient();
}