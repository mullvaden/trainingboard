﻿var app = angular.module('app', []);
app.value('$', $);
app.factory('trainingEventData', ['$', '$rootScope', function ($, $rootScope) {
    function trainingEventsOperations() {
        //Objects needed for SignalR
        var connection;
        var proxy;

        //To set values to fields in the controller
        var setMarketState;
        var setValues;
        var updateStocks;

        //This function will be called by controller to set callback functions
        var setCallbacks = function (setMarketStateCallback, setValuesCallback, updateStocksCallback) {
            setMarketState = setMarketStateCallback;
            setValues = setValuesCallback;
            updateStocks = updateStocksCallback;
        };

        var initializeClient = function () {
            //Creating connection and proxy objects
            connection = $.hubConnection();
            proxy = connection.createHubProxy('trainingHub');

            configureProxyClientFunctions();

            start();
        };

        var configureProxyClientFunctions = function () {
            proxy.on('marketOpened', function () {
                //set market state as open
                $rootScope.$apply(setMarketState(true));
            });

            proxy.on('marketClosed', function () {
                //set market state as closed
                $rootScope.$apply(setMarketState(false));
            });

            proxy.on('marketReset', function () {
                //Reset stock values
                initializeTrainingEvents();
            });

            proxy.on('updateTrainingEvents', function (data) {
                $rootScope.$apply(setValues(data));
            });
        };

        var initializeTrainingEvents = function () {
            //Getting values of trainingEvents from the hub and setting it to controllers field
            proxy.invoke('getAllTrainingEvents').done(function (data) {
                $rootScope.$apply(setValues(data));
            });
        };

        var start = function () {
            //Starting the connection and initializing market
            connection.start().pipe(function () {
                initializeTrainingEvents();
            });
        };

        var openMarket = function () {
            proxy.invoke('openMarket');
        };

        var closeMarket = function () {
            proxy.invoke('closeMarket');
        };

        var reset = function () {
            proxy.invoke('reset');
        };

        return {
            initializeClient: initializeClient,
            openMarket: openMarket,
            closeMarket: closeMarket,
            reset: reset,
            setCallbacks: setCallbacks
        };
    };

    return trainingEventsOperations;
}]);