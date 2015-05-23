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

    // Объявление функции, которая хаб вызывает при получении сообщений
    queue.client.callClient = function () {
        // Добавление сообщений на веб-страницу 
        $('#DivMain').append('<p> Приглашаем клиента </p>');
    };
    queue.client.loginEmployee = function (idLink) {
         
        $("#id_" + idLink).attr("class", "employeeLink");
    };

    queue.client.logoffEmployee = function (idLink) {

        $("#id_" + idLink).attr("class", "employeeOffLink");
    };

    queue.client.disabledBtnInQueue = function () {
        $("#tblOperations input").attr({ "disabled": "disabled", "class": "likeDisabledLink" });
    };

    queue.client.enabledBtnInQueue = function () {
        $("#tblOperations input").removeAttr("disabled").attr("class","likeLink");
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

    queue.client.addClientInQueue = function (prevId, qId, qNumber, qOperationName, qClientName) {
        // $(queue.prevId).after('<div id="queue_' + queue.Id + '"> <a href="javascript:return false;" class="employeeLink" onclick="$(this).siblings(' + "'div.details'" + ').show();">' + queue.Number + '&nbsp;&nbsp;&nbsp;' + queue.Client.Name + ' </a></div>');
        // var obj = jQuery.parseJSON(queue);
        //alert(qId);
        $('#queue_' + prevId).after('<div id="queue_' + qId + '"> <a href="javascript:return false;" class="employeeLink" onclick="$(this).siblings(' +
                "'div.details'" + ').show();">' + qNumber + '&nbsp;&nbsp;&nbsp;' + qClientName + ' </a> <div class="details"> ' +
                qOperationName + ' <a href="javascript:return false;" class="hideDiv" onclick="$(this).parent().hide();">Скрыть</a> </div></div>')
        
    };

    // Открываем соединение
    $.connection.hub.start().done(function () {
        queue.server.connect();
        

         //вызов клиента
        $("#btnCallClient").click(function () {
            queue.server.callClient();
        });
    });
})();

