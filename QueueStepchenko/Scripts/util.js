(function () {

    
    // Ссылка на автоматически-сгенерированный прокси хаба
    var queue = $.connection.queueHub;
    // Объявление функции, которую хаб вызывает при приглашении клиента
    //queue.client.call = function (name) {
    //    $('#EmployeeCalls').append('<p>Сотрудник <b>' + name+ '</b> приглашает вас </p>');
    //};
    //$.extend($.connection.queueHub, {
    //    Call: function (name) {
    //            $('#EmployeeCalls').append('<p>Сотрудник <b>' + name+ '</b> приглашает вас </p>');  };
    //})


    queue.client.callFromEmployee = function (clientName, operationName) {
        $('#callEmployee').html('<h4> Приглашается клиент: ' + clientName + '<br/><br/>' +
                        'Операция: '+operationName+'<br/><br/><span id="numberCall"></span></h4>');
    };

    queue.client.callToClient = function (employeeName) {
        $('#callClient').html('<h4> Вас приглашает сотрудник: ' + employeeName + '<br/><br/><span id="numberCall"></span></h4>');
        $('#BtnAccept').attr("class", "btn btn-default");
    };

    queue.client.changeNumberCall = function (numberCall, maxNumberCall, second) {
        clearInterval(interv);
        $('#numberCall').html('Вызов <span class="forTimer"> &nbsp;' + numberCall + '&nbsp;</span> из &nbsp;' + maxNumberCall +
                        '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span id = "timer" class="forTimer">&nbsp;' + second + '&nbsp;</span>');
        second = second - 1;
        var interv = setInterval(function () { $('#timer').html('&nbsp;' + second + '&nbsp;'); if (second == 0) { clearInterval(interv); }; second = second - 1; }, 1000);
     };

    queue.client.servicingClient = function (employeeName) {
        $('#callClient').html('<h4> Вас обслуживает сотрудник: ' + employeeName + '<br/><br/></h4>');
        $('#BtnAccept').attr("class", "noVisible");
    };

    queue.client.servicingEmployee = function (clientName, operationName) {
        $('#callEmployee').html('<h4> Обслуживается клиент: ' + clientName + '<br/><br/>' +
                        'Операция: ' + operationName + '<br/><br/></h4>');
    };

    queue.client.noClient = function () {
        $('#callEmployee').html('<h3> Ожидающий клиент отсутствует </h3>');
    };

    queue.client.addMessageEmployee = function (message) {
        $('#callEmployee').append('<h4 class="redColor">'+message+'</h4>');
    };

    queue.client.addMessageClient = function (message) {
        $('#callClient').append('<h4 class="redColor">' + message + '</h4>');
    };

    queue.client.changeClass = function (id, className) {
        $(id).attr("class", className);
    };

    queue.client.disabledBtnInQueue = function () {
        $("#tblOperations input").attr("disabled", "disabled");
    };

    queue.client.enabledBtnInQueue = function () {
        $("#tblOperations input").removeAttr("disabled");
    };

    queue.client.changeCountClients = function (count, idOperation) {

        $("#clients_" + idOperation).text(count);
        $("#clientsInHead_" + idOperation).text(count);
    };

    queue.client.changeCountEmployees = function (operations) {
        var obj = jQuery.parseJSON(operations);
        $(obj).each(function (index, operation) { $("#empls_" + operation.id).text(operation.count); });
    };

    queue.client.removeClientFromQueue = function (idQueue) {
        $('#queue_' + idQueue).remove();
    };

    queue.client.addClientInQueue = function (prevId, qId, qNumber, qOperationName, qClientName,isWaitExtra,className) {
        var htmlStr = '<div id="queue_' + qId + '"> <a href="javascript:return false;" class="'+className+'" onclick="$(this).siblings(' +
                "'div.details'" + ').show();">' + qNumber + ' &nbsp;&nbsp;&nbsp; ' + qClientName + ' </a> <div class="details"> ';
        if (isWaitExtra) {
            htmlStr = htmlStr + '<span class="redColor">Вне очереди<br/></span>';
        };
        htmlStr = htmlStr+qOperationName + ' <a href="javascript:return false;" class="hideDiv" onclick="$(this).parent().hide();">Скрыть</a> </div></div>';
        if (prevId == 0) {
            $('RightSidebar').prepend(htmlStr);
        }
        else {
            $('#queue_' + prevId).after(htmlStr);
        };
    };


    queue.client.refreshMainClient = function () {
        $('#refreshClient').click();
    };


    // Открываем соединение
    $.connection.hub.start().done(function () {
        queue.server.connect();
        

        // //вызов клиента
        //$("#btnCallClient").click(function () {
        //    queue.server.callClient();
        //});
    });
})();

